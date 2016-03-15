using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.Config;

namespace ElectronTransferView.FunctionManager
{
    public partial class ModifyFrom : Form
    {
        public static event EventHandler FromModifyFromToOption;
        public string btnNameFromOption = string.Empty;
        public List<string> noVerifyFnoFromOption =new List<string>();
        private readonly Dictionary<string,List<int>> factoryDefault=new Dictionary<string, List<int>>
        {
            {"SbmcModify", new List<int> { 144, 141, 156, 73, 74, 169, 177 }},
            {"OwnShipModify", new List<int> { 140, 141, 151, 156, 157, 73, 74, 75, 169, 41, 160 }},
            {"ConnectivityModify", new List<int> { 320, 41, 160 }},
            {"TzModify", new List<int> { 159, 144, 82, 73, 74, 199, 320, 77, 171, 172, 160 }}
        }; 
        public ModifyFrom()
        {
            InitializeComponent();
        }
        private void btnFinish_Click(object sender, EventArgs e)
        {
            noVerifyFnoFromOption.Clear();
            for (int i = 0; i < lbFilterFeature.Items.Count; i++)
            {
                var fno = lbFilterFeature.Items[i].ToString().Split('_')[0];
                noVerifyFnoFromOption.Add(fno);
            }
            if (FromModifyFromToOption!=null)
            {
                FromModifyFromToOption(new object(),
                    new ModifyFormLinkToOptionArgs {BtnName = btnNameFromOption, NoVerifyFno = noVerifyFnoFromOption});
            }
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OperatorItemMethod(lbAllFeature,lbFilterFeature);
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            OperatorItemMethod(lbFilterFeature, lbAllFeature);
        }

        private void OperatorItemMethod(ListBox source,ListBox target)
        {
            for (int i = 0; i < source.SelectedItems.Count; i++)
            {
                target.Items.Add(source.SelectedItems[i]);
            }
            List<int> indexs=new List<int>();
            for (int i = 0; i < source.SelectedItems.Count; i++)
            {
                indexs.Add(source.SelectedIndices[i]);
            }
            if(indexs.Count<=0) return;
            for (int i = 0; i < indexs.Count; i++)
            {
                source.Items.RemoveAt(indexs[indexs.Count-1-i]);
            }
        }
        private void ModifyFrom_Load(object sender, EventArgs e)
        {
            LoadAfterFilterData(noVerifyFnoFromOption);
        }
        private void btnReModify_Click(object sender, EventArgs e)
        {
            //获取原始默认值
            var factoryDefalultList = factoryDefault[btnNameFromOption];
            List<string> temp=new List<string>();
            foreach (var item in factoryDefalultList)
            {
                temp.Add(item.ToString());
            }
            LoadAfterFilterData(temp);
        }

        private void LoadAfterFilterData(List<string> noVerifyList )
        {
            var features = DBEntityFinder.Instance.GetAllFeatureType();
            lbAllFeature.Items.Clear();
            lbFilterFeature.Items.Clear();
            foreach (var feature in features)
            {
                if (feature.Key == 0) continue;//公共表的FNO
                if (noVerifyList.Any(o => o.Equals(feature.Key.ToString())))
                {
                    lbFilterFeature.Items.Add(feature.Key + "_" + feature.Value);
                    continue;
                }
                lbAllFeature.Items.Add(feature.Key + "_" + feature.Value);
            }
        }
        private void lbFilterFeature_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
                lbFilterFeature.ClearSelected();
        }

        private void lbAllFeature_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Escape)
                lbAllFeature.ClearSelected();
        }
    }
}
