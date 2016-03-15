using System;
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferFramework;

namespace ElectronTransferDal.Cad
{
    public class ObjectColorTool:Singleton<ObjectColorTool>
    {
        /// <summary>
        /// 高亮显示设备
        /// </summary>
        /// <param name="objectIds">设备ObjectId</param>
        /// <returns></returns>
        public Dictionary<ObjectId, int> GetObjectSetColor(List<ObjectId> objectIds)
        {
            var objectColorList = new Dictionary<ObjectId, int>();
            try
            {
                DCadApi.isModifySymbol = true;
                var db = HostApplicationServices.WorkingDatabase;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        foreach (var objectId in objectIds)
                        {
                            try
                            {
                                var entity = trans.GetObject(objectId, OpenMode.ForRead);
                                if (entity != null)
                                {
                                    var ent = entity as Entity;
                                    ent.UpgradeOpenAndRun();
                                    objectColorList.Add(ent.ObjectId, ent.ColorIndex);
                                    ent.ColorIndex = 4;
                                }
                            }
                            catch (Exception ex)
                            {
                                LogManager.Instance.Error(ex.Message);
                            }
                        }
                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
                PublicMethod.Instance.UpdateScreen();
            }
            return objectColorList;
        }

        /// <summary>
        /// 高亮显示设备
        /// </summary>
        /// <param name="objectId">设备ObjectId</param>
        /// <returns></returns>
        public Dictionary<ObjectId, int> GetObjectSetColor(ObjectId objectId)
        {
            var objectColorList = new Dictionary<ObjectId, int>();
            try
            {
                DCadApi.isModifySymbol = true;
                var db = HostApplicationServices.WorkingDatabase;

                using (var trans = db.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        var entity = trans.GetObject(objectId, OpenMode.ForRead);
                        if (entity != null)
                        {
                            var ent = entity as Entity;
                            ent.UpgradeOpenAndRun();
                            objectColorList.Add(ent.ObjectId, ent.ColorIndex);
                            ent.ColorIndex = 4;
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            finally
            {
                DCadApi.isModifySymbol = false;
                PublicMethod.Instance.UpdateScreen();
            }
            return objectColorList;
        }

        public void GetObjectSetColor(Dictionary<ObjectId, int> objColorList)
        {
            try
            {
                DCadApi.isModifySymbol = true;
                var db = HostApplicationServices.WorkingDatabase;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        foreach (var item in objColorList)
                        {
                            try
                            {
                                var entity = trans.GetObject(item.Key, OpenMode.ForRead);
                                if (entity != null)
                                {
                                    var ent = entity as Entity;
                                    ent.UpgradeOpenAndRun();
                                    ent.ColorIndex = item.Value;
                                }
                            }
                            catch (Autodesk.AutoCAD.Runtime.Exception)
                            {
                                //有可能当前设备已经被删除
                            }
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
                 PublicMethod.Instance.UpdateScreen();
            }
            finally
            {
                DCadApi.isModifySymbol = false;
               
            }
        }
        public int SetObjectColor(ObjectId objectId,int colorIndex)
        {
            var index = 0;
            try
            {
                var db = HostApplicationServices.WorkingDatabase;
                using (var trans = db.TransactionManager.StartTransaction())
                {
                    using (PublicMethod.Instance.acDoc.LockDocument())
                    {
                        var entity = trans.GetObject(objectId, OpenMode.ForRead);
                        if (entity != null)
                        {
                            var ent = entity as Entity;
                            index = ent.ColorIndex;
                            ent.UpgradeOpenAndRun();
                            ent.ColorIndex = colorIndex;
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
                PublicMethod.Instance.UpdateScreen();
            }
            return index;
        }
    }
}
