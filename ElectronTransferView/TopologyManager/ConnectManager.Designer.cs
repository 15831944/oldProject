namespace ElectronTransferView.ConnectivityManager
{
    partial class ConnectManager
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
/*            CleanSelcol();*/
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
            this.connect_tree = new System.Windows.Forms.TreeView();
            this.CongroupBox = new System.Windows.Forms.GroupBox();
            this.OwngroupBox = new System.Windows.Forms.GroupBox();
            this.owner_tree = new System.Windows.Forms.TreeView();
            this.containgroupBox = new System.Windows.Forms.GroupBox();
            this.contain_tree = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.connectMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.连接 = new System.Windows.Forms.ToolStripMenuItem();
            this.打断 = new System.Windows.Forms.ToolStripMenuItem();
            this.ownerMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.从属于设备 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除从属关系 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除从属设备 = new System.Windows.Forms.ToolStripMenuItem();
            this.添加从属设备 = new System.Windows.Forms.ToolStripMenuItem();
            this.批量添加从属设备 = new System.Windows.Forms.ToolStripMenuItem();
            this.批量删除从属设备 = new System.Windows.Forms.ToolStripMenuItem();
            this.containMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.包含 = new System.Windows.Forms.ToolStripMenuItem();
            this.包含批量 = new System.Windows.Forms.ToolStripMenuItem();
            this.被包含 = new System.Windows.Forms.ToolStripMenuItem();
            this.被包含批量 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除线 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除杆 = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.devname = new System.Windows.Forms.Label();
            this.BreakConMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.BreakdConnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.被包含自动 = new System.Windows.Forms.ToolStripMenuItem();
            this.添加从属设备自动 = new System.Windows.Forms.ToolStripMenuItem();
            this.CongroupBox.SuspendLayout();
            this.OwngroupBox.SuspendLayout();
            this.containgroupBox.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.connectMenuStrip.SuspendLayout();
            this.ownerMenuStrip.SuspendLayout();
            this.containMenuStrip.SuspendLayout();
            this.BreakConMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // connect_tree
            // 
            this.connect_tree.BackColor = System.Drawing.SystemColors.Window;
            this.connect_tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.connect_tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connect_tree.ItemHeight = 20;
            this.connect_tree.Location = new System.Drawing.Point(3, 17);
            this.connect_tree.Name = "connect_tree";
            this.connect_tree.Size = new System.Drawing.Size(233, 219);
            this.connect_tree.TabIndex = 0;
            this.connect_tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ctr_NodeMouseDoubleClick);
            this.connect_tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.connect_tree_NodeMouseClick);
            // 
            // CongroupBox
            // 
            this.CongroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CongroupBox.Controls.Add(this.connect_tree);
            this.CongroupBox.Location = new System.Drawing.Point(3, 3);
            this.CongroupBox.Name = "CongroupBox";
            this.CongroupBox.Size = new System.Drawing.Size(239, 239);
            this.CongroupBox.TabIndex = 1;
            this.CongroupBox.TabStop = false;
            this.CongroupBox.Text = "连接";
            // 
            // OwngroupBox
            // 
            this.OwngroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.OwngroupBox.Controls.Add(this.owner_tree);
            this.OwngroupBox.Location = new System.Drawing.Point(3, 3);
            this.OwngroupBox.Name = "OwngroupBox";
            this.OwngroupBox.Size = new System.Drawing.Size(239, 209);
            this.OwngroupBox.TabIndex = 2;
            this.OwngroupBox.TabStop = false;
            this.OwngroupBox.Text = "从属";
            // 
            // owner_tree
            // 
            this.owner_tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.owner_tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.owner_tree.Location = new System.Drawing.Point(3, 17);
            this.owner_tree.Name = "owner_tree";
            this.owner_tree.Size = new System.Drawing.Size(233, 189);
            this.owner_tree.TabIndex = 0;
            this.owner_tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ctr_NodeMouseDoubleClick);
            this.owner_tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.owner_tree_NodeMouseClick);
            // 
            // containgroupBox
            // 
            this.containgroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.containgroupBox.Controls.Add(this.contain_tree);
            this.containgroupBox.Location = new System.Drawing.Point(3, 3);
            this.containgroupBox.Name = "containgroupBox";
            this.containgroupBox.Size = new System.Drawing.Size(239, 201);
            this.containgroupBox.TabIndex = 3;
            this.containgroupBox.TabStop = false;
            this.containgroupBox.Text = "包含";
            // 
            // contain_tree
            // 
            this.contain_tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.contain_tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contain_tree.Location = new System.Drawing.Point(3, 17);
            this.contain_tree.Name = "contain_tree";
            this.contain_tree.Size = new System.Drawing.Size(233, 181);
            this.contain_tree.TabIndex = 0;
            this.contain_tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ctr_NodeMouseDoubleClick);
            this.contain_tree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.contain_tree_NodeMouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Location = new System.Drawing.Point(0, 29);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.CongroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(245, 650);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.OwngroupBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.containgroupBox);
            this.splitContainer2.Size = new System.Drawing.Size(245, 403);
            this.splitContainer2.SplitterDistance = 205;
            this.splitContainer2.TabIndex = 3;
            // 
            // connectMenuStrip
            // 
            this.connectMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连接,
            this.打断});
            this.connectMenuStrip.Name = "contextMenuStrip1";
            this.connectMenuStrip.Size = new System.Drawing.Size(95, 48);
            this.connectMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.connectMenuStrip_ItemClicked);
            // 
            // 连接
            // 
            this.连接.Name = "连接";
            this.连接.Size = new System.Drawing.Size(94, 22);
            this.连接.Text = "连接";
            // 
            // 打断
            // 
            this.打断.Name = "打断";
            this.打断.Size = new System.Drawing.Size(94, 22);
            this.打断.Text = "打断";
            // 
            // ownerMenuStrip
            // 
            this.ownerMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.从属于设备,
            this.删除从属关系,
            this.删除从属设备,
            this.添加从属设备,
            this.添加从属设备自动,
            this.批量添加从属设备,
            this.批量删除从属设备});
            this.ownerMenuStrip.Name = "ownerMenuStrip";
            this.ownerMenuStrip.Size = new System.Drawing.Size(179, 180);
            this.ownerMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ownerMenuStrip_ItemClicked);
            // 
            // 从属于设备
            // 
            this.从属于设备.Name = "从属于设备";
            this.从属于设备.Size = new System.Drawing.Size(178, 22);
            this.从属于设备.Text = "从属于设备";
            // 
            // 删除从属关系
            // 
            this.删除从属关系.Name = "删除从属关系";
            this.删除从属关系.Size = new System.Drawing.Size(178, 22);
            this.删除从属关系.Text = "删除从属关系";
            // 
            // 删除从属设备
            // 
            this.删除从属设备.Name = "删除从属设备";
            this.删除从属设备.Size = new System.Drawing.Size(178, 22);
            this.删除从属设备.Text = "删除从属设备";
            // 
            // 添加从属设备
            // 
            this.添加从属设备.Name = "添加从属设备";
            this.添加从属设备.Size = new System.Drawing.Size(178, 22);
            this.添加从属设备.Text = "添加从属设备";
            // 
            // 批量添加从属设备
            // 
            this.批量添加从属设备.Name = "批量添加从属设备";
            this.批量添加从属设备.Size = new System.Drawing.Size(178, 22);
            this.批量添加从属设备.Text = "批量添加从属设备";
            // 
            // 批量删除从属设备
            // 
            this.批量删除从属设备.Name = "批量删除从属设备";
            this.批量删除从属设备.Size = new System.Drawing.Size(178, 22);
            this.批量删除从属设备.Text = "批量删除从属设备";
            // 
            // containMenuStrip
            // 
            this.containMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.包含,
            this.包含批量,
            this.被包含,
            this.被包含批量,
            this.被包含自动,
            this.删除线,
            this.删除杆});
            this.containMenuStrip.Name = "containMenuStrip";
            this.containMenuStrip.Size = new System.Drawing.Size(143, 158);
            this.containMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.containMenuStrip_ItemClicked);
            // 
            // 包含
            // 
            this.包含.Name = "包含";
            this.包含.Size = new System.Drawing.Size(142, 22);
            this.包含.Text = "包含";
            // 
            // 包含批量
            // 
            this.包含批量.Name = "包含批量";
            this.包含批量.Size = new System.Drawing.Size(142, 22);
            this.包含批量.Text = "包含(批量)";
            // 
            // 被包含
            // 
            this.被包含.Name = "被包含";
            this.被包含.Size = new System.Drawing.Size(142, 22);
            this.被包含.Text = "被包含";
            // 
            // 被包含批量
            // 
            this.被包含批量.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.被包含批量.Name = "被包含批量";
            this.被包含批量.Size = new System.Drawing.Size(142, 22);
            this.被包含批量.Text = "被包含(批量)";
            // 
            // 删除线
            // 
            this.删除线.Name = "删除线";
            this.删除线.Size = new System.Drawing.Size(142, 22);
            this.删除线.Text = "删除线";
            // 
            // 删除杆
            // 
            this.删除杆.Name = "删除杆";
            this.删除杆.Size = new System.Drawing.Size(142, 22);
            this.删除杆.Text = "删除杆";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "当前选中设备:";
            // 
            // devname
            // 
            this.devname.AutoSize = true;
            this.devname.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.devname.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.devname.Location = new System.Drawing.Point(89, 8);
            this.devname.Name = "devname";
            this.devname.Size = new System.Drawing.Size(0, 12);
            this.devname.TabIndex = 6;
            // 
            // BreakConMenuStrip
            // 
            this.BreakConMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BreakdConnToolStripMenuItem});
            this.BreakConMenuStrip.Name = "BreakConMenuStrip";
            this.BreakConMenuStrip.Size = new System.Drawing.Size(95, 26);
            this.BreakConMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.BreakConMenuStrip_ItemClicked);
            // 
            // BreakdConnToolStripMenuItem
            // 
            this.BreakdConnToolStripMenuItem.Name = "BreakdConnToolStripMenuItem";
            this.BreakdConnToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.BreakdConnToolStripMenuItem.Text = "打断";
            // 
            // 被包含自动
            // 
            this.被包含自动.Name = "被包含自动";
            this.被包含自动.Size = new System.Drawing.Size(142, 22);
            this.被包含自动.Text = "被包含(自动)";
            // 
            // 添加从属设备自动
            // 
            this.添加从属设备自动.Name = "添加从属设备自动";
            this.添加从属设备自动.Size = new System.Drawing.Size(178, 22);
            this.添加从属设备自动.Text = "添加从属设备(自动)";
            // 
            // ConnectManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.devname);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.Name = "ConnectManager";
            this.Size = new System.Drawing.Size(245, 681);
            this.CongroupBox.ResumeLayout(false);
            this.OwngroupBox.ResumeLayout(false);
            this.containgroupBox.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.connectMenuStrip.ResumeLayout(false);
            this.ownerMenuStrip.ResumeLayout(false);
            this.containMenuStrip.ResumeLayout(false);
            this.BreakConMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox CongroupBox;
        private System.Windows.Forms.GroupBox OwngroupBox;
        private System.Windows.Forms.TreeView owner_tree;
        private System.Windows.Forms.GroupBox containgroupBox;
        private System.Windows.Forms.TreeView contain_tree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ContextMenuStrip connectMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 连接;
        private System.Windows.Forms.ToolStripMenuItem 打断;
        private System.Windows.Forms.ContextMenuStrip ownerMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 从属于设备;
        private System.Windows.Forms.ContextMenuStrip containMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 包含;
        private System.Windows.Forms.ToolStripMenuItem 被包含;
        private System.Windows.Forms.ToolStripMenuItem 删除从属设备;
        private System.Windows.Forms.ToolStripMenuItem 删除从属关系;
        private System.Windows.Forms.ToolStripMenuItem 添加从属设备;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label devname;
        private System.Windows.Forms.ToolStripMenuItem 删除线;
        private System.Windows.Forms.ToolStripMenuItem 删除杆;
        private System.Windows.Forms.ToolStripMenuItem 批量添加从属设备;
        internal System.Windows.Forms.TreeView connect_tree;
        private System.Windows.Forms.ContextMenuStrip BreakConMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem BreakdConnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 批量删除从属设备;
        private System.Windows.Forms.ToolStripMenuItem 被包含批量;
        private System.Windows.Forms.ToolStripMenuItem 包含批量;
        private System.Windows.Forms.ToolStripMenuItem 被包含自动;
        private System.Windows.Forms.ToolStripMenuItem 添加从属设备自动;
    }
}
