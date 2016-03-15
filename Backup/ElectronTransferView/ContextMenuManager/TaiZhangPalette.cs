using System;
using System.Windows.Forms;

namespace ElectronTransferView.ContextMenuManager
{
    public partial class TaiZhangPalette : UserControl
    {
        public TaiZhangPalette(string urlStr)
        {
            InitializeComponent();
            Refresh(urlStr);
        }

        public void Refresh(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                webBrowser1.Url = new Uri(url, UriKind.Absolute);
            }
        }
    }
}
