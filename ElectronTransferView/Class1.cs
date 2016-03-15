using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Specialized;
namespace VetoTest
{
    public class VetoCmds 
    {
        static StringCollection cmdList =
          new StringCollection();

        [CommandMethod("STRVE")]
        public void STRVE()
        {
            DocumentCollection dm =
              Application.DocumentManager;
            dm.DocumentLockModeChanged += new
              DocumentLockModeChangedEventHandler(
                vetoCommandIfInList
              );
        }

        [CommandMethod("ENDVE")]
        public void ENDVE()
        {
            DocumentCollection dm;
            dm = Application.DocumentManager;
            dm.DocumentLockModeChanged -= new
              DocumentLockModeChangedEventHandler(
                vetoCommandIfInList
              );
        }
        [CommandMethod("ADDVETO")]
        public void AddVeto()
        {
            Editor ed =
              Application.DocumentManager.MdiActiveDocument.Editor;
            PromptResult pr =
              ed.GetString("\nEnter command to veto: ");
            if (pr.Status == PromptStatus.OK)
            {
                if (cmdList.Contains(pr.StringResult.ToUpper()))
                {
                    ed.WriteMessage(
                      "\nCommand already in veto list."
                    );
                }
                else
                {
                    cmdList.Add(pr.StringResult.ToUpper());
                    ed.WriteMessage(
                      "\nCommand " +
                      pr.StringResult.ToUpper() +
                      " added to veto list.");
                }
            }
        }
        [CommandMethod("LISTVETOES")]
        public void ListVetoes()
        {
            Editor ed =
              Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("Commands currently on veto list:\n");
            foreach (string str in cmdList)
            {
                ed.WriteMessage(str + "\n");
            }
        }
        [CommandMethod("REMVETO")]
        public void RemoveVeto()
        {
            ListVetoes();
            Editor ed =
              Application.DocumentManager.MdiActiveDocument.Editor;
            PromptResult pr;
            pr =
              ed.GetString(
                "Enter command to remove from veto list: "
              );
            if (pr.Status == PromptStatus.OK)
            {
                if (cmdList.Contains(pr.StringResult.ToUpper()))
                {
                    cmdList.Remove(pr.StringResult.ToUpper());
                }
                else
                {
                    ed.WriteMessage(
                      "\nCommand not found in veto list."
                    );
                }
            }
        }
        private void vetoCommandIfInList(
          object sender,
          DocumentLockModeChangedEventArgs e)
        {
            if (cmdList.Contains(e.GlobalCommandName.ToUpper()))
            {
                e.Veto();
            }
        }
    }
}