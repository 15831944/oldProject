namespace ElectronTransferView.DataManager
{
    partial class CadDataSource
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CadDataSource));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Btn_Initialization = new System.Windows.Forms.Button();
            this.Btn_Select = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Txt_FilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Tool_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Btn_Initialization);
            this.groupBox1.Controls.Add(this.Btn_Select);
            this.groupBox1.Controls.Add(this.Txt_FilePath);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择数据源";
            // 
            // Btn_Initialization
            // 
            this.Btn_Initialization.Enabled = false;
            this.Btn_Initialization.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Initialization.Location = new System.Drawing.Point(251, 62);
            this.Btn_Initialization.Name = "Btn_Initialization";
            this.Btn_Initialization.Size = new System.Drawing.Size(77, 23);
            this.Btn_Initialization.TabIndex = 3;
            this.Btn_Initialization.Text = "加载数据源";
            this.Btn_Initialization.UseVisualStyleBackColor = true;
            this.Btn_Initialization.Click += new System.EventHandler(this.Btn_Initialization_Click);
            // 
            // Btn_Select
            // 
            this.Btn_Select.ImageList = this.imageList1;
            this.Btn_Select.Location = new System.Drawing.Point(298, 29);
            this.Btn_Select.Name = "Btn_Select";
            this.Btn_Select.Size = new System.Drawing.Size(30, 23);
            this.Btn_Select.TabIndex = 2;
            this.Btn_Select.Text = "…";
            this.Btn_Select.UseVisualStyleBackColor = true;
            this.Btn_Select.Click += new System.EventHandler(this.Btn_Select_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "确定经过.png");
            this.imageList1.Images.SetKeyName(1, "确定.png");
            this.imageList1.Images.SetKeyName(2, "选择经过.png");
            this.imageList1.Images.SetKeyName(3, "选择.png");
            // 
            // Txt_FilePath
            // 
            this.Txt_FilePath.Location = new System.Drawing.Point(54, 30);
            this.Txt_FilePath.Name = "Txt_FilePath";
            this.Txt_FilePath.ReadOnly = true;
            this.Txt_FilePath.Size = new System.Drawing.Size(238, 21);
            this.Txt_FilePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "路径：";
            // 
            // Tool_Status
            // 
            this.Tool_Status.Name = "Tool_Status";
            this.Tool_Status.Size = new System.Drawing.Size(0, 17);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Tool_Status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 102);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(336, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // CadDataSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 124);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CadDataSource";
            this.Text = "加载沿布图";
            this.Load += new System.EventHandler(this.CadDataSource_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Btn_Select;
        private System.Windows.Forms.TextBox Txt_FilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_Initialization;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel Tool_Status;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}