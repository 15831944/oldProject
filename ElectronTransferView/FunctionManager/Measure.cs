using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using DotNetARX;
using ElectronTransferDal.Cad;
using ElectronTransferModel.Config;
using acapi = Autodesk.AutoCAD.ApplicationServices.Application;

namespace ElectronTransferView.FunctionManager
{
    public partial class Measure : Form
    {
        // 存储测量距离产生的实体
        private List<ObjectId> measureIDs = new List<ObjectId>();

        public Color ccolor { get; set; }

        public Measure()
        {
            InitializeComponent();
            //设置默认值
            ComBox_Unit.Text = "米";
            cb_txtSize.Text = "2";
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {

            DocumentLock doclock = null;
            var doc = acapi.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            
            try
            {
                double txtsize;
                double sumValue=0;
                if (!double.TryParse(cb_txtSize.Text, out txtsize)) return;
                var unit = ComBox_Unit.Text.Trim();
                if (string.IsNullOrEmpty(unit)) return;

                txtsize = txtsize/10;
                // 隐藏窗口
                Visible = false;
                // 锁定文档
                doclock=doc.LockDocument();
                // 获取0层ID
                ObjectId layerID;
                using (var tr = doc.Database.TransactionManager.StartTransaction())
                {
                    doc.Database.SetCurrentLayer("背景地图");
                    layerID = doc.Database.AddLayer("背景地图");
                    tr.Commit();
                }
                var re = ed.GetPoint("select point:\n");
                if (re.Status != PromptStatus.OK) return;
                ed.WriteMessageWithReturn(re.Value);
                var jig = new measurejig();
                jig.prePosition = re.Value;
                jig.curPosition = re.Value;
                var res = ed.Drag(jig);
                int i = 1;
                var sumtext = new DBText();
                while (res.Status == PromptStatus.OK)
                {
                    // 添加连线
                    var line = new Line(jig.prePosition, jig.curPosition);
                    line.ColorIndex = 3;
                    using (var tr = doc.Database.TransactionManager.StartTransaction())
                    {
                        measureIDs.Add(doc.Database.AddToModelSpace(line));
                        line.LayerId = layerID;
                        line.Color = Autodesk.AutoCAD.Colors.Color.FromColor(ccolor);
                        tr.Commit();
                    }

                    if (i == 1)
                    {
                        // 总长度添加标注
                        sumtext.Position = jig.prePosition;
                        //sumtext.Height = txtsize / 111319.49079327357264771338267056;
                        sumtext.Height = txtsize/MapConfig.Instance.earthscale;
                        sumtext.TextString = string.Format("{0}{1}", sumValue, unit);
                        using (var tr = doc.Database.TransactionManager.StartTransaction())
                        {
                            measureIDs.Add(doc.Database.AddToModelSpace(sumtext));
                            sumtext.Color = Autodesk.AutoCAD.Colors.Color.FromColor(ccolor);
                            sumtext.LayerId = layerID;
                            tr.Commit();
                        }
                        i++;
                    }

                    // 计算距离
                    var dist = jig.GetDistance(jig.curPosition, jig.prePosition, ComBox_Unit.SelectedIndex+1);
                    if (jig.curPosition != jig.prePosition && dist > 0)
                    {
                        var dbtext = new DBText();
                        // 计算标注角度
                        var vec3d = jig.prePosition.GetVectorTo(jig.curPosition);
                        var vec2d = new Vector2d(vec3d.X, vec3d.Y);
                        var rolate = vec2d.Angle;
                        var midPt = GeTools.MidPoint(jig.prePosition, jig.curPosition);

                        // 添加距离标注
                        dbtext.Position = midPt;
                        dbtext.Height = txtsize/111319.49079327357264771338267056;
                        dbtext.Rotation = rolate;
                        dbtext.TextString = dist + unit;
                        using (var tr = doc.Database.TransactionManager.StartTransaction())
                        {
                            measureIDs.Add(doc.Database.AddToModelSpace(dbtext));
                            dbtext.Color = Autodesk.AutoCAD.Colors.Color.FromColor(ccolor);
                            dbtext.LayerId = layerID;
                            tr.Commit();
                        }
                        // 统计总长度
                        DCadApi.isModifySymbol = true;
                        using (var tr = doc.Database.TransactionManager.StartTransaction())
                        {
                            var ent = tr.GetObject(sumtext.ObjectId, OpenMode.ForWrite) as DBText;
                            sumValue += dist;
                            //sumtext.UpgradeOpen();
                            if (ent != null) ent.TextString = string.Format("{0}{1}", sumValue, unit);
                            tr.Commit();
                        }
                        DCadApi.isModifySymbol = false;
                    }
                    jig.prePosition = jig.curPosition;
                    res = ed.Drag(jig);
                }
            }
            catch (Exception ex)
            {
                ed.WriteMessage(ex + "\n");
            }
            finally
            {
                Visible=true;
                doclock.Dispose();
                DCadApi.isModifySymbol = false;
            }
        }

        private void Measure_FormClosed(object sender, FormClosedEventArgs e)
        {
            cleanMeasureIDs();
        }

        private void Measure_KeyDown(object sender, KeyEventArgs e)
        {
            //cleanMeasureIDs();
        }

        private void btn_clean_Click(object sender, EventArgs e)
        {
            cleanMeasureIDs();
        }

        private void cleanMeasureIDs()
        {
            
            var doc = acapi.DocumentManager.MdiActiveDocument;
            try
            {
                Visible = false;
                using (doc.LockDocument())
                {
                    using (var tr = doc.Database.TransactionManager.StartTransaction())
                    {
                        foreach (var id in measureIDs)
                        {
                            if (id.IsErased) continue;
                            var ent = tr.GetObject(id, OpenMode.ForWrite) as Entity;
                            if (ent != null) ent.Erase();
                        }
                        measureIDs.Clear();
                        tr.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.ToString());
            }
            finally
            {
                Visible = true;
            }
        }

        private void selColor_Click(object sender, EventArgs e)
        {
            var result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                ccolor = curColor.BackColor = colorDialog1.Color;
            }
        }

        private void Measure_Load(object sender, EventArgs e)
        {
            ccolor = curColor.BackColor = Color.Black;
        }
    }
}
