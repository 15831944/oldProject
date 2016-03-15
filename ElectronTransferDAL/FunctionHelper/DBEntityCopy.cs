using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferDal.Cad;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;

namespace ElectronTransferDal.FunctionHelper
{
    public class DBEntityCopy:Singleton<DBEntityCopy>
    {
        /// <summary>
        /// 电杆
        /// </summary>
        public HashSet<long> dgList { set; get; }
        /// <summary>
        /// 保存复制数据
        /// </summary>
        public  Dictionary<ObjectId, CopySymbolObject> _copyObjects = new Dictionary<ObjectId, CopySymbolObject>();

        /// <summary>
        /// 新旧G3E系列ID对应关系
        /// </summary>
        public  Dictionary<long, ExpandsData> _G3EIdMapping = new Dictionary<long, ExpandsData>();

        /// <summary>
        /// 连接数据
        /// </summary>
        public HashSet<Connectivity_n> _connectionTableData = new HashSet<Connectivity_n>();

        /// <summary>
        /// 连接关系的从属性关系数据
        /// </summary>
        public Dictionary<long, Common_n> _subordinateSbTableData = new Dictionary<long, Common_n>();

        /// <summary>
        /// 从属关系处理
        /// </summary>
        public Dictionary<long, Common_n> _subordinateDfTableData = new Dictionary<long, Common_n>();

        /// <summary>
        /// 导线包含关系
        /// </summary>
        public HashSet<Contain_n> _containDxTableData = new HashSet<Contain_n>();

        /// <summary>
        /// 包含关系
        /// </summary>
        public Dictionary<long, Contain_n> _containDgTableData = new Dictionary<long, Contain_n>();
        /// <summary>
        /// 是否满足复制要求
        /// </summary>
        public bool isAccord { set; get; }
        /// <summary>
        /// 复制错误信息
        /// </summary>
        public Dictionary<long, string> _copyErrorInfo{set;get;}
        /// <summary>
        /// 是否复制设备
        /// </summary>
        public bool isDBEntityCopy { set; get; }
        #region 复制

        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="objectIds">CAD实体对象集合</param>
        /// <returns></returns>
        public  Dictionary<ObjectId, CopySymbolObject> ObjectCopy(ObjectId[] objectIds)
        {
            isAccord = true;
            var point=new Point3d();
            G3EObject g3eObject = null;
            _copyErrorInfo=new Dictionary<long, string>();
            var copySymbolObject = new Dictionary<ObjectId, CopySymbolObject>();
            try
            {
                if (PublicMethod.Instance.IsExistPointSymbol(objectIds, ref point))
                {
                    dgList = new HashSet<long>();
                    foreach (var objectId in objectIds)
                    {
                        var entity = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead) as Entity;
                        if (!DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject)) continue;
                        //杂项标注不复制
                        if(g3eObject.G3E_FNO==250) continue;
                        var objectType = entity is BlockReference || entity is Polyline;
                        var isLabel=entity is DBText || entity is MText;
                        var csd = GetBrotherDBEntity(g3eObject, objectId, objectType,isLabel);
                        if (!copySymbolObject.ContainsKey(objectId))
                            copySymbolObject.Add(objectId, csd);
                    }
                }
                else
                {
                    PublicMethod.Instance.ShowMessage("不能单独复制标注！");
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return copySymbolObject;
        }


        /// <summary>
        /// 复制符号数据
        /// </summary>
        /// <param name="g3eObject">G3E系列ID</param>
        /// <param name="objectId">CAD实体对象ID</param>
        /// <param name="objectType">对象类型，true为设备，false为标注</param>
        /// <param name="isLabel">是否是标注，true为标注，反之</param>
        /// <returns></returns>
        private CopySymbolObject GetBrotherDBEntity(G3EObject g3eObject, ObjectId objectId, bool objectType,bool isLabel)
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
                            //保存电杆数据
                            if (ptSymbolValue.G3E_FNO == 201)
                            {
                                if (!dgList.Contains(ptSymbolValue.G3E_FID))
                                    dgList.Add(ptSymbolValue.G3E_FID);
                            }
                            //标注的LB数据
                            if (isLabel)
                                copyDBEntity = GetLabelDBEntityObjects(ptSymbolValue.GetType().Name, g3eObject,copyDBEntity);

                            //杂项标注
                            if (g3eObject.G3E_FNO == 250)
                                copyDBEntity=GetZxbzDBEntity(g3eObject.G3E_FID, copyDBEntity);
                        }
                        //属性数据
                        if (objectType)
                        {
                            copyDBEntity = GetDBEntityObjects(g3eObject, entry, copyDBEntity);
                        }
                        cso.g3eObject = g3eObject;
                        cso.objectID = objectId;
                        cso.hsDBEntity = copyDBEntity;
                    }
                }
            }
            catch (NotExistException ex)
            {
                LogManager.Instance.Error(ex);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return cso;
        }
        /// <summary>
        /// 获取杂项标注符号表
        /// </summary>
        /// <param name="ptClassName">组件名称</param>
        /// <param name="g3eObject">G3E系列ID</param>
        /// <param name="copyDBEntity">组件表集合</param>
        /// <returns></returns>
        private HashSet<DBEntityObject>GetLabelDBEntityObjects(string ptClassName,G3EObject g3eObject,HashSet<DBEntityObject> copyDBEntity)
        {
            var labelTable = ptClassName.Replace("_sdogeom", "");

            //标注符号表
            if (!string.IsNullOrEmpty(labelTable))
            {
                copyDBEntity = GetBrotherDBEntity(labelTable.Trim(), copyDBEntity, g3eObject.G3E_FID, false);
            }
            return copyDBEntity;
        }
        /// <summary>
        /// 获取杂项标注自身表
        /// </summary>
        /// <param name="g3e_fid">G3E_FID</param>
        /// <param name="backupEntity">组件表集合</param>
        /// <returns></returns>
        private HashSet<DBEntityObject> GetZxbzDBEntity(long g3e_fid, HashSet<DBEntityObject> backupEntity)
        {
            //杂项标注自身属性表数据
            var zxbzn = DBManager.Instance.GetEntity<Gg_gl_zxbz_n>(g3e_fid);
            backupEntity = GetBrotherDBEntity("Gg_gl_zxbz_n", zxbzn, backupEntity);
            return backupEntity;
        }
        /// <summary>
        /// 复制当前设备所需数据
        /// </summary>
        /// <param name="g3eObject">G3E系列ID</param>
        /// <param name="entry">当前设备的配置数据</param>
        /// <param name="copyDBEntity">组件表集合</param>
        /// <returns>返回当前设备所有组件表集合</returns>
        private  HashSet<DBEntityObject> GetDBEntityObjects(G3EObject g3eObject,DBSymbolEntry  entry, HashSet<DBEntityObject> copyDBEntity)
        {
            //自身属性
            if (!string.IsNullOrEmpty(entry.ComponentTable.SelfAttribute))
            {
                copyDBEntity = GetBrotherDBEntity(entry.ComponentTable.SelfAttribute.Trim(), copyDBEntity, g3eObject.G3E_FID, true);
            }
            //公共属性
            if (!string.IsNullOrEmpty(entry.ComponentTable.Common))
            {
                copyDBEntity = GetBrotherDBEntity(entry.ComponentTable.Common.Trim(), copyDBEntity, g3eObject.G3E_FID, true);
            }
            //连接关系
            if (!string.IsNullOrEmpty(entry.ComponentTable.Connectivity))
            {
                copyDBEntity = GetBrotherDBEntity(entry.ComponentTable.Connectivity.Trim(), copyDBEntity, g3eObject.G3E_FID, true);
            }
            //功能位置
            if (!string.IsNullOrEmpty(entry.ComponentTable.Gnwz))
            {
                copyDBEntity = GetBrotherDBEntity(entry.ComponentTable.Gnwz.Trim(), copyDBEntity, g3eObject.G3E_FID, true);
            }
            //包含表
            if (!string.IsNullOrEmpty(entry.ComponentTable.Contain))
            {
                bool isNeed = g3eObject.G3E_FNO==201;
                //电杆必须有包含关系（导线不是必须）
                copyDBEntity = GetContainDBEntity(entry.ComponentTable.Contain.Trim(), copyDBEntity, g3eObject.G3E_FID, isNeed);
            }
            //详表
            if (!string.IsNullOrEmpty(entry.ComponentTable.Detailreference))
            {
                copyDBEntity = GetBrotherDBEntity(entry.ComponentTable.Detailreference.Trim(), copyDBEntity, g3eObject.G3E_FID, false);
            }
            //受电馈线
            if (!string.IsNullOrEmpty(entry.ComponentTable.Gg_Pd_Sdkx_Ac))
            {
                copyDBEntity = GetSdkxDBEntity(entry.ComponentTable.Gg_Pd_Sdkx_Ac.Trim(), copyDBEntity, g3eObject.G3E_FID);
            }
            //其他数据（台架、开关柜）
            copyDBEntity=GetOtherDBEntity(g3eObject, copyDBEntity);
            return copyDBEntity;
        }

        /// <summary>
        /// 复制符号数据
        /// </summary>
        /// <param name="className">组件表名称</param>
        /// <param name="copyDBEntity">组件表集合</param>
        /// <param name="oldFid">当前设备的G3E_FID</param>
        /// <param name="isNeed">是否校验组件表，true校验，反之</param>
        /// <returns>返回复制的组件表集合</returns>
        private  HashSet<DBEntityObject> GetBrotherDBEntity(string className, HashSet<DBEntityObject> copyDBEntity, long oldFid,bool isNeed)
        {
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), className.Trim());
                var brotherValue = DBManager.Instance.GetEntity(type, oldFid);
                if (brotherValue != null)
                {
                    if (brotherValue.HasAttribute("SBMC"))
                        ClearCommon(brotherValue.Clone() as DBEntity);
                    copyDBEntity = GetBrotherDBEntity(className, brotherValue, copyDBEntity);
                }
                else
                {
                    isAccord = !isNeed;
                    _copyErrorInfo.Add(oldFid, className);
                }
            }
            catch (Exception ex)
            {
                if (isNeed)
                {
                    isAccord = false;
                    _copyErrorInfo.Add(oldFid, className);
                    LogManager.Instance.Error(ex + string.Format("复制错误：{0} {1}", className, oldFid));
                }
            }
            return copyDBEntity;
        }

        private HashSet<DBEntityObject> GetContainDBEntity(string className, HashSet<DBEntityObject> copyDBEntity, long oldFid, bool isNeed)
        {
            try
            {
                //包含表特殊查询
                var Contains = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == oldFid);
                if (Contains.Any())
                {
                    copyDBEntity = Contains.Aggregate(copyDBEntity, (current, item) => GetBrotherDBEntity(className, item, current));
                }else
                {
                    isAccord = !isNeed;
                }
            }catch(Exception ex)
            {
                if (isNeed)
                {
                    isAccord = false;
                    LogManager.Instance.Error(ex + string.Format("复制错误：{0} {1}", className, oldFid));
                }
            }
            return copyDBEntity;
        }
        
        /// <summary>
        /// 受电馈线
        /// </summary>
        /// <param name="className"></param>
        /// <param name="copyDBEntity"></param>
        /// <param name="oldFid"></param>
        /// <returns></returns>
        private HashSet<DBEntityObject> GetSdkxDBEntity(string className, HashSet<DBEntityObject> copyDBEntity, long oldFid)
        {
            try
            {
                //包含表特殊查询
                var sdkxs = DBManager.Instance.GetEntities<Gg_pd_sdkx_ac>(o => o.G3E_FID == oldFid);
                copyDBEntity = sdkxs.Aggregate(copyDBEntity, (current, sdkx) => GetBrotherDBEntity(className, sdkx, current));
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return copyDBEntity;
        }
        private HashSet<DBEntityObject>GetBrotherDBEntity(string className,DBEntity dbEntity,HashSet<DBEntityObject> copyDBEntity)
        {
            if (dbEntity != null)
            {
                var dbEntityObject = new DBEntityObject {className = className.Trim(), dbEntity = dbEntity};
                copyDBEntity.Add(dbEntityObject);
            }
            return copyDBEntity;
        }

        private HashSet<DBEntityObject> GetOtherDBEntity(G3EObject g3eObject,HashSet<DBEntityObject> copyDBEntity)
        {
            if (g3eObject.G3E_FNO == 148)
            {
                //台架
                copyDBEntity = GetTjDBEntity(g3eObject.G3E_FID, copyDBEntity);
            }
            return copyDBEntity;
        }
        /// <summary>
        /// 获取台架数据
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="copyDBEntity"> </param>
        private HashSet<DBEntityObject> GetTjDBEntity(long g3e_fid, HashSet<DBEntityObject> copyDBEntity)
        {
            var parentGnwz = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(g3e_fid);
            if (parentGnwz != null)
            {
                if (parentGnwz.GNWZ_FL2.Equals("台架"))
                {
                    copyDBEntity = GetTjDBEntity(parentGnwz.GNWZ_SSTJ, copyDBEntity);
                }
            }
            return copyDBEntity;
        }
        private HashSet<DBEntityObject> GetTjDBEntity(string gnwz_sstj, HashSet<DBEntityObject> copyDBEntity)
        {
            if (string.IsNullOrEmpty(gnwz_sstj))
            {
                LogManager.Instance.Error("错误信息：功能位置所属台架字段空值！");
            }
            else
            {
                var comTJ =DBManager.Instance.GetEntity<Common_n>(o => o.G3E_FNO == 199 && o.G3E_FID.ToString().Equals(gnwz_sstj));
                if (comTJ != null)
                {
                    copyDBEntity = GetBrotherDBEntity("Common_n", comTJ, copyDBEntity);

                    var self = DBManager.Instance.GetEntity<Gg_gz_tj_n>(comTJ.G3E_FID);
                    if (self != null)
                    {
                        copyDBEntity = GetBrotherDBEntity("Gg_gz_tj_n", self, copyDBEntity);
                    }
                    var gnwz = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(comTJ.G3E_FID);
                    if (gnwz != null)
                    {
                        copyDBEntity = GetBrotherDBEntity("Gg_pd_gnwzmc_n", gnwz, copyDBEntity);
                    }
                }
            }
            return copyDBEntity;
        }

        #endregion

        #region 粘贴

        /// <summary>
        /// 粘贴选择
        /// </summary>
        /// <param name="oldId">旧对象ID</param>
        /// <param name="newId">新对象ID</param>
        public  void ObjectPaste(ObjectId oldId, ObjectId newId)
        {
            G3EObject oldG3eObject = null;
            try
            {
                if (DBEntityFinder.Instance.GetG3EIds(oldId, ref oldG3eObject))
                {
                    ObjectPaste(oldG3eObject, oldId, newId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        /// <param name="oldG3eObject"></param>
        /// <param name="oldId"></param>
        /// <param name="newId"></param>
        private void ObjectPaste(G3EObject oldG3eObject, ObjectId oldId, ObjectId newId)
        {
            //生成新的G3E系列ID
            CreateG3EFID(oldG3eObject.G3E_FID, oldG3eObject.G3E_ID, newId);
            if (_copyObjects.Count <= 0) return;
            var cso = _copyObjects.SingleOrDefault(o => o.Key == oldId);
            if (cso.Value == null) return;
            var newIds = _G3EIdMapping.SingleOrDefault(o => o.Key == oldG3eObject.G3E_FID);
            AddDBEntity(newIds.Value.newG3e_id, newIds.Value.newG3e_fid, newId, cso);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newG3e_id"></param>
        /// <param name="newG3e_fid"> </param>
        /// <param name="newId"></param>
        /// <param name="cso"> </param>
        private  void AddDBEntity(long newG3e_id, long newG3e_fid, ObjectId newId, KeyValuePair<ObjectId, CopySymbolObject> cso)
        {
            try
            {
                //插入坐标数据
                var pt = InsertElectronSymbol(newG3e_id, newG3e_fid, newId, cso.Value);
                //插入设备属性数据
                pt = InsertElectronBase(cso.Value, pt);
                DBManager.Instance.Insert(pt);
                PublicMethod.Instance.AddDBSymbolFinder(newId, pt);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 添加坐标表数据
        /// </summary>
        /// <param name="newG3e_id"></param>
        /// <param name="newG3e_fid"></param>
        /// <param name="objectId"></param>
        /// <param name="cso"></param>
        /// <returns></returns>
        private  ElectronSymbol InsertElectronSymbol(long newG3e_id, long newG3e_fid, ObjectId objectId,
                                                    CopySymbolObject cso)
        {
            ElectronSymbol newObject_pt = null;
            if (cso.pointDBEntity != null)
            {
                newObject_pt = cso.pointDBEntity.Clone() as ElectronSymbol;
                newObject_pt.ClearSiblings();
                newObject_pt.G3E_ID = newG3e_id;
                newObject_pt.G3E_FID = newG3e_fid;
                newObject_pt.EntityState = EntityState.Insert;
                newObject_pt.EntityState2 = EntityState2.Copy;
                newObject_pt.SetValue("LTT_ID", (decimal)MapConfig.Instance.LTTID);
                newObject_pt = ConvertGeometry.Instance.UpdateG3E_GEOMETRY(newObject_pt, objectId);
            }
            return newObject_pt;
        }

        /// <summary>
        /// 添加属性表数据
        /// </summary>
        /// <param name="cso"></param>
        /// <param name="es"></param>
        /// <returns></returns>
        private  ElectronSymbol InsertElectronBase(CopySymbolObject cso, ElectronSymbol es)
        {
            var flag = false;
            var otherIdMapping = new Dictionary<long, ExpandsData>();
            foreach (var dbEntity in cso.hsDBEntity)
            {
                var newObject = dbEntity.dbEntity as ElectronBase;
                if (dbEntity.className.Contains("Contain"))
                {
                    //如果电杆存在，则根据电杆的FID去导线的包含关系表去查询，
                    //根据导线的包含关系表中的G3E_OWNERFID是否等于电杆的FID，
                    //反之只有导线复制的话，则取导线的任意一条对应的包含关系清空G3E_OWNERFID值即可。
                    if (es.G3E_FNO != 141)
                        SetContainTable(newObject, es.G3E_FID, false);
                    else
                    {
                        var bl = HasDg(cso.hsDBEntity);
                        if (bl)
                        {
                            //判断当前数据是否有被电杆包含
                            if (HasDg(dbEntity.dbEntity))
                            {
                                SetContainTable(newObject, es.G3E_FID, false);
                            }
                        }
                        else
                        {
                            //导线有被电杆包含，但是当前没有拷贝电杆，所以只取其中任意一条包含数据，清空G3E_OWNERFID值即可。
                            if (!flag)
                            {
                                SetContainTable(newObject, es.G3E_FID, true);
                                flag = true;
                            }
                        }
                    }
                }
                else if (dbEntity.className.Equals("Gg_pd_sdkx_ac"))
                {
                    var g3d_id = CYZCommonFunc.getid();
                    var sdkxac = newObject as Gg_pd_sdkx_ac;
                    sdkxac.G3E_ID = g3d_id;
                    sdkxac.G3E_FID = es.G3E_FID;
                    sdkxac.EntityState = EntityState.Insert;
                    sdkxac.SetValue("LTT_ID", (decimal) MapConfig.Instance.LTTID);
                    DBManager.Instance.Insert(sdkxac);
                }
                else
                {
                    //台架和开关柜
                    if (newObject.G3E_FNO == 198 || newObject.G3E_FNO == 199)
                    {
                        otherIdMapping = CreateG3EFID(newObject.G3E_FID, es.G3E_ID, otherIdMapping);
                        AddKggDBEntity(es.G3E_FID,newObject, otherIdMapping);
                    }
                    else
                        InsertBrotherTable(dbEntity.className, newObject, es);
                }
            }
            return es;
        }

        /// <summary>
        /// 新增台架
        /// </summary>
        /// <param name="panrentFid"></param>
        /// <param name="dbEntity"></param>
        /// <param name="idMapping"></param>
        private void AddKggDBEntity(long panrentFid,DBEntity dbEntity, Dictionary<long, ExpandsData> idMapping)
        {
            var newObject_n = dbEntity.Clone() as ElectronBase;
            var newIds = idMapping.SingleOrDefault(o => o.Key == newObject_n.G3E_FID);

            if(newObject_n.G3E_FNO==199)
            {
                if (newObject_n.GetType().Name.Equals("Gg_pd_gnwzmc_n"))
                {
                    UpdateGnwzSstj(panrentFid, newIds.Value.newG3e_fid);
                }
            }if (newObject_n.GetType().Name.Equals("Common_n"))
                ClearCommon(newObject_n);
            if (newObject_n.GetType().Name.Equals("Gg_gz_tj_n"))
            {
                if (newObject_n.HasAttribute("GNWZ_FID"))
                    newObject_n.SetValue("GNWZ_FID", panrentFid.ToString());
            }

            newObject_n.G3E_ID = newIds.Value.newG3e_id;
            newObject_n.G3E_FID = newIds.Value.newG3e_fid;
            newObject_n.EntityState = EntityState.Insert;
            newObject_n.SetValue("LTT_ID", (decimal) MapConfig.Instance.LTTID);
            DBManager.Instance.Insert(newObject_n);
        }
        /// <summary>
        /// 更新父设备的所属台架
        /// </summary>
        /// <param name="parentG3e_fid"></param>
        /// <param name="childG3e_fid"></param>
        public void UpdateGnwzSstj(long parentG3e_fid, long childG3e_fid)
        {
            var parent = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(parentG3e_fid);
            if (parent == null) return;
            parent.GNWZ_SSTJ = childG3e_fid.ToString();
            DBManager.Instance.Update(parent);
        }

        /// <summary>
        /// 判断集合里是否有被电杆包含
        /// </summary>
        /// <param name="electronBase"></param>
        /// <returns></returns>
        private  bool HasDg(IEnumerable<DBEntityObject> electronBase)
        {
            var result = false;
            foreach (var item in electronBase)
            {
                var dbEntity = item.dbEntity;
                if (!(dbEntity is Contain_n)) continue;
                if (HasDg(dbEntity))
                {
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// 判断导线是否被电杆包含
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public  bool HasDg(DBEntity dbEntity)
        {
            var result = false;
            var contain = dbEntity as Contain_n;
            var dx = dgList.SingleOrDefault(o => o == contain.G3E_OWNERFID);
            if (dx > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 添加组件表
        /// </summary>
        /// <param name="className"></param>
        /// <param name="dbEntity"></param>
        /// <param name="es"></param>
        public void InsertBrotherTable(string className, DBEntity dbEntity, ElectronSymbol es)
        {
            try
            {
                var newObject_n = dbEntity.Clone() as ElectronBase;
                switch (className)
                {
                    case "Connectivity_n":
                        _connectionTableData.Add(newObject_n as Connectivity_n);
                        newObject_n.EntityState = EntityState.Add_Nal_Nal;
                        break;
                    case "Common_n":
                        //存储电房的G3E_ID
                        if (es.G3E_GEOMETRY is Polygon)
                            _subordinateDfTableData.Add((dbEntity as ElectronBase).G3E_ID, newObject_n as Common_n);
                        else
                        {
                            var comm = newObject_n as Common_n;
                            _subordinateSbTableData.Add(comm.G3E_ID, comm);
                        }
                        //清空设备名称
                        ClearCommon(newObject_n);
                        newObject_n.EntityState = EntityState.Insert;
                        break;
                    case "Detailreference_n":
                        var detailref = newObject_n as Detailreference_n;
                        detailref.G3E_DETAILID = CYZCommonFunc.getid();
                        detailref.EntityState = EntityState.Insert;
                        detailref.DETAIL_USERNAME = es.G3E_FID.ToString();
                        newObject_n = detailref;
                        break;
                    case "Gg_pd_gnwzmc_n":
                        ClearGnwzmcn(newObject_n);
                        newObject_n.EntityState = EntityState.Insert;
                        break;
                    default:
                        newObject_n.EntityState = EntityState.Insert;
                        break;

                }
                newObject_n.G3E_FID = es.G3E_FID;
                newObject_n.G3E_ID = es.G3E_ID;
                newObject_n.SetValue("LTT_ID", (decimal) MapConfig.Instance.LTTID);
                DBManager.Instance.Insert(newObject_n);
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 清空公共属性表特殊值
        /// </summary>
        /// <param name="electronBase"></param>
        /// <returns></returns>
        private DBEntity ClearCommon(DBEntity electronBase)
        {
            if (electronBase.HasAttribute("SBMC"))
                electronBase.SetValue("SBMC", electronBase.GetValue<int>("G3E_FNO") == 144 ? "站房引线" : "");
            if (electronBase.HasAttribute("TYRQ"))
            {
                electronBase.SetValue("TYRQ", null);
            }
            if (electronBase.HasAttribute("BZ1"))
            {
                electronBase.SetValue("BZ1", "");
            }
            return electronBase;
        }
        /// <summary>
        /// 清空功能位置表里的特殊值
        /// </summary>
        /// <param name="electronBase"></param>
        /// <returns></returns>
        private DBEntity ClearGnwzmcn(DBEntity electronBase)
        {
            //清空所属开关柜属性
            if(electronBase.HasAttribute("GNWZ_SSKGG"))
            {
                electronBase.SetValue("GNWZ_SSKGG", "");
            }
            if (electronBase.HasAttribute("GNWZ_SSGT"))
            {
                electronBase.SetValue("GNWZ_SSGT", "");
            }
            if (electronBase.HasAttribute("GNWZ_SSTJ"))
            {
                electronBase.SetValue("GNWZ_SSTJ", "");
            }
            if (electronBase.HasAttribute("GNWZ_SSTQHTJ"))
            {
                electronBase.SetValue("GNWZ_SSTQHTJ", "");
            }
            return electronBase;
        }

        /// <summary>
        /// 插入包含表数据
        /// </summary>
        /// <param name="newObject"></param>
        /// <param name="newG3e_fid"></param>
        /// <param name="flag">true清空G3E_OWNERFID值，反之 </param>
        private  void SetContainTable(ICloneable newObject, long newG3e_fid, bool flag)
        {
            var contain = newObject.Clone() as Contain_n;

            contain.G3E_ID = CYZCommonFunc.getid();
            contain.G3E_FID = newG3e_fid;
            contain.G3E_CID = 1;
            contain.G3E_CNO = 38;
            contain.EntityState = EntityState.Insert;
            contain.EntityState2 = EntityState2.Copy;
            contain.SetValue("LTT_ID", (decimal)MapConfig.Instance.LTTID);
            if (flag)
                contain.G3E_OWNERFID = 0;

            DBManager.Instance.Insert(contain);

            var value = newObject as Contain_n;
            switch (contain.G3E_FNO)
            {
                case 141:
                    _containDxTableData.Add(contain);
                    break;
                case 201:
                    _containDgTableData.Add(value.G3E_FID, contain);
                    break;
            }
        }

        /// <summary>
        /// 生成新的G3E系列ID
        /// </summary>
        /// <param name="OldG3e_fid"></param>
        /// <param name="OldG3e_id"> </param>
        /// <param name="newObjectId"> </param>
        public  void CreateG3EFID(long OldG3e_fid, long OldG3e_id,ObjectId newObjectId)
        {
            var entity = PublicMethod.Instance.GetObject(newObjectId, OpenMode.ForRead);
            if (!_G3EIdMapping.ContainsKey(OldG3e_fid))
            {
                var ed = new ExpandsData
                             {
                                 newG3e_id = CYZCommonFunc.getid(),
                                 newG3e_fid = CYZCommonFunc.getid(),
                                 oldG3e_id = OldG3e_id,
                                 oldG3e_fid = OldG3e_fid,
                                 newObjectId = newObjectId
                             };
                _G3EIdMapping.Add(OldG3e_fid, ed);
            }
            else
            {
                //只存储点符号的对象ID（不存储标注）
                if (entity is BlockReference)
                {
                    var aa = _G3EIdMapping[OldG3e_fid];
                    aa.newObjectId = newObjectId;
                }
            }
        }

        /// <summary>
        /// 生成新的G3E系列ID
        /// </summary>
        /// <param name="OldG3e_fid"></param>
        /// <param name="oldParentG3e_id"> </param>
        /// <param name="g3e_IdMapping"></param>
        /// <returns></returns>
        public Dictionary<long,ExpandsData> CreateG3EFID(long OldG3e_fid,long oldParentG3e_id, Dictionary<long,ExpandsData> g3e_IdMapping)
        {
            if (!g3e_IdMapping.ContainsKey(OldG3e_fid))
            {
                var ed = new ExpandsData
                             {
                                 newG3e_id = CYZCommonFunc.getid(),
                                 newG3e_fid = CYZCommonFunc.getid(),
                                 oldG3e_fid = OldG3e_fid,
                                 oldParentG3e_Id = oldParentG3e_id
                             };
                g3e_IdMapping.Add(OldG3e_fid, ed);
            }
            return g3e_IdMapping;
        }


        #endregion


        #region 处理数据 连接关系、从属关系


        /// <summary>
        /// 更新连接关系表
        /// 把node1、node2相同的值更新
        /// 把不相同的赋0
        /// </summary>
        public void UpdateConnectionData()
        {
            var g3enode_cadnode = new Dictionary<string, string>();
            var compare = _connectionTableData.Select(item => item.Clone() as Connectivity_n).ToList();

            try
            {
                if (compare.Count > 0)
                {
                    var nodes = GetNodes(compare);
                    foreach (var item in compare)
                    {
                        bool bNode1 = false, bNode2 = false;
                        if (g3enode_cadnode.ContainsKey(item.NODE1_ID.ToString()))
                        {
                            item.NODE1_ID = long.Parse(g3enode_cadnode[item.NODE1_ID.ToString()]);
                            bNode1 = true;
                        }
                        else
                        {
                            Connectivity_n item1 = item;
                            if (nodes.Count(o => o == item1.NODE1_ID.ToString()) > 1)
                            {
                                string nodeId = CYZCommonFunc.getid().ToString();
                                g3enode_cadnode.Add(item.NODE1_ID.ToString(), nodeId);
                                item.NODE1_ID = long.Parse(nodeId);
                                bNode1 = true;
                            }
                            else
                            {
                                item.NODE1_ID = 0;
                            }
                        }
                        if (g3enode_cadnode.ContainsKey(item.NODE2_ID.ToString()))
                        {
                            item.NODE2_ID = long.Parse(g3enode_cadnode[item.NODE2_ID.ToString()]);
                            bNode2 = true;
                        }
                        else
                        {
                            Connectivity_n item1 = item;
                            if (nodes.Count(o => o == item1.NODE2_ID.ToString()) > 1)
                            {
                                string nodeId = CYZCommonFunc.getid().ToString();
                                g3enode_cadnode.Add(item.NODE2_ID.ToString(), nodeId);
                                item.NODE2_ID = long.Parse(nodeId);
                                bNode2 = true;
                            }
                            else
                            {
                                item.NODE2_ID = 0;
                            }
                        }
                        if (bNode1 && bNode2)
                            item.EntityState = EntityState.Add_Add_Add;
                        else if (!bNode1 && bNode2)
                            item.EntityState = EntityState.Add_Nal_Add;
                        else if (!bNode1 && !bNode2)
                            item.EntityState = EntityState.Add_Nal_Nal;
                        else if (bNode1 && !bNode2)
                            item.EntityState = EntityState.Add_Add_Nal;
                    }
                    compare.ForEach(o => DBManager.Instance.Update(o));
                    //处理从属关系
                    UpdateSubordinateTable();
                    //处理包含关系
                    UpdateContainTable();
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                _connectionTableData.Clear();
                _subordinateSbTableData.Clear();
                _containDxTableData.Clear();
                _containDgTableData.Clear();
                _subordinateDfTableData.Clear();
            }
        }

        private List<string> GetNodes(IEnumerable<Connectivity_n> compare)
        {
            var nodes = new List<string>();
            foreach (var item in compare)
            {
                if (item.NODE1_ID == null)
                {
                }
                else if (item.NODE1_ID.ToString() == "")
                {
                }
                else if (item.NODE1_ID == 0)
                {
                }
                else
                {
                    nodes.Add(item.NODE1_ID.ToString());
                }

                if (item.NODE2_ID == null)
                {
                }
                else if (item.NODE2_ID.ToString() == "")
                {
                }
                else if (item.NODE2_ID == 0)
                {
                }
                else
                {
                    nodes.Add(item.NODE2_ID.ToString());
                }
            }
            return nodes;
        }

        /// <summary>
        /// 更新从属关系
        /// </summary>
        private void UpdateSubordinateTable()
        {
            if (_subordinateSbTableData.Count <= 0 && _subordinateDfTableData.Count <= 0) return;
            foreach (var item in Instance._subordinateDfTableData)
            {
                KeyValuePair<long, Common_n> item1 = item;
                var values = _subordinateSbTableData.Where(o => o.Value.OWNER1_ID == item1.Key);
                foreach (var clone in values.Select(ownerid => ownerid.Value.Clone() as Common_n))
                {
                    clone.OWNER1_ID = item.Value.G3E_ID;
                    DBManager.Instance.Update(clone);
                }
            }
            UpdateDFSubordinate();
        }

        private void UpdateDFSubordinate()
        {
            if (_subordinateDfTableData.Count <= 0) return;
            //获取所有设备的从属OWNER1_ID
            var owner = _subordinateDfTableData.Select(o => o.Value.OWNER1_ID).Distinct().ToList();
            foreach (var disOwner in owner)
            {
                if (disOwner == null) continue;
                long? owner1 = disOwner;
                var dfData = _subordinateDfTableData.FirstOrDefault(o => o.Key == owner1).Value;
                if (dfData == null) continue;
                long? disOwner1 = disOwner;
                foreach (
                    var item in _subordinateDfTableData.Where(o => o.Value.OWNER1_ID == disOwner1).ToList())
                {
                    item.Value.OWNER1_ID = dfData.G3E_ID;
                    DBManager.Instance.Update(item.Value);
                }
            }
        }

        /// <summary>
        /// 更新包含关系
        /// </summary>
        private void UpdateContainTable()
        {
            foreach (var item in Instance._containDgTableData)
            {
                KeyValuePair<long, Contain_n> item2 = item;
                foreach (var item1 in _containDxTableData.Where(item1 => item1.G3E_OWNERFID == item2.Key))
                {
                    item1.G3E_OWNERFID = item.Value.G3E_FID;
                    DBManager.Instance.Update(item1);
                }
            }
        }

        #endregion

        #region 多实体复制

        /// <summary>
        /// 多实体复制
        /// </summary>
        public void DragAndCopyEntities(ObjectId[] objectIds)
        {
            try
            {
                if (objectIds.Any())
                {
                    var point = new Point3d();
                    var dragger = new ManyEntityCopy();
                    if (PublicMethod.Instance.IsExistPointSymbol(objectIds, ref point))
                    {
                        DCadApi.isModifySymbol = true;
                        var dragResult = dragger.StartDrag(point, objectIds);

                        if (dragResult.Status == PromptStatus.OK)
                        {
                            CreateTransformedCopy(dragger.DisplacementMatrix, objectIds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }

        private void CreateTransformedCopy(Matrix3d transformationMatrix, params ObjectId[] entitiesToCopy)
        {
            var acDoc = PublicMethod.Instance.acDoc;
            using (var trans = acDoc.Editor.Document.TransactionManager.StartTransaction())
            {
                var entityCollection = new ObjectIdCollection(entitiesToCopy);
                var currentDb = acDoc.Editor.Document.Database;
                var idMap = new IdMapping();

                currentDb.DeepCloneObjects(entityCollection, currentDb.CurrentSpaceId, idMap, false);
                if (isAccord)
                {
                    foreach (IdPair pair in idMap)
                    {
                        var cloneEntity = (Entity) PublicMethod.Instance.GetObject(pair.Value, OpenMode.ForRead);
                        cloneEntity.TransformBy(transformationMatrix);
                    }
                }
                trans.Commit();
            }
        }

        #endregion

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <returns></returns>
        public string GetCopyErrorInfo()
        {
            var sb = new StringBuilder();
            if (_copyErrorInfo != null)
            {
                foreach (var item in _copyErrorInfo)
                {
                    sb.Append(string.Format("FID：{0} 表名：{1}\n", item.Key, item.Value));
                }
            }
            return sb.ToString();
        }
    }
    #region 类

    public class CopySymbolObject
    {
        public G3EObject g3eObject { set; get; }
        /// <summary>
        /// 对象ID
        /// </summary>
        public ObjectId objectID { set; get; }

        /// <summary>
        /// 属性数据
        /// </summary>
        public HashSet<DBEntityObject> hsDBEntity  {set;get;}

        /// <summary>
        /// 坐标数据
        /// </summary>
        public ElectronSymbol pointDBEntity { set; get; }
    }
    public class DBEntityObject
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public string className { set; get; }

        /// <summary>
        /// 属性数据
        /// </summary>
        public DBEntity dbEntity { set; get; }
    }

    public class ExpandsData
    {
        public long newG3e_id { set; get; }
        public long newG3e_fid { set; get; }
        public long oldG3e_id { set; get; }
        public long oldG3e_fid { set; get; }
        public long oldParentG3e_Id { set; get; }
        /// <summary>
        /// 新ID
        /// </summary>
        public ObjectId newObjectId { set; get; }
    }
    public class SHBManagerArges : EventArgs
    {
        public long g3e_fid { set; get; }

    #endregion
    }
}
