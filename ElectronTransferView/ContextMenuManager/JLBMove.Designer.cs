namespace ElectronTransferView.ContextMenuManager
{
    partial class JLBMove
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
            this.btn_JLBmove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_JLBWarn = new System.Windows.Forms.Label();
            this.tx_JLBYDH = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_JLBmove);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lb_JLBWarn);
            this.groupBox1.Controls.Add(this.tx_JLBYDH);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 83);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // btn_JLBmove
            // 
            this.btn_JLBmove.Location = new System.Drawing.Point(234, 24);
            this.btn_JLBmove.Name = "btn_JLBmove";
            this.btn_JLBmove.Size = new System.Drawing.Size(53, 23);
            this.btn_JLBmove.TabIndex = 2;
            this.btn_JLBmove.Text = "迁移";
            this.btn_JLBmove.UseVisualStyleBackColor = true;
            this.btn_JLBmove.Click += new System.EventHandler(this.btn_JLBmove_Click);
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
            // lb_JLBWarn
            // 
            this.lb_JLBWarn.AutoSize = true;
            this.lb_JLBWarn.Location = new System.Drawing.Point(10, 59);
            this.lb_JLBWarn.Name = "lb_JLBWarn";
            this.lb_JLBWarn.Size = new System.Drawing.Size(0, 12);
            this.lb_JLBWarn.TabIndex = 3;
            // 
            // tx_JLBYDH
            // 
            this.tx_JLBYDH.Location = new System.Drawing.Point(59, 25);
            this.tx_JLBYDH.Name = "tx_JLBYDH";
            this.tx_JLBYDH.Size = new System.Drawing.Size(167, 21);
            this.tx_JLBYDH.TabIndex = 1;
            // 
            // JLBMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 103);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "JLBMove";
            this.Text = "计量表迁移";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_JLBmove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_JLBWarn;
        private System.Windows.Forms.TextBox tx_JLBYDH;
    }
}