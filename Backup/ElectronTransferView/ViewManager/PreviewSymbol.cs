using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ElectronTransferView.ViewManager
{
    public partial class PreviewSymbol : Form
    {
        private readonly string filePath;
        public PreviewSymbol(string symbolPath)
        {
            InitializeComponent();
            filePath = symbolPath;
        }

        private void PreviewSymbol_Load(object sender, EventArgs e)
        {
            pictureBox1.Load(filePath);
        }
    }
}
