namespace ElectronTransferView.ContextMenuManager
{
    partial class SHBMove
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
            this.btn_SHBmove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_SHBWarn = new System.Windows.Forms.Label();
            this.tx_SHBYDH = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_SHBmove);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lb_SHBWarn);
            this.groupBox1.Controls.Add(this.tx_SHBYDH);
            this.groupBox1.Location = new System.Drawing.Point(10, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(319, 83);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // btn_SHBmove
            // 
            this.btn_SHBmove.Location = new System.Drawing.Point(260, 25);
            this.btn_SHBmove.Name = "btn_SHBmove";
            this.btn_SHBmove.Size = new System.Drawing.Size(53, 23);
            this.btn_SHBmove.TabIndex = 2;
            this.btn_SHBmove.Text = "迁移";
            this.btn_SHBmove.UseVisualStyleBackColor = true;
            this.btn_SHBmove.Click += new System.EventHandler(this.btn_SHBmove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "客户协议号:";
            // 
            // lb_SHBWarn
            // 
            this.lb_SHBWarn.AutoSize = true;
            this.lb_SHBWarn.Location = new System.Drawing.Point(10, 59);
            this.lb_SHBWarn.Name = "lb_SHBWarn";
            this.lb_SHBWarn.Size = new System.Drawing.Size(0, 12);
            this.lb_SHBWarn.TabIndex = 3;
            // 
            // tx_SHBYDH
            // 
            this.tx_SHBYDH.Location = new System.Drawing.Point(83, 25);
            this.tx_SHBYDH.Name = "tx_SHBYDH";
            this.tx_SHBYDH.Size = new System.Drawing.Size(167, 21);
            this.tx_SHBYDH.TabIndex = 1;
            // 
            // SHBMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 109);
            this.Controls.Add(this.groupBox1);
            this.Name = "SHBMove";
            this.Text = "户表迁移";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_SHBmove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_SHBWarn;
        private System.Windows.Forms.TextBox tx_SHBYDH;
    }
}