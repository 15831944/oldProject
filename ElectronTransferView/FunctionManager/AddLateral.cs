using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using DotNetARX;
using ElectronTransferBll.DBEntityHelper;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;
using ElectronTransferDal.Cad;
using Autodesk.AutoCAD.DatabaseServices; 
using ElectronTransferDal.Common;

namespace ElectronTransferView.FunctionManager
{
    public partial class AddLateral : Form
    {
        public static string LateralName { get; set; }
        public static string DevType { get; set; }

        public static XProps dgXprops;
        public static XProps dxXprops;
        public static string DevStr;
        public static int flag;
        public static int igh { get; set; }

        private Dictionary<string, string> propertyList = new Dictionary<string, string>
        {
            {"CD_SSDW","Common_n" },
            { "CD_SSBDZ","Common_n"},
            { "CD_SSXL","Common_n"},
            { "CD_CLLB","Gg_gz_dg_n"}
        };
        public AddLateral()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            try
            {
                //电杆的公共属性默认给导线
                GenerateHelper.SetPropertyValue(dgXprops, "Common_n", "CD_SSDW", GenerateHelper.GetPropertyValue(dxXprops, "Common_n", "CD_SSDW"));
                GenerateHelper.SetPropertyValue(dgXprops, "Common_n", "CD_SSBDZ", GenerateHelper.GetPropertyValue(dxXprops, "Common_n", "CD_SSBDZ"));
                GenerateHelper.SetPropertyValue(dgXprops, "Common_n", "CD_SSXL", GenerateHelper.GetPropertyValue(dxXprops, "Common_n", "CD_SSXL"));
               
                if (string.IsNullOrEmpty(LateralName))
                {
                    MessageBox.Show("输入新增支线名称！");
                    return;
                }

                propertyGrid1.Refresh();
                propertyGrid2.Refresh();
                GenerateHelper.SetAllBrowsable(dgXprops,propertyList, false);
                Enabled = false;
                Visible = false;
                AddLateralLine();
                Visible = true;
                Enabled = true;
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 新增支线 
        /// </summary>
        private void AddLateralLine()
        {
            if (!LateralLine.PickFirst()) return;
            var iid = "201_" + flag;
            var idd = PublicMethod.Instance.GetBlockObjId(iid);
            if (idd.IsNull) return;
            var subline = new LateralLine(idd);
            using (PublicMethod.Instance.acDoc.LockDocument())
            {
                subline.doit(dgXprops, dxXprops, Ck_dllx.Checked);
            }
            if (LateralLine.dxObjIdList.Count > 0)
                InsertLateral(LateralLine.dxObjIdList, LateralLine.dgObjIdList);
        }

        public void InsertLateral(Dictionary<ObjectId, LateralLineData> dxObjIdList, ObjectIdList dgObjIdList)
        {
            GenerateHelper.SetPropertyValue(dxXprops,"Common_n", "SBMC", LateralName);
            var ghStr = GenerateHelper.GetPropertyValue(dgXprops, "Gg_gz_dg_n", "GH").ToString();
            var sbGh = ghStr.Substring(1, ghStr.Length-1);

            //添加导线
            AddDX(dxObjIdList);
            //添加电杆
            AddDG(dgObjIdList, sbGh);


            //设置记忆值
            DBEntityFinder.Instance.GetMemoryDevice(141, dxXprops);
            DBEntityFinder.Instance.GetMemoryDevice(201, dgXprops);
            GenerateHelper.SetPropertyValue(dgXprops, "Common_n", "SBMC", string.Format("{0}{1}{2}", LateralName, ghStr, DevStr));
            //Nametext.Text = string.Empty;
        }
        private void AddDX(Dictionary<ObjectId, LateralLineData> dxObjIdList/*,string sbGh*/)
        {
            var strObjID = ObjectId.Null;
            var endObjID = ObjectId.Null;
            //var dxH = int.Parse(sbGh);
            foreach (var item in dxObjIdList)
            {
                //var dxMc = string.Format("#{0}线", dxH);
                //GenerateHelper.SetPropertyValue(dxXprops, "SBMC", LateralName + dxMc);
                //添加导线
                DCadApi.InsertSymbol(item.Key, dxXprops, 141, item.Value.lineString);
                var angle = item.Value.lineAngle;
                //添加标注
                DCadApi.AddSymbolLabel(item.Key, angle);
                if (!item.Value.startObjID.IsNull)
                {
                    strObjID = item.Value.startObjID;
                }
                if (!item.Value.endObjID.IsNull)
                    endObjID = item.Value.endObjID;
                //dxH++;
            }
            //不是连续导线的时候自动添加连接关系
            if (!Ck_dllx.Checked)
                AutoTopology(strObjID, endObjID);
        }

        private void AddDG(IEnumerable<ObjectId> dgObjIdList, string dgH)
        {
            var ghIndex = 0;
            var isInteger= int.TryParse(dgH, out ghIndex);
            //添加杆
            foreach (var objId in dgObjIdList)
            {
                var gh = string.Empty; 
                if (isInteger)
                    gh = string.Format("#{0}", ghIndex);
                else
                    gh = string.Format("#{0}", dgH);

                string dgSbmc = string.Format("{0}{1}{2}", LateralName, gh, DevStr);

                GenerateHelper.SetPropertyValue(dgXprops,"Gg_gz_dg_n", "GH", gh);
                GenerateHelper.SetPropertyValue(dgXprops,"Common_n", "SBMC", dgSbmc);

                DxCommonToDg();

                DCadApi.InsertSymbol(objId, dgXprops, 201, null);
                DCadApi.AddSymbolLabel(objId, 0);
                ghIndex++;
            }
            //添加包含关系
            AutoContain();
        }
        /// <summary>
        /// 初始化公共属性（导线To电杆）
        /// </summary>
        private void DxCommonToDg()
        {
            //所属变电站
            GenerateHelper.SetPropertyValue(dgXprops,"Common_n", "CD_SSBDZ", GenerateHelper.GetPropertyValue(dxXprops, "Common_n", "CD_SSBDZ"));
            //材料类别（铁塔、水泥杆。。。）
            GenerateHelper.SetPropertyValue(dgXprops, "Gg_gz_dg_n", "CD_CLLB", DevType);

            var hwbs = GenerateHelper.GetPropertyValue(dxXprops, "Common_n", "WHBS");
            //维护班所
            GenerateHelper.SetPropertyValue(dgXprops, "Common_n", "WHBS", hwbs);
            //所属供电所
            //GenerateHelper.SetPropertyValue(dgXprops, "Common_n", "GNWZ_SSGDS",
            //                                GenerateHelper.GetPropertyValue(dxXprops, "Common_n", "GNWZ_SSGDS"));
        }

        /// <summary>
        /// 自动建连接
        /// </summary>
        /// <param name="strObjID">第一个设备</param>
        /// <param name="endObjID">第二个设备</param>
        private static void AutoTopology(ObjectId strObjID,ObjectId endObjID)
        {
            try
            {
                var at = new AutoConnect();
                at.autocbyobjid(strObjID, endObjID);
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex + "自动建连接关系错误！");
            }
        }

        /// <summary>
        /// 自动建包含
        /// </summary>
        private void AutoContain()
        {
            try
            {
                var dxObjList = LateralLine.dxObjIdList;
                //取导线数据
                var objectIds = dxObjList.Keys.ToList();
                if (objectIds.Count == 1)
                {
                    var dxObjId = objectIds[0];
                    //连续导线包含关系建立
                    ContinuouslyDxAutoContains(dxObjId);
                }
                else
                {
                    //断开导线包含关系建立
                    BreakDxAutoContains(objectIds);
                }
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex + "自动建包含关系错误！");
            }
        }
        /// <summary>
        /// 连续导线自动建立包含关系
        /// </summary>
        /// <param name="dxObjId">导线实体对象ID</param>
        private void ContinuouslyDxAutoContains(ObjectId dxObjId)
        {
            var dxPt = DBSymbolFinder.Instance[dxObjId];

            //获取导线包含表cid
            var cid = DBEntityFinder.Instance.GetContainCount(dxPt.G3E_FID);
            foreach (var item in LateralLine.dgObjIdList)
            {
                //取得电杆坐标数据
                var dgPt = DBSymbolFinder.Instance[item];
                //添加包含关系
                InsertDBEntity.AddContain(cid, dgPt.G3E_FID, dxPt);
            }
        }
        /// <summary>
        /// 断开导线自动建立包含关系
        /// </summary>
        /// <param name="dxObjIds">导线实体对象ID集合</param>
        private void BreakDxAutoContains(IList<ObjectId> dxObjIds)
        {
            //电杆对象ID集合
            var dgObjIds = LateralLine.dgObjIdList;
            //电杆数
            var dgCount = dgObjIds.Count;
          
            for (int i = 0; i < dgCount; i++)
            {
                //取得电杆坐标数据
                var dgPt = GetPtValue(i, dgObjIds);

                //最后一根电杆只包含一条线
                if (dgCount-1==i)
                {
                    var dxPt = GetPtValue(i, dxObjIds);
                    //获取导线包含表cid
                    var cid = DBEntityFinder.Instance.GetContainCount(dxPt.G3E_FID);
                    InsertDBEntity.AddContain(cid, dgPt.G3E_FID, dxPt);
                }
                else
                {
                    //电杆前面的导线
                    var previousDxPt = GetPtValue(i, dxObjIds);
                    //获取导线包含表cid
                    var cid = DBEntityFinder.Instance.GetContainCount(previousDxPt.G3E_FID);

                    cid = InsertDBEntity.AddContain(cid, dgPt.G3E_FID, previousDxPt);
                    //电杆后面的导线
                    var lastDxPt = GetPtValue(i+1, dxObjIds);
                    InsertDBEntity.AddContain(cid, dgPt.G3E_FID, lastDxPt);
                }
            }
        }
        private ElectronSymbol GetPtValue(int index,IList<ObjectId> objIds)
        {
            var objectId = objIds[index];
            return DBSymbolFinder.Instance[objectId];
        }
      


        private void button_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddLateral_Load(object sender, EventArgs e)
        {
            dxXprops = GenerateObj.Instance.GenderObjByFno(141);
            dxXprops=DBEntityFinder.Instance.GetMemoryDevice(141, dxXprops);

            //默认为连接导线
            GenerateHelper.SetPropertyValue(dxXprops, "Gg_pd_gnwzmc_n", "GNWZ_XLFL", "支线");

            propertyGrid1.SelectedObject = dxXprops;
            dgXprops = GenerateObj.Instance.GenderObjByFno(201);
            dgXprops = DBEntityFinder.Instance.GetMemoryDevice(201, dgXprops);



            //电杆的维护班所隐藏
            GenerateHelper.SetSingleBrowsable(dgXprops, "Common_n", "WHBS", false);
            //GenerateHelper.SetSingleBrowsable(dgXprops, "Common_n", "GNWZ_SSGDS", false);

            GenerateHelper.SetAllBrowsable(dgXprops, propertyList, false);
            propertyGrid2.SelectedObject = dgXprops;
            comboBox1.SelectedIndex = 0;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ProChanged(dxXprops, e);
        }

        private void propertyGrid2_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ProChanged(dgXprops, e);  
        }
        public  void ProChanged(XProps obj, PropertyValueChangedEventArgs e)
        {   
            string temp = e.ChangedItem.Label;
            if (temp.Contains('*'))
            {
                temp = temp.Replace('*', ' ').Trim();
            }
            switch (temp)
            {
                case "杆号":
                {
                    var dxSbmc = GenerateHelper.GetPropertyValue(dxXprops,"Common_n", "SBMC");
                    //GenerateHelper.SetPropertyValue(obj, "", dxSbmc.ToString() + e.ChangedItem.Value + DevStr);
                }
                    break;
            }
        }

        private void Nametext_TextChanged(object sender, EventArgs e)
        {
            LateralName = Nametext.Text;
            GenerateHelper.SetPropertyValue(dxXprops,"Common_n", "SBMC", LateralName);

            var dgsbmc = string.Format("{0}#1{1}", LateralName, DevStr);
            GenerateHelper.SetPropertyValue(dgXprops,"Common_n", "SBMC", dgsbmc);
            propertyGrid1.Refresh();
            propertyGrid2.Refresh();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevType = comboBox1.Text;
            DevStr = DevType;
            flag = 0;

            switch (DevStr)
            {
                case "水泥杆":
                    flag = 1;
                    DevStr = "杆";
                    DevType = "水泥杆";
                    break;
                case "钢杆":
                    flag = 0;
                    DevStr = "杆";
                    DevType = "钢杆";
                    break;
                case "铁塔":
                    flag = 2;
                    DevStr = "塔";
                    DevType = "铁塔";
                    break;
            }
            igh = 1;
            GenerateHelper.SetPropertyValue(dgXprops,"Gg_gz_dg_n", "CD_CLLB", DevType);
            GenerateHelper.SetPropertyValue(dgXprops,"Gg_gz_dg_n", "GH", "#1");
            var gh = GenerateHelper.GetPropertyValue(dgXprops, "Gg_gz_dg_n","GH");

            string dgsbmc = string.Format("{0}{1}{2}", LateralName, gh, DevStr);

            string dxsbmc = string.Format("{0}#1线", LateralName);

            GenerateHelper.SetPropertyValue(dgXprops,"Common_n", "SBMC", dgsbmc);
            GenerateHelper.SetPropertyValue(dxXprops,"Common_n", "SBMC", dxsbmc);

            propertyGrid1.Refresh();
            propertyGrid2.Refresh();
        }

        private void button_ok_MouseEnter(object sender, EventArgs e)
        {
            button_ok.ImageIndex = 0;
            toolTip1.Show("新增", (Button) sender);
        }

        private void button_ok_MouseLeave(object sender, EventArgs e)
        {
            button_ok.ImageIndex = 1;
            toolTip1.Hide((Button)sender);
        }

        private void button_close_MouseEnter(object sender, EventArgs e)
        {
            button_ok.ImageIndex = 2;
            toolTip1.Show("取消", (Button)sender);
        }

        private void button_close_MouseLeave(object sender, EventArgs e)
        {
            button_ok.ImageIndex = 3;
            toolTip1.Hide((Button)sender);
        }
                
    }
}
