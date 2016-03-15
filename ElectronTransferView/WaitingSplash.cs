using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ElectronTransferView
{
    public partial class WaitingSplash : Form
    {
        private string messTip;
        public WaitingSplash(string mess)
        {
            InitializeComponent();
            messTip = mess;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Opacity -= 0.05;
            if (Opacity <= 0)
            {
                timer1.Stop();
                this.Close();
            }
        }

        public void Stop() 
        {
            timer1.Start();
        }

        private void WaitingSplash_Load(object sender, EventArgs e)
        {
            Opacity = 1;
            Lal_Tip.Text = messTip;
            if (pictureBox1.Image!=null && ImageAnimator.CanAnimate(pictureBox1.Image))
            ImageAnimator.Animate(pictureBox1.Image, (object sender1, EventArgs e1)=> ImageAnimator.UpdateFrames(pictureBox1.Image)  );
        }

        
    }
}
