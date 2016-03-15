namespace ElectronTransferView.SearchManager
{
    partial class YHHSearch
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
            this.label1 = new System.Windows.Forms.Label();
            this.tx_SHBYDH = new System.Windows.Forms.TextBox();
            this.btn_SHBok = new System.Windows.Forms.Button();
            this.lb_SHBWarn = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SHBSearch = new System.Windows.Forms.TabPage();
            this.JLBSearch = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_JLBOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_JLBWarn = new System.Windows.Forms.Label();
            this.tx_JLBYHH = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SHBSearch.SuspendLayout();
            this.JLBSearch.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "用户号:";
            // 
            // tx_SHBYDH
            // 
            this.tx_SHBYDH.Location = new System.Drawing.Point(59, 25);
            this.tx_SHBYDH.Name = "tx_SHBYDH";
            this.tx_SHBYDH.Size = new System.Drawing.Size(167, 21);
            this.tx_SHBYDH.TabIndex = 1;
            // 
            // btn_SHBok
            // 
            this.btn_SHBok.Location = new System.Drawing.Point(234, 24);
            this.btn_SHBok.Name = "btn_SHBok";
            this.btn_SHBok.Size = new System.Drawing.Size(53, 23);
            this.btn_SHBok.TabIndex = 2;
            this.btn_SHBok.Text = "搜索";
            this.btn_SHBok.UseVisualStyleBackColor = true;
            this.btn_SHBok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // lb_SHBWarn
            // 
            this.lb_SHBWarn.AutoSize = true;
            this.lb_SHBWarn.Location = new System.Drawing.Point(10, 59);
            this.lb_SHBWarn.Name = "lb_SHBWarn";
            this.lb_SHBWarn.Size = new System.Drawing.Size(0, 12);
            this.lb_SHBWarn.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_SHBok);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lb_SHBWarn);
            this.groupBox1.Controls.Add(this.tx_SHBYDH);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 83);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SHBSearch);
            this.tabControl1.Controls.Add(this.JLBSearch);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(317, 128);
            this.tabControl1.TabIndex = 5;
            // 
            // SHBSearch
            // 
            this.SHBSearch.Controls.Add(this.groupBox1);
            this.SHBSearch.Location = new System.Drawing.Point(4, 21);
            this.SHBSearch.Name = "SHBSearch";
            this.SHBSearch.Padding = new System.Windows.Forms.Padding(3);
            this.SHBSearch.Size = new System.Drawing.Size(309, 103);
            this.SHBSearch.TabIndex = 0;
            this.SHBSearch.Text = "散户表搜索";
            this.SHBSearch.UseVisualStyleBackColor = true;
            // 
            // JLBSearch
            // 
            this.JLBSearch.Controls.Add(this.groupBox2);
            this.JLBSearch.Location = new System.Drawing.Point(4, 21);
            this.JLBSearch.Name = "JLBSearch";
            this.JLBSearch.Padding = new System.Windows.Forms.Padding(3);
            this.JLBSearch.Size = new System.Drawing.Size(309, 103);
            this.JLBSearch.TabIndex = 1;
            this.JLBSearch.Text = "计量表搜索";
            this.JLBSearch.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_JLBOK);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lb_JLBWarn);
            this.groupBox2.Controls.Add(this.tx_JLBYHH);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(295, 83);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "查询条件";
            // 
            // btn_JLBOK
            // 
            this.btn_JLBOK.Location = new System.Drawing.Point(234, 24);
            this.btn_JLBOK.Name = "btn_JLBOK";
            this.btn_JLBOK.Size = new System.Drawing.Size(53, 23);
            this.btn_JLBOK.TabIndex = 2;
            this.btn_JLBOK.Text = "搜索";
            this.btn_JLBOK.UseVisualStyleBackColor = true;
            this.btn_JLBOK.Click += new System.EventHandler(this.btn_JLBOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "用户号:";
            // 
            // lb_JLBWarn
            // 
            this.lb_JLBWarn.AutoSize = true;
            this.lb_JLBWarn.Location = new System.Drawing.Point(10, 59);
            this.lb_JLBWarn.Name = "lb_JLBWarn";
            this.lb_JLBWarn.Size = new System.Drawing.Size(0, 12);
            this.lb_JLBWarn.TabIndex = 3;
            // 
            // tx_JLBYHH
            // 
            this.tx_JLBYHH.Location = new System.Drawing.Point(59, 25);
            this.tx_JLBYHH.Name = "tx_JLBYHH";
            this.tx_JLBYHH.Size = new System.Drawing.Size(167, 21);
            this.tx_JLBYHH.TabIndex = 1;
            // 
            // YHHSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 151);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YHHSearch";
            this.Text = "户表/计量表搜索";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SHBSearch_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.SHBSearch.ResumeLayout(false);
            this.JLBSearch.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tx_SHBYDH;
        private System.Windows.Forms.Button btn_SHBok;
        private System.Windows.Forms.Label lb_SHBWarn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage SHBSearch;
        private System.Windows.Forms.TabPage JLBSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_JLBOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_JLBWarn;
        private System.Windows.Forms.TextBox tx_JLBYHH;
    }
}