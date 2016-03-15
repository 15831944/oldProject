using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;

namespace ElectronTransferDal.Cad
{
    public class LabelTransOverrule : TransformOverrule
    {
        public static LabelTransOverrule theOverrule =
            new LabelTransOverrule();

        internal static List<ObjectId> _curves =
            new List<ObjectId>();

        private static bool overruling = false;

        public LabelTransOverrule()
        {
        }


        public override void TransformBy(Entity e, Matrix3d mat)
        {	  
            DBPoint pt = e as DBPoint;
            if (pt != null)
            {
                Database db = HostApplicationServices.WorkingDatabase;

                bool found = false;

                OpenCloseTransaction tr =
                    db.TransactionManager.StartOpenCloseTransaction();
                using (tr)
                {

                    foreach (ObjectId curId in _curves)
                    {
                        DBObject obj = tr.GetObject(curId, OpenMode.ForRead);
                        Polyline cur = obj as Polyline;
                        if (cur != null)
                        {

                            Point3d ptOnCurve =
                                cur.GetClosestPointTo(pt.Position, false);
                            Vector3d dist = ptOnCurve - pt.Position;
                            if (dist.IsZeroLength(Tolerance.Global))
                            {
                                Point3d pos =
                                    cur.GetClosestPointTo(
                                        pt.Position.TransformBy(mat),
                                        false
                                        );
                                pt.Position = pos;
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            base.TransformBy(e, mat);
                        }
                    }
                }
            }
        }


        [CommandMethod("POC")]
        public static void CreatePointOnCurve(ObjectId curId, Point3d pickedPoint)
        {
            Document doc =
                Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // Ask the user to select a curve

            PromptEntityOptions opts =
                new PromptEntityOptions(
                    "\n选择线: "
                    );
            opts.SetRejectMessage(
                "\n实体必须是线."
                );
            opts.AddAllowedClass(typeof(Polyline), false);

            PromptEntityResult per = ed.GetEntity(opts);

            curId = per.ObjectId;
            if (curId != ObjectId.Null)
            {
                // Let's make sure we'll be able to see our point
using (doc.LockDocument())
                    {
                db.Pdmode = 97; // square with a circle
                db.Pdsize = -10; // relative to the viewport size

                Transaction tr =
                    doc.TransactionManager.StartTransaction();
                using (tr)
                {
                    
                        DBObject obj =
                            tr.GetObject(curId, OpenMode.ForRead);

                        Curve cur = obj as Curve;
                        if (cur != null)
                        {
                            // Out initial point should be the closest point
                            // on the curve to the one picked

                            Point3d pos =
                                cur.GetClosestPointTo(pickedPoint, false);
                            DBPoint pt = new DBPoint(pos);

                            // Add it to the same space as the curve

                            BlockTableRecord btr =
                                (BlockTableRecord) tr.GetObject(
                                    cur.BlockId,
                                    OpenMode.ForWrite
                                                       );
                            ObjectId ptId = btr.AppendEntity(pt);
                            tr.AddNewlyCreatedDBObject(pt, true);
                        }
                        tr.Commit();
                    }
                    // And add the curve to our central list

                    _curves.Add(curId);
                }

                // Turn on the transform overrule if it isn't already

                if (!overruling)
                {
                    AddOverrule(
                        GetClass(typeof (DBPoint)),
                        theOverrule,
                        true
                        );
                    overruling = true;
                    Overruling = true;
                }
            }
        }
    }
}
