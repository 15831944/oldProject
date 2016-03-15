using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Common;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;

namespace ElectronTransferDal.FunctionHelper
{
    public class DBEntityErased:Singleton<DBEntityErased>
    {
        /// <summary>
        /// 删除集抄箱时的批量删除事件
        /// </summary>
        /// <param name="fid">集抄箱fid</param>
        /// <param name="backupDBEntity">备份实体</param>
        /// <returns></returns>
        public delegate List<DBEntity> BatchDeleteFromJCXDelegate(long fid, List<DBEntity> backupDBEntity);
        public event BatchDeleteFromJCXDelegate BatchDeleteFromJCXEvent;

        /// <summary>
        /// 删除集抄箱时的批量删除事件
        /// </summary>
        /// <param name="fid">集抄箱fid</param>
        /// <param name="backupDBEntity">备份实体</param>
        /// <returns></returns>
        public delegate List<DBEntity> BatchDeleteFromJLBDelegate(long fid, List<DBEntity> backupDBEntity);
        public event BatchDeleteFromJLBDelegate BatchDeleteFromJLBEvent;


        public void ObjectErased(ObjectErasedEventArgs e)
        {
            try
            {
                var objectId = e.DBObject.Id;
                var isErased = e.Erased;
                G3EObject g3eObject = null;
                if (DBEntityFinder.Instance.GetG3EIds(objectId, ref g3eObject))
                {
                    var dbSymbolEntry = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == g3eObject.G3E_FNO);
                    if (dbSymbolEntry != null)
                    {
                        if (isErased)
                        {
                            //存储备份实体
                            var backupEntity = new List<DBEntity>();
                            if (e.DBObject is DBText || e.DBObject is MText)
                            {
                                //删除标注坐标数据
                                backupEntity = ErasedPointSymbolTable(objectId, backupEntity);
                                //杂项标注属性删除
                                if (g3eObject.G3E_FNO == 250)
                                    backupEntity = EraseZxbzDBEntity(g3eObject.G3E_FID, backupEntity);
                                //删除标注lb
                                backupEntity = ErasedLabelLB(objectId, g3eObject, backupEntity);
                                ObjectHistoryManagerAdd(objectId, backupEntity);
                            }
                            else
                            {
                                //删除设备数据
                                backupEntity = ObjectErased(dbSymbolEntry, objectId, g3eObject, backupEntity);
                                ObjectHistoryManagerAdd(objectId, backupEntity);

                                //删除标注
                                DeleteLinkageLabel(g3eObject.G3E_FID);
                            }
                        }
                        else
                        {
                            Redo(objectId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error("删除设备错误！\n"+ex);
            }
        }
        /// <summary>
        /// 添加备份实体
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="backupEntity"></param>
        private void ObjectHistoryManagerAdd(ObjectId objectId, List<DBEntity> backupEntity)
        {
            if (!ObjectHistoryManager.Instance.ContainsKey(objectId))
                ObjectHistoryManager.Instance.Add(objectId, backupEntity);
        }
        /// <summary>
        /// 恢复数据
        /// </summary>
        /// <param name="objectId"></param>
        private void Redo(ObjectId objectId)
        {
            if (ObjectHistoryManager.Instance.ContainsKey(objectId))
            {
                var historyValue = ObjectHistoryManager.Instance[objectId];
                foreach (var item in historyValue)
                {
                    //恢复从属关系
                    //if (item.ErasedOWNER1_ID != null)
                    //    item.SetValue("OWNER1_ID", item.ErasedOWNER1_ID);

                    //恢复状态
                    item.IsErased = false;
                    if (item.RedoState)
                        DBManager.Instance.Update(item);
                    else
                        DBManager.Instance.Insert(item);

                    //更新缓存
                    if (item.IsCoordinate)
                    {
                        PublicMethod.Instance.UpdateDBSymbolFinder(objectId, item as ElectronSymbol);
                    }
                }
                ObjectHistoryManager.Instance.Remove(objectId);
            }
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        /// <param name="dbSymbolEntry">设备对象</param>
        /// <param name="objectId"> </param>
        /// <param name="g3eObject"> </param>
        /// <param name="backupEntity"> </param>
        public List<DBEntity> ObjectErased(DBSymbolEntry dbSymbolEntry, ObjectId objectId,G3EObject g3eObject, List<DBEntity> backupEntity)
        {
            //删除其他数据（开关柜、台架）
            backupEntity = ErasedOtherDBEntity(g3eObject, backupEntity);
            //删除坐标表
            if (!string.IsNullOrEmpty(dbSymbolEntry.SymbolPtTable))
            {
                //删除设备坐标
                backupEntity = ErasedPointSymbolTable(objectId, backupEntity);

                //更新连接关系
                backupEntity = UpdateConnectionData(g3eObject.G3E_FID, backupEntity);
            }
            //删除自身属性
            if (!string.IsNullOrEmpty(dbSymbolEntry.ComponentTable.SelfAttribute))
            {
                backupEntity = ErasedBrotherTable(dbSymbolEntry.ComponentTable.SelfAttribute.Trim(), g3eObject.G3E_FID, backupEntity);
            }
            //删除公共属性
            if (!string.IsNullOrEmpty(dbSymbolEntry.ComponentTable.Common))
            {
                backupEntity = ErasedBrotherTable(dbSymbolEntry.ComponentTable.Common.Trim(), g3eObject.G3E_FID, backupEntity);
            }
            //删除连接关系
            if (!string.IsNullOrEmpty(dbSymbolEntry.ComponentTable.Connectivity))
            {
                backupEntity = ErasedConnectionTable(g3eObject.G3E_FID, backupEntity);
            }
            //删除功能位置
            if (!string.IsNullOrEmpty(dbSymbolEntry.ComponentTable.Gnwz))
            {
                backupEntity = ErasedBrotherTable(dbSymbolEntry.ComponentTable.Gnwz.Trim(), g3eObject.G3E_FID, backupEntity);
            }
            //删除包含关系
            if (!string.IsNullOrEmpty(dbSymbolEntry.ComponentTable.Contain))
            {
                backupEntity = ErasedContainTable(g3eObject.G3E_FID, backupEntity);
            }
            //删除详表
            if (!string.IsNullOrEmpty(dbSymbolEntry.ComponentTable.Detailreference))
            {
                if (g3eObject.G3E_FNO == 159)
                {
                    //删除户表数据
                    backupEntity = BatchDeleteFromJCXEvent(g3eObject.G3E_FID, backupEntity);
                }
                else if (g3eObject.G3E_FNO == 148 || g3eObject.G3E_FNO == 84)
                {
                    //删除详图数据
                    backupEntity = BatchDeleteFromJLBEvent(g3eObject.G3E_FID, backupEntity);
                }
                backupEntity = ErasedBrotherTable(dbSymbolEntry.ComponentTable.Detailreference.Trim(), g3eObject.G3E_FID, backupEntity);
            }
            //受电馈线
            if (!string.IsNullOrEmpty(dbSymbolEntry.ComponentTable.Gg_Pd_Sdkx_Ac))
            {
                backupEntity = ErasedSdkx_Ac(g3eObject.G3E_FID, backupEntity);
            }
            //删除杂项标注图形
            EraseZxbzGraph(g3eObject.G3E_FID);
            return backupEntity;
        }

        /// <summary>
        /// 删除其他数据（开关柜、台架）
        /// </summary>
        /// <param name="g3eObject"> </param>
        /// <param name="backupEntity"></param>
        /// <returns></returns>
        private List<DBEntity> ErasedOtherDBEntity(G3EObject g3eObject, List<DBEntity> backupEntity)
        {
            if (g3eObject.G3E_FNO == 142 || g3eObject.G3E_FNO == 149 || g3eObject.G3E_FNO == 163)
            {
                //删除开关柜
                backupEntity = ErasedKggDBEntity(g3eObject.G3E_FID, backupEntity);
            }
            if (g3eObject.G3E_FNO == 148)
            {
                //删除台架数据
                backupEntity = ErasedTjDBEntity(g3eObject.G3E_FID, backupEntity);
            }
            return backupEntity;
        }


        /// <summary>
        /// 删除设备坐标表数据
        /// </summary>
        /// <param name="objectId"> </param>
        /// <param name="backupEntity">备份实体</param>
        private List<DBEntity> ErasedPointSymbolTable(ObjectId objectId, List<DBEntity> backupEntity)
        {
            try
            {
                //获取缓存里的坐标表
                var symbolValue = PublicMethod.Instance.GetDBSymbolFinder(objectId) as ElectronSymbol;
                if (symbolValue != null)
                {
                    symbolValue.IsCoordinate = true;
                    symbolValue.IsErased = true;
                    switch (symbolValue.EntityState)
                    {
                        case EntityState.Insert:
                            symbolValue.RedoState = false;
                            backupEntity.Add(symbolValue.Clone() as DBEntity);
                            DBManager.Instance.Delete(symbolValue);
                            PublicMethod.Instance.UpdateDBSymbolFinder(objectId, symbolValue);
                            break;
                        case EntityState.Update:
                        case EntityState.None:
                            symbolValue.RedoState = true;
                            backupEntity.Add(symbolValue.Clone() as DBEntity);
                            symbolValue.EntityState = EntityState.Delete;
                            DBManager.Instance.Update(symbolValue);
                            break;
                    }
                    if (symbolValue.G3E_GEOMETRY is Polygon)
                    {
                        //删除是电房的话就需要更新从属于它的设备的OWNER1_ID，改为0
                        backupEntity =UpdateCommonDBEntity(symbolValue.G3E_ID,backupEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return backupEntity;
        }


        #region 删除杂项标注数据
        /// <summary>
        /// 删除杂项标注图形
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        private void EraseZxbzGraph(long g3e_fid)
        {
            //try
            //{
            //    var zxbzn = DBEntityFinder.Instance.GetZxbzsByG3e_Fid(g3e_fid.ToString());
            //    if (zxbzn != null)
            //    {
            //        IList<long> zxfids = zxbzn.Select(item => item.G3E_FID).ToList();
            //        foreach(var zxfid in zxfids)
            //        {
            //            EraseZxbzObj(zxfid);
            //        }
            //    }
            //}catch(Exception ex)
            //{
            //    LogManager.Instance.Error("删除杂项标注错误！\n" + ex);
            //}
        }
        private List<DBEntity> EraseZxbzDBEntity(long g3e_fid, List<DBEntity> backupEntity)
        {
            //删除杂项标注自身属性表数据
            var zxbzn = DBManager.Instance.GetEntity<Gg_gl_zxbz_n>(g3e_fid);
            backupEntity = GetEntityStateByErasedDBEntity(zxbzn, backupEntity);
            //删除杂项标注表数据
            var zxbzlb = DBManager.Instance.GetEntity<Gg_gl_zxbz_lb>(g3e_fid);
            backupEntity = GetEntityStateByErasedDBEntity(zxbzlb, backupEntity);

            ////删除杂项标注坐标表数据
            //var zxbzlbsdo = DBManager.Instance.GetEntity<Gg_gl_zxbz_lb_sdogeom>(g3e_fid);
            //backupEntity = GetEntityStateByErasedDBEntity(zxbzlbsdo, backupEntity);

            return backupEntity;
        }

        private void EraseZxbzObj(long zxbzfid)
        {
            try
            {
                //删掉图形
                var finderValue = DBEntityFinder.Instance.GetDBSymbolFinderByFidOrEntityType(zxbzfid, EntityType.ZxLabel);
                var items = new Dictionary<ObjectId, ElectronSymbol>();

                foreach (var fv in finderValue)
                {
                    items.Add(fv.Key, fv.Value);
                }

                foreach (var item in items)
                {
                    //获取标注CAD对象
                    var entity = PublicMethod.Instance.GetObject(item.Key, OpenMode.ForRead) as Entity;
                    //如果标注已删则退出
                    if (!entity.IsErased)
                    {
                        //打开指定图层
                        PublicMethod.Instance.SetLayerDisplay(entity.LayerId, false);
                        //删除标注对象
                        PublicMethod.Instance.EraseObject(item.Key);
                    }
                }
            }catch(Exception ex)
            {
                LogManager.Instance.Error("删除杂项标注图形错误！\n"+ex);
            }
        }
        #endregion

        #region 删除台架数据
        /// <summary>
        /// 删除台架数据
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="backupEntity"> </param>
        public List<DBEntity> ErasedTjDBEntity(long g3e_fid, List<DBEntity> backupEntity)
        {
            var entity = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(g3e_fid);
            if (entity != null)
            {
                if (!string.IsNullOrEmpty(entity.GNWZ_FL2))
                {
                    if (entity.GNWZ_FL2.Equals("台架"))
                    {
                        backupEntity = DeleteTJDataUndo(entity.GNWZ_SSTJ, backupEntity);
                    }
                }
            }
            return backupEntity;
        }
        private List<DBEntity> DeleteTJDataUndo(string gnwz_sstj, List<DBEntity> backupEntity)
        {
            try
            {
                //公共属性
                var comTJ =DBManager.Instance.GetEntity<Common_n>(o => o.G3E_FNO == 199 && o.G3E_FID.ToString().Equals(gnwz_sstj));
                if (comTJ != null)
                {
                    backupEntity = GetEntityStateByErasedDBEntity(comTJ, backupEntity);
                    //自身属性
                    var self = DBManager.Instance.GetEntity<Gg_gz_tj_n>(comTJ.G3E_FID);
                    backupEntity = GetEntityStateByErasedDBEntity(self, backupEntity);
                    //功能位置
                    var gnwz = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(comTJ.G3E_FID);
                    backupEntity = GetEntityStateByErasedDBEntity(gnwz, backupEntity);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return backupEntity;
        }
        #endregion

        #region 删除开关柜数据
        /// <summary>
        /// 删除开关柜数据
        /// </summary>
        /// <param name="g3e_fid">电房g3e_id</param>
        /// <param name="backupEntity"> </param>
        public List<DBEntity> ErasedKggDBEntity(long g3e_fid, List<DBEntity> backupEntity)
        {
            //查询电房Common_n表的G3E_ID
            var dfCom = DBEntityFinder.Instance.GetCommon_n(g3e_fid);
            if (dfCom != null)
            {
                //根据电房g3e_id查询开关柜
                var kggCom = DBManager.Instance.GetEntities<Common_n>(o => o.OWNER1_ID == dfCom.G3E_ID);
                if (kggCom.Any())
                {
                    IList<Common_n> items = kggCom.ToList();
                    foreach (var item in items)
                    {
                        if (item == null) continue;
                        if (item.G3E_FNO != 198) continue;
                        backupEntity = GetEntityStateByErasedDBEntity(item, backupEntity);
                        //自身属性
                        var n = DBManager.Instance.GetEntity<Gg_pd_kgg_n>(item.G3E_FID);
                        backupEntity = GetEntityStateByErasedDBEntity(n, backupEntity);
                        //功能位置
                        var gnwz = DBManager.Instance.GetEntity<Gg_pd_gnwzmc_n>(item.G3E_FID);
                        backupEntity = GetEntityStateByErasedDBEntity(gnwz, backupEntity);

                        //清空所属开关柜属性字段
                        backupEntity = UpdateGnwzDBEntity(item.G3E_FID, backupEntity);
                    }
                }
            }
            return backupEntity;
        }

        #endregion

        #region 处理关系
        /// <summary>
        /// 删除设备属性数据
        /// </summary>
        /// <param name="className">实体类名</param>
        /// <param name="g3e_fid">g3e_fid</param>
        /// <param name="backupEntity">备份实体</param>
        private List<DBEntity> ErasedBrotherTable(string className, long g3e_fid, List<DBEntity> backupEntity)
        {
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), className);
                //获取要删除的数据
                var value = DBManager.Instance.GetEntity(type, g3e_fid);

                if (value != null)
                {
                    //Insert状态的直接删除掉，反之更新状态
                    backupEntity = GetEntityStateByErasedDBEntity(value, backupEntity);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return backupEntity;
        }

        /// <summary>
        /// 删除电房的时候更新从属关系
        /// </summary>
        /// <param name="g3e_id"></param>
        /// <param name="backupEntity"> </param>
        private List<DBEntity> UpdateCommonDBEntity(long g3e_id,List<DBEntity> backupEntity)
        {
            try
            {
                var commonValue = DBManager.Instance.GetEntities<Common_n>(o => o.OWNER1_ID == g3e_id);
                foreach (var item in commonValue)
                {
                    if (item != null)
                    {
                        if (item.G3E_FNO != 198)
                        {
                            if (item.OWNER1_ID != 0)
                            {
                                var comClone = item.Clone() as DBEntity;
                                comClone.RedoState = true;
                                backupEntity.Add(comClone);
                                item.ErasedOWNER1_ID = item.OWNER1_ID;
                                item.OWNER1_ID = 0;
                                DBManager.Instance.Update(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return backupEntity;
        }

        /// <summary>
        /// 删除电房的时候更新从属关系
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="backupEntity"> </param>
        private List<DBEntity> UpdateGnwzDBEntity(long g3e_fid, List<DBEntity> backupEntity)
        {
            try
            {
                var gnwzs = DBManager.Instance.GetEntities<Gg_pd_gnwzmc_n>(o => o.GNWZ_SSKGG == g3e_fid.ToString());
                if (gnwzs != null && gnwzs.Any())
                {
                    //if (!string.IsNullOrEmpty(gnwz_n.GNWZ_SSKGG))
                    //{
                    //    //备份
                    //    var gnwzClone = gnwz_n.Clone() as Gg_pd_gnwzmc_n;
                    //    gnwzClone.RedoState = true;
                    //    backupEntity.Add(gnwzClone);
                    //    //更新
                    //    gnwz_n.GNWZ_SSKGG = "";
                    //    DBManager.Instance.Update(gnwz_n);
                    //}

                    foreach (var item in gnwzs)
                    {  
                        //备份
                        var gnwzClone = item.Clone() as Gg_pd_gnwzmc_n;
                        gnwzClone.RedoState = true;
                        backupEntity.Add(gnwzClone);
                        //更新
                        item.GNWZ_SSKGG = null;
                        if (item.EntityState == EntityState.None)
                            item.EntityState = EntityState.Update;
                        DBManager.Instance.Update(item);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return backupEntity;
        }

        /// <summary>
        /// 删除设备检查该设备是否已经有连接关系，有则删除连接关系
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="backupEntity"> </param>
        private List<DBEntity> UpdateConnectionData(long g3e_fid, List<DBEntity> backupEntity)
        {
            var conn = DBManager.Instance.GetEntity<Connectivity_n>(g3e_fid);
            if (conn != null)
            {
                var node1 =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE1_ID == conn.NODE1_ID || o.NODE2_ID == conn.NODE1_ID).Where(
                            o => o.G3E_FID != g3e_fid);
                if (node1.Count() == 1)
                {
                    if (conn.NODE1_ID != 0)
                    {
                        if (node1.FirstOrDefault() != null)
                        {
                            var value = node1.FirstOrDefault();
                            value.RedoState = true;
                            backupEntity.Add(value.Clone() as DBEntity);
                        }
                        UpdateConnectionData(conn.NODE1_ID, node1.FirstOrDefault());
                    }
                }
                var node2 =
                    DBManager.Instance.GetEntities<Connectivity_n>(
                        o => o.NODE1_ID == conn.NODE2_ID || o.NODE2_ID == conn.NODE2_ID).Where(
                            o => o.G3E_FID != g3e_fid);
                if (node2.Count() == 1)
                {
                    if (conn.NODE2_ID != 0)
                    {
                        if (node2.FirstOrDefault() != null)
                        {
                            var value = node2.FirstOrDefault();
                            value.RedoState = true;
                            backupEntity.Add(value.Clone() as DBEntity);
                        }
                        UpdateConnectionData(conn.NODE2_ID, node2.FirstOrDefault());
                    }
                }
            }
            return backupEntity;
        }

        private void UpdateConnectionData(long? NODE_ID, Connectivity_n newConn)
        {
            try
            {
                if (NODE_ID == 0) return;
                var entityState = newConn.EntityState.ToString();
                //站房母线不更改单个节点
                if (newConn.G3E_FNO == 143) return;
                if (newConn.NODE1_ID == NODE_ID)
                {
                    newConn.NODE1_ID = 0;
                    if (newConn.EntityState == EntityState.None || newConn.EntityState == EntityState.Update)
                    {
                        newConn.EntityState = EntityState.Old_Del_Old;
                    }
                    else
                    {
                        var strEntityState = entityState.Remove(4, 3).Insert(4, "Del");
                        newConn.EntityState =
                            (EntityState)Enum.Parse(typeof(EntityState), strEntityState);
                    }
                }
                else
                {
                    newConn.NODE2_ID = 0;
                    if (newConn.EntityState == EntityState.None || newConn.EntityState == EntityState.Update)
                    {
                        newConn.EntityState = EntityState.Old_Old_Del;
                    }
                    else
                    {
                        var strEntityState = entityState.Remove(8, 3).Insert(8, "Del");
                        newConn.EntityState =
                            (EntityState)Enum.Parse(typeof(EntityState), strEntityState);
                    }
                }
                DBManager.Instance.Update(newConn);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        #endregion

        /// <summary>
        /// 删除设备的标注数据
        /// </summary>
        /// <param name="g3e_fid"></param>
        private void DeleteLinkageLabel(long g3e_fid)
        {
            try
            {
                //从缓存里获取标注数据
                var finderValue = DBEntityFinder.Instance.GetDBSymbolFinderByFidOrEntityType(g3e_fid, EntityType.Label);
                var items = finderValue.ToDictionary(fv => fv.Key, fv => fv.Value);
                foreach (var item in items)
                {
                    //获取标注CAD对象
                    var entity = PublicMethod.Instance.GetObject(item.Key, OpenMode.ForRead) as Entity;
                    //如果标注已删则退出
                    if (!entity.IsErased)
                    {
                        //打开指定图层
                        PublicMethod.Instance.SetLayerDisplay(entity.LayerId, false);
                        //删除标注对象
                        PublicMethod.Instance.EraseObject(item.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error("删除标注错误！\n" + ex);
            }
        }
        private List<DBEntity> ErasedLabelLB(ObjectId objectId,G3EObject g3eObject, List<DBEntity> backupEntity)
        {
            //获取缓存里的坐标表
            var symbolValue = PublicMethod.Instance.GetDBSymbolFinder(objectId) as ElectronSymbol;
            var className = symbolValue.GetType().Name.Replace("_sdogeom", "");
            backupEntity=ErasedBrotherTable(className, g3eObject.G3E_FID, backupEntity);
            return backupEntity;
        }

        /// <summary>
        /// 删除设备连接关系表数据
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="backupEntity">实体备份</param>
        private List<DBEntity> ErasedConnectionTable(long g3e_fid, List<DBEntity> backupEntity)
        {
            try
            {
                var dbEntity = DBManager.Instance.GetEntity<Connectivity_n>(g3e_fid) as ElectronBase;
                backupEntity = ErasedConnectionTable(dbEntity, backupEntity);
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return backupEntity;
        }

        private List<DBEntity> ErasedConnectionTable(DBEntity dbEntity, List<DBEntity> backupEntity)
        {
            if (dbEntity != null)
            {
                //处理连接关系
                var entityState = dbEntity.EntityState.ToString().Split('_');
                backupEntity = entityState[0] == "Add" ? DeleteDBEntity(dbEntity, backupEntity) : UpdateDBEntity(dbEntity, backupEntity);
            }
            return backupEntity;
        }

        /// <summary>
        /// 删除包含关系表
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="backupEntity"> </param>
        private List<DBEntity> ErasedContainTable(long g3e_fid, List<DBEntity> backupEntity)
        {
            try
            {
                var contains = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_FID == g3e_fid);
                IList<Contain_n> items = contains.ToList();
                foreach (var contain in items)
                {
                    backupEntity = GetEntityStateByErasedDBEntity(contain, backupEntity);
                    backupEntity = ErasedContain(g3e_fid, backupEntity);
                }

            }catch(Exception exception)
            {
                LogManager.Instance.Info(string.Format("删除包含关系出错！G3E_FID:{0}{1}", g3e_fid,exception));
            }
            return backupEntity;
        }

        /// <summary>
        /// 删除跟它有包含关系的从属关系表
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="backupEntity">备份实体</param>
        private List<DBEntity> ErasedContain(long g3e_fid, List<DBEntity> backupEntity)
        {
            try
            {
                var finderValue = DBManager.Instance.GetEntities<Contain_n>(o => o.G3E_OWNERFID == g3e_fid);
                IList<Contain_n> items = finderValue.ToList();
                backupEntity = items.Select(value => value.Clone() as Contain_n).Aggregate(backupEntity, (current, contain) => GetEntityStateByErasedDBEntity(contain, current));
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return backupEntity;
        }
       
        /// <summary>
        /// 删除受电馈线
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <param name="backupEntity"></param>
        /// <returns></returns>
        private List<DBEntity> ErasedSdkx_Ac(long g3e_fid,List<DBEntity> backupEntity)
        {
            try
            {
                var sdkxs =DBEntityFinder.Instance.GetSdkxAcByG3e_FID(g3e_fid);
                if (sdkxs != null)
                {
                    IList<DBEntity> items = sdkxs.Cast<DBEntity>().ToList();
                    backupEntity = items.Select(value => value.Clone() as Gg_pd_sdkx_ac).Aggregate(backupEntity, (current, sdkx) => GetEntityStateByErasedDBEntity(sdkx, current));
                }
            }catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return backupEntity;
        }


        /// <summary>
        /// 删除新增数据
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="backupEntity"></param>
        /// <returns></returns>
        private List<DBEntity> DeleteDBEntity(DBEntity dbEntity, List<DBEntity> backupEntity)
        {
            //false标识Insert状态数据
            dbEntity.RedoState = false;
            dbEntity.IsErased = true;
            //克隆一份保存起来
            backupEntity.Add(dbEntity.Clone() as DBEntity);
            //删掉原始数据
            DBManager.Instance.Delete(dbEntity);
            return backupEntity;
        }
        /// <summary>
        /// 删除原始数据（实为更新数据状态）
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="backupEntity"></param>
        /// <returns></returns>
        private List<DBEntity> UpdateDBEntity(DBEntity dbEntity, List<DBEntity> backupEntity)
        {
            //true标识为原始数据
            dbEntity.RedoState = true;
            dbEntity.IsErased = true;
            //克隆
            backupEntity.Add(dbEntity.Clone() as DBEntity);
            //改变状态
            dbEntity.EntityState = EntityState.Delete;
            DBManager.Instance.Update(dbEntity);
            return backupEntity;
        }
        /// <summary>
        /// 根据状态删除实体
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <param name="backupEntity"></param>
        /// <returns></returns>
        public List<DBEntity> GetEntityStateByErasedDBEntity(DBEntity dbEntity, List<DBEntity> backupEntity)
        {
            if (dbEntity != null)
            {
                switch (dbEntity.EntityState)
                {
                    case EntityState.Insert:
                        backupEntity = DeleteDBEntity(dbEntity, backupEntity);
                        break;
                    case EntityState.Update:
                    case EntityState.None:
                        backupEntity = UpdateDBEntity(dbEntity, backupEntity);
                        break;
                    default:
                        backupEntity = ErasedConnectionTable(dbEntity, backupEntity);
                        break;
                }
            }
            return backupEntity;
        }
    }
}
