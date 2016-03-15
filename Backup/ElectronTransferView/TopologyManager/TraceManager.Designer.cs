namespace ElectronTransferView.TopologyManager
{
    partial class TraceManager
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_choosedev = new System.Windows.Forms.Button();
            this.rb_down = new System.Windows.Forms.RadioButton();
            this.rb_up = new System.Windows.Forms.RadioButton();
            this.tb_end = new System.Windows.Forms.TextBox();
            this.rb_all = new System.Windows.Forms.RadioButton();
            this.tb_start = new System.Windows.Forms.TextBox();
            this.rb_p2p = new System.Windows.Forms.RadioButton();
            this.bt_trace = new System.Windows.Forms.Button();
            this.selColor = new System.Windows.Forms.Button();
            this.curColor = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.bt_clean = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.bt_choosedev);
            this.groupBox1.Controls.Add(this.rb_down);
            this.groupBox1.Controls.Add(this.rb_up);
            this.groupBox1.Controls.Add(this.tb_end);
            this.groupBox1.Controls.Add(this.rb_all);
            this.groupBox1.Controls.Add(this.tb_start);
            this.groupBox1.Controls.Add(this.rb_p2p);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 96);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "追踪类型";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "起始设备:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "终止设备:";
            // 
            // bt_choosedev
            // 
            this.bt_choosedev.Location = new System.Drawing.Point(296, 57);
            this.bt_choosedev.Name = "bt_choosedev";
            this.bt_choosedev.Size = new System.Drawing.Size(72, 20);
            this.bt_choosedev.TabIndex = 12;
            this.bt_choosedev.Text = "设备选择";
            this.bt_choosedev.UseVisualStyleBackColor = true;
            this.bt_choosedev.Click += new System.EventHandler(this.bt_choosedev_Click);
            // 
            // rb_down
            // 
            this.rb_down.AutoSize = true;
            this.rb_down.Location = new System.Drawing.Point(22, 60);
            this.rb_down.Name = "rb_down";
            this.rb_down.Size = new System.Drawing.Size(47, 16);
            this.rb_down.TabIndex = 1;
            this.rb_down.TabStop = true;
            this.rb_down.Text = "下游";
            this.rb_down.UseVisualStyleBackColor = true;
            this.rb_down.CheckedChanged += new System.EventHandler(this.rb_down_CheckedChanged);
            // 
            // rb_up
            // 
            this.rb_up.AutoSize = true;
            this.rb_up.Location = new System.Drawing.Point(87, 60);
            this.rb_up.Name = "rb_up";
            this.rb_up.Size = new System.Drawing.Size(47, 16);
            this.rb_up.TabIndex = 2;
            this.rb_up.TabStop = true;
            this.rb_up.Text = "上游";
            this.rb_up.UseVisualStyleBackColor = true;
            this.rb_up.CheckedChanged += new System.EventHandler(this.rb_up_CheckedChanged);
            // 
            // tb_end
            // 
            this.tb_end.Enabled = false;
            this.tb_end.Location = new System.Drawing.Point(268, 25);
            this.tb_end.Name = "tb_end";
            this.tb_end.Size = new System.Drawing.Size(100, 21);
            this.tb_end.TabIndex = 5;
            // 
            // rb_all
            // 
            this.rb_all.AutoSize = true;
            this.rb_all.Location = new System.Drawing.Point(149, 60);
            this.rb_all.Name = "rb_all";
            this.rb_all.Size = new System.Drawing.Size(59, 16);
            this.rb_all.TabIndex = 3;
            this.rb_all.TabStop = true;
            this.rb_all.Text = "全追踪";
            this.rb_all.UseVisualStyleBackColor = true;
            this.rb_all.CheckedChanged += new System.EventHandler(this.rb_all_CheckedChanged);
            // 
            // tb_start
            // 
            this.tb_start.Enabled = false;
            this.tb_start.Location = new System.Drawing.Point(85, 25);
            this.tb_start.Name = "tb_start";
            this.tb_start.Size = new System.Drawing.Size(100, 21);
            this.tb_start.TabIndex = 5;
            // 
            // rb_p2p
            // 
            this.rb_p2p.AutoSize = true;
            this.rb_p2p.Location = new System.Drawing.Point(222, 60);
            this.rb_p2p.Name = "rb_p2p";
            this.rb_p2p.Size = new System.Drawing.Size(59, 16);
            this.rb_p2p.TabIndex = 4;
            this.rb_p2p.TabStop = true;
            this.rb_p2p.Text = "点对点";
            this.rb_p2p.UseVisualStyleBackColor = true;
            this.rb_p2p.CheckedChanged += new System.EventHandler(this.rb_p2p_CheckedChanged);
            // 
            // bt_trace
            // 
            this.bt_trace.Location = new System.Drawing.Point(318, 181);
            this.bt_trace.Name = "bt_trace";
            this.bt_trace.Size = new System.Drawing.Size(75, 23);
            this.bt_trace.TabIndex = 8;
            this.bt_trace.Text = "追踪";
            this.bt_trace.UseVisualStyleBackColor = true;
            this.bt_trace.Click += new System.EventHandler(this.bt_trace_Click);
            // 
            // selColor
            // 
            this.selColor.Location = new System.Drawing.Point(138, 18);
            this.selColor.Name = "selColor";
            this.selColor.Size = new System.Drawing.Size(77, 20);
            this.selColor.TabIndex = 12;
            this.selColor.Text = "颜色选择";
            this.selColor.UseVisualStyleBackColor = true;
            this.selColor.Click += new System.EventHandler(this.selColor_Click);
            // 
            // curColor
            // 
            this.curColor.BackColor = System.Drawing.SystemColors.Control;
            this.curColor.Enabled = false;
            this.curColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.curColor.Location = new System.Drawing.Point(87, 22);
            this.curColor.Name = "curColor";
            this.curColor.Size = new System.Drawing.Size(31, 14);
            this.curColor.TabIndex = 11;
            this.curColor.TabStop = false;
            this.curColor.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "当前颜色:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.curColor);
            this.groupBox2.Controls.Add(this.selColor);
            this.groupBox2.Location = new System.Drawing.Point(12, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(381, 50);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "追踪颜色设置";
            // 
            // bt_clean
            // 
            this.bt_clean.Location = new System.Drawing.Point(234, 181);
            this.bt_clean.Name = "bt_clean";
            this.bt_clean.Size = new System.Drawing.Size(75, 23);
            this.bt_clean.TabIndex = 8;
            this.bt_clean.Text = "清除";
            this.bt_clean.UseVisualStyleBackColor = true;
            this.bt_clean.Click += new System.EventHandler(this.bt_clean_Click);
            // 
            // TraceManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 209);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bt_clean);
            this.Controls.Add(this.bt_trace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "TraceManager";
            this.Text = "拓扑追踪";
            this.Load += new System.EventHandler(this.TopologyTrace_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rb_down;
        private System.Windows.Forms.RadioButton rb_up;
        private System.Windows.Forms.TextBox tb_end;
        private System.Windows.Forms.RadioButton rb_all;
        private System.Windows.Forms.TextBox tb_start;
        private System.Windows.Forms.RadioButton rb_p2p;
        private System.Windows.Forms.Button bt_trace;
        private System.Windows.Forms.Button selColor;
        private System.Windows.Forms.Button curColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button bt_clean;
        private System.Windows.Forms.Button bt_choosedev;
    }
}