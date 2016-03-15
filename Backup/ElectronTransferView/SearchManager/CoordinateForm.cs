using System;using System.Windows.Forms;
using ArxMap;

namespace ElectronTransferView.SearchManager
{
    public partial class CoordinateForm : Form
    {
        public static bool isMessageOutput { set; get; }
        public CoordinateForm()
        {
            InitializeComponent();
            AddHandler();
        }
        private void AddHandler()
        {
            mouse.LeftButtonDownEventHander += mouse_LeftButtonDownEventHander;
        }

        void mouse_LeftButtonDownEventHander(object sender, EventArgs e)
        {
            if (isMessageOutput) return;
            var leftButton = (FixEntityArgs) e;
            Txt_Point.Text = string.Format("{0},{1}", leftButton.Position.X, leftButton.Position.Y);
        }

        private void Txt_Point_MouseEnter(object sender, EventArgs e)
        {
            isMessageOutput = true;
        }

        private void Txt_Point_MouseLeave(object sender, EventArgs e)
        {
            isMessageOutput = false;
        }
    }
}
