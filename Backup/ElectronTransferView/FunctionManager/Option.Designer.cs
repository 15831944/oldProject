namespace ElectronTransferView.FunctionManager
{
    partial class Option
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Check_Map = new System.Windows.Forms.CheckBox();
            this.noVerifyTz = new System.Windows.Forms.TextBox();
            this.noVerifyOwnship = new System.Windows.Forms.TextBox();
            this.noVerifyConnectivity = new System.Windows.Forms.TextBox();
            this.TzModify = new System.Windows.Forms.Button();
            this.OwnShipModify = new System.Windows.Forms.Button();
            this.ConnectivityModify = new System.Windows.Forms.Button();
            this.SbmcModify = new System.Windows.Forms.Button();
            this.noVerifySbmc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Btn_SelectTZServer = new System.Windows.Forms.Button();
            this.Txt_TzServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Btn_Save = new System.Windows.Forms.Button();
            this.Btn_SelectMapPath = new System.Windows.Forms.Button();
            this.Txt_Lables = new System.Windows.Forms.TextBox();
            this.Txt_MapPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Check_Map);
            this.groupBox1.Controls.Add(this.noVerifyTz);
            this.groupBox1.Controls.Add(this.noVerifyOwnship);
            this.groupBox1.Controls.Add(this.noVerifyConnectivity);
            this.groupBox1.Controls.Add(this.TzModify);
            this.groupBox1.Controls.Add(this.OwnShipModify);
            this.groupBox1.Controls.Add(this.ConnectivityModify);
            this.groupBox1.Controls.Add(this.SbmcModify);
            this.groupBox1.Controls.Add(this.noVerifySbmc);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Btn_SelectTZServer);
            this.groupBox1.Controls.Add(this.Txt_TzServer);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Btn_Save);
            this.groupBox1.Controls.Add(this.Btn_SelectMapPath);
            this.groupBox1.Controls.Add(this.Txt_Lables);
            this.groupBox1.Controls.Add(this.Txt_MapPath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(524, 381);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "配置";
            // 
            // Check_Map
            // 
            this.Check_Map.AutoSize = true;
            this.Check_Map.Location = new System.Drawing.Point(76, 27);
            this.Check_Map.Name = "Check_Map";
            this.Check_Map.Size = new System.Drawing.Size(72, 16);
            this.Check_Map.TabIndex = 22;
            this.Check_Map.Text = "地图浏览";
            this.Check_Map.UseVisualStyleBackColor = true;
            this.Check_Map.CheckedChanged += new System.EventHandler(this.Check_Map_CheckedChanged);
            // 
            // noVerifyTz
            // 
            this.noVerifyTz.Location = new System.Drawing.Point(155, 316);
            this.noVerifyTz.Name = "noVerifyTz";
            this.noVerifyTz.ReadOnly = true;
            this.noVerifyTz.Size = new System.Drawing.Size(301, 21);
            this.noVerifyTz.TabIndex = 21;
            // 
            // noVerifyOwnship
            // 
            this.noVerifyOwnship.Location = new System.Drawing.Point(155, 283);
            this.noVerifyOwnship.Name = "noVerifyOwnship";
            this.noVerifyOwnship.ReadOnly = true;
            this.noVerifyOwnship.Size = new System.Drawing.Size(301, 21);
            this.noVerifyOwnship.TabIndex = 20;
            // 
            // noVerifyConnectivity
            // 
            this.noVerifyConnectivity.Location = new System.Drawing.Point(155, 251);
            this.noVerifyConnectivity.Name = "noVerifyConnectivity";
            this.noVerifyConnectivity.ReadOnly = true;
            this.noVerifyConnectivity.Size = new System.Drawing.Size(301, 21);
            this.noVerifyConnectivity.TabIndex = 19;
            // 
            // TzModify
            // 
            this.TzModify.Location = new System.Drawing.Point(464, 314);
            this.TzModify.Name = "TzModify";
            this.TzModify.Size = new System.Drawing.Size(47, 23);
            this.TzModify.TabIndex = 18;
            this.TzModify.Text = "编辑";
            this.TzModify.UseVisualStyleBackColor = true;
            this.TzModify.Click += new System.EventHandler(this.Modify_Click);
            // 
            // OwnShipModify
            // 
            this.OwnShipModify.Location = new System.Drawing.Point(464, 282);
            this.OwnShipModify.Name = "OwnShipModify";
            this.OwnShipModify.Size = new System.Drawing.Size(47, 23);
            this.OwnShipModify.TabIndex = 17;
            this.OwnShipModify.Text = "编辑";
            this.OwnShipModify.UseVisualStyleBackColor = true;
            this.OwnShipModify.Click += new System.EventHandler(this.Modify_Click);
            // 
            // ConnectivityModify
            // 
            this.ConnectivityModify.Location = new System.Drawing.Point(464, 250);
            this.ConnectivityModify.Name = "ConnectivityModify";
            this.ConnectivityModify.Size = new System.Drawing.Size(47, 23);
            this.ConnectivityModify.TabIndex = 16;
            this.ConnectivityModify.Text = "编辑";
            this.ConnectivityModify.UseVisualStyleBackColor = true;
            this.ConnectivityModify.Click += new System.EventHandler(this.Modify_Click);
            // 
            // SbmcModify
            // 
            this.SbmcModify.Location = new System.Drawing.Point(464, 220);
            this.SbmcModify.Name = "SbmcModify";
            this.SbmcModify.Size = new System.Drawing.Size(47, 23);
            this.SbmcModify.TabIndex = 15;
            this.SbmcModify.Text = "编辑";
            this.SbmcModify.UseVisualStyleBackColor = true;
            this.SbmcModify.Click += new System.EventHandler(this.Modify_Click);
            // 
            // noVerifySbmc
            // 
            this.noVerifySbmc.Location = new System.Drawing.Point(155, 221);
            this.noVerifySbmc.Name = "noVerifySbmc";
            this.noVerifySbmc.ReadOnly = true;
            this.noVerifySbmc.Size = new System.Drawing.Size(301, 21);
            this.noVerifySbmc.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 320);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "取消台账校验：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 287);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "取消从属关系校验：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 255);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "取消连接关系校验：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "取消设备名称重复校验：";
            // 
            // Btn_SelectTZServer
            // 
            this.Btn_SelectTZServer.Location = new System.Drawing.Point(464, 82);
            this.Btn_SelectTZServer.Name = "Btn_SelectTZServer";
            this.Btn_SelectTZServer.Size = new System.Drawing.Size(47, 23);
            this.Btn_SelectTZServer.TabIndex = 9;
            this.Btn_SelectTZServer.Text = "…";
            this.Btn_SelectTZServer.UseVisualStyleBackColor = true;
            this.Btn_SelectTZServer.Click += new System.EventHandler(this.Btn_SelectTZServer_Click);
            // 
            // Txt_TzServer
            // 
            this.Txt_TzServer.Location = new System.Drawing.Point(76, 83);
            this.Txt_TzServer.Name = "Txt_TzServer";
            this.Txt_TzServer.ReadOnly = true;
            this.Txt_TzServer.Size = new System.Drawing.Size(380, 21);
            this.Txt_TzServer.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "台帐服务:";
            // 
            // Btn_Save
            // 
            this.Btn_Save.Location = new System.Drawing.Point(436, 352);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(75, 23);
            this.Btn_Save.TabIndex = 6;
            this.Btn_Save.Text = "保 存";
            this.Btn_Save.UseVisualStyleBackColor = true;
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // Btn_SelectMapPath
            // 
            this.Btn_SelectMapPath.Location = new System.Drawing.Point(464, 50);
            this.Btn_SelectMapPath.Name = "Btn_SelectMapPath";
            this.Btn_SelectMapPath.Size = new System.Drawing.Size(47, 23);
            this.Btn_SelectMapPath.TabIndex = 4;
            this.Btn_SelectMapPath.Text = "…";
            this.Btn_SelectMapPath.UseVisualStyleBackColor = true;
            this.Btn_SelectMapPath.Click += new System.EventHandler(this.Btn_SelectMapPath_Click);
            // 
            // Txt_Lables
            // 
            this.Txt_Lables.Location = new System.Drawing.Point(76, 114);
            this.Txt_Lables.Multiline = true;
            this.Txt_Lables.Name = "Txt_Lables";
            this.Txt_Lables.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Txt_Lables.Size = new System.Drawing.Size(380, 87);
            this.Txt_Lables.TabIndex = 3;
            // 
            // Txt_MapPath
            // 
            this.Txt_MapPath.Location = new System.Drawing.Point(76, 52);
            this.Txt_MapPath.Name = "Txt_MapPath";
            this.Txt_MapPath.Size = new System.Drawing.Size(380, 21);
            this.Txt_MapPath.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "标注显示：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "地图路径：";
            // 
            // Option
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 393);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Option";
            this.Text = "选项";
            this.Load += new System.EventHandler(this.Option_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Btn_SelectMapPath;
        private System.Windows.Forms.TextBox Txt_Lables;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_Save;
        private System.Windows.Forms.TextBox Txt_MapPath;
        private System.Windows.Forms.Button Btn_SelectTZServer;
        private System.Windows.Forms.TextBox Txt_TzServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox noVerifyTz;
        private System.Windows.Forms.TextBox noVerifyOwnship;
        private System.Windows.Forms.TextBox noVerifyConnectivity;
        private System.Windows.Forms.Button TzModify;
        private System.Windows.Forms.Button OwnShipModify;
        private System.Windows.Forms.Button ConnectivityModify;
        private System.Windows.Forms.Button SbmcModify;
        private System.Windows.Forms.TextBox noVerifySbmc;
        private System.Windows.Forms.CheckBox Check_Map;
    }
}