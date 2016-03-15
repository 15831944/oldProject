using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Factory;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferDal.XmlDal;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using CADColor = Autodesk.AutoCAD.Colors.Color;

namespace ElectronTransferBll
{
    /// <summary>
    /// 中压电缆业务类
    /// 用于处理中压电缆的业务逻辑
    /// </summary>
    public class CABLEManager
    {  
        public static List<Point3d> basePoint;
        #region 成员变量
        public static double _pointScale = 1.0/MapConfig.Instance.earthscale;
        public static event EventHandler ParcentHandler;
        ///// <summary>
        ///// 数据源
        ///// </summary>
        //private static XmlDBManager xmlDBManager { set; get; }
        /// <summary>
        /// 线宽0.01
        /// </summary>
        public static double _lineWidth000 = 0.003;
        /// <summary>
        /// 线宽0.02
        /// </summary>
        public static double _lineWidth001 = 0.02;
        /// <summary>
        /// 线宽0.033
        /// </summary>
        public static double _lineWidth002 = 0.033;
        /// <summary>
        /// 线宽0.08
        /// </summary>
        public static double _lineWidth004 = 0.08;
        /// <summary>
        /// 电缆线
        /// </summary>
        public static string _strLineTypeDashe = "DashLines";
        /// <summary>
        /// 导线
        /// </summary>
        public static string _strLineTypeContinuous = "Continuous";
        public static int _color0 = 8224125;
        public static int _color1;
        public static int _color2 = 255;
        public static int _color3 = 16711680;
        #endregion 

        #region 计算百分比
        /// <summary>
        /// 计算百分比
        /// </summary>
        /// <param name="index"></param>
        /// <param name="sum"></param>
        private static void GetParcentHandler(int index, int sum)
        {
            var parcent = GetPercent(index, sum);
            if (ParcentHandler != null)
            {
                ParcentHandler(new object(), new ParcentArgs { Parcent = parcent });
            }
        }
        private static double GetPercent(double index, int sum)
        {
            return Math.Round(index / sum * 100, 0);
        }
        #endregion

        #region 加载点线面设备
        /// <summary>
        /// 加载所有设备
        /// </summary>
        public static void LoadLayout(bool flag)
        {
            var xmlDBManager = DBManager.Instance as XmlDBManager;
            LoadAllSymbol(xmlDBManager, flag);
            //锁定lock图层
            PublicMethod.Instance.LockLayer("lock", true);
        }

        /// <summary>
        /// 加载设备
        /// </summary>
        /// <param name="xmlDBManager">数据源</param>
        /// <param name="isParcent">是否计算百分比 </param>
        public static void LoadAllSymbol(XmlDBManager xmlDBManager, bool isParcent)
        {
            var index = 0;
            var sum = SimpleDBSymbolsConfig.Instance.DBSymbol.Count;
            //台架和开关柜无符号
            var specialSymbol = new[] { 198, 199 };
            basePoint = new List<Point3d>();
            PublicMethod.Instance.dicObjIds = new Dictionary<ObjectId, DBEntity>();
            DBEntityCopy.Instance.dgList = new HashSet<long>();
            foreach (var symbol in SimpleDBSymbolsConfig.Instance.DBSymbol)
            {
                if (isParcent)
                {
                    //计算百分比
                    GetParcentHandler(index, sum);
                }
                if (symbol.G3E_FNO == 250)
                    continue;
                if (specialSymbol.Contains(symbol.G3E_FNO))
                {
                    DBEntityFinder.Instance.GetSpecialSymbolData(symbol.G3E_FNO);
                    continue;
                }
                //加载设备
                LoadSymbol(symbol,xmlDBManager);
                ////加载标注
                LoadSymbolLable(symbol, xmlDBManager);
                index++;
            }
            //加载杂项标注
            //LoadZxbzSymbol(xmlDBManager);
        }

        /// <summary>
        /// 加载点线面设备
        /// </summary>
        /// <param name="dbSymbolEntry"></param>
        /// <param name="xmlDbManager"> </param>
        private static void LoadSymbol(DBSymbolEntry dbSymbolEntry,XmlDBManager xmlDbManager)
        {
            try
            {
                if (!string.IsNullOrEmpty(dbSymbolEntry.SymbolPtTable))
                {
                    var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), dbSymbolEntry.SymbolPtTable.Trim());
                    if (xmlDbManager.Has(type))
                    {
                        //获取坐标数据
                        var pts = xmlDbManager.GetEntities(type).Cast<ElectronSymbol>();
                        //颜色转换
                        var color = Color.FromColor(System.Drawing.Color.FromArgb(int.Parse(dbSymbolEntry.OtherProperty.SymbolColor)));
                        //加载图形
                        AddElectronSymbol(dbSymbolEntry.OtherProperty.LayerName.Trim(), pts, color, dbSymbolEntry.OtherProperty.LineType, dbSymbolEntry.OtherProperty.LineWidth, xmlDbManager);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 在cad指定图层上添加多个电力符号
        /// </summary>
        /// <param name="_layerName">图层名称</param>
        /// <param name="_esymbs">多个电力符号对象</param>
        /// <param name="color"> </param>
        /// <param name="strLineType"> </param>
        /// <param name="lineWidth"> </param>
        /// <param name="xmlDbManager"> </param>
        /// <returns></returns>
        private static void AddElectronSymbol(string _layerName, IEnumerable<ElectronSymbol> _esymbs, CADColor color, string strLineType, double lineWidth, XmlDBManager xmlDbManager)
        {
            try
            {
                if (!_esymbs.Any()) return;
                //添加图层
                var LayerId = DCadApi.addLayer(_layerName, CADColor.FromRgb(255, 255, 255));
                if (LayerId != ObjectId.Null)
                {
                    foreach (var _ee in _esymbs)
                    {
                        //保存原始的FID
                        _ee.OriginalG3e_Fid = _ee.G3E_FID;
                        if (_ee.KxType)
                        {
                            //其他馈线统一在 lock 图层上
                            var lockcolor = CADColor.FromRgb(51, 51, 51);
                            var lockId = DCadApi.addLayer("lock", lockcolor);
                            AddElectronSymbol(lockId, _ee, lockcolor, strLineType, lineWidth,xmlDbManager);
                            continue;
                        }
                        //加载符号
                        AddElectronSymbol(LayerId, _ee, color, strLineType, lineWidth,xmlDbManager);
                    }
                }
            }
            catch (Exception exx)
            {
                LogManager.Instance.Error(exx);
            }
        }

        /// <summary>
        /// 在cad指定图层上添加电力符号
        /// </summary>
        /// <param name="objId">图层名称</param>
        /// <param name="_esymb">电力符号对象</param>
        /// <param name="color"> </param>
        /// <param name="strLineType"> </param>
        /// <param name="lineWidth"> </param>
        /// <returns></returns>
        private static void AddElectronSymbol(ObjectId objId, ElectronSymbol _esymb, CADColor color, string strLineType, double lineWidth,XmlDBManager xmlDbManager)
        {
            if (_esymb.EntityState != EntityState.Delete)
            {
                if (objId != ObjectId.Null)
                {
                    if (_esymb.G3E_GEOMETRY is Multipoint)
                    {
                        AddPointSymbol(objId, _esymb, color, xmlDbManager);
                    }
                    else if (_esymb.G3E_GEOMETRY is LineString)
                    {
                        AddLineSymbol(objId, _esymb, color, strLineType, lineWidth, xmlDbManager);
                    }
                    else if (_esymb.G3E_GEOMETRY is Polygon)
                    {
                        AddPolygonSymbol(objId, _esymb, color, strLineType, lineWidth, xmlDbManager);
                    }
                }
            }
        }

        #region 绘制点设备
        /// <summary>
        /// 绘制点设备
        /// </summary>
        /// <param name="layerId">图层id</param>
        /// <param name="_esymb">符号对象</param>
        /// <param name="color">符号颜色</param>
        public static void AddPointSymbol(ObjectId layerId, ElectronSymbol _esymb, CADColor color,XmlDBManager xmlDbManager)
        {
            try
            {
                var newBlockName = string.Empty;
                //获取gis图元样式
                var value = GetSymbolExecution(_esymb, color,xmlDbManager);

                //根据符号属性获取块定义名称
                var blockName = DCadApi.GetBlockDefinitionName(value, _esymb.G3E_FNO.ToString());
                //获取CAD块定义
                DCadApi.InsertBlock(blockName, ref newBlockName);

                var id = PublicMethod.Instance.GetBlockObjId(newBlockName);
                if (!id.IsNull)
                {
                    AddBasePoint(_esymb);
                    _esymb.BlockName = blockName;
                    var reference = GetBlockReference(id, layerId, value.color, _esymb);
                    //添加到模型数据库
                    var objectId = AddToModelSpace(reference);
                    PublicMethod.Instance.dicObjIds.Add(objectId, _esymb);

                    //保存电杆数据
                    AdddgList(_esymb);
                    if (!_esymb.KxType)
                    {
                        //记录添加符号信息
                        PublicMethod.Instance.AddDBSymbolFinder(objectId, _esymb);
                        //保存定位锁定符号信息数据
                        DCadApi.AddDBSymbolLTTIDFinder(_esymb, objectId, reference.Position, value.SBMC);
                    }
                }
                else
                    PublicMethod.Instance.ShowMessage(string.Format("{0}符号不存在！", _esymb.G3E_FNO));
            }
            catch (Exception exx)
            {
                LogManager.Instance.Error(string.Format("G3E_FID：{0}{1}",_esymb.G3E_FID,exx));
            }
        }
        /// <summary>
        /// 保存电杆数据
        /// </summary>
        /// <param name="electronSymbol"></param>
        private static void AdddgList(ElectronSymbol electronSymbol)
        {
            if (electronSymbol.G3E_FNO != 201) return;
            if (!DBEntityCopy.Instance.dgList.Contains(electronSymbol.G3E_FID))
                DBEntityCopy.Instance.dgList.Add(electronSymbol.G3E_FID);
        }
        private static BlockReference GetBlockReference(ObjectId objectId, ObjectId layerId, CADColor color, ElectronSymbol _esymb)
        {
            //符号比例
            var scale = new Scale3d(1.0 / MapConfig.Instance.earthscale);

            //获取坐标
            var point = new Point3d((_esymb.G3E_GEOMETRY as Multipoint).Points[0].X,
                (_esymb.G3E_GEOMETRY as Multipoint).Points[0].Y, 0);
            //获取角度
            var rotateAngle = Math.Atan2((_esymb.G3E_GEOMETRY as Multipoint).Points[1].Y,
                                     (_esymb.G3E_GEOMETRY as Multipoint).Points[1].X);
            var reference = new BlockReference(point, objectId)
            {
                ScaleFactors = scale,
                Rotation = rotateAngle,
                LayerId = layerId,
                Color = color
            };
            return reference;
        }
        private static ObjectId AddToModelSpace(BlockReference reference)
        {
            //添加夹点
            DCadApi.AddPinchPoint(reference);
            //添加符号-标注标记
            DCadApi.AddLabelPinchPoint(reference);
            //添加到模型空间
            return PublicMethod.Instance.ToModelSpace(reference);
        }
        #endregion

        #region 绘制面设备
        /// <summary>
        ///  绘制面设备
        /// </summary>
        /// <param name="layerId"></param>
        /// <param name="_esymb"></param>
        /// <param name="color"> </param>
        /// <param name="strLineType"></param>
        /// <param name="lineWidth"></param>
        public static void AddPolygonSymbol(ObjectId layerId, ElectronSymbol _esymb, Color color, string strLineType, double lineWidth,XmlDBManager xmlDbManager)
        {
            int index = 0;
            try
            {
                AddBasePoint(_esymb);
                var symbolEventArgs = GetSymbolExecution(_esymb, lineWidth,color,xmlDbManager);

                var line = DCadApi.SetPolyline(layerId, symbolEventArgs.color, strLineType);
                foreach (var point in
                        (_esymb.G3E_GEOMETRY as Polygon).UniqueLineString.Points)
                {
                    line.AddVertexAt(index, new Point2d(point.X, point.Y), 0.0,
                                     lineWidth / MapConfig.Instance.earthscale,
                                     lineWidth / MapConfig.Instance.earthscale);
                    index++;
                }
                //闭合
                line.Closed = true;
                DCadApi.AddLabelPinchPoint(line);
                var objectId = PublicMethod.Instance.ToModelSpace(line);
                PublicMethod.Instance.dicObjIds.Add(objectId, _esymb);
                //是否是其他馈线
                if (!_esymb.KxType)
                {
                    PublicMethod.Instance.AddDBSymbolFinder(objectId, _esymb);
                    var extents = line.GeometricExtents;
                    DCadApi.AddDBSymbolLTTIDFinder(_esymb, extents, symbolEventArgs.SBMC, objectId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        #endregion

        #region 绘制线设备

        /// <summary>
        /// 绘制线设备
        /// </summary>
        /// <param name="layerId"></param>
        /// <param name="_esymb"></param>
        /// <param name="color"> </param>
        /// <param name="strLineType"></param>
        /// <param name="lineWidth"></param>
        /// <param name="xmlDbManager"> </param>
        public static void AddLineSymbol(ObjectId layerId, ElectronSymbol _esymb, Color color, string strLineType, double lineWidth,XmlDBManager xmlDbManager)
        {
            var index = 0;
            try
            {
                AddBasePoint(_esymb);
                var symbolEventArgs = GetSymbolExecution(_esymb, lineWidth,color,xmlDbManager);
                var line = DCadApi.SetPolyline(layerId, symbolEventArgs.color, strLineType);
                foreach (var point in (_esymb.G3E_GEOMETRY as LineString).Points)
                {
                    line.AddVertexAt(index, new Point2d(point.X, point.Y), 0.0,
                                     symbolEventArgs.lineWidth / MapConfig.Instance.earthscale,
                                     symbolEventArgs.lineWidth / MapConfig.Instance.earthscale);
                    index++;
                }

                DCadApi.AddLabelPinchPoint(line);
                var objectId = PublicMethod.Instance.ToModelSpace(line);
                PublicMethod.Instance.dicObjIds.Add(objectId, _esymb);
                //是否是其他馈线
                if (!_esymb.KxType)
                {
                    PublicMethod.Instance.AddDBSymbolFinder(objectId, _esymb);
                    var extents = line.GeometricExtents;
                    DCadApi.AddDBSymbolLTTIDFinder(_esymb, extents, symbolEventArgs.SBMC, objectId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        #endregion
        private static void AddBasePoint(ElectronSymbol es)
        {
            if(es.BasePoint==null) return;
            if (!basePoint.Contains(GetG3E_GEOMETRYToPoint(es.BasePoint)))
            {
                basePoint.Add(GetG3E_GEOMETRYToPoint(es.BasePoint));
            }
        } 
        /// <summary>
        /// 转换坐标
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private static Point3d GetG3E_GEOMETRYToPoint(Geometry g)
        {
            return new Point3d((g as Multipoint).Points[0].X,
                                        (g as Multipoint).Points[0].Y, 0);
        }
        #endregion

        #region 获取设备样式

        /// <summary>
        /// 获取设备样式
        /// </summary>
        /// <param name="es"></param>
        /// <param name="lineWidth"></param>
        /// <param name="color"></param>
        /// <param name="xmlDbManager"> </param>
        /// <returns></returns>
        public static SymbolEventArgs GetSymbolExecution(ElectronSymbol es, double lineWidth,CADColor color,XmlDBManager xmlDbManager)
        {
            var symboleargs = new SymbolEventArgs
            {
                color=color,
                lineWidth = lineWidth,
                electronSymbol = es,
                DBManager = xmlDbManager
            };
            ExecutionManager.Instance.GetFactory(typeof(SymbolExecutionFactory)).GetExecution(es.G3E_FNO).Execute(es, symboleargs);
            return symboleargs;
        }

        /// <summary>
        /// 获取设备样式
        /// </summary>
        /// <param name="es"></param>
        /// <param name="color"></param>
        /// <param name="xmlDbManager"> </param>
        /// <returns></returns>
        public static SymbolEventArgs GetSymbolExecution(ElectronSymbol es, CADColor color, XmlDBManager xmlDbManager)
        {
            var value = new SymbolEventArgs
            {
                color = color,
                electronSymbol = es,
                DBManager = xmlDbManager
            };
            //匹配符号
            ExecutionManager.Instance.GetFactory(typeof(SymbolExecutionFactory)).GetExecution(es.G3E_FNO).Execute(es, value);
            return value;
        }

        /// <summary>
        /// 获取源符号的块定义名称
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="g3eObject"></param>
        /// <param name="xmlDbManager"> </param>
        /// <returns></returns>
        public static SymbolEventArgs GetSymbolExecution(ObjectId objectId, G3EObject g3eObject, XmlDBManager xmlDbManager)
        {
            SymbolEventArgs value = null;
            try
            {
                var pt = DBSymbolFinder.Instance[objectId];
                value = new SymbolEventArgs { DBManager = xmlDbManager, electronSymbol = pt };
                //匹配符号
                ExecutionManager.Instance.GetFactory(typeof(SymbolExecutionFactory)).GetExecution(g3eObject.G3E_FNO).
                    Execute(pt, value);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error("获取符号样式失败！\n" + ex);
            }
            //获取块定义名称
            return value;
        }
        #endregion 
        
        #region 加载设备标注
        /// <summary>
        /// 加载点线面设备标注
        /// </summary>
        /// <param name="dbSymbolEntry"></param>
        /// <param name="xmlDBManager"> </param>
        public static void LoadSymbolLable(DBSymbolEntry dbSymbolEntry, XmlDBManager xmlDBManager)
        {
            var i = 0;
            foreach (var label in dbSymbolEntry.Label)
            {
                LoadSymbolLabel(xmlDBManager, label.Name, label.LabelPtTable, i);
                i++;
            }
        }

        /// <summary>
        /// 加载标注
        /// </summary>
        /// <param name="xmlDBManager"> </param>
        /// <param name="layerLableName">层名称</param>
        /// <param name="className">类型名称</param>
        /// <param name="index"></param>
        private static void LoadSymbolLabel(XmlDBManager xmlDBManager, string layerLableName, string className, int index)
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
                    DCadApi.addLabelLayer(layerLableName, lb, lbs, Color.FromRgb(255, 255, 255),xmlDBManager);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        private static IEnumerable<ElectronBase> GetLabel_lb_sdogeom(XmlDBManager xmlDBManager, string className)
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
        #endregion

        #region 加载设备杂项标注
        /// <summary>
        /// 加载杂项标注
        /// </summary>
        public static void LoadZxbzSymbol(XmlDBManager xmlDBManager)
        {
            long g3e_fid = 0;
            try
            {
                var notRelationZxbz = DBEntityFinder.Instance.GetNotRelationZxbzN(xmlDBManager);
                foreach (var zxbzn in notRelationZxbz)
                {
                    if (zxbzn.EntityState != EntityState.Delete)
                    {
                        g3e_fid = zxbzn.G3E_FID;
                        AddZxbzSymbol(xmlDBManager, zxbzn);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("不存在"))
                    LogManager.Instance.Error("加载杂项标注错误！g3e_fid:" + g3e_fid + ex);
            }
        }
        private static void AddZxbzSymbol(XmlDBManager xmlDBManager, ElectronTransferModel.V9_4.Gg_gl_zxbz_n zxbzn)
        {
            var zxbzGeom = DBEntityFinder.Instance.GetZxbzGeomByG3e_fid(xmlDBManager,zxbzn.G3E_FID);
            if (zxbzGeom != null)
            {
                //获取坐标
                var point = new Point3d((zxbzGeom.G3E_GEOMETRY as Multipoint).Points[0].X,
                                        (zxbzGeom.G3E_GEOMETRY as Multipoint).Points[0].Y, 0);
                //获取角度
                var rotateAngle = Math.Atan2((zxbzGeom.G3E_GEOMETRY as Multipoint).Points[1].Y,
                                             (zxbzGeom.G3E_GEOMETRY as Multipoint).Points[1].X);

                //杂项标注图层
                var layerID = DCadApi.addLayer("杂项标注");
                //标注大小
                var lbHeight = 0.35 / MapConfig.Instance.earthscale;
                //标注颜色
                var color = CADColor.FromRgb(0, 0, 0);

                if (!string.IsNullOrEmpty(zxbzn.MIF_TEXT))
                {
                    Entity tEntity;
                    var lb = DBEntityFinder.Instance.GetLabel_LB(zxbzGeom);
                    var alignment = DBEntityFinder.Instance.GetG3eAlignment(lb);
                    if (zxbzn.MIF_TEXT.Contains("\n"))
                    {
                        tEntity = SymbolLabel.Mtext(zxbzn.MIF_TEXT, point, rotateAngle, lbHeight, layerID);
                    }
                    else
                    {
                        tEntity = SymbolLabel.AddText(zxbzn.MIF_TEXT, point, rotateAngle, lbHeight, layerID, color,
                                                      alignment);
                    }
                    DCadApi.AddPinchPoint(tEntity);
                    DCadApi.AddLabelPinchPoint(tEntity);
                    //添加到当前模型中
                    var objectId = PublicMethod.Instance.ToModelSpace(tEntity);
                    zxbzGeom.IsErased = false;
                    zxbzGeom.EntityType = EntityType.ZxLabel;
                    PublicMethod.Instance.AddDBSymbolFinder(objectId, zxbzGeom);
                }
            }
        }
        #endregion
    }
    public class ParcentArgs:EventArgs
    {
        /// <summary>
        /// 百分比
        /// </summary>
        public double Parcent { set; get; }
    }
}
