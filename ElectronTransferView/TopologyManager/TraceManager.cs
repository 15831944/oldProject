using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.EditorInput;
using DotNetARX;
using ElectronTransferDal.Cad;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using acapi = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ElectronTransferView.TopologyManager
{
    public enum TraceStyle
    {
        traceup,
        tracedown,
        traceall,
        tracep2p
    };

    public partial class TraceManager : Form
    {
        // 当前颜色
        public Color ccolor { get; set; }
        // 起始设备
        public long _startfid { get; set; }
        // 终止设备
        public long _endfid { get; set; }

        private TraceStyle ts;

        public TraceManager()
        {
            InitializeComponent();
        }

        private void selColor_Click(object sender, EventArgs e)
        {
            var result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                PublicMethod.Instance.traceColor = ccolor = curColor.BackColor = colorDialog1.Color;
            }
        }

        private void TopologyTrace_Load(object sender, EventArgs e)
        {
            ccolor = curColor.BackColor = PublicMethod.Instance.traceColor;

            rb_down.Checked = true;
            ts = TraceStyle.tracedown;
        }

        private void bt_trace_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
                switch (ts)
                {
                    case TraceStyle.tracedown:
                        TopologyTrace.TraceDown(_startfid);
                        break;
                    case TraceStyle.traceup:
                        TopologyTrace.TraceUp(_startfid);
                        break;
                    case TraceStyle.traceall:
                        TopologyTrace.TraceAll(_startfid);
                        break;
                    case TraceStyle.tracep2p:
                        TopologyTrace.TraceP2P(_startfid, _endfid);
                        break;
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
            finally
            {
                acapi.UpdateScreen();
                Show();
            }
        }

        private void bt_clean_Click(object sender, EventArgs e)
        {
            try
            {
                Hide();
                TopologyTrace.CleanTrace();
            }
            finally
            {
                Show();
            }
        }

        private void bt_choosedev_Click(object sender, EventArgs e)
        {
            var db = HostApplicationServices.WorkingDatabase;
            var ed = db.GetEditor();
            try
            {
                Hide();
                //选择要追踪的设备
                var pentr = ed.GetEntity("选择要追踪的设备:\n");
                if (pentr.Status == PromptStatus.OK)
                {
                    var entid = pentr.ObjectId;
                    long id = 0, fid = 0, fno = 0;
                    if (!DBEntityFinder.Instance.GetG3EIds(entid, ref id, ref fid, ref fno)) { return; }
                    _startfid = fid;
                    tb_start.Text = _startfid.ToString();
                }
                if (ts == TraceStyle.tracep2p)
                {
                    PublicMethod.Instance.AlertDialog("选择追踪结束的设备:\n");
                    pentr = ed.GetEntity("选择追踪结束的设备:\n");
                    if (pentr.Status == PromptStatus.OK)
                    {
                        var entid = pentr.ObjectId;
                        long id = 0, fid = 0, fno = 0;
                        if (!DBEntityFinder.Instance.GetG3EIds(entid, ref id, ref fid, ref fno)) { return; }
                        _endfid = fid;
                        tb_end.Text = _endfid.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog(ex.Message);
                LogManager.Instance.Error(ex);
            }
            finally
            {
                Show();
                DCadApi.isModifySymbol = false;
            }
        }

        private void rb_up_CheckedChanged(object sender, EventArgs e)
        {
            ts = TraceStyle.traceup;
        }

        private void rb_down_CheckedChanged(object sender, EventArgs e)
        {
            ts = TraceStyle.tracedown;
        }

        private void rb_all_CheckedChanged(object sender, EventArgs e)
        {
            ts = TraceStyle.traceall;
        }

        private void rb_p2p_CheckedChanged(object sender, EventArgs e)
        {
            ts = TraceStyle.tracep2p;
        }
    }
}
