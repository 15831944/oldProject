namespace ElectronTransferView.SearchManager
{
    partial class CoordinateForm
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
            this.Txt_Point = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Txt_Point
            // 
            this.Txt_Point.Location = new System.Drawing.Point(0, 0);
            this.Txt_Point.Name = "Txt_Point";
            this.Txt_Point.ReadOnly = true;
            this.Txt_Point.Size = new System.Drawing.Size(317, 21);
            this.Txt_Point.TabIndex = 1;
            this.Txt_Point.MouseLeave += new System.EventHandler(this.Txt_Point_MouseLeave);
            this.Txt_Point.MouseEnter += new System.EventHandler(this.Txt_Point_MouseEnter);
            // 
            // CoordinateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 21);
            this.Controls.Add(this.Txt_Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoordinateForm";
            this.Text = "经纬度坐标";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Txt_Point;

    }
}