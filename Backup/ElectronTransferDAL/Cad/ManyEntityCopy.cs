using System.Collections;
using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;

namespace ElectronTransferDal.Cad
{
    public class ManyEntityCopy:DrawJig
    { 
        #region 私有成员

        private Point3d m_basePoint;
        private Point3d m_previousCursorPosition;
        private Point3d m_currentCursorPosition;
        private Matrix3d m_displacemenMatrix = Matrix3d.Identity;
        private List<Entity> m_entitiesToDrag;

        #endregion

        #region 共有成员

        public virtual PromptResult StartDrag(Point3d basePoint, params ObjectId[] entitiesToDrag)
        {
            m_basePoint = TransformPointToCurrentUcs(basePoint);

            // 准备初始化变换
            m_previousCursorPosition = m_basePoint;

            m_entitiesToDrag = CloneEntitiesToDrag(entitiesToDrag);

            return Application.DocumentManager.MdiActiveDocument.Editor.Drag(this);
        }

        public virtual Matrix3d DisplacementMatrix
        {
            get
            {
                var displacementVector = m_basePoint.GetVectorTo(m_currentCursorPosition);
                return Matrix3d.Displacement(TransformVectorToWcs(displacementVector));
            }
        }

        #endregion

        #region 保护成员
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            var pntops = new JigPromptPointOptions {Message = "请选择插入点\n"};
            var userFeedback = prompts.AcquirePoint(pntops);
            m_currentCursorPosition = userFeedback.Value;

            if (CursorHasMoved())
            {
                // 得到移动方向
                var displacementVector = m_previousCursorPosition.GetVectorTo(m_currentCursorPosition);

                // 得到移动矩阵
                m_displacemenMatrix = Matrix3d.Displacement(displacementVector);

                // 保存鼠标位置
                m_previousCursorPosition = m_currentCursorPosition;

                return SamplerStatus.OK;
            }
            return SamplerStatus.NoChange;
        }

        protected override bool WorldDraw(WorldDraw draw)
        {
            var displacementMatrix = m_displacemenMatrix;

            foreach (var draggingEntity in m_entitiesToDrag)
            {
                draggingEntity.TransformBy(displacementMatrix);
                draw.Geometry.Draw(draggingEntity);
            }
            return true;
        }

        #endregion

        #region 私有成员

        protected List<Entity> CloneEntitiesToDrag(IList entities)
        {
            var entitiesToDrag = new List<Entity>(entities.Count);

            using (Transaction dragTrans = Application.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction())
            {
                foreach (ObjectId entityId in entities)
                {
                    var drawableEntity = (Entity)dragTrans.GetObject(entityId, OpenMode.ForRead);
                    drawableEntity.UpgradeOpenAndRun();
                    //entitiesToDrag.Add(drawableEntity);
                    entitiesToDrag.Add((Entity)drawableEntity.Clone());
                }
            }
            return entitiesToDrag;
        }

        private bool CursorHasMoved()
        {
            return m_currentCursorPosition != m_previousCursorPosition;
        }

        private Vector3d TransformVectorToWcs(Vector3d vector)
        {
            return vector.TransformBy(Matrix3d.Identity);
        }

        private Point3d TransformPointToCurrentUcs(Point3d basePoint)
        {
            var currentUcs = Application.DocumentManager.MdiActiveDocument.Editor.CurrentUserCoordinateSystem;

            return basePoint.TransformBy(currentUcs);
        }

        #endregion
    }
}
