namespace ElectronTransferView.SyncTZDataToGIS
{
    partial class SyncTzDataToCAD
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btSync = new System.Windows.Forms.Button();
            this.lbShowSyncData = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btSync
            // 
            this.btSync.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btSync.Location = new System.Drawing.Point(4, 13);
            this.btSync.Name = "btSync";
            this.btSync.Size = new System.Drawing.Size(385, 23);
            this.btSync.TabIndex = 0;
            this.btSync.Text = "开始同步";
            this.btSync.UseVisualStyleBackColor = true;
            this.btSync.Click += new System.EventHandler(this.btSync_Click);
            // 
            // lbShowSyncData
            // 
            this.lbShowSyncData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbShowSyncData.FormattingEnabled = true;
            this.lbShowSyncData.ItemHeight = 12;
            this.lbShowSyncData.Location = new System.Drawing.Point(4, 42);
            this.lbShowSyncData.Name = "lbShowSyncData";
            this.lbShowSyncData.Size = new System.Drawing.Size(385, 328);
            this.lbShowSyncData.TabIndex = 1;
            this.lbShowSyncData.SelectedIndexChanged += new System.EventHandler(this.lbShowSyncData_SelectedIndexChanged);
            // 
            // SyncTzDataToCAD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbShowSyncData);
            this.Controls.Add(this.btSync);
            this.Name = "SyncTzDataToCAD";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(393, 379);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btSync;
        private System.Windows.Forms.ListBox lbShowSyncData;
    }
}
