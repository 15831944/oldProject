using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace ElectronTransferDal.Cad
{
    public class DCadLine : Polyline3d
    {
        public ulong g3e_fid { get; set; }

        public DCadLine(Poly3dType ptype, Point3dCollection ps, bool b) :
            base(ptype,ps,b)
        {
           
        
        }
    }
}
