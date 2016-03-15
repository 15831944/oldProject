namespace ElectronTransferView.FunctionManager
{
    partial class MoveSymbol
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_Move = new System.Windows.Forms.Button();
            this.Txt_Point = new System.Windows.Forms.TextBox();
            this.Btn_SelectSymbol = new System.Windows.Forms.Button();
            this.Btn_SelectPoint = new System.Windows.Forms.Button();
            this.Lal_SymbolInfo = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Lal_SymbolInfo);
            this.groupBox1.Controls.Add(this.Btn_SelectPoint);
            this.groupBox1.Controls.Add(this.Btn_SelectSymbol);
            this.groupBox1.Controls.Add(this.Txt_Point);
            this.groupBox1.Controls.Add(this.Btn_Move);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择需移动的设备：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "移动至：";
            // 
            // Btn_Move
            // 
            this.Btn_Move.Location = new System.Drawing.Point(239, 82);
            this.Btn_Move.Name = "Btn_Move";
            this.Btn_Move.Size = new System.Drawing.Size(75, 23);
            this.Btn_Move.TabIndex = 2;
            this.Btn_Move.Text = "移 动";
            this.Btn_Move.UseVisualStyleBackColor = true;
            this.Btn_Move.Click += new System.EventHandler(this.Btn_Move_Click);
            // 
            // Txt_Point
            // 
            this.Txt_Point.Location = new System.Drawing.Point(58, 51);
            this.Txt_Point.Name = "Txt_Point";
            this.Txt_Point.Size = new System.Drawing.Size(217, 21);
            this.Txt_Point.TabIndex = 3;
            // 
            // Btn_SelectSymbol
            // 
            this.Btn_SelectSymbol.Location = new System.Drawing.Point(281, 20);
            this.Btn_SelectSymbol.Name = "Btn_SelectSymbol";
            this.Btn_SelectSymbol.Size = new System.Drawing.Size(33, 23);
            this.Btn_SelectSymbol.TabIndex = 4;
            this.Btn_SelectSymbol.Text = "…";
            this.Btn_SelectSymbol.UseVisualStyleBackColor = true;
            this.Btn_SelectSymbol.Click += new System.EventHandler(this.Btn_SelectSymbol_Click);
            // 
            // Btn_SelectPoint
            // 
            this.Btn_SelectPoint.Location = new System.Drawing.Point(281, 50);
            this.Btn_SelectPoint.Name = "Btn_SelectPoint";
            this.Btn_SelectPoint.Size = new System.Drawing.Size(33, 23);
            this.Btn_SelectPoint.TabIndex = 6;
            this.Btn_SelectPoint.Text = "…";
            this.Btn_SelectPoint.UseVisualStyleBackColor = true;
            this.Btn_SelectPoint.Click += new System.EventHandler(this.Btn_SelectPoint_Click);
            // 
            // Lal_SymbolInfo
            // 
            this.Lal_SymbolInfo.AutoSize = true;
            this.Lal_SymbolInfo.Location = new System.Drawing.Point(125, 25);
            this.Lal_SymbolInfo.Name = "Lal_SymbolInfo";
            this.Lal_SymbolInfo.Size = new System.Drawing.Size(0, 12);
            this.Lal_SymbolInfo.TabIndex = 7;
            // 
            // MoveSymbol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 126);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MoveSymbol";
            this.Text = "移动设备";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_SelectSymbol;
        private System.Windows.Forms.TextBox Txt_Point;
        private System.Windows.Forms.Button Btn_Move;
        private System.Windows.Forms.Button Btn_SelectPoint;
        private System.Windows.Forms.Label Lal_SymbolInfo;
    }
}