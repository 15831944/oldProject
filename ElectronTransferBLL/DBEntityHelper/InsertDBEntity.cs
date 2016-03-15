using System;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;

namespace ElectronTransferBll.DBEntityHelper
{
    /// <summary>
    /// 添加实体
    /// </summary>
    public class InsertDBEntity
    {
        /// <summary>
        /// 新增受电馈线
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="gdbdz"></param>
        /// <param name="kxh"></param>
        /// <returns>返回G3e_id</returns>
        public static long InsertSDKX(Common_n dbEntity,string gdbdz,string kxh)
        {
            try
            {
                var cid = DBEntityFinder.Instance.GetSdkxCount(dbEntity.G3E_FID);
                cid++;
                var g3eid = CYZCommonFunc.getid();
                var newEnt = new Gg_pd_sdkx_ac
                                 {
                                     G3E_ID = g3eid,
                                     G3E_FID = dbEntity.G3E_FID,
                                     G3E_FNO = dbEntity.G3E_FNO,
                                     G3E_CID = cid,
                                     G3E_CNO = 14904,
                                     LTT_DATE = dbEntity.LTT_DATE,
                                     LTT_ID = MapConfig.Instance.LTTID,
                                     LTT_STATUS = dbEntity.LTT_STATUS,
                                     LTT_TID = dbEntity.LTT_TID,
                                     SCM_DID = dbEntity.SCM_DID,
                                     GDBDZ = gdbdz,
                                     KXH = kxh,
                                     EntityState = EntityState.Insert
                                 };

                DBManager.Instance.Insert(newEnt);
                return newEnt.G3E_ID;
            }catch(Exception ex )
            {
                LogManager.Instance.Error(ex);
            }
            return 0;
        }
        /// <summary>
        /// 新增受电馈线
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns>返回G3e_id</returns>
        public static long InsertSDKX(Common_n dbEntity)
        {
            try
            {
                var cid = DBEntityFinder.Instance.GetSdkxCount(dbEntity.G3E_FID);
                cid++;
                var g3eid = CYZCommonFunc.getid();
                var newEnt = new Gg_pd_sdkx_ac
                                 {
                                     G3E_ID = g3eid,
                                     G3E_FID = dbEntity.G3E_FID,
                                     G3E_FNO = dbEntity.G3E_FNO,
                                     G3E_CID = cid,
                                     G3E_CNO = 14904,
                                     LTT_DATE = dbEntity.LTT_DATE,
                                     LTT_ID = MapConfig.Instance.LTTID,
                                     LTT_STATUS = dbEntity.LTT_STATUS,
                                     LTT_TID = dbEntity.LTT_TID,
                                     SCM_DID = dbEntity.SCM_DID,
                                     GDBDZ = dbEntity.CD_SSBDZ,
                                     KXH = dbEntity.CD_SSXL,
                                     EntityState = EntityState.Insert
                                 };

                DBManager.Instance.Insert(newEnt);
                return newEnt.G3E_ID;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return 0;
        }

        /// <summary>
        /// 添加包含关系
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="dgG3e_fid"></param>
        /// <param name="dxPt"></param>
        public static int AddContain(int cid, long dgG3e_fid, ElectronSymbol dxPt)
        {
            try
            {
                cid++;
                var g3e_id = CYZCommonFunc.getid();
                var contain = new Contain_n
                                  {
                                      G3E_CID = cid,
                                      G3E_FNO = dxPt.G3E_FNO,
                                      G3E_FID = dxPt.G3E_FID,
                                      G3E_CNO = 38,
                                      G3E_ID = g3e_id,
                                      G3E_OWNERFID = dgG3e_fid,
                                      G3E_OWNERFNO = 201,
                                      EntityState = EntityState.Insert,
                                      LTT_ID = MapConfig.Instance.LTTID
                                  };
                DBManager.Instance.Insert(contain);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return cid;
        }

        #region 新增杂项标注
        /// <summary>
        /// 新增杂项标注
        /// </summary>
        /// <param name="parentfid">关联设备G3e_fid</param>
        /// <param name="text">标注内容</param>
        /// <param name="bzlx">标注类型</param>
        /// <param name="ssdw">所属单位</param>
        /// <param name="mpt">标注坐标</param>
        /// <param name="pt">返回标注坐标表对象</param>
        /// <returns>true成功，反之</returns>
        public static bool InsertZXBZ(long parentfid,string text, string bzlx, string ssdw,Multipoint mpt,ref ElectronSymbol pt)
        {
            try
            {
                var g3e_id = CYZCommonFunc.getid();
                var g3e_fid = CYZCommonFunc.getid();
                InsertZXBZN(g3e_id, g3e_fid, parentfid, text, bzlx, ssdw);
                InsertZXBZLB(g3e_id, g3e_fid);
                pt=InsertZXBZLBSDOGEOM(g3e_id, g3e_fid,mpt);
                return true;
            }catch(Exception ex)
            {
                LogManager.Instance.Error("新增杂项标注错误！" + ex);
            }
            return false;
        }
        /// <summary>
        /// 新增杂项标注自身属性表
        /// </summary>
        /// <param name="g3e_id"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="parentfid">关联设备G3e_fid</param>
        /// <param name="text">标注内容</param>
        /// <param name="bzlx">标注类型</param>
        /// <param name="ssdw">所属单位</param>
        private static void InsertZXBZN(long g3e_id, long g3e_fid, long parentfid, string text, string bzlx, string ssdw)
        {
            var zxbzn = new Gg_gl_zxbz_n
                            {
                                G3E_ID = g3e_id,
                                G3E_FID = g3e_fid,
                                G3E_CNO = 25001,
                                G3E_FNO = 250,
                                G3E_CID = 1,
                                MIF_TEXT = text,
                                CD_BZLX = bzlx,
                                CD_SSDW = ssdw,
                                //BZ_DYSB = parentfid.ToString(),
                                EntityState = EntityState.Insert,
                                LTT_ID = MapConfig.Instance.LTTID
                            };
            DBManager.Instance.Insert(zxbzn);
        }
        /// <summary>
        /// 杂项标注表
        /// </summary>
        /// <param name="g3e_id"></param>
        /// <param name="g3e_fid"></param>
        private static void InsertZXBZLB(long g3e_id, long g3e_fid)
        {
            var zxbzlb = new Gg_gl_zxbz_lb
                             {
                                 G3E_ID = g3e_id,
                                 G3E_FID = g3e_fid,
                                 G3E_CID = 1,
                                 G3E_CNO = 25002,
                                 G3E_FNO = 250,
                                 G3E_ALIGNMENT = 9,
                                 EntityState = EntityState.Insert,
                                 LTT_ID = MapConfig.Instance.LTTID
                             };
            DBManager.Instance.Insert(zxbzlb);
        }
        /// <summary>
        /// 杂项标注坐标表
        /// </summary>
        /// <param name="g3e_id"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="mpt"></param>
        /// <returns></returns>
        private static ElectronSymbol InsertZXBZLBSDOGEOM(long g3e_id, long g3e_fid,Geometry mpt)
        {
            var zxbzsdogeom = new Gg_gl_zxbz_lb_sdogeom
                                  {
                                      G3E_CID = 1,
                                      G3E_CNO = 25002,
                                      G3E_ID = g3e_id,
                                      G3E_FID = g3e_fid,
                                      G3E_FNO = 250,
                                      G3E_GEOMETRY = mpt,
                                      EntityType = EntityType.ZxLabel,
                                      EntityState = EntityState.Insert,
                                      LTT_ID = MapConfig.Instance.LTTID
                                  };
            DBManager.Instance.Insert(zxbzsdogeom);
            return zxbzsdogeom;
        }
        #endregion

        #region 新增标注
        public static void AddLabelLb(G3EObject g3eObject, string className,long g3e_cno)
        {
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), className);
                var instance = ReflectionUtils.CreateObject(
                    new
                        {
                            G3E_CID = 1,
                            G3E_ID = g3eObject.G3E_ID,
                            G3E_FID = g3eObject.G3E_FID,
                            G3E_FNO = g3eObject.G3E_FNO,
                            G3E_CNO = g3e_cno,
                            LTT_ID = Convert.ToDecimal(MapConfig.Instance.LTTID),
                            EntityState = EntityState.Insert
                        }, type) as ElectronSymbol;

                DBManager.Instance.Insert(instance);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        public static void AddLabelLn(G3EObject g3eObject, string className, long g3e_cno)
        {
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className);
                var instance = ReflectionUtils.CreateObject(
                    new
                    {
                        G3E_CID = 1,
                        G3E_ID = g3eObject.G3E_ID,
                        G3E_FID = g3eObject.G3E_FID,
                        G3E_FNO = g3eObject.G3E_FNO,
                        G3E_CNO = g3e_cno,
                        LTT_ID = Convert.ToDecimal(MapConfig.Instance.LTTID),
                        EntityState = EntityState.Insert
                    }, type) as ElectronSymbol;

                DBManager.Instance.Insert(instance);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        #endregion

        #region 新增型号规格
        public static bool InsertXHGE(int g3e_fno, string xhge, double? edrl)
        {
            try
            {
                var cdxhge = new Cd_xhge
                                 {
                                     G3E_FNO = g3e_fno,
                                     NAME = xhge,
                                     EDRL = edrl,
                                     EntityState = EntityState.Insert
                                 };
                CDDBManager.Instance.Insert(cdxhge);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return false;
        }

        #endregion
    }
}
