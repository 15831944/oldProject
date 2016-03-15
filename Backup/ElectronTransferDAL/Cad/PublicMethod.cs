using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DotNetARX;
using ElectronTransferDal.Common;
using ElectronTransferDal.Common.Exceptions;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferDal.Factory;
using ElectronTransferDal.Query;
using ElectronTransferFramework.Serialize;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;
using CADException = Autodesk.AutoCAD.Runtime.Exception;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;
using ElectronTransferFramework;
using Exception = System.Exception;
using Vector3d = Autodesk.AutoCAD.Geometry.Vector3d;
using System;
using CADColor = Autodesk.AutoCAD.Colors.Color;

namespace ElectronTransferDal.Cad
{
    public class PublicMethod : Singleton<PublicMethod>
    {
        #region 全局变量

        public bool active = true;
        public bool wrapup { set; get; }
        public bool echocmd = true;
        public bool isSelecting { set; get; }
        public System.Drawing.Color traceColor = System.Drawing.Color.Aqua;
        /// <summary>
        /// 是否工单锁定
        /// </summary>
        public bool isLockLTTID;

        /// <summary>
        /// 锁定工单图框ID
        /// </summary>
        public ObjectId lockFrameObjectID { set; get; }

        public Dictionary<ObjectId, CADColor> _dicEntities = new Dictionary<ObjectId, Color>();
        /// <summary>
        /// 符号集键值 旧fid，新的实体对象ID
        /// </summary>
        public Dictionary<ObjectId, DBEntity> dicObjIds { set; get; }


        // 1头 != 2头  2头一般为0(单节点) 的设备
        // 74 低压终端, 85 PT柜, 171 用户自带发电机, 172 电动机, 173 站房接地刀闸
        //174 无功补偿器, 177 避雷器, 307 电压互感器, 309 高压电机
        public readonly int[] N2is0 = new[] { 74, 85, 171, 172, 173, 174, 177, 307, 309 };
        //1头 =  2头
        // 143 站房母线, 159 低压集中抄表箱
        public readonly int[] N1isN2 = new[] { 143, 159 };

        //多行文字样式ID
        public ObjectId MultTextStyleId { get; set; }

        #endregion

        #region CAD公共方法

        /// <summary>
        /// 命令行
        /// </summary>
        public Editor Editor
        {
            get { return Application.DocumentManager.MdiActiveDocument.Editor; }
        }

        /// <summary>
        /// 是否让实体处于选中状态
        /// </summary>
        public bool _isSelectedEntityChoice;

        /// <summary>
        /// 当前文档
        /// </summary>
        public Document acDoc
        {
            get { return Application.DocumentManager.MdiActiveDocument; }
        }

        /// <summary>
        /// 得到相应文档的数据库对象
        /// </summary>
        public Database DB
        {
            get { return acDoc.Database; }
        }

        ///  <summary> 
        ///  当前工作的数据库 
        ///  </summary>  
        ///  <returns></returns> 
        public Database WorkingDataBase
        {
            get { return HostApplicationServices.WorkingDatabase; }
        }

        ///  <summary> 
        ///  当前活动文档数据库 
        ///  </summary>  
        ///  <returns></returns> 
        public Database DocumentDatabase()
        {
            return Application.DocumentManager.MdiActiveDocument.Database;
        }

        /// <summary>
        /// 提示
        /// </summary>
        public void AlertDialog(string str)
        {
            Application.ShowAlertDialog(str);
        }

        /// <summary>
        /// 发送AutoCAD命令
        /// </summary>
        /// <param name="strCmd">命令</param>
        /// <returns></returns>
        public bool SendCommend(string strCmd)
        {
            try
            {
                Application.DocumentManager.MdiActiveDocument.SendStringToExecute(
                    strCmd, active, wrapup, echocmd);
                return true;
            }
            catch (CADException exception)
            {
                LogManager.Instance.Error(exception);
                return false;
            }
        }

        /// <summary>
        /// 打印AutoCAD消息
        /// </summary>
        /// <param name="strMsg"></param>
        public void ShowMessage(string strMsg)
        {
            Editor.WriteMessage(strMsg);
            Editor.WriteMessage("\n");
        }

        ///  <summary> 
        ///  获取块表记录ObjectId  
        ///  </summary>  
        ///  <param name="db">数据库</param> 
        ///  <returns></returns> 
        public ObjectId Id(Database db)
        {
            return db.BlockTableId;
        }

        ///  <summary> 
        ///  获取块表 
        ///  </summary>  
        ///  <param name="db">数据库</param> 
        ///  <returns></returns> 
        public BlockTable BlockTable(Database db)
        {
            BlockTable bt;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                tr.Commit();
            }
            return bt;
        }
        ///  <summary> 
        ///  获取块表 
        ///  </summary>  
        ///  <returns></returns> 
        public BlockTable BlockTable()
        {
            BlockTable bt;
            using (var tr = DB.TransactionManager.StartTransaction())
            {
                bt = tr.GetObject(DB.BlockTableId, OpenMode.ForRead) as BlockTable;
                tr.Commit();
            }
            return bt;
        }

        ///  <summary> 
        ///  由块表记录名获得块表记录 
        ///  </summary>  
        ///  <param name="btrName">块表记录名</param> 
        ///  <param name="db">数据库</param> 
        ///  <returns></returns> 
        public BlockTableRecord GetBlock(string btrName, Database db)
        {
            var block = new BlockTableRecord();
            using (var tr = db.TransactionManager.StartTransaction())
            {
                var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                block = tr.GetObject(bt[btrName], OpenMode.ForRead) as BlockTableRecord;
                tr.Commit();
            }
            return block;
        }

        ///  <summary> 
        ///  由对象ObjectId 获得对象 
        ///  </summary>  
        ///  <param name="id">ObjectId </param> 
        ///  <param  name="mode">打开模式</param> 
        ///  <param name="db">数据库</param> 
        ///  <returns></returns> 
        public DBObject GetObject(ObjectId id, OpenMode mode, Database db)
        {
            DBObject Obj=null;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                if (!id.IsErased)
                {
                    Obj = tr.GetObject(id, mode);
                    tr.Commit();
                }
            }
            return Obj;
        }

        ///  <summary> 
        ///  由对象ObjectId 获得对象 
        ///  </summary>  
        ///  <param name="id">ObjectId </param> 
        ///  <param  name="mode">打开模式</param>
        /// <returns></returns> 
        public DBObject GetObject(ObjectId id, OpenMode mode)
        {
            DBObject Obj=null;
            using (var tr =acDoc.Database.TransactionManager.StartTransaction())
            {
                if (!id.IsErased)
                {
                    Obj = tr.GetObject(id, mode);
                    tr.Commit();
                }
            }
            return Obj;
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool EraseObject(ObjectId id)
        {
            using (var tr = WorkingDataBase.TransactionManager.StartTransaction())
            {
                using (acDoc.LockDocument())
                {
                    id.GetObject(OpenMode.ForWrite).Erase(true);
                    tr.Commit();
                }
                return true;
            }
        }

        /// <summary>
        /// 选择实体集合
        /// </summary>
        /// <returns>返回实体集合</returns>
        public ObjectIdCollection SelectEntities(string message)
        {
            var ids = new ObjectIdCollection();
            var options = new PromptSelectionOptions();
            options.MessageForAdding = message;

            var selection = Editor.GetSelection(options);
            if (selection.Status == PromptStatus.OK)
            {
                ids = new ObjectIdCollection(selection.Value.GetObjectIds());
            }
            return ids;
        }

        /// <summary>
        /// 选择实体集合
        /// </summary>
        /// <returns>返回实体集合</returns>
        public ObjectId[] SelectEntities(ref Point3d sPoint, ref Point3d ePoint)
        {
            ObjectId[] objIds = null;
            try
            {
                var ppr =
                    Editor.GetPoint("\n选择第一个点的捕捉窗口:");
                if (ppr.Status != PromptStatus.OK)
                    return null;
                sPoint = ppr.Value;

                ppr =
                    Editor.GetCorner(
                        "\n选择第二点捕获窗口:",
                        sPoint
                        );
                if (ppr.Status != PromptStatus.OK)
                    return null;
                ePoint = ppr.Value;
                var selection = Editor.SelectWindow(sPoint, ePoint);
                if (selection.Status == PromptStatus.OK)
                {
                    objIds = selection.Value.GetObjectIds();
                }
            }
            catch (CADException exception)
            {
                LogManager.Instance.Error(exception);
            }
            return objIds;
        }

        /// <summary>
        /// 实体高亮
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="bl"> </param>
        public void EntityHighlight(ObjectIdCollection ids, bool bl)
        {
            using (acDoc.LockDocument())
            {
                using (var tr = acDoc.TransactionManager.StartTransaction())
                {
                    if (bl)
                        ids.HighlightEntities();
                    else
                        ids.UnHighlightEntities();
                    tr.Commit();
                }
            }
        }

        /// <summary>
        /// 让实体处于选中状态
        /// </summary>
        /// <param name="objectIds">实体集</param>
        public void SetImpliedSelection(ObjectIdCollection objectIds)
        {
            _isSelectedEntityChoice = true;
            if (objectIds.Count > 0)
            {
                var objs = ObjectIdCollectionToOjbectIds(objectIds);
                Editor.SetImpliedSelection(objs);
            }
        }

        public ObjectId[] ObjectIdCollectionToOjbectIds(ObjectIdCollection objectIds)
        {
            var objIds = new ObjectId[objectIds.Count];
            objectIds.CopyTo(objIds, 0);
            return objIds;
        }

        /// <summary>
        /// 选择实体
        /// </summary>
        /// <param name="strPrompt">文字提示</param>
        /// <returns>返回实体id</returns>
        public ObjectId GetEntity(string strPrompt)
        {
            ObjectId id = ObjectId.Null;
            try
            {
                var options = new PromptEntityOptions(strPrompt);
                var entity = Editor.GetEntity(options);
                if (entity.Status == PromptStatus.OK)
                {
                    id = entity.ObjectId;
                }
            }
            catch (CADException)
            {
                return id;
            }
            return id;
        }
        /// <summary>
        ///  选择集
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ObjectId[] GetObjectIds(string message)
        {
            //选择集操作
            var selOpt = new PromptSelectionOptions();
            selOpt.MessageForAdding = message;
            var selRes = Editor.GetSelection(selOpt);
            if (selRes.Status == PromptStatus.OK)
            {
                var sSet = selRes.Value;
                return sSet.GetObjectIds();
            }
            return null;
        }

        /// <summary>
        /// 获取坐标点
        /// </summary>
        /// <param name="strPrompt">文字提示</param>
        /// <returns>返回坐标点</returns>
        public Point3d GetPoint(string strPrompt)
        {
            var options = new PromptPointOptions(string.Format("\n{0}", strPrompt));
            options.AllowNone = true;
            var point = Editor.GetPoint(options);
            if (point.Status != PromptStatus.OK)
            {
                return Point3d.Origin;
            }
            return point.Value;
        }

        /// <summary>
        /// 获取块定义ID
        /// </summary>
        /// <param name="blockName">块定义名称</param>
        /// <returns>块定义ID</returns>
        public ObjectId GetBlockObjId(string blockName)
        {
            var newBlockName = string.Empty;
            DCadApi.InsertBlock(blockName, ref newBlockName);
            var bt = BlockTable();
            if (bt.Has(newBlockName))
            {
                var btr = (BlockTableRecord)GetObject(bt[newBlockName], OpenMode.ForRead);
                return btr.ObjectId;
            }
            return ObjectId.Null;
        }

        /// <summary>
        /// 把实体ent增加到当前database模型空间
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public ObjectId ToModelSpace(Entity ent)
        {
            var entId = new ObjectId();
            var db = WorkingDataBase;
            DCadApi.isModifySymbol = true;
            try
            {
                using (acDoc.LockDocument())
                {
                    using (var trans = db.TransactionManager.StartTransaction())
                    {
                        var bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                        var btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                        entId = btr.AppendEntity(ent);
                        trans.AddNewlyCreatedDBObject(ent, true);
                        trans.Commit();
                    }
                }
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
            return entId;
        }
        

        /// <summary>
        /// 重新生成模型 
        /// </summary>
        public void RegenerateModel()
        {
            //SendCommend("re\n");
            if (!DCadApi.isModifySymbol)
                Editor.Regen();
        }

        /// <summary>
        /// 撤销
        /// </summary>
        public void Unappended()
        {
            DCadApi.isEraseRollback = true;
            SendCommend("u\n");
        }
        /// <summary>
        /// 撤销追加
        /// </summary>
        public void Reappended()
        {
            DCadApi.isEraseRollback = true;
            SendCommend("redo\n");
        }

        /// <summary>
        /// 提示用户输入实数
        /// </summary>
        /// <param name="message"></param>
        /// <returns>用户输入的实数</returns>
        public double GetDouble(string message)
        {
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            var d = ed.GetDouble(message);
            return d.Status == PromptStatus.OK ? d.Value : 0.0;
        }

        /// <summary>
        /// 终止其他命令
        /// </summary>
        public void Termination()
        {
            SendCommend(" ");
        }

        /// <summary>
        /// 是否正在执行系统命令
        /// </summary>
        /// <param name="currentCMD">当前命令字符串</param>
        /// <returns></returns>
        public bool IsExecuteSystemCMD(string currentCMD)
        {
            var obj = GetSystemCMD("CMDNAMES");
            if (!string.IsNullOrEmpty(obj) && !obj.Equals(currentCMD.ToUpper()))
            {
                AlertDialog("上一命令没有终止！不能执行当前操作");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取系统命令
        /// </summary>
        /// <param name="cmdName"></param>
        /// <returns></returns>
        public string GetSystemCMD(string cmdName)
        {
            return Application.GetSystemVariable(cmdName).ToString();
        }

        /// <summary>
        /// 添加DBSymbolFinder
        /// </summary>
        /// <param name="objectId">实体对象ID</param>
        /// <param name="es">实体对象数据</param>
        public void AddDBSymbolFinder(ObjectId objectId, ElectronSymbol es)
        {
            if (!DBSymbolFinder.Instance.ContainsKey(objectId))
            {
                //标记
                es.IsCoordinate = true;
                DBSymbolFinder.Instance.Add(objectId, es);
            }
        }

        /// <summary>
        /// 修改DBSymbolFinder
        /// </summary>
        /// <param name="objectId">实体对象ID</param>
        /// <param name="es">实体对象数据</param>
        public void UpdateDBSymbolFinder(ObjectId objectId, ElectronSymbol es)
        {
            if (DBSymbolFinder.Instance.ContainsKey(objectId))
            {
                DBSymbolFinder.Instance[objectId] = es;
            }
        }

        /// <summary>
        /// 修改DBSymbolFinder
        /// </summary>
        /// <param name="oldObjId">旧对象ID</param>
        /// <param name="newObjId">新对象ID </param>
        public void UpdateDBSymbolFinder(ObjectId oldObjId,ObjectId newObjId)
        {
            if (DBSymbolFinder.Instance.ContainsKey(oldObjId))
            {
                var es= DBSymbolFinder.Instance[oldObjId];
                DBSymbolFinder.Instance.Add(newObjId, es);
            }
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public DBEntity GetDBSymbolFinder(ObjectId objectId)
        {
            DBEntity dbEntity = null;
            if (DBSymbolFinder.Instance.ContainsKey(objectId))
            {
                dbEntity = DBSymbolFinder.Instance[objectId];
            }
            return dbEntity;
        }

        /// <summary>
        /// 刷新屏幕
        /// </summary>
        public void UpdateScreen()
        {
            Application.UpdateScreen();
        }

        #endregion

        #region 图层处理

        /// <summary>
        /// 获取层表􃸼
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public LayerTable GetLayerTable(Database db)
        {
            LayerTable layertable;
            using (var trans = db.TransactionManager.StartTransaction())
            {
                layertable = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
            }
            return layertable;
        }


        /// <summary>
        /// 设置层
        /// </summary>
        /// <param name="layerName"></param>
        /// <param name="isHidden"></param>
        public void SetLayerDisplay(string layerName, bool isHidden)
        {
            try
            {
                using (var tr = acDoc.TransactionManager.StartTransaction())
                {
                    var lt = (LayerTable)tr.GetObject(DB.LayerTableId, OpenMode.ForRead);
                    if (lt.Has(layerName))
                    {
                        using (acDoc.LockDocument())
                        {
                            DCadApi.isModifySymbol = true;
                            var ltr = (LayerTableRecord)tr.GetObject(lt[layerName], OpenMode.ForRead);
                            ltr.UpgradeOpenAndRun();
                            if (ltr.ObjectId == acDoc.Database.Clayer)
                            {
                                ltr.IsOff = isHidden;
                                ltr.IsLocked = isHidden;
                            }
                            else
                            {
                                ltr.IsFrozen = isHidden;
                            }
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
                DCadApi.isModifySymbol = false;
            }
        }

        /// <summary>
        /// 锁图层
        /// </summary>
        /// <param name="layerId">层ID</param>
        /// <param name="isLock">true锁定，false解锁</param>
        public void LockLayer(ObjectId layerId, bool isLock)
        {
            try
            {
                using (var tr = acDoc.TransactionManager.StartTransaction())
                {
                    using (acDoc.LockDocument())
                    {
                        DCadApi.isModifySymbol = true;
                        var ltr = (LayerTableRecord)tr.GetObject(layerId, OpenMode.ForRead);
                        ltr.UpgradeOpenAndRun();
                        ltr.IsLocked = isLock;
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
                DCadApi.isModifySymbol = false;
            }
        }

        /// <summary>
        /// 锁图层
        /// </summary>
        /// <param name="layerName">层ID</param>
        /// <param name="isLock">true锁定，false解锁</param>
        public void LockLayer(string layerName, bool isLock)
        {
            try
            {
                using (var tr = acDoc.TransactionManager.StartTransaction())
                {
                    var lt = (LayerTable) tr.GetObject(DB.LayerTableId, OpenMode.ForRead);
                    if (lt.Has(layerName))
                    {
                        using (acDoc.LockDocument())
                        {
                            DCadApi.isModifySymbol = true;
                            var ltr = (LayerTableRecord) tr.GetObject(lt[layerName], OpenMode.ForRead);
                            ltr.UpgradeOpenAndRun();
                            ltr.IsLocked = isLock;
                            tr.Commit();
                        }
                    }
                }
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
        }

        /// <summary>
        /// 获取层的锁定状态
        /// </summary>
        /// <param name="layerId">层ID</param>
        /// <returns></returns>
        public bool GetLayerLockStatus(ObjectId layerId)
        {
            bool result = false;
            try
            {
                using (var tr = acDoc.TransactionManager.StartTransaction())
                {
                    using (acDoc.LockDocument())
                    {
                        var ltr = (LayerTableRecord)tr.GetObject(layerId, OpenMode.ForRead);
                        result = ltr.IsLocked;
                        tr.Commit();
                    }
                }
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
            return result;
        }
#pragma warning disable 649
        private bool isLayerShowStatus;
        public bool isLayerHiddenStatus;
#pragma warning restore 649
        /// <summary>
        /// 设置标注层的显示状态
        /// </summary>
        /// <param name="pointValue">比例</param>
        public void SetLayerDisplay(double pointValue)
        {
            try
            {
                if (pointValue < 0.0006) //0.00019
                {
                    if (!isLayerShowStatus)
                    {
                        isLayerShowStatus = SetLayerDisplay(false);
                        isLayerHiddenStatus = false;
                    }
                }
                else
                {
                    if (!isLayerHiddenStatus)
                    {
                        isLayerHiddenStatus = SetLayerDisplay(true);
                        isLayerShowStatus = false;
                    }
                }
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 设置所有层显示
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        public bool SetLayerDisplay(bool bl)
        {
            bool result = false;
            try
            {
                DCadApi.isModifySymbol = true;
                if (Instance.isSelecting) return false;
                using (var tr = acDoc.TransactionManager.StartTransaction())
                {
                    var lt = GetLayerTable(DB);
                    using (acDoc.LockDocument())
                    {
                        foreach (ObjectId id in lt)
                        {
                            var ltr = (LayerTableRecord) tr.GetObject(id, OpenMode.ForRead);
                            ltr.UpgradeOpenAndRun();
                            if (ltr.Name.Contains("标注"))
                            {
                                ltr.IsOff = bl;
                                ltr.IsLocked = bl;
                                result = true;
                            }
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
                DCadApi.isModifySymbol = false;
            }
            return result;
        }

        /// <summary>
        /// 设置指定图层
        /// </summary>
        /// <param name="layerId"></param>
        /// <param name="bl"></param>
        /// <returns></returns>
        public bool SetLayerDisplay(ObjectId layerId, bool bl)
        {
            bool result = false;
            try
            {
                DCadApi.isModifySymbol = true;
                using (var tr = WorkingDataBase.TransactionManager.StartTransaction())
                {
                    using (acDoc.LockDocument())
                    {
                        var ltr = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                        ltr.IsOff = bl;
                        ltr.IsLocked = bl;
                        result = true;
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
                DCadApi.isModifySymbol = false;
            }
            return result;
        }

        /// <summary>
        /// 设置指定图层
        /// </summary>
        /// <param name="objectId">对象ID</param>
        /// <param name="isOff">是否打开</param>
        /// <param name="isLocked">是否锁定</param>
        /// <returns></returns>
        public bool SetLayerDisplay(ObjectId objectId, bool isOff,bool isLocked)
        {
            bool result = false;
            try
            {
                DCadApi.isModifySymbol = true;
                using (var tr = acDoc.TransactionManager.StartTransaction())
                {
                    using (acDoc.LockDocument())
                    {
                        var ltr = tr.GetObject(objectId, OpenMode.ForWrite) as LayerTableRecord;
                        ltr.IsOff = isOff;
                        ltr.IsLocked = isLocked;
                        result = true;
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
                DCadApi.isModifySymbol = false;
            }
            return result;
        }

        /// <summary>
        /// 获取图层Off状态
        /// </summary>
        /// <returns></returns>
        public bool GetLayerOff(string layerName)
        {
            var bjisoff = false;
            using (acDoc.LockDocument())
            {
                using (var tr = DB.TransactionManager.StartTransaction())
                {
                    var bjlayerid = DB.AddLayer(layerName);
                    if (!bjlayerid.IsNull || bjlayerid.IsValid)
                    {
                        var bjlayer = tr.GetObject(bjlayerid, OpenMode.ForRead) as LayerTableRecord;
                        if (bjlayer != null)
                            bjisoff = bjlayer.IsOff;
                    }
                    tr.Commit();
                }
            }
            return bjisoff;
        }
        #endregion

        #region 绘制多段线

        /// <summary>
        /// 绘制多段线
        /// </summary>
        /// <param name="ise">数据</param>
        /// <param name="layerId">所属层ID</param>
        public void DrawPolyLineJig(InsertSymbolEventArgs ise, ObjectId layerId)
        {
            try
            {
                var ucs = Editor.CurrentUserCoordinateSystem;
                var jig = new PolylineJig(ucs, ise.lineWidth / MapConfig.Instance.earthscale);
                bool bSuccess = true, bComplete = false, bKeyword = false;
                PromptResult res;
                do
                {
                    res = Editor.Drag(jig);

                    bSuccess = (res.Status == PromptStatus.OK);
                    if (bSuccess)
                    {
                        jig.AddLatestVertex(ise.symbolColor, ise.lineTypeStr, layerId);
                    }
                    bKeyword = (res.Status == PromptStatus.Keyword);
                    if (bKeyword)
                    {
                        jig.RemoveLastVertex();
                    }
                    if (res.Status == PromptStatus.None)
                    {
                        jig.CompletePolyline();
                    }
                    bComplete = (res.Status == PromptStatus.Cancel) || (res.Status == PromptStatus.None);
                } while ((bSuccess && !bComplete) || bKeyword);
                if (bComplete && res.Status != PromptStatus.Cancel)
                {
                    InsertPolyline(ise, jig);
                }
            }
            catch (Exception ex)
            {
                Editor.WriteMessage(ex.Message);
            }
        }
        public ObjectId DrawPolyLineJig(double lineWidth, Color color, string lineType, ObjectId layerId, ref LineString lineString)
        {
            var objectId =ObjectId.Null;
            try
            {
                var ucs = Editor.CurrentUserCoordinateSystem;
                var jig = new PolylineJig(ucs, lineWidth);
                bool bSuccess = true, bComplete = false, bKeyword = false;
                PromptResult res;
                do
                {
                    res = Editor.Drag(jig);

                    bSuccess = (res.Status == PromptStatus.OK);
                    if (bSuccess)
                    {
                        jig.AddLatestVertex(color, lineType, layerId);
                    }
                    bKeyword = (res.Status == PromptStatus.Keyword);
                    if (bKeyword)
                    {
                        jig.RemoveLastVertex();
                    }
                    if (res.Status == PromptStatus.None)
                    {
                        jig.CompletePolyline();
                    }
                    bComplete = (res.Status == PromptStatus.Cancel) || (res.Status == PromptStatus.None);
                } while ((bSuccess && !bComplete) || bKeyword);
                if (bComplete && res.Status != PromptStatus.Cancel)
                {
                    var pl = jig.GetEntity() as Polyline;
                    if (pl != null && pl.NumberOfVertices > 1)
                    {
                        DCadApi.AddPinchPoint(pl);
                        DCadApi.AddLabelPinchPoint(pl);
                        objectId = ToModelSpace(pl);
                        lineString = jig.lineValue;

                        //计算角度
                        //var angle = GetAngle(pl.GetPoint2dAt(0), pl.GetPoint2dAt(1));

                        //SymbolLabel.AddSymbolLabel(ise, pl.StartPoint.X, pl.StartPoint.Y, angle);
                    }
                }
            }
            catch (Exception ex)
            {
                Editor.WriteMessage(ex.Message);
            }
            return objectId;
        }

        private void InsertPolyline(InsertSymbolEventArgs ise, PolylineJig plJig)
        {
            var pl = plJig.GetEntity() as Polyline;
            if (pl != null && pl.NumberOfVertices > 1)
            {
                DCadApi.AddPinchPoint(pl);
                DCadApi.AddLabelPinchPoint(pl);
                ObjectId objId = ToModelSpace(pl);
                ise.lineStringValue = plJig.lineValue;

                ExecutionManager.Instance.GetFactory(typeof(InsertToXmlExecutionFactory)).GetExecution(ise.g3e_fno).
                    Execute
                    (null, ise);

                AddDBSymbolFinder(objId, ise.insertobj);
                DCadApi.AddDBSymbolLTTIDFinder(ise.insertobj, pl.GeometricExtents, "", objId);

                //计算角度
                var angle = GetAngle(pl.GetPoint2dAt(0), pl.GetPoint2dAt(1));

                SymbolLabel.AddSymbolLabel(ise, pl.StartPoint.X, pl.StartPoint.Y, angle);
            }
        }

        /// <summary>
        /// 获取两点角度
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public double GetAngle(Point2d startPoint, Point2d endPoint)
        {
            var vec2 = endPoint - startPoint;
            return vec2.Angle;
        }

        #endregion

        #region 绘制矩形

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="layerId"></param>
        /// <param name="ise"></param>
        public void DrawRectangle(int fno, ObjectId layerId, InsertSymbolEventArgs ise)
        {
            try
            {
                var ppr =
                    Editor.GetPoint("\n指定第一个角点:");
                if (ppr.Status != PromptStatus.OK)
                    return;
                var sPoint = ppr.Value;
                ppr = Editor.GetCorner(
                    "\n指定另一个角点:",
                    ppr.Value
                    );
                if (ppr.Status != PromptStatus.OK)
                    return;


                var pl = new Polyline();
                pl.LayerId = layerId;
                pl.Color = ise.symbolColor;
                pl.Linetype = ise.lineTypeStr;
                var lineW = ise.lineWidth / MapConfig.Instance.earthscale;
                pl.AddVertexAt(0, new Point2d(sPoint.X, sPoint.Y), 0, lineW, lineW);
                pl.AddVertexAt(1, new Point2d(sPoint.X, ppr.Value.Y), 0, lineW, lineW);
                pl.AddVertexAt(2, new Point2d(ppr.Value.X, ppr.Value.Y), 0, lineW, lineW);
                pl.AddVertexAt(3, new Point2d(ppr.Value.X, sPoint.Y), 0, lineW, lineW);
                pl.AddVertexAt(4, new Point2d(sPoint.X, sPoint.Y), 0, lineW, lineW);
                //线段闭合
                pl.Closed = true;

                DCadApi.AddPinchPoint(pl);
                DCadApi.AddLabelPinchPoint(pl);
                var objId = ToModelSpace(pl);

                ise.polygonPointValue = ConvertGeometry.Instance.GetPolygon(objId);
                ExecutionManager.Instance.GetFactory(typeof(InsertToXmlExecutionFactory)).GetExecution(
                    ise.g3e_fno).Execute(null, ise);

                AddDBSymbolFinder(objId, ise.insertobj);
                DCadApi.AddDBSymbolLTTIDFinder(ise.insertobj, pl.GeometricExtents, ise.SBMC, objId);
                SymbolLabel.AddSymbolLabel(ise, Math.Min(sPoint.X, ppr.Value.X),
                                           Math.Max(sPoint.Y, ppr.Value.Y) + Math.Abs(sPoint.Y - ppr.Value.Y) / 4, 0);
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        public ObjectId DrawRectangle(ObjectId layerId, Color color, double lineWidth,ref Polygon polyGon)
        {
            var objectId = ObjectId.Null;
            try
            {
                var ppr =
                    Editor.GetPoint("\n指定第一个角点:");
                if (ppr.Status != PromptStatus.OK)
                    return objectId;
                var sPoint = ppr.Value;
                ppr = Editor.GetCorner(
                    "\n指定另一个角点:",
                    ppr.Value
                    );
                if (ppr.Status != PromptStatus.OK)
                    return objectId;


                var pl = new Polyline();
                pl.LayerId = layerId;
                pl.Color = color;
                pl.Linetype = "Continuous";
                var lineW = lineWidth;
                pl.AddVertexAt(0, new Point2d(sPoint.X, sPoint.Y), 0, lineW, lineW);
                pl.AddVertexAt(1, new Point2d(sPoint.X, ppr.Value.Y), 0, lineW, lineW);
                pl.AddVertexAt(2, new Point2d(ppr.Value.X, ppr.Value.Y), 0, lineW, lineW);
                pl.AddVertexAt(3, new Point2d(ppr.Value.X, sPoint.Y), 0, lineW, lineW);
                pl.AddVertexAt(4, new Point2d(sPoint.X, sPoint.Y), 0, lineW, lineW);


                DCadApi.AddPinchPoint(pl);
                DCadApi.AddLabelPinchPoint(pl);
                objectId = ToModelSpace(pl);

                polyGon = ConvertGeometry.Instance.GetPolygon(objectId);


                //SymbolLabel.AddSymbolLabel(ise, Math.Min(sPoint.X, ppr.Value.X),
                                           //Math.Max(sPoint.Y, ppr.Value.Y) + Math.Abs(sPoint.Y - ppr.Value.Y) / 4, 0);
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
            return objectId;
        }

        #endregion

        /// <summary>
        /// 是否存在点符号
        /// </summary>
        /// <param name="objectIds"></param>
        /// <param name="point">返回任意设备坐标</param>
        /// <returns></returns>
        public bool IsExistPointSymbol(IEnumerable<ObjectId> objectIds,ref Point3d point)
        {
            var result = false;
            foreach (var id in objectIds)
            {
                var entity = GetObject(id, OpenMode.ForRead) as Entity;
                if (entity is BlockReference)
                {
                    var block = entity as BlockReference;
                    point = block.Position;
                    result = true;
                    break;
                }
                if (entity is Polyline)
                {
                    var pl = entity as Polyline;
                    point = pl.StartPoint;
                    result = true;
                    break;
                }
            }
            return result;
        }
      

        public void ConfigError(object g3e_fno)
        {
            ShowMessage(string.Format("FNO:{0} 在配置文件中不存在！", g3e_fno));
        }

        /// <summary>
        /// 获取兄弟属性数据
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="className"> </param>
        public DBEntity GetBrotherTable(ObjectId objectId, string className)
        {
            DBEntity dbEntity = null;
            if (DBSymbolFinder.Instance.ContainsKey(objectId))
            {
                var values=DBSymbolFinder.Instance[objectId];
                var brotherValues = values.GetSiblings();
                if (brotherValues.Any())
                {
                    dbEntity = brotherValues.SingleOrDefault(o => o.GetType().Name == className);
                    if (dbEntity == null)
                    {
                        var g3e_fid = values.GetValue<long>("G3E_FID");
                        dbEntity = DBManager.Instance.GetEntity<Connectivity_n>(g3e_fid);
                    }
                }
            }
            return dbEntity;
        }
        //public void GetBrotherTable(Type type)
        //{
        //    //DBSymbolFinder.Instance.Where(o => o.Value.GetSiblings().Where(p => p.GetType() == type));
        //}

        /// <summary>
        /// 选择设备和标注
        /// </summary>
        public void SelectSymbolOrLabelEntity(ObjectId[] objectids)
        {
            //显示所有层
            SetLayerDisplay(false);
            var listObjID = GetSymbolOrLabel(objectids);
            SetImpliedSelection(listObjID);
        }

        /// <summary>
        /// 查找所有设备包括标注
        /// </summary>
        /// <param name="objectids">返回objectid集合</param>
        /// <returns></returns>
        public ObjectIdCollection GetSymbolOrLabel(IEnumerable<ObjectId> objectids)
        {
            var listObjID = new ObjectIdCollection();
            G3EObject g3eObject=null;
            foreach (var id in objectids)
            {
                var entity = GetObject(id, OpenMode.ForRead);
                //查找点、线、面设备的标注
                if (entity is BlockReference || entity is Polyline)
                {
                    if (DBEntityFinder.Instance.GetG3EIds(id, ref g3eObject))
                    {
                        var values = DBEntityFinder.Instance.GetDBSymbolFinderByFid(g3eObject.G3E_FID);
                        foreach (var item in values.Where(item => !listObjID.Contains(item.Key)))
                        {
                            listObjID.Add(item.Key);
                        }
                        //var zxbzs = DBEntityFinder.Instance.GetZxbzsByG3e_Fid(g3eObject.G3E_FID.ToString());
                        //if(zxbzs!=null)
                        //{
                        //    foreach (var zx in zxbzs)
                        //    {
                        //        values = DBEntityFinder.Instance.GetDBSymbolFinderByFid(zx.G3E_FID);
                        //        foreach (var item in values)
                        //        {
                        //            listObjID.Add(item.Key);
                        //        }
                        //    }
                        //}
                    }
                }else
                {
                    if (!listObjID.Contains(id))
                        listObjID.Add(id);
                }
            }
            return listObjID;
        }

        /// <summary>
        /// 表不存在的异常处理
        /// </summary>
        /// <param name="ex"></param>
        public void NotExistTable(NotExistException ex)
        {
            if (ex.Message.Contains("不存在"))
            {
                var strlx = ex.Message.Replace("不存在", "");
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), strlx);
                var connectn = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                DBManager.Instance.Insert(connectn);
            }
            else
            {
                Instance.AlertDialog(ex.Message);
            }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="g3e_fno"></param>
        /// <param name="g3e_fid"></param>
        /// <param name="message"></param>
        public void GetError(long g3e_fno,long g3e_fid,object message)
        {
            LogManager.Instance.Error(string.Format("\nFNO:{0}\nFID:{1}\n错误信息:{2}\n", g3e_fno, g3e_fid, message));
        }

        /// <summary>
        /// 根据FNO获取设备类型
        /// </summary>
        /// <param name="fno"></param>
        /// <returns></returns>
        public  string GetDeviceType(int fno)
        {
            var tables = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
            if (tables != null)
            {
                return tables.LayerName;
            }
            return null;
        }

        public Point3d GetDevPoint3d(ObjectId objId)
        {
            var p3d = new Point3d();
            try
            {
                var dbObj = GetObject(objId, OpenMode.ForRead);
                if (dbObj != null)
                {
                    if (dbObj is BlockReference)
                    {
                        var blkref = dbObj as BlockReference;
                        p3d = blkref.Position;
                    }
                }
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
            return p3d;
        }


        #region 工单验证、锁定

        private double[] pointions;
        /// <summary>
        /// 全景视图
        /// </summary>
        public void GetPanoramaView()
        {
            //定位到上次视图位置
            if (MapConfig.Instance.WindowsDefaultLocation.Any())
            {
                var scopes = MapConfig.Instance.WindowsDefaultLocation;
                Zoom(scopes[0], scopes[2], scopes[1], scopes[3]);
            }
            else
            {
                //获取全景视图
                DBEntityFinder.Instance.GetPanoramaView();
            }
            //获取工单视图
            pointions = DBEntityFinder.Instance.GetWorkorderView();
            //绘制工单锁定框
            if (pointions.Any())
                DrawRectangle(new Point2d(pointions[0], pointions[2]), new Point2d(pointions[1], pointions[3]));
        }
        /// <summary>
        /// 工单锁定范围定位
        /// </summary>
        public void Zoom()
        {
            if (pointions!=null && pointions.Any())
                SendCommend("zoom\n" + pointions[0] + "," + pointions[2] + "\n" + pointions[1] + "," + pointions[3] + "\n");
        }        
        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="maxX"></param>
        /// <param name="minX"></param>
        /// <param name="maxY"></param>
        /// <param name="minY"></param>
        public void Zoom(double maxX,double minX,double maxY,double minY)
        {
            if (minX > 0)
                SendCommend("zoom\n" + minX + "," + minY + "\n" + maxX + "," + maxY + "\n");
        }

        /// <summary>
        /// 绘制工单锁定图框
        /// </summary>
        /// <param name="minPoint"></param>
        /// <param name="maxpoint"></param>
        private void DrawRectangle(Point2d minPoint, Point2d maxpoint)
        {
            var lineW = 0.08 / MapConfig.Instance.earthscale;
            try
            {
                var layerId = DCadApi.addLayer("工单锁定框");
                var pl = new Polyline {LayerId = layerId, Color = Color.FromRgb(0, 255, 0), Linetype = "Continuous"};
                pl.AddVertexAt(0, new Point2d(minPoint.X, minPoint.Y), 0, lineW, lineW);
                pl.AddVertexAt(1, new Point2d(minPoint.X, maxpoint.Y), 0, lineW, lineW);
                pl.AddVertexAt(2, new Point2d(maxpoint.X, maxpoint.Y), 0, lineW, lineW);
                pl.AddVertexAt(3, new Point2d(maxpoint.X, minPoint.Y), 0, lineW, lineW);
                pl.AddVertexAt(4, new Point2d(minPoint.X, minPoint.Y), 0, lineW, lineW);
                Instance.lockFrameObjectID = ToModelSpace(pl);
                LockLayer(layerId, true);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        #endregion

        #region 暂时没用

        /// <summary>
        /// 实体联动高亮
        /// </summary>
        /// <param name="objectId"></param>
        public Dictionary<ObjectId, Color> EitityLinkageHighlight(ObjectId objectId)
        {
            var dic = new Dictionary<ObjectId, Color>();
            try
            {
                long g3e_id = 0, g3e_fid = 0, g3e_fno = 0;
                if(DBEntityFinder.Instance.GetG3EIds(objectId, ref g3e_id, ref g3e_fid, ref g3e_fno))
                {
                    var values =DBEntityFinder.Instance. GetDBSymbolFinderByFid(g3e_fid);
                    foreach (var item in values)
                    {
                        var oldColor = DCadApi.EditorPointSymbol(item.Key, Color.FromRgb(0, 255, 0));
                        dic.Add(item.Key, oldColor);
                    }
                }
            }
            catch (CADException ex)
            {
                LogManager.Instance.Error(ex);
            }
            return dic;
        }

        /// <summary>
        /// 取消高亮
        /// </summary>
        public void UnHighlightEntities(Dictionary<ObjectId, Color> dicEntities)
        {
            foreach (var item in dicEntities)
            {
                using (var tr = acDoc.TransactionManager.StartTransaction())
                {
                    var entity = tr.GetObject(item.Key, OpenMode.ForRead) as Entity;
                    SetLayerDisplay(entity.LayerId, false);
                }
                DCadApi.EditorPointSymbol(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 取消高亮
        /// </summary>
        public void UnHighlightEntities()
        {
            foreach (var item in Instance._dicEntities)
            {
                using (var tr = acDoc.TransactionManager.StartTransaction())
                {
                    var entity = tr.GetObject(item.Key, OpenMode.ForRead) as Entity;
                    SetLayerDisplay(entity.LayerId, false);
                }
                DCadApi.EditorPointSymbol(item.Key, item.Value);
            }
            UnHighlightDispose();
        }

        public void UnHighlightDispose()
        {
            Instance._dicEntities.Clear();
        }

        #endregion


        #region 线路连调

        /// <summary>
        /// 线路连调
        /// </summary>
        /// <returns></returns>
        public bool EditPolyLine()
        {
            var flag = false;
            try
            {
                var transaction = acDoc.TransactionManager.StartTransaction();
                var id = GetEntity("\n请选择线段或多段线");
                DCadApi.isModifySymbol = true;
                using (transaction)
                {
                    if (id.IsNull) return false;
                    var entity = transaction.GetObject(id, OpenMode.ForRead) as Entity;
                    entity.UpgradeOpenAndRun();
                    var point = GetPoint("请选择位置\n");
                    if (entity is Polyline)
                    {
                        var lineValue = new LineString();
                        using (acDoc.LockDocument())
                        {
                            var polyline = entity as Polyline;
                            var num = 0.02 / MapConfig.Instance.earthscale;
                            var polyline2 = new Polyline();
                            polyline2.AddVertexAt(0, new Point2d(polyline.StartPoint.X, polyline.StartPoint.Y), 0.0, num,
                                                  num);
                            polyline2.AddVertexAt(1, new Point2d(polyline.StartPoint.X, point.Y), 0.0, num, num);
                            if (point.X > polyline.EndPoint.X)
                            {
                                polyline2.AddVertexAt(2, new Point2d(polyline.EndPoint.X, point.Y), 0.0, num, num);
                            }
                            else
                            {
                                polyline2.AddVertexAt(2,
                                                      point.X < polyline.StartPoint.X
                                                          ? new Point2d(polyline.StartPoint.X, point.Y)
                                                          : new Point2d(point.X, point.Y), 0.0, num, num);
                            }
                            polyline2.AddVertexAt(3, new Point2d(polyline.EndPoint.X, point.Y), 0.0, num, num);
                            polyline2.AddVertexAt(4, new Point2d(polyline.EndPoint.X, polyline.EndPoint.Y), 0.0, num,
                                                  num);
                            polyline2.Linetype = polyline.Linetype;
                            polyline2.LinetypeScale = polyline.LinetypeScale;
                            polyline2.Layer = polyline.Layer;
                            polyline2.Color = polyline.Color;
                            var objectid = ToModelSpace(polyline2);
                            lineValue = ConvertGeometry.Instance.GetLineString(objectid);
                            if (DBSymbolFinder.Instance.ContainsKey(polyline.ObjectId))
                            {
                                var value = DBSymbolFinder.Instance[polyline.ObjectId];
                                value.G3E_GEOMETRY = lineValue;
                                DBManager.Instance.Update(value);
                                AddDBSymbolFinder(objectid, value);
                                DBSymbolFinder.Instance.Remove(polyline.ObjectId);
                            }
                            polyline.Erase();
                        }
                        transaction.Commit();
                        EditPolyLine();
                        flag = true;
                    }
                }
            }
            catch (CADException ex)
            {
                Editor.WriteMessage(ex.Message);
                flag = false;
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
            return flag;
        }

        #endregion

        #region 线路调整

        [CommandMethod("adjustpolyline")]
        public bool AdjustPolyLine()
        {
            bool flag2;
            try
            {
                var id = GetEntity("\n请选择线段或多段线");
                if (id.IsNull) return false;
                DCadApi.isModifySymbol = true;

                var entity = GetObject(id, OpenMode.ForRead, WorkingDataBase) as Entity;
                entity.UpgradeOpenAndRun();
                if ((entity is Line || entity is Polyline))
                {
                    Editor.WriteMessage("\n选择错误！请选择线段或多段线");
                    AdjustPolyLine();
                }
                var point = GetPoint("请选择位置");

                if (entity is Polyline)
                {
                    var polyline2 = entity as Polyline;
                    var num3 = 0.02 / MapConfig.Instance.earthscale;
                    var pointd2 = new Point3d(polyline2.StartPoint.X, polyline2.StartPoint.Y,
                                              polyline2.StartPoint.Z);
                    var pointd3 = new Point3d(polyline2.EndPoint.X, polyline2.EndPoint.Y, polyline2.EndPoint.Z);
                    var line = new Line(pointd2, pointd3);
                    var line2 = new Line(polyline2.StartPoint, point);
                    double num = line.Area;
                    double num2 = line2.Area;
                    var polyline = new Polyline();
                    if (num2 < num)
                    {
                        num2 += 6.2831853071795862;
                    }
                    if (((line.StartPoint.X < line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)) ||
                        ((line.StartPoint.X > line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)))
                    {
                        if ((num2 - num) <= 3.1415926535897931)
                        {
                            if (((line.StartPoint.X < line.EndPoint.X) && (line.StartPoint.Y > line.EndPoint.Y)) ||
                                ((line.StartPoint.X < line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)))
                            {
                                polyline.AddVertexAt(0, new Point2d(line.StartPoint.X, line.StartPoint.Y), 0.0, num3,
                                                     num3);
                                polyline.AddVertexAt(1, new Point2d(line.StartPoint.X, line.EndPoint.Y), 0.0, num3,
                                                     num3);
                                polyline.AddVertexAt(2, new Point2d(line.EndPoint.X, line.EndPoint.Y), 0.0, num3,
                                                     num3);
                            }
                            if (((line.StartPoint.X > line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)) ||
                                ((line.StartPoint.X > line.EndPoint.X) && (line.StartPoint.Y > line.EndPoint.Y)))
                            {
                                polyline.AddVertexAt(0, new Point2d(line.StartPoint.X, line.StartPoint.Y), 0.0, num3,
                                                     num3);
                                polyline.AddVertexAt(1, new Point2d(line.EndPoint.X, line.StartPoint.Y), 0.0, num3,
                                                     num3);
                                polyline.AddVertexAt(2, new Point2d(line.EndPoint.X, line.EndPoint.Y), 0.0, num3,
                                                     num3);
                            }
                        }
                        else
                        {
                            if (((line.StartPoint.X < line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)) ||
                                ((line.StartPoint.X < line.EndPoint.X) && (line.StartPoint.Y > line.EndPoint.Y)))
                            {
                                polyline.AddVertexAt(0, new Point2d(line.StartPoint.X, line.StartPoint.Y), 0.0, num3,
                                                     num3);
                                polyline.AddVertexAt(1, new Point2d(line.EndPoint.X, line.StartPoint.Y), 0.0, num3,
                                                     num3);
                                polyline.AddVertexAt(2, new Point2d(line.EndPoint.X, line.EndPoint.Y), 0.0, num3,
                                                     num3);
                            }
                            if (((line.StartPoint.X > line.EndPoint.X) && (line.StartPoint.Y > line.EndPoint.Y)) ||
                                ((line.StartPoint.X > line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)))
                            {
                                polyline.AddVertexAt(0, new Point2d(line.StartPoint.X, line.StartPoint.Y), 0.0, num3,
                                                     num3);
                                polyline.AddVertexAt(1, new Point2d(line.StartPoint.X, line.EndPoint.Y), 0.0, num3,
                                                     num3);
                                polyline.AddVertexAt(2, new Point2d(line.EndPoint.X, line.EndPoint.Y), 0.0, num3,
                                                     num3);
                            }
                        }
                    }
                    else if ((num2 - num) >= 3.1415926535897931)
                    {
                        if (((line.StartPoint.X < line.EndPoint.X) && (line.StartPoint.Y > line.EndPoint.Y)) ||
                            ((line.StartPoint.X < line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)))
                        {
                            polyline.AddVertexAt(0, new Point2d(line.StartPoint.X, line.StartPoint.Y), 0.0, num3,
                                                 num3);
                            polyline.AddVertexAt(1, new Point2d(line.StartPoint.X, line.EndPoint.Y), 0.0, num3, num3);
                            polyline.AddVertexAt(2, new Point2d(line.EndPoint.X, line.EndPoint.Y), 0.0, num3, num3);
                        }
                        if (((line.StartPoint.X > line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)) ||
                            ((line.StartPoint.X > line.EndPoint.X) && (line.StartPoint.Y > line.EndPoint.Y)))
                        {
                            polyline.AddVertexAt(0, new Point2d(line.StartPoint.X, line.StartPoint.Y), 0.0, num3,
                                                 num3);
                            polyline.AddVertexAt(1, new Point2d(line.EndPoint.X, line.StartPoint.Y), 0.0, num3, num3);
                            polyline.AddVertexAt(2, new Point2d(line.EndPoint.X, line.EndPoint.Y), 0.0, num3, num3);
                        }
                    }
                    else
                    {
                        if (((line.StartPoint.X < line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)) ||
                            ((line.StartPoint.X < line.EndPoint.X) && (line.StartPoint.Y > line.EndPoint.Y)))
                        {
                            polyline.AddVertexAt(0, new Point2d(line.StartPoint.X, line.StartPoint.Y), 0.0, num3,
                                                 num3);
                            polyline.AddVertexAt(1, new Point2d(line.EndPoint.X, line.StartPoint.Y), 0.0, num3, num3);
                            polyline.AddVertexAt(2, new Point2d(line.EndPoint.X, line.EndPoint.Y), 0.0, num3, num3);
                        }
                        if (((line.StartPoint.X > line.EndPoint.X) && (line.StartPoint.Y > line.EndPoint.Y)) ||
                            ((line.StartPoint.X > line.EndPoint.X) && (line.StartPoint.Y < line.EndPoint.Y)))
                        {
                            polyline.AddVertexAt(0, new Point2d(line.StartPoint.X, line.StartPoint.Y), 0.0, num3,
                                                 num3);
                            polyline.AddVertexAt(1, new Point2d(line.StartPoint.X, line.EndPoint.Y), 0.0, num3, num3);
                            polyline.AddVertexAt(2, new Point2d(line.EndPoint.X, line.EndPoint.Y), 0.0, num3, num3);
                        }
                    }
                    polyline.Linetype = polyline2.Linetype;
                    polyline.LinetypeScale = polyline2.LinetypeScale;
                    polyline.Layer = polyline2.Layer;
                    polyline.Color = polyline2.Color;
                    var objectId = ToModelSpace(polyline);
                    var lineValue = ConvertGeometry.Instance.GetLineString(objectId);
                    if (DBSymbolFinder.Instance.ContainsKey(polyline2.ObjectId))
                    {
                        var value = DBSymbolFinder.Instance[polyline2.ObjectId];
                        value.G3E_GEOMETRY = lineValue;
                        DBManager.Instance.Update(value);
                        AddDBSymbolFinder(objectId, value);
                        DBSymbolFinder.Instance.Remove(polyline2.ObjectId);
                    }
                    polyline2.Erase();
                    Editor.WriteMessage(line.Area.ToString());
                }
                flag2 = true;
            }
            catch (Autodesk.AutoCAD.Runtime.Exception)
            {
                flag2 = false;
            }
            finally
            {
                DCadApi.isModifySymbol = false;
            }
            return flag2;
        }

        #endregion
        #region 算法

        private double min(double x, double y)
        {
            if (x > y) return y;
            return x;
        }

        private double max(double x, double y)
        {
            if (x > y) return x;
            return y;
        }
        public double PointIsRangeOfPolygon(Point3d pt, Point3d[] ptPolygon)
        {
            var nCount = ptPolygon.Length;
            //记录是否在多边形上
            var isBeside = false;
            //多边行的外接矩形
            double maxx, maxy, minx, miny;
            if (nCount > 0)
            {
                maxx = ptPolygon[0].X;
                minx = ptPolygon[0].X;
                maxy = ptPolygon[0].Y;
                miny = ptPolygon[0].Y;
                for (int i = 0; i < nCount; i++)
                {
                    if (ptPolygon[i].X >= maxx)
                        maxx = ptPolygon[i].X;
                    else if (ptPolygon[i].X <= minx)
                        minx = ptPolygon[i].X;
                    if (ptPolygon[i].Y >= maxy)
                        maxy = ptPolygon[i].Y;
                    else if (ptPolygon[i].Y <= miny)
                        miny = ptPolygon[i].Y;

                }
                if (pt.X > maxx || pt.X < minx || pt.Y > maxy || pt.Y < miny)
                {
                    return -1;
                }
            }
            //射线法判断
            int nCross = 0;
            for (int i = 0; i < nCount ; i++)
            {
                var p1 = ptPolygon[i];
                var p2 = ptPolygon[(i + 1)%nCount];
                if (p1.Y == p2.Y)
                {
                    if (pt.Y == p1.Y && pt.X >= min(p1.X, p2.X) && (pt.X <= max(p1.X, p2.X)))
                    {
                        isBeside = true;
                        continue;
                    }
                }
                //焦点在p1p2延长线上
                if (pt.Y < min(p1.Y, p2.Y) || pt.Y > max(p1.Y, p2.Y))
                    continue;
                //求交点的X坐标
                var x = (double) (pt.Y - p1.Y)*(double) (p2.X - p1.X)/(double) (p2.Y - p1.Y) + p1.X;
                if (x>pt.X )
                {
                    nCross++;//只统一单边交点
                }
                else if (x==pt.X)
                {
                    isBeside = true;
                }

            }
            if (isBeside) return 0; //多边形上
            else if (nCross%2 == 1) return 1;//多边形内
            return -1;//多边形外
        }

        private Point3d[] GetDfVertex(long g3eFid)
        {
            var objId = DBEntityFinder.Instance.GetObjectIdByFid(g3eFid);
            var obj = GetObject(objId, OpenMode.ForRead);
            var ent = obj as Entity;
            var polyline = ent as Polyline;
            var vertexNum = polyline.NumberOfVertices;
            var pt = new Point3d[vertexNum];
            for (var i = 0; i < vertexNum ; i++)
            {
                pt[i] = polyline.GetPoint3dAt(i);
            }
            return pt;
        }

        public bool IsRangeOfDf(long sourceFid,long destFid)
        {
            var objId = DBEntityFinder.Instance.GetObjectIdByFid(sourceFid);
            var ent = GetObject(objId, OpenMode.ForRead);
            var pt = (ent as BlockReference).Position;
            var ptDf = GetDfVertex(destFid);
            var res = PointIsRangeOfPolygon(pt, ptDf);
            if (res > 0)
                return true;
            return false;
        }

        public bool IsPointFeature(long devFid)
        {
            var objId = DBEntityFinder.Instance.GetObjectIdByFid(devFid);
            if (objId.IsNull) return false;
            var ent = GetObject(objId, OpenMode.ForRead) as Entity;
            if (ent is BlockReference)
            {
                return true;
            }
            return false;
        }

        public bool IsLineFeature(long devFid)
        {
            var objId = DBEntityFinder.Instance.GetObjectIdByFid(devFid);
            if (objId.IsNull) return false;
            var ent = GetObject(objId, OpenMode.ForRead) as Entity;
            if (ent is Polyline)
            {
                return true;
            }
            return false;
        }
        #endregion
        /// <summary>
        /// 禁用命令
        /// </summary>
        public void Undefine()
        {
            SendCommend("UNDEFINE\n" + "COPYCLIP\n");
            SendCommend("UNDEFINE\n" + "COPYBASE\n");
            SendCommend("UNDEFINE\n" + "CUTCLIP\n");
            SendCommend("UNDEFINE\n" + "PASTECLIP\n");
            SendCommend("UNDEFINE\n" + "DDEDIT\n");
            SendCommend("UNDEFINE\n" + "_bedit\n");
            SendCommend("UNDEFINE\n" + "QSAVE\n");
            SendCommend("UNDEFINE\n" + "PROPERTIES\n");
            SendCommend("UNDEFINE\n" + "BREAK\n");
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        public void Submit()
        {
            DBManager.Instance.Submit();
        }
        public void SaveScreenXY(Vector3d wMax, Vector3d wMin)
        {
            try
            {
                var scope = new List<double> {wMax.X, wMax.Y, wMin.X, wMin.Y};
                MapConfig.Instance.WindowsDefaultLocation = scope;
                SaveClientMapConfig();
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        /// <summary>
        /// 保存客户端配置文件
        /// </summary>
        public bool SaveClientMapConfig()
        {
            try
            {
                var clientXmlPath = Path.GetDirectoryName(MapConfig.Instance.ClientXmlPath);
                //程序集物理路径
                if (clientXmlPath != null)
                {
                    var xmlPath = Path.Combine(clientXmlPath, "MapConfig.xml");
                    if (File.Exists(xmlPath))
                        XmlSerializeUtils.Save(xmlPath, MapConfig.Instance, new Type[] {});
                }
                return true;
            }catch(Exception ex )
            {
                LogManager.Instance.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 保存本地配置文件
        /// </summary>
        public bool SaveLocalMapConfig()
        {
            try
            {
                //程序集物理路径
                var LocalPluginPath = Assembly.GetExecutingAssembly().GetPhysicalDirectory();
                //程序集物理路径
                var xmlPath = Path.Combine(LocalPluginPath, "MapConfig.xml");
                if (File.Exists(xmlPath))
                    XmlSerializeUtils.Save(xmlPath, MapConfig.Instance, new Type[] { });
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return false;
            }
        }
        
        /// <summary>
        /// 获取点坐标
        /// </summary>
        /// <param name="es"></param>
        /// <returns></returns>
        public string GetGeometry(ElectronSymbol es)
        {
            var pointStr = string.Empty; ;
            if(es.G3E_GEOMETRY is Multipoint)
            {
                var geom = es.G3E_GEOMETRY as Multipoint;
                pointStr = string.Format("{0} {1}",geom.Points[0].X,geom.Points[0].Y);
            }
            return pointStr;
        }

        public bool Add_Detailreference_n(long g3e_id, long g3e_fid, long g3e_fno)
        {
            var df = new Detailreference_n();
            df.G3E_CID = 1;
            df.G3E_ID = g3e_id;
            df.G3E_CNO = 35;
            df.G3E_FID = g3e_fid;
            df.G3E_FNO = (int)g3e_fno;
            df.LTT_ID = MapConfig.Instance.LTTID;
            df.EntityState = EntityState.Insert;
            df.G3E_DETAILID = CYZCommonFunc.getid();
            df.DETAIL_USERNAME = g3e_fid.ToString();
            df.DETAIL_LEGENDNUMBER = 35;
            if (g3e_fno == 148)
                df.DETAIL_LEGENDNUMBER = 34;
            else if (g3e_fno == 159)
                df.DETAIL_LEGENDNUMBER = 6;
            df.DETAIL_MBRXLO = Convert.ToDecimal(112.356172100576);
            df.DETAIL_MBRYLO = Convert.ToDecimal(21.9600071382762);
            df.DETAIL_MBRXHIGH = Convert.ToDecimal(112.356316442584);
            df.DETAIL_MBRYHIGH = Convert.ToDecimal(21.9601062562456);
            df.DETAIL_MBRXOFFSET = 50000;
            df.DETAIL_MBRYOFFSET = 50000;
            return DBManager.Instance.Insert(df);
        }
    }
}