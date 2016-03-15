using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferModel.Geo;
using Vector3d = Autodesk.AutoCAD.Geometry.Vector3d;

namespace ElectronTransferDal.Cad
{
    public class PolylineJig : EntityJig
    {
        private readonly Point3dCollection m_pts;
        public Point3d m_tempPoint;
        private readonly Plane m_plane;
        public LineString lineValue = new LineString();
        private double lineWidth;
        public PolylineJig(Matrix3d ucs,double width)
            : base(new Polyline())
        {
            m_pts = new Point3dCollection();
            var origin = new Point3d(0, 0, 0);
            var normal = new Vector3d(0, 0, 1);
            normal = normal.TransformBy(ucs);
            m_plane = new Plane(origin, normal);
            var pline = Entity as Polyline;
            pline.SetDatabaseDefaults();
            pline.Normal = normal;
            pline.AddVertexAt(0, new Point2d(0, 0), 0, width, width);

            lineWidth = width;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var jigOpts =
                new JigPromptPointOptions();
            jigOpts.UserInputControls =
                (UserInputControls.Accept3dCoordinates |
                 UserInputControls.NullResponseAccepted |
                 UserInputControls.NoNegativeResponseAccepted
                );
            if (m_pts.Count == 0)
            {
                jigOpts.Message =
                    "\n选择第一个点: ";
            }
            else if (m_pts.Count > 0)
            {
                jigOpts.UseBasePoint = true;
                jigOpts.Cursor = CursorType.RubberBand;
                jigOpts.BasePoint = m_pts[m_pts.Count - 1];
                jigOpts.Message = "\n下一个点: ";
                jigOpts.SetMessageAndKeywords(
                           "\n下一个点[Undo]: ",
                           "放弃"
                         );
            }
            PromptPointResult res =prompts.AcquirePoint(jigOpts);
            if (res.Value == m_tempPoint)
            {
                return SamplerStatus.NoChange;
            }
            if (res.Status == PromptStatus.OK)
            {
                m_tempPoint = res.Value;
                return SamplerStatus.OK;
            }
            return SamplerStatus.Cancel;
        }

        protected override bool Update()
        {
            Polyline pline = Entity as Polyline;
            pline.SetPointAt(pline.NumberOfVertices - 1,m_tempPoint.Convert2d(m_plane));
            return true;
        }

        public Entity GetEntity()
        {
            return Entity;
        }

        public void AddLatestVertex(Color color, string lineTypeStr, ObjectId layerId)
        {
            m_pts.Add(m_tempPoint);
            Polyline pline = Entity as Polyline;
            pline.Color = color;
            pline.LayerId = layerId;
            pline.Linetype = lineTypeStr;
            pline.LinetypeScale = 2;
            pline.AddVertexAt(pline.NumberOfVertices, new Point2d(0, 0), 0, lineWidth, lineWidth);
         
            lineValue.Points.Add(new Point(new[] { m_tempPoint.X, m_tempPoint.Y, 0 }));
        }

        public void RemoveLastVertex()
        {
            m_pts.RemoveAt(m_pts.Count - 1);
            Polyline pline = Entity as Polyline;
            pline.RemoveVertexAt(m_pts.Count);

            lineValue.Points.RemoveAt(lineValue.Points.Count - 1);
        }

        public void CompletePolyline()
        {
            Polyline pline = Entity as Polyline;
            if(pline.NumberOfVertices > 0)
            {
                pline.RemoveVertexAt(pline.NumberOfVertices - 1);
            }
        }
    }
}
