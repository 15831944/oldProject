using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;

namespace ElectronTransferBll.DBEntityHelper
{
    /// <summary>
    /// 属性复制
    /// </summary>
    public class AttributeCopy
    {
        #region 属性复制

        /// <summary>
        /// 属性复制
        /// </summary>
        /// <param name="objectId">实体对象Id</param>
        public static CopySymbolObject GetAttribute(ObjectId objectId)
        {
            G3EObject g3eObject = null;
            CopySymbolObject copySymbolObject =null;
            if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
            {
                copySymbolObject= GetBrotherDBEntity(g3eObject, objectId);
            }
            return copySymbolObject;
        }

        /// <summary>
        /// 复制符号数据
        /// </summary>
        /// <param name="g3eObject"> </param>
        /// <param name="objectId"> </param>
        /// <returns></returns>
        private static CopySymbolObject GetBrotherDBEntity(G3EObject g3eObject, ObjectId objectId)
        {
            var cso = new CopySymbolObject();
            var copyDBEntity = new HashSet<DBEntityObject>();
            try
            {
                var entry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == g3eObject.G3E_FNO);
                if (entry != null)
                {
                    if (!string.IsNullOrEmpty(entry.SymbolPtTable))
                    {
                        //坐标表数据
                        var ptSymbolValue = PublicMethod.Instance.GetDBSymbolFinder(objectId) as ElectronSymbol;
                        if (ptSymbolValue != null)
                        {
                            cso.pointDBEntity = ptSymbolValue;
                        }
                        //属性
                        copyDBEntity = GetDBEntityObjects(g3eObject, entry, copyDBEntity);
                        cso.g3eObject = g3eObject;
                        cso.objectID = objectId;
                        cso.hsDBEntity = copyDBEntity;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return cso;
        }

        private static HashSet<DBEntityObject> GetDBEntityObjects(G3EObject g3eObject, DBSymbolEntry entry,
                                                                  HashSet<DBEntityObject> copyDBEntity)
        {
            //自身属性
            if (!string.IsNullOrEmpty(entry.ComponentTable.SelfAttribute))
            {
                copyDBEntity = GetBrotherDBEntity(entry.ComponentTable.SelfAttribute.Trim(), copyDBEntity, g3eObject.G3E_FID);
            }
            //公共属性
            if (!string.IsNullOrEmpty(entry.ComponentTable.Common))
            {
                copyDBEntity = GetBrotherDBEntity(entry.ComponentTable.Common.Trim(), copyDBEntity, g3eObject.G3E_FID);
            }
            //连接关系
            if (!string.IsNullOrEmpty(entry.ComponentTable.Connectivity))
            {
                copyDBEntity = GetBrotherDBEntity(entry.ComponentTable.Connectivity.Trim(), copyDBEntity, g3eObject.G3E_FID);
            }
            //功能位置
            if (!string.IsNullOrEmpty(entry.ComponentTable.Gnwz))
            {
                copyDBEntity = GetBrotherDBEntity(entry.ComponentTable.Gnwz.Trim(), copyDBEntity, g3eObject.G3E_FID);
            }
            return copyDBEntity;
        }


        /// <summary>
        /// 复制符号数据
        /// </summary>
        /// <param name="className"></param>
        /// <param name="copyDBEntity"></param>
        /// <param name="oldFid"></param>
        /// <returns></returns>
        private static HashSet<DBEntityObject> GetBrotherDBEntity(string className, HashSet<DBEntityObject> copyDBEntity,
                                                                  long oldFid)
        {
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), className.Trim());
                var brotherValue = DBManager.Instance.GetEntity(type, oldFid);
                if (brotherValue != null)
                {
                    copyDBEntity = GetBrotherDBEntity(className, brotherValue, copyDBEntity);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex + string.Format("复制错误：{0} {1}", className, oldFid));
            }
            return copyDBEntity;
        }

        private static HashSet<DBEntityObject> GetBrotherDBEntity(string className, DBEntity dbEntity,
                                                                  HashSet<DBEntityObject> copyDBEntity)
        {
            if (dbEntity != null)
            {
                var dbEntityObject = new DBEntityObject {className = className.Trim(), dbEntity = dbEntity};
                copyDBEntity.Add(dbEntityObject);
            }
            return copyDBEntity;
        }

        #endregion

        #region 粘贴

        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="objectIds"></param>
        /// <param name="copySymbolObject"> </param>
        public static int CopyAttribute(ObjectId[] objectIds, CopySymbolObject copySymbolObject)
        {
            var copyCount = 0;
            //获取原设备的样式
            var eventArgs = CABLEManager.GetSymbolExecution(copySymbolObject.objectID, copySymbolObject.g3eObject,DBManager.Instance as XmlDBManager);
            if (eventArgs != null)
            {
                //获取原设备的块定义名称
                var blockName = DCadApi.GetBlockDefinitionName(eventArgs, copySymbolObject.g3eObject.G3E_FNO.ToString());
                foreach (var objectId in objectIds)
                {
                    G3EObject g3eObject = null;
                    var dbObject=PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead);
                    //排除标注
                    if (dbObject is BlockReference || dbObject is Polyline)
                    {
                        if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
                        {
                            //校验工单
                            if (DBEntityFinder.Instance.VerifyLTTID(objectId))
                            {
                                //判断类型
                                if (copySymbolObject.g3eObject.G3E_FNO == g3eObject.G3E_FNO)
                                {
                                    foreach (var dbEntity in copySymbolObject.hsDBEntity)
                                    {
                                        SetBrotherDBEntity(dbEntity, g3eObject);
                                    }
                                    //更新符号
                                    UpdateSymbol(blockName, objectId, eventArgs);
                                    copyCount++;
                                }
                            }
                        }
                    }
                }
            }
            return copyCount;
        }

        /// <summary>
        /// 复制符号数据
        /// </summary>
        /// <param name="dbEntityObject"></param>
        /// <param name="g3eObject"></param>
        /// <returns></returns>
        private static void SetBrotherDBEntity(DBEntityObject dbEntityObject, G3EObject g3eObject)
        {
            try
            {
                var className = dbEntityObject.className;
                var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), className);
                var targetDBEntity = DBManager.Instance.GetEntity(type, g3eObject.G3E_FID);
                if (targetDBEntity != null)
                {
                    //原数据
                    var sourceDBEntity = dbEntityObject.dbEntity;
                    var properties = targetDBEntity.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        var hasSetter = HasSetter(property);
                        //条件过滤
                        var filter = Filter(property);
                        if (!hasSetter || !filter)
                            continue;

                        object sourceValue = null; 
                        sourceValue = property.GetValue(sourceDBEntity, null);
                        //源数据为空不修改
                        if (sourceValue == null) continue;
                        //目标值
                        var targetValue = targetDBEntity.GetValue(property.Name);
                        //值不同才修改
                        if (sourceValue.Equals(targetValue)) continue;
                        if (property.Name.Equals("EntityState"))
                        {
                            if (targetDBEntity.EntityState == EntityState.None)
                            {
                                sourceValue = targetDBEntity.GetType().Name.Equals("Connectivity_n") ? EntityState.Old_Nal_Nal : EntityState.Update;
                            }
                            else
                                continue;
                        }
                        property.SetValue(targetDBEntity, sourceValue, null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex + string.Format("属性复制错误：{0} {1}", dbEntityObject.className, g3eObject.G3E_FID));
            }
        }

        private static bool HasSetter(PropertyInfo property)
        {
            var hasSetter = property.GetAccessors().Any(o => o.Name.StartsWith("set_"));
            return hasSetter;
        }
        /// <summary>
        /// 过滤
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool Filter(PropertyInfo property)
        {
            IList<string> surfaceShowFid = new List<string>
            { "G3E_ID", "G3E_FID", "SBMC", "NODE1_ID", "NODE2_ID", "OWNER1_ID",
                "GNWZ_FL2", "DLT", "AZWZ", "PBMC", "SSBYQ", "GNWZ_SSTQHTJ", "GNWZ_SSTJ",
                "SSDF", "GNWZ_SSDF", "DYKGG", "GNWZ_SSKGG", "GNWZ_SSGT","BZ1","BZ2" };
            return !surfaceShowFid.Any(o => o.Equals(property.Name));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sourceBlockName">原设备块定义名称</param>
        /// <param name="targetObjId">目标对象</param>
        /// <param name="eventArgs">设备样式</param>
        private static void UpdateSymbol(string sourceBlockName,ObjectId targetObjId,SymbolEventArgs eventArgs)
        {
            try
            {
                var targetBlockName = string.Empty;
                //获取目标设备块定义名称
                var entity = PublicMethod.Instance.GetObject(targetObjId, OpenMode.ForRead);
                if (entity is BlockReference)
                {
                    targetBlockName = (entity as BlockReference).Name;
                }
                var objId = ObjectId.Null;
                //获取目标设备对象数据
                var psd = DCadApi.GetSymbolDataByObjId(targetObjId);
                if (!string.IsNullOrEmpty(sourceBlockName) && !sourceBlockName.Equals(targetBlockName) && entity is BlockReference)
                {
                    psd.blockName = sourceBlockName;
                    psd.color = eventArgs.color;
                    //更新设备
                    DCadApi.ReplacePointSymbol(psd, ref objId);
                }
                else
                {
                    //这里是针对线改变颜色
                    if (psd.color != eventArgs.color)
                    {
                        //更新颜色
                        DCadApi.EditorPointSymbol(psd.objectId, eventArgs.color);
                        psd.color = eventArgs.color;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        #endregion
    }
}
