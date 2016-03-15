namespace ElectronTransferView.FunctionManager
{
    partial class AnalysisManager
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
            this.Btn_Analysis = new System.Windows.Forms.Button();
            this.Btn_ClearColor = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Btn_ClearColor);
            this.groupBox1.Controls.Add(this.Btn_Analysis);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 165);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "统计";
            // 
            // Btn_Analysis
            // 
            this.Btn_Analysis.Location = new System.Drawing.Point(64, 20);
            this.Btn_Analysis.Name = "Btn_Analysis";
            this.Btn_Analysis.Size = new System.Drawing.Size(95, 23);
            this.Btn_Analysis.TabIndex = 0;
            this.Btn_Analysis.Text = "查看编辑设备";
            this.Btn_Analysis.UseVisualStyleBackColor = true;
            this.Btn_Analysis.Click += new System.EventHandler(this.Btn_Analysis_Click);
            // 
            // Btn_ClearColor
            // 
            this.Btn_ClearColor.Location = new System.Drawing.Point(235, 19);
            this.Btn_ClearColor.Name = "Btn_ClearColor";
            this.Btn_ClearColor.Size = new System.Drawing.Size(75, 23);
            this.Btn_ClearColor.TabIndex = 1;
            this.Btn_ClearColor.Text = "还原设备";
            this.Btn_ClearColor.UseVisualStyleBackColor = true;
            this.Btn_ClearColor.Click += new System.EventHandler(this.Btn_ClearColor_Click);
            // 
            // AnalysisManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 189);
            this.Controls.Add(this.groupBox1);
            this.Name = "AnalysisManager";
            this.Text = "AnalysisManager";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Btn_ClearColor;
        private System.Windows.Forms.Button Btn_Analysis;
    }
}