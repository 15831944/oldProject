using System;
using System.Linq;
using System.Windows.Forms;
using ElectronTransferBll.DBEntityHelper;
using ElectronTransferModel.Config;
using ElectronTransferDal.Query;

namespace ElectronTransferView.FunctionManager
{
    public partial class XHGEManagerForm : Form
    {
        public XHGEManagerForm()
        {
            InitializeComponent();
        }
        
        private void XHGEManagerForm_Load(object sender, EventArgs e)
        {
            var containXHGE = (from item in DeviceAttributeConfig.Instance.Attributes let xhge = item.Common.PropertiesFromTable.SingleOrDefault(o => o.Field == "CD_XHGE" && !o.ReadOnly)
                               where xhge != null select item).ToDictionary(item => item.Fno, item => item.LayerName).ToList();
       
            Combox_sblx.DisplayMember = "Value";
            Combox_sblx.ValueMember = "Key";
            Combox_sblx.DataSource = containXHGE;
        }
        private void Btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                var g3e_fno = Combox_sblx.Text.Trim();
                var xhge = Txt_xhge.Text.Trim();
                if (!string.IsNullOrEmpty(g3e_fno) && !string.IsNullOrEmpty(xhge))
                {
                    var fno = int.Parse(Combox_sblx.SelectedValue.ToString());
                    var sourceXHGE = DBEntityFinder.Instance.GetCDXHGE(fno, xhge);
                    if (sourceXHGE == null)
                    {
                        var result = InsertDBEntity.InsertXHGE(fno, xhge, null);
                        var message = result ? "添加成功！" : "添加失败！";
                        MessageBox.Show(message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("该型号规格已存在！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                else
                {
                    MessageBox.Show("请把信息填写完整！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }catch(Exception exception)
            {
                MessageBox.Show("添加失败！\n错误信息："+exception.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
