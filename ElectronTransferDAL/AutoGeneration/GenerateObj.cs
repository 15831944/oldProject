using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using ElectronTransferFramework;
using System.ComponentModel;
using ElectronTransferModel.Config;
using ElectronTransferDal.Common;
using ElectronTransferModel.Base;
using System.Reflection;

namespace ElectronTransferDal.AutoGeneration
{
    public class GenerateObj:Singleton<GenerateObj>
    {
        private Dictionary<string, TypeConverter> dict = new Dictionary<string, TypeConverter>();
        public GenerateObj()
        {
            addDict();
        }
        /// <summary>
        /// 所有属性下拉列表表名和对应的值
        /// </summary>
        private void addDict()
        {
            #region
            //Dictionary<string, TypeConverter> tempDict = new Dictionary<string, TypeConverter>();
            //tempDict.Add("Cd_lb", new JcdlbList());
            //tempDict.Add("Cd_ptgfl", new PtList());
            //tempDict.Add("Cd_jlblx", new JlbList());
            //tempDict.Add("Cd_zclx", new ZclxList());
            //tempDict.Add("Cd_jddzys", new JddzysList());
            //tempDict.Add("Cd_sfdqkx", new SfdygdList());
            //tempDict.Add("Cd_eddl", new EddlList());
            //tempDict.Add("Cd_pblb", new PblbList());
            //tempDict.Add("Cd_sblx_jlg", new JlgList());
            //tempDict.Add("Cd_zsbyqfl", new ZsbyqList());
            //tempDict.Add("Cd_dbqy", new DbqyList());
            //tempDict.Add("Cd_sfczd", new SfczdList());
            //tempDict.Add("Cd_mhxx", new MhfsList());
            //tempDict.Add("Cd_gzzt", new GzztList());
            //tempDict.Add("Cd_xs", new XsList());
            //tempDict.Add("Cd_cbx_lx", new CbxList());
            //tempDict.Add("Cd_zsdlqlx", new ZsdlqList());
            //tempDict.Add("Cd_dldlyb", new DlybList());
            //tempDict.Add("Cd_sbpj", new SbpjList());
            //tempDict.Add("Cd_ydlb", new YdlbList());
            //tempDict.Add("Cd_xhge", new XhggList());
            //tempDict.Add("Cd_smzq", new SmzqList());
            //tempDict.Add("Cd_dydj", new DydjList());
            //tempDict.Add("Cd_dqtz", new DqtzList());
            //tempDict.Add("gnwz_fl2", new AzwzList());
            //tempDict.Add("Cd_cllb", new CllbList());
            //tempDict.Add("Cd_txfs", new TxfsList());
            //tempDict.Add("Cd_sblx_ttu", new PwyList());
            //tempDict.Add("Cd_dflx", new DfList());
            //tempDict.Add("Cd_jmj", new ZyJmjList());
            //tempDict.Add("Cd_bdcxfl", new BdcxflList());
            //tempDict.Add("Cd_dlfxxfl", new DlfxxflList());
            //tempDict.Add("Cd_cxd", new CxdList());
            //tempDict.Add("Cd_sfsdlt", new DltflList());
            //tempDict.Add("gnwz_fl", new FlList());
            //tempDict.Add("Cd_dykgfl", new DykgflList());
            //tempDict.Add("Cd_jdlx", new JdlxList());
            //tempDict.Add("Cd_yfznms", new SfznmsList());
            //tempDict.Add("Cd_xlfl", new XlflList());
            //tempDict.Add("Cd_cqsx", new CqsxList());
            //tempDict.Add("Cd_sfsdl", new SfdfhczList());
            //tempDict.Add("Cd_sfdd", new SfddList());
            //tempDict.Add("Cd_dqzt", new DqztList());
            //tempDict.Add("Cd_ssfl", new GnssflList());
            //tempDict.Add("Cd_sfdcjg", new SfdcjgList());
            //tempDict.Add("Cd_sfdyrdq", new SfdrdqList());
            //tempDict.Add("Cd_sfzcx", new SfzcxList());
            //tempDict.Add("Cd_ggjsqk", new TgjsqkList());
            //tempDict.Add("Cd_whgs", new WhgsList());
            //tempDict.Add("Cd_bs", new SsgdsList());
            //foreach (var item in tempDict)
            //{
            //    dict.Add(item.Key.ToUpper(), item.Value);
            //}
            #endregion

            #region 都通过通用方法来匹配
            //dict.Add("Cd_lb", new JcdlbList());
            //dict.Add("Cd_ptgfl", new PtList());
            //dict.Add("Cd_jlblx", new JlbList());
            //dict.Add("Cd_zclx", new ZclxList());
            //dict.Add("Cd_jddzys", new JddzysList());
            //dict.Add("Cd_sfdqkx", new SfdygdList());
            //dict.Add("Cd_pblb", new PblbList());
            //dict.Add("Cd_sblx_jlg", new JlgList());
            //dict.Add("Cd_zsbyqfl", new ZsbyqList());
            //dict.Add("Cd_dbqy", new DbqyList());
            //dict.Add("Cd_sfczd", new SfczdList());
            //dict.Add("Cd_mhxx", new MhfsList());
            //dict.Add("Cd_gzzt", new GzztList());
            //dict.Add("Cd_xs", new XsList());
            //dict.Add("Cd_cbx_lx", new CbxList());
            //dict.Add("Cd_zsdlqlx", new ZsdlqList());
            //dict.Add("Cd_dldlyb", new DlybList());
            //dict.Add("Cd_sbpj", new SbpjList());
            //dict.Add("Cd_ydlb", new YdlbList());
            //dict.Add("Cd_smzq", new SmzqList());
            //dict.Add("Cd_dydj", new DydjList());
            //dict.Add("Cd_dqtz", new DqtzList());
            //dict.Add("Cd_cllb", new CllbList());
            //dict.Add("Cd_txfs", new TxfsList());
            //dict.Add("Cd_sblx_ttu", new PwyList());
            //dict.Add("Cd_dflx", new DfList());
            //dict.Add("Cd_jmj", new ZyJmjList());
            //dict.Add("Cd_bdcxfl", new BdcxflList());
            //dict.Add("Cd_dlfxxfl", new DlfxxflList());
            //dict.Add("Cd_cxd", new CxdList());
            //dict.Add("Cd_sfsdlt", new DltflList());
            //dict.Add("Cd_dykgfl", new DykgflList());
            //dict.Add("Cd_jdlx", new JdlxList());
            //dict.Add("Cd_yfznms", new SfznmsList());
            //dict.Add("Cd_xlfl", new XlflList());
            //dict.Add("Cd_cqsx", new CqsxList());
            //dict.Add("Cd_sfsdl", new SfdfhczList());
            //dict.Add("Cd_sfdd", new SfddList());
            //dict.Add("Cd_dqzt", new DqztList());
            //dict.Add("Cd_ssfl", new GnssflList());
            //dict.Add("Cd_sfdcjg", new SfdcjgList());
            //dict.Add("Cd_sfdyrdq", new SfdrdqList());
            //dict.Add("Cd_sfzcx", new SfzcxList());
            //dict.Add("Cd_ggjsqk", new TgjsqkList());
            //dict.Add("Cd_whgs", new WhgsList());
            #endregion


            //dict.Add("Cd_xhge", new XhggList());
            //dict.Add("gnwz_fl2", new AzwzList());
            //dict.Add("gnwz_fl", new FlList());
            dict.Add("Cd_eddl", new EddlList());
            dict.Add("Cd_bs", new SsgdsList());
     
        }
        /// <summary>
        /// 根据设备FNO生成绑定对象
        /// </summary>
        /// <param name="fno">设备fno</param>
        /// <returns></returns>
        public XProps GenderObjByFno(int fno)
        {
            XProps xps = new XProps();
            var entity = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
            if (entity == null)
            {
                return null;
            }
            if (fno == 0)
            {
                GenderObjByTable(entity.Common, fno, xps, true);
                GenderObjByTable(entity.Common, fno, xps, false);
            }
            else
            {
                GenderObjByBrowsable(entity, fno, xps, true);
                GenderObjByBrowsable(entity, fno, xps, false);
            }
         
            return xps;
        }
        /// <summary>
        /// 根据浏览属性生成对象
        /// </summary>
        /// <param name="entitytables"></param>
        /// <param name="entity"></param>
        /// <param name="fno"></param>
        /// <param name="xps"></param>
        /// <param name="flag"></param>
        private void GenderObjByBrowsable(SimpleAttrEntry entity, int fno, XProps xps, bool flag)
        {
            GenderObjByTable(entity.Common, fno, xps, flag);
            GenderObjByTable(entity.SelfAttribute, fno, xps, flag);
            GenderObjByTable(entity.Connectivity, fno, xps, flag);
            GenderObjByTable(entity.Gnwz, fno, xps, flag);
        }
        /// <summary>
        /// 根据表名填充绑定对象的值
        /// </summary>
        /// <param name="tableName">字段对应的表名</param>
        /// <param name="fieldCollection">字段集合</param>
        /// <param name="fno">设备fno</param>
        /// <param name="xps">绑定对象</param>
        /// <param name="flag">是否是必填</param>
        private void GenderObjByTable(TableWithProperty tableWithProperty, int fno, XProps xps, bool flag)
        {
            if (tableWithProperty != null)
            {
                var fieldCollection = tableWithProperty.PropertiesFromTable;
                if (fieldCollection != null&&fieldCollection.Any())
                {
                    foreach (var item in fieldCollection)
                    {
                        if (item.Browsable == flag)
                        {
                            XProp xp = new XProp
                            {
                                Category = tableWithProperty.TableName,
                                Name = item.Field,
                                DisplayName = flag ? "*" + item.DisplayName : item.DisplayName,
                                ReadOnly = item.ReadOnly,
                                ProType = GetFieldType(fno, tableWithProperty.TableName.Trim(), item.Field),
                                Converter = GetConverterByField(tableWithProperty.TableName, item.DropDownTable, item.Field, fno),
                                //Editor = GetUIEditor(tableName.Trim(), item.Field),
                                Value = GetDefaultValue(fno, tableWithProperty.TableName.Trim(), item),
                                Browsable = item.Browsable,
                                SaveValueByFid = item.SaveValueByFid,
                            };
                            xps.Add(xp);
                        }
                    }   
                }
            }
        }
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private object GetDefaultValue(int fno, string tableName, Property field)
        {
            object res=null;
            try
            {
                var type = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), tableName);
                PropertyInfo[] infos = type.GetProperties();
                var info = infos.SingleOrDefault(o => o.Name.Equals(field.Field));
                if (info != null)
                {
                    type = info.PropertyType;
                    if (!type.IsGenericType)
                    {
                        return field.DefaultValue;
                    }
                    Type temp = type.GetGenericTypeDefinition();
                    if (temp == typeof (Nullable<>))
                    {
                        type = Nullable.GetUnderlyingType(type);
                        if (type == typeof (DateTime) && !field.ReadOnly)
                            return null;
                        if (string.IsNullOrEmpty(field.DefaultValue))
                            return null;
                        //if (type == typeof(DateTime))
                        //    return null;
                        //if (string.IsNullOrEmpty(field.DefaultValue))
                        //    return null;


                        res = Convert.ChangeType(field.DefaultValue, type);
                    }

                }
                else
                {
                    LogManager.Instance.Error(string.Format("Fno={0}的设备,在表{1}中不存在{2}字段",fno,tableName,field.Field));
                }
            }
            catch(Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }

            return res;
            //throw new Exception("请查看DeviceAttributeConfig.xml中Fno=" + fno + "的设备属性是否对应正确");
        }

        /// <summary>
        /// 获取字段的类型
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private Type GetFieldType(int fno, string tableName, string fieldName)
        {
            Type type = null;
            try
            {
                type = TypeCache.Instance.GetTypeFromCache(typeof(DBEntity), tableName);
                //var obj = ReflectionUtils.CreateObject(new { }, type) as DBEntity;
                PropertyInfo[] infos = type.GetProperties();
                var info = infos.SingleOrDefault(o => o.Name.Equals(fieldName));
                if (info != null)
                {
                    type = info.PropertyType;
                    if (!type.IsGenericType)
                    {
                    }
                    else
                    {
                        Type temp = type.GetGenericTypeDefinition();
                        if (temp == typeof(Nullable<>))
                        {
                            var TempType = Nullable.GetUnderlyingType(type);
                            if (TempType == typeof(DateTime))
                                type = TempType;//只有日期才这样做
                        }
                    }
                    
                }
                else
                {
                    LogManager.Instance.Error(string.Format("Fno={0}的设备,在表{1}中不存在{2}字段", fno, tableName, fieldName));
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            return type;
        }

        /// <summary>
        /// 根据表名检索下拉列表
        /// </summary>
        /// <param name="CurrentTableName">当前字段所在的表名</param>
        /// <param name="tableName">表名</param>
        /// <param name="field">当前下拉列表的字段</param>
        /// <returns>下拉列表转换器</returns>
        private TypeConverter GetConverterByField(string CurrentTableName,string tableName, string field, int fno)
        {
            TypeConverter converter = null;
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    if (tableName.Equals("Cd_ssxl"))
                    {
                        if (field.Equals("CD_SSDW"))
                        {
                            converter = new SsgdjList();
                        }
                        if (field.Equals("CD_SSBDZ"))
                        {
                            converter = new SsbdzList();
                        }
                        if (field.Equals("CD_SSXL"))
                        {
                            converter = new SsxlList();
                        }
                    }
                    //else if (tableName.Equals("Cd_xhge"))
                    //{
                    //    XhggList.fno = fno;
                    //    converter= new XhggList();
                    //}
                    //else if (tableName.Equals("gnwz_fl2"))
                    //{
                    //    AzwzList.fno = fno;
                    //    converter= new AzwzList();
                    //}
                    //else if (tableName.Equals("gnwz_fl"))
                    //{
                    //    FlList.fno = fno;
                    //    converter= new FlList();
                    //}
                    else if (dict.Keys.Contains(tableName))
                    {
                        converter = dict[tableName];
                    }
                    else
                    {
                        converter = new DropDownItem(tableName, fno);
                    }

                }
                //if (CurrentTableName.Equals("Common_n")&&field.Equals("BZ"))
                //{
                //    converter = new BZList();
                //}
            }
            catch (Exception ex)
            {
                var assembly = Assembly.GetExecutingAssembly();
                LogManager.Instance.Error(ex.Message);
            }
            return converter;
        }

       
    }
   
}
