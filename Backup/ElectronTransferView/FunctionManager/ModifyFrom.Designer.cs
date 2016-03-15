namespace ElectronTransferView.FunctionManager
{
    partial class ModifyFrom
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
            this.lbAllFeature = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbFilterFeature = new System.Windows.Forms.ListBox();
            this.btnFinish = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnReModify = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbAllFeature);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(160, 334);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设备集";
            // 
            // lbAllFeature
            // 
            this.lbAllFeature.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAllFeature.FormattingEnabled = true;
            this.lbAllFeature.ItemHeight = 12;
            this.lbAllFeature.Location = new System.Drawing.Point(3, 17);
            this.lbAllFeature.Name = "lbAllFeature";
            this.lbAllFeature.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbAllFeature.Size = new System.Drawing.Size(154, 304);
            this.lbAllFeature.TabIndex = 0;
            this.lbAllFeature.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbAllFeature_KeyDown);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbFilterFeature);
            this.groupBox2.Location = new System.Drawing.Point(230, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(160, 334);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "过滤项";
            // 
            // lbFilterFeature
            // 
            this.lbFilterFeature.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFilterFeature.FormattingEnabled = true;
            this.lbFilterFeature.ItemHeight = 12;
            this.lbFilterFeature.Location = new System.Drawing.Point(3, 17);
            this.lbFilterFeature.Name = "lbFilterFeature";
            this.lbFilterFeature.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lbFilterFeature.Size = new System.Drawing.Size(154, 304);
            this.lbFilterFeature.TabIndex = 0;
            this.lbFilterFeature.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbFilterFeature_KeyDown);
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(315, 360);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 2;
            this.btnFinish.Text = "完成";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(178, 119);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(46, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "--->";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(178, 194);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(46, 23);
            this.btnMove.TabIndex = 4;
            this.btnMove.Text = "<---";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // btnReModify
            // 
            this.btnReModify.Location = new System.Drawing.Point(15, 360);
            this.btnReModify.Name = "btnReModify";
            this.btnReModify.Size = new System.Drawing.Size(75, 23);
            this.btnReModify.TabIndex = 5;
            this.btnReModify.Text = "恢复默认值";
            this.btnReModify.UseVisualStyleBackColor = true;
            this.btnReModify.Click += new System.EventHandler(this.btnReModify_Click);
            // 
            // ModifyFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 395);
            this.Controls.Add(this.btnReModify);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModifyFrom";
            this.Text = "校验过滤项编辑";
            this.Load += new System.EventHandler(this.ModifyFrom_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lbAllFeature;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.ListBox lbFilterFeature;
        private System.Windows.Forms.Button btnReModify;
    }
}