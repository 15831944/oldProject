using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using Exception = Autodesk.AutoCAD.Runtime.Exception;

namespace ElectronTransferBll
{
    public class CustomSymbol
    {
        /// <summary>
        /// 数据源
        /// </summary>
        private static XmlDBManager xmlDBManager;
        public static Dictionary<ObjectId, CopySymbolObject> copyCustomsSymbolData = new Dictionary<ObjectId, CopySymbolObject>();

        #region 创建自定义模版
        /// <summary>
        /// 创建xml
        /// </summary>
        /// <param name="filePath">xml保存路径</param>
        private static XmlDBManager CreateDBXml(string filePath)
        {
            var xmldb = new XmlDBManager {FileName = filePath};
            xmldb.Initialize();
            return xmldb;
        }
        /// <summary>
        /// 创建自定义符号
        /// </summary>
        /// <param name="xmlPath">自定义符号存储路径</param>
        /// <param name="objIds">所选择的实体</param>
        /// <param name="basePoint">自定义符号基点</param>
        public static bool CreateCustomsSymbol(string xmlPath, ObjectId[] objIds, Point3d basePoint)
        {
            try
            {
                //复制当前选择的设备数据
                CopyCustomsSymbolInfo(objIds);
                if (DBEntityCopy.Instance.isAccord)
                {
                    //创建自定义符号xml
                    var xmlDB = CreateDBXml(xmlPath);
                    xmlDB = GetCustomsSymbolXmlDB(xmlDB, objIds, basePoint);
                    return xmlDB.Save(xmlPath);
                }
                var errorInfo = DBEntityCopy.Instance.GetCopyErrorInfo();
                var eInfo = string.Format("数据缺失，不满足自定义模版条件！\n\n{0}", errorInfo);
                PublicMethod.Instance.AlertDialog(eInfo);
                return false;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return false;
        }
        /// <summary>
        /// 获取自定义模版数据源
        /// </summary>
        /// <param name="xmlDB"></param>
        /// <param name="objIds"></param>
        /// <param name="basePoint"></param>
        /// <returns></returns>
        private static XmlDBManager GetCustomsSymbolXmlDB(XmlDBManager xmlDB, IEnumerable<ObjectId> objIds, Point3d basePoint)
        {
            return (from objectId in objIds where DBEntityFinder.Instance.HasG3EIDS(objectId) 
                    select copyCustomsSymbolData.FirstOrDefault(o => o.Key == objectId) 
                    into cso 
                    where cso.Value != null select cso).Aggregate(xmlDB, (current, cso) => AddXmlDB(cso.Value, current, basePoint));
        }

        /// <summary>
        /// 复制当前选择的符号的数据
        /// </summary>
        /// <param name="objIds"></param>
        private static void CopyCustomsSymbolInfo(ObjectId[] objIds)
        {
            try
            {
                copyCustomsSymbolData=DBEntityCopy.Instance.ObjectCopy(objIds);
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }


        /// <summary>
        /// 保存复制数据
        /// </summary>
        /// <param name="cso"> </param>
        /// <param name="xmlDB"> </param>
        /// <param name="basePoint"> </param>
        private static XmlDBManager AddXmlDB(CopySymbolObject cso, XmlDBManager xmlDB, Point3d basePoint)
        {
            if (cso.pointDBEntity != null)
            {
                var newObject_pt = cso.pointDBEntity;
                newObject_pt.BasePoint = ConvertGeometry.Instance.GetMultipoint(basePoint.X, basePoint.Y, 0);
                xmlDB.Insert(newObject_pt);
            }
            if (cso.hsDBEntity != null)
            {
                foreach (var nData in cso.hsDBEntity)
                {
                    xmlDB.Insert(nData.dbEntity);
                }
            }
            return xmlDB;
        }

        ///// <summary>
        ///// 保存复制的标注数据
        ///// </summary>
        ///// <param name="dbEntity"></param>
        ///// <param name="xmlDB"> </param>
        //private static XmlDBManager SaveCopylabelData(DBEntity dbEntity, XmlDBManager xmlDB)
        //{
        //    if (dbEntity != null)
        //    {
        //        xmlDB.Insert(dbEntity);
        //    }
        //    return xmlDB;
        //}

        #endregion

        #region 读取数据

        /// <summary>
        /// 加载xml 添加自定义符号
        /// </summary>
        /// <param name="path">自定义符号路径</param>
        /// <param name="customSymbolName">自定义符号名称</param>
        public static void LoadSymbolSetXMl(string path, string customSymbolName)
        {
            try
            {
                Clear();
                if (!DBCustomFinder.Instance.ContainsKey(customSymbolName))
                {
                    xmlDBManager = CreateDBXml(path); 
                    if (xmlDBManager.Count() == 0)
                    {
                        PublicMethod.Instance.AlertDialog(string.Format("{0}该模版已损坏！", customSymbolName));
                        DBCustomFinder.Instance.Remove(customSymbolName);
                        return;
                    }
                    DBCustomFinder.Instance.Add(customSymbolName, xmlDBManager);
                }
                else
                {
                    xmlDBManager = DBCustomFinder.Instance[customSymbolName];
                }
                CABLEManager.LoadAllSymbol(xmlDBManager, false);
                InsertCustomSymbol(CABLEManager.basePoint[0], PublicMethod.Instance.dicObjIds);
                //处理拓扑关系
                DBEntityCopy.Instance.UpdateConnectionData();
            }
            catch (System.Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
            finally
            {
                PublicMethod.Instance.dicObjIds.Clear();
            }
        }

        /// <summary>
        /// 准备数据，CAD绘图
        /// </summary>
        /// <param name="basePoint">自定义符号基点</param>
        /// <param name="dicObjIds"> </param>
        private static void InsertCustomSymbol(Point3d basePoint,Dictionary<ObjectId,DBEntity> dicObjIds )
        {
            using (PublicMethod.Instance.acDoc.LockDocument())
            {
                DragAndCopyEntities(basePoint, dicObjIds);
            }
        }
        private static void Clear()
        {
            DBEntityCopy.Instance._G3EIdMapping.Clear();
            DBEntityCopy.Instance._connectionTableData.Clear();
            DBEntityCopy.Instance._subordinateSbTableData.Clear();
            DBEntityCopy.Instance._subordinateDfTableData.Clear();
        }
        #endregion

    

        #region 拖拽移动自定义模版
        /// <summary>
        /// 拖拽绘制
        /// </summary>
        /// <param name="point"></param>
        /// <param name="objectIds"></param>
        public static void DragAndCopyEntities(Point3d point, Dictionary<ObjectId, DBEntity> objectIds)
        {
            try
            {
                DCadApi.isInsertSymbol = true;
                DCadApi.isModifySymbol = true;
                var dragger = new ManyEntityCopy();
                var objIds = GetObjIds(objectIds);
                var dragResult = dragger.StartDrag(point, objIds);
                if (dragResult.Status == PromptStatus.OK)
                {
                    CreateTransformedInsert(dragger.DisplacementMatrix, objectIds);
                }
                else
                {
                    ObjectsErase(objectIds);
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                DCadApi.isInsertSymbol = false;
                DCadApi.isModifySymbol = false;
            }
        }
        private static void ObjectsErase(Dictionary<ObjectId, DBEntity> objectIds)
        {
            foreach (var item in objectIds)
            {
                PublicMethod.Instance.EraseObject(item.Key);
            }
        }
        private static ObjectId[] GetObjIds(ICollection<KeyValuePair<ObjectId, DBEntity>> dicObjIds)
        {
            var objIds = new ObjectId[dicObjIds.Count];
            var oc = new ObjectIdCollection();
            foreach (var item in dicObjIds)
            {
                oc.Add(item.Key);
            }
            oc.CopyTo(objIds, 0);
            return objIds;
        }
        private static void CreateTransformedInsert(Matrix3d transformationMatrix, Dictionary<ObjectId, DBEntity> entitiesToInsert)
        {
            var currentEditor = Application.DocumentManager.MdiActiveDocument.Editor;
            using (var trans = currentEditor.Document.TransactionManager.StartTransaction())
            {
                //存储新旧ObjectID关系
                //var dicG3e_fid = new Dictionary<long, ExpandsData>();
                foreach (var item in entitiesToInsert)
                {
                    var insertEntity = (Entity)trans.GetObject(item.Key, OpenMode.ForRead);
                    insertEntity.UpgradeOpenAndRun();
                    insertEntity.TransformBy(transformationMatrix);

                    var G3e_fno = item.Value.GetValue<int>("G3E_FNO");
                    var oldG3e_fid = item.Value.GetValue<long>("G3E_FID");
                    var oldG3e_id = item.Value.GetValue<long>("G3E_ID");
                    DBEntityCopy.Instance.CreateG3EFID(oldG3e_fid, oldG3e_id, item.Key);
                    var newIds = DBEntityCopy.Instance._G3EIdMapping.SingleOrDefault(o => o.Key == oldG3e_fid);
                    var newG3e_id = newIds.Value.newG3e_id;
                    var newG3e_fid = newIds.Value.newG3e_fid;
                    if (insertEntity is DBText || insertEntity is MText)
                    {
                        InsertLabelDBEntity(item, newG3e_id, newG3e_fid);
                        //标注lb
                        InsertLabelLB(oldG3e_fid, item.Value.GetType().Name, newG3e_id, newG3e_fid);
                        //杂项标注
                        if (G3e_fno == 250)
                            InsertZxbzDBEntity(oldG3e_fid, newG3e_id, newG3e_fid);
                    }
                    else
                    {
                        InsertPointDBEntity(item, newG3e_id, newG3e_fid, oldG3e_fid, G3e_fno);
                    }
                }
                trans.Commit();
            }
        }



        /// <summary>
        /// 新增标注数据
        /// </summary>
        private static void InsertLabelDBEntity(KeyValuePair<ObjectId, DBEntity> kvPair, long newG3e_id, long newG3e_fid)
        {
            var newObject_Label = kvPair.Value.Clone() as ElectronSymbol;

            newObject_Label.G3E_ID = newG3e_id;
            newObject_Label.G3E_FID = newG3e_fid;
            newObject_Label.EntityState = EntityState.Insert;
            newObject_Label.EntityType =newObject_Label.G3E_FNO==250? EntityType.ZxLabel:EntityType.Label;
            newObject_Label.SetValue("LTT_ID", (decimal)MapConfig.Instance.LTTID);
            newObject_Label = ConvertGeometry.Instance.UpdateG3E_GEOMETRY(newObject_Label, kvPair.Key);

            DBManager.Instance.Insert(newObject_Label);
            PublicMethod.Instance.UpdateDBSymbolFinder(kvPair.Key, newObject_Label);
        }

        private static void InsertLabelLB(long g3e_fid, string className, long newG3e_id, long newG3e_fid)
        {
            var labelTable = className.Replace("_sdogeom", "");

            //标注符号表
            if (string.IsNullOrEmpty(labelTable)) return;
            var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), labelTable);
            if (!xmlDBManager.Has(type)) return;
            var lb = xmlDBManager.GetEntity(type, g3e_fid);
            if (lb == null) return;
            var newObject_Label = lb.Clone() as ElectronBase;

            newObject_Label.G3E_ID = newG3e_id;
            newObject_Label.G3E_FID = newG3e_fid;
            newObject_Label.EntityState = EntityState.Insert;
            newObject_Label.SetValue("LTT_ID", (decimal)MapConfig.Instance.LTTID);
            DBManager.Instance.Insert(newObject_Label);
        }

        /// <summary>
        /// 插入杂项标注的自身属性表
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="newG3e_id"></param>
        /// <param name="newG3e_fid"></param>
        private static void InsertZxbzDBEntity(long g3e_fid,long newG3e_id,long newG3e_fid)
        {
            //杂项标注自身属性表数据
            var zxbzn = xmlDBManager.GetEntity<Gg_gl_zxbz_n>(g3e_fid);
            if (zxbzn == null) return;
            var newObject_Label = zxbzn.Clone() as Gg_gl_zxbz_n;
            //var bzdysb=GetBZ_DYSB(newObject_Label.BZ_DYSB);
            newObject_Label.G3E_ID = newG3e_id;
            newObject_Label.G3E_FID = newG3e_fid;
            //newObject_Label.BZ_DYSB = bzdysb;
            newObject_Label.EntityState = EntityState.Insert;
            newObject_Label.SetValue("LTT_ID", (decimal)MapConfig.Instance.LTTID);
            DBManager.Instance.Insert(newObject_Label);
        }
        /// <summary>
        /// 获取杂标的对应设备FID
        /// </summary>
        /// <param name="oldG3e_fid"></param>
        /// <returns></returns>
        private static string GetBZ_DYSB(string oldG3e_fid)
        {
            var dysbfid = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(oldG3e_fid))
                {
                    var g3e_fid = long.Parse(oldG3e_fid);
                    var newIds = DBEntityCopy.Instance._G3EIdMapping.SingleOrDefault(o => o.Key == g3e_fid);
                    dysbfid = newIds.Value.newG3e_fid.ToString();
                }
            }catch
            {
                LogManager.Instance.Error("获取杂标对应设备FID错误！");
            }
            return dysbfid;
        }

        /// <summary>
        /// 添加数据，更改G3E_ID
        /// </summary>
        /// <param name="kv"> </param>
        /// <param name="newG3e_id"> </param>
        /// <param name="newG3e_fid"> </param>
        /// <param name="oldG3e_fid"> </param>
        /// <param name="g3e_fno"> </param>
        private static void InsertPointDBEntity(KeyValuePair<ObjectId, DBEntity> kv, long newG3e_id, long newG3e_fid, long oldG3e_fid, long g3e_fno)
        {
            ElectronSymbol newObject_pt = null;
            var entry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == g3e_fno);
            if (entry == null) return;
            if (!string.IsNullOrEmpty(entry.SymbolPtTable.Trim()))
            {
                newObject_pt = kv.Value.Clone() as ElectronSymbol;
                if (newObject_pt != null)
                {
                    newObject_pt.G3E_ID = newG3e_id;
                    newObject_pt.G3E_FID = newG3e_fid;
                    newObject_pt.EntityState = EntityState.Insert;
                    newObject_pt.SetValue("LTT_ID", (decimal)MapConfig.Instance.LTTID);
                    newObject_pt = ConvertGeometry.Instance.UpdateG3E_GEOMETRY(newObject_pt, kv.Key);
                    newObject_pt.ClearSiblings();
                }
            }
            //自身
            if (!string.IsNullOrEmpty(entry.ComponentTable.SelfAttribute))
            {
                newObject_pt = InsertElectronBase(entry.ComponentTable.SelfAttribute.Trim(), oldG3e_fid, newObject_pt);
            }
            //公共
            if (!string.IsNullOrEmpty(entry.ComponentTable.Common))
            {
                newObject_pt = InsertElectronBase(entry.ComponentTable.Common.Trim(), oldG3e_fid, newObject_pt);
            }
            //连接
            if (!string.IsNullOrEmpty(entry.ComponentTable.Connectivity))
            {
                newObject_pt = InsertElectronBase(entry.ComponentTable.Connectivity.Trim(), oldG3e_fid, newObject_pt);
            }
            //功能位置
            if (!string.IsNullOrEmpty(entry.ComponentTable.Gnwz))
            {
                newObject_pt = InsertElectronBase(entry.ComponentTable.Gnwz.Trim(), oldG3e_fid, newObject_pt);
            }
            //包含
            if (!string.IsNullOrEmpty(entry.ComponentTable.Contain))
            {
                newObject_pt = InsertElectronBase(entry.ComponentTable.Contain.Trim(), oldG3e_fid, newObject_pt);
            }
            //详表
            if (!string.IsNullOrEmpty(entry.ComponentTable.Detailreference))
            {
                newObject_pt = InsertElectronBase(entry.ComponentTable.Detailreference.Trim(), oldG3e_fid, newObject_pt);
            }
            //其他数据（台架、开关柜）
            newObject_pt=GetOtherDBEntity(oldG3e_fid,newObject_pt);
            DBManager.Instance.Insert(newObject_pt);
            PublicMethod.Instance.UpdateDBSymbolFinder(kv.Key, newObject_pt);
        }
        /// <summary>
        /// 获取其他数据（台架、开关柜）
        /// </summary>
        /// <param name="oldG3e_fid"></param>
        /// <param name="newObject_pt"></param>
        /// <returns></returns>
        private static ElectronSymbol GetOtherDBEntity(long oldG3e_fid, ElectronSymbol newObject_pt)
        {
            if (newObject_pt.G3E_FNO == 148)
            {
                //台架
                newObject_pt= GetTjDBEntity(oldG3e_fid, newObject_pt);
            }
            return newObject_pt;
        }
        private static ElectronSymbol GetTjDBEntity(long oldG3e_fid, ElectronSymbol newObject_pt)
        {
            var parentGnwz = xmlDBManager.GetEntity<Gg_pd_gnwzmc_n>(oldG3e_fid);
            if (parentGnwz != null)
            {
                if (parentGnwz.GNWZ_FL2.Equals("台架"))
                {
                    var comTJ = xmlDBManager.GetEntity<Common_n>(o => o.G3E_FNO == 199 && o.G3E_FID.ToString().Equals(parentGnwz.GNWZ_SSTJ));
                    if (comTJ != null)
                    {
                        var newG3e_id = CYZCommonFunc.getid();
                        var newG3e_fid = CYZCommonFunc.getid();

                        var com = xmlDBManager.GetEntity<Common_n>(comTJ.G3E_FID);
                        InsertElectronBase(com, newG3e_id, newG3e_fid);

                        var self = xmlDBManager.GetEntity<Gg_gz_tj_n>(comTJ.G3E_FID);
                        if (self != null)
                        {
                            //更新变压器的fid
                            self.GNWZ_FID = newObject_pt.G3E_FID.ToString();
                            InsertElectronBase(self, newG3e_id, newG3e_fid);
                        }
                        var gnwz = xmlDBManager.GetEntity<Gg_pd_gnwzmc_n>(comTJ.G3E_FID);
                        if (gnwz != null)
                        {
                            InsertElectronBase(gnwz, newG3e_id, newG3e_fid);
                            //更新父设备的所属台架
                            DBEntityCopy.Instance.UpdateGnwzSstj(newObject_pt.G3E_FID, newG3e_fid);
                        }
                    }
                }
            }
            return newObject_pt;
        }

        private static void InsertElectronBase(DBEntity dbEntity,long newG3e_id, long newG3e_fid)
        {
            if (dbEntity == null) return;
            var newObject_n = dbEntity.Clone() as ElectronBase;
            newObject_n.G3E_FID = newG3e_fid;
            newObject_n.G3E_ID = newG3e_id;
            newObject_n.EntityState = EntityState.Insert;
            newObject_n.SetValue("LTT_ID", (decimal)MapConfig.Instance.LTTID);
            DBManager.Instance.Insert(newObject_n);
        }

        private static ElectronSymbol InsertElectronBase(string className, long oldG3e_fid,ElectronSymbol newObject_pt)
        {
            try
            {
                var flag = false;
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className);
                if (xmlDBManager.Has(type))
                {
                    //包含表特殊处理
                    if (className.Contains("Contain_n"))
                    {
                        var contains = xmlDBManager.GetEntities<Contain_n>(o => o.G3E_FID == oldG3e_fid);
                        if (contains != null)
                            foreach (var item in contains)
                            {
                                //如果电杆存在，则根据电杆的FID去导线的包含关系表去查询，
                                //根据导线的包含关系表中的G3E_OWNERFID是否等于电杆的FID，
                                //反之只有导线复制的话，则取导线的任意一条对应的包含关系清空G3E_OWNERFID值即可。
                                if (newObject_pt.G3E_FNO != 141)
                                    SetContainTable(item, newObject_pt.G3E_FID, false);
                                else
                                {
                                    var bl = HasDg(contains);
                                    if (bl)
                                    {
                                        //判断当前数据是否有被电杆包含
                                        if (DBEntityCopy.Instance.HasDg(item))
                                        {
                                            SetContainTable(item, newObject_pt.G3E_FID, false);
                                        }
                                    }
                                    else
                                    {
                                        //导线有被电杆包含，但是当前没有拷贝电杆，所以只取其中任意一条包含数据，清空G3E_OWNERFID值即可。
                                        if (!flag)
                                        {
                                            SetContainTable(item, newObject_pt.G3E_FID, true);
                                            flag = true;
                                        }
                                    }
                                }
                            }
                    }
                    else
                    {
                        var brotherValue = xmlDBManager.GetEntity(type, oldG3e_fid);
                        if (brotherValue != null)
                        {
                            DBEntityCopy.Instance.InsertBrotherTable(className, brotherValue, newObject_pt);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return newObject_pt;
        }
        /// <summary>
        /// 判断集合里是否有被电杆包含
        /// </summary>
        /// <param name="electronBase"></param>
        /// <returns></returns>
        private static bool HasDg(IEnumerable<Contain_n> electronBase)
        {
            var result = false;
            foreach (var item in electronBase)
            {
                if (DBEntityCopy.Instance.HasDg(item))
                {
                    result = true;
                }
            }
            return result;
        }

        private static void SetContainTable(ICloneable dbEntity, long newG3e_fid, bool flag)
        {
            var contain = dbEntity.Clone() as Contain_n;
            if (contain == null) return;
            contain.G3E_FID = newG3e_fid;
            contain.G3E_ID = CYZCommonFunc.getid();
            contain.EntityState = EntityState.Insert;
            contain.SetValue("LTT_ID", (decimal)MapConfig.Instance.LTTID);
            DBManager.Instance.Insert(contain);
            if (flag)
                contain.G3E_OWNERFID = 0;

            var value = dbEntity as Contain_n;
            switch (contain.G3E_FNO)
            {
                case 141:
                    DBEntityCopy.Instance._containDxTableData.Add(contain);
                    break;
                case 201:
                    if (value != null) DBEntityCopy.Instance._containDgTableData.Add(value.G3E_FID, contain);
                    break;
            }
        }
        #endregion
    }
}