using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal.Cad;

namespace ElectronTransferView.TopologyManager
{
    public partial class SetColor : Form
    {
        public Color ccolor { get; set; }
        public SetColor()
        {
            InitializeComponent();
        }

        private void selColor_Click(object sender, EventArgs e)
        {
            var result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                ccolor = curColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SetColor_Load(object sender, EventArgs e)
        {
            ccolor = curColor.BackColor = PublicMethod.Instance.traceColor;
            //colorDialog1.AllowFullOpen = true;
            //colorDialog1.FullOpen = true;
        }
    }
}
