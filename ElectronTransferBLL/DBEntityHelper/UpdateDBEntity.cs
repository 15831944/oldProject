using System;
using System.Collections.Generic;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.V9_4;

namespace ElectronTransferBll.DBEntityHelper
{
    /// <summary>
    /// 更新实体
    /// </summary>
    public static class UpdateDBEntity
    {
        /// <summary>
        /// 更新受电馈线
        /// </summary>
        /// <param name="g3e_id"></param>
        /// <param name="bdz"> </param>
        /// <param name="kxh"> </param>
        public static bool UpdateSDKX_AC(long g3e_id,string bdz,string kxh)
        {
            try
            {
                var sdkx = DBEntityFinder.Instance.GetSdkxAcByG3e_ID(g3e_id);
                if (sdkx != null)
                {
                    var newObj = sdkx.Clone() as Gg_pd_sdkx_ac;
                    newObj.GDBDZ = bdz;
                    newObj.KXH = kxh;
                    if (newObj.EntityState == EntityState.None)
                    {
                        newObj.EntityState = EntityState.Update;
                    }
                    DBManager.Instance.Update(newObj);
                    return true;
                }
                return false;
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 更新受电馈线
        /// </summary>
        /// <param name="bdz"> </param>
        /// <param name="kxh"> </param>
        /// <param name="sdkx"> </param>
        public static bool UpdateSDKX_AC(string bdz, string kxh,Gg_pd_sdkx_ac sdkx)
        {
            try
            {
                var newObj = sdkx.Clone() as Gg_pd_sdkx_ac;
                newObj.GDBDZ = bdz;
                newObj.KXH = kxh;
                if (newObj.EntityState == EntityState.None)
                {
                    newObj.EntityState = EntityState.Update;
                }
                DBManager.Instance.Update(newObj);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 更新公共属性表
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static bool UpdateCommon(Common_n com)
        {
            try
            {
                var newObj = com.Clone() as Common_n;
                //只有原始数据才改变状态
                if (newObj.EntityState == EntityState.None)
                    newObj.EntityState = EntityState.Update;
                DBManager.Instance.Update(newObj);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 更新功能位置表
        /// </summary>
        /// <param name="gnwz"> </param>
        /// <returns></returns>
        public static bool UpdateGNWZ(Gg_pd_gnwzmc_n gnwz)
        {
            try
            {
                var newObj = gnwz.Clone() as Gg_pd_gnwzmc_n;
                //只有原始数据才改变状态
                if (newObj != null && newObj.EntityState == EntityState.None)
                    newObj.EntityState = EntityState.Update;
                DBManager.Instance.Update(newObj);
                return true;
            }
            catch { return false; }
        }

        #region  更新杂标
        public static bool UpdateZXBZN(string text, string bzlx, string ssdw, string dysbfid, Gg_gl_zxbz_n zxbzn)
        {
            try
            {
                zxbzn.CD_SSDW = ssdw;
                zxbzn.CD_BZLX = bzlx;
                zxbzn.MIF_TEXT = text;
                //zxbzn.BZ_DYSB = dysbfid;
                if (zxbzn.EntityState == EntityState.None)
                    zxbzn.EntityState = EntityState.Update;
                DBManager.Instance.Update(zxbzn);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 更新所属开关柜
        /// </summary>
        /// <param name="gnwzs"></param>
        public static void UpdateSSKGG(IEnumerable<Gg_pd_gnwzmc_n> gnwzs)
        {
            if (gnwzs!=null)
            {
                foreach (var item in gnwzs)
                {
                    item.GNWZ_SSKGG = null;
                    if (item.EntityState == EntityState.None)
                        item.EntityState = EntityState.Update;
                    DBManager.Instance.Update(item);
                }
            }
        }
    }
}
