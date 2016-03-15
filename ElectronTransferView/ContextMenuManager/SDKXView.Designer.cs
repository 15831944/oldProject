namespace ElectronTransferView.ContextMenuManager
{
    partial class SDKXView
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
            this.button_delete = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_changeEnts = new System.Windows.Forms.Button();
            this.richTextBox_FID = new System.Windows.Forms.RichTextBox();
            this.GBox_Option = new System.Windows.Forms.GroupBox();
            this.radioButton_clear = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton_chexiao = new System.Windows.Forms.RadioButton();
            this.radioButton_add = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox_sc_kxmc = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_sc_bdz = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBox_xg_kxmc = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_xg_bdz = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.GBox_Option.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(268, 284);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(69, 23);
            this.button_delete.TabIndex = 0;
            this.button_delete.Text = "新 增";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(349, 284);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(69, 23);
            this.button_clear.TabIndex = 2;
            this.button_clear.Text = "清 空";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button_changeEnts);
            this.groupBox1.Controls.Add(this.richTextBox_FID);
            this.groupBox1.Controls.Add(this.GBox_Option);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.button_delete);
            this.groupBox1.Controls.Add(this.button_clear);
            this.groupBox1.Location = new System.Drawing.Point(7, -7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 322);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "在图中选取的电力设备：";
            // 
            // button_changeEnts
            // 
            this.button_changeEnts.Location = new System.Drawing.Point(349, 22);
            this.button_changeEnts.Name = "button_changeEnts";
            this.button_changeEnts.Size = new System.Drawing.Size(65, 23);
            this.button_changeEnts.TabIndex = 7;
            this.button_changeEnts.Text = "选择设备";
            this.button_changeEnts.UseVisualStyleBackColor = true;
            this.button_changeEnts.Click += new System.EventHandler(this.button_changeEnts_Click);
            // 
            // richTextBox_FID
            // 
            this.richTextBox_FID.Location = new System.Drawing.Point(10, 75);
            this.richTextBox_FID.Name = "richTextBox_FID";
            this.richTextBox_FID.ReadOnly = true;
            this.richTextBox_FID.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox_FID.Size = new System.Drawing.Size(413, 80);
            this.richTextBox_FID.TabIndex = 6;
            this.richTextBox_FID.Text = "";
            // 
            // GBox_Option
            // 
            this.GBox_Option.Controls.Add(this.radioButton_clear);
            this.GBox_Option.Controls.Add(this.label5);
            this.GBox_Option.Controls.Add(this.radioButton_chexiao);
            this.GBox_Option.Controls.Add(this.radioButton_add);
            this.GBox_Option.Location = new System.Drawing.Point(8, 9);
            this.GBox_Option.Name = "GBox_Option";
            this.GBox_Option.Size = new System.Drawing.Size(333, 40);
            this.GBox_Option.TabIndex = 5;
            this.GBox_Option.TabStop = false;
            // 
            // radioButton_clear
            // 
            this.radioButton_clear.AutoSize = true;
            this.radioButton_clear.Location = new System.Drawing.Point(259, 16);
            this.radioButton_clear.Name = "radioButton_clear";
            this.radioButton_clear.Size = new System.Drawing.Size(47, 16);
            this.radioButton_clear.TabIndex = 10;
            this.radioButton_clear.Tag = "del";
            this.radioButton_clear.Text = "删除";
            this.radioButton_clear.UseVisualStyleBackColor = true;
            this.radioButton_clear.CheckedChanged += new System.EventHandler(this.radioButton_clear_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "修改馈线类型：";
            // 
            // radioButton_chexiao
            // 
            this.radioButton_chexiao.AutoSize = true;
            this.radioButton_chexiao.Location = new System.Drawing.Point(183, 16);
            this.radioButton_chexiao.Name = "radioButton_chexiao";
            this.radioButton_chexiao.Size = new System.Drawing.Size(47, 16);
            this.radioButton_chexiao.TabIndex = 9;
            this.radioButton_chexiao.Tag = "update";
            this.radioButton_chexiao.Text = "修改";
            this.radioButton_chexiao.UseVisualStyleBackColor = true;
            this.radioButton_chexiao.CheckedChanged += new System.EventHandler(this.radioButton_chexiao_CheckedChanged);
            // 
            // radioButton_add
            // 
            this.radioButton_add.AutoSize = true;
            this.radioButton_add.Checked = true;
            this.radioButton_add.Location = new System.Drawing.Point(107, 16);
            this.radioButton_add.Name = "radioButton_add";
            this.radioButton_add.Size = new System.Drawing.Size(47, 16);
            this.radioButton_add.TabIndex = 8;
            this.radioButton_add.TabStop = true;
            this.radioButton_add.Tag = "add";
            this.radioButton_add.Text = "新增";
            this.radioButton_add.UseVisualStyleBackColor = true;
            this.radioButton_add.CheckedChanged += new System.EventHandler(this.radioButton_add_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox_sc_kxmc);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.comboBox_sc_bdz);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(7, 160);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(416, 51);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "馈线集合";
            // 
            // comboBox_sc_kxmc
            // 
            this.comboBox_sc_kxmc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_sc_kxmc.FormattingEnabled = true;
            this.comboBox_sc_kxmc.Location = new System.Drawing.Point(241, 21);
            this.comboBox_sc_kxmc.MaxDropDownItems = 10;
            this.comboBox_sc_kxmc.Name = "comboBox_sc_kxmc";
            this.comboBox_sc_kxmc.Size = new System.Drawing.Size(167, 20);
            this.comboBox_sc_kxmc.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(182, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "馈线名称：";
            // 
            // comboBox_sc_bdz
            // 
            this.comboBox_sc_bdz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_sc_bdz.FormattingEnabled = true;
            this.comboBox_sc_bdz.Location = new System.Drawing.Point(58, 21);
            this.comboBox_sc_bdz.MaxDropDownItems = 10;
            this.comboBox_sc_bdz.Name = "comboBox_sc_bdz";
            this.comboBox_sc_bdz.Size = new System.Drawing.Size(121, 20);
            this.comboBox_sc_bdz.TabIndex = 5;
            this.comboBox_sc_bdz.SelectedIndexChanged += new System.EventHandler(this.comboBox_sc_bdz_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "变电站：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBox_xg_kxmc);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBox_xg_bdz);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(7, 219);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(416, 51);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "新增";
            // 
            // comboBox_xg_kxmc
            // 
            this.comboBox_xg_kxmc.FormattingEnabled = true;
            this.comboBox_xg_kxmc.Location = new System.Drawing.Point(240, 19);
            this.comboBox_xg_kxmc.MaxDropDownItems = 10;
            this.comboBox_xg_kxmc.Name = "comboBox_xg_kxmc";
            this.comboBox_xg_kxmc.Size = new System.Drawing.Size(167, 20);
            this.comboBox_xg_kxmc.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(182, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "馈线名称：";
            // 
            // comboBox_xg_bdz
            // 
            this.comboBox_xg_bdz.FormattingEnabled = true;
            this.comboBox_xg_bdz.Location = new System.Drawing.Point(58, 19);
            this.comboBox_xg_bdz.MaxDropDownItems = 10;
            this.comboBox_xg_bdz.Name = "comboBox_xg_bdz";
            this.comboBox_xg_bdz.Size = new System.Drawing.Size(121, 20);
            this.comboBox_xg_bdz.TabIndex = 1;
            this.comboBox_xg_bdz.SelectedIndexChanged += new System.EventHandler(this.comboBox_xg_bdz_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "变电站：";
            // 
            // SDKXView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(442, 318);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SDKXView";
            this.Text = "批量修改受电馈线";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.GBox_Option.ResumeLayout(false);
            this.GBox_Option.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBox_xg_bdz;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GBox_Option;
        private System.Windows.Forms.ComboBox comboBox_sc_kxmc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_sc_bdz;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_xg_kxmc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richTextBox_FID;
        private System.Windows.Forms.RadioButton radioButton_clear;
        private System.Windows.Forms.RadioButton radioButton_chexiao;
        private System.Windows.Forms.RadioButton radioButton_add;
        private System.Windows.Forms.Button button_changeEnts;
        private System.Windows.Forms.Label label6;
    }
}