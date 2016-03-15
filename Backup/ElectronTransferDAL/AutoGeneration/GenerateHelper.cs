using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Xml.Linq;
using Autodesk.AutoCAD.Windows.Data;
using ElectronTransferDal.HelperEventArgs;
using ElectronTransferModel.Base;
using ElectronTransferFramework;
using ElectronTransferModel.V9_4;
using ElectronTransferModel.Config;
using ElectronTransferDal.Common;
using ElectronTransferDal.Cad;
using ElectronTransferDal.Factory;
using System.Reflection;
using ElectronTransferDal.Query;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using System.ComponentModel;
using ElectronTransferModel;

namespace ElectronTransferDal.AutoGeneration
{
    public static class GenerateHelper
    {
        #region  记录没有改之前设备的属性

        public static AllTablesFromFeature Atff = new AllTablesFromFeature();
        #endregion

        public static UserNameReplaceFidSpecialFeature Unrf = new UserNameReplaceFidSpecialFeature();

        /// <summary>
        /// 保存特殊设备选择
        /// </summary>
        public static ChoiceDev choiceDevice = new ChoiceDev();


        public static List<SpecialFilterOption> SpecialFilterOptions = GetAllSpecialFilterOptions();

        /// <summary>
        /// 获取所有含有特殊保存方式的设备集合（所谓的特殊保存方式就是有字段的值保存了其他设备的FID）
        /// </summary>
        /// <returns></returns>
        private static List<SpecialFilterOption> GetAllSpecialFilterOptions()
        {
            List<SpecialFilterOption> temp = new List<SpecialFilterOption>();
            int fnoTemp = 0;
            try
            {
                var fnos = DeviceAttributeConfig.Instance.Attributes.Select(o => o.Fno);
                foreach (int fno in fnos)
                {
                    if (fno == 0) continue;
                    fnoTemp = fno;
                    var xprops = GenerateObj.Instance.GenderObjByFno(fno);
                    if (xprops != null)
                    {
                        foreach (var xprop in xprops)
                        {
                            if (xprop.SaveValueByFid)
                            {
                                var specialFO = new SpecialFilterOption
                                {
                                    FNO = fno,
                                    TableName = xprop.Category,
                                    FromOtherFidOfFiledName = xprop.Name,
                                    DisplayName = xprop.DisplayName.Replace("*", ""),
                                    saveValuebyFid = xprop.SaveValueByFid,
                                    SpecialXProps = xprops
                                };
                                temp.Add(specialFO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.Instance.ShowMessage(
                    string.Format("SpecialFilterOption初始化异常，请检查DeviceAttributeConfig.xml中Fno={0}的节点设备", fnoTemp));
                LogManager.Instance.Error(ex.Message);
            }
            return temp;
        }
        public static string GetLayeraName(int fno)
        {
            string layerName = string.Empty;
            var ent = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
            if (ent != null)
            {
                layerName = ent.LayerName;
            }
            return layerName;
        }
        /// <summary>
        /// 获取特殊字段的显示名
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetSpecialFilterOptionsOfDisplayName()
        {
            return SpecialFilterOptions.Select(o => o.DisplayName).Distinct();
        }
        /// <summary>
        /// 根据FNO获取功能位置表
        /// </summary>
        /// <param name="fno"></param>
        /// <returns></returns>
         public static string GetGnwzTableNameByFno(int fno)
         {
             string tableName = string.Empty;
             var ent = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
            if (ent != null)
            {
                if (ent.Gnwz != null)
                {
                    tableName = ent.Gnwz.TableName;
                }
            }
            return tableName;
        }
        /// <summary>
        /// 根据FNO获取连接关系表
        /// </summary>
        /// <param name="fno"></param>
        /// <returns></returns>
         public static string GetConnTableNameByFno(int fno)
         {
             string tableName = string.Empty;
             var ent = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
             if (ent != null)
             {
                 if (ent.Connectivity != null)
                 {
                     tableName = ent.Connectivity.TableName;
                 }
             }
             return tableName;
         }
        /// <summary>
        /// 根据FNO获取自身属性表
        /// </summary>
        /// <param name="fno"></param>
        /// <returns></returns>
         public static string GetSelfTableNameByFno(int fno)
         {
             string tableName = string.Empty;
             var ent = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
             if (ent != null)
             {
                 if (ent.SelfAttribute != null)
                 {
                     tableName = ent.SelfAttribute.TableName;
                 }
             }
             return tableName;
         }
        /// <summary>
        /// 根据FNO获取公共属性表
        /// </summary>
        /// <param name="fno"></param>
        /// <returns></returns>
         public static string GetCommonTableNameByFno(int fno)
         {
             string tableName = string.Empty;
             var ent = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
             if (ent != null)
             {
                 if (ent.Common != null)
                 {
                     tableName = ent.Common.TableName;
                 }
             }
             return tableName;
         }
        /// <summary>
        /// 根据当前实体的Fid和Fno获取它的安装位置的值，不需要知道安装位置的具体字段名，只需在配置文件配置好即可
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fno"></param>
        /// <returns></returns>
        public static object GetCurrentEntityAzwzByFidAndFno(long fid, int fno)
        {
            object obj = null;
            var simpleEntry = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o=>o.Fno==fno);
            if (simpleEntry != null)
            {
                if (simpleEntry.InstallLocationOption != null)
                {
                    var azwzName = simpleEntry.InstallLocationOption.FieldName;
                    var tableName = simpleEntry.InstallLocationOption.TableName;
                    var entType = TypeCache.Instance.GetTypeFromCache(typeof (DBEntity), tableName.Trim());
                    if (entType != null)
                    {
                        var ent = DBManager.Instance.GetEntity(entType,fid);
                        if (ent != null)
                        {
                            obj = ent.GetValue(azwzName);
                        }
                    }
                }
            }
            return obj;
        }
        /// <summary>
        /// 是否都是默认值
        /// </summary>
        /// <param name="xprops"></param>
        /// <returns></returns>
        public static bool IsAllDefault(XProps xprops)
        {
            foreach (var item in xprops)
            {
                if (!IsDefault(item.ProType, item.Value))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 获取类型对应的是否是默认值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefault(Type type, object value)
        {
            var defaultValue = DefaultValueFactory.Instance.GetDefaultValue(type);
            if (defaultValue == null)
            {
                if (value == null)
                    return true;
                if (string.IsNullOrEmpty(value.ToString().Trim()))
                    return true;
            }
            return Equals(defaultValue, value);
        }
        /// <summary>
        /// 获取绑定对象的属性集合
        /// </summary>
        /// <param name="xprops"></param>
        /// <returns></returns>
        public static IEnumerable<XProp> GetXProp(XProps xprops)
        {
            List<XProp> xpropList = new List<XProp>();
            if (xprops == null) return xpropList;
            foreach (var item in xprops)
            {
                xpropList.Add(item);
            }
            return xpropList;
        }

        /// <summary>
        /// 获取绑定对象的所有属性字段
        /// </summary>
        /// <param name="xprops"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetPropertyNames(XProps xprops)
        {
            return xprops.Select(o => o.Name);
        }
        /// <summary>
        /// 判断指定表中指定的属性是否存在
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="tableName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool HasPropertyName(XProps xprops,string tableName, string propertyName)
        {
            if (xprops == null) return false;
            var res = GetPropertyNameFromTable(xprops, tableName);
            if (!res.Any(o => o.Equals(propertyName)))
                return false;
            return true;
        }
        /// <summary>
        /// 根据字段名返回该字段对应的表名和字段集合
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetXPropList(XProps xprops, string propertyName)
        {
            Dictionary<string ,string> TableAndField=new Dictionary<string, string>();
            if (xprops != null)
            {
                foreach (var item in xprops)
                {
                    if (item.Name.Equals(propertyName))
                    {
                        TableAndField.Add(item.Category,item.Name);
                    }
                }
            }
            return TableAndField;
        }

        public static XProp GetFirstXPropOfHasValue(XProps xprops, string propertyName)
        {
            var res = GetXPropList(xprops, propertyName);//有可能获取不同表的字段
            foreach (var item in res)
            {
                foreach (var xprop in xprops)
                {
                    if (xprop.Category.Equals(item.Key) && xprop.Name.Equals(item.Value))
                    {
                        if (xprop.Value != null)
                            return xprop;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取绑定对象的指定表中指定属性列
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="propertyName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static XProp GetSingleXProp(XProps xprops,string tableName, string propertyName)
        {
            if (xprops == null) return null;
            var res = GetPropertyNameFromTable(xprops, tableName);
            if (!res.Any()) return null;
            return xprops.SingleOrDefault(o =>o.Category.Equals(tableName)&&o.Name.Equals(propertyName));
        }

        /// <summary>
        /// 获取指定表中指定字段的属性值
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="propertyName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(XProps xprops,string tableName, string propertyName)
        {
            var res = GetSingleXProp(xprops,tableName, propertyName);
            if (res == null) return null;
            return res.Value;
        }

        /// <summary>
        /// 设置指定属性字段的值
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="tableName"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(XProps xprops,string tableName, string propertyName, object value)
        {
            var res = GetSingleXProp(xprops,tableName, propertyName);
            if (res != null)
                res.Value = value;
        }

        /// <summary>
        /// 设置全部属性的显示为flag
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="flag"></param>
        public static void SetAllBrowsable(XProps xprops,bool flag)
        {
            if (xprops != null)
            {
                foreach (var item in xprops)
                {
                    item.Browsable = flag;
                }
            }
        }
        /// <summary>
        /// 设置全部属性的只读属性为flag
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="flag"></param>
        public static void SetAllReadOnly(XProps xprops,bool flag)
        {
            if (xprops != null)
            {
                foreach (var xProp in xprops)
                {
                    xProp.ReadOnly = flag;
                }
            }
        }
        /// <summary>
        /// 设置某个属性的浏览属性
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="tableName"></param>
        /// <param name="propertyName"></param>
        /// <param name="flag"></param>
        public static void SetSingleBrowsable(XProps xprops,string tableName, string propertyName, bool flag)
        {
            var res = GetSingleXProp(xprops,tableName, propertyName);
            if (res != null)
            {
                res.Browsable = flag;
            }
        }

        /// <summary>
        /// 设置某个属性的显示状态属性
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="tableName"></param>
        /// <param name="propertyName"></param>
        /// <param name="flag"></param>
        public static void SetSingleReadOnly(XProps xprops,string tableName, string propertyName, bool flag)
        {
            var res = GetSingleXProp(xprops,tableName, propertyName);
            if (res != null)
            {
                res.ReadOnly = flag;
            }
        }

        /// <summary>
        /// 获取某个属性是否是只读
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="tableName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool IsReadOnly(XProps xprops,string tableName, string propertyName)
        {
            var res = GetSingleXProp(xprops,tableName, propertyName);
            if (res != null)
                return res.ReadOnly;
            return false;
        }
        /// <summary>
        /// 设置组属性的浏览属性
        /// </summary>
        /// <param name="xprops"> </param>
        /// <param name="propertyNameList">字典的键位字段名字，值为该字段对应的表名</param>
        /// <param name="flag"></param>
        public static void SetAllBrowsable(XProps xprops, Dictionary<string,string> propertyNameList, bool flag)
        {
            if (!propertyNameList.Any()) return;
            foreach (var item in propertyNameList)
            {
                SetSingleBrowsable(xprops, item.Value,item.Key, flag);
            }
        }
        /// <summary>
        /// 设置设备的所有显示属性
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="propertyNameList"></param>
        /// <param name="flag"></param>
        public static void SetAllReadOnly(XProps xprops, List<string> propertyNameList, bool flag)
        {
            if (!propertyNameList.Any()) return;
            foreach (var item in propertyNameList)
            {
                foreach (var xprop in xprops)
                {
                    if (xprop.Name.Equals(item))
                    {
                        xprop.ReadOnly = flag;
                    }
                }
            }
        }
        /// <summary>
        /// 查找该fno
        /// </summary>
        /// <param name="fno"></param>
        public static string QueryZXField(int fno)
        {
            string propertyName = string.Empty;
            var simpleAttrEntry = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
            if (simpleAttrEntry != null)
            {
                if (simpleAttrEntry.SSzxOption != null)
                {
                    propertyName = simpleAttrEntry.SSzxOption.FieldName;
                }
            }
            return propertyName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="showFieldList">要显示的字段集合</param>
        /// <param name="noShowFieldList">无需显示的字段集合</param>
        /// <param name="azwz"></param>
        /// <param name="isEmptyValue">是否清空功能位置设施分类</param>
        /// <param name="isEmptyValue">是否覆盖原来的功能位置设施分类的值</param>
        private static void GradeGenerateByAzwz(XProps xprops,List<SpecialFilterOption> showFieldList,List<SpecialFilterOption> noShowFieldList,object azwz,bool isCoverGnwzSSFLValue )
        {
            showFieldList.ForEach(o=>SetSingleBrowsable(xprops,o.TableName,o.FromOtherFidOfFiledName,true));
            noShowFieldList.ForEach(o=>SetSingleBrowsable(xprops,o.TableName,o.FromOtherFidOfFiledName,false));
            noShowFieldList.ForEach(o=>SetPropertyValue(xprops,o.TableName,o.FromOtherFidOfFiledName,null));
           
            //针对功能位置设施分类处理
            var isHasGnwzSsfl = HasPropertyName(xprops, "Gg_pd_gnwzmc_n", "GNWZ_SSFL");
            if (isHasGnwzSsfl)
            {
                if (isCoverGnwzSSFLValue)
                {
                    if (azwz==null||string.IsNullOrEmpty(azwz.ToString()))
                    {
                        SetPropertyValue(xprops, "Gg_pd_gnwzmc_n", "GNWZ_SSFL", null);
                    }
                    else
                    {
                        if (azwz.Equals("房内") || azwz.Equals("户内"))
                        {
                            SetPropertyValue(xprops, "Gg_pd_gnwzmc_n", "GNWZ_SSFL", "配电房设施");
                        }
                        else
                        {
                            SetPropertyValue(xprops, "Gg_pd_gnwzmc_n", "GNWZ_SSFL", "配电线设施");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取指定设备指定字段的显示名
        /// </summary>
        /// <param name="fno">设备FNO</param>
        /// <param name="propertyName">字段名</param>
        /// <returns></returns>
        public static string GetDisplayName(int fno, string propertyName)
        {
            string dispalyName = string.Empty;
            var xprops = GenerateObj.Instance.GenderObjByFno(fno);
            if (xprops != null)
            {
                var xprop=xprops.SingleOrDefault(o =>!string.IsNullOrEmpty(o.Name)&&o.Name.Equals(propertyName));
                if (xprop != null)
                {
                    dispalyName = xprop.DisplayName;
                }
            }
            return dispalyName;
        }
        private static List<AzwzOption> GetAzwzOptionByFno(int fno)
        {
            List<AzwzOption> ao = new List<AzwzOption>();
            var simpleEntry=DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
            if (simpleEntry != null)
            {
                if (simpleEntry.InstallLocationOption != null)
                {
                    if (simpleEntry.InstallLocationOption.AzwzCollection != null && simpleEntry.InstallLocationOption.AzwzCollection.Any())
                    {
                        ao.AddRange(simpleEntry.InstallLocationOption.AzwzCollection);
                    }
                }
            }
            return ao;
        }

        public static List<SpecialFilterOption> GetFieldsFromAzwzCollection(List<FieldCollection> collections)
        {
            List<SpecialFilterOption> coll = new List<SpecialFilterOption>();
            if (collections.Any())
            {
                collections.ForEach(
                    o =>
                        coll.Add(new SpecialFilterOption
                        {
                            FromOtherFidOfFiledName = o.FieldName,
                            TableName = o.TableName
                        }));
            }
            return coll;
        }
        /// <summary>
        /// 根据安装位置绑定不同的对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="azwz">安装位置</param>
        /// <param name="fno">设备FNO</param>
        /// <param name="flag">是否要覆盖原来的值</param>
        public static void GenerateByAzwz(ref XProps obj, object azwz, int fno,bool flag)
        {
            var res = GetAzwzOptionByFno(fno);
            if (res.Any())
            {
                if (fno == 148 && azwz != null && azwz.ToString().Equals("台架"))
                    SetPropertyValue(obj, GetGnwzTableNameByFno(148), "GNWZ_SSTJ", GetPropertyValue(obj, GetCommonTableNameByFno(148), "SBMC"));
                if (azwz == null || string.IsNullOrEmpty(azwz.ToString()))
                {
                    List<SpecialFilterOption> temp=new List<SpecialFilterOption>();
                   
                    foreach (var azwzOption in res)
                    {
                        var res1 = GetFieldsFromAzwzCollection(azwzOption.FilterFields);
                        var res2 = GetFieldsFromAzwzCollection(azwzOption.ShowFields);
                        if (temp.Any())
                        {
                            var res3 = res1.Concat(res2)
                                .Distinct(new SpecialFilterOptionCompare())
                                .OrderBy(o => o.FromOtherFidOfFiledName).ToList();
                            temp =
                                temp.Concat(res3)
                                    .Distinct(new SpecialFilterOptionCompare())
                                    .OrderBy(o => o.FromOtherFidOfFiledName)
                                    .ToList();

                        }
                        else
                        {
                            temp = res1.Concat(res2)
                               .Distinct(new SpecialFilterOptionCompare())
                               .OrderBy(o => o.FromOtherFidOfFiledName).ToList();
                        }
                       
                    }

                    GradeGenerateByAzwz(obj,new List<SpecialFilterOption>(),temp,azwz,flag );
                 }
                else
                {
                    var fields = res.SingleOrDefault(o => o.AzwzState.Trim().Equals(azwz.ToString()));
                    if (fields != null)
                    {

                        GradeGenerateByAzwz(obj, GetFieldsFromAzwzCollection(fields.ShowFields),
                            GetFieldsFromAzwzCollection(fields.FilterFields), azwz, flag);
                    }
                }

            }
            #region 优化前
            //switch (fno)
            //{
            //    #region

            //case 148:
            //{
            //    #region 148
            //    if (azwz == null || string.IsNullOrEmpty(azwz.ToString().Trim()))
            //    {
            //        GradeGenerateByAzwz(obj,new List<string>(),new List<string>{"GNWZ_SSTJ"},null,flag );
            //    }
            //    else if (azwz.Equals("台架"))
            //    {
            //        SetPropertyValue(obj, GetGnwzTableNameByFno(148), "GNWZ_SSTJ", GetPropertyValue(obj, GetCommonTableNameByFno(148), "SBMC"));
            //        GradeGenerateByAzwz(obj,new List<string>{"GNWZ_SSTJ"},new List<string>(),azwz.ToString(),flag );
            //    }
            //    else if (azwz.Equals("房内") || azwz.Equals("箱式"))
            //    {
            //        GradeGenerateByAzwz(obj,  new List<string>(), new List<string> { "GNWZ_SSTJ" }, azwz.ToString(), flag);
            //    }
            //    #endregion

            //}
            //    break;
            //case 146:
            //{
            //    #region 146
            //    if (azwz == null || string.IsNullOrEmpty(azwz.ToString().Trim()))
            //    {
            //        GradeGenerateByAzwz(obj,  new List<string>(), azwzColl.Keys.ToList(), null, flag);
            //    }
            //    else if (azwz.Equals("户外"))
            //    {
            //        GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSTJ" }, new List<string> { "GNWZ_XLFL", "GNWZ_SSGT", "GNWZ_SSKGG" }, azwz.ToString(), flag);
            //    }
            //    else if (azwz.Equals("户内"))
            //    {
            //        GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSKGG" }, new List<string> { "GNWZ_XLFL", "GNWZ_SSGT", "GNWZ_SSTJ" }, azwz.ToString(), flag);
            //    }
            //    else if (azwz.Equals("柱上"))
            //    {
            //        GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_XLFL", "GNWZ_SSGT" }, new List<string> { "GNWZ_SSKGG", "GNWZ_SSTJ" }, azwz.ToString(), flag);
            //    }
            //    #endregion

            //}
            //    break;
            //case 177:
            //    {
            //        #region 177
            //        //if (azwz == null || string.IsNullOrEmpty(azwz.ToString()))
            //        //{
            //        //    GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_XLFL", "GNWZ_SSKGG", "GNWZ_SSTJ" }, null, true, flag);
            //        //}
            //        //else if (azwz.Equals("户外"))
            //        //{
            //        //    GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSTJ" }, new List<string> { "GNWZ_XLFL", "GNWZ_SSKGG" }, azwz.ToString(), false, flag);
            //        //}
            //        //else if (azwz.Equals("户内"))
            //        //{
            //        //    GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSKGG" }, new List<string> { "GNWZ_XLFL", "GNWZ_SSTJ" }, azwz.ToString(), false, flag);
            //        //}
            //        //else if (azwz.Equals("柱上"))
            //        //{
            //        //    GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_XLFL" }, new List<string> { "GNWZ_SSKGG", "GNWZ_SSTJ" }, azwz.ToString(), false, flag);
            //        //}

            //        #endregion
            //    }
            //    break;
            ////低压柜
            //case 81:
            //    {
            //        #region 81
            //        //if (azwz == null || string.IsNullOrEmpty(azwz.ToString()))
            //        //{
            //        //    GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSTJ" },null, true, flag);
            //        //}
            //        //else if (azwz.Equals("户外"))
            //        //{
            //        //    GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSTJ" }, new List<string> { }, azwz.ToString(), false, flag);
            //        //}
            //        //else if (azwz.Equals("户内"))
            //        //{
            //        //    GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSTJ" }, azwz.ToString(), false, flag);
            //        //}

            //        #endregion
            //    }
            //    break;
            ////高压表
            //case 82:
            //    {
            //        #region 82
            //        if (azwz == null || string.IsNullOrEmpty(azwz.ToString()))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSTJ" }, null, flag);
            //        }
            //        else if (azwz.Equals("户外") || azwz.Equals("柱上"))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSTJ" }, new List<string> { }, azwz.ToString(), flag);
            //        }
            //        else if (azwz.Equals("户内"))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSTJ" }, azwz.ToString(), flag);
            //        }
            //        #endregion

            //    }
            //    break;
            ////计量柜
            //case 84:
            //    {
            //        #region 84
            //        if (azwz == null || string.IsNullOrEmpty(azwz.ToString()))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSTJ" }, null,  flag);
            //        }
            //        else if (azwz.Equals("户外") || azwz.Equals("柱上"))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSTJ" }, new List<string> { }, azwz.ToString(),  flag);
            //        }
            //        else if (azwz.Equals("户内"))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSTJ" }, azwz.ToString(),  flag);
            //        }

            //        #endregion
            //    }
            //    break;
            ////DTU
            //case 90:
            //    {
            //        #region 90
            //        if (azwz == null || string.IsNullOrEmpty(azwz.ToString()))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSTJ", "GNWZ_SSDF" }, null,  flag);
            //        }
            //        else if (azwz.Equals("户外") || azwz.Equals("柱上"))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSTJ" }, new List<string> { "GNWZ_SSDF" }, azwz.ToString(),  flag);
            //        }
            //        else if (azwz.Equals("户内"))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSDF" }, new List<string> { "GNWZ_SSTJ" }, azwz.ToString(),  flag);
            //        }

            //        #endregion
            //    }
            //    break;
            ////FTU
            //case 180:
            //{
            //    #region 180
            //    if (azwz == null || string.IsNullOrEmpty(azwz.ToString()))
            //    {
            //        GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSTJ", "GNWZ_SSGT" }, null,  flag);
            //    }
            //    else if (azwz.Equals("户外"))
            //    {
            //        GradeGenerateByAzwz(obj,  new List<string> { "GNWZ_SSTJ", "GNWZ_SSGT" }, new List<string> { }, azwz.ToString(),  flag);
            //    }
            //    else if (azwz.Equals("户内"))
            //    {
            //        GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSTJ", "GNWZ_SSGT" }, azwz.ToString(),  flag);
            //    }

            //    #endregion
            //}
            //    break;
            ////电压互感器
            //case 307:
            //    {
            //        #region 307
            //        if (azwz == null || string.IsNullOrEmpty(azwz.ToString()))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> { }, new List<string> { "GNWZ_SSKGG", "GNWZ_SSGT" }, null,  flag);
            //        }
            //        else if (azwz.Equals("户内"))
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> {"GNWZ_SSKGG" }, new List<string> {  "GNWZ_SSGT" }, azwz.ToString(),  flag);
            //        }
            //        else
            //        {
            //            GradeGenerateByAzwz(obj,  new List<string> {   "GNWZ_SSGT"}, new List<string> { "GNWZ_SSKGG"}, azwz.ToString(),  flag);
            //        }

            //        #endregion
            //    }
            //    break;

            //    #endregion
            //}
            #endregion
          
        }

        /// <summary>
        /// 检测设备是否含有安装位置属性
        /// </summary>
        /// <param name="obj">检测对象</param>
        /// <param name="fno">对象G3e_FNO</param>
        /// <param name="flag">专门针对功能位置设施分类字段值，true表示是插入的值，false表示是查看值因此不能再次用默认值</param>
        public static void GenerateXPropsByAzwz(XProps obj, int fno,bool flag)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            var simpleattrEntry=DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
            if (simpleattrEntry != null)
            {
                if (simpleattrEntry.InstallLocationOption != null)
                {
                    var tableName = simpleattrEntry.InstallLocationOption.TableName;
                    var fieldName = simpleattrEntry.InstallLocationOption.FieldName;
                    var azwz = GetPropertyValue(obj, tableName, fieldName);
                    GenerateByAzwz(ref obj,azwz,fno,flag);
                    
                }
            }
            #region  优化前
            //if (fno == 146)
            //{
            //    var dlt = GetPropertyValue(obj, GetSelfTableNameByFno(fno), "DLT");
            //    if (dlt != null && !string.IsNullOrEmpty(dlt.ToString()))
            //    {
            //        GenerateByAzwz(ref obj,dlt,fno,flag);
            //    }
            //}else if (fno == 177)
            //{
            //    var azwz = GetPropertyValue(obj, GetSelfTableNameByFno(fno), "AZWZ");
            //    if (azwz != null && !string.IsNullOrEmpty(azwz.ToString()))
            //    {
            //        GenerateByAzwz(ref obj, azwz, fno, flag);
            //    }
            //}
            //else
            //{
            //    var fl = GetPropertyValue(obj, GetGnwzTableNameByFno(fno), "GNWZ_FL2");//这里还可以优化，根据fno获取表名
            //    if (fl != null && !string.IsNullOrEmpty(fl.ToString()))
            //    {
            //        GenerateByAzwz(ref obj, fl, fno, flag);
            //    }
            //}
            #endregion
        }
        /// <summary>
        /// 复位对象的值
        /// </summary>
        /// <param name="xprops"></param>
        public static void ResetXProps(ref XProps xprops)
        {
            foreach (var item in xprops)
            {
                Type type = item.ProType;
                if (!type.IsGenericType)
                {
                    var defaultValue = DefaultValueFactory.Instance.GetDefaultValue(type);
                    SetPropertyValue(xprops,item.Category, item.Name, defaultValue);
                }
                else
                {
                    Type temp = type.GetGenericTypeDefinition();
                    if (temp == typeof (Nullable<>))
                    {
                        SetPropertyValue(xprops,item.Category, item.Name, null);
                    }
                }
            }
        }
        /// <summary>
        /// 拷贝值到Model层指定字段
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段新值</param>
        /// <param name="entity">model实体</param>
        /// <returns>是否拷贝成功</returns>
        public static bool CopyDataToModel(string fieldName, object fieldValue, DBEntity entity)
        {
            bool flag = false;
            if (entity.GetPropertyNames().Contains(fieldName))
            {
                PropertyInfo[] infos = entity.GetType().GetProperties();
                if (infos.Any())
                {
                    PropertyInfo info = infos.SingleOrDefault(o => o.Name.Equals(fieldName));
                    if (info != null)
                    {
                        Type type = info.PropertyType;
                        if (fieldValue == null)
                        {
                            entity.SetValue(fieldName, null);
                            flag = true;
                        }
                        else if (string.IsNullOrEmpty(fieldValue.ToString()))
                        {
                            entity.SetValue(fieldName, null);
                            flag = true;
                        }
                        else
                        {
                            if (!type.IsGenericType)
                            {
                                var obj = Convert.ChangeType(fieldValue, type);
                                entity.SetValue(fieldName, obj);
                                flag = true;
                            }
                            else
                            {
                                Type temp = type.GetGenericTypeDefinition();
                                if (temp == typeof (Nullable<>))
                                {
                                    try
                                    {
                                        var obj = Convert.ChangeType(fieldValue, Nullable.GetUnderlyingType(type));
                                        entity.SetValue(fieldName, obj);
                                        flag = true;
                                    }
                                    catch
                                    {
                                        flag = false;
                                    }
                                  
                                }
                            }
                        }
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 特殊值拷贝，根据安装位置清空一些特殊值
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fieldName"></param>
        /// <param name="dest"></param>
        /// <param name="sourceXProps"></param>
        /// <param name="fno"></param>
        private static void SpecialValueCopy(string fid, string fieldName, DBEntity dest, XProps sourceXProps, int fno)
        {
            if (!string.IsNullOrEmpty(fid))
            {
                if (fid.Equals("0"))
                {
                    CopyDataToModel(fieldName, null, dest);
                }
                else
                    CopyDataToModel(fieldName, fid, dest);
            }
            //下面的这些字段值必须清空
            var clearFieldValue = FilterField(sourceXProps, fno);
            if (clearFieldValue.Any())
            {
                if (clearFieldValue.Contains(fieldName))
                {
                    if (dest.HasAttribute(fieldName))
                    {
                        CopyDataToModel(fieldName, null, dest);
                    }
                }

            }
        }
        /// <summary>
        /// CAD中设备数据拷入数据库
        /// </summary>
        /// <param name="source">CAD设备数据源</param>
        /// <param name="dest">数据库设备源</param>
        /// <param name="fno"> </param>
        public static void PartialCopyFromCAD(XProps source, DBEntity dest, int fno)
        {
            var interset = GetPropertyNameInterset(source, dest, fno);
            if (!interset.Any()) return;
            foreach (var item in interset)
            {
                var specialFiled = SpecialFilterOptions.SingleOrDefault(o =>o.FNO==fno&&o.FromOtherFidOfFiledName.Equals(item)&&o.saveValuebyFid);
                if (specialFiled!=null)
                {
                    switch (item)
                    {
                        case "PBMC":
                        case "SSBYQ":
                            SpecialValueCopy(Unrf.byqFid, item, dest, source, fno);
                            break;
                        case "SSDF":
                        case "GNWZ_SSDF":
                            SpecialValueCopy(Unrf.dfFid, item, dest, source, fno);
                            break;
                        case "DYKGG":
                        case "GNWZ_SSKGG":
                            SpecialValueCopy(Unrf.kggFid, item, dest, source, fno);
                            break;
                        case "GNWZ_SSTJ":
                            SpecialValueCopy(Unrf.tjFid, item, dest, source, fno);
                            break;
                        case "GNWZ_SSGT":
                            SpecialValueCopy(Unrf.gtFid, item, dest, source, fno);
                            break;
                        case "GNWZ_SSTQHTJ":
                            SpecialValueCopy(Unrf.tqtjFid, item, dest, source, fno);
                            break;
                        case "BZ1":
                        case "BZ2":
                            SpecialValueCopy(Unrf.zxFid, item, dest, source, fno);
                            break;

                    }
                }
                else
                {
                    try
                    {
                        //如果该字段是需要从台账同步的就无需覆盖原来的值，尤其是批量修改
                        if (!IsReadOnly(source,dest.GetType().Name, item))
                        {
                            CopyDataToModel(item, GetPropertyValue(source,dest.GetType().Name, item), dest);
                        }
                    }
                    catch
                    {
                        throw new UpdataArgumentException(dest.GetType().Name,item);
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前绑定对象与对应的模型对象的交集
        /// </summary>
        /// <param name="source">源对象即就是绑定的对象</param>
        /// <param name="dest">必须是DBEntity类型</param>
        /// <param name="fno"></param>
        /// <returns></returns>
        public static List<string> GetPropertyNameInterset(XProps source, DBEntity dest, int fno)
        {
            List<string> sourcePropertyNames = new List<string>();
            if (dest != null &&dest.GetType().BaseType.Name.Equals("ElectronBase"))
            {
                var tableName = dest.GetType().Name;
                sourcePropertyNames = GetPropertyNameFromTable(source, tableName);
            }
            return sourcePropertyNames;
        }

        /// <summary>
        /// 获取指定表中的所有字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static List<string> GetPropertyNameFromTable(XProps xprops, string tableName)
        {
            List<string> li = new List<string>();
            foreach (var item in xprops)
            {
                if (item.Category.Equals(tableName))
                {
                    li.Add(item.Name);
                }
            }
            return li;
        }

        /// <summary>
        /// 拷贝数据到CAD设备
        /// </summary>
        /// <param name="source">数据库源数据</param>
        /// <param name="dest">CAD设备</param>
        /// <param name="fno"></param>
        public static void PartialCopyToCAD(DBEntity source, XProps dest, int fno)
        {
            try
            {
                List<string> interset = GetPropertyNameInterset(dest, source, fno);
                if (!interset.Any()) return;
                foreach (var item in interset)
                {
                    SetPropertyValue(dest,source.GetType().Name, item, source.GetValue(item));
                    var res = SpecialFilterOptions.Where(o => o.FNO == fno);
                    if (res != null && res.Any())
                    {
                        var sfo = res.SingleOrDefault(o => o.FromOtherFidOfFiledName.Equals(item));
                        if (sfo != null)
                        {
                            var fid = source.GetValue(item);
                            if (fid != null)
                            {
                                long resFid = 0;
                                if (long.TryParse(fid.ToString(), out resFid))
                                {
                                    var com = DBManager.Instance.GetEntity<Common_n>(resFid);
                                    if (com != null)
                                        SetPropertyValue(dest, source.GetType().Name, item, com.SBMC);
                                }
                            }
                        }
                    }
                   
                }
            }
            catch (Exception)
            {
                LogManager.Instance.Error(fno.ToString()+source);
            }

        }

        /// <summary>
        /// 根据fno过滤出与安装位置无关的特殊属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fno"></param>
        public static List<string> FilterField(XProps obj, long fno)
        {
            List<string> propertyNames = new List<string>();
            try
            {
                var simpleEntry = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
                if (simpleEntry != null)
                {
                    if (simpleEntry.InstallLocationOption != null)
                    {
                        var tableName = simpleEntry.InstallLocationOption.TableName;
                        var fieldName = simpleEntry.InstallLocationOption.FieldName;
                        var azwz = GetPropertyValue(obj, tableName, fieldName);
                        var azwzInfo = GetAzwzOptionByFno((int)fno);
                        if (azwzInfo.Any())
                        {
                            if (azwz == null || string.IsNullOrEmpty(azwz.ToString()))
                            {
                                List<SpecialFilterOption> temp = new List<SpecialFilterOption>();

                                foreach (var azwzOption in azwzInfo)
                                {
                                    var res1 = GetFieldsFromAzwzCollection(azwzOption.FilterFields);
                                    var res2 = GetFieldsFromAzwzCollection(azwzOption.ShowFields);
                                    if (temp.Any())
                                    {
                                        var res3 = res1.Concat(res2)
                                            .Distinct(new SpecialFilterOptionCompare())
                                            .OrderBy(o => o.FromOtherFidOfFiledName).ToList();
                                        temp =
                                            temp.Concat(res3)
                                                .Distinct(new SpecialFilterOptionCompare())
                                                .OrderBy(o => o.FromOtherFidOfFiledName)
                                                .ToList();

                                    }
                                    else
                                    {
                                        temp = res1.Concat(res2)
                                           .Distinct(new SpecialFilterOptionCompare())
                                           .OrderBy(o => o.FromOtherFidOfFiledName).ToList();
                                    }

                                }
                                if (temp.Any())
                                {
                                    temp.ForEach(o=>propertyNames.Add(o.FromOtherFidOfFiledName));
                                }
                            }
                            else
                            {
                                var res = azwzInfo.SingleOrDefault(o => o.AzwzState.Equals(azwz.ToString()));
                                if (res != null)
                                {
                                    var fieldAndTable=GetFieldsFromAzwzCollection(res.FilterFields);
                                    if (fieldAndTable.Any())
                                    {
                                        fieldAndTable.ForEach(o=>propertyNames.Add(o.FromOtherFidOfFiledName));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            #region 根据备注过滤

            try
            {
                var ent = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
                if (ent != null)
                {
                    if (ent.Common != null && ent.Common.PropertiesFromTable != null && ent.Common.PropertiesFromTable.Any())
                    {
                        //到此说明有公共属性
                        var xprop = GetSingleXProp(obj, ent.Common.TableName, "BZ");
                        if (xprop != null)
                        {
                            var bz = xprop.Value;
                            //根据备注的项来过滤
                            CheckIsHasNoVerifyFiled(fno, bz,ref propertyNames);
                        }
                    }
                    else
                    {
                        //去自身表找备注
                        var xprop = GetSingleXProp(obj, ent.SelfAttribute.TableName, "BZ");
                        if (xprop != null)
                        {
                            var bz = xprop.Value;
                            CheckIsHasNoVerifyFiled(fno, bz,ref propertyNames);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }

            #endregion
            return propertyNames.Distinct().ToList();
            


            #region  根据安装位置过滤
            //switch (fno)
            //{
            //    case 148:
            //        {
            //            #region 148
            //            var value = GetPropertyValue(obj, GetGnwzTableNameByFno(148), "GNWZ_FL2");
            //            if (value == null)
            //            {
            //                propertyNames.Add("GNWZ_SSTJ");
            //            }
            //            else
            //            {
            //                if (!value.ToString().Equals("台架"))
            //                {
            //                    propertyNames.Add("GNWZ_SSTJ");
            //                }
            //            }
            //            #endregion
            //        }
            //        break;
            //    case 146:
            //        {
            //            #region 146
            //            var value = GetPropertyValue(obj, GetSelfTableNameByFno(146), "DLT");
            //            if (value == null)
            //            {
            //                propertyNames.AddRange(new string[] { "GNWZ_XLFL", "GNWZ_SSGT", "GNWZ_SSTJ", "GNWZ_SSKGG" });
            //            }
            //            else
            //            {
            //                string temp = value.ToString();
            //                if (temp.Equals("户外"))
            //                {
            //                    propertyNames.AddRange(new string[] { "GNWZ_XLFL", "GNWZ_SSKGG", "GNWZ_SSGT" });
            //                }
            //                else if (temp.Equals("户内"))
            //                {
            //                    propertyNames.AddRange(new string[] { "GNWZ_XLFL", "GNWZ_SSGT", "GNWZ_SSTJ" });
            //                }
            //                else if (temp.Equals("柱上"))
            //                {
            //                    propertyNames.AddRange(new string[] { "GNWZ_SSTJ", "GNWZ_SSKGG" });
            //                }
            //                else
            //                {
            //                    propertyNames.AddRange(new string[] { "GNWZ_XLFL", "GNWZ_SSGT", "GNWZ_SSTJ", "GNWZ_SSKGG" });
            //                }
            //            }

            //            #endregion
            //        }
            //        break;
            //    case 173:
            //        {
            //            //azwzColl = new List<string>(new string[] { "GNWZ_SSKGG" });
            //        }
            //        break;
            //    case 177:
            //        {
            //            #region 177
            //            var value = GetPropertyValue(obj, GetSelfTableNameByFno(177), "AZWZ");
            //            if (value == null)
            //            {
            //                propertyNames.AddRange(new string[] { "GNWZ_XLFL", "GNWZ_SSTJ", "GNWZ_SSKGG" });
            //            }
            //            else
            //            {
            //                string temp = value.ToString();
            //                if (temp.Equals("户外"))
            //                {
            //                    propertyNames.AddRange(new string[] { "GNWZ_XLFL", "GNWZ_SSKGG" });
            //                }
            //                else if (temp.Equals("户内"))
            //                {
            //                    propertyNames.AddRange(new string[] { "GNWZ_XLFL", "GNWZ_SSTJ" });
            //                }
            //                else if (temp.Equals("柱上"))
            //                {
            //                    propertyNames.AddRange(new string[] { "GNWZ_SSTJ", "GNWZ_SSKGG" });
            //                }
            //                else
            //                {
            //                    propertyNames.AddRange(new string[] { "GNWZ_XLFL", "GNWZ_SSTJ", "GNWZ_SSKGG" });
            //                }
            //            }

            //            #endregion
            //        }
            //        break;
            //    //低压柜
            //    case 81:
            //        {
            //            #region

            //            //var value = GetPropertyValue(obj,"Gg_pd_gnwzmc_n", "GNWZ_FL2");
            //            //if (value == null)
            //            //{
            //            //    propertyNames.Add("GNWZ_SSTJ");
            //            //}
            //            //else
            //            //{
            //            //    //这里就包括了为空或户内的情况
            //            //    if (!value.ToString().Equals("户外"))
            //            //    {
            //            //        propertyNames.Add("GNWZ_SSTJ");
            //            //    }
            //            //}

            //            #endregion
            //        }
            //        break;
            //    //高压表
            //    case 82:
            //        {
            //            #region 82
            //            var value = GetPropertyValue(obj,GetGnwzTableNameByFno(82), "GNWZ_FL2");
            //            if (value == null)
            //            {
            //                propertyNames.Add("GNWZ_SSTJ");
            //            }
            //            else
            //            {
            //                if (string.IsNullOrEmpty(value.ToString()) || value.ToString().Equals("户内"))
            //                {
            //                    propertyNames.Add("GNWZ_SSTJ");
            //                }
            //            }
            //            #endregion

            //        }
            //        break;
            //    //计量柜
            //    case 84:
            //        {
            //            #region 84
            //            var value = GetPropertyValue(obj, GetGnwzTableNameByFno(84), "GNWZ_FL2");
            //            if (value == null)
            //            {
            //                propertyNames.Add("GNWZ_SSTJ");
            //            }
            //            else
            //            {
            //                if (string.IsNullOrEmpty(value.ToString()) || value.ToString().Equals("户内"))
            //                {
            //                    propertyNames.Add("GNWZ_SSTJ");
            //                }
            //            }
            //            #endregion

            //        }
            //        break;
            //    //DTU
            //    case 90:
            //        {
            //            #region 90
            //            var value = GetPropertyValue(obj, GetGnwzTableNameByFno(90), "GNWZ_FL2");
            //            if (value == null)
            //            {
            //                propertyNames.AddRange(new[] { "GNWZ_SSDF", "GNWZ_SSTJ" });
            //            }
            //            else
            //            {
            //                if (string.IsNullOrEmpty(value.ToString()))
            //                {
            //                    propertyNames.AddRange(new[] { "GNWZ_SSDF", "GNWZ_SSTJ" });
            //                }
            //                else if (value.ToString().Equals("户内"))
            //                {
            //                    propertyNames.Add("GNWZ_SSTJ");
            //                }
            //                else if (value.ToString().Equals("户外") || value.ToString().Equals("柱上"))
            //                {
            //                    propertyNames.Add("GNWZ_SSDF");
            //                }
            //            }
            //            #endregion

            //        }
            //        break;
            //    //FTU
            //    case 180:
            //        {
            //            #region 180
            //            var value = GetPropertyValue(obj,GetGnwzTableNameByFno(180), "GNWZ_FL2");
            //            if (value == null)
            //            {
            //                propertyNames.AddRange(new string[] { "GNWZ_SSTJ", "GNWZ_SSGT" });
            //            }
            //            else
            //            {
            //                if (!value.ToString().Equals("户外"))
            //                {
            //                    propertyNames.AddRange(new string[] { "GNWZ_SSTJ", "GNWZ_SSGT" });
            //                }
            //            }
            //            #endregion
            //        }
            //        break;
            //    ///配网仪
            //    case 181:
            //        {
            //            //azwzColl = new List<string>(new string[] { "PBMC" });

            //        }
            //        break;
            //    //电压互感器
            //    case 307:
            //        {
            //            #region 307
            //            var value = GetPropertyValue(obj, GetGnwzTableNameByFno(307), "GNWZ_FL2");
            //            if (value == null)
            //            {
            //                propertyNames.AddRange(new[] { "GNWZ_SSGT", "GNWZ_SSKGG" });
            //            }
            //            else
            //            {
            //                if (string.IsNullOrEmpty(value.ToString()))
            //                {
            //                    propertyNames.AddRange(new[] { "GNWZ_SSGT", "GNWZ_SSKGG" });
            //                }
            //                else if (value.ToString().Equals("户内"))
            //                {
            //                    propertyNames.Add("GNWZ_SSGT");
            //                }
            //                else
            //                {
            //                    propertyNames.Add("GNWZ_SSKGG");
            //                }
            //            }
            //            #endregion
            //        }
            //        break;
            //}
            #endregion

          
        }

        private static void CheckIsHasNoVerifyFiled(long fno,object bz,ref List<string> propertyNames)
        {
            if (bz != null && !string.IsNullOrEmpty(bz.ToString()))
            {
                var res = bz.ToString().Replace("无", "").Split(',');
                foreach (var item in res)
                {
                    var specialFilterOption = SpecialFilterOptions.SingleOrDefault(o => o.FNO == fno && item.Equals(o.DisplayName));
                    if (specialFilterOption != null)
                    {
                        if(!propertyNames.Contains(specialFilterOption.FromOtherFidOfFiledName))
                            propertyNames.Add(specialFilterOption.FromOtherFidOfFiledName);
                    }
                }
            }
        }
        /// <summary>
        /// 是否需要填写所属开关柜
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsFillSskggValue(RequiredDevTables obj)
        {
            bool flag = false;
            //146的单独在VerifyRuleExecutionFactory中处理
            IList<int> fnoList = new List<int>{40,173,177,86,174};
            if (fnoList.Contains(obj.Fno))
            {
                //先找到他们的从属关系，只有在公用电房，专用电房才是必须填写，在箱式设备里面可以不填
                if (obj.ComObj != null)
                {
                    var owner = DBManager.Instance.GetEntity<Common_n>(o => o.G3E_ID == obj.ComObj.OWNER1_ID);
                    if (owner != null)
                    {
                        if (owner.G3E_FNO != 149)
                            flag = true;
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// 校验空值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="choosePropertyName">过滤掉的字段</param>
        /// <param name="fno"></param>
        /// <returns></returns>
        public static StringBuilder FilterVerify(RequiredDevTables obj, List<string> choosePropertyName,int fno)
        {
            StringBuilder sBuilder = new StringBuilder();
            XProps temp = GenerateObj.Instance.GenderObjByFno(fno);
            try
            {
                foreach (var attr in temp)
                {
                    if (attr.Name.Equals("CD_SSDW"))
                    {
                        IList<string> gdjList=DBEntityFinder.Instance.GetGDJListByKx(DBEntityFinder.Instance.GetSDKXList());
                        string gdj = string.Empty;
                        if (fno == 159&&obj.SelfObj!=null)
                        {
                            gdj = obj.SelfObj.GetAttribute("CD_SSDW");
                        }
                        else
                        {
                            if (obj.ComObj != null)
                                gdj = obj.ComObj.CD_SSDW;
                        }
                        if (!string.IsNullOrEmpty(gdj)&&!gdjList.Contains(gdj))
                        {
                            sBuilder.AppendLine(" 当前设备所属单位录入不一致 ");
                        }
                    }
                    if (attr.Name.Equals("CD_SSBDZ"))
                    {
                        continue;
                    }
                    if (fno == 198 && attr.Name.Equals("CD_SSXL"))
                    {
                        continue;
                    }
                    if (choosePropertyName.Any(o => o.Equals(attr.Name)))
                    {
                        continue;
                    }
                    if (obj.ComObj != null)
                    {
                        if ((attr.Name.Equals("GNWZ_SSKGG") || attr.Name.Equals("DYKGG")) && !IsFillSskggValue(obj))
                            continue;
                    }
                  
                    #region   检查该填的值是否都填好了

                    object value = GetAttributeValueByField(attr, obj);
                    #endregion
                
                    if (attr.Browsable) //只校验必填项
                    {
                        if (value == null || string.IsNullOrEmpty(value.ToString().Trim())||value.Equals(default(DateTime)))
                        {
                            if (attr.DisplayName.Contains('*'))
                                sBuilder.AppendLine(attr.DisplayName.Replace('*', ' ').Trim() + "不能为空 ");
                            else
                                sBuilder.AppendLine(attr.DisplayName + "不能为空 ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.Error(ex.Message);
            }
            return sBuilder;
        }
        /// <summary>
        /// 根据字段名获取模型表中的字段值
        /// </summary>
        /// <returns></returns>
        public static object GetAttributeValueByField(XProp xProp,RequiredDevTables obj)
        {
            object value = null;
            if (xProp.Category == "Common_n" && obj.ComObj != null)
            {
                value = obj.ComObj.GetValue(xProp.Name);
            }
            else if (xProp.Category == "Connectivity_n" && obj.ConnectObj != null)
            {
                value = obj.ConnectObj.GetValue(xProp.Name);
            }
            else if (xProp.Category == "Gg_pd_gnwzmc_n" && obj.GnwzObj != null)
            {
                value = obj.GnwzObj.GetValue(xProp.Name);
            }
            else
            {
                if (obj.SelfObj != null && obj.SelfObj.HasAttribute(xProp.Name))
                    value = obj.SelfObj.GetValue(xProp.Name);

            }
            return value;
        }
        public static StringBuilder VerifySingleDeviceAttribute(RequiredDevTables obj,int g3e_fno)
        {
            var noVerifyPropertyNames = FilterField(obj.DevObj, g3e_fno);
            return FilterVerify(obj, noVerifyPropertyNames, g3e_fno);
        }
        /// <summary>
        /// 判断功能位置名称是否重复
        /// </summary>
        /// <param name="xprops"></param>
        /// <param name="CurrentFno"></param>
        /// <returns></returns>
        public static bool IsDistinctSBMC(XProps xprops,int CurrentFno)
        {
            List<string> noVerify = new List<string>();
            if (!string.IsNullOrEmpty(MapConfig.Instance.NoVerifySbmcFeatures))
            {
                noVerify.AddRange(MapConfig.Instance.NoVerifySbmcFeatures.Split(','));
            }
            if (noVerify.Any(o=>o.Equals(CurrentFno.ToString())))
            {
                return false;
            }
            Dictionary<long, string> dict = DBEntityFinder.Instance.GetAllSymbolSBMC();
            var sbmc = GetPropertyValue(xprops, "Common_n", "SBMC");//这里记得查看
            var col = dict.Values.Where(o => !string.IsNullOrEmpty(o) && o.Trim().Equals(sbmc));
            if (col != null && col.Count() > 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  检测增量设备属性是否完整
        /// </summary>
        /// <param name="obj">合并后的设备对象</param>
        /// <param name="g3e_fid"></param>
        /// <param name="g3e_fno">校验对象的fno</param>
        /// <returns>返回校验的错误信息</returns>
        public static StringBuilder FieldIsNull(RequiredDevTables obj, long g3e_fid, int g3e_fno)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                VerifyRuleEventArgs vre = new VerifyRuleEventArgs()
                {
                    DevTables=obj,
                    ErrorMsg = null
                };
                ExecutionManager.Instance.GetFactory(typeof(VerifyRuleExecutionFactory)).GetExecution(g3e_fno).Execute(null, vre);
                if (vre.ErrorMsg != null && vre.ErrorMsg.Length > 0)
                    sb.AppendLine(vre.ErrorMsg.ToString());
                if (!vre.IsUseFactoryVerify)
                {
                    var verifyRes = VerifySingleDeviceAttribute(obj, g3e_fno);
                    if (verifyRes!=null&&!string.IsNullOrEmpty(verifyRes.ToString().Trim()))
                        sb.AppendLine(verifyRes.ToString());
                }
                #region 校验功能位置名称是否重复
                if (IsDistinctSBMC(obj.DevObj,obj.Fno))
                {
                    sb.AppendLine(" 功能位置名称重复 ");
                }
                #endregion
                //检查受电馈线是否填写
                var entitytables = SimpleDBSymbolsConfig.Instance.DBSymbol.SingleOrDefault(o => o.G3E_FNO == g3e_fno);
                if (entitytables != null)
                {
                    if (!string.IsNullOrEmpty(entitytables.ComponentTable.Gg_Pd_Sdkx_Ac))
                    {
                        var sdkx = DBManager.Instance.GetEntities<Gg_pd_sdkx_ac>(o => o.G3E_FID == g3e_fid);
                        if (sdkx == null || !sdkx.Any())
                        {
                            sb.AppendLine(" 缺少受电馈线录入 ");
                        }
                        else//查看当前设备的受电馈线表里面是否有所属线路里面的馈线
                        {
                            if (obj != null && obj.ComObj != null)
                            {
                                var ssxl = obj.ComObj.CD_SSXL;
                                if (!IsOriginalFeature(obj))
                                {
                                    if (!sdkx.Any(o => !string.IsNullOrEmpty(o.KXH) && o.KXH.Equals(ssxl)))
                                    {
                                        sb.AppendLine(" 受电馈线录入不一致 ");
                                    }
                                }

                            }
                        }
                    }
                }
                return sb;
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                LogManager.Instance.Error(ex);
                return sb.AppendLine(" 该设备在校验中出错 ");
            }
        }
        /// <summary>
        /// 判断该设备是否是原始设备
        /// </summary>
        /// <param name="rdt"></param>
        /// <returns>true原始设备，false新增设备</returns>
        public static bool IsOriginalFeature(RequiredDevTables rdt)
        {
            if (rdt != null)
            {
                if (GetIsOleFeature(rdt.ComObj)) return true;
                if (GetIsOleFeature(rdt.SelfObj)) return true;
                if (GetIsOleFeature(rdt.ConnectObj)) return true;
                if (GetIsOleFeature(rdt.GnwzObj)) return true;
            }
            return false;
        }

        public static bool GetIsOleFeature(DBEntity entity)
        {
            bool flag = false;
            if (entity != null)
            {
                if (entity.EntityState == EntityState.None)
                {
                    flag = true;
                }
                else if (entity.EntityState == EntityState.Update)
                {
                    flag = true;
                }
            }
            return flag;
        }
        /// <summary>
        /// 判断当前绑定对象必填字段是否完整
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public static bool IsNullField(XProps props)
        {
            if (props != null)
            {
                foreach (var item in props)
                {
                    if (item.Browsable)
                    {
                        var obj = GetPropertyValue(props,item.Category, item.Name);
                        if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                            return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 对供电局变电站清空操作
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        private static void SetGDJAndBDZValue(XProps obj,string fieldName)
        {
            var res = GetXPropList(obj, fieldName);
            if (res.Any())
            {
                foreach (var item in res)
                {
                    SetPropertyValue(obj,item.Key,item.Value,null);
                }
            }
        }
        /// <summary>
        /// 获取用户录入的所属单位
        /// </summary>
        /// <param name="obj">当前录入属性的对象</param>
        /// <param name="oldGdj"></param>
        /// <param name="newGdj"></param>
        public static void SetSSDW(XProps obj,object oldGdj,string newGdj)
        {
            try
            {
                if (string.IsNullOrEmpty(newGdj))
                {

                    SetGDJAndBDZValue(obj, "CD_SSDW");
                    SetGDJAndBDZValue(obj, "CD_SSBDZ");
                    SetGDJAndBDZValue(obj, "CD_SSXL");
                    //SetGDJAndBDZValue(obj, "GNWZ_SSGDS");
                    SetGDJAndBDZValue(obj, "WHBS");
                }else if ((oldGdj==null)||(oldGdj != null && !newGdj.Equals(oldGdj.ToString())))
                {
                    SetGDJAndBDZValue(obj, "CD_SSBDZ");
                    SetGDJAndBDZValue(obj, "CD_SSXL");
                    //SetGDJAndBDZValue(obj, "GNWZ_SSGDS");
                    SetGDJAndBDZValue(obj, "WHBS");
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 获取变电站
        /// </summary>
        /// <param name="obj">当前绑定对象</param>
        /// <param name="oldValue"> </param>
        /// <param name="newValue"> </param>
        public static void SetSSBDZ(XProps obj,object oldValue,string newValue)
        {
            try
            {
                if (string.IsNullOrEmpty(newValue))
                {
                    SetGDJAndBDZValue(obj, "CD_SSBDZ");
                    SetGDJAndBDZValue(obj, "CD_SSXL");
                }else if((oldValue==null)||(oldValue!=null&&!newValue.Equals(oldValue.ToString())))
                {
                    SetGDJAndBDZValue(obj, "CD_SSXL");
                }

            }
            catch (System.Exception ex)
            {
                LogManager.Instance.Error(ex);
            }
        }

        /// <summary>
        /// 清空静态字段的值
        /// </summary>
        public static void EmptyFidRecord()
        {
            Unrf = new UserNameReplaceFidSpecialFeature();
        }

        public static void EmptyPrevFeatureAttribute()
        {
            Atff=new AllTablesFromFeature();
        }
        #region 台账同步校验接口
        /// <summary>
        /// 校验台帐是否存在
        /// </summary>
        /// <param name="g3e_fids">传入台账的fid集合，用逗号分隔</param>
        public static string VerifyTzExists(string g3e_fids)
        {
            try
            {
                var urlstr = string.Format(
                    "http://localhost:9090/emmis/equipGisMappingTemp/cadRestful/checkEquipmentForFuncplace.gis?sourceFunpalce={0}", g3e_fids);
                System.Net.WebClient wc = new System.Net.WebClient();
                var redata = wc.DownloadData(urlstr);
                return System.Text.Encoding.UTF8.GetString(redata);

            }
            catch
            {
                return "fail,"+g3e_fids;
            }
        }

      

        public static void FindSyncData()
        {
            
        }
        #endregion
    }

    public sealed class AllTablesFromFeature
    {
        /// <summary>
        /// 临时记录鼠标点击的实体公共属性
        /// </summary>
        public  Common_n com { get; set; }

        /// <summary>
        /// 临时记录鼠标点击的实体自身属性
        /// </summary>
        public  DBEntity self { get; set; }

        /// <summary>
        /// 临时记录鼠标点击的实体连接关系属性
        /// </summary>
        public  Connectivity_n connectivity { get; set; }

        /// <summary>
        /// 临时记录鼠标点击的实体功能位置属性
        /// </summary>
        public  Gg_pd_gnwzmc_n gnwz { get; set; }
    }
    public  sealed class ChoiceDev
    {
        public string G3eFid { get; set; }
        public string DeviceSbmc { get; set; }
    }

    public sealed class UpdataArgumentException : Exception
    {
        public string FieldName { get; set; }
        public string TableName { get; set; }
        public UpdataArgumentException()
        {
        }

        public UpdataArgumentException(string tableName,string fieldname)
        {
            TableName = tableName;
            FieldName = fieldname;
        }
    }
    public sealed class RequiredDevTables
    {
        public XProps DevObj { get; set; }
        public DBEntity SelfObj { get; set; }
        public Common_n ComObj { get; set; }
        public Connectivity_n ConnectObj { get; set; }
        public Gg_pd_gnwzmc_n GnwzObj { get; set; }
        public int Fno { get; set; }
        public long Fid { get; set; }
    }
    public sealed class FieldTableNameAndBZMerger
    {
        public string Field { get; set; }
        public string TableName { get; set; }
        public string BzDeclare { get; set; }

    }
    public sealed class UserNameReplaceFidSpecialFeature
    {
        /// <summary>
        /// 记录电房的g3eId
        /// </summary>
        public  long kggG3EID { get; set; }
        /// <summary>
        /// 记录变压器FID
        /// </summary>
        public  String byqFid { get; set; }

        /// <summary>
        /// 记录台架的FID
        /// </summary>
        public  String tjFid { get; set; }

        /// <summary>
        /// 记录开关柜的FID
        /// </summary>
        public  String kggFid { get; set; }

        /// <summary>
        /// 记录电房的FID
        /// </summary>
        public  String dfFid { get; set; }

        /// <summary>
        /// 记录杆塔的FID
        /// </summary>
        public  String gtFid { get; set; }

        /// <summary>
        /// 记录台区台架的FID
        /// </summary>
        public  String tqtjFid { get; set; }

        /// <summary>
        /// 记录支线的FID
        /// </summary>
        public  String zxFid { get; set; }
    }
    public sealed class SpecialFilterOption
    {
        public int FNO { get; set; }
        public string TableName { get; set; }
        /// <summary>
        /// 用FID来作为其值得字段名
        /// </summary>
        public string FromOtherFidOfFiledName { get; set; }
        public string DisplayName { get; set; }
        public bool saveValuebyFid { get; set; }
        public XProps SpecialXProps { get; set; }
       
    }
    public class BulkChangeEnt
    {
        public G3EObject SingleG3EObject { get; set; }
        public string LayerName { get; set; }
    }
    public class SpecialFilterOptionCompare : IEqualityComparer<SpecialFilterOption>
    {
        #region IEqualityComparer<SpecialFilterOption> 成员

        public bool Equals(SpecialFilterOption x, SpecialFilterOption y)
        {
            if (object.ReferenceEquals(x, y)) return true;
            if (object.ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            return x.FromOtherFidOfFiledName == y.FromOtherFidOfFiledName && x.TableName == y.TableName;
        }

        public int GetHashCode(SpecialFilterOption obj)
        {
            return obj.FromOtherFidOfFiledName.GetHashCode();
        }

        #endregion
    }

   
}
