using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using CADColor = Autodesk.AutoCAD.Colors.Color;
using Color = System.Drawing.Color;
using ElectronTransferDal.HelperEventArgs;

namespace ElectronTransferDal.Query
{
    public class DBEntityFinder : Singleton<DBEntityFinder>
    {
        /// <summary>
        ///记忆通用的属性 
        /// </summary>
        public Dictionary<int, XProps> MemoryDevice = new Dictionary<int, XProps>();
        /// <summary>
        /// 存储符号
        /// </summary>
        public Dictionary<string, string> blockAllList { set; get; }

        public List<string> blockList{set;get;}

        /// <summary>
        /// 符号库
        /// </summary>
        public string SymbolLibraryPath;
        /// <summary>
        /// 图片
        /// </summary>
        public string SymbolPicturePath;
        #region 条件查询

        /// <summary>
        /// 获取G3EIds
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="g3e_id"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="g3e_fno"></param>
        /// <returns></returns>
        public bool GetG3EIds(ObjectId objectId, ref long g3e_id, ref long g3e_fid, ref long g3e_fno)
        {
            if (DBSymbolFinder.Instance.ContainsKey(objectId))
            {
                var value = DBSymbolFinder.Instance[objectId];
                g3e_id = value.G3E_ID;
                g3e_fid = value.G3E_FID;
                g3e_fno = value.G3E_FNO;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取G3EIds
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="g3eObject"> </param>
        /// <returns></returns>
        public bool  GetG3EIds(ObjectId objectId,ref G3EObject g3eObject)
        {
            if (DBSymbolFinder.Instance.ContainsKey(objectId))
            {
                var value = DBSymbolFinder.Instance[objectId];
                if (value != null)
                {
                    g3eObject = new G3EObject
                                    {
                                        G3E_ID = value.G3E_ID,
                                        G3E_FID = value.G3E_FID,
                                        G3E_FNO = value.G3E_FNO
                                    };
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 判断ObjectID是否存在
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public bool HasG3EIDS(ObjectId objectId)
        {
            return DBSymbolFinder.Instance.ContainsKey(objectId);
        }

        /// <summary>
        /// 根据G3e_fid获取实体对象
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public ObjectId GetObjectIdByFid(long g3e_fid)
        {
            try
            {
                var values = GetDBSymbolFinderByFidOrEntityType(g3e_fid, EntityType.None);
                foreach (var item in values)
                {
                    return item.Key;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(string.Format("G3E_FID获取实体对象错误！{0}",ex));
            }
            return ObjectId.Null;
        }

        /// <summary>
        /// 根据G3e_fid获取实体对象
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="entityType"> </param>
        /// <returns></returns>
        public ObjectId GetObjectIdByFid(long g3e_fid,EntityType entityType)
        {
            try
            {
                var values = GetDBSymbolFinderByFidOrEntityType(g3e_fid, entityType);
                foreach (var item in values)
                {
                    return item.Key;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(string.Format("G3E_FID获取实体对象错误！{0}", ex));
            }
            return ObjectId.Null;
        }

        /// <summary>
        /// 根据G3e_fid和符号类型获取对象
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<ObjectId, ElectronSymbol>> GetDBSymbolFinderByFid(long g3e_fid)
        {
            try
            {
                return DBSymbolFinder.Instance.Where(o => o.Value.G3E_FID == g3e_fid && !o.Value.IsErased);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return null;
        }
        /// <summary>
        /// 根据G3e_fid获取点设备数据
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public KeyValuePair<ObjectId, ElectronSymbol> GetObjectIDByG3eFid(long g3e_fid)
        {
            var keyValuePair = new KeyValuePair<ObjectId, ElectronSymbol>();
            try
            {
                keyValuePair= DBSymbolFinder.Instance.SingleOrDefault(o => o.Value.G3E_FID == g3e_fid && o.Value.EntityType==EntityType.None && !o.Value.IsErased);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return keyValuePair;
        }

        /// <summary>
        /// 根据G3e_fid和符号类型获取对象
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="et"> </param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<ObjectId, ElectronSymbol>> GetDBSymbolFinderByFidOrEntityType(long g3e_fid, EntityType et)
        {
            try
            {
                return DBSymbolFinder.Instance.Where(o => o.Value.G3E_FID == g3e_fid && o.Value.EntityType == et && !o.Value.IsErased);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return null;
        }

        /// <summary>
        /// 根据G3e_fno和符号类型获取对象
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<ObjectId, ElectronSymbol>> GetDBSymbolFinderByFnoOrEntityType(long g3e_fno)
        {
            try
            {
                return DBSymbolFinder.Instance.Where(o => o.Value.G3E_FNO == g3e_fno && o.Value.EntityType!=EntityType.Label && !o.Value.IsErased);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return null;
        }
        /// <summary>
        /// 根据FNO获取设备数据（含有图形符号的设备）
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <returns></returns>
        public Dictionary<long, string> GetSymbolSBMCByFno(int g3e_fno)
        {
            var dic = new Dictionary<long, string>();
            try
            {
                var values = GetDBSymbolFinderByFnoOrEntityType(g3e_fno);

                foreach (var item in values)
                {
                    var com = DBManager.Instance.GetEntity<Common_n>(item.Value.G3E_FID);
                    if (com != null)
                    {
                        if (dic.Keys.Contains(item.Value.G3E_FID))
                        {
                            continue;
                        }
                        dic.Add(com.G3E_FID, com.GetValue<string>("SBMC"));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }

            return dic;
        }

        public Dictionary<long, string> GetAllSymbolSBMC()
        {
            var dic = new Dictionary<long, string>();
            try
            {
                var values = GetDevInLttID();
                foreach (var item in values)
                {
                    var com = DBManager.Instance.GetEntity<Common_n>(item.Value.G3E_FID);
                    if (com != null)
                    {
                        if (dic.Keys.Contains(item.Value.G3E_FID))
                        {
                            continue;
                        }
                        dic.Add(com.G3E_FID, com.GetValue<string>("SBMC"));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }

            return dic;
        }
        /// <summary>
        /// 根据FNO获取设备集合(只是获取没有图形符号的设备)
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <returns></returns>
        public Dictionary<long, string> GetSymbolByFno(int g3e_fno)
        {
            var dic = new Dictionary<long, string>();
            if (g3e_fno == 141)
            {
                var zxList =
                    DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(
                        o => o.G3E_FNO == 141 && !string.IsNullOrEmpty(o.GNWZ_XLFL) && o.GNWZ_XLFL.Equals("支线"));
                if (zxList != null && zxList.Any())
                {
                    zxList.ToList().ForEach(o=>dic.Add(o.G3E_FID,DBManager.Instance.GetEntity<Common_n>(o.G3E_FID)!=null?DBManager.Instance.GetEntity<Common_n>(o.G3E_FID).SBMC:""));
                }
            }
            else if (g3e_fno == 199)
            {
                var tjList = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FNO == 199);
                if (tjList != null && tjList.Any())
                {
                    tjList.ToList().ForEach(o=>dic.Add(o.G3E_FID,o.SBMC));
                }
                //除了列出台架还增加了非台架变压器
                var byqList = DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o => o.G3E_FNO == 148);
                if (byqList != null && byqList.Any())
                {
                    //var byqListFilter=byqList.Where(o => (string.IsNullOrEmpty(o.GNWZ_FL2))||(!string.IsNullOrEmpty(o.GNWZ_FL2) && !o.GNWZ_FL2.Equals("台架")));//存在没有安装位置的变压器
                    //if (byqListFilter != null && byqListFilter.Any())
                    //{
                    byqList.ToList()
                        .ForEach(
                            o =>
                                dic.Add(o.G3E_FID,
                                    DBManager.Instance.GetEntity<Common_n>(o.G3E_FID) != null
                                        ? (!string.IsNullOrEmpty(o.GNWZ_FL2)?(o.GNWZ_FL2 + "/"):"")+
                                           DBManager.Instance.GetEntity<Common_n>(o.G3E_FID).SBMC
                                        : (!string.IsNullOrEmpty(o.GNWZ_FL2) ? (o.GNWZ_FL2 + "/") : "")));
                    //}
                }
            }
            else if (g3e_fno == 148)
            {
                var byqList = DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o => o.G3E_FNO == 148);
                if (byqList != null && byqList.Any())
                {
                    //var byqLstFilters =
                    //    byqList.Where(
                    //        o =>
                    //            (string.IsNullOrEmpty(o.GNWZ_FL2)) ||
                    //            (!string.IsNullOrEmpty(o.GNWZ_FL2) && !o.GNWZ_FL2.Equals("台架")));
                    //if(byqLstFilters!=null&&byqLstFilters.Any())
                    byqList.ToList()
                        .ForEach(
                            o =>
                                dic.Add(o.G3E_FID,
                                    DBManager.Instance.GetEntity<Common_n>(o.G3E_FID) != null
                                        ? (!string.IsNullOrEmpty(o.GNWZ_FL2) ? (o.GNWZ_FL2 + "/") : "") +
                                          DBManager.Instance.GetEntity<Common_n>(o.G3E_FID).SBMC
                                        : (!string.IsNullOrEmpty(o.GNWZ_FL2) ? (o.GNWZ_FL2 + "/") : "")));
                }
                //除了变压器还增加了安装位置为台架的台架
                var tjByqList = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FNO == 199);
                if (tjByqList != null && tjByqList.Any())
                {
                    tjByqList.ToList().ForEach(o=>dic.Add(o.G3E_FID,o.SBMC));
                }

            }else if (g3e_fno == 198)
            {
                var kggList = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FNO == 198);
                if (kggList != null && kggList.Any())
                {
                    kggList.ToList().ForEach(o=>dic.Add(o.G3E_FID,o.SBMC));
                }
            }
            return dic;
        }

        public void GetSpecialSymbolData(int fno)
        {
            try
            {
                if (fno == 198)
                {
                    GetKGGData(fno);
                }
                else if (fno == 199)
                {
                    GetTJData(fno);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        private void GetKGGData(int g3e_fno)
        {
            var com = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FNO == g3e_fno);
            foreach (var item in com)
            {
                var pt = new Gg_pd_kgg_ar_sdogeom();
                pt.AddSibling(item);
                var gn = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(item.G3E_FID);
                if (gn != null)
                    pt.AddSibling(gn);
                var n = DBManager.Instance.GetEntity<Gg_pd_kgg_n>(item.G3E_FID);
                if (n != null)
                {
                    pt.AddSibling(n);
                }

                if (item.LTT_ID != null)
                {
                    var eo = new EntityObj
                    {
                        electronSymbol = pt,
                        G3e_Fno = g3e_fno,
                        SBMC = item.SBMC,
                        LTT_ID = (decimal)item.LTT_ID
                    };
                    if (!DBSymbolLTTIDFinder.Instance.ContainsKey(item.G3E_FID))
                    {
                        DBSymbolLTTIDFinder.Instance.Add(item.G3E_FID, eo);
                    }
                }
            }
        }

        private void GetTJData(int g3e_fno)
        {
            var com = DBManager.Instance.GetEntities<Common_n>(o => o.G3E_FNO == g3e_fno);
            foreach (var item in com)
            {
                var pt = new Gg_gz_tj_ar_sdogeom();
                pt.AddSibling(item);
                var gn = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(item.G3E_FID);
                if (gn != null)
                    pt.AddSibling(gn);
                var n = DBManager.Instance.GetEntity<Gg_gz_tj_n>(item.G3E_FID);
                if (n != null)
                {
                    pt.AddSibling(n);
                }

                if (item.LTT_ID != null)
                {
                    var eo = new EntityObj
                    {
                        electronSymbol = pt,
                        G3e_Fno = g3e_fno,
                        SBMC = item.SBMC,
                        LTT_ID = (decimal)item.LTT_ID
                    };
                    if (!DBSymbolLTTIDFinder.Instance.ContainsKey(item.G3E_FID))
                    {
                        DBSymbolLTTIDFinder.Instance.Add(item.G3E_FID, eo);
                    }
                }
            }
        }
        /// <summary>
        /// 通过集抄箱fid获取下面所有户表的数量
        /// </summary>
        /// <param name="fid">集抄箱fid</param>
        /// <returns>返回-1代表查找失败</returns>
        public int GetSHBCountFromJCX(long fid)
        {
            try
            {
                int count = 0;
                var t = DBManager.Instance.GetEntity<Detailreference_n>(fid);
                if (t == null) throw new Exception("Detailreference_n中找不到数据");
                var ents = GetDYSHB_PT(t.G3E_DETAILID);
                if (ents != null)
                {
                    foreach (long tmp in ents)
                    {
                        var ent = DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(tmp);
                        if (ent != null)
                        {
                            if (ent.EntityState != EntityState.Delete)
                                count++;
                        }
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
               PublicMethod.Instance.ShowMessage(String.Format("加载数据过程中出现错误:{0}", ex.Message));
                return -1;
            }
        }

        /// <summary>
        /// 根据Detailid获取户表所有数据
        /// </summary>
        /// <param name="g3e_detailid"></param>
        /// <returns></returns>
        public IEnumerable<long> GetDYSHB_PT(long? g3e_detailid)
        {
            try
            {
                return DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(o => o.G3E_DETAILID == g3e_detailid).Select(o => o.G3E_FID).ToList();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 查询包含
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public int GetContainCount(long g3e_fid)
        {
            try
            {
                var brotherValue = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == g3e_fid);
                return brotherValue.Count();
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 查询包含
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public IEnumerable<Contain_n> GetContains(long g3e_fid)
        {
            try
            {
                return DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_OWNERFID == g3e_fid);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取公共属性
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Common_n GetCommonByG3e_FID(long g3e_fid)
        {
            return DBManager.Instance.GetEntity<Common_n>(g3e_fid);
        }
        /// <summary>
        /// 获取从属
        /// </summary>
        /// <param name="g3e_id">G3e_ID</param>
        /// <returns></returns>
        public IEnumerable<Common_n> GetCommonsByG3e_ID(long g3e_id)
        {
            return DBManager.Instance.GetEntities<Common_n>(o => o.OWNER1_ID == g3e_id);
        }
       
        public IEnumerable<KeyValuePair<ObjectId, ElectronSymbol>> GetDevInLttID()
        {
            return DBSymbolFinder.Instance.Where(o =>(!o.Value.IsErased)
                &&(o.Value.EntityType ==EntityType.None) &&
                o.Value.GetValue("LTT_ID").ToString() == MapConfig.Instance.LTTID.ToString());
        }
        /// <summary>
        /// 获取在工单范围内的所有实体
        /// </summary>
        /// <returns></returns>
        public List<G3EObject> GetG3EObjectsInLttID()
        {
            List<G3EObject> objs=new List<G3EObject>();
            var comList = DBManager.Instance.GetEntities<Common_n>(o => o.LTT_ID == MapConfig.Instance.LTTID&&o.EntityState!= EntityState.Delete);
            if (comList != null && comList.Any())
            {
                comList.ToList()
                    .ForEach(o => objs.Add(new G3EObject {G3E_FNO = o.G3E_FNO, G3E_FID = o.G3E_FID, G3E_ID = o.G3E_ID}));
            }
            //获取没有公共属性表的设备：160（低压散户表）,159（集中超表箱）,188（负控）
            try
            {
                var shbList =
              DBManager.Instance.GetEntities<Gg_pd_dyshb_n>(o => o.LTT_ID == MapConfig.Instance.LTTID && o.EntityState != EntityState.Delete);
                if (shbList != null && shbList.Any())
                {
                    shbList.ToList().ForEach(o => objs.Add(new G3EObject { G3E_FNO = o.G3E_FNO, G3E_FID = o.G3E_FID, G3E_ID = o.G3E_ID }));
                }
            }
            catch
            {
            }
            try
            {
                var jcxList = DBManager.Instance.GetEntities<Gg_pd_cbx_n>(o => o.LTT_ID == MapConfig.Instance.LTTID && o.EntityState != EntityState.Delete);
                if (jcxList != null && jcxList.Any())
                {
                    jcxList.ToList().ForEach(o => objs.Add(new G3EObject { G3E_FNO = o.G3E_FNO, G3E_FID = o.G3E_FID, G3E_ID = o.G3E_ID }));
                }
            }
            catch
            {
            }

            try
            {
                var fkList = DBManager.Instance.GetEntities<Gg_jc_fk_n>(o => o.LTT_ID == MapConfig.Instance.LTTID && o.EntityState != EntityState.Delete);
                if (fkList != null && fkList.Any())
                {
                    fkList.ToList().ForEach(o => objs.Add(new G3EObject { G3E_FNO = o.G3E_FNO, G3E_FID = o.G3E_FID, G3E_ID = o.G3E_ID }));
                }

            }
            catch
            {
            }
          
            return objs;
        }
        /// <summary>
        /// 获取设备的组件表
        /// </summary>
        /// <param name="fno"></param>
        /// <returns></returns>
        public ComponentTable GetSelfTalbeNameByFno(int fno)
        {
            var component = new ComponentTable();
            var ent = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == fno);
            if (ent != null&&ent.ComponentTable!=null)
            {
               component = ent.ComponentTable;
            }
            return component;
        }
        /// <summary>
        /// 根据fno获取一张不为空的表名
        /// </summary>
        /// <param name="fno"></param>
        /// <returns></returns>
        public string GetIsNotNullTableName(int fno)
        {
            string tableName = string.Empty;
            var ent = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == fno);
            if (ent != null)
            {
                if (ent.ComponentTable != null)
                {
                    if (ent.ComponentTable.Common != null)
                    {
                        tableName = ent.ComponentTable.Common;
                    }else if (ent.ComponentTable.SelfAttribute != null)
                    {
                        tableName = ent.ComponentTable.SelfAttribute;
                    }

                }
            }
            return tableName;
        }
        #endregion

        #region 组件表查询
        /// <summary>
        /// 公共属性表
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Common_n GetCommon_n(long g3e_fid)
        {
            try
            {
                return DBManager.Instance.GetEntity<Common_n>(
                        o => o.G3E_FID == g3e_fid && o.EntityState != EntityState.Delete);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 公共属性表
        /// </summary>
        /// <param name="xmlDBManager"> </param>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Common_n GetCommon_n(XmlDBManager xmlDBManager,long g3e_fid)
        {
            try
            {
                return xmlDBManager.GetEntity<Common_n>(
                        o => o.G3E_FID == g3e_fid && o.EntityState != EntityState.Delete);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 连接关系表
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Connectivity_n GetConnectivity_n(long g3e_fid)
        {
            try
            {
                Connectivity_n conn=null;
                var conns=DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.G3E_FID == g3e_fid && o.EntityState != EntityState.Delete);
                if(conns!=null && conns.Any())
                {
                    foreach(var con in conns)
                    {
                        if(!con.Duplicated)
                            conn=con;
                    }
                }
                return conn;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 连接关系表
        /// </summary>
        /// <param name="xmlDBManager"> </param>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Connectivity_n GetConnectivity_n(XmlDBManager xmlDBManager, long g3e_fid)
        {
            try
            {
                Connectivity_n conn = null;
                var conns = xmlDBManager.GetEntities<Connectivity_n>(
                        o => o.G3E_FID == g3e_fid && o.EntityState != EntityState.Delete);
                if (conns != null && conns.Any())
                {
                    foreach (var con in conns)
                    {
                        if (!con.Duplicated)
                            conn = con;
                    }
                }
                return conn;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 功能位置
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Gg_pd_gnwzmc_n GetGnwzmc_n(long g3e_fid)
        {
            try
            {
                return DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(
                        o => o.G3E_FID == g3e_fid && o.EntityState != EntityState.Delete);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 功能位置
        /// </summary>
        /// <param name="xmlDBManager"> </param>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Gg_pd_gnwzmc_n GetGnwzmc_n(XmlDBManager xmlDBManager, long g3e_fid)
        {
            try
            {
                return xmlDBManager.GetEntity<Gg_pd_gnwzmc_n>(
                        o => o.G3E_FID == g3e_fid && o.EntityState != EntityState.Delete);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 样式查询
        /// <summary>
        /// 获取点符号样式
        /// </summary>
        /// <param name="color"></param>
        /// <param name="g3e_username"></param>
        /// <returns></returns>
        public CADColor GetPointStyle(CADColor color, string g3e_username)
        {
            try
            {
                //查找符号样式
                var style = CDDBManager.Instance.GetEntity<G3e_pointstyle>(o => o.G3E_USERNAME == g3e_username);
                if (style == null) return color;
                //匹配的颜色
                if (style.G3E_COLOR != null)
                    color = CADColor.FromColor(Color.FromArgb((int)style.G3E_COLOR));
            }
            catch { }
            return color;
        }

        /// <summary>
        /// 获取线符号样式
        /// </summary>
        /// <param name="sea"></param>
        /// <param name="g3e_username"></param>
        /// <returns></returns>
        public InsertSymbolEventArgs GetLineStyle(InsertSymbolEventArgs sea, string g3e_username)
        {
            try
            {
                //查找符号样式
                var style = CDDBManager.Instance.GetEntity<G3e_linestyle>(o => o.G3E_USERNAME == g3e_username);
                if (style == null) return sea;

                if (style.G3E_WIDTH != null) sea.lineWidth = (double)style.G3E_WIDTH;
                //匹配的颜色
                if (style.G3E_COLOR != null)
                    sea.symbolColor = CADColor.FromColor(Color.FromArgb((int)style.G3E_COLOR));
            }
            catch { }
            return sea;
        }

        /// <summary>
        /// 获取线符号样式
        /// </summary>
        /// <param name="sea"></param>
        /// <param name="g3e_username"></param>
        /// <returns></returns>
        public SymbolEventArgs GetLineStyle(SymbolEventArgs sea, string g3e_username)
        {
            try
            {
                //查找符号样式
                var style = CDDBManager.Instance.GetEntity<G3e_linestyle>(o => o.G3E_USERNAME == g3e_username);
                if (style == null) return sea;

                if (style.G3E_WIDTH != null) sea.lineWidth = (double)style.G3E_WIDTH;
                //匹配的颜色
                if (style.G3E_COLOR != null)
                    sea.color = CADColor.FromColor(Color.FromArgb((int)style.G3E_COLOR));
            }
            catch { }
            return sea;
        }

        /// <summary>
        /// 获取面符号样式
        /// </summary>
        /// <param name="color"></param>
        /// <param name="g3e_username"></param>
        /// <returns></returns>
        public CADColor GetAreaStyle(CADColor color, string g3e_username)
        {
            try
            {
                var style = CDDBManager.Instance.GetEntity<G3e_areastyle>(o => o.G3E_USERNAME == g3e_username);
                if (style == null) return color;
                if (style.G3E_FORECOLOR != null)
                    color = CADColor.FromColor(Color.FromArgb((int)style.G3E_FORECOLOR));
            }
            catch { }
            return color;
        }


        /// <summary>
        /// 获取点符号样式（符合符号名称-生命周期的情况）
        /// </summary>
        /// <param name="g3e_username"></param>
        /// <param name="com"></param>
        /// <param name="color"> </param>
        /// <returns></returns>
        public CADColor GetPointStyle(string g3e_username, Common_n com, CADColor color)
        {
            g3e_username = GetStyleUserName(com, g3e_username);
            return GetPointStyle(color, g3e_username);
        }

        public SymbolEventArgs GetLineStyle(string g3e_username, Common_n com, SymbolEventArgs sea)
        {
            g3e_username = GetStyleUserName(com, g3e_username);
            return GetLineStyle(sea, g3e_username);
        }

        public CADColor GetAreaStyle(string g3e_username, Common_n com, CADColor color)
        {
            g3e_username = GetStyleUserName(com, g3e_username);
            return GetAreaStyle(color, g3e_username);
        }

        /// <summary>
        /// 根据生命周期和是否带电获取样式
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="smzq"></param>
        /// <param name="sfdd"></param>
        /// <returns></returns>
        public string GetUserNameBySMZQorSFDD(string userName, string smzq, string sfdd)
        {
            if (!string.IsNullOrEmpty(smzq))
            {
                //停运和不带电情况
                if (!string.IsNullOrEmpty(sfdd))
                {
                    if (sfdd.Contains("不带电") || smzq.Equals("停运"))
                    {
                        userName = string.Format("{0}-停运", userName);
                    }
                    else
                        userName = string.Format("{0}-{1}", userName, smzq);
                }else
                {
                    userName = string.Format("{0}-{1}",userName,smzq);
                }
            }
            return userName;
        }


        private string GetStyleUserName(Common_n com, string g3e_username)
        {
            if (!string.IsNullOrEmpty(com.CD_SMZQ))
            {
                var entry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == com.G3E_FNO);
                Connectivity_n con = null;
                if (!string.IsNullOrEmpty(entry.ComponentTable.Connectivity))
                    con = GetConnectivity_n(com.G3E_FID);
                if (con == null)
                {
                    //停运或不带电颜色一样
                    if (com.CD_SMZQ.Contains("停运"))
                    {
                        g3e_username = string.Format("{0}-停运", g3e_username);
                    }
                    else
                        g3e_username = string.Format("{0}-{1}", g3e_username, com.CD_SMZQ);
                }
                else
                {
                    //停运或不带电颜色一样
                    if (com.CD_SMZQ.Equals("停运") || con.CD_SFDD.Equals("不带电"))
                    {
                        g3e_username = string.Format("{0}-停运", g3e_username);
                    }else if(com.CD_SMZQ.Equals("投运") && con.CD_SFDD.Equals("带电"))
                    {
                        g3e_username = string.Format("{0}-投运", g3e_username);
                    }
                    else
                        g3e_username = string.Format("{0}-{1}", g3e_username, com.CD_SMZQ);
                }
            }
            return g3e_username;
        }
        #endregion


        #region 工单定位

        public long GetLttID()
        {
            return MapConfig.Instance.LTTID;
        }
        /// <summary>
        /// 获取全景视图
        /// </summary>
        public void GetPanoramaView()
        {
            try
            {
                //获取当前工单的所有对象,过滤掉没有符号的两个设备
                var currentLTTIDs = DBSymbolLTTIDFinder.Instance.DBSymbols.Where(o=>o.objectType);
                if (currentLTTIDs.Any())
                {
                    const double scope = 0.00001583;
                    //计算图框坐标
                    var minX = currentLTTIDs.Min(o => o.minPoint.X) - scope;
                    var maxX = currentLTTIDs.Max(o => o.maxPoint.X) + scope;
                    var minY = currentLTTIDs.Min(o => o.minPoint.Y) - scope;
                    var maxY = currentLTTIDs.Max(o => o.maxPoint.Y) + scope;


                    PublicMethod.Instance.Zoom(maxX, minX, maxY, minY);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 获取工单视图
        /// </summary>
        /// <returns></returns>
        public double[] GetWorkorderView()
        {
            var pointions=new double[4];
            try
            {
                //获取当前工单的所有对象,过滤掉没有符号的两个设备
                var currentLTTIDs = DBSymbolLTTIDFinder.Instance.Where(o => o.Value.LTT_ID == MapConfig.Instance.LTTID && o.Value.objectType).ToList();
                if (currentLTTIDs.Count > 0)
                {
                    const double scope = 0.00001583;
                    //计算图框坐标
                    pointions[0] = currentLTTIDs.Min(o => o.Value.minPoint.X) - scope;
                    pointions[1] = currentLTTIDs.Max(o => o.Value.maxPoint.X) + scope;
                    pointions[2] = currentLTTIDs.Min(o => o.Value.minPoint.Y) - scope;
                    pointions[3] = currentLTTIDs.Max(o => o.Value.maxPoint.Y) + scope;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return pointions;
        }
        #endregion

       
        #region 工单校验查询
        /// <summary>
        /// 校验工单
        /// </summary>
        /// <param name="objectIds"></param>
        /// <param name="shb"> </param>
        /// <returns>true为被锁定的设备，false为未被锁定的设备</returns>
        public bool VerifyLTTID(IEnumerable<ObjectId> objectIds, ref Dictionary<string, int> shb)
        {
            var result = true;
            G3EObject g3eObject = null;
            foreach (var objectId in objectIds)
            {
                if (GetG3EIds(objectId, ref g3eObject))
                {
                    var value = DBSymbolFinder.Instance[objectId];
                    if (g3eObject.G3E_FNO == 159)
                    {
                        var cbx = DBManager.Instance.GetEntity<Gg_pd_cbx_n>(g3eObject.G3E_FID);
                        var shbCount = GetSHBCountFromJCX(g3eObject.G3E_FID);
                        var hh = string.Format("{0}{1}", cbx.HH, g3eObject.G3E_FID);
                        if (!shb.ContainsKey(hh))
                            shb.Add(hh, shbCount);
                    }
                    if (value != null)
                    {
                        if (value.GetValue<decimal>("LTT_ID") != MapConfig.Instance.LTTID)
                        {
                            result = false;
                            break;
                        }
                    }
                }
                else
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 校验工单
        /// </summary>
        /// <param name="objectIds"></param>
        /// <returns>返回true表示有未被锁定的设备</returns>
        public bool VerifyLTTID(ObjectId[] objectIds)
        {
            return (from objectId in objectIds where HasG3EIDS(objectId) 
                    select DBSymbolFinder.Instance[objectId] 
                    into value 
                    where value != null select value).Any(value => value.GetValue<decimal>("LTT_ID") != MapConfig.Instance.LTTID);
        }

        /// <summary>
        /// 校验工单
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns>true为被锁定的设备，false为未被锁定的设备</returns>
        public bool VerifyLTTID(ObjectId objectId)
        {
            var result = false;
            if (HasG3EIDS(objectId))
            {
                var value = DBSymbolFinder.Instance[objectId];
                if (value != null)
                {
                    if (value.GetValue<decimal>("LTT_ID") == MapConfig.Instance.LTTID)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 校验工单
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns>true为被锁定的设备，false为未被锁定的设备</returns>
        public bool VerifyLTTID(long g3e_fid)
        {
            var objid = GetObjectIdByFid(g3e_fid);
            return VerifyLTTID(objid);
        }

        public bool VerifyLTTID(decimal LTT_ID)
        {
            if (LTT_ID == MapConfig.Instance.LTTID)
                return true;
            return false;
        }
        #endregion

        #region 配置表数据查询
        /// <summary>
        /// 获取符号配置文件
        /// </summary>
        /// <param name="g3e_fno">FNO</param>
        /// <returns></returns>
        public DBSymbolEntry GetSymbolConfigByG3eFNO(long g3e_fno)
        {
            return SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == g3e_fno);
        }
       

        #endregion

        #region 记忆功能
        public XProps GetMemoryDevice(int g3e_fno,XProps newObj)
        {
            //设置记忆值
            if (MemoryDevice.Any())
            {
                if (MemoryDevice.Keys.Contains(g3e_fno))
                {
                    XProps oldObj = MemoryDevice[g3e_fno];
                    var ssdw = GenerateHelper.GetPropertyValue(oldObj, GetIsNotNullTableName(g3e_fno), "CD_SSDW");
                    var ssxl = GenerateHelper.GetPropertyValue(oldObj, GetIsNotNullTableName(g3e_fno), "CD_SSXL");
                    if (ssdw == null || ssxl == null || string.IsNullOrEmpty(ssdw.ToString()) ||
                        string.IsNullOrEmpty(ssxl.ToString()))
                    {
                        SetValueToNewXprops(g3e_fno,newObj);
                    }
                    else
                    {
                        CopyValueFromOldDev(newObj, oldObj);
                    }
                }
                else
                {
                    SetValueToNewXprops(g3e_fno,newObj);
                   
                }
            }
            else
            {
                MemoryDevice.Add(g3e_fno, newObj);
            }
            return newObj;
        }

        public void SetValueToNewXprops(int g3e_fno,XProps newObj)
        {
            var xpropsList = MemoryDevice.Values;
            foreach (var item in xpropsList)
            {
                var ssdw = GenerateHelper.GetPropertyValue(item, "Common_n", "CD_SSDW");
                var ssxl = GenerateHelper.GetPropertyValue(item, "Common_n", "CD_SSXL");
                if (ssdw != null && ssxl != null)
                {
                    if (!string.IsNullOrEmpty(ssdw.ToString()) && !string.IsNullOrEmpty(ssxl.ToString()))
                    {
                        SetNewObjValue(g3e_fno, newObj, item);
                    }
                }
            }
        }
        /// <summary>
        /// 拷贝记忆值
        /// </summary>
        /// <param name="newObj"></param>
        /// <param name="oldObj"></param>
        public void CopyValueFromOldDev(XProps newObj, XProps oldObj)
        {
            foreach (var item in oldObj)
            {
                var newObjReadOnly = GenerateHelper.GetSingleXProp(newObj, item.Category, item.Name).ReadOnly;
                if (!newObjReadOnly)//如果是需要同步的字段不需要拷贝原来的值
                {
                    var value = GenerateHelper.GetPropertyValue(oldObj,item.Category, item.Name);
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                        GenerateHelper.SetPropertyValue(newObj,item.Category, item.Name, value);
                }
            }
            EmptyDeviceAttribute(newObj);
        }

        private void SetNewObjValue(int fno, XProps newObj, XProps oldObj)
        {
            if (newObj == null || oldObj == null) return;
            List<string> fields = new List<string> {"CD_SSDW", "CD_SSBDZ", "CD_SSXL", "WHBS", "GNWZ_SSGDS"};
            var component = GetSelfTalbeNameByFno(fno);
            foreach (var item in oldObj)
            {
                if (fields.Contains(item.Name))
                {
                    var res = GenerateHelper.GetPropertyValue(oldObj, item.Category, item.Name);
                    GenerateHelper.SetPropertyValue(newObj, !string.IsNullOrEmpty(component.Common)?component.Common:component.SelfAttribute, item.Name, res);
                }
            }
        }


        public void EmptyDeviceAttribute(XProps prevDev)
        {
            EmptySpecialFieldValue(prevDev);
            var res = GenerateHelper.GetXPropList(prevDev, "SBMC");
            if (res.Any())
            {
                foreach (var item in res)
                {
                    GenerateHelper.SetPropertyValue(prevDev,item.Key, item.Value, null);
                }
            }
        }

        public void EmptySpecialFieldValue(XProps xprops)
        {
            foreach (var item in xprops)
            {
                if (item.SaveValueByFid)
                {
                    //if (item.Name.Equals("GNWZ_SSGT"))//考虑到同杆架设的情况
                    //{
                    //    continue;
                    //}
                    GenerateHelper.SetPropertyValue(xprops,item.Category, item.Name, null);
                }
            }
        }
        #endregion


        #region 馈线查询
        /// <summary>
        /// 馈线查询
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Cd_ssxl> GetSDKXXG()
        {
            try
            {
                return CDDBManager.Instance.GetEntities<Cd_ssxl>();
            }
            catch
            {
                PublicMethod.Instance.ShowMessage("Cd_ssxl表缺失数据！");
                return null;
            }
        }
        /// <summary>
        /// 根据供电局查询
        /// </summary>
        /// <param name="gdj">供电局</param>
        /// <returns></returns>
        public IEnumerable<Cd_ssxl> GetSDKXXGByGDJ(string gdj)
        {
            try
            {
                return CDDBManager.Instance.GetEntities<Cd_ssxl>(o => o.CD_SSDW == gdj);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取受电馈线
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public int GetSdkxCount(long g3e_fid)
        {
            try
            {
                var values = DBManager.Instance.GetEntities<Gg_pd_sdkx_ac>(o => o.G3E_FID == g3e_fid);
                return values.Count();
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 获取受电馈线
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public IEnumerable<Gg_pd_sdkx_ac> GetSdkxAcByG3e_FID(long g3e_fid)
        {
            try
            {
                return DBManager.Instance.GetEntities<Gg_pd_sdkx_ac>(o => o.G3E_FID == g3e_fid);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取受电馈线
        /// </summary>
        /// <param name="g3e_id"></param>
        /// <returns></returns>
        public Gg_pd_sdkx_ac GetSdkxAcByG3e_ID(long g3e_id)
        {
            try
            {
                return DBManager.Instance.GetEntity<Gg_pd_sdkx_ac>(o => o.G3E_ID == g3e_id);
            }
            catch
            {
                return null;
            }
        }

        public IList<string> GetSDKXList()
        {
            IList<string> coms = new List<string>();
            //var commons = DBManager.Instance.GetEntities<Common_n>(o => o.EntityState == EntityState.None);
            //if (commons != null && commons.Any())
            //{
            //    coms = commons.Where(o => !string.IsNullOrEmpty(o.CD_SSXL)).Select(o => o.CD_SSXL).Distinct();
            //}
            string kxList = MapConfig.Instance.KXMC;
            if (kxList.Contains(','))
            {
                coms = kxList.Split(',').ToList();
            }
            else
                coms.Add(kxList);
            return coms;
        }

        public IList<string> GetGDJListByKx(IList<string> kxList)
        {
            IList<string> gdjList = new List<string>();
            foreach (var kx in kxList)
            {
                var gdj =
                    CDDBManager.Instance.GetEntity<Cd_ssxl>(o => !string.IsNullOrEmpty(o.NAME) && o.NAME.Equals(kx));
                if (gdj != null)
                    gdjList.Add(gdj.CD_SSDW);
            }
            return gdjList;
        }
        /// <summary>
        /// 判断变电站是否存在馈线
        /// </summary>
        /// <param name="bdz">变电站</param>
        /// <param name="kx">馈线</param>
        /// <returns>true存在，反之</returns>
        public bool HasSSKX(string bdz, string kx)
        {
            try
            {
                var ssxl = CDDBManager.Instance.GetEntity<Cd_ssxl>(o => o.SSBDZ == bdz && o.NAME == kx);
                if (ssxl != null)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 获取受电馈线
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Gg_pd_sdkx_ac> GetSDKXByG3e_FIDS(IEnumerable<long> fids)
        {
            var pdSdkxCol = new List<Gg_pd_sdkx_ac>();
            foreach (var fid in fids)
            {
                var sdkxs = GetSdkxAcByG3e_FID(fid);
                if (sdkxs!=null && sdkxs.Any())
                    pdSdkxCol.AddRange(sdkxs);
            }
            return pdSdkxCol;
        } 
        public bool HasSDKX(long g3e_fid, string bdz,string kx)
        {
            try
            {
                var value=DBManager.Instance.GetEntities<Gg_pd_sdkx_ac>(o => o.G3E_FID == g3e_fid && o.GDBDZ == bdz && o.KXH == kx);
                return value.Any();
            }
            catch
            {
                return false;
            }
        }
        public bool DuplicatedSdkx(long g3e_fid, string bdz, string kx)
        {
            try
            {
                var value = DBManager.Instance.GetEntities<Gg_pd_sdkx_ac>(o => o.G3E_FID == g3e_fid && o.GDBDZ == bdz && o.KXH == kx);
                return value.Count()>1;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 杂项标注查询
        /// <summary>
        /// 获取标注类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Cd_bzlx> GetBZLX()
        {
            return CDDBManager.Instance.GetEntities<Cd_bzlx>();
        }
        /// <summary>
        /// 判断杂项标注是否存在
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public bool HasZxbz(string g3e_fid)
        {
            try
            {
                //var zxbzn = DBManager.Instance.GetEntity<Gg_gl_zxbz_n>(o => o.BZ_DYSB == g3e_fid);
                //return zxbzn != null;
            }catch(Exception ex)
            {
                if (!ex.Message.Contains("不存在"))
                    LogManager.Instance.Error(ex);
                return false;
            } return false;
        }
        /// <summary>
        /// 根据设备的g3e_fid获取对应的杂项标注
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Gg_gl_zxbz_n GetZxbzByG3e_Fid(string g3e_fid)
        {
            try
            {
                //return DBManager.Instance.GetEntity<Gg_gl_zxbz_n>(o => o.BZ_DYSB == g3e_fid);
            }catch(Exception ex)
            {
                if (!ex.Message.Contains("不存在"))
                    LogManager.Instance.Error(ex);
                return null;
            } return null;
        }
        /// <summary>
        /// 根据设备的g3e_fid获取对应的杂项标注
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Gg_gl_zxbz_n GetZxbzByG3e_Fid(long g3e_fid)
        {
            try
            {
                return DBManager.Instance.GetEntity<Gg_gl_zxbz_n>(o => o.G3E_FID == g3e_fid);
            }
            catch(Exception ex)
            {
                if (!ex.Message.Contains("不存在"))
                    LogManager.Instance.Error(ex);
                return null;
            }
        }
        /// <summary>
        /// 根据设备的g3e_fid获取对应的杂项标注
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public IEnumerable<Gg_gl_zxbz_n> GetZxbzsByG3e_Fid(string g3e_fid)
        {
            try
            {
                //return DBManager.Instance.GetEntities<Gg_gl_zxbz_n>(o => o.BZ_DYSB == g3e_fid);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("不存在"))
                    LogManager.Instance.Error(ex);
                return null;
            } return null;
        }

        /// <summary>
        /// 获取杂项标注坐标表
        /// </summary>
        /// <param name="xmlDBManager"> </param>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public Gg_gl_zxbz_lb_sdogeom GetZxbzGeomByG3e_fid(XmlDBManager xmlDBManager,long g3e_fid)
        {
            return xmlDBManager.GetEntity<Gg_gl_zxbz_lb_sdogeom>(g3e_fid);
        }
        /// <summary>
        /// 获取没有
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Gg_gl_zxbz_n> GetNotRelationZxbzN(XmlDBManager xmlDBManager)
        {
            return xmlDBManager.GetEntities<Gg_gl_zxbz_n>();
        }

        /// <summary>
        /// 根据坐标表查询lb表
        /// </summary>
        /// <param name="es"></param>
        /// <returns></returns>
        public DBEntity GetLabel_LB(ElectronSymbol es)
        {
            DBEntity eb = null;
            try
            {
                var className = es.GetType().Name.Replace("_sdogeom", "");
                var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), className);
                eb = DBManager.Instance.GetEntity(type, es.G3E_FID);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return eb;
        }

        #endregion

        #region 标注查询

        /// <summary>
        /// 查询标注状态
        /// </summary>
        /// <param name="G3e_fno"></param>
        /// <param name="G3e_fid"></param>
        /// <returns>true已添加，反之</returns>
        public bool GetLabelStatus(long G3e_fno, long G3e_fid)
        {
            var values = DBSymbolFinder.Instance.Where(o => o.Value.G3E_FID == G3e_fid
                && o.Value.FinderFuncNumber == G3e_fno
                && o.Value.EntityType == EntityType.Label
                && !o.Value.IsErased).ToList();
            return values.Count > 0;
        }
        
        /// <summary>
        /// 获取当前CAD中所有设备FNO和类型的组合
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllFeatureType()
        {
            var entities = DeviceAttributeConfig.Instance.Attributes;
            return entities.ToDictionary(simpleAttrEntry => simpleAttrEntry.Fno, simpleAttrEntry => simpleAttrEntry.LayerName);
        }
        /// <summary>
        /// 获取标注对齐方式
        /// </summary>
        /// <param name="esymlb"></param>
        /// <returns></returns>
        public int GetG3eAlignment(DBEntity esymlb)
        {
            var alignment = 5;
            try
            {
                if (esymlb != null)
                {
                    alignment = esymlb.GetValue<int>("G3E_ALIGNMENT");
                }
            }
            catch
            {
                var g3e_fid = esymlb == null ? "" : esymlb.GetValue("G3E_FID");
                LogManager.Instance.Error(string.Format("\n属性<G3E_ALIGNMENT>错误： {0}",g3e_fid ));
            }
            return alignment;
        }

        /// <summary>
        /// 查询已删除的数据
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="className"></param>
        /// <param name="entityState"> </param>
        /// <returns></returns>
        public DBEntity GetDBEntityByG3efidOrClassName(long g3e_fid, string className,EntityState entityState)
        {
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), className);
                return DBManager.Instance.GetEntity(type, o => o.EntityState == entityState);
            }catch(Exception exception)
            {
                if (!exception.Message.Contains("不存在"))
                    LogManager.Instance.Error(exception);
                return null;
            }
        }
        public KeyValuePair<ObjectId,ElectronSymbol> GetEraseStateLabel(long g3e_fid,int labelFlagFno)
        {
            return DBSymbolFinder.Instance.SingleOrDefault(o => o.Value.G3E_FID == g3e_fid && o.Value.EntityType==EntityType.Label && o.Value.IsErased && o.Value.FinderFuncNumber == labelFlagFno);
        }
        #endregion

        #region 支线查询
        public Gnwz_tz_zx GetGnwzTzZx(long g3e_fid)
        {
            try
            {
                return DBManager.Instance.GetEntity<Gnwz_tz_zx>(o => o.G3E_FID == g3e_fid && o.EntityState != EntityState.Delete);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 从属关系查询
        /// <summary>
        /// 查询从属关系
        /// </summary>
        /// <param name="g3e_id"></param>
        /// <returns></returns>
        public IEnumerable<Common_n> GetSymbolSubordinate(long g3e_id)
        {
            try
            {
                return DBManager.Instance.GetEntities<Common_n>(o => o.OWNER1_ID == g3e_id);
            }catch(Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
            return null;
        }
        #endregion

        #region 台架数据查询
        /// <summary>
        /// 获取设备的所属台架的公共属性数据
        /// </summary>
        /// <param name="sstj"></param>
        /// <returns></returns>
        public IEnumerable<Gg_pd_gnwzmc_n> GetSymbolSSTJ(string sstj)
        {
            try
            {
                return DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(
                        o => o.GNWZ_SSTJ == sstj);
            }catch(Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
            return null;
        }
        #endregion

        #region 所属开关柜查询
        /// <summary>
        /// 获取所属开关柜
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public IEnumerable<Gg_pd_gnwzmc_n> GetSSKGG (long g3e_fid)
        {
            try
            {
                return DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o => o.GNWZ_SSKGG == g3e_fid.ToString());
            }catch(Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
            return null;
        }
        #endregion

        #region 型号规格查询
        public Cd_xhge GetCDXHGE(int g3e_fno,string xhge)
        {
            return CDDBManager.Instance.GetEntity<Cd_xhge>(o => o.G3E_FNO == g3e_fno && o.NAME == xhge);
        }
        #endregion
    }
    public class G3EObject
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public int G3E_FNO { set; get; }
        /// <summary>
        /// 表序列
        /// </summary>
        public long G3E_ID { set; get; }
        /// <summary>
        /// 各表关联ID
        /// </summary>
        public long G3E_FID { set; get; }
    }
}
