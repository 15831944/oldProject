using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronTransferDal.Common;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using ElectronTransferFramework;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using ElectronTransferModel.Geo;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using ElectronTransferDal;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;
using eex=Autodesk.AutoCAD.Runtime.Exception;

[assembly:CommandClass(typeof(ElectronTransferDal.Cad.DIYCommand))]
namespace ElectronTransferDal.Cad
{
    public class DIYCommand
    {
        /// <summary>
        /// 两点打断
        /// </summary>
        public static void BreakLine2()
        {
            DCadApi.isModifySymbol = true;
            DCadApi.isEraseRollback = true;
            Editor ed = PublicMethod.Instance.Editor;

            try
            {
                //选择直线
                var entityOption = new PromptEntityOptions("\n选择对象:");
                entityOption.SetRejectMessage("\n请选择导线或电缆！");
                entityOption.AddAllowedClass(typeof(Polyline), true);
                var entityObj = ed.GetEntity(entityOption);
                if (entityObj.Status == PromptStatus.OK)
                {
                    if (!GetObjectType(entityObj.ObjectId))
                    {
                        PublicMethod.Instance.AlertDialog("只能选择导线或电缆！");
                        return;
                    }
                    if (DBEntityFinder.Instance.VerifyLTTID(entityObj.ObjectId))
                    {
                        //获取原线段的对象
                        var oldline = (Polyline)PublicMethod.Instance.GetObject(entityObj.ObjectId, OpenMode.ForRead);
                        //选择第二打断点
                        var ppo1 = new PromptPointOptions("\n选择第一点");
                        var point1 = ed.GetPoint(ppo1);
                        if (point1.Status != PromptStatus.OK) return;

                        if(point1.Value.Equals(oldline.StartPoint) || point1.Value.Equals(oldline.EndPoint))
                        {
                            PublicMethod.Instance.AlertDialog("选择打断点错误！不能选择线段的起始点！");
                            return;
                        }

                        //选择第二打断点
                        var ppo2 = new PromptPointOptions("\n选择第二点");
                        ppo2.AllowNone = true;
                        var point2 = ed.GetPoint(ppo2);
                        if (point2.Value.Equals(oldline.StartPoint) || point2.Value.Equals(oldline.EndPoint))
                        {
                            PublicMethod.Instance.AlertDialog("选择打断点错误！不能选择线段的起始点！");
                            return;
                        }

                        var pars = new List<double>();
                        var closestPoint = oldline.GetClosestPointTo(point1.Value, false);
                        pars.Add(oldline.GetParameterAtPoint(closestPoint));

                        if (point2.Status == PromptStatus.OK)
                        {
                            //如果选择了第二点,获取直线上两点的param值,并排序
                            var point = oldline.GetClosestPointTo(point2.Value, false);
                            pars.Add(oldline.GetParameterAtPoint(point));
                            pars.Sort();

                            var index = 0;
                            //按param值打断曲线
                            var objs = oldline.GetSplitCurves(new DoubleCollection(pars.ToArray()));
                            foreach (Polyline newline in objs)
                            {
                                //如果生成的直线起点或终点不是选择的打断点,把它加入数据库
                                if ((newline.StartPoint != closestPoint && newline.StartPoint != point) ^
                                    (newline.EndPoint != closestPoint && newline.EndPoint != point))
                                {
                                    var newObjId = PublicMethod.Instance.ToModelSpace(newline);
                                    //复制数据
                                    CopyDBEntity(entityObj.ObjectId, newObjId, index);
                                    index++;
                                }
                            }

                            //删除图形
                            PublicMethod.Instance.EraseObject(entityObj.ObjectId);
                            //删除缓存
                            DBSymbolFinder.Instance.Remove(entityObj.ObjectId);
                        }
                    }
                    else
                    {
                        PublicMethod.Instance.AlertDialog("该设备未锁定，不能编辑！");
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
                DCadApi.isEraseRollback = false;
            }
        }

        /// <summary>
        /// 按点打断
        /// </summary>
        public static void Break()
        {
            DCadApi.isModifySymbol = true;
            DCadApi.isEraseRollback = true;
            try
            {
                var ed = PublicMethod.Instance.Editor;
                //选择直线
                PromptEntityOptions entityOption = new PromptEntityOptions("\n选择对象");
                entityOption.SetRejectMessage("\n请选择导线或电缆！");
                entityOption.AddAllowedClass(typeof(Polyline), true);
                var entityObj = ed.GetEntity(entityOption);

                if (entityObj.Status == PromptStatus.OK)
                {
                    if(!GetObjectType(entityObj.ObjectId))
                    {
                        PublicMethod.Instance.AlertDialog("只能选择导线或电缆！");
                        return;
                    }
                    if (DBEntityFinder.Instance.VerifyLTTID(entityObj.ObjectId))
                    {
                        var pars = new List<double>();
                        var point = PublicMethod.Instance.GetPoint("\n选择打断点");
                        if (point.X == 0)
                            return;
                        var oldline = (Polyline)PublicMethod.Instance.GetObject(entityObj.ObjectId, OpenMode.ForRead);
                        if(point.Equals(oldline.StartPoint) || point.Equals(oldline.EndPoint))
                        {
                            PublicMethod.Instance.AlertDialog("选择打断点错误！不能选择线段的起始点！");
                            return;
                        }

                        //按点打断线条
                        var closestPoint = oldline.GetClosestPointTo(new Point3d(point.X, point.Y, point.Z), false);
                        pars.Add(oldline.GetParameterAtPoint(closestPoint));
                        //打断
                        var objs = oldline.GetSplitCurves(new DoubleCollection(pars.ToArray()));

                        var index = 0;
                        foreach (Polyline newline in objs)
                        {
                            var newObjId = PublicMethod.Instance.ToModelSpace(newline);
                            //复制数据
                            CopyDBEntity(entityObj.ObjectId, newObjId, index);
                            index++;
                        }

                        //删除图形
                        PublicMethod.Instance.EraseObject(entityObj.ObjectId);
                        //删除缓存
                        DBSymbolFinder.Instance.Remove(entityObj.ObjectId);
                    }
                    else
                    {
                        PublicMethod.Instance.AlertDialog("该设备未锁定，不能编辑！");
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.ToString());
            }
            finally
            {
                DCadApi.isModifySymbol = false;
                DCadApi.isEraseRollback = false;
            }
        }
        private static bool GetObjectType(ObjectId objectId)
        {
            var value=DBSymbolFinder.Instance[objectId];
            if (value == null)
                return false;
            return value.G3E_GEOMETRY is LineString;
        }

        private static void CopyDBEntity(ObjectId oldObjId, ObjectId newObjId, int index)
        {
            if (index == 0)
            {
                var pt = DBSymbolFinder.Instance[oldObjId];
                if (pt != null)
                {
                    //更新坐标
                    var es = ConvertGeometry.Instance.UpdateG3E_GEOMETRY(pt, newObjId);
                    DBManager.Instance.Update(es);
                    PublicMethod.Instance.AddDBSymbolFinder(newObjId, es);
                    //var conn=DBEntityFinder.Instance.GetConnectivity_n(es.G3E_FID);
                    //if(conn!=null)
                    //{
                    //    conn.NODE1_ID = 0;
                    //    conn.NODE2_ID = 0;
                    //    DBManager.Instance.Update(conn);
                    //}
                }
            }
            else
            {
                //复制数据
                DBEntityCopy.Instance._copyObjects = DBEntityCopy.Instance.ObjectCopy(new[] { oldObjId });
                //新增数据
                DBEntityCopy.Instance._G3EIdMapping.Clear();
                //粘贴
                DBEntityCopy.Instance.ObjectPaste(oldObjId, newObjId);
                DBEntityCopy.Instance.UpdateConnectionData();
            }
        }
    }
}
