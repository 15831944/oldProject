namespace ElectronTransferView.SearchManager
{
    partial class CoordinateFix
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordinateFix));
            this.btFixed = new System.Windows.Forms.Button();
            this.imageListBt = new System.Windows.Forms.ImageList(this.components);
            this.texCoord = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolTipBt = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btFixed
            // 
            this.btFixed.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btFixed.ImageIndex = 0;
            this.btFixed.Location = new System.Drawing.Point(327, 44);
            this.btFixed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btFixed.Name = "btFixed";
            this.btFixed.Size = new System.Drawing.Size(60, 23);
            this.btFixed.TabIndex = 4;
            this.btFixed.Text = "定 位";
            this.btFixed.UseVisualStyleBackColor = true;
            this.btFixed.MouseLeave += new System.EventHandler(this.btFixed_MouseLeave);
            this.btFixed.Click += new System.EventHandler(this.btFixed_Click);
            this.btFixed.MouseEnter += new System.EventHandler(this.btFixed_MouseEnter);
            // 
            // imageListBt
            // 
            this.imageListBt.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBt.ImageStream")));
            this.imageListBt.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBt.Images.SetKeyName(0, "定位.png");
            this.imageListBt.Images.SetKeyName(1, "定位经过.png");
            // 
            // texCoord
            // 
            this.texCoord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.texCoord.Location = new System.Drawing.Point(7, 44);
            this.texCoord.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.texCoord.Name = "texCoord";
            this.texCoord.Size = new System.Drawing.Size(314, 23);
            this.texCoord.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.texCoord);
            this.groupBox1.Controls.Add(this.btFixed);
            this.groupBox1.Location = new System.Drawing.Point(14, 17);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(413, 101);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "请输入经纬度坐标（中间用逗号分隔）";
            // 
            // toolTipBt
            // 
            this.toolTipBt.AutoPopDelay = 5000;
            this.toolTipBt.InitialDelay = 500;
            this.toolTipBt.ReshowDelay = 100;
            // 
            // CoordinateFix
            // 
            this.AcceptButton = this.btFixed;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 142);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoordinateFix";
            this.Text = "坐标定位";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CoordinateFix_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btFixed;
        private System.Windows.Forms.TextBox texCoord;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ImageList imageListBt;
        private System.Windows.Forms.ToolTip toolTipBt;
    }
}