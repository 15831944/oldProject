using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using DotNetARX;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;

namespace ElectronTransferDal.Cad
{
    public class DCadAddLateraLine:EntityJig
    {
        private ObjectId entId;
        public static Point3d m_fistPnt;
        private CommandStatus m_status;
        private Point3d m_startPnt, m_lastPnt;

        public static ObjectIdList dxObjIdList = new ObjectIdList();
        public static ObjectIdList dgObjIdList = new ObjectIdList();
        public static List<LineString> listLineString = new List<LineString>();


        private double scale = 1 / MapConfig.Instance.earthscale;
        public DCadAddLateraLine(ObjectId blockId)
            : base(new BlockReference(Point3d.Origin, blockId))
        {
            (Entity as BlockReference).ScaleFactors = new Scale3d(scale);
            (Entity as BlockReference).ColorIndex = 1;
            entId = blockId;


        }
        protected override bool Update()
        {
            (Entity as BlockReference).Position = m_lastPnt;
            return true;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions promOpts = new JigPromptPointOptions("");
            promOpts.UserInputControls =
                (UserInputControls.Accept3dCoordinates
                 | UserInputControls.NoNegativeResponseAccepted
                 | UserInputControls.NoZeroResponseAccepted
                 | UserInputControls.NullResponseAccepted);

            switch (m_status)
            {
                case CommandStatus.kWaitForStartPnt:
                    promOpts.BasePoint = m_fistPnt;
                    promOpts.UseBasePoint = true;
                    promOpts.Cursor = CursorType.RubberBand;
                    promOpts.Message = "\n输入插入点:";

                    break;
                case CommandStatus.kWatiForOtherPnt:
                    promOpts.BasePoint = m_startPnt;
                    promOpts.UseBasePoint = true;
                    promOpts.Cursor = CursorType.RubberBand;
                    promOpts.Message = "\n输入插入点:";

                    break;
                default:
                    return SamplerStatus.Cancel;
            }

            PromptPointResult res = prompts.AcquirePoint(promOpts);

            if (res.Value == m_lastPnt)
            {
                return SamplerStatus.NoChange;
            }
            if (res.Status == PromptStatus.OK)
            {
                m_lastPnt = res.Value;
                return SamplerStatus.OK;
            }
            return SamplerStatus.Cancel;
        }

        public void DrawLateralLine(object dg, object dx)
        {
            DCadApi.isInsertSymbol = true;
            DCadApi.isModifySymbol = true;
            Editor ed = PublicMethod.Instance.Editor;
            var lineStyle=GetDxStyle(dx);
            var symbolStyle = GetDgStyle(dg);
            m_status = CommandStatus.kWatiForOtherPnt;
            while (m_status == CommandStatus.kWatiForOtherPnt)
            {
                var lineString = new LineString();
                var pl = SetPolyLine(lineStyle.symbolColor);
                pl.AddVertexAt(pl.NumberOfVertices, new Point2d(m_fistPnt.X, m_fistPnt.Y), 0, lineStyle.lineWidth / MapConfig.Instance.earthscale,
                               lineStyle.lineWidth / MapConfig.Instance.earthscale);
                lineString= AddLineString(lineString, m_fistPnt);


                var res = ed.Drag(this);
                if (res.Status == PromptStatus.OK)
                {
                    pl.AddVertexAt(pl.NumberOfVertices, new Point2d(m_lastPnt.X, m_lastPnt.Y), 0, lineStyle.lineWidth / MapConfig.Instance.earthscale,
                                         lineStyle.lineWidth / MapConfig.Instance.earthscale);
                    lineString = AddLineString(lineString, m_lastPnt);

                    var lineObjId=PublicMethod.Instance.ToModelSpace(pl);
                    listLineString.Add(lineString);
                    dxObjIdList.Add(lineObjId);


                    if (!haspole(m_lastPnt, ed))
                    {
                        //数据库处理
                        BlockReference blkref = new BlockReference(m_lastPnt, entId);
                        blkref.ScaleFactors = new Scale3d(scale);
                        blkref.LayerId = GetDGLayerID();

                        blkref.Color = symbolStyle.symbolColor;
                        var objId = PublicMethod.Instance.ToModelSpace(blkref);
                        dgObjIdList.Add(objId);
                    }
                    m_fistPnt = m_lastPnt;
                }
                else
                    m_status = CommandStatus.kCancelAndStop;
            }
        }

        private Polyline SetPolyLine(Color color)
        {
            var pline = new Polyline
                            {
                                Linetype="ByBlock",
                                Color = color,
                                LayerId=GetDXLayerID()
                            };
            return pline;
        }
        private LineString AddLineString(LineString lineString,Point3d point)
        {
            lineString.Points.Add(new Point(new[] { point.X, point.Y, 0 }));
            return lineString;
        }
        private ObjectId GetDGLayerID()
        {
            return DCadApi.addLayer("杆塔");
        }
        private ObjectId GetDXLayerID()
        {
            return DCadApi.addLayer("10kV导线");
        }
        private InsertSymbolEventArgs GetDgStyle(object dg)
        {
            //return DCadApi.InitializeInsertSymbolValue(201, dg, null);===
            return null;
        }
        private InsertSymbolEventArgs GetDxStyle(object dx)
        {
            //return DCadApi.InitializeInsertSymbolValue(141, dx, null);==
            return null;
        }
        public static bool PickFirst()
        {
            PromptPointResult ppr = PublicMethod.Instance.Editor.GetPoint("请选择第一个点\n");
            if (ppr.Status != PromptStatus.OK) return false;
            m_fistPnt = ppr.Value;
            return true;
        }

        /// <summary>
        /// 判断插入点pt 附近是否存在杆塔
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        private bool haspole(Point3d pt, Editor ed)
        {
            //创建选择集过滤器，只选择块对象 
            TypedValue[] filList = new TypedValue[1];
            filList[0] = new TypedValue((int)DxfCode.Start, "INSERT");
            SelectionFilter filter = new SelectionFilter(filList);

            // 通过窗选, 0.3是1/2边长
            PromptSelectionResult ssRes = ed.SelectWindow(new Point3d(pt.X - 0.000002, pt.Y - 0.000002, 0),
                                                          new Point3d(pt.X + 0.000002, pt.Y + 0.000002, 0), filter);
            if (ssRes.Status == PromptStatus.OK)
            {
                SelectionSet ss = ssRes.Value;
                if (ss.Count > 0)
                {
                    ed.WriteMessage("插入点附近已存在杆塔!\n");
                    return true;
                }
            }
            return false;
        }
    }
}
