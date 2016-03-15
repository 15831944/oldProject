using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ElectronTransferModel.Geo;

namespace ElectronTransferModel.Base
{
    /// <summary>
    /// 所有设备的标注基类
    /// </summary>
    public class ElectronLabel : ElectronBase
    {
        public int g3e_alignment { get; set; }
        
        public Geometry g3e_geometry { get; set; }
        

        

        public ElectronLabel() 
        {
            //gtype = egtype.point;
        }
        
    }
}
