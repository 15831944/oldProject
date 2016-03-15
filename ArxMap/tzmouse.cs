using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.ApplicationServices;

namespace ArxMap
{
    class tzmouse
    {
        public void AddContextMenu()
        {
            ContextMenuExtension ce = new ContextMenuExtension();
            ce.Title = "台账管理";
            MenuItem mi1 = new MenuItem("属性修改");
            mi1.Click += new EventHandler(mi1_Click);
            MenuItem mi2 = new MenuItem("批量修改");
            mi2.Click += new EventHandler(mi2_Click);
            ce.MenuItems.Add(mi1);
            ce.MenuItems.Add(mi2);
            Autodesk.AutoCAD.ApplicationServices.Application.AddDefaultContextMenuExtension(ce);

        }
        void mi1_Click(object sender, EventArgs e)
        {
            ShowWindows();
        }
        void mi2_Click(object sender, EventArgs e)
        {
            ShowWindows();
        }
        public void ShowWindows()
        {
            elec form = new WinWeb();
            Application.ShowModelessDialog(form);
        }
    }
}
