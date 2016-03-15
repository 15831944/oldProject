using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferBll;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Factory;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using CADColor = Autodesk.AutoCAD.Colors.Color;

namespace ElectronTransferView.FunctionManager
{
    public partial class MergerIncrementXml : Form
    {
        public MergerIncrementXml()
        {
            InitializeComponent();
        }

        private string xmlPath { set; get; }
        private XmlDBManager xmlDBManager;

        #region 选择增量文件
        private void Btn_Select_Click(object sender, EventArgs e)
        {
            var openFile = new OpenFileDialog {Filter = "xml文件|*.xml|" + SystemSetting.FileExtension};
            if(openFile.ShowDialog()==DialogResult.OK)
            {
                Txt_ZlXml.Text = openFile.FileName;
                xmlPath = openFile.FileName;
            }
        }
        #endregion

        #region 初始化增量文件
        /// <summary>
        /// 初始化增量数据
        /// </summary>
        private bool InitiationIncrementXml()
        {
            var result = false;
            try
            {
                if (File.Exists(xmlPath))
                {
                    xmlDBManager = new XmlDBManager {FileName = xmlPath};
#if EnableLock
                xmlDBManager.Password = XmlPassword.Instance.Password;
#endif
                    xmlDBManager.Initialize();
                    if (HasIncrement(xmlDBManager))
                    {
                        //合并增量数据源
                        (DBManager.Instance as XmlDBManager).MergeIncrement(xmlDBManager);
                        var duplicateRecord = (DBManager.Instance as XmlDBManager).MergeIncrement(xmlDBManager);
                        Duplicate(xmlDBManager, duplicateRecord);
                        result = true;
                    }
                }
            }catch(Exception exception)
            {
                LogManager.Instance.Error(exception);
            }
            return result;
        }
        private static void Duplicate(XmlDBManager xmlDbManager, IEnumerable<DBEntity> duplicateRecord)
        {
            foreach (var dbEntity in duplicateRecord)
            {
                xmlDbManager.Delete(dbEntity);
            }
        }
        /// <summary>
        /// 判断是否是增量Xml文件
        /// </summary>
        /// <param name="xmlDBManager"></param>
        /// <returns></returns>
        private bool HasIncrement(XmlDBManager xmlDBManager)
        {
            try
            {
                var cadVersion = xmlDBManager.GetEntity<CadVersion>(o=>o.G3E_FID==1);
                if (cadVersion == null)
                    return false;
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region 合并增量
        private void BtnMerger_Click(object sender, EventArgs e)
        {
            try
            {
                var incrementFilePath = Txt_ZlXml.Text;
                if(string.IsNullOrEmpty(incrementFilePath))
                {
                    SetToolStatusTip("请选择要合并的增量文件！");
                    Txt_ZlXml.Focus();
                    return;
                }
                ToolTipText("正在努力为您加载…");
                var result=InitiationIncrementXml();
                if (result)
                {
                    ErgodicIncrementXml(xmlDBManager);
                    MergerElseStateSymbol(xmlDBManager);
                    Close();
                }
                else
                {
                    SetToolStatusTip("合并失败!该文件格式不正确！");
                }

            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        private void SetToolStatusTip(string message)
        {
            ToolStatusTip.Text =message;
            ToolStatusTip.ForeColor = Color.Red;
        }

        /// <summary>
        ///  加载增量数据
        /// </summary>
        /// <param name="xmlDBManager"></param>
        private void ErgodicIncrementXml(XmlDBManager xmlDBManager)
        {
            var index = 0.0;
            //过滤无符号数据
            var filter = new[] { 198, 199, 250 };
            var sum = SimpleDBSymbolsConfig.Instance.DBSymbol.Count;
            foreach (var symbol in SimpleDBSymbolsConfig.Instance.DBSymbol)
            {
                //计算百分比
                ToolTipText(index,sum);
                if (filter.Contains(symbol.G3E_FNO))
                    continue;
                //加载增量设备
                LoadIncrementSymbol(symbol, xmlDBManager);
                //加载增量设备标注
                LoadIncrementLalbel(symbol, xmlDBManager);
                index++;
            }
            //加载增量设备杂项标注
            //CABLEManager.LoadZxbzSymbol(xmlDBManager);
        }
        #endregion

        #region 进度提示
        /// <summary>
        /// 加载进度提示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="sum"></param>
        private void ToolTipText(double index,int sum)
        {
            var message = string.Format("正在努力为您加载…{0}%", Math.Round(index / sum * 100, 0));
            ToolTipText(message);
        }
        private void ToolTipText(string message)
        {
            ToolStatusTip.Text = message;
            ToolStatusTip.ForeColor = Color.Black;
            Refresh();
        }
        #endregion

        #region 加载新增的点线面设备
        /// <summary>
        /// 加载点线面设备
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="xmlDBManager"> </param>
        private void LoadIncrementSymbol(DBSymbolEntry symbol, XmlDBManager xmlDBManager)
        {
            try
            {
                if (!string.IsNullOrEmpty(symbol.SymbolPtTable))
                {
                    var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), symbol.SymbolPtTable.Trim());
                    if (xmlDBManager.Has(type))
                    {
                        //获取坐标数据
                        var pts = xmlDBManager.GetEntities(type).Cast<ElectronSymbol>();
                        //颜色转换
                        var color = CADColor.FromColor(Color.FromArgb(int.Parse(symbol.OtherProperty.SymbolColor)));

                        //加载图形
                        AddElectronSymbol(symbol.OtherProperty.LayerName.Trim(), pts, color, symbol.OtherProperty.LineType, symbol.OtherProperty.LineWidth);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("给定关键字不在字典中"))
                    LogManager.Instance.Error("加载设备错误！" + ex);
            }
        }
        /// <summary>
        /// 加载图形
        /// </summary>
        /// <param name="_layerName"></param>
        /// <param name="esymbs"></param>
        /// <param name="color"></param>
        /// <param name="strLineType"></param>
        /// <param name="lineWidth"></param>
        private void AddElectronSymbol(string _layerName, IEnumerable<ElectronSymbol> esymbs, CADColor color, string strLineType, double lineWidth)
        {
            if (esymbs.Any())
            {
                //添加图层
                var layerId = DCadApi.addLayer(_layerName, CADColor.FromRgb(255, 255, 255));
                if (layerId != ObjectId.Null)
                {
                    foreach (var _ee in esymbs)
                    {
                        //加载符号
                        AddElectronSymbol(layerId, _ee, color, strLineType, lineWidth);
                    }
                }
            }
        }
        /// <summary>
        /// 加载图形
        /// </summary>
        /// <param name="layerId"></param>
        /// <param name="esymb"></param>
        /// <param name="color"></param>
        /// <param name="strLineType"></param>
        /// <param name="lineWidth"></param>
        private void AddElectronSymbol(ObjectId layerId, ElectronSymbol esymb, CADColor color, string strLineType, double lineWidth)
        {
            //处理新增数据
            if(esymb.EntityState==EntityState.Insert)
            {
                LoadIncrementSymbol(layerId, esymb, color, strLineType, lineWidth);

            }
        }
        /// <summary>
        /// 展示新增状态数据
        /// </summary>
        /// <param name="layerId"></param>
        /// <param name="esymb"></param>
        /// <param name="color"></param>
        /// <param name="strLineType"></param>
        /// <param name="lineWidth"></param>
        private void LoadIncrementSymbol(ObjectId layerId, ElectronSymbol esymb, CADColor color, string strLineType, double lineWidth)
        {
            if (esymb.G3E_GEOMETRY is Multipoint)
            {
                //加载点设备
                CABLEManager.AddPointSymbol(layerId, esymb, color,xmlDBManager);
            }
            else if (esymb.G3E_GEOMETRY is LineString)
            {
                //加载线设备
                CABLEManager.AddLineSymbol(layerId, esymb, color, strLineType, lineWidth, xmlDBManager);
            }
            else if (esymb.G3E_GEOMETRY is Polygon)
            {
                //加载面设备
                CABLEManager.AddPolygonSymbol(layerId, esymb, color, strLineType, lineWidth, xmlDBManager);
            }
        }

        #endregion

        #region 合并更新过的数据
        /// <summary>
        /// 存储更新过的设备
        /// </summary>
        private Dictionary<ObjectId,ElectronSymbol> updateStateGEOMETRY { set; get; }

        private List<long> notPointFidList { set; get; }
        private List<long> pointFidList { set; get; }
        /// <summary>
        /// 合并其他状态数据
        /// </summary>
        /// <param name="xmlDBManager"></param>
        private void MergerElseStateSymbol(XmlDBManager xmlDBManager)
        {
            updateStateGEOMETRY = new Dictionary<ObjectId, ElectronSymbol>();
            notPointFidList = new List<long>();
            pointFidList = new List<long>();
            var tables=xmlDBManager.GetInerementTables();
            foreach (var table in tables)
            {
                //查询修改过的数据
                var entities = table.Entities.Where(o => o.EntityState != EntityState.Insert && o.EntityState != EntityState.None);
                foreach (var entity in entities)
                {
                    //过滤新增状态的连接关系（Add_Add_Add)
                    var entityState = entity.EntityState.ToString().Split('_');
                    if (entityState[0] == "Add")
                    {
                        continue;
                    }
                    //DBManager.Instance.Update(entity);
                    UpdateGraph(entity);
                }
            }
            //更新点设备图形
            UpdatePointGraph();
        }
        private void UpdateGraph(DBEntity dbEntity)
        {
            //存储坐标数据
            if (dbEntity.IsCoordinate)
            {
                var objectId = ObjectId.Null;
                var pt = dbEntity as ElectronSymbol;
                switch (pt.EntityType)
                {
                    case EntityType.None:
                        objectId = DBEntityFinder.Instance.GetObjectIdByFid(pt.G3E_FID, EntityType.None);
                        if (!updateStateGEOMETRY.ContainsKey(objectId))
                            updateStateGEOMETRY.Add(objectId, pt);

                        GetGraphG3eFID(dbEntity, pointFidList);
                        break;
                    case EntityType.Label:
                        objectId = DBEntityFinder.Instance.GetObjectIdByFid(pt.G3E_FID, EntityType.Label);
                        SymbolLabel.UpdateDBText(pt,objectId);
                        break;
                    case EntityType.ZxLabel:
                        objectId = DBEntityFinder.Instance.GetObjectIdByFid(pt.G3E_FID, EntityType.ZxLabel);
                        break;
                }
                //更新缓存
                PublicMethod.Instance.UpdateDBSymbolFinder(objectId, pt);
            }else
            {
                GetNotGraphG3eFID(dbEntity, notPointFidList);
            }
        }
        /// <summary>
        /// 获取只更改属性没有更改坐标的设备
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="notPointFidList"></param>
        private void GetNotGraphG3eFID(DBEntity dbEntity, ICollection<long> notPointFidList)
        {
            var fid = dbEntity.GetValue<long>("G3E_FID");
            if (!notPointFidList.Contains(fid))
                notPointFidList.Add(fid);
        }
        /// <summary>
        /// 获取只更更改坐标的设备
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="pointFidList"></param>
        private void GetGraphG3eFID(DBEntity dbEntity, ICollection<long> pointFidList)
        {
            var fid = dbEntity.GetValue<long>("G3E_FID");
            if (!pointFidList.Contains(fid))
                pointFidList.Add(fid);
        }

        /// <summary>
        /// 更新点设备图形
        /// </summary>
        private void UpdatePointGraph()
        {
            try
            {
                if (updateStateGEOMETRY.Any())
                {
                    foreach (var item in updateStateGEOMETRY)
                    {
                        //获取图形ID
                        if (item.Value == null) continue;
                        if (item.Key.IsNull)
                            continue;
                        if (item.Value.G3E_GEOMETRY is Multipoint)
                        {
                            UpdatePointSymbol(item.Key, item.Value);
                        }
                        else if (item.Value.G3E_GEOMETRY is Polygon)
                        {
                            UpdatePolygon(item.Key, item.Value);
                        }
                        else
                        {
                            UpdatePolyline(item.Key, item.Value);
                        }
                    }
                }
                UpdateNotPointGraph();
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(string.Format("更新图形错误！{0}", ex));
            }
        }
        /// <summary>
        /// 更新只更改属性没有更改坐标的设备
        /// </summary>
        private void UpdateNotPointGraph()
        {
            if (!notPointFidList.Any()) return;
            //差集
            var exceptFid=notPointFidList.Except(pointFidList);
            foreach (var fid in exceptFid)
            {
                var keyValuePair = DBEntityFinder.Instance.GetObjectIDByG3eFid(fid);
                if (!keyValuePair.Key.IsNull)
                    UpdatePointGraph(keyValuePair.Key, keyValuePair.Value);
            }
        }

        #region UpdatePolygon

        /// <summary>
        /// 更新面设备
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="pt"></param>
        private void UpdatePolygon(ObjectId objectId, ElectronSymbol pt)
        {
            try
            {
                DCadApi.isModifySymbol = true;
                using (var tran = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        var entity = PublicMethod.Instance.GetObject(objectId, OpenMode.ForWrite) as Entity;
                        if (entity is Polyline)
                        {
                            var polyline = entity as Polyline;
                            var graphPointCount = polyline.NumberOfVertices;
                            var pointCount = (pt.G3E_GEOMETRY as Polygon).UniqueLineString.Points.Count;

                            if (pointCount >= graphPointCount)
                            {
                                for (var i = 0; i < pointCount - graphPointCount; i++)
                                {
                                    polyline.AddVertexAt(0, Point2d.Origin, 0, 1, 1);
                                }
                            }
                            else
                            {
                                for (var i = 0; i < graphPointCount - pointCount; i++)
                                {
                                    polyline.RemoveVertexAt(0);
                                }
                            }
                            //更新坐标点
                            for (var i = 0; i < polyline.NumberOfVertices; i++)
                            {
                                var tpt = (pt.G3E_GEOMETRY as Polygon).UniqueLineString.Points[i];
                                polyline.SetPointAt(i, new Point2d(tpt.X, tpt.Y));
                            }
                            tran.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(string.Format("更新面设备错误！G3E_FID：{0}{1}",pt.G3E_FID,ex));
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }

        #endregion 

        #region UpdatePolyline
        /// <summary>
        /// 更新线设备
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="pt"></param>
        private void UpdatePolyline(ObjectId objectId, ElectronSymbol pt)
        {
            try
            {
                DCadApi.isModifySymbol = true;
                using (var tran = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        var entity = PublicMethod.Instance.GetObject(objectId, OpenMode.ForWrite) as Entity;
                        if (entity is Polyline)
                        {
                            var polyline = entity as Polyline;
                            var graphPointCount = polyline.NumberOfVertices;
                            var pointCount = (pt.G3E_GEOMETRY as LineString).Points.Count;

                            if (pointCount >= graphPointCount)
                            {
                                for (var i = 0; i < pointCount - graphPointCount; i++)
                                {
                                    polyline.AddVertexAt(0, Point2d.Origin, 0, 1, 1);
                                }
                            }
                            else
                            {
                                for (var i = 0; i < graphPointCount - pointCount; i++)
                                {
                                    polyline.RemoveVertexAt(0);
                                }
                            }
                            //更新坐标点
                            for (var i = 0; i < polyline.NumberOfVertices; i++)
                            {
                                var tpt = (pt.G3E_GEOMETRY as LineString).Points[i];
                                polyline.SetPointAt(i, new Point2d(tpt.X, tpt.Y));
                            }
                            tran.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(string.Format("更新线设备错误！G3E_FID：{0}{1}", pt.G3E_FID, ex));
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }
        #endregion

        #region UpdatePoint
        /// <summary>
        /// 更新点设备
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="pt"></param>
        private void UpdatePointSymbol(ObjectId objectId,ElectronSymbol pt)
        {
            try
            {
                DCadApi.isModifySymbol = true;
                using (var tran = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        var entity = PublicMethod.Instance.GetObject(objectId, OpenMode.ForWrite);
                        if (entity is BlockReference)
                        {
                            var block = entity as BlockReference;
                            //获取坐标
                            var point = new Point3d((pt.G3E_GEOMETRY as Multipoint).Points[0].X,
                                (pt.G3E_GEOMETRY as Multipoint).Points[0].Y, 0);
                            //获取角度
                            var rotateAngle = Math.Atan2((pt.G3E_GEOMETRY as Multipoint).Points[1].Y,
                                (pt.G3E_GEOMETRY as Multipoint).Points[1].X);

                            block.Position = point;
                            block.Rotation = rotateAngle;
                            tran.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(string.Format("更新点设备错误！G3E_FID：{0}{1}", pt.G3E_FID, ex));
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
            //更新图形
            UpdatePointGraph(objectId, pt);
        }
        /// <summary>
        /// 更新点设备图形
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="pt"></param>
        private void UpdatePointGraph(ObjectId objectId,ElectronSymbol pt)
        {
            try
            {
                var newObjectId = ObjectId.Null;
                var psd = DCadApi.GetSymbolDataByObjId(objectId);
                //获取点设备样式
                var value=GetPointStyle(psd.color, pt);

                //根据符号属性获取块定义名称
                var blockName = DCadApi.GetBlockDefinitionName(value, pt.G3E_FNO.ToString());

                //点符号（点符号是以CAD图块的形式存在的）
                if (!string.IsNullOrEmpty(blockName) && !string.IsNullOrEmpty(psd.blockName))
                {
                    if (blockName != psd.blockName)
                    {
                        psd.blockName = blockName;
                        psd.color = value.color;
                        DCadApi.ReplacePointSymbol(psd, ref newObjectId);
                    }
                    else
                    {
                        //下面是针对点符号，只改变颜色的
                        if (psd.color != value.color)
                        {
                            DCadApi.EditorPointSymbol(psd.objectId, value.color);
                            psd.color = value.color;
                        }
                    }
                } //线面符号
                else
                {
                    //这里是针对线改变颜色
                    if (psd.color != value.color)
                    {
                        DCadApi.EditorPointSymbol(psd.objectId, value.color);
                        psd.color = value.color;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(string.Format("更新符号失败！{0}{1}",pt.G3E_FID, ex));
            }
        }
        /// <summary>
        /// 获取点设备样式
        /// </summary>
        /// <param name="color"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        private SymbolEventArgs GetPointStyle(CADColor color,ElectronSymbol pt)
        {
            var value = new SymbolEventArgs
            {
                color = color,
                electronSymbol = pt,
                DBManager = DBManager.Instance as XmlDBManager
            };
            //匹配符号
            ExecutionManager.Instance.GetFactory(typeof(SymbolExecutionFactory))
                .GetExecution(pt.G3E_FNO)
                .Execute(pt, value);
            return value;
        } 
        #endregion
        #endregion

        #region 加载增量标注
        /// <summary>
        /// 加载点线面设备标注
        /// </summary>
        /// <param name="dbSymbolEntry"></param>
        /// <param name="xmlDBManager"> </param>
        public void LoadIncrementLalbel(DBSymbolEntry dbSymbolEntry, XmlDBManager xmlDBManager)
        {
            try
            {
                for (var i = 0; i < dbSymbolEntry.Label.Count; i++)
                {
                    LoadSymbolLabel(xmlDBManager, dbSymbolEntry.Label[i].Name, dbSymbolEntry.Label[i].LabelPtTable, i);
                }
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(string.Format("加载增量标注错误！{0}", exception));
            }
        }

        /// <summary>
        /// 加载标注
        /// </summary>
        /// <param name="xmlDBManager"> </param>
        /// <param name="layerLableName">层名称</param>
        /// <param name="className">类型名称</param>
        /// <param name="index"></param>
        private void LoadSymbolLabel(XmlDBManager xmlDBManager, string layerLableName, string className, int index)
        {
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className);
                if (xmlDBManager.Has(type))
                {
                    var lb = xmlDBManager.GetEntities(type).Cast<ElectronSymbol>().ToList();
                    //大于0，则有多个标注
                    if (index > 0)
                    {
                        lb.ForEach(o => o.FinderFuncNumber = o.G3E_FNO * 10 + index);
                    }
                    else
                    {
                        lb.ForEach(o => o.FinderFuncNumber = o.G3E_FNO);
                    }
                    var lbs = GetLabel_lb_sdogeom(xmlDBManager, className);
                    DCadApi.addLabelLayer(layerLableName, lb, lbs, CADColor.FromRgb(255, 255, 255),xmlDBManager);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        private IEnumerable<ElectronBase> GetLabel_lb_sdogeom(XmlDBManager xmlDBManager, string className)
        {
            try
            {
                var lbClassName = className.Replace("_sdogeom", "");
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), lbClassName);
                if (xmlDBManager.Has(type))
                    return xmlDBManager.GetEntities(type).Cast<ElectronBase>();
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return new List<ElectronBase>();
        }

        /// <summary>
        /// 在cad指定图层上添加多个标注
        /// </summary>
        /// <param name="_layerName">图层名称</param>
        /// <param name="_esymb">多个标注对象</param>
        /// <param name="lbs"> </param>
        /// <param name="color"> </param>
        /// <param name="xmlDbManager"> </param>
        /// <returns></returns>
        public static void addLabelLayer(string _layerName, IEnumerable<ElectronSymbol> _esymb, IEnumerable<ElectronBase> lbs, CADColor color,XmlDBManager xmlDbManager)
        {
            try
            {
                var objId =DCadApi.addLayer(_layerName, CADColor.FromRgb(255, 255, 255));
                if (objId != ObjectId.Null)
                {
                    foreach (var _ee in _esymb)
                    {
                        if (_ee.EntityState == EntityState.Insert)
                        {
                            //var g3e_cno = _ee.GetValue<int>("G3E_CNO");
                            var lb = DCadApi.GetLabel_LB(lbs, _ee.G3E_FID);
                            DCadApi.addLabelLayer(objId, _ee, lb, color, xmlDbManager);
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                LogManager.Instance.Error(exx);
            }
        }
        #endregion
    }
}
