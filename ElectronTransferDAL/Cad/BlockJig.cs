using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;

namespace ElectronTransferDal.Cad
{
    public class BlockJig:EntityJig
    { 
        #region Fields

        private bool bl;
        public int mCurJigFactorNumber = 1;

        public static Point3d mPosition;
        public static double mdeltaAngle, mbaseAngle;
        public static Point3d EndPoint;

        #endregion

        #region Constructors

        public BlockJig(Entity ent, bool result)
            : base(ent)
        {
            mbaseAngle = 0.0;
            bl = result;
        }

        #endregion

        #region Overrides

        protected override bool Update()
        {
            switch (mCurJigFactorNumber)
            {
                case 1:
                    if (Entity is BlockReference)
                        (Entity as BlockReference).Position = mPosition;
                    else if (Entity is DBText)
                    {
                        (Entity as DBText).Position = mPosition;
                        (Entity as DBText).AlignmentPoint = mPosition;
                    }else if(Entity is MText)
                    {
                        (Entity as MText).Location = mPosition;
                    }
                    break;
                case 2:
                    if (bl)
                    {
                        var mat = Matrix3d.Rotation(mdeltaAngle - mbaseAngle,
                                                         PublicMethod.Instance.Editor.CurrentUserCoordinateSystem.
                                                             CoordinateSystem3d.
                                                             Zaxis,
                                                         mPosition);
                        if (Entity is BlockReference)
                            (Entity as BlockReference).TransformBy(mat);
                        else if(Entity is DBText)
                            (Entity as DBText).TransformBy(mat);
                        else if (Entity is MText)
                            (Entity as MText).TransformBy(mat);
                        mbaseAngle = mdeltaAngle;
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            switch (mCurJigFactorNumber)
            {
                case 1:
                    var promtOp1 = new JigPromptPointOptions
                                       {
                                           UserInputControls = (UserInputControls.Accept3dCoordinates
                                                                | UserInputControls.NoNegativeResponseAccepted
                                                                | UserInputControls.NoZeroResponseAccepted
                                                                | UserInputControls.NullResponseAccepted),
                                           Message = "指定插入点: \n"
                                       };

                    var prResult1 = prompts.AcquirePoint(promtOp1);
                    if (prResult1.Value.Equals(mPosition))
                        return SamplerStatus.NoChange;
                    if (prResult1.Status == PromptStatus.OK)
                    {

                        mPosition = prResult1.Value;
                        return SamplerStatus.OK;
                    }
                    return SamplerStatus.Cancel;
                case 2:
                    if (bl)
                    {
                        var promtOpt2 = new JigPromptAngleOptions
                                            {
                                                UserInputControls = UserInputControls.NullResponseAccepted,
                                                BasePoint = mPosition,
                                                UseBasePoint = true,
                                                Cursor = CursorType.RubberBand,
                                                Message = "指定旋转角度: \n"
                                            };

                        var prResult2 = prompts.AcquireAngle(promtOpt2);

                        if (prResult2.Status == PromptStatus.OK)
                        {
                            if (prResult2.Value.Equals(mdeltaAngle))
                                return SamplerStatus.NoChange;
                            mdeltaAngle = prResult2.Value;
                            return SamplerStatus.OK;
                        }
                    }
                    return SamplerStatus.Cancel;
            }
            return SamplerStatus.OK;
        }

        #endregion

        #region Method to Call

        /// <summary>
        /// 步骤模仿
        /// </summary>
        /// <param name="bref"></param>
        /// <param name="result">是否有旋转操作</param>
        /// <returns></returns>
        public static bool Jig(Entity bref,bool result)
        {
            try
            {
                var jigger = new BlockJig(bref, result);
                PromptResult pr = null;
                pr = PublicMethod.Instance.Editor.Drag(jigger);
                if (pr.Status != PromptStatus.OK)
                {
                    return false;
                }
                jigger.mCurJigFactorNumber++;
                PublicMethod.Instance.Editor.Drag(jigger);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
