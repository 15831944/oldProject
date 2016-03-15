using System;
using System.Linq;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferView
{
    
    public class DevEventHandler
    {
        /// <summary>
        /// 根据设备FNO和FID获取组件表
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public static RequiredDevTables GetDevTables(long g3e_fno, long g3e_fid)
        {
            try
            {
                var DevTables = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == (int)g3e_fno);
                if (DevTables == null)
                {
                    PublicMethod.Instance.ShowMessage("数据源中不存在所查找设备");
                    return null;
                }
                var rdt = new RequiredDevTables {Fno = (int) g3e_fno, Fid = g3e_fid};
                GenerateHelper.EmptyPrevFeatureAttribute();
                try
                {
                    if (!string.IsNullOrEmpty(DevTables.Common.TableName))
                    {
                        rdt.ComObj = DBManager.Instance.GetEntity<Common_n>(g3e_fid);
                        if (rdt.ComObj != null)
                            GenerateHelper.Atff.com = rdt.ComObj.Clone() as Common_n;
                        else
                        {
                            GenerateHelper.Atff.com = null;
                            PublicMethod.Instance.ShowMessage(string.Format("缺失公共属性表数据！G3E_FID：{0}\n", g3e_fid));
                        }
                    }
                }
                catch (NotExistException ex)
                {
                    rdt.ComObj = null;
                    PublicMethod.Instance.GetError(g3e_fno, g3e_fid, ex.Message);
                }
                try
                {
                    if (!string.IsNullOrEmpty(DevTables.SelfAttribute.TableName))
                    {
                        var self = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), DevTables.SelfAttribute.TableName.Trim());
                        if (self != null)
                            rdt.SelfObj = DBManager.Instance.GetEntity(self, g3e_fid);

                        if (rdt.SelfObj != null)
                        {
                            GenerateHelper.Atff.self = rdt.SelfObj.Clone() as DBEntity;
                            //RecordClickDevFid(rdt.SelfObj);
                        }
                        else
                        {
                            GenerateHelper.Atff.self = null;
                            PublicMethod.Instance.ShowMessage(string.Format("缺失功自身属性表数据！G3E_FID：{0}\n", g3e_fid));
                        }
                    }
                }
                catch (NotExistException ex)
                {
                    rdt.SelfObj = null;
                }
                try
                {
                    if (!string.IsNullOrEmpty(DevTables.Connectivity.TableName))
                    {
                        rdt.ConnectObj = DBManager.Instance.GetEntity<Connectivity_n>(g3e_fid);

                        if (rdt.ConnectObj != null)
                            GenerateHelper.Atff.connectivity = rdt.ConnectObj.Clone() as Connectivity_n;
                        else
                        {
                            GenerateHelper.Atff.connectivity = null;
                            PublicMethod.Instance.ShowMessage(string.Format("缺失连接关系表数据！G3E_FID：{0}\n", g3e_fid));
                        }
                    }
                }
                catch (NotExistException ex)
                {
                    rdt.ConnectObj = null;
                }
                try
                {
                    if (!string.IsNullOrEmpty(DevTables.Gnwz.TableName))
                    {
                        rdt.GnwzObj = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(g3e_fid);
                        if (rdt.GnwzObj != null)
                        {
                            GenerateHelper.Atff.gnwz = rdt.GnwzObj.Clone() as Gg_pd_gnwzmc_n;
                            //RecordClickDevFid(rdt.GnwzObj);
                        }
                        else
                        {
                            GenerateHelper.Atff.gnwz = null;
                            PublicMethod.Instance.ShowMessage(string.Format("缺失功能位置表数据！G3E_FID：{0}\n", g3e_fid));
                        }
                    }
                }
                catch (NotExistException ex)
                {
                    rdt.GnwzObj = null;
                }
                if (rdt.ComObj == null && rdt.SelfObj == null && rdt.ConnectObj == null && rdt.GnwzObj == null)
                {
                    return null;
                }
                var xprops=GenerateObj.Instance.GenderObjByFno((int) g3e_fno);
                if (xprops == null)
                {
                    if (g3e_fno != 250)
                        LogManager.Instance.Error("FNO=" + g3e_fno + "的对象生成失败！！！");
                    return null;
                }
                FieldReflection(xprops, rdt, (int)g3e_fno);
                rdt.DevObj = xprops;
                if (rdt.DevObj == null) return null;
                return rdt;
            }
            catch (SystemException ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            return null;
        }
        

        public static DBEntity  GetDBEntityFromCache(string className, long g3eFid)
        {
            try
            {

                ObjectId objectId = DBEntityFinder.Instance.GetObjectIdByFid(g3eFid);
                if (objectId.IsNull)
                {
                    return null;
                }
                if (DBSymbolFinder.Instance.ContainsKey(objectId))
                {
                    var brotherValues = DBSymbolFinder.Instance[objectId].GetSiblings();
                    if (brotherValues.Any())
                    {
                        return brotherValues.SingleOrDefault(o => o.GetType().Name == className);
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return null;

        }
        public static void FieldReflection(XProps source, RequiredDevTables rdt, int g3eFno)
        {
            try
            {
                GenerateHelper.ResetXProps(ref source);
                if (rdt.SelfObj != null)
                {
                    GenerateHelper.PartialCopyToCAD(rdt.SelfObj, source, g3eFno);
                }
                if (rdt.ConnectObj != null)
                {
                    GenerateHelper.PartialCopyToCAD(rdt.ConnectObj, source, g3eFno);
                }
                if (rdt.GnwzObj != null)
                {
                    GenerateHelper.PartialCopyToCAD(rdt.GnwzObj, source, g3eFno);
                }
                //把这个放最后的目的是因为所有设备的BZ 字段都取公共表里面的BZ
                if (rdt.ComObj != null)
                {
                    GenerateHelper.PartialCopyToCAD(rdt.ComObj, source, g3eFno);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
    }
}

