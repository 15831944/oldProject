using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.ApplicationServices;

namespace ElectronTransferDal.Cad
{
    public class MirrorJip: DrawJig
    {
        public Point3d m_MirrorPt1, m_MirrorPt2;//镜像的两个端点
        private Entity[] m_EntArr;//镜像源对象数组
        private Entity[] m_EntCopyArr;//镜像克隆对象数组
        private Matrix3d m_InverseMt = Matrix3d.Identity;//镜像逆矩阵
        public Matrix3d temp { get; set; }
        public MirrorJip(Point3d mirrorPt1, Entity[] entArr, Entity[] entCopyArr)
        {
            m_MirrorPt2 = m_MirrorPt1 = mirrorPt1;
            m_EntArr = entArr;
            m_EntCopyArr = entCopyArr;
        }
        //用传入的第一个点构造
        protected override bool WorldDraw(WorldDraw draw)
        {
            for (int i = 0; i < m_EntCopyArr.Length; i++)
            {
                draw.Geometry.Draw(m_EntCopyArr[i]);
            }
            return true;
        }
        /// <summary>
        /// 用来指定第二点镜像实体
        /// </summary>
        /// <param name="prompts"></param>
        /// <returns></returns>
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Matrix3d mt = ed.CurrentUserCoordinateSystem;
            JigPromptPointOptions optJip = new JigPromptPointOptions("\n指定镜像第二个点:");
            optJip.Cursor = CursorType.RubberBand;
            //指定基点
            Point3d wcsMirrorpt1 = m_MirrorPt1.TransformBy(mt);
            optJip.BasePoint = wcsMirrorpt1;
            optJip.UseBasePoint = true;
            PromptPointResult resJip = prompts.AcquirePoint(optJip);
            Point3d curpt = resJip.Value;
            if (m_MirrorPt2 != curpt)
            {
                m_MirrorPt2 = curpt;
                Matrix3d mirrorMt = Matrix3d.Mirroring(new Line3d(m_MirrorPt2, wcsMirrorpt1));
               
                for (int i = 0; i < m_EntCopyArr.Length; i++)
                {
                    m_EntCopyArr[i].TransformBy(m_InverseMt);
                    m_EntCopyArr[i].TransformBy(mirrorMt);
                }
                //在拖拽过程中，每一次Sampler函数响应时，用TransformBy变换镜像对象的位置后
                //需要用Inverse方法，得到变换的逆矩阵，在下一次响应Sampler函数时，先将镜像对象
                //变换回原来的位置，再进行新的变换
                m_InverseMt = mirrorMt.Inverse();
                temp = mirrorMt;
                return SamplerStatus.OK;
            }
            return SamplerStatus.NoChange;
        }


    }
}
