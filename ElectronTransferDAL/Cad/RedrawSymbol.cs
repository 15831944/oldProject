using System;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.Geo;
using CADColor=Autodesk.AutoCAD.Colors.Color;
using ElectronTransferModel;

namespace ElectronTransferDal.Cad
{
    /// <summary>
    /// 设备重绘
    /// </summary>
    public class RedrawSymbol
    {
        /// <summary>
        /// 重绘线设备和面设备
        /// </summary>
        /// <param name="objectId">对象ObjectID</param>
        public static void RedrawPolyLine(ObjectId objectId)
        {
            try
            {
                if (!DBEntityFinder.Instance.VerifyLTTID(objectId))
                {
                    MessageBox.Show("该设备未锁定！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                var entity = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead) as Entity;
                if (entity is Polyline)
                {
                    var sbmc = string.Empty;
                    //获取设备的坐标数据
                    var ptValue = DBSymbolFinder.Instance[objectId];
                    //获取设备的公共属性
                    var com = DBEntityFinder.Instance.GetCommon_n(ptValue.G3E_FID);
                    if (com!=null)
                    {
                        sbmc = !string.IsNullOrEmpty(com.SBMC) ? com.SBMC : com.G3E_FID.ToString();
                    }
                    if (MessageBox.Show(string.Format("确定要重绘【{0}】吗？", sbmc), "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                        DialogResult.OK)
                    {
                        var polyLine = entity as Polyline;
                        if (ptValue.G3E_GEOMETRY is LineString)
                        {
                            RedrawLine(polyLine, ptValue);
                        }
                        else if (ptValue.G3E_GEOMETRY is Polygon)
                        {
                            RedrawPolyGon(polyLine, ptValue);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("暂不支持重绘！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogManager.Instance.Error("重绘设备\n" + ex);
            }
        }
        /// <summary>
        /// 重绘线设备
        /// </summary>
        /// <param name="polyLine">线对象</param>
        /// <param name="ptValue">线设备坐标数据</param>
        private static void RedrawLine(Polyline polyLine, ElectronSymbol ptValue)
        {
            var objectId = polyLine.ObjectId;
            var laryerId = polyLine.LayerId;
            var color = polyLine.Color;
            var lineWidth = polyLine.GetStartWidthAt(0);
            var lineType = polyLine.Linetype;
            var lineString = new LineString();
            //重新绘制图形
            var newObjectId = PublicMethod.Instance.DrawPolyLineJig(lineWidth, color, lineType, laryerId, ref lineString);
            if (newObjectId.IsNull) return;
            ptValue.G3E_GEOMETRY = lineString;
            //判断状态
            if (ptValue.EntityState == EntityState.None)
                ptValue.EntityState = EntityState.Update;
            //更新数据
            DBManager.Instance.Update(ptValue);
            //添加
            PublicMethod.Instance.AddDBSymbolFinder(newObjectId, ptValue);
            //移除旧数据
            DBSymbolFinder.Instance.Remove(objectId);

            //删除图形
            EraseObject(objectId);
            MessageBox.Show("操作成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        /// <summary>
        /// 重绘面设备
        /// </summary>
        /// <param name="polyLine">线对象</param>
        /// <param name="ptValue">面设备坐标数据</param>
        private static void RedrawPolyGon(Polyline polyLine, ElectronSymbol ptValue)
        {
            var objectId = polyLine.ObjectId;
            var laryerId = polyLine.LayerId;
            var color = polyLine.Color;
            var lineWidth = polyLine.GetStartWidthAt(0);
            var polyGon = new Polygon();
            var newObjectId = PublicMethod.Instance.DrawRectangle(laryerId, color, lineWidth, ref polyGon);
            if (newObjectId.IsNull) return;
            ptValue.G3E_GEOMETRY = polyGon;
            if (ptValue.EntityState == EntityState.None)
                ptValue.EntityState = EntityState.Update;
            DBManager.Instance.Update(ptValue);
            //添加
            PublicMethod.Instance.AddDBSymbolFinder(newObjectId, ptValue);
            //移除旧数据
            DBSymbolFinder.Instance.Remove(objectId);

            //删除图形
            EraseObject(objectId);
            MessageBox.Show("操作成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        /// <summary>
        /// 删除图形
        /// </summary>
        /// <param name="objectId">对象ObjectID</param>
        private static void EraseObject(ObjectId objectId)
        {
            try
            {
                DCadApi.isEraseRollback = true;
                PublicMethod.Instance.EraseObject(objectId);
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }finally
            {
                DCadApi.isEraseRollback = false;
            }
        }
    }
}
