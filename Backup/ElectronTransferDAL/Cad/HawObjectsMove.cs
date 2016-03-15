using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace ElectronTransferDal.Cad
{
    public class HawObjectsMove:DrawJig
    {
         //变量
        Point3d oldPt;//实体移动之前的位置
        Point3d newPt;//实体移动之后的位置
        Vector3d v;//实体移动的向量

        List<Entity> ents = new List<Entity>();
        List<Point3d> oldPts = new List<Point3d>();
        public HawObjectsMove(List<Entity> ents)
        {
            oldPt = Point3d.Origin;
            newPt = Point3d.Origin;
            v = new Vector3d();
            this.ents = ents;

            Entity ent = ents[0];
            if (ent is BlockReference)
            {
                BlockReference br = ent as BlockReference;
                oldPt = br.Position;
            }

            for (int i = 1; i < ents.Count; i++)
            {
                if (ents[i] is BlockReference)
                {
                    BlockReference br = ents[i] as BlockReference;
                    oldPts.Add(br.Position);
                }
            }
        }

        //更新
        protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        {
            Entity ent = ents[0];
            ent.UpgradeOpen();
            if (ent is BlockReference)
            {
                BlockReference br = ent as BlockReference;
                br.Position = newPt;
                draw.Geometry.Draw(br);
            }

            v = newPt.GetVectorTo(oldPt); 
            
            for(int i =1;i<ents.Count;i++)
            {
                Entity entity = ents[i];
                entity.UpgradeOpen();
                if (entity is BlockReference)
                {
                    BlockReference br = entity as BlockReference;
                    br.Position = oldPts[i - 1] + v;
                    draw.Geometry.Draw(entity);
                }
            }

            return true;
        }
        //取样函数
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions promtOp = new JigPromptPointOptions();
            promtOp.UserInputControls =
                (UserInputControls.Accept3dCoordinates
                 | UserInputControls.NoNegativeResponseAccepted
                 | UserInputControls.NoZeroResponseAccepted
                 | UserInputControls.NullResponseAccepted);
            promtOp.Message = "指定插入点: \n";

            PromptPointResult res = prompts.AcquirePoint(promtOp);
            if (PromptStatus.OK == res.Status)
            {
                if (res.Value == newPt)
                {
                    return SamplerStatus.NoChange;
                }
                newPt = res.Value;
            }
            else
            {
                return SamplerStatus.Cancel;
            }
            return SamplerStatus.OK;
        }
    }
}
