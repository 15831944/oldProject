using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using DotNetARX;
using ElectronTransferDal.Common;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;
using CADException = Autodesk.AutoCAD.Runtime.Exception;
using CADColor = Autodesk.AutoCAD.Colors.Color;
using System.Drawing;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferDal.Cad
{
    enum CommandStatus
    {
        kWaitForStartPnt,
        kWatiForOtherPnt,
        kCancelAndStop
    };
    public class LateralLine:EntityJig
    {
          #region 共有成员

        public static int i;
        public static ObjectIdList dgObjIdList {set;get;}
        public static Dictionary<ObjectId, LateralLineData> dxObjIdList { set; get; }

        public static Dictionary<ObjectId, InsertSymbolEventArgs> ObjIdList =
            new Dictionary<ObjectId, InsertSymbolEventArgs>(); 

        private double scale = 1/MapConfig.Instance.earthscale;
        private Polyline pline;
        private ObjectId id = ObjectId.Null;
        public static Point3d m_fistPnt;
        private Point3d m_startPnt, m_lastPnt;
        private CommandStatus m_status;
        private ObjectId entid;

        public LateralLine(ObjectId pBlkDefId)
            : base(new BlockReference(Point3d.Origin, pBlkDefId))
        {
            (Entity as BlockReference).ScaleFactors = new Scale3d(scale);
            (Entity as BlockReference).ColorIndex = 1;
            entid = pBlkDefId;
            m_status = CommandStatus.kWaitForStartPnt;
            pline = new Polyline();
            pline.Linetype = "ByBlock";
            pline.ColorIndex = 1;
        }

        public static bool PickFirst()
        {
            PromptPointResult ppr = PublicMethod.Instance.Editor.GetPoint("请选择第一个点\n");
            if (ppr.Status != PromptStatus.OK) return false;
            m_fistPnt = ppr.Value;
            return true;
        }

        public static ObjectId ChosePole()
        {
            PromptKeywordOptions keyopt = new PromptKeywordOptions("\n选择杆塔种类 (钢杆0/水泥杆1/铁塔2)" + "[0/1/2]", "0 1 2");
            PromptResult keyRes = PublicMethod.Instance.Editor.GetKeywords(keyopt);
            return PublicMethod.Instance.GetBlockObjId("201_" + keyRes.StringResult);
        }

        /// <summary>
        ///  拖曳的执行过程
        /// </summary>
        public void doit(XProps dg, XProps dx, bool dxStatus)
        {
            //如果杆塔ID是空,退出程序
            if (entid == ObjectId.Null)
            {
                return;
            }
            Editor ed = PublicMethod.Instance.Editor;
            DCadApi.isInsertSymbol = true;
            DCadApi.isModifySymbol = true;
            try
            {
                dgObjIdList = new ObjectIdList();
                //获取杆的层ID
                var pointLaryerId = GetDGLayerID();
                //获取导线的层ID
                var lineLaryerId = GetDXLayerID();

                //获取导线宽度
                var lineStyle = GetDxStyle(dx);

                //获取符号颜色
                var pointStyle = GetDgStyle(dg);

                pline.AddVertexAt(pline.NumberOfVertices, new Point2d(m_fistPnt.X, m_fistPnt.Y), 0, lineStyle.lineWidth,lineStyle.lineWidth);

                PromptResult res = ed.Drag(this);
                if (res.Status == PromptStatus.OK)
                {
                    if (pline.NumberOfVertices == 1)
                    {
                        SetPolyLine(pline, lineStyle.symbolColor, lineLaryerId);
                    }
                    m_startPnt = m_lastPnt;
                    m_status = CommandStatus.kWatiForOtherPnt;
                  

                    AddLineSymbol(m_lastPnt, lineStyle.lineWidth);

                    if (!haspole(m_lastPnt, ed))
                    {
                        AddPointSymbol(pointStyle, pointLaryerId);
                    }
                }

                while (m_status == CommandStatus.kWatiForOtherPnt)
                {
                    res = ed.Drag(this);
                    if (res.Status == PromptStatus.OK)
                    {
                        m_startPnt = m_lastPnt;

                        if (pline.NumberOfVertices == 1)
                        {
                            id = SetPolyLine(pline, lineStyle.symbolColor, lineLaryerId);
                        }

                        AddLineSymbol(m_lastPnt, lineStyle.lineWidth);

                        //判断是否存在杆塔
                        if (!haspole(m_lastPnt, ed))
                        {
                            AddPointSymbol(pointStyle, pointLaryerId);
                        }
                    }

                    //var bKeyword = res.Status == PromptStatus.Keyword;
                    //if (bKeyword)
                    //{
                    //    try
                    //    {
                    //        pl.RemoveVertexAt(pl.NumberOfVertices - 1);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        PublicMethod.Instance.ShowMessage(ex.ToString());
                    //    }
                    //}

                    if(res.Status==PromptStatus.None)
                    {
                        m_status = CommandStatus.kCancelAndStop;
                        if (!dxStatus)
                            AddPolyline(id, lineLaryerId, lineStyle.symbolColor, lineStyle.lineWidth);
                        else
                        {
                            AddPolyline(id);
                        }
                    }
                }
            }
            catch (CADException eex)
            {
                LogManager.Instance.Error(eex.ToString());
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.ToString());
            }
            finally
            {
                DCadApi.isInsertSymbol = false;
                DCadApi.isModifySymbol = false;
            }
        }
        private void AddPointSymbol(InsertSymbolEventArgs pointStyle,ObjectId dgLaryerId)
        {
            var blkref = new BlockReference(m_lastPnt, entid)
                             {
                                 ScaleFactors=new Scale3d(scale),
                                 LayerId=dgLaryerId,
                                 Color=pointStyle.symbolColor
                             };
            var objId = PublicMethod.Instance.ToModelSpace(blkref);
            dgObjIdList.Add(objId);
        }
        private Polyline AddLineSymbol(Point3d point,double lineWidth)
        {
            Polyline pl;
            var db = HostApplicationServices.WorkingDatabase;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                pl = tr.GetObject(id, OpenMode.ForRead) as Polyline;
                pl.UpgradeOpenAndRun();
                pl.AddVertexAt(pline.NumberOfVertices, new Point2d(point.X, point.Y), 0,
                               lineWidth,
                               lineWidth);
                tr.Commit();
            }
            return pl;
        }
        /// <summary>
        /// 添加连续导线
        /// </summary>
        /// <param name="objectId"></param>
        private void AddPolyline(ObjectId objectId)
        {
            dxObjIdList = new Dictionary<ObjectId, LateralLineData>();
            var lineString =ConvertGeometry.Instance.GetLineString(objectId);
            var lld = new LateralLineData
            {
                lineString = lineString,
                lineAngle = 0
            };
            dxObjIdList.Add(id, lld);
        }
        /// <summary>
        /// 添加分段导线
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="lineLayerID"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        private void AddPolyline(ObjectId objectId, ObjectId lineLayerID, CADColor color,double lineWidth)
        {
            var ptcol = new Point2dCollection();
            for (int i = 0; i < pline.NumberOfVertices; i++)
            {
                ptcol.Add(pline.GetPoint2dAt(i));
            }
            PublicMethod.Instance.EraseObject(objectId);

            dxObjIdList = new Dictionary<ObjectId, LateralLineData>();
            for (var i = 0; i < ptcol.Count - 1; i++)
            {
                var lineString = new LineString();
                var pl = new Polyline();
                pl.LayerId = lineLayerID;
                pl.Color = color;
                pl.Linetype = "Continuous";

                var startPoint = ptcol[i];
                var endPoint = ptcol[i + 1];

                pl.AddVertexAt(pl.NumberOfVertices, new Point2d(startPoint.X, startPoint.Y), 0, lineWidth, lineWidth);
                lineString.Points.Add(new ElectronTransferModel.Geo.Point(new[] {startPoint.X, startPoint.Y, 0}));
                pl.AddVertexAt(pl.NumberOfVertices, new Point2d(endPoint.X, endPoint.Y), 0, lineWidth, lineWidth);
                lineString.Points.Add(new ElectronTransferModel.Geo.Point(new[] {endPoint.X, endPoint.Y, 0}));
                var id = PublicMethod.Instance.ToModelSpace(pl);

                var angle = PublicMethod.Instance.GetAngle(new Point2d(startPoint.X, startPoint.Y),
                                                           new Point2d(endPoint.X, endPoint.Y));


                var lld = new LateralLineData
                              {
                                  lineString = lineString,
                                  lineAngle = angle
                              };
                if (i == 0)
                    lld.startObjID=id;
                if (i == 1)
                    lld.endObjID = id;
                dxObjIdList.Add(id, lld);
            }
        }
        /// <summary>
        /// 获取杆塔层ObjectID
        /// </summary>
        /// <returns></returns>
        private ObjectId GetDGLayerID()
        {
            return DCadApi.addLayer("杆塔");
        }
        /// <summary>
        /// 获取导线图层ObjectID
        /// </summary>
        /// <returns></returns>
        private ObjectId GetDXLayerID()
        {
            return DCadApi.addLayer("10kV导线");
        }
        /// <summary>
        /// 获取杆塔样式
        /// </summary>
        /// <param name="dg"></param>
        /// <returns></returns>
        private InsertSymbolEventArgs GetDgStyle(XProps dg)
        {
            return DCadApi.InitializeInsertSymbolValue(201, dg, null);
        }
        /// <summary>
        /// 获取导线样式
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        private InsertSymbolEventArgs GetDxStyle(XProps dx)
        {
            var lineStyle = DCadApi.InitializeInsertSymbolValue(141, dx, null);
            lineStyle.lineWidth = lineStyle.lineWidth / MapConfig.Instance.earthscale;
            return lineStyle;
        }
        private ObjectId SetPolyLine(Polyline pline,CADColor color,ObjectId lineLayerId)
        {
            pline.LayerId = lineLayerId;
            pline.Color = color;
            id = PublicMethod.Instance.ToModelSpace(pline);
            return id;
        }
        #endregion

        #region 保护成员

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
                    promOpts.SetMessageAndKeywords(
                         "\n下一个点[Undo]: ",
                         "放弃"
                       );
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

        protected override bool Update()
        {
            (Entity as BlockReference).Position = m_lastPnt;
            /*                pline.SetPointAt(pline.NumberOfVertices - 1, new Point2d(m_lastPnt.X, m_lastPnt.Y));*/
            return true;
        }

        #endregion

        #region 私有成员

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
            filList[0] = new TypedValue((int) DxfCode.Start, "INSERT");
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
        #endregion
    }
    public class LateralLineData
    {
        public LineString lineString { set; get; }
        public double lineAngle { set; get; }
        public ObjectId startObjID { set; get; }
        public ObjectId endObjID { set; get; }
    }
}
