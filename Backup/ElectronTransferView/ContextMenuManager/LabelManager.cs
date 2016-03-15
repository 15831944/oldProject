using System;
using System.Linq;
using System.Windows.Forms;
using ArxMap;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferView.ViewManager;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class LabelManager : UserControl
    {
        public FixEntityArgs eventArgs { set; get; }
        private const string added = "已添加";
        private const string didnotadd = "未添加";
        private const string adddefeat = "添加失败";
        public LabelManager()
        {
            InitializeComponent();
        }

        private void LabelManager_Load(object sender, EventArgs e)
        {
            try
            {
                BindLabelSource();
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            if (!isInsert)
                AddSymbolLabel();
            else
                MessageBox.Show("命令未结束，请选择标注插入点！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void Btn_Del_Click(object sender, EventArgs e)
        {
            var selectCount = listView1.SelectedItems.Count;
            if (selectCount < 1)
            {
                PublicMethod.Instance.AlertDialog("请选择要删除的标注");
                return;
            }
            if (MessageBox.Show("确定要删除选择的标注吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==DialogResult.OK)
            {
                for (var i = 0; i < selectCount; i++)
                {
                    var index = listView1.SelectedItems[i].Index;
                    var finderFuncNumber = listView1.Items[index].SubItems[4].Text;
                    var funcNumber = int.Parse(finderFuncNumber);

                    if (SymbolLabel.DeleteLabel(funcNumber, eventArgs.g3eObject.G3E_FID))
                    {
                        SetItemsStatus(index, "删除成功!");
                        Btn_Del.Enabled = false;
                    }
                    else
                    {
                        SetItemsStatus(index, "删除失败！");
                    }
                }
            }
        }
        private bool isInsert;
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (!isInsert)
                    AddSymbolLabel();
                else
                    MessageBox.Show("命令未结束，请选择标注插入点！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error("新增标注错误！"+ex);
            }
        }


        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            var selectCount = listView1.SelectedItems.Count;
            for (var i = 0; i < selectCount; i++)
            {
                var index = listView1.SelectedItems[i].Index;
                var G3e_Fno = listView1.Items[index].SubItems[4].Text;
                IsInsertLabel =DBEntityFinder.Instance.GetLabelStatus(int.Parse(G3e_Fno), eventArgs.g3eObject.G3E_FID);
                if (IsInsertLabel)
                {
                    Btn_Add.Enabled = false;
                    Btn_Del.Enabled = true;
                }
                else
                {
                    Btn_Add.Enabled = selectCount <= 1;
                    Btn_Del.Enabled = false;
                    break;
                }
            }
        }

        /// <summary>
        ///  绑定标注数据
        /// </summary>
        public void BindLabelSource()
        {
            var dbSymbols = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == eventArgs.g3eObject.G3E_FNO);
            if (dbSymbols != null)
            {
                listView1.Items.Clear();
                for (var i = 0; i < dbSymbols.Label.Count; i++)
                {
                    var str = new string[5];
                    var g3e_fno = i == 0 ? eventArgs.g3eObject.G3E_FNO : eventArgs.g3eObject.G3E_FNO * 10 + i;
                    var bl = DBEntityFinder.Instance.GetLabelStatus(g3e_fno, eventArgs.g3eObject.G3E_FID);
                    var status = bl ? added : didnotadd;
                    str[0] = dbSymbols.Label[i].Name;
                    str[1] = status;
                    str[2] = dbSymbols.Label[i].LabelPtTable;
                    str[3] = dbSymbols.Label[i].CNO;
                    str[4] = g3e_fno.ToString();
                    var lvi = new ListViewItem(str, 0);

                    listView1.Items.Add(lvi);
                }
            }
        }

        /// <summary>
        /// 是否已插入标注
        /// </summary>
        private bool IsInsertLabel;
   

        private void AddSymbolLabel()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (!IsInsertLabel)
                {
                    var index = listView1.SelectedItems[0].Index;
                    var lableClassName = listView1.Items[index].SubItems[2].Text;
                    var g3e_cno = listView1.Items[index].SubItems[3].Text;
                    var G3e_Fno = listView1.Items[index].SubItems[4].Text;
                    if(eventArgs.ObjId.IsNull) return;
                
                    var lText = SymbolLabel.GetLabelText(g3e_cno,eventArgs.g3eObject);
                    if (!string.IsNullOrEmpty(lText.lbText))
                    {
                        isInsert = true;
                        Btn_Add.Enabled = false;
                        ViewHelper.labelManagerPs.KeepFocus = false;
                        //添加标注
                        var result = SymbolLabel.AddLabel(eventArgs.g3eObject, int.Parse(G3e_Fno), lableClassName,
                                             Convert.ToInt32(g3e_cno), lText);
                        //设置控件状态
                        SetItemsStatus(index, result ? added : adddefeat);
                        ViewHelper.labelManagerPs.KeepFocus = true;
                        isInsert = false;
                    }
                    else
                    {
                        var text = listView1.Items[index].SubItems[0].Text;
                        var displayName = GenerateHelper.GetDisplayName(eventArgs.g3eObject.G3E_FNO, text);
                        MessageBox.Show(string.Format("【{0}】属性值没有填写！请先填写该属性！", displayName), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                PublicMethod.Instance.AlertDialog("请选择要添加的标注");
            }
        }
        private void SetItemsStatus(int index, string status)
        {
            listView1.Items[index].SubItems[1].Text = status;
        }
    }
}
