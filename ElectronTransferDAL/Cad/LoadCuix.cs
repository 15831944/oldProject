using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Customization;
using System.Collections.Specialized;
using System.Data;
using Autodesk.AutoCAD.Runtime;

namespace ElectronTransferDal.Cad
{
    public class LoadCuix
    {
        //贴一个卸载自定义的cuix的方法和一个创建cuix的方法：
        [CommandMethod("tcui1")]
        public void UnloadGUI()
        {
            try
            {
                string strCuiFile = "123";
                string mainCuiFile = (string)Application.GetSystemVariable("MENUNAME") + ".cuix";
                CustomizationSection cs = new CustomizationSection(mainCuiFile, true);
                CustomizationSection partCUI = null;
                bool has = false;
                foreach (string f in cs.PartialCuiFiles)
                {
                    if (f.ToLower() == strCuiFile.ToLower())
                    {
                        has = true;
                        partCUI = new CustomizationSection(strCuiFile);
                        break;
                    }
                }
                if (has)
                {
                    cs.RemovePartialMenu(partCUI);
                    cs.Save();
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("->UnloadGUI:" + ex.Message);
            }
        }

        [CommandMethod("tcui2")]
        public void BuildCUI(string strMenuGroupName, string strCuiFile, System.Data.DataTable dtMacroGroup, System.Data.DataTable dtPopMenu)
        {
            try
            {
                // 为我们的局部菜单创建一个自定义区域（customization section）
                CustomizationSection pcs = new CustomizationSection();
                pcs.MenuGroupName = strMenuGroupName;

                // 添加菜单的命令组
                MacroGroup mg = new MacroGroup(dtMacroGroup.TableName, pcs.MenuGroup);
                //  pcs.MenuGroup.MacroGroups.Add(mg);
                MenuMacro mm = null;
                foreach (System.Data.DataRow r in dtMacroGroup.Rows)
                {
                    mm = mg.CreateMenuMacro(r["name"].ToString(), r["command"].ToString(), r["UID"].ToString());
                    mm.macro.HelpString = r["HelpString"].ToString();
                    // mg.AddMacro(mm);
                }
                //添加菜单项的设置
                StringCollection sc = new StringCollection();
                sc.Add("POP1");
                PopMenu pm = new PopMenu(dtPopMenu.TableName, sc, dtPopMenu.Namespace, pcs.MenuGroup);
                pcs.MenuGroup.PopMenus.Add(pm);
                PopMenuItem pmi = null;

                foreach (DataRow r in dtPopMenu.Rows)
                {
                    // 添加下拉菜单
                    if (Convert.ToBoolean(r["IsSeparator"]))
                    {
                        pmi = new PopMenuItem(pm);     //用此构造方法，即为分割条             
                        pm.PopMenuItems.Add(pmi);
                    }
                    else
                    {
                        foreach (MenuMacro m in mg.MenuMacros)
                        {
                            if (m.ElementID == r["MenuMacroID"].ToString())
                            {
                                pmi = new PopMenuItem(m, r["NameRef"].ToString(), pm, -1);
                                pm.PopMenuItems.Add(pmi);
                                break;
                            }

                        }
                    }
                }
                // 最后保存文件
                pcs.SaveAs(strCuiFile);

            }
            catch (System.Exception ex)
            {
                throw new System.Exception("->BuildMenuCUI:" + ex.Message);
            }
        }
    }
}
