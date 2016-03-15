using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArxMap.Config;
using ElectronTransferDal.Cad;
using ElectronTransferModel.Config;

namespace ElectronTransferView.FunctionManager
{
    public partial class Option : Form
    {
        public Option()
        {
            InitializeComponent();
          
        }
        void ModifyFrom_FromModifyFormToOption(object sender, EventArgs e)
        {
            var args = (ModifyFormLinkToOptionArgs) e;
            if (args == null) return;
            var value = BtnLinkToVerifyItems[args.BtnName];
            if (value != null)
            {
                var str = string.Empty;
                foreach (var item in args.NoVerifyFno)
                {
                    str += item;
                    str += ",";
                }
                if(!string.IsNullOrEmpty(str))
                    str = str.Substring(0, str.Length-1);
                value.TextBoxName.Text = str;
            }
            BtnLinkToVerifyItems[args.BtnName].NoVerifyItem = args.NoVerifyFno;
        }
        private readonly Dictionary<string, TxtCombinToContent> BtnLinkToVerifyItems = new Dictionary<string, TxtCombinToContent>(); 
        private void Option_Load(object sender, EventArgs e)
        {
            InitialMap();
            Txt_Lables.Text = MapConfig.Instance.ListLabelShow;
            Txt_TzServer.Text = MapConfig.Instance.TZPacketPath;
            noVerifySbmc.Text = MapConfig.Instance.NoVerifySbmcFeatures;
            noVerifyConnectivity.Text = MapConfig.Instance.NoVerifyConnectivityFeature;
            noVerifyOwnship.Text = MapConfig.Instance.NoVerifyOwnshipFeature;
            noVerifyTz.Text = MapConfig.Instance.NoVerifyTzFeature;
            FillDict();
            ModifyFrom.FromModifyFromToOption -= ModifyFrom_FromModifyFormToOption;
            ModifyFrom.FromModifyFromToOption += ModifyFrom_FromModifyFormToOption;
        }
        private string oldPath;
        private void InitialMap()
        {
            oldPath = GetMapPath();
            Txt_MapPath.Text = oldPath;
            Check_Map.Checked = MapConfig.Instance.BrowsableMap;
            if (MapConfig.Instance.BrowsableMap)
            {
                Txt_MapPath.ReadOnly = false;
                Btn_SelectMapPath.Enabled = true;
            }
            else
            {
                Txt_MapPath.ReadOnly = true;
                Btn_SelectMapPath.Enabled = false;
            }
        }
        private void FillDict()
        {
            BtnLinkToVerifyItems.Clear();
            BtnLinkToVerifyItems.Add(SbmcModify.Name,new TxtCombinToContent{TextBoxName = noVerifySbmc,NoVerifyItem =MapConfig.Instance.NoVerifySbmcFeatures.Split(',').ToList() });
            BtnLinkToVerifyItems.Add(ConnectivityModify.Name,new TxtCombinToContent{TextBoxName = noVerifyConnectivity,NoVerifyItem = MapConfig.Instance.NoVerifyConnectivityFeature.Split(',').ToList()});
            BtnLinkToVerifyItems.Add(OwnShipModify.Name,new TxtCombinToContent{TextBoxName =noVerifyOwnship,NoVerifyItem = MapConfig.Instance.NoVerifyOwnshipFeature.Split(',').ToList()});
            BtnLinkToVerifyItems.Add(TzModify.Name,new TxtCombinToContent{TextBoxName = noVerifyTz,NoVerifyItem = MapConfig.Instance.NoVerifyTzFeature.Split(',').ToList()});
        }
        private string GetMapPath()
        {
            var mapPath = MapConfig.Instance.BaseDir;
            if(!string.IsNullOrEmpty(mapPath))
            {
                var index = mapPath.IndexOf("{");
                mapPath = mapPath.Substring(0, index - 2);
            }
            return mapPath;
        }
        private void Save()
        {
            //地图
            var newMapPath=MapConfig.Instance.BaseDir.Replace(oldPath, Txt_MapPath.Text);
            //标注
            var lables = Txt_Lables.Text.Trim();
            MapConfig.Instance.ListLabelShow = lables;
            var lablesList = lables.Split(',');
            MapConfig.Instance.labels = lablesList;
            MapConfig.Instance.BaseDir = newMapPath;
            MapConfig.Instance.NoVerifyConnectivityFeature = noVerifyConnectivity.Text;
            MapConfig.Instance.NoVerifyOwnshipFeature = noVerifyOwnship.Text;
            MapConfig.Instance.NoVerifySbmcFeatures = noVerifySbmc.Text;
            MapConfig.Instance.NoVerifyTzFeature = noVerifyTz.Text;
            if (CYZMapConfig.cMapConfPtr != null)
            {
                //将地图应用到当前操作
                CYZMapConfig.cMapConfPtr.BaseDir = newMapPath;
            }
            MapConfig.Instance.BrowsableMap = Check_Map.Checked;

            //台帐服务
            MapConfig.Instance.TZPacketPath = Txt_TzServer.Text.Trim();

            var str = PublicMethod.Instance.SaveLocalMapConfig() ? "保存成功！" : "保存失败！";
            PublicMethod.Instance.AlertDialog(str);
        }

        private void Btn_SelectMapPath_Click(object sender, EventArgs e)
        {
            var openFile = new FolderBrowserDialog();
            if(openFile.ShowDialog()==DialogResult.OK)
            {
                Txt_MapPath.Text = openFile.SelectedPath;
            }
        }

        private void Btn_SelectTZServer_Click(object sender, EventArgs e)
        {
            var openFile = new FolderBrowserDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                Txt_TzServer.Text = openFile.SelectedPath;
            }
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void ShowVerifyFilterForm(string btnName,TxtCombinToContent noVerifyFno )
        {
            var mf = new ModifyFrom
            {
                btnNameFromOption = btnName,
                noVerifyFnoFromOption = noVerifyFno.NoVerifyItem
            };
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(mf);
        }
          
        private void Modify_Click(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            if (btn != null)
            {
                ShowVerifyFilterForm(btn.Name,BtnLinkToVerifyItems[btn.Name]);
            }
        }

        private void Check_Map_CheckedChanged(object sender, EventArgs e)
        {
            Txt_MapPath.ReadOnly = !Check_Map.Checked;
            Btn_SelectMapPath.Enabled = Check_Map.Checked;
        }
    }

    public class ModifyFormLinkToOptionArgs : EventArgs
    {
        public ModifyFormLinkToOptionArgs() { }
        public string BtnName { get; set; }
        public List<string> NoVerifyFno { get; set; } 
    }
    public class TxtCombinToContent
    {
        public TxtCombinToContent() { }
        public TextBox TextBoxName { get; set; }
        public List<string> NoVerifyItem { get; set; }
    }
}
