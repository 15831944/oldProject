using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal;
using ElectronTransferDal.Query;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferView.ContextMenuManager;
using System.Net;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Common;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.V9_4;
using ElectronTransferModel.Config;
using ElectronTransferDal.Cad;
using ElectronTransferView.SearchManager;

namespace ElectronTransferView.SyncTZDataToGIS
{
    public partial class SyncTzDataToCAD : UserControl
    {
        private List<G3EObject> recordSyncData = new List<G3EObject>();
        private Dictionary<int, ItemOfData> recorDictionary = new Dictionary<int, ItemOfData>();
        public static event EventHandler SyncTzToRefPanel;
        public SyncTzDataToCAD()
        {
            InitializeComponent();
        }

        public void RestListBox()
        {
            btSync.Enabled = true;
            recorDictionary.Clear();
            recordSyncData.Clear();
            if(lbShowSyncData.Items.Count>0)
                lbShowSyncData.Items.Clear();
        }
        private void lbShowSyncData_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lbShowSyncData.SelectedIndex;
            if (index < -1) return;
            try
            {
                var iod = recorDictionary[index];
                if (iod != null)
                {
                    if (iod.RDT != null)
                    {
                        if (SyncTzToRefPanel != null)
                        {
                            ObjectId objId = ObjectId.Null;
                            objId = DBEntityFinder.Instance.GetObjectIdByFid(iod.RDT.Fid);
                            SyncTzToRefPanel(new object(),
                                new orderWorkifyArgs { FID = iod.RDT.Fid, FNO = iod.RDT.Fno, ObjId = objId });
                            FixEntity.Instance.Fix(iod.RDT.Fid);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void btSync_Click(object sender, EventArgs e)
        {
            btSync.Enabled = false;
            lbShowSyncData.Refresh();
            LoadNecessarySyncDev();
            if (lbShowSyncData.Items.Count > 0)
                lbShowSyncData.Items.Clear();
            lbShowSyncData.Items.Add("开始同步");
            lbShowSyncData.Refresh();
            WebClient webClient = new WebClient();
            int index = 1;//上面已经占用了一个位置
            foreach (var item in recordSyncData)
            {
                try
                {
                    string syncDataUrl = string.Empty;
                    var syncDataTables = DevEventHandler.GetDevTables(item.G3E_FNO, item.G3E_FID);
                    if (syncDataTables == null)
                    {
                        var str = string.Format("FID={0}的设备数据获取失败", item.G3E_FID);
                        recorDictionary.Add(index, new ItemOfData() { Fid = item.G3E_FID, Index = index, SyncResult = str, RDT = null });
                        lbShowSyncData.Items.Add(str);
                        continue;
                    }
                    if (CommonHelp.Instance.IsInsertDevice(item.G3E_FNO, item.G3E_FID))
                    {
                        syncDataUrl =
                            string.Format(
                                " http://localhost:9090/emmis/equipGisMappingTemp/cadRestful/synchAttributeToCAD.gis?g3e_fid=a{0}&g3e_fno={1}",
                                item.G3E_FID, item.G3E_FNO);
                    }
                    else
                    {
                        syncDataUrl =
                            string.Format(
                                " http://localhost:9090/emmis/equipGisMappingTemp/cadRestful/synchAttributeToCAD.gis?g3e_fid={0}&g3e_fno={1}",
                                item.G3E_FID, item.G3E_FNO);
                    }
                    byte[] redata = new byte[] { };
                    try
                    {
                        redata = webClient.DownloadData(syncDataUrl);
                    }
                    catch (WebException ex)
                    {
                        lbShowSyncData.Items.Add("下载台账数据失败");
                        return;
                    }
                    string syncRes = System.Text.Encoding.UTF8.GetString(redata);
                    if (string.IsNullOrEmpty(syncRes.Trim()))
                    {
                        var str = string.Format("FID={0}的设备缺少台账录入...", item.G3E_FID);
                        recorDictionary.Add(index, new ItemOfData() { Fid = item.G3E_FID, Index = index, SyncResult = str, RDT = syncDataTables });
                        lbShowSyncData.Items.Add(str);
                        continue;
                    }
                    var tzValues = GetSyncData(syncRes);
                    //开始同步数据
                    if (tzValues.Any())
                    {
                        ShowSyncResultToListBox(index, tzValues, syncDataTables);
                        if (item.G3E_FNO == 148 && syncDataTables.GnwzObj != null)
                        {
                            var azwzValue = GenerateHelper.GetCurrentEntityAzwzByFidAndFno(syncDataTables.Fid, syncDataTables.Fno);
                            if (azwzValue != null && azwzValue.ToString().Equals("台架"))
                            {
                                if (syncDataTables.GnwzObj.HasAttribute("GNWZ_SSTJ"))
                                {
                                    var tj = syncDataTables.GnwzObj.GNWZ_SSTJ;
                                    long tjFid = 0;
                                    if (long.TryParse(tj, out tjFid))
                                    {
                                        var value = DevEventHandler.GetDevTables(199, tjFid);
                                        ShowSyncResultToListBox(index, tzValues, value);

                                    }
                                }

                            }
                        }
                    }
                    //更新标注
                    SymbolLabel.ShowSymbolLabel(item.G3E_FNO, item.G3E_FID);
                }
                catch
                {
                }
                finally
                {
                    index++;
                    lbShowSyncData.Refresh();
                }
            }
            lbShowSyncData.Items.Add("同步完成.");
        }
        private void ShowSyncResultToListBox(int index, List<TzTableValue> tzValues, RequiredDevTables syncDataTables)
        {
            var res = SyncDataFrom(tzValues, syncDataTables);
            string rec = GetSyncFailedRecord(res, syncDataTables.Fno);
            var entity = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == syncDataTables.Fno);
            if (string.IsNullOrEmpty(rec))
            {
                var str = string.Format("FID={0}的{1}同步成功.", syncDataTables.Fid, entity != null ? entity.LayerName : "");
                recorDictionary.Add(index, new ItemOfData() { Fid = syncDataTables.Fid, Index = index, SyncResult = str, RDT = syncDataTables });
                lbShowSyncData.Items.Add(str);
            }
            else
            {
                var str = string.Format("FID={0}的{1}中{2}同步失败.", syncDataTables.Fid,
                    entity != null ? entity.LayerName : "", rec);
                recorDictionary.Add(index, new ItemOfData() { Fid = syncDataTables.Fid, Index = index, SyncResult = str, RDT = syncDataTables });
                lbShowSyncData.Items.Add(str);
            }
        }
        private string GetSyncFailedRecord(List<SyncRecord> records, int fno)
        {
            string failedRec = string.Empty;
            if (records.Any())
            {
                foreach (var syncRecord in records)
                {
                    if (syncRecord.Fields.Any())
                    {
                        string str = string.Empty;
                        var xprops = GenerateObj.Instance.GenderObjByFno(fno);
                        foreach (string field in syncRecord.Fields)
                        {
                            if (xprops != null)
                            {
                                var xprop = xprops.SingleOrDefault(o => o.Name.Equals(field));
                                str += xprop.DisplayName.Replace("*", "");
                            }
                            else
                            {
                                str += field;
                            }
                        }
                        string temp = string.Format(",{0}", str);
                        failedRec += temp;
                    }
                }
            }
            return failedRec;
        }
        private List<SyncRecord> SyncDataFrom(List<TzTableValue> tzValues, RequiredDevTables rdt)
        {
            List<SyncRecord> syncRecords = new List<SyncRecord>();
            foreach (var field in tzValues)
            {
                if (string.IsNullOrEmpty(field.TableName)) continue;
                switch (field.TableName)
                {
                    case "COMMON_N":
                        {
                            Common_n com = rdt.ComObj;
                            if (com != null)
                            {
                                syncRecords.Add(new SyncRecord { TableName = field.TableName, Fields = SyncDataToModel(field, com) });
                            }
                        }
                        break;
                    case "CONNECTIVITY_N":
                        {
                            var connectivity = rdt.ConnectObj;
                            if (connectivity != null)
                            {
                                syncRecords.Add(new SyncRecord { TableName = field.TableName, Fields = SyncDataToModel(field, connectivity) }); ;
                            }
                        }
                        break;
                    case "GG_PD_GNWZMC_N":
                        {
                            var gnwz = rdt.GnwzObj;
                            if (gnwz != null)
                                syncRecords.Add(new SyncRecord { TableName = field.TableName, Fields = SyncDataToModel(field, gnwz) });
                        }
                        break;
                    default:
                        {
                            DBEntity self = rdt.SelfObj;
                            if (self != null)
                            {
                                syncRecords.Add(new SyncRecord { TableName = field.TableName, Fields = SyncDataToModel(field, self) });
                            }
                        }
                        break;
                }
            }
            return syncRecords;
        }
        private List<string> SyncDataToModel(TzTableValue field, DBEntity entity)
        {
            List<string> syncFailedFields = new List<string>();
            foreach (var value in field.Values)
            {
                bool isModify = GenerateHelper.CopyDataToModel(value.cloumName, value.cloumValue, entity);
                if (isModify)
                {
                    if (entity.EntityState == EntityState.None)
                        entity.EntityState = EntityState.Update;
                    DBManager.Instance.Update(entity);
                }
                else
                {
                    syncFailedFields.Add(value.cloumName);
                }
            }
            return syncFailedFields;
        }
        private void LoadNecessarySyncDev()
        {
            if (recordSyncData.Any())
                recordSyncData.Clear();
            var values = DBEntityFinder.Instance.GetG3EObjectsInLttID();
            if (values.Any())
            {
                foreach (var g3EObject in values)
                {
                    int fno = g3EObject.G3E_FNO;
                    var xprops = GenerateObj.Instance.GenderObjByFno(fno);
                    if (xprops != null)
                    {
                        //必须有需要同步的字段才同步
                        var xpropList = xprops.Where(o => o.ReadOnly);
                        if (xpropList != null && xpropList.Any())
                        {
                            recordSyncData.Add(g3EObject);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 解析url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private List<TzTableValue> GetSyncData(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            //表集合
            var tables = str.Split(']');
            List<TzTableValue> tztables = new List<TzTableValue>();
            foreach (var table in tables)
            {
                if (string.IsNullOrEmpty(table.Trim())) break;
                TzTableValue ttv = new TzTableValue();
                var tableIndex = table.IndexOf('?');
                //表名
                var tableName = table.Substring(0, tableIndex);
                var keyValuesIndex = tableIndex + 1;
                //键值对字符串
                var keyValues = table.Substring(keyValuesIndex, table.Length - keyValuesIndex);
                //键值对
                var cloums = keyValues.Split(';');
                List<KeyValue> kvList = new List<KeyValue>();
                foreach (var cloum in cloums)
                {
                    if (string.IsNullOrEmpty(cloum.Trim())) break;
                    var cloumIndex = cloum.IndexOf(':');
                    var cloumName = cloum.Substring(0, cloumIndex);
                    var cloumValueIndex = cloumIndex + 1;
                    var cloumValue = cloum.Substring(cloumValueIndex, cloum.Length - cloumValueIndex);

                    KeyValue kv = new KeyValue
                    {
                        cloumName = cloumName,
                        cloumValue = cloumValue
                    };
                    kvList.Add(kv);
                }
                ttv.Values = kvList;
                ttv.TableName = tableName;
                tztables.Add(ttv);
            }
            return tztables;
        }
    }

    public sealed class SyncRecord
    {
        public string TableName { get; set; }
        public List<String> Fields { get; set; }
    }

    public sealed class ItemOfData
    {
        public ItemOfData()
        {
        }
        public int Index { get; set; }
        public long Fid { get; set; }
        public string SyncResult { get; set; }
        public RequiredDevTables RDT { get; set; }
    }
}
