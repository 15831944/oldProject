using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferDal.Cad;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using ElectronTransferDal.FunctionHelper;
using ElectronTransferDal.Query;
using ElectronTransferFramework;
using System.IO;
using ElectronTransferModel.Geo;
using ElectronTransferModel.V9_4;

namespace ArxMap
{
    public class ObjectOperate
    {
        /// <summary>
        /// 是否回滚
        /// </summary>
        public static bool isRollback;

        #region 删除
        /// 删除校验
        public static void EraseVerity(ObjectId[] ImpliedSelectionIds)
        {
            var shb = new Dictionary<string, int>();
            if (ImpliedSelectionIds == null)
            {
                PublicMethod.Instance.Termination();
                return;
            }
            if (DBEntityFinder.Instance.VerifyLTTID(ImpliedSelectionIds, ref shb))
            {
                var strBuilder = GetErasePrompt(shb, ImpliedSelectionIds);
                if (MessageBox.Show(strBuilder.ToString(), "删除提示",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    DCadApi.isEraseRollback = false;
                    ImpliedSelectionIds = null;
                }
                else
                {
                    PublicMethod.Instance.Unappended();
                }
            }
            else
            {
                ShowMessage("当前选择设备未被锁定，不能对其操作！");
                PublicMethod.Instance.SendCommend(" ");
            }
        }
        public static bool EraseMirrorSource(ObjectId[] ImpliedSelectionIds)
        {
            var shb = new Dictionary<string, int>();
            if (ImpliedSelectionIds == null)
            {
                PublicMethod.Instance.Termination();
                return false;
            }
            if (DBEntityFinder.Instance.VerifyLTTID(ImpliedSelectionIds, ref shb))
            {
                DCadApi.isEraseRollback = false;
                ImpliedSelectionIds = null;
                return true;
            }
            ShowMessage("当前选择设备未被锁定，不能对其操作！");
            PublicMethod.Instance.SendCommend(" ");
            return false;
        }

        private static StringBuilder GetErasePrompt(ICollection<KeyValuePair<string, int>> shb, IEnumerable<ObjectId> ImpliedSelectionIds)
        {
            var strBuilder = new StringBuilder();
            if (shb.Count > 0)
            {
                var tooptip = string.Format("当前选择设备{0}个！\n", ImpliedSelectionIds.Count());
                strBuilder.Append(tooptip);
                if (shb.Count > 0)
                {
                    var str = string.Format("其中集中抄表箱{0}个\n", shb.Count);
                    strBuilder.Append(str);
                }
                foreach (var str in shb.Select(item => string.Format("集中抄表箱：{0} 户表：{1}个\n", item.Key, item.Value)))
                {
                    strBuilder.Append(str);
                }
                strBuilder.Append("确定要删除吗？");
            }
            else
            {
                strBuilder = GetErasePrompt(ImpliedSelectionIds, strBuilder);
            }
            return strBuilder;
        }
        /// <summary>
        /// 删除提示
        /// </summary>
        /// <param name="ImpliedSelectionIds"></param>
        /// <param name="strBuilder"></param>
        /// <returns></returns>
        private static StringBuilder GetErasePrompt(IEnumerable<ObjectId> ImpliedSelectionIds, StringBuilder strBuilder)
        {
            if(ImpliedSelectionIds.Count()==1)
            {
                var objectId = ImpliedSelectionIds.First();
                var pt = DBSymbolFinder.Instance[objectId];
                if(pt.G3E_GEOMETRY is Polygon)
                {
                    var com=DBEntityFinder.Instance.GetCommon_n(pt.G3E_FID);
                    if (com == null) return
                           strBuilder.Append(string.Format("当前选择设备{0}个\n确定要删除吗？", ImpliedSelectionIds.Count()));
                    var coms = DBEntityFinder.Instance.GetSymbolSubordinate(com.G3E_ID);
                    if(coms==null)
                    {
                        strBuilder.Append(string.Format("当前选择设备{0}个\n确定要删除吗？", ImpliedSelectionIds.Count()));
                        return strBuilder;
                    }
                    strBuilder.Append(coms.Any()
                                          ? string.Format("当前设备有从属关系\n确定要删除吗？")
                                          : string.Format("当前选择设备{0}个\n确定要删除吗？", ImpliedSelectionIds.Count()));
                }
                else
                {
                    switch (pt.G3E_FNO)
                    {
                        case 148:
                            {
                                var result = HasTJ(pt.G3E_FID);
                                strBuilder.Append(result
                                         ? string.Format("当前台架下有开关\n确定要删除吗？")
                                         : string.Format("当前选择设备{0}个\n确定要删除吗？", ImpliedSelectionIds.Count()));
                            }
                            break;
                        case 201:
                            {
                                var result = HasContains(pt.G3E_FID);
                                var coms = DBEntityFinder.Instance.GetSymbolSubordinate(pt.G3E_ID);
                                if (coms == null && !result)
                                    strBuilder.Append(string.Format("当前选择设备{0}个\n确定要删除吗？", ImpliedSelectionIds.Count()));

                                if (coms != null && result)
                                    strBuilder.Append("当前设备有从属关系和包含关系");
                                else
                                {
                                    if (coms != null)
                                    {
                                        strBuilder.Append(coms.Any()
                                                              ? string.Format("当前设备有从属关系")
                                                              : "");
                                    }
                                    strBuilder.Append(result
                                                          ? string.Format("\n当前设备有包含关系")
                                                          : "");
                                }
                                strBuilder.Append("\n确定要删除吗？");
                            }
                            break;
                        default:
                            strBuilder.Append(string.Format("当前选择设备{0}个\n确定要删除吗？", ImpliedSelectionIds.Count()));
                            break;
                    }
                }
            }else
            {
                strBuilder.Append(string.Format("当前选择设备{0}个\n确定要删除吗？", ImpliedSelectionIds.Count()));
            }
            return strBuilder;
        }
        /// <summary>
        /// 是否是台架
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        private static bool HasTJ(long g3e_fid)
        {
            try
            {
                var entity = DBEntityFinder.Instance.GetGnwzmc_n(g3e_fid);
                if (entity != null)
                {
                    if (!string.IsNullOrEmpty(entity.GNWZ_FL2))
                    {
                        if (entity.GNWZ_FL2.Equals("台架"))
                        {
                            var sstj = DBEntityFinder.Instance.GetSymbolSSTJ(entity.GNWZ_SSTJ);
                            if (sstj == null)
                                return false;
                            if (sstj.Count() > 1)
                                return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return false;
        }

        /// <summary>
        /// 是否有包含关系
        /// </summary>
        /// <param name="g3e_fid"></param>
        /// <returns></returns>
        private static bool HasContains(long g3e_fid)
        {
            try
            {
                var contains = DBEntityFinder.Instance.GetContains(g3e_fid);
                if (contains != null)
                {
                    if (contains.Count() == 1)
                    {
                        var contain = contains.First();
                        return contain.G3E_OWNERFID != 0;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
            return false;
        }

        public static void db_ObjectErased(ObjectErasedEventArgs e)
        {
            try
            {
                if (DBEntityFinder.Instance.HasG3EIDS(e.DBObject.Id))
                {
                    if (!DCadApi.isEraseRollback)
                    {
                        DBEntityErased.Instance.ObjectErased(e);
                        mouse._selectedObjectIds = null;
                        mouse.selectedEntityId = ObjectId.Null;
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }
        #endregion

        #region 修改
        private static void ShowMessage()
        {
            PublicMethod.Instance.ShowMessage("当前设备未锁定");
        }
        public static void GripStretchVerity(ObjectId objectId)
        {
            if (!DBEntityFinder.Instance.VerifyLTTID(objectId))
            {
                isRollback = true;
                DCadApi.isModifySymbol = true;
                ShowMessage();
                PublicMethod.Instance.SendCommend("x\n");
            }
            else
            {
                isRollback = false;
                DCadApi.isModifySymbol = false;
            }
        }
        /// <summary>
        /// 修改校验
        /// </summary>
        public static void UpdateVerity(bool flag, ObjectId[] selectedObjectIds)
        {
            if (DBEntityFinder.Instance.VerifyLTTID(selectedObjectIds))
            {
                isRollback = true;
                DCadApi.isModifySymbol = true;
                ShowMessage();
            }
            else
            {
                isRollback = false;
                DCadApi.isModifySymbol = false;
                if (flag)
                    PublicMethod.Instance.SelectSymbolOrLabelEntity(selectedObjectIds);
            }
        }
        public static void UpdateVerity(ObjectId objectId)
        {
            if (!DBEntityFinder.Instance.VerifyLTTID(objectId))
            {
                isRollback = true;
                DCadApi.isModifySymbol = true;
                ShowMessage();
                ux();
            }
            else
            {
                isRollback = false;
                DCadApi.isModifySymbol = false;
            }
        }

        public static void ShowMessage(string str)
        {
            PublicMethod.Instance.AlertDialog(str);
            PublicMethod.Instance.Unappended();
        }

        public static void db_ObjectModified(ObjectEventArgs e)
        {
            if (!DCadApi.isModifySymbol)
            {
                DBEntityModified.Instance.ObjectModified(e);
            }
            else
            {
                //回滚
                if (isRollback)
                {
                    isRollback = false;
                    ShowMessage("当前选择有未被锁定的设备，不能修改！");
                }
            }
        }
        private static void ux()
        {
            PublicMethod.Instance.SendCommend("u\n");
            PublicMethod.Instance.SendCommend("x\n");
        }
        #endregion

        #region 复制
        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="objectIds"></param>
        public static void CopyObjectDBEntity(ObjectId[] objectIds)
        {
            if (objectIds != null)
            {
                DBEntityCopy.Instance._copyObjects = DBEntityCopy.Instance.ObjectCopy(objectIds);
            }
        }
        public static void ObjectClone(IdMapping idMapping,ObjectId[] objectIds)
        {
            try
            {
                G3EIdMappingDispose();
                DCadApi.isModifySymbol = true;
                CopyObjectDBEntity(objectIds);
                //是否满足条件
                if (DBEntityCopy.Instance.isAccord)
                {
                    foreach (IdPair ip in idMapping)
                    {
                        var oldid = ip.Key;
                        var newid = ip.Value;

                        DBEntityCopy.Instance.ObjectPaste(oldid, newid);
                    }
                    if (DBEntityCopy.Instance._G3EIdMapping.Count > 0)
                    {
                        //更新连接关系和从属关系
                        DBEntityCopy.Instance.UpdateConnectionData();
                    }
                }
                else
                {
                    var errorInfo = DBEntityCopy.Instance.GetCopyErrorInfo();
                    if (!string.IsNullOrEmpty(errorInfo))
                    {
                        var eInfo = string.Format("数据缺失，不满足复制条件！Esc键取消！\n\n{0}", errorInfo);
                        PublicMethod.Instance.AlertDialog(eInfo);
                        PublicMethod.Instance.Unappended();
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(ex.Message);
            }
            finally { DCadApi.isModifySymbol = false;
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        private static void G3EIdMappingDispose()
        {
            DBEntityCopy.Instance._G3EIdMapping.Clear();
        }
        #endregion

        public static void IsLoadCADPlugin(string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);
            if (name.Contains("Drawing1"))
            {
                PublicMethod.Instance.SendCommend("az\n");
            }
            else
            {
                PublicMethod.Instance.SendCommend("xz\n");
            }
        }
    }
}
