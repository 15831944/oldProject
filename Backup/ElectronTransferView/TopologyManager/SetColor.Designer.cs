namespace ElectronTransferView.TopologyManager
{
    partial class SetColor
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
            this.curColor = new System.Windows.Forms.Button();
            this.selColor = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前颜色:";
            // 
            // curColor
            // 
            this.curColor.BackColor = System.Drawing.SystemColors.Control;
            this.curColor.Enabled = false;
            this.curColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.curColor.Location = new System.Drawing.Point(87, 16);
            this.curColor.Name = "curColor";
            this.curColor.Size = new System.Drawing.Size(31, 14);
            this.curColor.TabIndex = 1;
            this.curColor.TabStop = false;
            this.curColor.UseVisualStyleBackColor = false;
            // 
            // selColor
            // 
            this.selColor.Location = new System.Drawing.Point(138, 12);
            this.selColor.Name = "selColor";
            this.selColor.Size = new System.Drawing.Size(50, 20);
            this.selColor.TabIndex = 2;
            this.selColor.Text = "选择";
            this.selColor.UseVisualStyleBackColor = true;
            this.selColor.Click += new System.EventHandler(this.selColor_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(139, 38);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(49, 20);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // SetColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(210, 63);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.selColor);
            this.Controls.Add(this.curColor);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetColor";
            this.Text = "追踪颜色设置";
            this.Load += new System.EventHandler(this.SetColor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button curColor;
        private System.Windows.Forms.Button selColor;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ColorDialog colorDialog1;

    }
}