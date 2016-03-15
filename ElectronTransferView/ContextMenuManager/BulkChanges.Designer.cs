namespace ElectronTransferView.ContextMenuManager
{
    partial class BulkChanges
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BulkChanges));
            this.CmbBulkChange = new System.Windows.Forms.ComboBox();
            this.pGridBulkChange = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.btRef = new System.Windows.Forms.Button();
            this.imageListBulk = new System.Windows.Forms.ImageList(this.components);
            this.toolTipBulk = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // CmbBulkChange
            // 
            this.CmbBulkChange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbBulkChange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbBulkChange.FormattingEnabled = true;
            this.CmbBulkChange.Location = new System.Drawing.Point(106, 12);
            this.CmbBulkChange.MaxDropDownItems = 100;
            this.CmbBulkChange.Name = "CmbBulkChange";
            this.CmbBulkChange.Size = new System.Drawing.Size(191, 20);
            this.CmbBulkChange.TabIndex = 0;
            this.CmbBulkChange.SelectedIndexChanged += new System.EventHandler(this.CmbBulkChange_SelectedIndexChanged);
            // 
            // pGridBulkChange
            // 
            this.pGridBulkChange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pGridBulkChange.HelpVisible = false;
            this.pGridBulkChange.Location = new System.Drawing.Point(0, 38);
            this.pGridBulkChange.Name = "pGridBulkChange";
            this.pGridBulkChange.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.pGridBulkChange.Size = new System.Drawing.Size(373, 416);
            this.pGridBulkChange.TabIndex = 1;
            this.pGridBulkChange.ToolbarVisible = false;
            this.pGridBulkChange.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pGridBulkChange_PropertyValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请选择设备类别：";
            // 
            // btRef
            // 
            this.btRef.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRef.Location = new System.Drawing.Point(308, 11);
            this.btRef.Name = "btRef";
            this.btRef.Size = new System.Drawing.Size(58, 23);
            this.btRef.TabIndex = 3;
            this.btRef.Text = "更 新";
            this.btRef.UseVisualStyleBackColor = true;
            this.btRef.MouseLeave += new System.EventHandler(this.btRef_MouseLeave);
            this.btRef.Click += new System.EventHandler(this.btRef_Click);
            this.btRef.MouseEnter += new System.EventHandler(this.btRef_MouseEnter);
            // 
            // imageListBulk
            // 
            this.imageListBulk.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBulk.ImageStream")));
            this.imageListBulk.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBulk.Images.SetKeyName(0, "更新.png");
            this.imageListBulk.Images.SetKeyName(1, "更新经过.png");
            // 
            // BulkChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btRef);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pGridBulkChange);
            this.Controls.Add(this.CmbBulkChange);
            this.Name = "BulkChanges";
            this.Size = new System.Drawing.Size(373, 454);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CmbBulkChange;
        private System.Windows.Forms.PropertyGrid pGridBulkChange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btRef;
        private System.Windows.Forms.ImageList imageListBulk;
        private System.Windows.Forms.ToolTip toolTipBulk;
    }
}
