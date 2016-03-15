using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferView.SearchManager
{
    public partial class MultilineInPut : Form
    {
        public MultilineInPut()
        {
            InitializeComponent();
            flag = true;
            btAccept.ImageIndex = 0;
           
        }
        //public string[] FilterItems = new string[] { "无开关柜", "无台架", "无杆塔", "无变压器", "无电房", "无支线", "无台区或台架" };
        public IList<string> FilterItems=new List<string>();
        /// <summary>
        /// 标志是否是多行文本
        /// </summary>
        public static bool flag { get; set; }
        /// <summary>
        /// 多行文本内容
        /// </summary>
        public  string inputText { get; set; }
        /// <summary>
        /// 记录过滤项当前选择的项索引
        /// </summary>
        private int RecordIndexFromFilter { get; set; }
        private void btAccept_Click(object sender, EventArgs e)
        {
            string temp = string.Empty;
            foreach (var item in lbFilterAfter.Items)
            {
                temp += item.ToString();
                temp += ",";
            }
            if (!string.IsNullOrEmpty(temp))
            {
                RichText.strSBMC=temp.Substring(0,temp.Length - 1);
            }
            this.Close();
        }
        private void OperatorItemMethod(ListBox source, ListBox target)
        {
            for (int i = 0; i < source.SelectedItems.Count; i++)
            {
                target.Items.Add(source.SelectedItems[i]);
            }
            List<int> indexs = new List<int>();
            for (int i = 0; i < source.SelectedItems.Count; i++)
            {
                indexs.Add(source.SelectedIndices[i]);
            }
            if (indexs.Count <= 0) return;
            for (int i = 0; i < indexs.Count; i++)
            {
                source.Items.RemoveAt(indexs[indexs.Count - 1 - i]);
            }
        }
        private void AddItem_Click(object sender, EventArgs e)
        {
            OperatorItemMethod(lbFilterBefore,lbFilterAfter);
        }

        private void SubItem_Click(object sender, EventArgs e)
        {
            OperatorItemMethod(lbFilterAfter,lbFilterBefore);
        }

        
        private void MultilineInPut_Load(object sender, EventArgs e)
        {
            var result = GenerateHelper.GetSpecialFilterOptionsOfDisplayName().ToArray();
            if (result.Any())
            {
                foreach (var item in result)
                {
                    FilterItems.Add("无" + item);
                }
            }
            if (!string.IsNullOrEmpty(inputText))
            {
                var res = inputText.Split(',');
                var exceptList=FilterItems.Except(res.ToList());
                lbFilterBefore.Items.AddRange(exceptList.ToArray());
                lbFilterAfter.Items.AddRange(res);
            }
            else
            {
                lbFilterBefore.Items.AddRange(FilterItems.ToArray());
                lbFilterAfter.Items.Clear();
            }
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbEdit.Text))
            {
                lbFilterAfter.Items.Add(tbEdit.Text);
                tbEdit.Text = string.Empty;
            }
        }

        private void lbFilterAfter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lbFilterAfter.ClearSelected();
            }
        }

        private void lbFilterBefore_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lbFilterBefore.ClearSelected();
            }
        }

        private void lbFilterAfter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RecordIndexFromFilter = lbFilterAfter.SelectedIndex;
        }
    }
}
