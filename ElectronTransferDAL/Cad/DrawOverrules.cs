
using Autodesk.AutoCAD.ApplicationServices;
 using Autodesk.AutoCAD.Runtime;
 using Autodesk.AutoCAD.DatabaseServices;
 using Autodesk.AutoCAD.EditorInput;
 using Autodesk.AutoCAD.Geometry;
 using Autodesk.AutoCAD.GraphicsInterface;
 using Autodesk.AutoCAD.Colors;

namespace DrawOverrules
{
    public abstract class PipeDrawOverrule : DrawableOverrule
    {
        private const string regAppName = "TTIF_PIPE";

        public PipeDrawOverrule()
        {
            // Tell AutoCAD to filter on our application name
            // (this means our overrule will only be called
            // on objects possessing XData with this name)

            SetXDataFilter(regAppName);
        }

        // Get the XData for a particular object
        // and return the "pipe radius" if it exists

        public static double PipeRadiusForObject(DBObject obj)
        {
            double res = 0.0;

            ResultBuffer rb = obj.XData;
            if (rb != null)
            {
                bool foundStart = false;

                foreach (TypedValue tv in rb)
                {
                    if (tv.TypeCode == (int) DxfCode.ExtendedDataRegAppName &&
                        tv.Value.ToString() == regAppName)
                        foundStart = true;
                    else
                    {
                        if (foundStart == true)
                        {
                            if (tv.TypeCode == (int) DxfCode.ExtendedDataReal)
                            {
                                res = (double) tv.Value;
                                break;
                            }
                        }
                    }
                }
                rb.Dispose();
            }
            return res;
        }

        // Set the "pipe radius" in the XData of a particular object

        public static void SetPipeRadiusOnObject(
            Transaction tr, DBObject obj, double radius
            )
        {
            Database db = obj.Database;

            // Make sure the application is registered
            // (we could separate this out to be called
            // only once for a set of operations)

            RegAppTable rat =
                (RegAppTable) tr.GetObject(
                    db.RegAppTableId,
                    OpenMode.ForRead
                                  );

            if (!rat.Has(regAppName))
            {
                rat.UpgradeOpen();
                RegAppTableRecord ratr = new RegAppTableRecord();
                ratr.Name = regAppName;
                rat.Add(ratr);
                tr.AddNewlyCreatedDBObject(ratr, true);
            }

            // Create the XData and set it on the object

            ResultBuffer rb =
                new ResultBuffer(
                    new TypedValue(
                        (int) DxfCode.ExtendedDataRegAppName, regAppName
                        ),
                    new TypedValue(
                        (int) DxfCode.ExtendedDataReal, radius
                        )
                    );
            obj.XData = rb;
            rb.Dispose();
        }
    }

    // An overrule to make a pipe out of line

    public class LinePipeDrawOverrule : PipeDrawOverrule
    {
        public static LinePipeDrawOverrule theOverrule =
            new LinePipeDrawOverrule();

        private SweepOptions sweepOpts = new SweepOptions();

        public override bool WorldDraw(Drawable d, WorldDraw wd)
        {
            double radius = 0.0;

            if (d is DBObject)
                radius = PipeRadiusForObject((DBObject) d);

            if (radius > 0.0)
            {
                Line line = d as Line;

                if (line != null)
                {
                    // Draw the line as is, with overruled attributes

                    base.WorldDraw(line, wd);
                    if (!line.Id.IsNull && line.Length > 0.0)
                    {
                        // Draw a pipe around the line

                        EntityColor c =
                            wd.SubEntityTraits.TrueColor;
                        wd.SubEntityTraits.TrueColor =
                            new EntityColor(0x00AfAfff);
                        wd.SubEntityTraits.LineWeight =
                            LineWeight.LineWeight000;
                        Circle clr =
                            new Circle(
                                line.StartPoint,
                                line.EndPoint - line.StartPoint,
                                radius
                                );
                        ExtrudedSurface pipe = new ExtrudedSurface();
                        try
                        {
                            pipe.CreateExtrudedSurface(
                                clr, line.EndPoint - line.StartPoint, sweepOpts
                                );
                        }
                        catch
                        {
                            Document doc =
                                Application.DocumentManager.MdiActiveDocument;
                            doc.Editor.WriteMessage(
                                "\nFailed with CreateExtrudedSurface."
                                );
                        }
                        clr.Dispose();
                        pipe.WorldDraw(wd);
                        pipe.Dispose();
                        wd.SubEntityTraits.TrueColor = c;
                    }
                    return true;
                }
            }
            return base.WorldDraw(d, wd);
        }

        public override int SetAttributes(Drawable d, DrawableTraits t)
        {
            int b = base.SetAttributes(d, t);

            double radius = 0.0;

            if (d is DBObject)
                radius = PipeRadiusForObject((DBObject) d);

            if (radius > 0.0)
            {
                // Set color to index 6

                t.Color = 6;

                // and lineweight to .40 mm

                t.LineWeight = LineWeight.LineWeight040;
            }
            return b;
        }
    }

    // An overrule to make a pipe out of circle

    public class CirclePipeDrawOverrule : PipeDrawOverrule
    {
        public static CirclePipeDrawOverrule theOverrule =
            new CirclePipeDrawOverrule();

        private SweepOptions sweepOpts = new SweepOptions();

        public override bool WorldDraw(Drawable d, WorldDraw wd)
        {
            double radius = 0.0;

            if (d is DBObject)
                radius = PipeRadiusForObject((DBObject) d);

            if (radius > 0.0)
            {
                Circle circle = d as Circle;

                if (circle != null)
                {
                    // Draw the circle as is, with overruled attributes

                    base.WorldDraw(circle, wd);

                    // Needed to avoid ill-formed swept surface

                    if (circle.Radius > radius)
                    {
                        // Draw a pipe around the cirle

                        EntityColor c = wd.SubEntityTraits.TrueColor;
                        wd.SubEntityTraits.TrueColor =
                            new EntityColor(0x3fffe0e0);
                        wd.SubEntityTraits.LineWeight =
                            LineWeight.LineWeight000;
                        Vector3d normal =
                            (circle.Center - circle.StartPoint).
                                CrossProduct(circle.Normal);
                        Circle clr =
                            new Circle(
                                circle.StartPoint, normal, radius
                                );
                        SweptSurface pipe = new SweptSurface();
                        pipe.CreateSweptSurface(clr, circle, sweepOpts);
                        clr.Dispose();
                        pipe.WorldDraw(wd);
                        pipe.Dispose();
                        wd.SubEntityTraits.TrueColor = c;
                    }
                    return true;
                }
            }
            return base.WorldDraw(d, wd);
        }

        public override int SetAttributes(Drawable d, DrawableTraits t)
        {
            int b = base.SetAttributes(d, t);

            double radius = 0.0;

            if (d is DBObject)
                radius = PipeRadiusForObject((DBObject) d);

            if (radius > 0.0)
            {
                // Set color to index 2

                t.Color = 2;

                // and lineweight to .60 mm

                t.LineWeight = LineWeight.LineWeight060;
            }
            return b;
        }
    }

    public class LinePipeTransformOverrule : TransformOverrule
    {
        public static LinePipeTransformOverrule theOverrule =
            new LinePipeTransformOverrule();

        private SweepOptions sweepOpts = new SweepOptions();

        public override void Explode(Entity e, DBObjectCollection objs)
        {
            double radius = 0.0;

            if (e is DBObject)
                radius = PipeDrawOverrule.PipeRadiusForObject(e);

            if (radius > 0.0)
            {
                Line line = e as Line;

                if (line != null)
                {
                    if (!line.Id.IsNull && line.Length > 0.0)
                    {
                        // Draw a pipe around the line

                        Circle clr =
                            new Circle(
                                line.StartPoint,
                                line.EndPoint - line.StartPoint,
                                radius
                                );
                        ExtrudedSurface pipe = new ExtrudedSurface();
                        try
                        {
                            pipe.CreateExtrudedSurface(
                                clr, line.EndPoint - line.StartPoint, sweepOpts
                                );
                        }
                        catch
                        {
                            Document doc =
                                Application.DocumentManager.MdiActiveDocument;
                            doc.Editor.WriteMessage(
                                "\nFailed with CreateExtrudedSurface."
                                );
                        }
                        clr.Dispose();
                        objs.Add(pipe);
                    }
                    return;
                }
            }
            base.Explode(e, objs);
        }
    }

    public class CirclePipeTransformOverrule : TransformOverrule
    {
        public static CirclePipeTransformOverrule theOverrule =
            new CirclePipeTransformOverrule();

        private SweepOptions sweepOpts = new SweepOptions();

        public override void Explode(Entity e, DBObjectCollection objs)
        {
            double radius = 0.0;

            if (e is DBObject)
                radius = PipeDrawOverrule.PipeRadiusForObject(e);

            if (radius > 0.0)
            {
                Circle circle = e as Circle;

                if (circle != null)
                {
                    // Needed to avoid ill-formed swept surface

                    if (circle.Radius > radius)
                    {
                        // Draw a pipe around the cirle

                        Vector3d normal =
                            (circle.Center - circle.StartPoint).
                                CrossProduct(circle.Normal);
                        Circle clr =
                            new Circle(
                                circle.StartPoint, normal, radius
                                );
                        SweptSurface pipe = new SweptSurface();
                        pipe.CreateSweptSurface(clr, circle, sweepOpts);
                        clr.Dispose();
                        objs.Add(pipe);
                    }
                    return;
                }
            }
            base.Explode(e, objs);
        }
    }


    public class CopyOverrule : ObjectOverrule
    {
        public static CopyOverrule theOverrule =
            new CopyOverrule();

        public override DBObject DeepClone(
            DBObject dbObject, DBObject ownerObject,
            IdMapping idMap, bool isPrimary
            )
        {
            // First we deep clone the object via the parent

            DBObject res =
                base.DeepClone(dbObject, ownerObject, idMap, isPrimary);

            // Then we check for our XData

            if (PipeDrawOverrule.PipeRadiusForObject(res) > 0.0)
            {
                // A transaction is needed by the set function to access
                // the RegApp table - we could also assume the app name
                // is registered and have a separate implementation
                // not taking the transaction...        
                // Just as we might also have chosen to remove the XData

                Transaction tr =
                    dbObject.Database.TransactionManager.StartTransaction();
                using (tr)
                {
                    PipeDrawOverrule.SetPipeRadiusOnObject(tr, res, 0.0);
                    tr.Commit();
                }
            }
            return res;
        }
    }

    public class Commands
    {
        private double _radius = 0.0;

        public void Overrule(bool enable)
        {
            // Regen to see the effect
            // (turn on/off Overruling and LWDISPLAY)

            DrawableOverrule.Overruling = enable;
            if (enable)
                Application.SetSystemVariable("LWDISPLAY", 1);
            else
                Application.SetSystemVariable("LWDISPLAY", 0);

            Document doc =
                Application.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute("REGEN3\n", true, false, false);
            doc.Editor.Regen();
        }

        [CommandMethod("OVERRULE1")]
        public void OverruleStart()
        {
            ObjectOverrule.AddOverrule(
                RXClass.GetClass(typeof (Line)),
                LinePipeDrawOverrule.theOverrule,
                true
                );
            ObjectOverrule.AddOverrule(
                RXClass.GetClass(typeof (Circle)),
                CirclePipeDrawOverrule.theOverrule,
                true
                );
            ObjectOverrule.AddOverrule(
                RXClass.GetClass(typeof (Line)),
                LinePipeTransformOverrule.theOverrule,
                true
                );
            ObjectOverrule.AddOverrule(
                RXClass.GetClass(typeof (Circle)),
                CirclePipeTransformOverrule.theOverrule,
                true
                );
            ObjectOverrule.AddOverrule(
                RXClass.GetClass(typeof (Entity)),
                CopyOverrule.theOverrule,
                true
                );
            Overrule(true);
        }

        [CommandMethod("OVERRULE0")]
        public void OverruleEnd()
        {
            ObjectOverrule.RemoveOverrule(
                RXClass.GetClass(typeof (Line)),
                LinePipeDrawOverrule.theOverrule
                );
            ObjectOverrule.RemoveOverrule(
                RXClass.GetClass(typeof (Circle)),
                CirclePipeDrawOverrule.theOverrule
                );
            ObjectOverrule.RemoveOverrule(
                RXClass.GetClass(typeof (Line)),
                LinePipeTransformOverrule.theOverrule
                );
            ObjectOverrule.RemoveOverrule(
                RXClass.GetClass(typeof (Circle)),
                CirclePipeTransformOverrule.theOverrule
                );
            ObjectOverrule.RemoveOverrule(
                RXClass.GetClass(typeof (Entity)),
                CopyOverrule.theOverrule
                );
            Overrule(false);
        }

        [CommandMethod("MP", CommandFlags.UsePickSet)]
        public void MakePipe()
        {
            Document doc =
                Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            // Ask the user to select the entities to make into pipes

            PromptSelectionOptions pso =
                new PromptSelectionOptions();
            pso.AllowDuplicates = false;
            pso.MessageForAdding =
                "\n选择对象变为管道: ";

            PromptSelectionResult selRes =
                doc.Editor.GetSelection(pso);

            // If the user didn't make valid selection, we return

            if (selRes.Status != PromptStatus.OK)
                return;

            SelectionSet ss = selRes.Value;

            // Ask the user for the pipe radius to set

            PromptDoubleOptions pdo =
                new PromptDoubleOptions(
                    "\n指定管道半径:"
                    );

            // Use the previous value, if if already called

            if (_radius > 0.0)
            {
                pdo.DefaultValue = _radius;
                pdo.UseDefaultValue = true;
            }
            pdo.AllowNegative = false;
            pdo.AllowZero = false;

            PromptDoubleResult pdr =
                ed.GetDouble(pdo);

            // Return if something went wrong

            if (pdr.Status != PromptStatus.OK)
                return;

            // Set the "last radius" value for when
            // the command is called next

            _radius = pdr.Value;

            // Use a transaction to edit our various objects

            Transaction tr =
                db.TransactionManager.StartTransaction();
            using (tr)
            {
                // Loop through the selected objects

                foreach (SelectedObject o in ss)
                {
                    // We could choose only to add XData to the objects
                    // we know will use it (Lines and Circles, for now)

                    DBObject obj =
                        tr.GetObject(o.ObjectId, OpenMode.ForWrite);
                    PipeDrawOverrule.SetPipeRadiusOnObject(tr, obj, _radius);
                }
                tr.Commit();
            }
        }
    }

}
