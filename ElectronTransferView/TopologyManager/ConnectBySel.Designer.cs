namespace ElectronTransferView.TopologyManager
{
    partial class ConnectBySel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectBySel));
            this.ConnlistView = new System.Windows.Forms.ListView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btn_ok = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btn_Down = new System.Windows.Forms.Button();
            this.btn_Up = new System.Windows.Forms.Button();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Del = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btn_clean = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnlistView
            // 
            this.ConnlistView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnlistView.Location = new System.Drawing.Point(10, 45);
            this.ConnlistView.Name = "ConnlistView";
            this.ConnlistView.Size = new System.Drawing.Size(269, 254);
            this.ConnlistView.TabIndex = 1;
            this.ConnlistView.UseCompatibleStateImageBehavior = false;
            this.ConnlistView.View = System.Windows.Forms.View.List;
            this.ConnlistView.VisibleChanged += new System.EventHandler(this.ConnectByUser_VisibleChanged);
            this.ConnlistView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ConnlistView_ItemSelectionChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(10, 10);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(269, 2);
            this.progressBar1.TabIndex = 8;
            // 
            // btn_ok
            // 
            this.btn_ok.ImageIndex = 9;
            this.btn_ok.ImageList = this.imageList1;
            this.btn_ok.Location = new System.Drawing.Point(162, 18);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(29, 25);
            this.btn_ok.TabIndex = 7;
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.VisibleChanged += new System.EventHandler(this.ConnectByUser_VisibleChanged);
            this.btn_ok.MouseLeave += new System.EventHandler(this.btn_ok_MouseLeave);
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            this.btn_ok.MouseEnter += new System.EventHandler(this.btn_ok_MouseEnter);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "上移经过.png");
            this.imageList1.Images.SetKeyName(1, "上移.png");
            this.imageList1.Images.SetKeyName(2, "下移经过.png");
            this.imageList1.Images.SetKeyName(3, "下移.png");
            this.imageList1.Images.SetKeyName(4, "增加经过.png");
            this.imageList1.Images.SetKeyName(5, "增加.png");
            this.imageList1.Images.SetKeyName(6, "删减经过.png");
            this.imageList1.Images.SetKeyName(7, "删减.png");
            this.imageList1.Images.SetKeyName(8, "选择经过.png");
            this.imageList1.Images.SetKeyName(9, "选择.png");
            // 
            // btn_Down
            // 
            this.btn_Down.ImageIndex = 3;
            this.btn_Down.ImageList = this.imageList1;
            this.btn_Down.Location = new System.Drawing.Point(48, 18);
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.Size = new System.Drawing.Size(29, 25);
            this.btn_Down.TabIndex = 6;
            this.btn_Down.UseVisualStyleBackColor = true;
            this.btn_Down.VisibleChanged += new System.EventHandler(this.ConnectByUser_VisibleChanged);
            this.btn_Down.MouseLeave += new System.EventHandler(this.btn_Down_MouseLeave);
            this.btn_Down.Click += new System.EventHandler(this.btn_down_Click);
            this.btn_Down.MouseEnter += new System.EventHandler(this.btn_Down_MouseEnter);
            // 
            // btn_Up
            // 
            this.btn_Up.ImageIndex = 1;
            this.btn_Up.ImageList = this.imageList1;
            this.btn_Up.Location = new System.Drawing.Point(10, 18);
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.Size = new System.Drawing.Size(29, 25);
            this.btn_Up.TabIndex = 5;
            this.btn_Up.UseVisualStyleBackColor = true;
            this.btn_Up.VisibleChanged += new System.EventHandler(this.ConnectByUser_VisibleChanged);
            this.btn_Up.MouseLeave += new System.EventHandler(this.btn_Up_MouseLeave);
            this.btn_Up.Click += new System.EventHandler(this.btn_up_Click);
            this.btn_Up.MouseEnter += new System.EventHandler(this.btn_Up_MouseEnter);
            // 
            // btn_Add
            // 
            this.btn_Add.ImageIndex = 5;
            this.btn_Add.ImageList = this.imageList1;
            this.btn_Add.Location = new System.Drawing.Point(86, 18);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(29, 25);
            this.btn_Add.TabIndex = 9;
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.MouseLeave += new System.EventHandler(this.btn_Add_MouseLeave);
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            this.btn_Add.MouseEnter += new System.EventHandler(this.btn_Add_MouseEnter);
            // 
            // btn_Del
            // 
            this.btn_Del.ImageIndex = 7;
            this.btn_Del.ImageList = this.imageList1;
            this.btn_Del.Location = new System.Drawing.Point(124, 18);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(29, 25);
            this.btn_Del.TabIndex = 10;
            this.btn_Del.UseVisualStyleBackColor = true;
            this.btn_Del.MouseLeave += new System.EventHandler(this.btn_Del_MouseLeave);
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            this.btn_Del.MouseEnter += new System.EventHandler(this.btn_Del_MouseEnter);
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 600;
            // 
            // btn_clean
            // 
            this.btn_clean.Location = new System.Drawing.Point(200, 18);
            this.btn_clean.Name = "btn_clean";
            this.btn_clean.Size = new System.Drawing.Size(38, 25);
            this.btn_clean.TabIndex = 11;
            this.btn_clean.Text = "清空";
            this.btn_clean.UseVisualStyleBackColor = true;
            this.btn_clean.Click += new System.EventHandler(this.btn_clean_Click);
            // 
            // ConnectBySel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_clean);
            this.Controls.Add(this.btn_Del);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_Down);
            this.Controls.Add(this.btn_Up);
            this.Controls.Add(this.ConnlistView);
            this.Name = "ConnectBySel";
            this.Size = new System.Drawing.Size(291, 307);
            this.VisibleChanged += new System.EventHandler(this.ConnectByUser_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ListView ConnlistView;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_Down;
        private System.Windows.Forms.Button btn_Up;
        internal System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Del;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btn_clean;
    }
}
