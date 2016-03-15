using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;
using ElectronTransferModel.Geo;

namespace ElectronTransferDal.Cad
{
    public class MoveSymbolEventArgs:ValueEventArgs
    {
        /// <summary>
        /// 点设备坐标
        /// </summary>
        public Multipoint multipointValue { set; get; }
        /// <summary>
        /// 线设备坐标
        /// </summary>
        public LineString lineStringValue { set; get; }
    }
}
