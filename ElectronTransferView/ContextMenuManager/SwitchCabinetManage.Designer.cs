namespace ElectronTransferView.ContextMenuManager
{
    partial class SwitchCabinetManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SwitchCabinetManage));
            this.SwitchCabinetAttr = new System.Windows.Forms.GroupBox();
            this.btBulk = new System.Windows.Forms.Button();
            this.TZInfo = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.pgSwitchCabinet = new System.Windows.Forms.PropertyGrid();
            this.imageListSwitch = new System.Windows.Forms.ImageList(this.components);
            this.lbSwitchCabinetcol = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ModifySwitch = new System.Windows.Forms.Button();
            this.DelSwitch = new System.Windows.Forms.Button();
            this.AddSwitch = new System.Windows.Forms.Button();
            this.toolTipSwitch = new System.Windows.Forms.ToolTip(this.components);
            this.SwitchCabinetAttr.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SwitchCabinetAttr
            // 
            this.SwitchCabinetAttr.Controls.Add(this.btBulk);
            this.SwitchCabinetAttr.Controls.Add(this.TZInfo);
            this.SwitchCabinetAttr.Controls.Add(this.btSave);
            this.SwitchCabinetAttr.Controls.Add(this.pgSwitchCabinet);
            this.SwitchCabinetAttr.Location = new System.Drawing.Point(218, 12);
            this.SwitchCabinetAttr.Name = "SwitchCabinetAttr";
            this.SwitchCabinetAttr.Size = new System.Drawing.Size(286, 307);
            this.SwitchCabinetAttr.TabIndex = 0;
            this.SwitchCabinetAttr.TabStop = false;
            this.SwitchCabinetAttr.Text = "开关柜属性";
            // 
            // btBulk
            // 
            this.btBulk.Location = new System.Drawing.Point(92, 275);
            this.btBulk.Name = "btBulk";
            this.btBulk.Size = new System.Drawing.Size(75, 23);
            this.btBulk.TabIndex = 3;
            this.btBulk.Text = "批量修改";
            this.btBulk.UseVisualStyleBackColor = true;
            this.btBulk.Click += new System.EventHandler(this.btBulk_Click);
            // 
            // TZInfo
            // 
            this.TZInfo.Location = new System.Drawing.Point(6, 275);
            this.TZInfo.Name = "TZInfo";
            this.TZInfo.Size = new System.Drawing.Size(72, 23);
            this.TZInfo.TabIndex = 2;
            this.TZInfo.Text = "台帐属性";
            this.TZInfo.UseVisualStyleBackColor = true;
            this.TZInfo.MouseLeave += new System.EventHandler(this.TZInfo_MouseLeave);
            this.TZInfo.Click += new System.EventHandler(this.TZInfo_Click);
            this.TZInfo.MouseEnter += new System.EventHandler(this.TZInfo_MouseEnter);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(219, 275);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(60, 23);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "保 存";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.MouseLeave += new System.EventHandler(this.btSave_MouseLeave);
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            this.btSave.MouseEnter += new System.EventHandler(this.btSave_MouseEnter);
            // 
            // pgSwitchCabinet
            // 
            this.pgSwitchCabinet.HelpVisible = false;
            this.pgSwitchCabinet.Location = new System.Drawing.Point(6, 20);
            this.pgSwitchCabinet.Name = "pgSwitchCabinet";
            this.pgSwitchCabinet.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgSwitchCabinet.Size = new System.Drawing.Size(273, 244);
            this.pgSwitchCabinet.TabIndex = 0;
            this.pgSwitchCabinet.ToolbarVisible = false;
            this.pgSwitchCabinet.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgSwitchCabinet_PropertyValueChanged);
            // 
            // imageListSwitch
            // 
            this.imageListSwitch.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSwitch.ImageStream")));
            this.imageListSwitch.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSwitch.Images.SetKeyName(0, "新增.png");
            this.imageListSwitch.Images.SetKeyName(1, "新增经过.png");
            this.imageListSwitch.Images.SetKeyName(2, "修改-编缉.png");
            this.imageListSwitch.Images.SetKeyName(3, "修改经过-编缉经过.png");
            this.imageListSwitch.Images.SetKeyName(4, "删除.png");
            this.imageListSwitch.Images.SetKeyName(5, "删除经过.png");
            this.imageListSwitch.Images.SetKeyName(6, "台帐属性.png");
            this.imageListSwitch.Images.SetKeyName(7, "台帐属性经过.png");
            this.imageListSwitch.Images.SetKeyName(8, "保存.png");
            this.imageListSwitch.Images.SetKeyName(9, "保存经过.png");
            // 
            // lbSwitchCabinetcol
            // 
            this.lbSwitchCabinetcol.FormattingEnabled = true;
            this.lbSwitchCabinetcol.ItemHeight = 12;
            this.lbSwitchCabinetcol.Location = new System.Drawing.Point(6, 20);
            this.lbSwitchCabinetcol.Name = "lbSwitchCabinetcol";
            this.lbSwitchCabinetcol.Size = new System.Drawing.Size(195, 244);
            this.lbSwitchCabinetcol.TabIndex = 1;
            this.lbSwitchCabinetcol.SelectedIndexChanged += new System.EventHandler(this.lbSwitchCabinetcol_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ModifySwitch);
            this.groupBox1.Controls.Add(this.DelSwitch);
            this.groupBox1.Controls.Add(this.lbSwitchCabinetcol);
            this.groupBox1.Controls.Add(this.AddSwitch);
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 307);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "开关柜";
            // 
            // ModifySwitch
            // 
            this.ModifySwitch.Location = new System.Drawing.Point(74, 275);
            this.ModifySwitch.Name = "ModifySwitch";
            this.ModifySwitch.Size = new System.Drawing.Size(60, 23);
            this.ModifySwitch.TabIndex = 5;
            this.ModifySwitch.Text = "修 改";
            this.ModifySwitch.UseVisualStyleBackColor = true;
            this.ModifySwitch.MouseLeave += new System.EventHandler(this.ModifySwitch_MouseLeave);
            this.ModifySwitch.Click += new System.EventHandler(this.ModifySwitch_Click);
            this.ModifySwitch.MouseEnter += new System.EventHandler(this.ModifySwitch_MouseEnter);
            // 
            // DelSwitch
            // 
            this.DelSwitch.Location = new System.Drawing.Point(141, 275);
            this.DelSwitch.Name = "DelSwitch";
            this.DelSwitch.Size = new System.Drawing.Size(60, 23);
            this.DelSwitch.TabIndex = 4;
            this.DelSwitch.Text = "删 除";
            this.DelSwitch.UseVisualStyleBackColor = true;
            this.DelSwitch.MouseLeave += new System.EventHandler(this.DelSwitch_MouseLeave);
            this.DelSwitch.Click += new System.EventHandler(this.DelSwitch_Click);
            this.DelSwitch.MouseEnter += new System.EventHandler(this.DelSwitch_MouseEnter);
            // 
            // AddSwitch
            // 
            this.AddSwitch.Location = new System.Drawing.Point(7, 275);
            this.AddSwitch.Name = "AddSwitch";
            this.AddSwitch.Size = new System.Drawing.Size(60, 23);
            this.AddSwitch.TabIndex = 3;
            this.AddSwitch.Text = "新 增";
            this.AddSwitch.UseVisualStyleBackColor = true;
            this.AddSwitch.MouseLeave += new System.EventHandler(this.AddSwitch_MouseLeave);
            this.AddSwitch.Click += new System.EventHandler(this.AddSwitch_Click);
            this.AddSwitch.MouseEnter += new System.EventHandler(this.AddSwitch_MouseEnter);
            // 
            // SwitchCabinetManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 323);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SwitchCabinetAttr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SwitchCabinetManage";
            this.Text = "开关柜管理";
            this.Load += new System.EventHandler(this.SwitchCabinetManage_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SwitchCabinetManage_FormClosing);
            this.SwitchCabinetAttr.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SwitchCabinetAttr;
        private System.Windows.Forms.PropertyGrid pgSwitchCabinet;
        private System.Windows.Forms.ListBox lbSwitchCabinetcol;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button AddSwitch;
        private System.Windows.Forms.Button DelSwitch;
        private System.Windows.Forms.Button ModifySwitch;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button TZInfo;
        private System.Windows.Forms.ImageList imageListSwitch;
        private System.Windows.Forms.ToolTip toolTipSwitch;
        private System.Windows.Forms.Button btBulk;
    }
}