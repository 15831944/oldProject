using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.V9_4;

namespace ElectronTransferBll.DBEntityHelper
{
    /// <summary>
    /// 删除实体
    /// </summary>
    public class DeleteDBEntity
    {
        /// <summary>
        /// 删除受电馈线
        /// </summary>
        /// <param name="g3e_id"></param>
        public static bool DeleteSDKX(long g3e_id)
        {
            try
            {
                var sdkx = DBEntityFinder.Instance.GetSdkxAcByG3e_ID(g3e_id);
                if (sdkx != null)
                {
                    var newObj = sdkx.Clone() as Gg_pd_sdkx_ac;
                    if (newObj.EntityState == EntityState.None || newObj.EntityState == EntityState.Update)
                    {
                        newObj.EntityState = EntityState.Delete;
                        DBManager.Instance.Update(newObj);
                    }
                    else
                        DBManager.Instance.Delete(newObj);
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
        /// 批量删除受电馈线
        /// </summary>
        /// <param name="g3e_ids"></param>
        public static void DeleteSDKX(string[] g3e_ids)
        {
            foreach (var g3e_id in g3e_ids)
            {
                if (!string.IsNullOrEmpty(g3e_id))
                {
                    DeleteSDKX(long.Parse(g3e_id));
                }
            }
        }
        ///// <summary>
        ///// 删除受电馈线
        ///// </summary>
        ///// <param name="kxh"></param>
        //public static bool DeleteSDKX(string kxh)
        //{
        //    try
        //    {
        //        var sdkx = DBEntityFinder.Instance.GetSdkxAcByKXH(kxh);
        //        return DeleteSDKX(sdkx);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Instance.Error(ex);
        //        return false;
        //    }
        //}
        /// <summary>
        /// 删除受电馈线
        /// </summary>
        /// <param name="sdkx"></param>
        public static bool DeleteSDKX(Gg_pd_sdkx_ac sdkx)
        {
            try
            {
                if (sdkx != null)
                {
                    var newObj = sdkx.Clone() as Gg_pd_sdkx_ac;
                    if (newObj.EntityState == EntityState.None || newObj.EntityState == EntityState.Update)
                    {
                        newObj.EntityState = EntityState.Delete;
                        DBManager.Instance.Update(newObj);
                    }
                    else
                        DBManager.Instance.Delete(newObj);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return false;
            }
        }
    }
}
