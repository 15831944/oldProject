using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using DotNetARX;
using ElectronTransferDal.Common;
using ElectronTransferDal.Factory;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Query;
using ElectronTransferDal.XmlDal;
using ElectronTransferFramework;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using CADException = Autodesk.AutoCAD.Runtime.Exception;    
using CADColor = Autodesk.AutoCAD.Colors.Color;
using Exception = System.Exception;
using ElectronTransferModel.V9_4;
using ElectronTransferModel;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferDal.Cad
{
    public class DCadApi
    {
        /// <summary>
        /// 是否加载了符号库
        /// </summary>
        public static bool IsLoadSymbolLibrary;
        /// <summary>
        /// 是否插入了符号
        /// </summary>
        public static bool isInsertSymbol;
        /// <summary>
        /// 是否是自定义符号
        /// </summary>
        public static bool isCustomSymbol;
        /// <summary>
        /// 是否修改了符号
        /// </summary>
        public static bool isModifySymbol;

        /// <summary>
        /// 是否删除回滚
        /// </summary>
        public static bool isEraseRollback;
        
        /// <summary>
        /// 是否刷新图层状态
        /// </summary>
        public static bool isRefreshLayer;

        public static bool isShowLayerManage;

       
        public static readonly string RegAppName = "GripFilerXdata";
       
        /// <summary>
        /// 添加扩展属性
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="RegAppName"></param>
        static public void SaveExtendedData(Entity ent,string RegAppName)
        {
            using (PublicMethod.Instance.acDoc.LockDocument())
            {
                using (var transaction = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
                {
                    var rat = (RegAppTable)transaction.GetObject(PublicMethod.Instance.DB.RegAppTableId, OpenMode.ForRead, false);
                    if (!rat.Has(RegAppName))
                    {
                        rat.UpgradeOpen();
                        var regapp = new RegAppTableRecord {Name = RegAppName};
                        rat.Add(regapp);
                        transaction.AddNewlyCreatedDBObject(regapp, true);
                    }
                    transaction.Commit();
                }
            }
            var rb = new ResultBuffer
                         {
                             new TypedValue((int) DxfCode.ExtendedDataRegAppName, RegAppName),
                             new TypedValue((int) DxfCode.ExtendedDataInteger32, 148)
                         };

            ent.XData = rb;
        }

        /// <summary>
        /// 在cad上添加一个图层
        /// </summary>
        /// <param name="_layerName">图层名称</param>
        /// <returns></returns>
        public static ObjectId addLayer(string _layerName)
        {
            try
            {
                return addLayer(_layerName, CADColor.FromRgb(255, 0, 0));
            }
            catch (Exception exx)
            {
                LogManager.Instance.Error(exx);
            }
            return ObjectId.Null;
        }

        /// <summary>
        /// 在cad上添加一个图层
        /// </summary>
        /// <param name="LayerName">图层名称</param>
        /// <param name="_c">颜色 </param>
        /// <returns></returns>
        public static ObjectId addLayer(string LayerName, CADColor _c)
        {
            ObjectId layerId;
            var db = HostApplicationServices.WorkingDatabase;
            var doc = Application.DocumentManager.MdiActiveDocument;
            using (doc.LockDocument())
            {
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    var lt = (LayerTable) trans.GetObject(db.LayerTableId, OpenMode.ForWrite);

                    if (lt.Has(LayerName) == false)
                    {
                        var ltr = new LayerTableRecord {Name = LayerName, Color = _c};
                        layerId = lt.Add(ltr);
                        trans.AddNewlyCreatedDBObject(ltr, true);
                    }
                    else
                    {
                        layerId = lt[LayerName];
                    }
                    db.Clayer = layerId;
                    trans.Commit();
                }
            }
            return layerId;
        }

        #region 在cad指定图层上添加电力符号

        /// <summary>
        /// 在cad指定图层上添加标注
        /// </summary>
        /// <param name="layerId">图层id</param>
        /// <param name="esymb">标注对象</param>
        /// <param name="esymlb"> </param>
        /// <param name="color"> </param>
        /// <param name="xmlDbManager"> </param>
        /// <returns></returns>
        public static void addLabelLayer(ObjectId layerId, ElectronSymbol esymb, DBEntity esymlb, CADColor color, XmlDBManager xmlDbManager)
        {
            if (esymb.G3E_GEOMETRY is Multipoint)
            {
                SymbolLabel.DrawLabelSymbol(layerId, esymb, esymlb, xmlDbManager);
            }
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
        public static void addLabelLayer(string _layerName, IEnumerable<ElectronSymbol> _esymb, IEnumerable<ElectronBase> lbs, CADColor color, XmlDBManager xmlDbManager)
        {
            try
            {
                var objId = addLayer(_layerName, CADColor.FromRgb(255, 255, 255));
                if (objId != ObjectId.Null)
                {
                    foreach (var _ee in _esymb)
                    {
                        if (_ee.EntityState != EntityState.Delete)
                        {
                            var lb = GetLabel_LB(lbs, _ee.G3E_FID); 
                            addLabelLayer(objId, _ee, lb, color, xmlDbManager);
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                LogManager.Instance.Error(exx);
            }
        }
        public static ElectronBase GetLabel_LB(IEnumerable<ElectronBase> lbs, long g3e_fid)
        {
            try
            {
                ElectronBase eb = null;
                if (lbs != null && lbs.Any())
                {
                    eb = lbs.SingleOrDefault(o => o.G3E_FID == g3e_fid && !o.Duplicated);
                }
                return eb;
            }
            catch (Exception exception)
            {
                LogManager.Instance.Error(string.Format("G3E_FID：{0}\n{1}" ,g3e_fid,exception));
            }
            return null;
        }

        /// <summary>
        /// 在cad指定图层上添加多个标注
        /// </summary>
        /// <param name="layerID">图ID</param>
        /// <param name="_esymb">多个标注对象</param>
        /// <param name="color"> </param>
        /// <param name="xmlDbManager"> </param>
        /// <returns></returns>
        public static void addLabelLayer(ObjectId layerID, IEnumerable<ElectronSymbol> _esymb, CADColor color,XmlDBManager xmlDbManager)
        {
            try
            {
                if (layerID != ObjectId.Null)
                {
                    foreach (var _ee in _esymb)
                    {
                        addLabelLayer(layerID, _ee,null, color,xmlDbManager);
                    }
                }
            }
            catch (Exception exx)
            {
                LogManager.Instance.Error(exx);
            }
        }
   


        public static void AddDBSymbolLTTIDFinder(ElectronSymbol es, Extents3d extents, string sbmc, ObjectId objectId)
        {
            if(!DBSymbolLTTIDFinder.Instance.ContainsKey(es.G3E_FID))
            {
                DBSymbolLTTIDFinder.Instance.Add(es.G3E_FID,
                                                 new EntityObj
                                                     {
                                                         objectType=true,
                                                         objectID = objectId,
                                                         LTT_ID = (Decimal) es.GetValue("LTT_ID"),
                                                         maxPoint = extents.MaxPoint,
                                                         minPoint = extents.MinPoint,
                                                         SBMC = sbmc,
                                                         G3e_Fno = es.G3E_FNO
                                                     });
            }
        }

        /// <summary>
        /// 保存所有设备的坐标、其他信息（用以计算工单锁定范围）
        /// </summary>
        /// <param name="es"></param>
        /// <param name="objectId"></param>
        /// <param name="position"></param>
        /// <param name="SBMC"></param>
        public static void AddDBSymbolLTTIDFinder(ElectronSymbol es, ObjectId objectId, Point3d position, string SBMC)
        {
            if (!DBSymbolLTTIDFinder.Instance.ContainsKey(es.G3E_FID))
            {
                DBSymbolLTTIDFinder.Instance.Add(es.G3E_FID,
                                                 new EntityObj
                                                     {
                                                         objectType=true,
                                                         objectID = objectId,
                                                         LTT_ID = (Decimal) es.GetValue("LTT_ID"),
                                                         maxPoint = position,
                                                         minPoint = position,
                                                         SBMC = SBMC,
                                                         G3e_Fno = es.G3E_FNO
                                                     });
            }
        }
        public static void AddDBSymbolLTTIDFinder(long g3e_fid,EntityObj entityObj)
        {
            if (!DBSymbolLTTIDFinder.Instance.ContainsKey(g3e_fid))
            {
                DBSymbolLTTIDFinder.Instance.Add(g3e_fid,entityObj);
            }
        }

        public static Polyline SetPolyline(ObjectId layerId, CADColor color, string lineType)
        {
            var pl = new Polyline
            {
                LayerId = layerId,
                Color = color,
                LinetypeScale = 2,
                Linetype = lineType
            };
            return pl;
        }


        #endregion     

        #region 绘制点数据

        /// <summary>
        /// 准备数据
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <param name="symbolObj"></param>
        /// <param name="specialDev"></param>
        /// <returns></returns>
        public static InsertSymbolEventArgs InitializeInsertSymbolValue(int g3e_fno,XProps symbolObj, SpecilaDevField specialDev)
        {
            var g3e_id = CYZCommonFunc.getid();
            var g3e_fid = CYZCommonFunc.getid();
            var ise = new InsertSymbolEventArgs
            {
                symbolObj = symbolObj,
                g3e_fno = g3e_fno,
                g3e_fid = g3e_fid,
                g3e_id = g3e_id,
                spf = specialDev
            };
            //找符号
            ExecutionManager.Instance.GetFactory(typeof(InsertSymbolExecutionFactory)).GetExecution(g3e_fno).Execute(null, ise);
            return ise;
        }
        /// <summary>
        /// 特殊设备属性设置
        /// </summary>
        /// <param name="fno">设备fno</param>
        /// <param name="obj">插入的设备</param>
        public static void SetDevAttribute(int fno, ref  XProps obj)
        {
            if (fno == 147 || fno == 151)
            {
                var smzq = GenerateHelper.GetPropertyValue(obj, "Common_n", "CD_SMZQ");
                if (smzq != null)
                {
                    if (smzq.ToString().Equals("待投运"))
                        GenerateHelper.SetPropertyValue(obj, "Common_n", "BZ", "备用");
                    GenerateHelper.SetPropertyValue(obj, "Common_n", "BZ", null);
                }
            }
        }
        /// <summary>
        /// 手动添加设备
        /// </summary>
        /// <param name="b_Name">符号名称：148_0_变压器</param>
        /// <param name="symbolObj">属性对象</param>
        /// <param name="specialDev"> </param>
        public static void InsertSymbol(string b_Name, XProps symbolObj, SpecilaDevField specialDev)
        {
            var g3e_fno = 0;
            isInsertSymbol = true;
            isModifySymbol = true;
            var blockDefinitionName = string.Empty;
            try
            {
                //获取图元块名称
                GetSymbolInfoByFno(ref b_Name, ref blockDefinitionName, ref g3e_fno);
                //特殊符号特殊设置
                SetDevAttribute(g3e_fno, ref symbolObj);
                //找符号
                var ise = InitializeInsertSymbolValue(g3e_fno, symbolObj, specialDev);
                //添加图层
                var layerId = addLayer(b_Name, CADColor.FromRgb(0, 0, 0));
                switch (ise.isSymbolType)
                {
                    //点符号
                    case egtype.multipoint:
                        InsertBlock(ise, g3e_fno, layerId);
                        break;
                    //线符号
                    case egtype.linestring:
                        PublicMethod.Instance.DrawPolyLineJig(ise, layerId);
                        break;
                    //面符号
                    case egtype.polygon:
                        PublicMethod.Instance.DrawRectangle(g3e_fno, layerId, ise);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                isInsertSymbol = false;
                isModifySymbol = false;
            }
        }
        /// <summary>
        /// 添加符号
        /// </summary>
        /// <param name="ise"></param>
        /// <param name="g3e_fno"></param>
        /// <param name="layerId"></param>
        private static void InsertBlock(InsertSymbolEventArgs ise,long g3e_fno,ObjectId layerId)
        {
            var newBlockName=string.Empty;
            //获取块定义名称
            var blockName = string.IsNullOrEmpty(ise.bTypeStr)
                                ? g3e_fno.ToString()
                                : string.Format("{0}_{1}", g3e_fno, ise.bTypeStr);
            InsertBlock(blockName, ref newBlockName);
            var bt = PublicMethod.Instance.BlockTable(PublicMethod.Instance.WorkingDataBase);
            if (bt.Has(newBlockName))
            {
                var objId = ObjectId.Null;
                var objectid = bt[newBlockName];

                using (var bref = new BlockReference(Point3d.Origin, objectid))
                {
                    bref.LayerId = layerId;
                    bref.Color = ise.symbolColor;
                    bref.ScaleFactors = new Scale3d(ise.scale);
                    if (BlockJig.Jig(bref, true))
                    {
                        AddPinchPoint(bref);
                        AddLabelPinchPoint(bref);
                        objId = PublicMethod.Instance.ToModelSpace(bref);
                        SaveSymbol(ise, objId);
                    }
                }
            }
            else
            {
                PublicMethod.Instance.ShowMessage(string.Format("{0}符号不存在！", blockName));
            }
        }



        /// <summary>
        /// 保存符号数据
        /// </summary>
        /// <param name="ise"></param>
        /// <param name="objectId"></param>
        public static void SaveSymbol(InsertSymbolEventArgs ise, ObjectId objectId)
        {
            var obj = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead);
            var blockRef = obj as BlockReference;
            //转换实体坐标数据
            ise.multipointValue = ConvertGeometry.Instance.GetMultipoint(objectId);

            ise.blockName = blockRef.Name;
            if (ise.g3e_fno == 148)
            {
                AddAttribute(ise);
            }
            //数据保存到xml
            ExecutionManager.Instance.GetFactory(typeof(InsertToXmlExecutionFactory)).
                GetExecution(ise.g3e_fno).Execute(null, ise);

            AddDBSymbolLTTIDFinder(ise.insertobj, objectId, blockRef.Position, ise.SBMC);
            PublicMethod.Instance.AddDBSymbolFinder(objectId, ise.insertobj);
            SymbolLabel.AddSymbolLabel(ise, blockRef.Position.X, blockRef.Position.Y, blockRef.Rotation);
        }


        /// <summary>
        /// 添加属性(添加没有符号的设备数据)
        /// </summary>
        /// <param name="ise"></param>
        private static void AddAttribute(InsertSymbolEventArgs ise)
        {
            var fl = GenerateHelper.GetPropertyValue(ise.symbolObj, "Gg_pd_gnwzmc_n", "GNWZ_FL2");
            if (fl!=null&&fl.ToString().Contains("台架"))
            {
                var dict = AddAttribute(ise.symbolObj, 199,ise.g3e_fid);
                if (dict != null)
                {
                    GenerateHelper.Unrf.tjFid = dict.G3eFid;
                    ise.spf.Sstj = GenerateHelper.Unrf.tjFid;
                }
            }
            else
            {
                GenerateHelper.Unrf.tjFid = null;
                ise.spf.Sstj = null;
            }
        }


        /// <summary>
        /// 添加属性(添加没有符号的设备数据)
        /// </summary>
        /// <param name="symbolObj"></param>
        /// <param name="g3e_fno"> </param>
        /// <param name="parentFeatureFid"></param>
        public static ChoiceDev AddAttribute(XProps symbolObj, int g3e_fno,long parentFeatureFid)
        {
            var g3e_id = CYZCommonFunc.getid();
            var g3e_fid = CYZCommonFunc.getid();
            var ise = new InsertSymbolEventArgs
            {
                symbolObj = symbolObj,
                g3e_fno = g3e_fno,
                g3e_fid = g3e_fid,
                g3e_id = g3e_id,
                parentFid=parentFeatureFid
            };
            ExecutionManager.Instance.GetFactory(typeof(InsertToXmlExecutionFactory)).GetExecution(g3e_fno).Execute(null, ise);
            var eo = new EntityObj
            {
                G3e_Fno = g3e_fno,
                SBMC = ise.SBMC,
                LTT_ID = MapConfig.Instance.LTTID
            };
            AddDBSymbolLTTIDFinder(g3e_fid, eo);
            return new ChoiceDev
            {
                G3eFid=g3e_fid.ToString(),
                DeviceSbmc=ise.SBMC
            };
        }

        /// <summary>
        /// 添加支线
        /// </summary>
        /// <param name="objectId">对象ID</param>
        /// <param name="symbolObj">数据对象</param>
        /// <param name="g3e_fno">FNO</param>
        /// <param name="lineString"> </param>
        public static void InsertSymbol(ObjectId objectId, XProps symbolObj, int g3e_fno,LineString lineString)
        {
            isInsertSymbol = true;
            isModifySymbol = true;
            try
            {
                //找符号
                var ise = InitializeInsertSymbolValue(g3e_fno, symbolObj, new SpecilaDevField());
                //添加数据
                SaveLateralData(ise, objectId, lineString);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                isInsertSymbol = false;
                isModifySymbol = false;
            }
        }

        /// <summary>
        /// 保存支线数据
        /// </summary>
        /// <param name="ise">数据对象</param>
        /// <param name="objectId">对象ID</param>
        /// <param name="lineString"> </param>
        private static void SaveLateralData(InsertSymbolEventArgs ise, ObjectId objectId, LineString lineString)
        {
            if (ise.g3e_fno==141)
            {
                ise.lineStringValue = lineString;
                ExecutionManager.Instance.GetFactory(typeof(InsertToXmlExecutionFactory)).GetExecution(ise.g3e_fno).Execute(null, ise);
                PublicMethod.Instance.AddDBSymbolFinder(objectId, ise.insertobj);
            }
            else
            {
                //转换成实体
                var obj = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead);
                var blockRef = obj as BlockReference;
                //转换该实体坐标数据
                ise.multipointValue = ConvertGeometry.Instance.GetMultipoint(objectId);
                ise.blockName = blockRef.Name;
                //把数据添加到xml
                ExecutionManager.Instance.GetFactory(typeof(InsertToXmlExecutionFactory)).
                GetExecution(ise.g3e_fno).Execute(null, ise);
                //保存数据记录
                PublicMethod.Instance.AddDBSymbolFinder(objectId, ise.insertobj);
            }
        }
        
        private static string GetLastIndexOfStr(string str)
        {
            if (str != null)
            {
                var index = str.LastIndexOf("_", StringComparison.Ordinal);
                if(index>0)
                {
                    str = str.Substring(index+1, str.Length - index-1);
                }
            }
            return str;
        }
        private static string GetIndexOfStr(string str,bool bl)
        {
            var index = bl ? str.IndexOf("_") : str.LastIndexOf("_", StringComparison.Ordinal);
            if (index > 0)
            {
                str = str.Substring(0,index);
            }
            return str;
        }
        /// <summary>
        /// 添加标注
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="angle"></param>
        public static void AddSymbolLabel(ObjectId objectId,double angle)
        {
            //转换成实体
            var obj = PublicMethod.Instance.GetObject(objectId, OpenMode.ForRead);

            if (obj is BlockReference)
            {
                var blockRef = obj as BlockReference;
                var multipointValue = ConvertGeometry.Instance.GetMultipoint(objectId);
                var values = DBSymbolFinder.Instance[objectId];
                var ise = new InsertSymbolEventArgs
                              {
                                  g3e_id = values.G3E_ID,
                                  g3e_fid = values.G3E_FID,
                                  g3e_fno = values.G3E_FNO,
                                  blockName = blockRef.Name,
                                  multipointValue = multipointValue,
                                  insertobj=values
                              };
                SymbolLabel.AddSymbolLabel(ise, blockRef.Position.X, blockRef.Position.Y, blockRef.Rotation);
            }else if(obj is Polyline)
            {
                var pl = obj as Polyline;
                var multipointValue = ConvertGeometry.Instance.GetLineString(objectId);
                var values = DBSymbolFinder.Instance[objectId];
                var ise = new InsertSymbolEventArgs
                {
                    g3e_id = values.G3E_ID,
                    g3e_fid = values.G3E_FID,
                    g3e_fno = values.G3E_FNO,
                    lineStringValue = multipointValue,
                    insertobj=values
                };
                SymbolLabel.AddSymbolLabel(ise, pl.StartPoint.X, pl.StartPoint.Y, angle);
            }
        }

        /// <summary>
        /// 获取符号信息 如:148_0_变压器
        /// </summary>
        /// <param name="blockName">名称：变压器</param>
        /// <param name="blockDefinitionName">块定义名称：148_0</param>
        /// <param name="fno">fno:148</param>
        /// <returns></returns>
        public static void GetSymbolInfoByFno(ref string blockName, ref string blockDefinitionName, ref int fno)
        {
            if (blockDefinitionName == null) throw new ArgumentNullException("blockDefinitionName");
            fno = int.Parse(GetIndexOfStr(blockName, true));
            blockDefinitionName = GetIndexOfStr(blockName, false);
            blockName = GetLastIndexOfStr(blockName);
        }

        public static PointSymbolData GetSymbolDataByObjId(ObjectId objId)
        {
            G3EObject g3eObject = null;
            if (!DBEntityFinder.Instance.GetG3EIds(objId, ref g3eObject))
                return null;
            PointSymbolData symbolInfo;
            var entity = PublicMethod.Instance.GetObject(objId, OpenMode.ForRead) as Entity;
            if (entity == null) return null;
            if (entity is BlockReference)
            {
                var blockReference = entity as BlockReference;
                symbolInfo = new PointSymbolData
                                 {
                                     g3eObject = g3eObject,
                                     blockName = blockReference.Name,
                                     point = blockReference.Position,
                                     rotateAngle = blockReference.Rotation,
                                     color = blockReference.Color,
                                     objectId = blockReference.ObjectId
                                 };
            }
            else if (entity is Polyline)
            {
                var polyline = entity as Polyline;
                symbolInfo = new PointSymbolData
                                 {
                                     color = polyline.Color,
                                     g3eObject = g3eObject,
                                     objectId = polyline.ObjectId
                                 };
            }
            else
            {
                return null;
            }
            return symbolInfo;
        }

        /// <summary>
        /// 更新符号
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="symObj"> </param>
        /// <param name="newObjId"> </param>
        public static bool UpdateSymbol(ObjectId objectId, XProps symObj,out ObjectId newObjId)
        {
            bool flag = false;
            try
            {
                var psd = GetSymbolDataByObjId(objectId);
                if (psd == null)
                {
                    newObjId = objectId;
                    return flag;
                }
                newObjId = psd.objectId;
                var pt = PublicMethod.Instance.GetDevPoint3d(psd.objectId);
                if (pt.X != 0 && pt.Y != 0)
                {
                    psd.point = pt;
                }
                flag = true;
                try
                {
                    var objId = ObjectId.Null;
                    var ise = new InsertSymbolEventArgs { symbolObj = symObj, g3e_fno = psd.g3eObject.G3E_FNO };

                    //根据属性找符号
                    ExecutionManager.Instance.GetFactory(typeof(InsertSymbolExecutionFactory)).GetExecution(
                        psd.g3eObject.G3E_FNO).Execute(null, ise);

                    var blockDefinitionName = string.IsNullOrEmpty(ise.bTypeStr)
                                                  ? null
                                                  : string.Format("{0}_{1}", psd.g3eObject.G3E_FNO, ise.bTypeStr);

                    //点符号（点符号是以CAD图块的形式存在的）
                    if (!string.IsNullOrEmpty(blockDefinitionName) && !string.IsNullOrEmpty(psd.blockName))
                    {
                        if (blockDefinitionName != psd.blockName)
                        {
                            psd.blockName = blockDefinitionName;
                            psd.color = ise.symbolColor;
                            if (!ReplacePointSymbol(psd, ref objId)) //替换符号失败
                            {
                                flag = false;
                            }
                            else
                            {
                                newObjId = objId;
                            }
                        }
                        else
                        {
                            //下面是针对点符号，只改变颜色的
                            if (psd.color != ise.symbolColor)
                            {
                                EditorPointSymbol(psd.objectId, ise.symbolColor);
                                psd.color = ise.symbolColor;
                            }
                        }
                    } //线面符号
                    else
                    {
                        //这里是针对线改变颜色
                        if (psd.color != ise.symbolColor)
                        {
                            EditorPointSymbol(psd.objectId, ise.symbolColor);
                            psd.color = ise.symbolColor;
                        }
                    }
                }
                catch
                {
                    flag = false;
                }
            }catch
            {
                newObjId = objectId;
            }
            return flag;
        }

        /// <summary>
        /// 检测插入设备必填项是否有值
        /// </summary>
        public static bool CheckFieldIsNull(object obj)
        {
            try
            {
                foreach (var attr in obj.GetType().GetProperties())
                {
                    var props = TypeDescriptor.GetProperties(obj)[attr.Name];
                    var attri = (BrowsableAttribute)props.Attributes[typeof(BrowsableAttribute)];
                    var dispalyname = (DisplayNameAttribute)Attribute.GetCustomAttribute(attr, typeof(DisplayNameAttribute));
                    if (attri != null)
                    {
                        if (attri.Browsable)
                        {
                            var value = obj.GetValue(attr.Name);
                            if (value == null || value.ToString().Trim().Length <= 0||value.ToString().Equals("0"))
                            {
                                PublicMethod.Instance.AlertDialog(dispalyname.DisplayName + " 字段输入有误或不能为空！！！");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        PublicMethod.Instance.AlertDialog(dispalyname.DisplayName + " 字段没有浏览属性！！！");
                        return false;
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return true;
        }
        /// <summary>
        /// 获取块定义名称
        /// </summary>
        /// <param name="e"></param>
        /// <param name="blockName"></param>
        public static string GetBlockDefinitionName(SymbolEventArgs e, string blockName)
        {
            try
            {
                if (!string.IsNullOrEmpty(e.SymbolType))
                {
                    blockName = string.Format("{0}_{1}", blockName, e.SymbolType);
                }
                if (!string.IsNullOrEmpty(e.BreakerStatus))
                {
                    blockName = string.Format("{0}_{1}", blockName, e.BreakerStatus);
                    
                }
            }catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return blockName;
        }

        /// <summary>
        /// 插入块定义
        /// </summary>
        /// <param name="blockName"></param>
        /// <param name="newBolckName"></param>
        public static void InsertBlock(string blockName,ref string newBolckName)
        {
            try
            {
                //查询是否已插入过
                var value=DBEntityFinder.Instance.blockList.FirstOrDefault(o => o.Equals(blockName));
                if (!string.IsNullOrEmpty(value))
                {
                    newBolckName = blockName;
                }
                else
                {
                    var dicBlock = DBEntityFinder.Instance.blockAllList.SingleOrDefault(o => o.Key.Equals(blockName));
                    if (!string.IsNullOrEmpty(dicBlock.Value))
                    {
                        var filePath = dicBlock.Value;
                        if (File.Exists(filePath))
                        {
                            var result=InsertBlock(ref newBolckName, filePath);
                            if (result)
                            {
                                if (!DBEntityFinder.Instance.blockList.Equals(newBolckName))
                                    DBEntityFinder.Instance.blockList.Add(newBolckName);
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 插入块定义
        /// </summary>
        /// <param name="blockList">符号库路径</param>
        public static void InsertBlock(List<string> blockList)
        {
            try
            {
                var acDb = PublicMethod.Instance.WorkingDataBase;
                using (PublicMethod.Instance.acDoc.LockDocument())
                {
                    using (var tr = acDb.TransactionManager.StartTransaction())
                    {
                        foreach (var item in blockList)
                        {
                            var exDb = new Database(false, true);
                            exDb.ReadDwgFile(item, FileShare.ReadWrite, true, null);
                            var fno = Path.GetFileNameWithoutExtension(item);
                            var index = fno.LastIndexOf("_", StringComparison.Ordinal);
                            if (index > 0)
                            {
                                //例如：208_0_公变  只插入208_0 208是fno号 0是变压器的分类编号
                                fno = fno.Substring(0, index);
                                acDb.Insert(fno, exDb, false);
                            }
                        }
                        acDb.CloseInput(true);
                        tr.Commit();
                    }
                }
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 插入块定义
        /// </summary>
        /// <param name="blockName">符号库路径</param>
        /// <param name="filePath"> </param>
        public static bool InsertBlock(ref string blockName,string filePath)
        {
            bool result;
            try
            {
                var acDb = PublicMethod.Instance.WorkingDataBase;
                using (var tr = acDb.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        var exDb = new Database(false, true);
                        exDb.ReadDwgFile(filePath, FileShare.ReadWrite, true, null);
                        var fno = Path.GetFileNameWithoutExtension(filePath);
                        var index = fno.LastIndexOf("_", StringComparison.Ordinal);
                        if (index > 0)
                        {
                            //例如：208_0_公变  只插入208_0 208是fno号 0是变压器的分类编号
                            blockName = fno.Substring(0, index);
                            acDb.Insert(blockName, exDb, false);
                        }
                        result = true;
                        acDb.CloseInput(true);
                        tr.Commit();
                    }
                }
            }
            catch (CADException ex)
            {
                result = false;
                LogManager.Instance.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 加载块定义
        /// </summary>
        /// <param name="dirPath"></param>
        public static void LoadBolckDefinition(string dirPath)
        {
            var blockList = new Dictionary<string, string>();
            if (DBEntityFinder.Instance.blockAllList == null)
                DBEntityFinder.Instance.blockAllList = GetListBySymbolPath(dirPath, false, blockList);
            //初始化
            DBEntityFinder.Instance.blockList = new List<string>();
            //var blockList = new List<string>();
            //blockList = GetListBySymbolPath(dirPath, false, blockList);
            //InsertBlock(blockList);
            IsLoadSymbolLibrary=true;
        }

        /// <summary>
        /// 递归遍历符号库，把符号插入cad块定义中
        /// </summary>
        /// <param name="dirPath">符号库路径</param>
        /// <param name="IsFolder"> </param>
        /// <param name="blockList"> </param>
        private static List<string> GetListBySymbolPath(string dirPath, bool IsFolder, List<string> blockList)
        {
            try
            {
                var directory = new DirectoryInfo(dirPath);
                var fileSystemInfos = !IsFolder ? directory.GetFileSystemInfos() : directory.GetFileSystemInfos("*.dwg");
                foreach (var fileSystemInfo in fileSystemInfos)
                {
                    if (fileSystemInfo is DirectoryInfo)
                    {
                        blockList = GetListBySymbolPath(fileSystemInfo.FullName, true, blockList);
                    }
                    else
                    {
                        blockList.Add(fileSystemInfo.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return blockList;
            }
            return blockList;
        }
         /// <summary>
        /// 递归遍历符号库，把符号插入cad块定义中
        /// </summary>
        /// <param name="dirPath">符号库路径</param>
        /// <param name="IsFolder"> </param>
        /// <param name="blockList"> </param>
        private static Dictionary<string, string> GetListBySymbolPath(string dirPath, bool IsFolder, Dictionary<string, string> blockList)
        {
            try
            {
                var directory = new DirectoryInfo(dirPath);
                var fileSystemInfos = !IsFolder ? directory.GetFileSystemInfos() : directory.GetFileSystemInfos("*.dwg");
                foreach (var fileSystemInfo in fileSystemInfos)
                {
                    if (fileSystemInfo is DirectoryInfo)
                    {
                        blockList = GetListBySymbolPath(fileSystemInfo.FullName, true, blockList);
                    }
                    else
                    {
                        var fno = Path.GetFileNameWithoutExtension(fileSystemInfo.FullName);
                        var index = fno.LastIndexOf("_", StringComparison.Ordinal);
                        if (index > 0)
                        {
                            //例如：208_0_公变  只插入208_0 208是fno号 0是变压器的分类编号
                            fno = fno.Substring(0, index);
                        }
                        blockList.Add(fno,fileSystemInfo.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return blockList;
            }
            return blockList;
        }

        
        /// <summary>
        /// 编辑点符号
        /// </summary>
        /// <param name="objectId">实体对象id</param>
        /// <param name="color">颜色</param>
        public static CADColor EditorPointSymbol(ObjectId objectId, CADColor color)
        {
            CADColor oldColor = null;
            try
            {
                isModifySymbol = true;
                using (PublicMethod.Instance.acDoc.LockDocument())
                {
                    using (var tr = PublicMethod.Instance.acDoc.TransactionManager.StartTransaction())
                    {
                        if (objectId.IsNull || !objectId.IsValid || objectId.IsErased) return color;
                        var ent = tr.GetObject(objectId, OpenMode.ForRead) as Entity;
                        using (ent.UpgradeOpenAndRun())
                        {
                            oldColor = ent.Color;
                            ent.Color = color;
                        }
                        tr.Commit();
                    }
                }
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                isModifySymbol = false;
            }
            return oldColor;
        }

        /// <summary>
        /// 自定义图元设备
        /// </summary>
        /// <param name="savePath">保存路径</param>
        /// <param name="symbolName">名称</param>
        /// <param name="objectIdCollection">实体对象集合</param>
        /// <param name="point">基点</param>
        public static bool CustomSymbolSetToDWG(string savePath,string symbolName,ObjectIdCollection objectIdCollection,Point3d point)
        {
            bool reval;
            var dm = Application.DocumentManager;
            var curDb = dm.MdiActiveDocument.Database;
            var destDb = new Database(true, true);

            try
            {
                using (PublicMethod.Instance.acDoc.LockDocument())
                {
                    using (var tr = curDb.TransactionManager.StartTransaction())
                    {
                        curDb.Wblock(destDb, objectIdCollection, point, DuplicateRecordCloning.Replace);
                       
                        tr.Commit();
                        destDb.SaveAs(string.Format(@"{0}\{1}.dwg", savePath,symbolName),DwgVersion.Current);
                        reval = true;
                    }
                }
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error("\n错误: " + ex.Message);
                reval = false;
            }
            return reval;
        }
        ///  <summary> 
        ///  数据库克隆 
        ///  </summary>  
        ///  <param name="idCollection">克隆的对象ID集合</param> 
        ///  <param name="fileName">克隆到的文件名</param> 
        public static void Clone(ObjectIdCollection idCollection, string fileName)
        {
            var ndb = new Database(true, true);
            ObjectId IdBtr;
            var db = idCollection[0].Database;
            var map = new IdMapping();
            using (var trans = ndb.TransactionManager.StartTransaction())
            {
                var bt = (BlockTable)trans.GetObject(ndb.BlockTableId, OpenMode.ForRead);

                var btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                IdBtr = btr.ObjectId;
                trans.Commit();
            }
            db.WblockCloneObjects(idCollection, IdBtr, map, DuplicateRecordCloning.Replace, false);
            ndb.SaveAs(fileName, DwgVersion.Current);
        } 



        /// <summary>
        /// 符号替换
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="objectId"> </param>
        public static bool ReplacePointSymbol(PointSymbolData ps, ref ObjectId objectId)
        {
            try
            {
                isModifySymbol = true;
                if (ps.objectId.IsNull)
                    return false;

                var newBlockName = string.Empty;
                var brf = PublicMethod.Instance.GetObject(ps.objectId, OpenMode.ForRead)as Entity;
                var layerId = brf.LayerId;
                //插入块定义
                InsertBlock(ps.blockName, ref newBlockName);
                var bt = PublicMethod.Instance.BlockTable(PublicMethod.Instance.WorkingDataBase);
                if (bt.Has(newBlockName))
                {
                    var objid = bt[newBlockName];
                    using (var bref = new BlockReference(ps.point, objid))
                    {
                        bref.ScaleFactors = new Scale3d(1.0/MapConfig.Instance.earthscale);
                        bref.LayerId = layerId;
                        bref.Color = ps.color;
                        bref.Rotation = ps.rotateAngle;
                        AddPinchPoint(bref);
                        AddLabelPinchPoint(bref);
                        objectId = PublicMethod.Instance.ToModelSpace(bref);
                    }
                     isEraseRollback = true;
                    //删除旧图形
                    PublicMethod.Instance.EraseObject(ps.objectId);
                    //更新缓存
                    MaintainDBSymbolFinder(ps.objectId, objectId,ps.blockName);
                }
                else
                {
                    PublicMethod.Instance.ShowMessage(string.Format("符号库中不存在该{0}符号\n",newBlockName));
                    return false;
                }
            }
            catch (NullReferenceException ex)
            {
                LogManager.Instance.Error(ex.Message + "__" + ex.Source + "__" + ex.TargetSite);
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally {
                isModifySymbol = false;
            }
            return true;
        }

        /// <summary>
        /// 维护DBSymbolFinder
        /// </summary>
        /// <param name="oldObjId">旧对象ID</param>
        /// <param name="newObjId">新对象ID</param>
        /// <param name="blockName"> </param>
        private static void MaintainDBSymbolFinder(ObjectId oldObjId,ObjectId newObjId,string blockName)
        {
            ElectronSymbol esObj = null;
            if (DBSymbolFinder.Instance.ContainsKey(oldObjId))
            {
                esObj = DBSymbolFinder.Instance[oldObjId];
                esObj.BlockName = blockName;
                DBSymbolFinder.Instance.Remove(oldObjId);
            }
            PublicMethod.Instance.AddDBSymbolFinder(newObjId, esObj);
        }

        /// <summary>
        /// 添加添加XData夹点
        /// </summary>
        /// <param name="bref"> </param>
        public static void AddPinchPoint(Entity bref)
        {
            SaveExtendedData(bref,RegAppName);
        }
        /// <summary>
        /// 添加XData
        /// </summary>
        /// <param name="entity"></param>
        public static void AddLabelPinchPoint(Entity entity)
        {
            SaveExtendedData(entity, DCadDrawEntityOverrule.RegAppName);
        }
        
        #endregion

        #region 还原指定字典里实体颜色
        static public void cleanObjCol(ObjectIdList nodeList, Dictionary<ObjectId, int> dict)
        {
            isModifySymbol = true;
            Database db = HostApplicationServices.WorkingDatabase;
            try
            {
                using (Application.DocumentManager.MdiActiveDocument.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        for (int i = 0; i < nodeList.Count(); i++)
                        {
                            var ent = tr.GetObject(nodeList[i], OpenMode.ForWrite) as Entity;
                            if (ent == null) continue;
                            if (!dict.Keys.Contains(ent.ObjectId)) continue;
                            ent.ColorIndex = dict[ent.ObjectId];
                        }
                        tr.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.AlertDialog("InitializeList 初始化列表错误: " + ex.Message);

            }
            finally
            {
                isModifySymbol = false;
            }
        } 
        #endregion

        #region 根据fid，fno获取设备名称
        static public string getDevNamebyfidfno(long fid, long fno)
        {
            string devName;
            var comm = DBManager.Instance.GetEntity<Common_n>(fid);
            if (comm != null)
            {
                devName = comm.SBMC;
            } 
            else
            {
                //159 160
                switch (fno)
                {
                    case 159://抄表箱
                        var v159 = DBManager.Instance.GetEntity<Gg_pd_cbx_n>(fid);
                        devName = v159.HH;
                        break;
                    case 160://低压散户表
                        var v160 = DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(fid);
                        devName = v160.HH;
                        break;
                    default:
                        devName = "";
                        break;
                }       
            }
            return devName;
        }
        #endregion

        /// <summary>
        /// 加载定位图
        /// </summary>
        public static void LoadFixMap()
        {
            try
            {
                var openFile = new System.Windows.Forms.OpenFileDialog {Filter = "dwg文件|*.dwg"};
                if (openFile.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
                var exDb = new Database(false, false);     //扩展数据库
                var acDb = HostApplicationServices.WorkingDatabase;      // 当前数据库
                var acDoc = Application.DocumentManager.MdiActiveDocument;  //当前文档
                using (acDoc.LockDocument())
                {
                    // 创建定位图图层,并把定位图中的图元全部移动到该图层中,且锁定
                    using (var trans = exDb.TransactionManager.StartTransaction())    //开始事务
                    {
                        exDb.ReadDwgFile(openFile.FileName, FileOpenMode.OpenForReadAndAllShare, true, null);
                        var exDbLayerId = exDb.AddLayer("定位图");
                        exDb.Clayer = exDbLayerId;
                        var bt = (BlockTable)trans.GetObject(exDb.BlockTableId, OpenMode.ForRead);
                        var btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite); 
                        foreach (var btrId in btr)
                        {
                            var ent = trans.GetObject(btrId, OpenMode.ForWrite) as Entity;
                            if (ent == null) continue;
                            ent.LayerId = exDbLayerId;
                        }
                        var dwtLayer = trans.GetObject(exDbLayerId, OpenMode.ForWrite) as LayerTableRecord;
                        if (dwtLayer != null)
                        {
                            dwtLayer.IsLocked = true;
                            //dwtLayer.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(0,0,0);
                        }
                        trans.Commit();
                    }
                    // 把其他图层删除
                    using (var trans = exDb.TransactionManager.StartTransaction()) //开始事务
                    {
                        var lt = trans.GetObject(exDb.LayerTableId, OpenMode.ForWrite, false) as LayerTable;
                        if (lt != null)
                        {
                            foreach (var id in lt)
                            {
                                var ltr = trans.GetObject(id, OpenMode.ForWrite, false) as LayerTableRecord;
                                if (ltr == null || ltr.Name == "定位图" || ltr.Name == "0" || ltr.ObjectId == exDb.Clayer) continue;
                                var idCol = new ObjectIdCollection { id };
                                acDb.Purge(idCol);
                                ltr.Erase();
                            }
                        }
                        trans.Commit();
                    }
                    // 把定位图中设备加进当前DB
                    using (var trans = exDb.TransactionManager.StartTransaction()) //开始事务
                    {
                        acDb.Insert(Matrix3d.Identity, exDb, true);
                        exDb.CloseInput(true);
                        trans.Commit();
                    }
                }
                // 视口移动到定位图范围
                var exMin = exDb.Limmin; //图形范围右上角点的坐标
                var exMax = exDb.Limmax; //图形范围左下角点的坐标
                PublicMethod.Instance.SendCommend(string.Format("zoom\n" + exMin.X + "," + exMin.Y + "\n" + exMax.X + "," + exMax.Y + "\n"));
                PublicMethod.Instance.AlertDialog("定位图加载完成");
            }
            catch (Exception e)
            {
                PublicMethod.Instance.AlertDialog("定位图加载失败.\n" + e.Message);
            }
        }
    }
}
