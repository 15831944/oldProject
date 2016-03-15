using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using DrawOverrules;

namespace ElectronTransferDal.Cad
{
    public class DCadCopyOverrule : ObjectOverrule
    {
        public static DCadCopyOverrule theOverrule =
            new DCadCopyOverrule();


        public override DBObject DeepClone(DBObject dbObject, DBObject ownerObject,IdMapping idMap, bool isPrimary)
        {
            DBObject res = base.DeepClone(dbObject, ownerObject, idMap, isPrimary);
            foreach (IdPair ip in idMap)
            {
                var oldid = ip.Key;
                var newid = ip.Value;
                if (DBSymbolFinder.Instance.ContainsKey(oldid))
                {
                    //PublicMethod.PasteSelectSymbol(oldid, newid);
                }
            }
            if (PipeDrawOverrule.PipeRadiusForObject(res) > 0.0)
            {
                Transaction tr = dbObject.Database.TransactionManager.StartTransaction();
                using (tr)
                {
                    PipeDrawOverrule.SetPipeRadiusOnObject(tr, res, 0.0);
                    
                    tr.Commit();
                }
            }
            return res;
        }
    }
}
