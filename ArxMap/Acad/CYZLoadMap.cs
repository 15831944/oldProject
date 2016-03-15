using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;

using ElectronTransferDal;

namespace ArxMap.Acad
{
    public class CYZLoadMap
    {
        public string ImgName;
        public string ImgFullName;

        private ObjectId parEntityId;
        private ObjectId objInTabEntityId;
           
        public bool newImg()
        {
            DocumentLock doclock=null;
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                doclock = doc.LockDocument();

                Database db = HostApplicationServices.WorkingDatabase;

                ObjectId bjdtId = ElectronTransferDal.Cad.DCadApi.addLayer("背景地图");
                if (bjdtId != ObjectId.Null)
                {
                    db.Clayer = bjdtId;

                    Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = db.TransactionManager;

                    using (Transaction t = tm.StartTransaction())
                    {
                        try
                        {
                            var imgDictID = RasterImageDef.GetImageDictionary(db);
                            DBDictionary imgDict;
                            if (imgDictID.OldId == 0)
                            {
                                imgDictID = RasterImageDef.CreateImageDictionary(db);
                            }

                            // create the raster image definition
                            RasterImageDef rasterDef = new RasterImageDef();

                            rasterDef.SourceFileName = ImgFullName;
                            rasterDef.Load();

                            // test the image dictionary and the raster before going further 
                            bool bTestLoad = rasterDef.IsLoaded;

                            imgDict = (DBDictionary)t.GetObject(imgDictID, OpenMode.ForWrite);

                            // add the raster definition to the dictionary iff it doesn't already exist
                            ObjectId rasterDefID;
                            if (!imgDict.Contains(ImgName))
                            {
                                rasterDefID = imgDict.SetAt(ImgName, rasterDef);
                            }

                            t.AddNewlyCreatedDBObject(rasterDef, true);

                            // now add the REAL raster image reference
                            RasterImage rasterRef = new RasterImage();
                            rasterRef.ImageDefId = rasterDef.ObjectId;
                            ObjectId testRefID = rasterRef.ObjectId;

                            BlockTable bt = (BlockTable)tm.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                            BlockTableRecord btr = (BlockTableRecord)tm.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                            btr.AppendEntity(rasterRef);

                            tm.AddNewlyCreatedDBObject(rasterRef, true);

                            rasterRef.AssociateRasterDef(rasterDef);

                            parEntityId = rasterRef.ObjectId;

                        }
                        catch (Autodesk.AutoCAD.Runtime.Exception eex)
                        { }
                        catch (System.Exception ex) { }

                        t.Commit();
                    }
                }

            }
            catch (Autodesk.AutoCAD.Runtime.Exception eex)
            {
                return false;
            }
            catch (System.Exception ex) { return false; }
            finally
            {
                doclock.Dispose(); 
            }

            return true;
        }


        void tttttt(object oo)
        {
            try
            {
                RasterImageDef __rasterDef = (RasterImageDef)oo;
                __rasterDef.Load();
            }
            catch (System.Exception ex) {  }
        }

        public bool removeobj()
        {
            DocumentLock doclock = null;
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                doclock = doc.LockDocument();

                RasterImage pAcDbRasterImage;		// Entity

                Database db = HostApplicationServices.WorkingDatabase;
                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = db.TransactionManager;

                using (Transaction t = tm.StartTransaction())
                {
                    try
                    {
                        pAcDbRasterImage = (RasterImage)t.GetObject(parEntityId, OpenMode.ForWrite);

                        if (pAcDbRasterImage != null)
                        {
                            pAcDbRasterImage.Erase();
                            // Close object.
                            pAcDbRasterImage.Close();

                            removeImgDefByName();
                        }
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception eex)
                    { }
                    catch (System.Exception ex) { }
                    t.Commit();
                }

            }
            catch (Autodesk.AutoCAD.Runtime.Exception eex)
            { return false; }
            catch (System.Exception ex) { return false; }
            finally 
            {
                doclock.Dispose();
            }

            return true;
        }

        public bool removeImgDefByName()
        {
            DocumentLock doclock = null;
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                doclock = doc.LockDocument();

                Database db = HostApplicationServices.WorkingDatabase;
                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = db.TransactionManager;

                using (Transaction t = tm.StartTransaction())
                {
                    try
                    {
                        // open the dictionary
                        Autodesk.AutoCAD.DatabaseServices.ObjectId imgDictID = RasterImageDef.GetImageDictionary(db);
                        DBDictionary imgDict;
                        if (imgDictID.OldId == 0)
                        {
                            imgDictID = RasterImageDef.CreateImageDictionary(db);
                        }

                        imgDict = (DBDictionary)t.GetObject(imgDictID, OpenMode.ForWrite);

                        if (imgDict.Contains(ImgName))
                        {
                            ObjectId rasterDefID = imgDict.GetAt(ImgName);
                            RasterImageDef rasterDef = (RasterImageDef)t.GetObject(rasterDefID, OpenMode.ForWrite);

                            rasterDef.Erase();


                            imgDict.Remove(ImgName);
                        }
                        imgDict.Close();
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception eex)
                    { }
                    catch (System.Exception ex) { }
                    t.Commit();
                }

            }
            catch (Autodesk.AutoCAD.Runtime.Exception eex)
            { return false; }
            catch (System.Exception ex) { return false; }
            finally
            {
                doclock.Dispose(); 
            }

            return true;

        }

        public bool setImgPosition(double _x, double _y, double _z)
        {
            DocumentLock doclock = null;
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                doclock = doc.LockDocument();

                RasterImage pAcDbRasterImage;		// Entity

                Database db = HostApplicationServices.WorkingDatabase; 
                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = db.TransactionManager;

                using (Transaction t = tm.StartTransaction())
                {
                    try
                    {
                        pAcDbRasterImage = (RasterImage)t.GetObject(parEntityId, OpenMode.ForWrite);
                        
                        if (pAcDbRasterImage != null)
                        {
                            Autodesk.AutoCAD.Geometry.Vector3d ptag = new Autodesk.AutoCAD.Geometry.Vector3d(_x, _y, _z);

                            Autodesk.AutoCAD.Geometry.Matrix3d mat = new Autodesk.AutoCAD.Geometry.Matrix3d();

                            mat = Autodesk.AutoCAD.Geometry.Matrix3d.Displacement(ptag);



                            pAcDbRasterImage.TransformBy(mat);

                            // Close object.
                            pAcDbRasterImage.Close();
                        }

                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception eex)
                    { }
                    catch (System.Exception ex) { }
                    t.Commit();
                }

            }
            catch (Autodesk.AutoCAD.Runtime.Exception eex)
            { return false; }
            catch (System.Exception ex) { return false; }
            finally
            {
                doclock.Dispose();
            }


            return true;
        }


        public bool setImgScale(double _xscale, double _yscale, double _zscale)
        {
            DocumentLock doclock = null;
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                doclock = doc.LockDocument();

                RasterImage pAcDbRasterImage;		// Entity

                Database db = HostApplicationServices.WorkingDatabase; 
                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = db.TransactionManager;

                using (Transaction t = tm.StartTransaction())
                {
                    try
                    {
                        pAcDbRasterImage = (RasterImage)t.GetObject(parEntityId, OpenMode.ForWrite);

                        if (pAcDbRasterImage != null)
                        {
                            double[] ddd = new Autodesk.AutoCAD.Geometry.Matrix3d().ToArray();
                            ddd[0] = _xscale;
                            ddd[5] = _yscale;
                            ddd[10] = _zscale;
                            ddd[15] = 1.0;
                            Autodesk.AutoCAD.Geometry.Matrix3d mat = new Autodesk.AutoCAD.Geometry.Matrix3d(ddd);

                           
                            pAcDbRasterImage.TransformBy(mat);

                            // Close object.
                            pAcDbRasterImage.Close();
                        }

                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception eex)
                    { }
                    catch (System.Exception ex) { }
                    t.Commit();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception eex)
            { return false; }
            catch (System.Exception ex) { return false; }
            finally
            {
                doclock.Dispose();
            }


            return true;
        }

        /// <summary>
        /// 􁬍􀦬􁇍􄈵􂱘􃒬􀳒􂃵􁑣􀠄􄹊􁈖
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="db"></param>
        public bool MoveTop(bool istop)
        {
            DocumentLock doclock = null;
            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                doclock = doc.LockDocument();

                Database db = HostApplicationServices.WorkingDatabase;

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        ObjectIdCollection idc = new ObjectIdCollection();
                        idc.Add(parEntityId);

                        BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite, false);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                        DrawOrderTable orderTable = tr.GetObject(btr.DrawOrderTableId, OpenMode.ForWrite) as DrawOrderTable;
                        if (istop)
                        {
                            orderTable.MoveToTop(idc);
                        }
                        else
                        {
                            orderTable.MoveToBottom(idc);
                        }
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception eex)
                    { }
                    catch (System.Exception ex) { }
                    tr.Commit();
                }
                
            }
            catch (Autodesk.AutoCAD.Runtime.Exception eex)
            { return false; }
            catch (System.Exception ex) { return false; }
            finally
            {
                doclock.Dispose();
            }


            return true;
        }


    }
}