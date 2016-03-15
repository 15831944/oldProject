using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using ElectronTransferDal.Cad;
using ElectronTransferModel;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using ElectronTransferDal.Cad;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace MouseTip
{
    public class MouseTip
    {
        //存储鼠标停留处的块参照的ObjectId
        static ObjectIdCollection blockIds = new ObjectIdCollection();
        static ObjectId preBlockId = ObjectId.Null;//存储鼠标停留处的块名
        [CommandMethod("StartMonitor")]
        public static void StartMonitor()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //添加鼠标监视事件
            ed.PointMonitor += new PointMonitorEventHandler(ed_PointMonitor);
        }
        static void ed_PointMonitor(object sender, PointMonitorEventArgs e)
        {
            string blockInfo = ""; //用于存储块参照的信息：名称和个数
            //获取命令行对象（鼠标监视事件的发起者），用于获取文档对象
            Editor ed = (Editor)sender;
            Document doc = ed.Document;
            //获取鼠标停留处的实体
            FullSubentityPath[] paths = e.Context.GetPickedEntities();
            try
            {
                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    //如果鼠标停留处有实体
                    if (paths.Length > 0)
                    {
                        //获取鼠标停留处的实体
                        FullSubentityPath path = paths[0];
                        BlockReference blockRef = trans.GetObject(path.GetObjectIds()[0], OpenMode.ForRead) as BlockReference;
                        if (blockRef != null)//如果鼠标停留处为块参照
                        {
                            //获取块参照所属的块表记录并以写的方式打开
                            //ObjectId blockId = blockRef.BlockTableRecord;
                            ObjectId blockId = path.GetObjectIds()[0];
                            //BlockTableRecord btr = trans.GetObject(blockId, OpenMode.ForRead) as BlockTableRecord;
                            long fid = 0, fno = 0, id = 0;
                            PublicMethod.GetInfoByObjid(blockId, ref id, ref fid, ref fno);
                            //获取属于同一块表记录的所有块参照
                            //ObjectIdCollection ids = btr.GetBlockReferenceIds(true, false);
                            //若鼠标停留处的块参照的块表记录与上一次的不同
                            if (preBlockId != blockId)
                            {
                                preBlockId = blockId;//重新设置块表记录名
                            }
                            string fnoName = default(string);
                            if (fno != 0)
                                fnoName = FeatureMapping.instance.features[fno.ToString()];
                            blockInfo += "FID : " + fid.ToString() + "\n设备类型 : " + fnoName;
                        }

                    }
                    trans.Commit();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage(ex.Message);
            }

            if (blockInfo != "")
            {
                e.AppendToolTipText(blockInfo);//在鼠标停留处显示提示信息                  
            }
        }
        [CommandMethod("StopMonitor")]
        public static void StopMonitor()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //不亮显块参照
            //blockIds.UnHighlightEntities();
            //blockIds.Clear();
            //停止鼠标监视事件
            ed.PointMonitor -= new PointMonitorEventHandler(ed_PointMonitor);
        }
    }
}
