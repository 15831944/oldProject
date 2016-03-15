namespace ElectronTransferView.SearchManager
{
    partial class Query
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Query));
            this.QuerytabControl = new System.Windows.Forms.TabControl();
            this.tpFidQuery = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvFidQuery = new System.Windows.Forms.ListView();
            this.Fidlable = new System.Windows.Forms.Label();
            this.DeviceFID_label = new System.Windows.Forms.Label();
            this.FID_textBox = new System.Windows.Forms.TextBox();
            this.FIDQuery = new System.Windows.Forms.Button();
            this.tpMcQuery = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvSMQuery = new System.Windows.Forms.ListView();
            this.txtMohu = new System.Windows.Forms.TextBox();
            this.DeviceMohuLable = new System.Windows.Forms.Label();
            this.DeviceNameMoHuQuery = new System.Windows.Forms.Button();
            this.Device_MoHuName = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbKeyWord = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lvMap = new System.Windows.Forms.ListView();
            this.textBox_key = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_search = new System.Windows.Forms.Button();
            this.imageListQuery = new System.Windows.Forms.ImageList(this.components);
            this.toolTipQuery = new System.Windows.Forms.ToolTip(this.components);
            this.QuerytabControl.SuspendLayout();
            this.tpFidQuery.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpMcQuery.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // QuerytabControl
            // 
            this.QuerytabControl.Controls.Add(this.tpFidQuery);
            this.QuerytabControl.Controls.Add(this.tpMcQuery);
            this.QuerytabControl.Controls.Add(this.tabPage1);
            this.QuerytabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuerytabControl.Location = new System.Drawing.Point(0, 0);
            this.QuerytabControl.Name = "QuerytabControl";
            this.QuerytabControl.SelectedIndex = 0;
            this.QuerytabControl.Size = new System.Drawing.Size(785, 581);
            this.QuerytabControl.TabIndex = 0;
            this.QuerytabControl.SelectedIndexChanged += new System.EventHandler(this.QuerytabControl_SelectedIndexChanged);
            // 
            // tpFidQuery
            // 
            this.tpFidQuery.BackColor = System.Drawing.Color.Transparent;
            this.tpFidQuery.Controls.Add(this.groupBox1);
            this.tpFidQuery.Controls.Add(this.Fidlable);
            this.tpFidQuery.Controls.Add(this.DeviceFID_label);
            this.tpFidQuery.Controls.Add(this.FID_textBox);
            this.tpFidQuery.Controls.Add(this.FIDQuery);
            this.tpFidQuery.Location = new System.Drawing.Point(4, 21);
            this.tpFidQuery.Name = "tpFidQuery";
            this.tpFidQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tpFidQuery.Size = new System.Drawing.Size(777, 556);
            this.tpFidQuery.TabIndex = 0;
            this.tpFidQuery.Text = "按设备FID查询";
            this.tpFidQuery.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lvFidQuery);
            this.groupBox1.Location = new System.Drawing.Point(8, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(761, 516);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备列表";
            // 
            // lvFidQuery
            // 
            this.lvFidQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFidQuery.FullRowSelect = true;
            this.lvFidQuery.Location = new System.Drawing.Point(3, 17);
            this.lvFidQuery.MultiSelect = false;
            this.lvFidQuery.Name = "lvFidQuery";
            this.lvFidQuery.Size = new System.Drawing.Size(755, 496);
            this.lvFidQuery.TabIndex = 0;
            this.lvFidQuery.UseCompatibleStateImageBehavior = false;
            this.lvFidQuery.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFidQuery_MouseDoubleClick);
            // 
            // Fidlable
            // 
            this.Fidlable.AutoSize = true;
            this.Fidlable.ForeColor = System.Drawing.Color.Red;
            this.Fidlable.Location = new System.Drawing.Point(359, 10);
            this.Fidlable.Name = "Fidlable";
            this.Fidlable.Size = new System.Drawing.Size(0, 12);
            this.Fidlable.TabIndex = 3;
            // 
            // DeviceFID_label
            // 
            this.DeviceFID_label.AutoSize = true;
            this.DeviceFID_label.Location = new System.Drawing.Point(13, 11);
            this.DeviceFID_label.Name = "DeviceFID_label";
            this.DeviceFID_label.Size = new System.Drawing.Size(59, 12);
            this.DeviceFID_label.TabIndex = 2;
            this.DeviceFID_label.Text = "设备FID：";
            // 
            // FID_textBox
            // 
            this.FID_textBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.FID_textBox.Location = new System.Drawing.Point(79, 7);
            this.FID_textBox.Name = "FID_textBox";
            this.FID_textBox.Size = new System.Drawing.Size(187, 21);
            this.FID_textBox.TabIndex = 1;
            this.FID_textBox.TextChanged += new System.EventHandler(this.FID_textBox_TextChanged);
            // 
            // FIDQuery
            // 
            this.FIDQuery.Location = new System.Drawing.Point(272, 6);
            this.FIDQuery.Name = "FIDQuery";
            this.FIDQuery.Size = new System.Drawing.Size(60, 23);
            this.FIDQuery.TabIndex = 0;
            this.FIDQuery.Text = "查 询";
            this.FIDQuery.UseVisualStyleBackColor = true;
            this.FIDQuery.MouseLeave += new System.EventHandler(this.FIDQuery_MouseLeave);
            this.FIDQuery.Click += new System.EventHandler(this.FIDQuery_Click);
            this.FIDQuery.MouseEnter += new System.EventHandler(this.FIDQuery_MouseEnter);
            // 
            // tpMcQuery
            // 
            this.tpMcQuery.BackColor = System.Drawing.Color.Transparent;
            this.tpMcQuery.Controls.Add(this.groupBox2);
            this.tpMcQuery.Controls.Add(this.txtMohu);
            this.tpMcQuery.Controls.Add(this.DeviceMohuLable);
            this.tpMcQuery.Controls.Add(this.DeviceNameMoHuQuery);
            this.tpMcQuery.Controls.Add(this.Device_MoHuName);
            this.tpMcQuery.Location = new System.Drawing.Point(4, 21);
            this.tpMcQuery.Name = "tpMcQuery";
            this.tpMcQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tpMcQuery.Size = new System.Drawing.Size(777, 556);
            this.tpMcQuery.TabIndex = 2;
            this.tpMcQuery.Text = "按设备名称查询";
            this.tpMcQuery.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lvSMQuery);
            this.groupBox2.Location = new System.Drawing.Point(8, 33);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(763, 515);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "匹配到的设备列表";
            // 
            // lvSMQuery
            // 
            this.lvSMQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSMQuery.FullRowSelect = true;
            this.lvSMQuery.Location = new System.Drawing.Point(3, 17);
            this.lvSMQuery.MultiSelect = false;
            this.lvSMQuery.Name = "lvSMQuery";
            this.lvSMQuery.Size = new System.Drawing.Size(757, 495);
            this.lvSMQuery.TabIndex = 0;
            this.lvSMQuery.UseCompatibleStateImageBehavior = false;
            this.lvSMQuery.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvSMQuery_MouseDoubleClick);
            this.lvSMQuery.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSMQuery_ColumnClick);
            // 
            // txtMohu
            // 
            this.txtMohu.Location = new System.Drawing.Point(79, 7);
            this.txtMohu.Name = "txtMohu";
            this.txtMohu.Size = new System.Drawing.Size(187, 21);
            this.txtMohu.TabIndex = 8;
            // 
            // DeviceMohuLable
            // 
            this.DeviceMohuLable.AutoSize = true;
            this.DeviceMohuLable.ForeColor = System.Drawing.Color.Red;
            this.DeviceMohuLable.Location = new System.Drawing.Point(357, 9);
            this.DeviceMohuLable.Name = "DeviceMohuLable";
            this.DeviceMohuLable.Size = new System.Drawing.Size(0, 12);
            this.DeviceMohuLable.TabIndex = 7;
            // 
            // DeviceNameMoHuQuery
            // 
            this.DeviceNameMoHuQuery.Location = new System.Drawing.Point(272, 6);
            this.DeviceNameMoHuQuery.Name = "DeviceNameMoHuQuery";
            this.DeviceNameMoHuQuery.Size = new System.Drawing.Size(60, 23);
            this.DeviceNameMoHuQuery.TabIndex = 5;
            this.DeviceNameMoHuQuery.Text = "查 询";
            this.DeviceNameMoHuQuery.UseVisualStyleBackColor = true;
            this.DeviceNameMoHuQuery.MouseLeave += new System.EventHandler(this.DeviceNameMoHuQuery_MouseLeave);
            this.DeviceNameMoHuQuery.Click += new System.EventHandler(this.DeviceNameMoHuQuery_Click);
            this.DeviceNameMoHuQuery.MouseEnter += new System.EventHandler(this.DeviceNameMoHuQuery_MouseEnter);
            // 
            // Device_MoHuName
            // 
            this.Device_MoHuName.AutoSize = true;
            this.Device_MoHuName.Location = new System.Drawing.Point(13, 11);
            this.Device_MoHuName.Name = "Device_MoHuName";
            this.Device_MoHuName.Size = new System.Drawing.Size(65, 12);
            this.Device_MoHuName.TabIndex = 3;
            this.Device_MoHuName.Text = "设备名称：";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.lbKeyWord);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.textBox_key);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button_search);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(777, 556);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "按地图要素查询";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbKeyWord
            // 
            this.lbKeyWord.AutoSize = true;
            this.lbKeyWord.ForeColor = System.Drawing.Color.Red;
            this.lbKeyWord.Location = new System.Drawing.Point(360, 9);
            this.lbKeyWord.Name = "lbKeyWord";
            this.lbKeyWord.Size = new System.Drawing.Size(0, 12);
            this.lbKeyWord.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lvMap);
            this.groupBox3.Location = new System.Drawing.Point(8, 34);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(761, 514);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "匹配到的地图要素";
            // 
            // lvMap
            // 
            this.lvMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMap.FullRowSelect = true;
            this.lvMap.Location = new System.Drawing.Point(3, 17);
            this.lvMap.MultiSelect = false;
            this.lvMap.Name = "lvMap";
            this.lvMap.Size = new System.Drawing.Size(755, 494);
            this.lvMap.TabIndex = 0;
            this.lvMap.UseCompatibleStateImageBehavior = false;
            this.lvMap.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvMap_MouseDoubleClick);
            this.lvMap.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvMap_ColumnClick);
            // 
            // textBox_key
            // 
            this.textBox_key.Location = new System.Drawing.Point(79, 7);
            this.textBox_key.Name = "textBox_key";
            this.textBox_key.Size = new System.Drawing.Size(187, 21);
            this.textBox_key.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "关键字：";
            // 
            // button_search
            // 
            this.button_search.Location = new System.Drawing.Point(272, 6);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(61, 23);
            this.button_search.TabIndex = 0;
            this.button_search.Text = "查 询";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // imageListQuery
            // 
            this.imageListQuery.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListQuery.ImageStream")));
            this.imageListQuery.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListQuery.Images.SetKeyName(0, "查询.png");
            this.imageListQuery.Images.SetKeyName(1, "查询经过.png");
            // 
            // Query
            // 
            this.AcceptButton = this.FIDQuery;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 581);
            this.Controls.Add(this.QuerytabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Query";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备查询";
            this.Load += new System.EventHandler(this.Query_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Query_FormClosing);
            this.Resize += new System.EventHandler(this.Query_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Query_KeyDown);
            this.QuerytabControl.ResumeLayout(false);
            this.tpFidQuery.ResumeLayout(false);
            this.tpFidQuery.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tpMcQuery.ResumeLayout(false);
            this.tpMcQuery.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl QuerytabControl;
        private System.Windows.Forms.TabPage tpFidQuery;
        private System.Windows.Forms.Label DeviceFID_label;
        private System.Windows.Forms.TextBox FID_textBox;
        private System.Windows.Forms.Button FIDQuery;
        private System.Windows.Forms.TabPage tpMcQuery;
        private System.Windows.Forms.Button DeviceNameMoHuQuery;
        private System.Windows.Forms.Label Device_MoHuName;
        private System.Windows.Forms.ImageList imageListQuery;
        private System.Windows.Forms.ToolTip toolTipQuery;
        private System.Windows.Forms.Label Fidlable;
        private System.Windows.Forms.Label DeviceMohuLable;
        private System.Windows.Forms.TextBox txtMohu;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_key;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvFidQuery;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvSMQuery;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lvMap;
        private System.Windows.Forms.Label lbKeyWord;
    }
}