namespace ElectronTransferView.SearchManager
{
    partial class DeviceGridView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DevGridView = new System.Windows.Forms.DataGridView();
            this.Device_FID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Device_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.substation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Circuits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ErpOrganisation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DevGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // DevGridView
            // 
            this.DevGridView.AllowUserToAddRows = false;
            this.DevGridView.AllowUserToDeleteRows = false;
            this.DevGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DevGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.DevGridView.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DevGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DevGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DevGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Device_FID,
            this.Device_name,
            this.status,
            this.unit,
            this.substation,
            this.Circuits,
            this.ErpOrganisation});
            this.DevGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DevGridView.Location = new System.Drawing.Point(0, 0);
            this.DevGridView.MultiSelect = false;
            this.DevGridView.Name = "DevGridView";
            this.DevGridView.ReadOnly = true;
            this.DevGridView.RowTemplate.Height = 23;
            this.DevGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DevGridView.Size = new System.Drawing.Size(863, 232);
            this.DevGridView.TabIndex = 0;
            this.DevGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DevGridView_CellClick);
            // 
            // Device_FID
            // 
            this.Device_FID.DataPropertyName = "G3E_FID";
            this.Device_FID.FillWeight = 85.7143F;
            this.Device_FID.HeaderText = "设备FID";
            this.Device_FID.Name = "Device_FID";
            this.Device_FID.ReadOnly = true;
            // 
            // Device_name
            // 
            this.Device_name.DataPropertyName = "SBMC";
            this.Device_name.FillWeight = 176.4432F;
            this.Device_name.HeaderText = "设备名称";
            this.Device_name.Name = "Device_name";
            this.Device_name.ReadOnly = true;
            // 
            // status
            // 
            this.status.DataPropertyName = "CD_SMZQ";
            this.status.FillWeight = 79.61922F;
            this.status.HeaderText = "运行状态";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // unit
            // 
            this.unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.unit.DataPropertyName = "CD_SSDW";
            this.unit.FillWeight = 78.26877F;
            this.unit.HeaderText = "所属单位";
            this.unit.Name = "unit";
            this.unit.ReadOnly = true;
            // 
            // substation
            // 
            this.substation.DataPropertyName = "CD_SSBDZ";
            this.substation.FillWeight = 100.6738F;
            this.substation.HeaderText = "所属变电站";
            this.substation.Name = "substation";
            this.substation.ReadOnly = true;
            // 
            // Circuits
            // 
            this.Circuits.DataPropertyName = "CD_SSXL";
            this.Circuits.FillWeight = 101.4619F;
            this.Circuits.HeaderText = "受电馈线";
            this.Circuits.Name = "Circuits";
            this.Circuits.ReadOnly = true;
            // 
            // ErpOrganisation
            // 
            this.ErpOrganisation.DataPropertyName = "CD_DYDJ";
            this.ErpOrganisation.FillWeight = 77.81897F;
            this.ErpOrganisation.HeaderText = "电压等级";
            this.ErpOrganisation.Name = "ErpOrganisation";
            this.ErpOrganisation.ReadOnly = true;
            // 
            // DeviceGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(863, 232);
            this.Controls.Add(this.DevGridView);
            this.Name = "DeviceGridView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设备清单";
            this.Load += new System.EventHandler(this.DeviceGridView_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DeviceGridView_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.DevGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DevGridView;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Device_FID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Device_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn substation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Circuits;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErpOrganisation;
    }
}