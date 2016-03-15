namespace ElectronTransferView.ContextMenuManager
{
    partial class DevAttribute
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
            this.pgDev = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // pgDev
            // 
            this.pgDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgDev.HelpVisible = false;
            this.pgDev.Location = new System.Drawing.Point(0, 0);
            this.pgDev.Name = "pgDev";
            this.pgDev.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pgDev.Size = new System.Drawing.Size(364, 510);
            this.pgDev.TabIndex = 0;
            this.pgDev.ToolbarVisible = false;
            this.pgDev.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgDev_PropertyValueChanged);
            // 
            // DevAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pgDev);
            this.Name = "DevAttribute";
            this.Size = new System.Drawing.Size(364, 510);
            this.VisibleChanged += new System.EventHandler(this.DevAttribute_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgDev;
    }
}
