using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using ElectronTransferDal.QueryVerifyHelper;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferView.Menu;
using ElectronTransferView.ViewManager;
using V94 = ElectronTransferModel.V9_4;
using ElectronTransferDal;
using ElectronTransferModel.Base;
using ElectronTransferDal.Cad;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferView.SearchManager;
using ElectronTransferDal.Common;
using ElectronTransferModel;
using System.Windows.Forms;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferModel.V9_4;


namespace ElectronTransferView.ContextMenuManager
{
    class CommonHelp:Singleton<CommonHelp>
    {

        public event EventHandler RefreshFeatureAttributeEvent;
        public long FromMouseFixEnitiyFid { get; set; }
        public int FromMouseFixEntityFno { get; set; }
        /// <summary>
        /// 记录属性数据
        /// </summary>
        public List<VerifyClass> objList = new List<VerifyClass>();
        /// <summary>
        /// 记录拓扑数据
        /// </summary>
        public List<VerifyClass> topology = new List<VerifyClass>();
        /// <summary>
        /// 刷新了拓扑关系的设备
        /// </summary>
        public IList<long> RefTopologyFeature = new List<long>();

        #region DevAttribute用到的接口
        public void UpdateAttribute(XProps CurPropertyObj, ref ObjectId objId, RequiredDevTables rdt)
        {
            if (rdt == null || objId.IsNull || CurPropertyObj == null) return;
            ObjectId newObjId = ObjectId.Null;

            //只有块才可以更换符号
            if (!DCadApi.UpdateSymbol(objId, CurPropertyObj, out newObjId))
            {
                objId = newObjId;
            }
            else
            {
                FixEntity.Instance.ResetEntityRecord(newObjId, objId);
                if (objId != newObjId)
                    objId = newObjId;
            }
            UpdateAttribute(rdt, CurPropertyObj,false);
        }
        /// <summary>
        /// 按表逐个更新属性
        /// </summary>
        /// <param name="CurPropertyObj"></param>
        /// <param name="rdt"></param>
        /// <param name="g3e_fno"></param>
        /// <param name="flag">是否是批量修改公共属性</param>
        private void RefAttribute(XProps CurPropertyObj, RequiredDevTables rdt, int g3e_fno,bool flag)
        {
            var tablename = string.Empty;                
            try
            {

                if (rdt.ComObj != null)
                {
                    tablename = rdt.ComObj.GetType().Name;
                    GenerateHelper.PartialCopyFromCAD(CurPropertyObj, rdt.ComObj, g3e_fno);
                    if (GenerateHelper.Atff.com != null && rdt.ComObj.CompareProperties(GenerateHelper.Atff.com).Any())
                    {
                        if (rdt.ComObj.EntityState == EntityState.None)
                            rdt.ComObj.EntityState = EntityState.Update;
                        DBManager.Instance.Update(rdt.ComObj);
                    }
                }
                if (!flag)
                {
                    #region  如果是批量修改公共属性表就只针对公共属性表做修改别的表不处理
                    if (rdt.SelfObj != null)
                    {
                        tablename = rdt.SelfObj.GetType().Name;
                        GenerateHelper.PartialCopyFromCAD(CurPropertyObj, rdt.SelfObj, g3e_fno);
                        if (GenerateHelper.Atff.self != null && rdt.SelfObj.CompareProperties(GenerateHelper.Atff.self).Any())
                        {
                            if (rdt.SelfObj.EntityState == EntityState.None)
                                rdt.SelfObj.EntityState = EntityState.Update;
                            DBManager.Instance.Update(rdt.SelfObj);
                        }
                    }
                    if (rdt.ConnectObj != null)
                    {
                        tablename=rdt.ConnectObj.GetType().Name;
                        GenerateHelper.PartialCopyFromCAD(CurPropertyObj, rdt.ConnectObj, g3e_fno);
                        if (GenerateHelper.Atff.connectivity != null &&
                            rdt.ConnectObj.CompareProperties(GenerateHelper.Atff.connectivity).Any())
                        {
                            if (rdt.ConnectObj.EntityState == EntityState.None)
                                rdt.ConnectObj.EntityState = EntityState.Old_Nal_Nal;
                            DBManager.Instance.Update(rdt.ConnectObj);
                        }
                    }
                    if (rdt.GnwzObj != null)
                    {
                        tablename = rdt.GnwzObj.GetType().Name;
                        GenerateHelper.PartialCopyFromCAD(CurPropertyObj, rdt.GnwzObj, g3e_fno);
                        if (g3e_fno == 199)
                        {
                            rdt.GnwzObj.GNWZ_SSTJ = rdt.ComObj != null ? rdt.ComObj.SBMC : null;
                        }
                        if (g3e_fno == 198)
                            rdt.GnwzObj.MC = rdt.ComObj != null ? rdt.ComObj.SBMC : null;
                        if (GenerateHelper.Atff.gnwz != null && rdt.GnwzObj.CompareProperties(GenerateHelper.Atff.gnwz).Any())
                        {
                            if (rdt.GnwzObj.EntityState == EntityState.None)
                                rdt.GnwzObj.EntityState = EntityState.Update;
                            DBManager.Instance.Update(rdt.GnwzObj);
                        }
                    }
                    #endregion
                }
                SymbolLabel.ShowSymbolLabel(rdt.Fno, rdt.Fid); //更新的时候自动更新标注
            }
            catch (UpdataArgumentException ee)
            {
                throw new UpdataArgumentException(tablename,ee.FieldName);
            }
            finally
            {
                if (RefreshFeatureAttributeEvent!=null)
                {
                    RefreshFeatureAttributeEvent(null, new RefreshAttributeArgs
                    {
                        Rdt = rdt
                    });
                }
            }
        }

        /// <summary>
        /// 更新变压器的详图设备馈线属性
        /// </summary>
        /// <param name="detail_fid">变压器G3E_FID</param>
        /// <returns></returns>
        public void UpdateJLB(long detail_fid)
        {
            try
            {
                var detail = DBManager.Instance.GetEntities<Detailreference_n>( o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (detail == null) return; 
                // 更正详图信息
                if (detail.DETAIL_USERNAME == null) detail.DETAIL_USERNAME = detail.G3E_FID.ToString();
                if (detail.G3E_DETAILID == null) detail.G3E_DETAILID = CYZCommonFunc.getid();
                if (detail.EntityState != EntityState.Insert) detail.EntityState = EntityState.Update;
                DBManager.Instance.Update(detail);
                // 得到 详图设备
                var jxjlbs = DBManager.Instance.GetEntities<Gg_jx_jlb_pt>( o => o.G3E_DETAILID == detail.G3E_DETAILID && o.EntityState != EntityState.Delete);
                if (jxjlbs == null) return;
                // 得到 计量表 或 变压器 公共属性
                var detail_com = DBManager.Instance.GetEntity<Common_n>(detail_fid);
                if (detail_com == null) return;
                // 更新详图设备馈线属性
                foreach (var jlb in jxjlbs)
                {
                    var s = GetDEV_COM(jlb.G3E_FID);
                    if (s == null)  continue;
                    s.CD_SSDW = detail_com.CD_SSDW;
                    s.CD_SSXL = detail_com.CD_SSXL;
                    s.CD_SSBDZ = detail_com.CD_SSBDZ;
                    //s.GNWZ_SSGDS = detail_com.GNWZ_SSGDS;
                    if (s.EntityState != EntityState.Insert) s.EntityState = EntityState.Update;
                    DBManager.Instance.Update(s);
                }
            }
            catch (NotExistException ex)
            {
                PublicMethod.Instance.NotExistTable(ex);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 更新集抄箱的详图设备馈线属性
        /// </summary>
        /// <param name="detail_fid">集抄箱G3E_FID</param>
        /// <returns></returns>
        public void UpdateSHB(long detail_fid)
        {
            try
            {
                var detail = DBManager.Instance.GetEntities<Detailreference_n>(o => o.G3E_FID == detail_fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                if (detail == null) return;
                // 更正详图信息
                if (detail.DETAIL_USERNAME == null) detail.DETAIL_USERNAME = detail.G3E_FID.ToString();
                if (detail.G3E_DETAILID == null) detail.G3E_DETAILID = CYZCommonFunc.getid();
                if (detail.EntityState != EntityState.Insert) detail.EntityState = EntityState.Update;
                // 得到 详图设备
                var jxshbs = DBManager.Instance.GetEntities<Gg_jx_shbd_pt>(o => o.G3E_DETAILID == detail.G3E_DETAILID && o.EntityState != EntityState.Delete);
                if (jxshbs == null) return ;
                // 得到集抄箱的自身属性
                var shb_n = DBManager.Instance.GetEntity<Gg_pd_cbx_n>(detail_fid);
                foreach (var shb in jxshbs)
                {
                    var s = DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(shb.G3E_FID);
                    if (s == null) continue;
                    s.CD_SSDW = shb_n.CD_SSDW;
                    s.CD_SSXL = shb_n.CD_SSXL;
                    if (s.EntityState != EntityState.Insert) s.EntityState = EntityState.Update;
                    DBManager.Instance.Update(s);
                }
            }
            catch (NotExistException ex)
            {
                if (ex.Message.Contains("不存在"))
                {
                    var strlx = ex.Message.Replace("不存在", "");
                    var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), strlx);
                    var connectn = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                    DBManager.Instance.Insert(connectn);
                }
                else
                {
                    PublicMethod.Instance.AlertDialog(ex.Message);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
            }
        }
        private static Common_n GetDEV_COM(long? fid)
        {
            try
            {
                var t =
                    DBManager.Instance.GetEntities<Common_n>(
                        o => o.G3E_FID == fid && o.EntityState != EntityState.Delete).FirstOrDefault();
                return t;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion


        #region BulkChanges用到的接口
        /// <summary>
        /// 更新公共属性
        /// </summary>
        public UpdataResult UpdataCommonAttribute(ref List<BulkChangeEnt> Current, XProps objDev)
        {
            UpdataResult ur = new UpdataResult();
            try
            {
                DCadApi.isModifySymbol = true;

                foreach (var ent in Current)
                {
                    try
                    {
                        //查看是否有没有公共属性表
                        var simpleEntry = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == ent.SingleG3EObject.G3E_FNO);
                        if (simpleEntry != null)
                        {
                            if (!simpleEntry.Common.PropertiesFromTable.Any())
                            {
                                ur.UpdataFailedNum++;
                                ur.UpdataErrorLog.AppendLine(ent.SingleG3EObject.G3E_FID.ToString() + "没有公共属性表");
                                continue;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        ur.UpdataFailedNum++;
                        //ur.UpdataErrorLog.AppendLine("CAD实体查找错误,ObjectID="+ent.SingleG3EObject.G3E_FID);
                    }

                    #region
                    try
                    {
                        var value = DevEventHandler.GetDevTables(ent.SingleG3EObject.G3E_FNO, ent.SingleG3EObject.G3E_FID);
                        if (value.DevObj != null)
                        {
                            PartialCopy(objDev, value.DevObj);

                            //查找看看该设备是否有符号，有的就要根据属性更新符号
                            ObjectId objId = DBEntityFinder.Instance.GetObjectIdByFid(ent.SingleG3EObject.G3E_FID);
                            if (objId.IsNull)
                            {
                                UpdateAttribute(value, value.DevObj, true);
                                ur.UpdataSucceedNum++;
                            }
                            else
                            {
                                ObjectId newObjId = ObjectId.Null;
                                bool isUpdate = DCadApi.UpdateSymbol(objId, value.DevObj, out newObjId);
                                if (isUpdate)
                                {
                                    UpdateAttribute(value, value.DevObj, true);
                                    ur.UpdataSucceedNum++;
                                }
                                else
                                {
                                    //存放更新失败的设备
                                    ur.UpdataFailedNum++;
                                    ur.UpdataErrorLog.AppendLine("Fid=" + ent.SingleG3EObject.G3E_FID + "的设备更新失败 ");
                                }
                            }

                        }
                    }
                    catch (Exception)
                    {
                        ur.UpdataFailedNum++;
                        ur.UpdataErrorLog.AppendLine("Fid=" + ent.SingleG3EObject.G3E_FID + "的设备更新失败 ");
                    }
                    PublicMethod.Instance._isSelectedEntityChoice = false;
                    #endregion
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
            return ur;
        }

        /// <summary>
        /// 确保所属单位，所属变电站，受电馈线三者同时修改
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CheckIsNUll(XProps obj,int fno)
        {
            var ssdw = GenerateHelper.GetFirstXPropOfHasValue(obj,"CD_SSDW");
            var ssbdz = GenerateHelper.GetFirstXPropOfHasValue(obj, "CD_SSBDZ");
            var ssxl = GenerateHelper.GetFirstXPropOfHasValue(obj, "CD_SSXL");
            List<XProp> temp = new List<XProp>();
            if(fno==159)
                 temp.AddRange(new List<XProp>{ssdw,  ssxl} );
            else
                temp.AddRange(new List<XProp> { ssdw, ssbdz, ssxl });
            if (temp.All(o => o == null)) return true;
            if (temp.All(o => o != null && o.Value != null)) return true;
            return false;
        }
        /// <summary>
        /// 更新选择的同类设备自身属性
        /// </summary>
        public UpdataResult UpdataSelfAttribute(ref List<BulkChangeEnt> Current, XProps objDev)
        {
            UpdataResult ur = new UpdataResult();
            try
            {
                DCadApi.isModifySymbol = true;
                foreach (var ent in Current)
                {
                    try
                    {
                        var value = DevEventHandler.GetDevTables(ent.SingleG3EObject.G3E_FNO, ent.SingleG3EObject.G3E_FID);
                        if (value.DevObj != null)
                        {
                            PartialCopy(objDev, value.DevObj);
                            //查找看看该设备是否有符号，有的就要根据属性更新符号
                            ObjectId objId = DBEntityFinder.Instance.GetObjectIdByFid(ent.SingleG3EObject.G3E_FID);
                            if (objId.IsNull)
                            {
                                UpdateAttribute(value, value.DevObj, false);
                                ur.UpdataSucceedNum++;
                            }
                            else
                            {
                                ObjectId newObjId = ObjectId.Null;
                                bool isUpdate = DCadApi.UpdateSymbol(objId, value.DevObj, out newObjId);
                                if (isUpdate)
                                {
                                    UpdateAttribute(value, value.DevObj, false);
                                    ur.UpdataSucceedNum++;
                                }
                                else
                                {
                                    //存放更新失败的设备
                                    ur.UpdataFailedNum++;
                                    ur.UpdataErrorLog.AppendLine("Fid=" + ent.SingleG3EObject.G3E_FID + "的设备更新失败");
                                }
                            }

                        }
                        PublicMethod.Instance._isSelectedEntityChoice = false;
                    }
                    catch (Exception)
                    {
                        ur.UpdataFailedNum++;
                        ur.UpdataErrorLog.AppendLine("Fid=" + ent.SingleG3EObject.G3E_FID + "的设备更新失败");
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
            return ur;
        }

        /// <summary>
        /// 更新指定的实体属性
        /// </summary>
        /// <param name="values"></param>
        /// <param name="objDev"></param>
        /// <param name="flag">是否是批量修改公共表</param>
        private void UpdateAttribute(RequiredDevTables values, XProps objDev,bool flag)
        {
            try
            {
                DCadApi.isModifySymbol = true;

                #region

                if (values.Fno == 148)
                {
                    var currentAzwz = GenerateHelper.GetPropertyValue(objDev, GenerateHelper.GetGnwzTableNameByFno(148), "GNWZ_FL2");
                    string oldAzwz = string.Empty;
                    if (values.GnwzObj != null)
                        oldAzwz = values.GnwzObj.GNWZ_FL2;
                    //安装位置由非台架变成台架
                    if (currentAzwz != null && currentAzwz.ToString().Equals("台架"))
                    {
//以前是台架只需更新
                        if (oldAzwz.Equals("台架"))
                        {
                            RefAttribute(objDev, values, 148,flag);
                            //因为都是台架所以更新完148后还要更新199，
                            long tjFid = 0;
                            if (values.GnwzObj != null && long.TryParse(values.GnwzObj.GNWZ_SSTJ, out tjFid))
                            {
                                var tjTables = DevEventHandler.GetDevTables(199, tjFid);
                                RefAttribute(objDev, tjTables, 199, flag);
                            }
                        }
                        else //以前为空或不是台架
                        {
                            //先添加一条台架记录
                            //注意在添加新的台架数据之前先看看有没有对应的原始的台架数据（即被标记为Delete的数据）
                            try
                            {
                                var tj =
                               DBManager.Instance.GetEntity<Gg_gz_tj_n>(o =>o.HasAttribute("GNWZ_FID")&&o.GNWZ_FID!=null&&o.GNWZ_FID.Equals(values.Fid.ToString()));
                                if (tj != null)
                                {
                                    tj.EntityState = EntityState.Update;
                                    tj.IsErased = false;
                                    DBManager.Instance.Update(tj);
                                    var tjcom = DBManager.Instance.GetEntity<Common_n>(tj.G3E_FID);
                                    if (tjcom != null)
                                    {
                                        tjcom.EntityState = EntityState.Update;
                                        tjcom.IsErased = false;
                                        DBManager.Instance.Update(tjcom);
                                    }
                                    var tjGnwz = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(tj.G3E_FID);
                                    if (tjGnwz != null)
                                    {
                                        tjGnwz.EntityState = EntityState.Update;
                                        tjGnwz.IsErased = false;
                                        DBManager.Instance.Update(tjGnwz);
                                    }
                                    GenerateHelper.Unrf.tjFid = tj.G3E_FID.ToString();
                                    var tjTables = DevEventHandler.GetDevTables(199, tj.G3E_FID);
                                    RefAttribute(objDev, tjTables, 199, flag);
                                }
                                else
                                {
                                    var tjRes = DCadApi.AddAttribute(objDev, 199, values.Fid);
                                    GenerateHelper.Unrf.tjFid = tjRes.G3eFid;
                                }
                            }
                            catch (NotExistException ex)
                            {
                                var tjRes = DCadApi.AddAttribute(objDev, 199, values.Fid);
                                GenerateHelper.Unrf.tjFid = tjRes.G3eFid;
                            }
                            RefAttribute(objDev, values, 148, flag);
                        }
                    } //安装位置由台架变成非台架
                    else if (currentAzwz == null || !currentAzwz.ToString().Equals("台架"))
                    {
                        //以前是台架只需删除
                        if (oldAzwz.Equals("台架"))
                        {
                            //先删除台架数据
                            DBEntityErased.Instance.ErasedTjDBEntity(values.ComObj.G3E_FID, new List<DBEntity>());
                            //把变压器的GNWZ_SSTJ变成null
                            GenerateHelper.Unrf.tjFid = null;
                            //GenerateHelper.SetPropertyValue(objDev, "GNWZ_SSTJ", null);
                            if (values.GnwzObj != null)
                            {
                                values.GnwzObj.GNWZ_SSTJ = null;
                            }
                            RefAttribute(objDev, values, 148, flag);
                        }
                        else
                        {
                            RefAttribute(objDev, values, 148, flag);
                        }

                    }
                }
                else
                {
                    RefAttribute(objDev, values, values.Fno, flag);
                }
                UpdataShbAdnJlbValue(values.Fno, values.Fid);
                #endregion

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
        #endregion

        public void PartialCopy(XProps newObj, XProps oldObj)
        {
            if (newObj.GetType() != oldObj.GetType())
                throw new ArgumentException("拷贝的两对象的类型不一致");
            foreach (var item in newObj)
            {
                if (item.ReadOnly) continue;
                var value = GenerateHelper.GetPropertyValue(newObj,item.Category, item.Name);
                if (item.ProType == typeof (DateTime))
                {
                    if (!value.Equals(default(DateTime)))
                    {
                        GenerateHelper.SetPropertyValue(oldObj,item.Category, item.Name, value);
                    }
                }
                else
                {
                    if (value != null && !string.IsNullOrEmpty(value.ToString().Trim()))
                    {
                        GenerateHelper.SetPropertyValue(oldObj,item.Category, item.Name, value);
                    }
                }
            }
        }

        public void ShowErrorMsg()
        {
            MessageBox.Show("更新失败", "CAD消息", MessageBoxButtons.OK);
        }
        /// <summary>
        /// 判断指定设备是否是插入的新设备
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="fid"></param>
        /// <returns></returns>
        public  bool IsInsertDevice(int fno,long fid)
        {
            var tables=DevEventHandler.GetDevTables(fno,fid);
            if (tables != null)
            {
                if ((tables.ComObj != null && tables.ComObj.EntityState == EntityState.Insert) ||
                    (tables.SelfObj != null && tables.SelfObj.EntityState == EntityState.Insert) ||
                    (tables.ConnectObj!=null&&tables.ConnectObj.EntityState==EntityState.Insert)||
                    (tables.GnwzObj!=null&&tables.GnwzObj.EntityState==EntityState.Insert)
                    )
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 根据列名排序
        /// </summary>
        /// <param name="e"></param>
        /// <param name="lvwColumnSort"></param>
        /// <param name="lv"></param>
        public void SortByColumn(ColumnClickEventArgs e, ListViewColumnSorter lvwColumnSort, ListView lv)
        {
            if (e.Column == lvwColumnSort.SortColumn)
            {
                if (lvwColumnSort.Order == SortOrder.Ascending)
                {
                    lvwColumnSort.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSort.Order = SortOrder.Ascending;
                }
            }
            else
            {
                lvwColumnSort.SortColumn = e.Column;
                lvwColumnSort.Order = SortOrder.Ascending;
            }
            lv.Sort();
            ReCreateSerialNumber(lv);
        }

        public void ReCreateSerialNumber(ListView lv)
        {
            for (int i = 0; i < lv.Items.Count; i++)
            {
               lv.Items[i].SubItems[0].Text = (i + 1).ToString();
            }
        }
        public void SizeLastColumn(ListView lv)
        {
            if(lv.Columns.Count>1)
            {
                lv.Columns[lv.Columns.Count - 1].Width = -2;
            }
        }
        public void ChangeAttributeRunTime(XProps xprop, int currentFno,List<long> fidList,  string PropertyName, string newValue, object oldValue)
        {
            if (xprop == null || string.IsNullOrEmpty(PropertyName))
                return;
            #region
            int len = 0;
            if (!string.IsNullOrEmpty(newValue))
                len = Encoding.Default.GetByteCount(newValue);
            try
            {
                switch (PropertyName)
                {
                    case "SBMC":
                    {
                        //if (CheckInput.IsSpecialCharacter(newValue))
                        //{
                        //    PublicMethod.Instance.ShowMessage("\n功能位置名称不能包含特殊字符!!!");
                        //    GenerateHelper.SetPropertyValue(xprop, GenerateHelper.GetCommonTableNameByFno(currentFno), PropertyName, oldValue);
                        //    return;
                        //}
                        if (len > 200)
                        {
                            PublicMethod.Instance.ShowMessage("\n功能位置名称最大输入200，当前为" + len);
                            GenerateHelper.SetPropertyValue(xprop, GenerateHelper.GetCommonTableNameByFno(currentFno), PropertyName, oldValue);
                            return;
                        }
                        if (currentFno == 148)
                        {
                            var value = GenerateHelper.GetPropertyValue(xprop, GenerateHelper.GetGnwzTableNameByFno(currentFno), "GNWZ_FL2");
                            if (value != null && value.ToString().Equals("台架"))
                            {
                                GenerateHelper.SetSingleBrowsable(xprop, GenerateHelper.GetGnwzTableNameByFno(currentFno), "GNWZ_SSTJ", true);
                                var sbmc = GenerateHelper.GetPropertyValue(xprop, GenerateHelper.GetCommonTableNameByFno(currentFno), "SBMC");
                                GenerateHelper.SetPropertyValue(xprop, GenerateHelper.GetGnwzTableNameByFno(currentFno), "GNWZ_SSTJ", sbmc);
                            }
                        }
                    }
                        break;
                    case "SBBM":
                        {
                            if (len > 20)
                            {
                                PublicMethod.Instance.ShowMessage("\n设备编码最大输入20,当前为" + len);
                                GenerateHelper.SetPropertyValue(xprop, GenerateHelper.GetCommonTableNameByFno(currentFno), PropertyName, oldValue);
                            }
                        }
                        break;
                    case "CD_SMZQ":
                    {
                        if (currentFno == 147 || currentFno == 151)
                        {
                            var smzq = GenerateHelper.GetPropertyValue(xprop, GenerateHelper.GetCommonTableNameByFno(currentFno), "CD_SMZQ");
                            if (smzq != null && smzq.ToString().Equals("待投运"))
                            {
                                GenerateHelper.SetPropertyValue(xprop, GenerateHelper.GetCommonTableNameByFno(currentFno), "BZ", "备用");
                            }
                            else
                                GenerateHelper.SetPropertyValue(xprop, GenerateHelper.GetCommonTableNameByFno(currentFno), "BZ", null);
                        }
                    }
                        break;
                    case "CD_SSDW":
                    {
                        GenerateHelper.SetSSDW(xprop, oldValue, newValue);

                    }
                        break;
                    case "CD_SSBDZ":
                    {
                        
                        GenerateHelper.SetSSBDZ(xprop, oldValue, newValue);
                    }
                        break;
                    case "CD_SSXL":
                    {
                        //SsxlList.str = newValue;
                        //如果馈线改了相应的特殊字段必须清空,因为那些值都是按馈线过滤出来的
                        if (!string.IsNullOrEmpty(newValue))
                        {
                            if (oldValue == null || (!newValue.Equals(oldValue.ToString())))
                            {
                                DBEntityFinder.Instance.EmptySpecialFieldValue(xprop);
                            }
                        }

                    }
                        break;
                    case "AZWZ":
                    case "GNWZ_FL2":
                    case "DLT":
                    {
                        GenerateHelper.GenerateByAzwz(ref xprop, newValue.Trim(), currentFno,true);
                        AutoGenerationHelper.Instance.AddUITypeEditor(xprop, currentFno);
                        // 根据安装位置清空从属关系
                        OwnerMethods.delDevOwner(fidList);
                        if (ViewHelper.ConnetPs != null && (fidList.Any() && ViewHelper.ConnetPs.Visible))
                            MenuControl.showConnectManager(fidList.Last());
                    }
                        break;
                    case "GNWZ_SSGT":
                    {
                        GenerateHelper.Unrf.gtFid = GenerateHelper.choiceDevice.G3eFid;
                    }
                        break;
                    case "GNWZ_SSTJ":
                    {
                        if (currentFno == 148)
                        {
                            var azwz = GenerateHelper.GetPropertyValue(xprop, GenerateHelper.GetGnwzTableNameByFno(currentFno), "GNWZ_FL2");
                            if (azwz != null && azwz.ToString().Equals("台架"))
                            {
                                var sbmc = GenerateHelper.GetPropertyValue(xprop, GenerateHelper.GetCommonTableNameByFno(currentFno), "SBMC");
                                GenerateHelper.SetPropertyValue(xprop, GenerateHelper.GetGnwzTableNameByFno(currentFno), "GNWZ_SSTJ", sbmc);
                            }
                        }
                        else
                            GenerateHelper.Unrf.tjFid = GenerateHelper.choiceDevice.G3eFid;
                    }
                        break;
                    case "DYKGG":
                    case "GNWZ_SSKGG":
                    {
                        GenerateHelper.Unrf.kggFid = GenerateHelper.choiceDevice.G3eFid;
                    }
                        break;
                    case "SSDF":
                    case "GNWZ_SSDF":
                    {
                        GenerateHelper.Unrf.dfFid = GenerateHelper.choiceDevice.G3eFid;
                    }
                        break;
                    case "PBMC":
                    case "SSBYQ":
                    {
                        GenerateHelper.Unrf.byqFid = GenerateHelper.choiceDevice.G3eFid;
                    }
                        break;
                    case "GNWZ_SSTQHTJ":
                    {
                        GenerateHelper.Unrf.tqtjFid = GenerateHelper.choiceDevice.G3eFid;
                    }
                        break;
                    case "BZ1":
                    case "BZ2":
                    {
                        GenerateHelper.Unrf.zxFid = GenerateHelper.choiceDevice.G3eFid;
                    }
                        break;

                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            finally
            {
                GenerateHelper.choiceDevice.G3eFid = null;
                GenerateHelper.choiceDevice.DeviceSbmc = null;
            }
            #endregion
        }
        /// <summary>
        /// 修改变压器和集抄箱的供电局的时候更新散户表和计量表的供电局
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="fidList"></param>
        public void UpdataShbAdnJlbValue(int fno,long fid )
        {
            switch (fno)
            {
                case 159:
                {
                    UpdateSHB(fid);
                }
                    break;
                case 148:
                {
                    UpdateJLB(fid);
                }
                    break;
            }
        }
    }

    public class orderWorkifyArgs : EventArgs
    {
        public orderWorkifyArgs()
        {
        }

        public long FID { get; set; }
        public int FNO { get; set; }
        public ObjectId ObjId { get; set; }
    }

    public class bulkOrderWorkifyArgs : EventArgs
    {
        public bulkOrderWorkifyArgs()
        {
        }
        public List<BulkChangeEnt> bulkChangeList { get; set; }
    }

    public class RefreshAttributeArgs:EventArgs
    {
        public RefreshAttributeArgs(){}
        public RequiredDevTables Rdt { get; set; }
    }

    public class UpdataResult
    {
        public long FID { get; set; }
        public int UpdataSucceedNum { get; set; }
        public int UpdataFailedNum { get; set; }

        private StringBuilder ErrorLog =new StringBuilder();

        public StringBuilder UpdataErrorLog
        {
            get { return ErrorLog; }
            set { ErrorLog.AppendLine(value.ToString()); }
        }

    }
}
