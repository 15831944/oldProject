namespace ElectronTransferView.SearchManager
{
    sealed partial class SpecialDeviceList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvDevCol = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sbmc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevCol)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDevCol
            // 
            this.dgvDevCol.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgvDevCol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevCol.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.Sbmc});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDevCol.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDevCol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevCol.Location = new System.Drawing.Point(0, 0);
            this.dgvDevCol.Name = "dgvDevCol";
            this.dgvDevCol.ReadOnly = true;
            this.dgvDevCol.RowTemplate.Height = 23;
            this.dgvDevCol.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevCol.Size = new System.Drawing.Size(440, 184);
            this.dgvDevCol.TabIndex = 1;
            this.dgvDevCol.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvDevCol_CellMouseClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "fid";
            this.id.HeaderText = "G3E_FID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // Sbmc
            // 
            this.Sbmc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Sbmc.DataPropertyName = "sbmc";
            this.Sbmc.HeaderText = "设备名称";
            this.Sbmc.Name = "Sbmc";
            this.Sbmc.ReadOnly = true;
            // 
            // SpecialDeviceList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(440, 184);
            this.Controls.Add(this.dgvDevCol);
            this.Name = "SpecialDeviceList";
            this.Text = "设备列表";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevCol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDevCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sbmc;
    }
}