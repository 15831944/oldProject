using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using ElectronTransferDal.Common;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;
using CADException = Autodesk.AutoCAD.Runtime.Exception;
using CADColor = Autodesk.AutoCAD.Colors.Color;

namespace ElectronTransferDal.Cad
{
    /// <summary>
    /// 标注类
    /// </summary>
    public class SymbolLabel
    {
        #region 自动成图时绘制标注
        /// <summary>
        /// 绘制标注
        /// </summary>
        /// <param name="layerId"></param>
        /// <param name="_esymb"></param>
        /// <param name="esymlb"> </param>
        /// <param name="xmlDbManager"> </param>
        public static void DrawLabelSymbol(ObjectId layerId, ElectronSymbol _esymb,DBEntity esymlb,XmlDBManager xmlDbManager)
        {
            try
            {
                var point = new Point3d((_esymb.G3E_GEOMETRY as Multipoint).Points[0].X, (_esymb.G3E_GEOMETRY as Multipoint).Points[0].Y, 0);
                var rotateAngle = Math.Atan2((_esymb.G3E_GEOMETRY as Multipoint).Points[1].Y,
                                                (_esymb.G3E_GEOMETRY as Multipoint).Points[1].X);
                var alignment = DBEntityFinder.Instance.GetG3eAlignment(esymlb);
                InsertLabel(_esymb, point, rotateAngle, layerId,alignment,xmlDbManager);
            }
            catch (Exception exx)
            {
                LogManager.Instance.Error(string.Format("添加标注错误：{0}\nG3E_FID：{1}",exx ,_esymb.G3E_FID));
            }
        }

      
        /// <summary>
        /// 插入标注
        /// </summary>
        /// <param name="_esymb"></param>
        /// <param name="position"></param>
        /// <param name="rotateAngle"></param>
        /// <param name="layerId"></param>
        /// <param name="alignment"></param>
        /// <param name="xmlDbManager"></param>
        public static void InsertLabel(ElectronSymbol _esymb, Point3d position, double rotateAngle, ObjectId layerId,int alignment,XmlDBManager xmlDbManager)
        {
            if (_esymb.EntityState != EntityState.Delete)
            {
                _esymb.EntityType = EntityType.Label;
                var value = new LabelEventArgs { xmlDB = xmlDbManager, color = CADColor.FromRgb(0, 0, 0) };
                value = GetLabelTextStyle(_esymb, value);
        
                if (string.IsNullOrEmpty(value.lbText))
                {
                    value.lbText = "";
                }
                //打开图层再添加标注
                PublicMethod.Instance.SetLayerDisplay(layerId, false);
                //标注高度
                var lbHeight = value.lbHeight/MapConfig.Instance.earthscale;

                if (value.lbText.Contains("\n"))
                {
                    var mText = Mtext(value.lbText, position, rotateAngle, lbHeight, layerId);
                    AddEntity(mText, _esymb);
                }
                else
                {
                    var mText = AddText(value.lbText, position, rotateAngle, lbHeight, layerId, value.color, alignment);
                    AddEntity(mText, _esymb);
                }
            }
        }

       
        private static ObjectId AddEntity(Entity entity, ElectronSymbol _esymb)
        {
            DCadApi.AddPinchPoint(entity);
            DCadApi.AddLabelPinchPoint(entity);
            var objectId = PublicMethod.Instance.ToModelSpace(entity);
            PublicMethod.Instance.dicObjIds.Add(objectId, _esymb);
            using (PublicMethod.Instance.acDoc.LockDocument())
            {
                PublicMethod.Instance.AddDBSymbolFinder(objectId, _esymb);
            }
            return objectId;
        }
        #endregion

        #region 标注对象

        /// <summary>
        /// 由插入点、文字内容、文字样式、文字高度、文字宽度创建单行文字 
        /// </summary>
        /// <param name="textString"></param>
        /// <param name="position"></param>
        /// <param name="rotateAngle"></param>
        /// <param name="lbHeight"></param>
        /// <param name="layerId"></param>
        /// <param name="color"> </param>
        /// <param name="alginment"> </param>
        /// <returns></returns>
        public static DBText AddText(string textString, Point3d position, double rotateAngle, double lbHeight, ObjectId layerId,CADColor color,int alginment)
        {
            var h = TextHorizontalMode.TextLeft;
            var v = TextVerticalMode.TextBottom;
            GetLabelAlignment(alginment,ref h,ref v);
            var dbText = new DBText
            {
                Height = lbHeight,
                TextString = textString,
                HorizontalMode = h,
                VerticalMode = v,
                AlignmentPoint=position,
                Rotation = rotateAngle,
                LayerId = layerId,
                Color = color,
                Position = position
            };
            return dbText;
        }
        /// <summary>
        /// 由插入点、文字内容、文字样式、文字高度、文字宽度创建单行文字 
        /// </summary>
        /// <param name="textString"></param>
        /// <param name="position"></param>
        /// <param name="rotateAngle"></param>
        /// <param name="lbHeight"></param>
        /// <param name="layerId"></param>
        /// <param name="color"> </param>
        /// <returns></returns>
        public static DBText AddText(string textString, Point3d position, double rotateAngle, double lbHeight, ObjectId layerId, CADColor color)
        {
            var dbText = new DBText
            {
                Height = lbHeight,
                TextString = textString,
                HorizontalMode = TextHorizontalMode.TextLeft,
                VerticalMode = TextVerticalMode.TextBottom,
                AlignmentPoint=position,
                Rotation = rotateAngle,
                LayerId = layerId,
                Color = color,
                Position=position
            };
            return dbText;
        }

        ///  <summary> 
        ///  由插入点、文字内容、文字样式、文字高度、文字宽度创建多行文字 
        ///  </summary>  
        ///  <param name="textString">文字内容</param> 
        ///  <param name="location"> 基点</param> 
        ///  <param name="lbHeight"> 文字高度</param> 
        ///  <param name="rot">文字转角</param>
        /// <param name="layerId">层ID</param>
        /// <returns></returns> 
        public static MText Mtext(string textString, Point3d location, double rot, double lbHeight, ObjectId layerId)
        {
            var txt = new MText
            {
                Location = location,
                Rotation = rot,
                Contents = textString,
                LayerId = layerId,
                TextHeight = lbHeight,
                Color = CADColor.FromRgb(0, 0, 0),
                TextStyleId = PublicMethod.Instance.MultTextStyleId
            };
            return txt;
        }

        ///  <summary> 
        ///  由插入点、文字内容、文字样式、文字高度、文字宽度创建多行文字 
        ///  </summary>  
        ///  <param name="textString">文字内容</param> 
        ///  <param name="location"> 基点</param> 
        ///  <param name="lbHeight"> 文字高度</param> 
        ///  <param name="rot">文字转角</param>
        /// <param name="layerId">层ID</param>
        /// <param name="alginment"> </param>
        /// <returns></returns> 
        public static MText Mtext(string textString, Point3d location, double lbHeight, double rot, ObjectId layerId, int alginment)
        {
            var txt = new MText
            {
                Location = location,
                Rotation = rot,
                Contents = textString,
                LayerId = layerId,
                TextHeight = lbHeight,
                Color = CADColor.FromRgb(0, 0, 0),
                TextStyleId = PublicMethod.Instance.MultTextStyleId
            };
            return txt;
        }
        #endregion

        #region 手动添加标注
        /// <summary>
        /// 添加标注
        /// </summary>
        /// <param name="ise"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="rotation"></param>
        public static void AddSymbolLabel(InsertSymbolEventArgs ise, double positionX, double positionY, double rotation)
        {
            var entry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == ise.g3e_fno);
            if (entry == null) PublicMethod.Instance.ConfigError(ise.g3e_fno);
         
            //控制标注显示
            if (!MapConfig.Instance.labels.Contains(ise.g3e_fno.ToString())) return;
            var increment = 0.0000020;
            for (var i = 0; i < entry.Label.Count; i++)
            {
                var g3e_fno = i == 0 ? ise.g3e_fno : ise.g3e_fno*10 + i;
                var g3e_cno = entry.Label[i].CNO;
                var value = GetLabelText(g3e_cno,ise.g3e_fid,ise.g3e_fno);
                if (!string.IsNullOrEmpty(value.lbText))
                {
                    increment += 0.000006;
                    var position = new Point3d();
                    var mpt = GetLabelPoint(positionX, positionY, rotation, increment, ref position);

                    //创建标注
                    var pt = CreateLabel(ise, mpt, entry.Label[i].LabelPtTable, Convert.ToInt32(entry.Label[i].CNO), g3e_fno);

                    var layerName = string.Format("{0}标注", entry.OtherProperty.LayerName);
                    AddLabel(pt, layerName, position, rotation, value);
                }
            }
        }

        private static void AddLabel(ElectronSymbol pt,string layerName,Point3d position,double rotation,LabelEventArgs value)
        {
            var layerId = DCadApi.addLayer(layerName);
            //打开图层再添加标注
            PublicMethod.Instance.SetLayerDisplay(layerId, false);
            //标注高度
            var lbHeight = value.lbHeight / MapConfig.Instance.earthscale;
            var mText = AddText(value.lbText, position, rotation, lbHeight, layerId, value.color,5);
            var objectId = AddEntity(mText, pt);
            PublicMethod.Instance.AddDBSymbolFinder(objectId, pt);
        }

        private static Multipoint GetLabelPoint(double positionX, double positionY, double rotation,double y,ref Point3d point)
        {
            positionX += 0.00000388;
            positionY += 0.00000008;
            point = new Point3d(positionX, positionY + y, 0);
            return ConvertGeometry.Instance.GetMultipoint(positionX, positionY+y, rotation);
        }


        /// <summary>
        /// 添加标注
        /// </summary>
        /// <param name="g3eObject"></param>
        /// <param name="labelFlagFno"></param>
        /// <param name="labelClassName"></param>
        /// <param name="G3E_CNO"></param>
        /// <param name="lea"></param>
        /// <returns></returns>
        public static bool AddLabel(G3EObject g3eObject, int labelFlagFno, string labelClassName, int G3E_CNO, LabelEventArgs lea)
        {
            var result = false;
            try
            {
                var values = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == g3eObject.G3E_FNO);
                var layerID = DCadApi.addLayer(string.Format("{0}标注", values.OtherProperty.LayerName));

                var lbHeight = lea.lbHeight / MapConfig.Instance.earthscale;

                var mText = AddText(lea.lbText, Point3d.Origin, 0, lbHeight, layerID, lea.color);
                if (BlockJig.Jig(mText, true))
                {
                    DCadApi.AddPinchPoint(mText);
                    DCadApi.AddLabelPinchPoint(mText);
                    var objId = PublicMethod.Instance.ToModelSpace(mText);
                    var mpt = ConvertGeometry.Instance.GetMultipoint(objId);
                    //查询原设备删除的标注
                    var labelPt = DBEntityFinder.Instance.GetDBEntityByG3efidOrClassName(g3eObject.G3E_FID, labelClassName,EntityState.Delete);
                    //原设备删除
                    if (labelPt!=null)
                    {
                        var labelObject = DBEntityFinder.Instance.GetEraseStateLabel(g3eObject.G3E_FID, labelFlagFno);
                        if (!labelObject.Key.IsNull)
                        {
                            //删除缓存
                            DBSymbolFinder.Instance.Remove(labelObject.Key);
                        }
                        var pt = labelPt.Clone() as ElectronSymbol;
                        pt.G3E_GEOMETRY = mpt;
                        pt.IsErased = false;
                        pt.EntityState = EntityState.Update;
                        DBManager.Instance.Update(pt);
                        UpdateLabelLB(pt);

                        //新增
                        PublicMethod.Instance.AddDBSymbolFinder(objId, pt);
                    }
                    else
                    {
                        ErasedCacheLabel(labelClassName, g3eObject.G3E_FID);
                        var pt = CreateLabel(g3eObject, mpt, labelClassName, G3E_CNO, labelFlagFno);
                        PublicMethod.Instance.AddDBSymbolFinder(objId, pt);
                    }
                    result = true;
                }

            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return result;
        }

        #endregion

        #region 创建标注对象
        /// <summary>
        /// 创建标注对象
        /// </summary>
        /// <param name="g3eObject"></param>
        /// <param name="multipoint"></param>
        /// <param name="className"></param>
        /// <param name="G3E_CNO"></param>
        /// <param name="labelFlagFno"></param>
        /// <returns></returns>
        private static ElectronSymbol CreateLabel(G3EObject g3eObject, Multipoint multipoint, string className, int G3E_CNO, int labelFlagFno)
        {
            long? g3e_cid = 1;
            ElectronSymbol pt = null;
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className);
                var instance = ReflectionUtils.CreateObject(
                    new
                    {
                        G3E_CID = g3e_cid,
                        G3E_ID = g3eObject.G3E_ID,
                        G3E_FID = g3eObject.G3E_FID,
                        G3E_FNO = g3eObject.G3E_FNO,
                        G3E_CNO = G3E_CNO,
                        G3E_GEOMETRY = multipoint,
                        LTT_ID = Convert.ToDecimal(MapConfig.Instance.LTTID),
                        EntityState = EntityState.Insert
                    }, type) as ElectronSymbol;

                DBManager.Instance.Insert(instance);

                pt = instance;
                pt.EntityType = EntityType.Label;
                pt.FinderFuncNumber = labelFlagFno;
                //新增标注LB表数据
                AddBortherDBEntity(className, pt,G3E_CNO);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return pt;
        }

        /// <summary>
        /// 创建标注
        /// </summary>
        /// <param name="ise"></param>
        /// <param name="multipoint"></param>
        /// <param name="className"></param>
        /// <param name="G3E_CNO"></param>
        /// <param name="G3E_FNO"></param>
        private static ElectronSymbol CreateLabel(InsertSymbolEventArgs ise, Multipoint multipoint, string className, int G3E_CNO, int G3E_FNO)
        {
            long? g3e_cid = 1;
            ElectronSymbol pt = null;
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className);
                pt = ReflectionUtils.CreateObject(
                    new
                    {
                        G3E_CID = g3e_cid,
                        G3E_ID = ise.g3e_id,
                        G3E_FID = ise.g3e_fid,
                        G3E_FNO = ise.g3e_fno,
                        G3E_CNO = G3E_CNO,
                        G3E_GEOMETRY = multipoint,
                        LTT_ID = Convert.ToDecimal(MapConfig.Instance.LTTID),
                        EntityState = EntityState.Insert
                    }, type) as ElectronSymbol;

                DBManager.Instance.Insert(pt);
                pt.EntityType = EntityType.Label;
                pt.FinderFuncNumber = G3E_FNO;
                //新增标注LB表数据
                AddBortherDBEntity(className, pt,G3E_CNO);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return pt;
        }
        /// <summary>
        /// 插入lb
        /// </summary>
        /// <param name="className"></param>
        /// <param name="pt"></param>
        /// <param name="G3E_CNO"></param>
        private static void AddBortherDBEntity(string className, ElectronSymbol pt,int G3E_CNO)
        {
            try
            {
                long? g3e_cid = 1;
                var lbClassName = className.Replace("_sdogeom", "");
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), lbClassName);
                var lb = ReflectionUtils.CreateObject(
                    new
                        {
                            G3E_CID = g3e_cid,
                            G3E_ID = pt.G3E_ID,
                            G3E_FID = pt.G3E_FID,
                            G3E_FNO = pt.G3E_FNO,
                            G3E_CNO = G3E_CNO,
                            G3E_ALIGNMENT = 9,
                            LTT_ID = Convert.ToDecimal(MapConfig.Instance.LTTID),
                            EntityState = EntityState.Insert
                        }, type) as ElectronBase;

                DBManager.Instance.Insert(lb);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        #endregion

        #region 删除标注
        /// <summary>
        /// 删除标注
        /// </summary>
        /// <param name="finderFuncNumber"></param>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        public static bool DeleteLabel(long finderFuncNumber, long g3e_fid)
        {
            try
            {
                //查找标注
                var finderValues = DBSymbolFinder.Instance.SingleOrDefault(o => o.Value.FinderFuncNumber == finderFuncNumber && o.Value.G3E_FID == g3e_fid && o.Value.EntityType==EntityType.Label);
                if (!finderValues.Key.IsNull)
                {
                    //打开图层
                    PublicMethod.Instance.SetLayerDisplay(false);
                    //删除实体对象
                    PublicMethod.Instance.EraseObject(finderValues.Key);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return false;
            }
        }
        #endregion

        #region 更新标注

        /// <summary>
        /// 更新已有的标注（修改属性时调用）
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <param name="g3e_fid"></param>
        public static void ShowSymbolLabel(int g3e_fno, long g3e_fid)
        {
            var objIds = new Dictionary<ObjectId, ElectronSymbol>();
            //根据FID查询标注
            var value = DBEntityFinder.Instance.GetDBSymbolFinderByFidOrEntityType(g3e_fid, EntityType.Label);
            if (value.Any())
            {
                foreach (var item in value)
                {
                    var entity = PublicMethod.Instance.GetObject(item.Key, OpenMode.ForRead) as Entity;
                    if (entity == null) continue;
                    //打开标注图层
                    PublicMethod.Instance.SetLayerDisplay(entity.LayerId, false);
                    if (entity is DBText || entity is MText)
                    {
                        objIds.Add(item.Key, item.Value);
                    }
                }
                foreach (var keyValuePair in objIds)
                {
                    UpdateDBText(keyValuePair.Value, keyValuePair.Key);
                }
            }
        }

        /// <summary>
        /// 更新标注
        /// </summary>
        /// <param name="objectId">对象ID</param>
        /// <param name="newTextString">新标注</param>
        /// <param name="oldTextString"> </param>
        public static void UpdateDBText(ObjectId objectId, string newTextString,string oldTextString)
        {
            using (var trans = PublicMethod.Instance.DB.TransactionManager.StartTransaction())
            {
                using (PublicMethod.Instance.acDoc.LockDocument())
                {
                    var entity = trans.GetObject(objectId, OpenMode.ForRead);
                    entity.UpgradeOpenAndRun();
                    if (entity is DBText)
                    {
                        var dbText = entity as DBText;
                        if (dbText.TextString.Equals(oldTextString))
                            dbText.TextString = newTextString;
                    }
                    else if (entity is MText)
                    {
                        var mText = entity as MText;
                        if (mText.Contents.Equals(oldTextString))
                            mText.Contents = newTextString;
                    }
                    trans.Commit();
                }
            }
        }

        /// <summary>
        /// 更新标注
        /// </summary>
        /// <param name="objectId">对象ID</param>
        /// <param name="newTextString">新标注</param>
        public static void UpdateDBText(ObjectId objectId, string newTextString)
        {
            using (var trans = PublicMethod.Instance.DB.TransactionManager.StartTransaction())
            {
                using (PublicMethod.Instance.acDoc.LockDocument())
                {
                    var entity = trans.GetObject(objectId, OpenMode.ForRead);
                    entity.UpgradeOpenAndRun();
                    if (entity is DBText)
                    {
                        var dbText = entity as DBText;
                        dbText.TextString = newTextString;
                    }else if(entity is MText)
                    {
                        var mText = entity as MText;
                        mText.Contents = newTextString;
                    }
                    trans.Commit();
                }
            }
        }

        /// <summary>
        /// 更新标注
        /// </summary>
        /// <param name="objectId">对象ID</param>
        /// <param name="newTextString">新标注</param>
        /// <param name="point3D"> </param>
        /// <param name="rotation"> </param>
        public static void UpdateDBText(ObjectId objectId, string newTextString,Point3d point3D,double rotation)
        {
            using (var trans = PublicMethod.Instance.DB.TransactionManager.StartTransaction())
            {
                using (PublicMethod.Instance.acDoc.LockDocument())
                {
                    var entity = trans.GetObject(objectId, OpenMode.ForRead);
                    entity.UpgradeOpenAndRun();
                    if(entity is DBText)
                    {
                        var dbText = entity as DBText;
                        dbText.TextString = newTextString;
                    }else if(entity is MText)
                    {
                        var mText = entity as MText;
                        mText.Contents = newTextString;
                    }
                    trans.Commit();
                }
            }
        }

        /// <summary>
        /// 更新标注
        /// </summary>
        /// <param name="electronSymbol">Fid</param>
        /// <param name="objectId">标注对象ID</param>
        public static void UpdateDBText1(ElectronSymbol electronSymbol,ObjectId objectId)
        {
            try
            {
                var value = new LabelEventArgs {xmlDB = DBManager.Instance as XmlDBManager};
                value = GetLabelTextStyle(electronSymbol, value);
                if (!string.IsNullOrEmpty((value.lbText)))
                {
                    UpdateDBText(objectId, value.lbText);
                }
            }catch(Exception exception)
            {
                LogManager.Instance.Error(string.Format("更新标注错误！G3E_FID：{0}\n错误信息：{1}",electronSymbol.G3E_FID,exception));
            }
        }
        /// <summary>
        /// 更新标注
        /// </summary>
        /// <param name="electronSymbol"></param>
        /// <param name="objectId">标注对象ID</param>
        public static void UpdateDBText(ElectronSymbol electronSymbol, ObjectId objectId)
        {
            try
            {
                var value = new LabelEventArgs { xmlDB = DBManager.Instance as XmlDBManager };
                value = GetLabelTextStyle(electronSymbol, value);
                if (!string.IsNullOrEmpty((value.lbText)))
                {
                    //获取坐标
                    var point = new Point3d((electronSymbol.G3E_GEOMETRY as Multipoint).Points[0].X,
                        (electronSymbol.G3E_GEOMETRY as Multipoint).Points[0].Y, 0);
                    //获取角度
                    var rotateAngle = Math.Atan2((electronSymbol.G3E_GEOMETRY as Multipoint).Points[1].Y,
                                             (electronSymbol.G3E_GEOMETRY as Multipoint).Points[1].X);
                    UpdateDBText(objectId, value.lbText,point,rotateAngle);
                }
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(string.Format("更新标注错误！G3E_FID：{0}\n错误信息：{1}", electronSymbol.G3E_FID, exception));
            }
        }
        #endregion

        #region 标注对齐方式(处理标注偏移)

        /// <summary>
        /// 标注对齐方式
        /// </summary>
        /// <param name="alignment"></param>
        /// <param name="textHorizontalMode"></param>
        /// <param name="textVerticalMode"></param>
        private static void GetLabelAlignment(int alignment,ref TextHorizontalMode textHorizontalMode,ref TextVerticalMode textVerticalMode)
        {
            if (alignment == 1|| alignment == 5 || alignment == 9)
            {
                textHorizontalMode = TextHorizontalMode.TextLeft;
            }
            else if (alignment == 0 || alignment == 4 || alignment == 8)
            {
                textHorizontalMode = TextHorizontalMode.TextMid;
            }
            else if (alignment == 2 || alignment == 6 || alignment == 10)
            {
                textHorizontalMode = TextHorizontalMode.TextRight;
            }

            if (alignment == 4 || alignment == 5 || alignment == 6)
            {
                textVerticalMode = TextVerticalMode.TextTop;
            }
            else if (alignment == 0 || alignment == 1 || alignment == 2)
            {
                textVerticalMode = TextVerticalMode.TextVerticalMid;
            }
            else if (alignment == 8 || alignment == 9 || alignment == 10)
            {
                textVerticalMode = TextVerticalMode.TextBottom;
            }
        }
        #endregion

        #region 获取标注样式

        /// <summary>
        /// 更新Lb表数据
        /// </summary>
        /// <param name="pt"></param>
        public static void UpdateLabelLB(ElectronSymbol pt)
        {
            var lbClassName = pt.GetType().Name.Replace("_sdogeom", "");
            var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), lbClassName);
            var labelLb = DBManager.Instance.GetEntity(type, pt.G3E_FID);
            if (labelLb != null)
            {
                var newLabelLb = labelLb as ElectronBase;
                if (newLabelLb.EntityState == EntityState.Delete)
                    newLabelLb.EntityState = EntityState.Update;
                newLabelLb.IsErased = false;
                DBManager.Instance.Update(newLabelLb);
            }
        }
        /// <summary>
        /// 删除缓存标注
        /// </summary>
        /// <param name="lableClassName"></param>
        /// <param name="g3e_fid"></param>
        private static void ErasedCacheLabel(string lableClassName, long g3e_fid)
        {
            var objectId = ObjectId.Null;
            var finderValue = DBSymbolFinder.Instance.Where(o => o.Value.G3E_FID == g3e_fid && o.Value.EntityType == EntityType.Label && o.Value.IsErased);
            foreach (var item in finderValue)
            {
                if (item.Value.GetType().Name == lableClassName)
                {
                    objectId = item.Key;
                }
            }
            //删除标注缓存
            if (!objectId.IsNull)
                DBSymbolFinder.Instance.Remove(objectId);
        }

        /// <summary>
        /// 获取标注值
        /// </summary>
        /// <param name="electronSymbol">标注的坐标表</param>
        /// <returns></returns>
        public static LabelEventArgs GetLabelText(ElectronSymbol electronSymbol)
        {
            var labelEventArgs = new LabelEventArgs { xmlDB = DBManager.Instance as XmlDBManager, color = CADColor.FromRgb(0, 0, 0) };
            labelEventArgs = GetLabelTextStyle(electronSymbol, labelEventArgs);
            return labelEventArgs;
        }

        /// <summary>
        /// 获取标注值
        /// </summary>
        /// <param name="g3e_cno"></param>
        /// <param name="g3eObject"> </param>
        /// <returns></returns>
        public static LabelEventArgs GetLabelText(string g3e_cno, G3EObject g3eObject)
        {
            var labelEventArgs = new LabelEventArgs { xmlDB = DBManager.Instance as XmlDBManager, color = CADColor.FromRgb(0, 0, 0) };
            labelEventArgs = GetLabelTextStyle(g3e_cno, g3eObject.G3E_FID, g3eObject.G3E_FNO, labelEventArgs);
            return labelEventArgs;
        }

        /// <summary>
        /// 获取标注值
        /// </summary>
        /// <param name="g3e_cno"></param>
        /// <param name="g3e_fno"> </param>
        /// <param name="g3e_fid"> </param>
        /// <returns></returns>
        public static LabelEventArgs GetLabelText(string g3e_cno, long g3e_fid, long g3e_fno)
        {
            var labelEventArgs = new LabelEventArgs { xmlDB = DBManager.Instance as XmlDBManager, color = CADColor.FromRgb(0, 0, 0) };
            labelEventArgs = GetLabelTextStyle(g3e_cno, g3e_fid, g3e_fno, labelEventArgs);
            return labelEventArgs;
        }


        /// <summary>
        /// 获取标注样式
        /// </summary>
        /// <param name="g3e_UserName"></param>
        /// <param name="e"></param>
        private static void GetG3e_textstyle(string g3e_UserName, LabelEventArgs e)
        {
            const double increment = 0.05;
            try
            {
                var textstyle = CDDBManager.Instance.GetEntity<G3e_textstyle>(o => o.G3E_USERNAME.ToUpper() == g3e_UserName.ToUpper());
                if (textstyle != null)
                {
                    if (textstyle.G3E_COLOR != null)
                        e.color = CADColor.FromColor(Color.FromArgb((int)textstyle.G3E_COLOR));
                    if (textstyle.G3E_SIZE != null)
                        e.lbHeight = (double)textstyle.G3E_SIZE + increment;
                }
                else
                {
                    e.lbHeight = 0.2 + increment;
                    e.color = CADColor.FromRgb(0, 0, 0);
                    LogManager.Instance.Error(string.Format("获取样式失败！{0}",g3e_UserName));
                }
            }
            catch
            {
                e.lbHeight = 0.2 + increment;
                e.color = CADColor.FromRgb(0, 0, 0);
                LogManager.Instance.Error(string.Format("获取样式失败！{0}", g3e_UserName));
            }
        }
        
        private static DBSymbolEntry GetDBSymbolEntry(long g3e_fno)
        {
            return SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == g3e_fno);
        }
        /// <summary>
        /// 获取标注样式
        /// </summary>
        /// <param name="electronSymbol"></param>
        /// <param name="labelEventArgs"></param>
        /// <returns></returns>
        private static LabelEventArgs GetLabelTextStyle(ElectronBase electronSymbol, LabelEventArgs labelEventArgs)
        {
            try
            {
                if (electronSymbol == null) return labelEventArgs;
                //获取标注配置文件
                var dbSymbolEntry = GetDBSymbolEntry(electronSymbol.G3E_FNO);
                if (dbSymbolEntry == null) return labelEventArgs;
                //获取标注
                var label = dbSymbolEntry.Label.SingleOrDefault(o => o.LabelPtTable == electronSymbol.GetType().Name);
                foreach (var LabelField in label.LabelCompose)
                {
                    //标注字段对应的表名
                    var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), LabelField.CorrespondTableName.Trim());
                    var dbEntity = labelEventArgs.xmlDB.GetEntity(type, electronSymbol.G3E_FID);
                    if (dbEntity == null) return labelEventArgs;
                    //判断标注字段是否存在
                    if (!dbEntity.HasAttribute(LabelField.Field.Trim())) continue;
                    //获取标注值
                    var labelText = dbEntity.GetValue(LabelField.Field.Trim());
                    //多条数据表示该标注又多个字段组合而成
                    if (label.LabelCompose.Count > 1)
                    {
                        //如果没有值则显示字段名称
                        if (labelText == null || ReferenceEquals(labelText, ""))
                            labelEventArgs.lbText += LabelField.Field.Trim() + LabelField.Unit.Trim();
                        else
                            labelEventArgs.lbText += labelText + LabelField.Unit.Trim();
                    }
                    else
                    {
                        //如果没有值则显示字段名称
                        if (labelText == null || ReferenceEquals(labelText, ""))
                            labelEventArgs.lbText += LabelField.Field.Trim();
                        else
                        {
                            labelEventArgs.lbText += labelText;
                        }
                    }
                }
                //获取标注样式（颜色、大小)
                GetG3e_textstyle(label.Name, labelEventArgs);
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(string.Format("获取标注样式失败！G3E_FID：{0}{1}", electronSymbol.G3E_FID, exception));
            }
            return labelEventArgs;
        }

        /// <summary>
        /// 获取标注样式
        /// </summary>
        /// <param name="g3e_cno"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="g3e_fno"></param>
        /// <param name="labelEventArgs"></param>
        /// <returns></returns>
        private static LabelEventArgs GetLabelTextStyle(string g3e_cno, long g3e_fid, long g3e_fno, LabelEventArgs labelEventArgs)
        {
            try
            {
                var dbSymbolEntry = GetDBSymbolEntry(g3e_fno);
                if (dbSymbolEntry == null) return labelEventArgs;
                //获取标注配置文件
                var label = dbSymbolEntry.Label.SingleOrDefault(o => o.CNO == g3e_cno);
                foreach (var LabelField in label.LabelCompose)
                {
                    //标注字段对应的表名
                    var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), LabelField.CorrespondTableName.Trim());
                    var dbEntity = labelEventArgs.xmlDB.GetEntity(type, g3e_fid);
                    if (dbEntity == null) return labelEventArgs;
                    //判断标注字段是否存在
                    if (!dbEntity.HasAttribute(LabelField.Field.Trim())) continue;
                    //获取标注值
                    var labelText = dbEntity.GetValue(LabelField.Field.Trim());
                    //多条数据表示该标注又多个字段组合而成
                    if (label.LabelCompose.Count > 1)
                    {
                        //如果没有值则显示字段名称
                        if (labelText == null || ReferenceEquals(labelText, ""))
                            continue;
                        labelEventArgs.lbText += labelText + LabelField.Unit.Trim();
                    }
                    else
                    {
                        if (labelText == null || ReferenceEquals(labelText, ""))
                            continue;
                        labelEventArgs.lbText += labelText;
                    }
                }
                if (!string.IsNullOrEmpty(labelEventArgs.lbText))
                    GetG3e_textstyle(label.Name, labelEventArgs); //获取标注样式（颜色、大小)
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(string.Format("获取标注样式失败！G3E_FID：{0}{1}", g3e_fid, exception));
            }
            return labelEventArgs;
        }

        #endregion
    }
}
