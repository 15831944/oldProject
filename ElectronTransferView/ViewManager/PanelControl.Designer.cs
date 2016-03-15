namespace ElectronTransferView.ViewManager
{
    partial class PanelControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolSM_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolSM_PreView = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolSM_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.DevNameGroup = new System.Windows.Forms.GroupBox();
            this.Tab_gnwz = new System.Windows.Forms.TabControl();
            this.tabPage_GNWZ = new System.Windows.Forms.TabPage();
            this.DevPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabPage_SDKX = new System.Windows.Forms.TabPage();
            this.DGV_SDKX = new System.Windows.Forms.DataGridView();
            this.GDBDZ = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.KXH = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.G3E_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.DevNameGroup.SuspendLayout();
            this.Tab_gnwz.SuspendLayout();
            this.tabPage_GNWZ.SuspendLayout();
            this.tabPage_SDKX.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_SDKX)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(301, 345);
            this.panel1.TabIndex = 3;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 124);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(301, 221);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            this.listView1.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.listView1_ItemMouseHover);
            this.listView1.MouseLeave += new System.EventHandler(this.listView1_MouseLeave);
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(38, 38);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolSM_Refresh,
            this.ToolSM_PreView,
            this.ToolSM_Delete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 70);
            // 
            // ToolSM_Refresh
            // 
            this.ToolSM_Refresh.Name = "ToolSM_Refresh";
            this.ToolSM_Refresh.Size = new System.Drawing.Size(112, 22);
            this.ToolSM_Refresh.Text = "刷新(&R)";
            this.ToolSM_Refresh.Visible = false;
            this.ToolSM_Refresh.Click += new System.EventHandler(this.ToolSM_Refresh_Click);
            // 
            // ToolSM_PreView
            // 
            this.ToolSM_PreView.Name = "ToolSM_PreView";
            this.ToolSM_PreView.Size = new System.Drawing.Size(112, 22);
            this.ToolSM_PreView.Text = "预览(&P)";
            this.ToolSM_PreView.Click += new System.EventHandler(this.ToolSM_PreView_Click);
            // 
            // ToolSM_Delete
            // 
            this.ToolSM_Delete.Name = "ToolSM_Delete";
            this.ToolSM_Delete.Size = new System.Drawing.Size(112, 22);
            this.ToolSM_Delete.Text = "删除(&D)";
            this.ToolSM_Delete.Click += new System.EventHandler(this.ToolSM_Delete_Click);
            // 
            // DevNameGroup
            // 
            this.DevNameGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DevNameGroup.Controls.Add(this.Tab_gnwz);
            this.DevNameGroup.Location = new System.Drawing.Point(6, 3);
            this.DevNameGroup.Name = "DevNameGroup";
            this.DevNameGroup.Size = new System.Drawing.Size(296, 364);
            this.DevNameGroup.TabIndex = 1;
            this.DevNameGroup.TabStop = false;
            this.DevNameGroup.Text = "属性";
            // 
            // Tab_gnwz
            // 
            this.Tab_gnwz.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tab_gnwz.Controls.Add(this.tabPage_GNWZ);
            this.Tab_gnwz.Controls.Add(this.tabPage_SDKX);
            this.Tab_gnwz.Location = new System.Drawing.Point(3, 17);
            this.Tab_gnwz.Name = "Tab_gnwz";
            this.Tab_gnwz.SelectedIndex = 0;
            this.Tab_gnwz.Size = new System.Drawing.Size(291, 344);
            this.Tab_gnwz.TabIndex = 1;
            // 
            // tabPage_GNWZ
            // 
            this.tabPage_GNWZ.Controls.Add(this.DevPropertyGrid);
            this.tabPage_GNWZ.Location = new System.Drawing.Point(4, 21);
            this.tabPage_GNWZ.Name = "tabPage_GNWZ";
            this.tabPage_GNWZ.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_GNWZ.Size = new System.Drawing.Size(283, 319);
            this.tabPage_GNWZ.TabIndex = 0;
            this.tabPage_GNWZ.Text = "功能位置";
            this.tabPage_GNWZ.UseVisualStyleBackColor = true;
            // 
            // DevPropertyGrid
            // 
            this.DevPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DevPropertyGrid.HelpVisible = false;
            this.DevPropertyGrid.Location = new System.Drawing.Point(-1, 0);
            this.DevPropertyGrid.Name = "DevPropertyGrid";
            this.DevPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.DevPropertyGrid.Size = new System.Drawing.Size(287, 319);
            this.DevPropertyGrid.TabIndex = 0;
            this.DevPropertyGrid.ToolbarVisible = false;
            this.DevPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.DevPropertyGrid_PropertyValueChanged);
            // 
            // tabPage_SDKX
            // 
            this.tabPage_SDKX.Controls.Add(this.DGV_SDKX);
            this.tabPage_SDKX.Location = new System.Drawing.Point(4, 21);
            this.tabPage_SDKX.Name = "tabPage_SDKX";
            this.tabPage_SDKX.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_SDKX.Size = new System.Drawing.Size(283, 319);
            this.tabPage_SDKX.TabIndex = 1;
            this.tabPage_SDKX.Text = "受电馈线";
            this.tabPage_SDKX.UseVisualStyleBackColor = true;
            // 
            // DGV_SDKX
            // 
            this.DGV_SDKX.BackgroundColor = System.Drawing.Color.White;
            this.DGV_SDKX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_SDKX.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GDBDZ,
            this.KXH,
            this.G3E_ID});
            this.DGV_SDKX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV_SDKX.Location = new System.Drawing.Point(3, 3);
            this.DGV_SDKX.Name = "DGV_SDKX";
            this.DGV_SDKX.RowTemplate.Height = 23;
            this.DGV_SDKX.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.DGV_SDKX.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_SDKX.Size = new System.Drawing.Size(277, 313);
            this.DGV_SDKX.TabIndex = 4;
            this.DGV_SDKX.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_SDKX_CellValueChanged);
            this.DGV_SDKX.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.DGV_SDKX_UserDeletingRow);
            this.DGV_SDKX.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_SDKX_RowValidated);
            this.DGV_SDKX.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_SDKX_CellClick);
            this.DGV_SDKX.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_SDKX_DataError);
            // 
            // GDBDZ
            // 
            this.GDBDZ.Frozen = true;
            this.GDBDZ.HeaderText = "所属变电站";
            this.GDBDZ.Name = "GDBDZ";
            this.GDBDZ.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GDBDZ.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.GDBDZ.Width = 90;
            // 
            // KXH
            // 
            this.KXH.Frozen = true;
            this.KXH.HeaderText = "所属馈线";
            this.KXH.Name = "KXH";
            this.KXH.Width = 150;
            // 
            // G3E_ID
            // 
            this.G3E_ID.HeaderText = "g3e_id";
            this.G3E_ID.Name = "G3E_ID";
            this.G3E_ID.Visible = false;
            this.G3E_ID.Width = 20;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.DevNameGroup);
            this.splitContainer1.Panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer1.Size = new System.Drawing.Size(307, 727);
            this.splitContainer1.SplitterDistance = 351;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 4;
            // 
            // PanelControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PanelControl";
            this.Size = new System.Drawing.Size(307, 727);
            this.Load += new System.EventHandler(this.PanelControl_Load);
            this.Resize += new System.EventHandler(this.PanelControl_Resize);
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.DevNameGroup.ResumeLayout(false);
            this.Tab_gnwz.ResumeLayout(false);
            this.tabPage_GNWZ.ResumeLayout(false);
            this.tabPage_SDKX.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_SDKX)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolSM_Refresh;
        private System.Windows.Forms.ToolStripMenuItem ToolSM_Delete;
        private System.Windows.Forms.ToolStripMenuItem ToolSM_PreView;
        private System.Windows.Forms.GroupBox DevNameGroup;
        private System.Windows.Forms.TabControl Tab_gnwz;
        private System.Windows.Forms.TabPage tabPage_GNWZ;
        private System.Windows.Forms.PropertyGrid DevPropertyGrid;
        private System.Windows.Forms.TabPage tabPage_SDKX;
        private System.Windows.Forms.DataGridView DGV_SDKX;
        private System.Windows.Forms.DataGridViewComboBoxColumn GDBDZ;
        private System.Windows.Forms.DataGridViewComboBoxColumn KXH;
        private System.Windows.Forms.DataGridViewTextBoxColumn G3E_ID;
        private System.Windows.Forms.SplitContainer splitContainer1;

    }
}
