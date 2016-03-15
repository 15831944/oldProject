using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ElectronTransferFramework;
using ElectronTransferModel.Config;
using ElectronTransferModel.V9_4;
using ElectronTransferModel.Base;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using ElectronTransferView.SearchManager;
using System.Windows.Forms;
using ElectronTransferDal.AutoGeneration;
using System.ComponentModel;
using ElectronTransferDal.Query;
namespace ElectronTransferView
{
    public class AutoGenerationHelper:Singleton<AutoGenerationHelper>
    {
        public Dictionary<string, UITypeEditor> editors = new Dictionary<string, UITypeEditor>();

        public AutoGenerationHelper()
        {
            editors.Add("PBMC", new SsbyqList());//181,159 所属变压器 自身属性表
            editors.Add("SSBYQ", new SsbyqList());//155  自身属性表
            editors.Add("GNWZ_SSTQHTJ", new SsbyqList());//157  156 功能位置表
            //功能位置表
            editors.Add("GNWZ_SSTJ", new SstjList());


            editors.Add("SSDF", new SsdfList());//点操机构86 SSDF自身属性表
            editors.Add("GNWZ_SSDF", new SsdfList());//ＤＴＵ　　90功能位置表

            editors.Add("DYKGG", new SskggList());//DYKGG 自身属性表 86
            editors.Add("GNWZ_SSKGG", new SskggList());//GNWZ_SSKGG 功能位置表 

            editors.Add("GNWZ_SSGT", new SsgtList());//功能位置表

            editors.Add("BZ1", new SszxList());
            editors.Add("BZ2", new SszxList());
            editors.Add("BZ",new RichText());
        }

        /// <summary>
        /// 给指定的字段设置选择框
        /// </summary>
        /// <param name="fno"></param>
        /// <param name="xprops"></param>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        private void SetEditor(int fno,XProps xprops, string tableName, string fieldName)
        {
            var xprop = GenerateHelper.GetSingleXProp(xprops, tableName, fieldName);
            if (xprop != null)
            {
                if (fno == 148&&fieldName.Equals("GNWZ_SSTJ"))
                {
                    xprop.Converter = new NoEditConverter();
                }
                else
                {
                    xprop.Editor = editors[fieldName];
                    xprop.Converter = new NoEditConverter();
                }
            }
        }

        public void AddUITypeEditor(XProps xprops,int fno)
        {
            if (fno==0) return;//这条数据时自己添加的
            foreach (var xProp in xprops)
            {
                if (xProp.Name.Equals("BZ"))
                {
                    if (xProp.Category.Equals("Common_n"))
                    {
                        SetEditor(fno,xprops, xProp.Category, "BZ");
                    }
                    else
                    {
                        //如果不是公共表的备注先查看该设备是否有公共表，没有的话就设置当前的，否则不设置
                        var ent = DeviceAttributeConfig.Instance.Attributes.SingleOrDefault(o => o.Fno == fno);
                        if (ent != null)
                        {
                            if (ent.Common != null && ent.Common.PropertiesFromTable != null &&
                                ent.Common.PropertiesFromTable.Any())//说明有公共表
                            {
                            }
                            else
                            {
                                SetEditor(fno,xprops, xProp.Category, "BZ");
                            }
                        }
                    }
                }
                if (xProp.SaveValueByFid)
                {
                    SetEditor(fno,xprops, xProp.Category, xProp.Name);
                }
               
                #region
                //switch (xProp.Name)
                //{
                //    case "PBMC":
                //        SetEditor(xprops,xProp.Category, "PBMC");
                //        break;
                //    case "GNWZ_SSTQHTJ":
                //        SetEditor(xprops,
                //            new Dictionary<string, string>
                //            {
                //                {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).Gnwz, "GNWZ_SSTQHTJ"}
                //            });
                //        break;
                //    case "SSBYQ":
                //        SetEditor(xprops,
                //            new Dictionary<string, string>
                //            {
                //                {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).SelfAttribute, "SSBYQ"}
                //            });
                //        break;
                //    case "GNWZ_SSTJ":
                //        SetEditor(xprops,
                //            new Dictionary<string, string>
                //            {
                //                {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).Gnwz, "GNWZ_SSTJ"}
                //            });
                //        break;
                //    case "SSDF":
                //        SetEditor(xprops,
                //            new Dictionary<string, string>
                //            {
                //                {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).SelfAttribute, "SSDF"}
                //            });
                //        break;
                //    case "GNWZ_SSDF":
                //        SetEditor(xprops,
                //            new Dictionary<string, string>
                //            {
                //                {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).Gnwz, "GNWZ_SSDF"}
                //            });
                //        break;
                //    case "DYKGG":
                //        SetEditor(xprops,
                //            new Dictionary<string, string>
                //            {
                //                {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).SelfAttribute, "DYKGG"}
                //            });
                //        break;
                //    case "GNWZ_SSKGG":
                //        SetEditor(xprops,
                //            new Dictionary<string, string>
                //            {
                //                {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).Gnwz, "GNWZ_SSKGG"}
                //            });
                //        break;
                //    case "GNWZ_SSGT":
                //        SetEditor(xprops,
                //            new Dictionary<string, string>
                //            {
                //                {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).Gnwz, "GNWZ_SSGT"}
                //            });
                //        break;
                //    case "BZ2":
                //        SetEditor(xprops,
                //            new Dictionary<string, string>
                //            {
                //                {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).Common, "BZ2"}
                //            });
                //        break;
                //    case "BZ":
                //        if (!GenerateHelper.NoCommonFeature.Contains(fno))
                //        {
                //            SetEditor(xprops,
                //               new Dictionary<string, string>
                //                {
                //                    {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).Common, "BZ"}
                //                });
                //        }
                //        else
                //        {
                //            SetEditor(xprops,
                //               new Dictionary<string, string>
                //                {
                //                    {DBEntityFinder.Instance.GetSelfTalbeNameByFno(fno).SelfAttribute, "BZ"}
                //                });
                //        }


                //        break;
                //}
                #endregion
            }
        }

        public object GetChooseValue(IServiceProvider provider,ITypeDescriptorContext context,int fno,object oldValue)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                var pd = context.Instance;
                if (pd != null)
                {
                    XProps xprops = (XProps) pd;

                    var ssxl = GenerateHelper.GetFirstXPropOfHasValue(xprops, "CD_SSXL");
                    if (ssxl != null && !string.IsNullOrEmpty(ssxl.Value.ToString()))
                    {
                        SpecialDeviceList DevCol = new SpecialDeviceList(fno, ssxl.Value.ToString());
                        if (SpecialDeviceList.devCount <= 0)
                        {
                            MessageBox.Show("当前搜索设备个数为：" + SpecialDeviceList.devCount, "提示", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return oldValue;
                        }

                        Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(DevCol);
                        if (string.IsNullOrEmpty(GenerateHelper.choiceDevice.G3eFid))
                        {
                            return oldValue;
                        }
                        return GenerateHelper.choiceDevice.DeviceSbmc;
                    }
                    else
                    {
                        MessageBox.Show("当前设备所填属性值是根据受电馈线过滤而得.", "提示", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        return oldValue;
                    }
                }

            }
            return oldValue;
        }

        public object GetChooseValueByFnoArray(IServiceProvider provider, ITypeDescriptorContext context, int[] fno,
            object oldValue)
        {
            IWindowsFormsEditorService edSvc =
                (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
            if (edSvc != null)
            {
                var pd = context.Instance;
                if (pd != null)
                {
                    XProps xprops = (XProps) pd;
                    var ssxl = GenerateHelper.GetFirstXPropOfHasValue(xprops, "CD_SSXL");
                    if (ssxl != null && !string.IsNullOrEmpty(ssxl.Value.ToString()))
                    {
                        SpecialDeviceList DevCol = new SpecialDeviceList(fno, ssxl.Value.ToString());
                        if (SpecialDeviceList.devCount <= 0)
                        {
                            MessageBox.Show("当前搜索设备个数为：" + SpecialDeviceList.devCount, "提示", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return oldValue;
                        }

                        Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(DevCol);
                        if (string.IsNullOrEmpty(GenerateHelper.choiceDevice.G3eFid))
                        {
                            return oldValue;
                        }
                        return GenerateHelper.choiceDevice.DeviceSbmc;
                    }
                    else
                    {
                        MessageBox.Show("当前设备所填属性值是根据受电馈线过滤而得.", "提示", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        return oldValue;
                    }

                }

            }
            return oldValue;
        }
    }
    /// <summary>
    /// 所属支线
    /// </summary>
    public class SszxList : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
                return UITypeEditorEditStyle.Modal;
            return base.GetEditStyle(context);
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return AutoGenerationHelper.Instance.GetChooseValueByFnoArray(provider, context, new int[] { 141, 140 }, value);
#region
//IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
//            if (edSvc != null)
//            {
//                var pd = context.Instance;
//                if (pd != null)
//                {
//                    XProps xprops = (XProps) pd;
//                    if (GenerateHelper.HasPropertyName(xprops, "CD_SSXL"))
//                    {
//                        var ssxl = GenerateHelper.GetPropertyValue(xprops, "CD_SSXL");
//                        if (ssxl != null && !string.IsNullOrEmpty(ssxl.ToString()))
//                        {
//                            SpecialDeviceList DevCol = new SpecialDeviceList(new int[] { 141, 140 }, ssxl.ToString());
//                            if (SpecialDeviceList.devCount <= 0)
//                            {
//                                MessageBox.Show("当前搜索设备个数为：" + SpecialDeviceList.devCount, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                                return value;
//                            }

//                            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(DevCol);
//                            if (string.IsNullOrEmpty(GenerateHelper.choiceDevice.G3eFid))
//                            {
//                                return value;
//                            }
//                            return GenerateHelper.choiceDevice.DeviceSbmc;
//                        }
//                    }
//                }
              
//            }
//            return value;
#endregion
            
        }
    }
    /// <summary>
    /// 所属变压器
    /// </summary>
    public class SsbyqList : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                return UITypeEditorEditStyle.Modal;
            }
            return base.GetEditStyle(context);
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            return AutoGenerationHelper.Instance.GetChooseValue(provider, context, 148, value);
        }
    }
    /// <summary>
    /// 所属台架
    /// </summary>
    public class SstjList : UITypeEditor
    {

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {

            return UITypeEditorEditStyle.Modal;

        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return AutoGenerationHelper.Instance.GetChooseValue(provider, context, 199, value);
#region

            //var edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            //if (edSvc != null)
            //{
            //      var pd = context.Instance;
            //    if (pd != null)
            //    {
            //        XProps xprops = (XProps) pd;
            //        if (GenerateHelper.HasPropertyName(xprops, "CD_SSXL"))
            //        {
            //            var ssxl = GenerateHelper.GetPropertyValue(xprops, "CD_SSXL");
            //            if (ssxl != null && !string.IsNullOrEmpty(ssxl.ToString()))
            //            {
            //                var DevCol = new SpecialDeviceList(199, ssxl.ToString());
            //                if (SpecialDeviceList.devCount <= 0)
            //                {
            //                    MessageBox.Show("当前搜索设备个数为：" + SpecialDeviceList.devCount, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                    return value;
            //                }
            //                Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(DevCol);
            //                if (string.IsNullOrEmpty(GenerateHelper.choiceDevice.G3eFid))
            //                {
            //                    return value;
            //                }
            //                return GenerateHelper.choiceDevice.DeviceSbmc;
            //            }
            //        }
            //    }
            //}
            //return value;
#endregion
        }
    }
    /// <summary>
    /// 所属电房
    /// </summary>
    public class SsdfList : UITypeEditor
    {

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {

            return UITypeEditorEditStyle.Modal;

        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            return AutoGenerationHelper.Instance.GetChooseValueByFnoArray(provider, context, new int[] { 149, 142, 163 }, value);
            #region
        //IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            //if (edSvc != null)
            //{
            //    SpecialDeviceList DevCol = new SpecialDeviceList(new int[]{149,142,163}, SsxlList.str);
            //    if (SpecialDeviceList.devCount <= 0)
            //    {
            //        MessageBox.Show("当前搜索设备个数为：" + SpecialDeviceList.devCount, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return value;
            //    }
            //    Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(DevCol);
            //    if (string.IsNullOrEmpty(GenerateHelper.choiceDevice.G3eFid))
            //    {
            //        return value;
            //    }
            //    return GenerateHelper.choiceDevice.DeviceSbmc;
            //}
            //return value;
            #endregion
    

        }
    }

    /// <summary>
    /// 所属开关柜
    /// </summary>
    public class SskggList : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;

        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            return AutoGenerationHelper.Instance.GetChooseValue(provider, context, 198, value);
            #region
            //IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            //if (edSvc != null)
            //{
            //    SpecialDeviceList DevCol = new SpecialDeviceList(198, SsxlList.str);
            //    if (SpecialDeviceList.devCount <= 0)
            //    {
            //        MessageBox.Show("当前馈线下搜索到开关柜个数为：" + SpecialDeviceList.devCount + "\n请查看开关柜管理或馈线设置！！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return value;
            //    }
            //    Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(DevCol);
            //    if (string.IsNullOrEmpty(GenerateHelper.choiceDevice.G3eFid))
            //    {
            //        return value;
            //    }
            //    return GenerateHelper.choiceDevice.DeviceSbmc;
            //}
            //return value;
            #endregion
          

        }
    }
    /// <summary>
    /// 所属杆塔
    /// </summary>
    public class SsgtList : UITypeEditor
    {

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;

        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            return AutoGenerationHelper.Instance.GetChooseValue(provider, context, 201, value);
            #region
            //IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            //if (edSvc != null)
            //{
            //    SpecialDeviceList DevCol = new SpecialDeviceList(201, SsxlList.str);
            //    if (SpecialDeviceList.devCount <= 0)
            //    {
            //        MessageBox.Show("当前搜索设备个数为：" + SpecialDeviceList.devCount, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return value;
            //    }
            //    Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(DevCol);
            //    if (string.IsNullOrEmpty(GenerateHelper.choiceDevice.G3eFid))
            //    {
            //        return value;
            //    }
            //    return GenerateHelper.choiceDevice.DeviceSbmc;
            //}
            //return value;
            #endregion


        }
    }

    public class NoEditConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }
    }
    /// <summary>
    /// 允许编辑多行文本====
    /// </summary>
    public class RichText : UITypeEditor
    {
        public static string strSBMC { get; set; }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
           
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                MultilineInPut mulDal = new MultilineInPut(){inputText = value!=null?value.ToString():null};
                Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(mulDal);
                string temp = strSBMC;
                strSBMC = string.Empty;
                return temp;
            }
            return value;

        }

    }
    /// <summary>
    /// 禁止编辑项
    /// </summary>
    public class NoEditor : ExpandableObjectConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
      
    }
    /// <summary>
    /// 让某项变灰
    /// </summary>
    public class EditorToGray : ExpandableObjectConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {

            return false;
        }
        //public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        //{
        //    return false;
        //}
    }
    public class DateItem : UITypeEditor
    {
        MonthCalendar dateControl = new MonthCalendar();
        IWindowsFormsEditorService edSvc = null;
        public DateItem()
        {
            dateControl.MaxSelectionCount = 1;
            //dateControl.MouseDown += new MouseEventHandler(dateControl_MouseDown);

        }

        //void dateControl_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        edSvc.CloseDropDown();
        //    }
        //}
        //public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        //{
        //    return UITypeEditorEditStyle.None ;
        //}
        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return false;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            try
            {
                if (edSvc == null)
                    edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                if (value == null)
                {
                    value = DateTime.Today;
                }
                if (value is string)
                {
                    dateControl.SelectionStart = DateTime.Parse(value as string);
                    string str = dateControl.SelectionStart.ToShortDateString();
                    edSvc.DropDownControl(dateControl);
                    value = dateControl.SelectionStart;
                    return value;
                }
                if (value is DateTime)
                {
                    dateControl.SelectionStart = (DateTime)value;
                    edSvc.DropDownControl(dateControl);
                    value = dateControl.SelectionStart;
                    return value;
                }
            }
            catch
            {
                return value;
            }
            return value;
        }

    }
}
 