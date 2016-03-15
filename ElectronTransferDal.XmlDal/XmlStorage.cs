using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElectronTransferFramework;
using ElectronTransferFramework.Serialize;
using ElectronTransferModel;
using ElectronTransferModel.Base;
using ElectronTransferModel.Config;

namespace ElectronTransferDal.XmlDal
{
    public static class XmlStorage
    {

        public static bool Save(this XmlDBManager manager, string path)
        {
            return manager.Save(path, string.Empty);
        }

        /// <summary>
        /// 保存为Xml文件
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="path">路径</param>
        /// <param name="password"> </param>
        /// <returns></returns>
        public static bool Save(this XmlDBManager manager, string path, string password)
        {
            try
            {
                manager.DB.
                    Save(path, password);
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return false;
                //throw;
            }
        }

        /// <summary>
        /// 保存增量为Xml文件
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool SaveVariable(this XmlDBManager manager, string path)
        {
            try
            {
                using (var newDb = XmlDB.Create(path))
                {
                    var version = MapConfig.Instance.Version;
                    version++;
                    foreach (var table in manager.DB.Tables)
                    {
                        var newTable = XmlTable.Create(table.Name);
                        var entities = table.Entities.Where(o => o.EntityState != EntityState.None);
                        if (entities.Any())
                        {
                            //更新版本号
                            entities.ToList().ForEach(o => o.Version = version);
                            newTable.AddRange(entities);
                            newDb.AddTable(newTable);
                        }
                    }
                    InsertCadVersion(version, newDb);
                    newDb.Save();
                    //保存版本号
                    SaveVersion(version);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="otherManager">另一个XmlDBManager</param>
        /// <returns></returns>
        public static bool Merge(this XmlDBManager manager, XmlDBManager otherManager)
        {
            try
            {
                foreach (var table in otherManager.DB.Tables)
                {
                    manager.InsertBulk(table.Entities);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return false;
            }
        }
        /// <summary>
        /// 合并增量数据
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="otherManager">另一个XmlDBManager的Insert状态数据</param>
        /// <returns></returns>
        public static List<DBEntity> MergeIncrement(this XmlDBManager manager, XmlDBManager otherManager)
        {
            var duplicateRecord = new List<DBEntity>();
            try
            {
                foreach (var table in otherManager.DB.Tables)
                {
                    //新增的数据
                    var entities = table.Entities.Where(o => o.EntityState != EntityState.None);
                    foreach (var entity in entities)
                    {
                        var g3e_fid = entity.GetValue<long>("G3E_FID");
                        if (manager.Has(entity.GetType()))
                        {
                            var dbentity = manager.GetEntity(entity.GetType(),
                                                             o => o.GetValue<long>("G3E_FID") == g3e_fid);
                            //判断实体是否已存在
                            if (dbentity == null)
                                manager.Insert(entity);
                            else
                            {
                                if (!duplicateRecord.Contains(entity))
                                    duplicateRecord.Add(entity);
                                manager.Update(entity);
                            }
                        }
                        else
                            manager.Insert(entity);
                    }
                    //更新update状态数据
                    var updateEntities = table.Entities.Where(o => o.EntityState == EntityState.Update);
                    foreach (var uentity in updateEntities)
                    {
                        manager.Update(uentity);
                        if (!duplicateRecord.Contains(uentity))
                            duplicateRecord.Add(uentity);
                    }
                }
                return duplicateRecord;
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
                return duplicateRecord;
            }
        }

        /// <summary>
        /// 保存版本号
        /// </summary>
        /// <param name="version"></param>
        private static void SaveVersion(int version)
        {
            try
            {
                MapConfig.Instance.Version = version;
                var clientXmlPath = Path.GetDirectoryName(MapConfig.Instance.ClientXmlPath);
                //程序集物理路径
                var xmlPath = Path.Combine(clientXmlPath, "MapConfig.xml");
                if (File.Exists(xmlPath))
                    XmlSerializeUtils.Save(xmlPath, MapConfig.Instance, new Type[] { });
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 创建CAD版本
        /// </summary>
        /// <param name="version"></param>
        /// <param name="xmlDB"> </param>
        private static void InsertCadVersion(int version, XmlDB xmlDB)
        {
            try
            {
                var cadVersion = new CadVersion
                {
                    G3E_ID = 1,
                    G3E_FID = 1,
                    G3E_FNO = 2014,
                    Version = version,
                    EntityState = EntityState.None
                };
                var versionTable = XmlTable.Create("CadVersion");
                versionTable.AddEntity(cadVersion);
                xmlDB.AddTable(versionTable);
            }
            catch
            {
                LogManager.Instance.Error("CADVersion添加失败！");
            }
        }
        public static IList<XmlTable> GetInerementTables(this XmlDBManager manager)
        {
            return manager.DB.Tables;
        }
    }
}
