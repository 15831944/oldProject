namespace ElectronTransferView.FunctionManager
{
    partial class XHGEManagerForm
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
            this.Btn_Add = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Txt_xhge = new System.Windows.Forms.TextBox();
            this.Lal_xhge = new System.Windows.Forms.Label();
            this.Combox_sblx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Btn_Add
            // 
            this.Btn_Add.Location = new System.Drawing.Point(203, 91);
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(75, 23);
            this.Btn_Add.TabIndex = 0;
            this.Btn_Add.Text = "新 增";
            this.Btn_Add.UseVisualStyleBackColor = true;
            this.Btn_Add.Click += new System.EventHandler(this.Btn_Add_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Txt_xhge);
            this.groupBox1.Controls.Add(this.Lal_xhge);
            this.groupBox1.Controls.Add(this.Combox_sblx);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Btn_Add);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 122);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // Txt_xhge
            // 
            this.Txt_xhge.Location = new System.Drawing.Point(82, 53);
            this.Txt_xhge.Name = "Txt_xhge";
            this.Txt_xhge.Size = new System.Drawing.Size(196, 21);
            this.Txt_xhge.TabIndex = 4;
            // 
            // Lal_xhge
            // 
            this.Lal_xhge.AutoSize = true;
            this.Lal_xhge.Location = new System.Drawing.Point(9, 56);
            this.Lal_xhge.Name = "Lal_xhge";
            this.Lal_xhge.Size = new System.Drawing.Size(65, 12);
            this.Lal_xhge.TabIndex = 3;
            this.Lal_xhge.Text = "型号规格：";
            // 
            // Combox_sblx
            // 
            this.Combox_sblx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Combox_sblx.FormattingEnabled = true;
            this.Combox_sblx.Location = new System.Drawing.Point(82, 24);
            this.Combox_sblx.Name = "Combox_sblx";
            this.Combox_sblx.Size = new System.Drawing.Size(196, 20);
            this.Combox_sblx.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "设备类型：";
            // 
            // XHGEManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 137);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XHGEManagerForm";
            this.Text = "新增型号规格";
            this.Load += new System.EventHandler(this.XHGEManagerForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_Add;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox Combox_sblx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Txt_xhge;
        private System.Windows.Forms.Label Lal_xhge;
    }
}