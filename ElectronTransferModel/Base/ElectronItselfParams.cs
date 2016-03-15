using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.Base
{
    /// <summary>
    /// 所有设备的自身属性基类
    /// </summary>
    public class ElectronItselfParams : ElectronBase
    {
        /// <summary>
        /// 电压等级
        /// </summary>
        public string BaseVoltage { get; set; }
        /// <summary>
        /// 运行状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 资产属性
        /// </summary>
        public string AssetClassification { get; set; }


        public ElectronItselfParams() 
        {
            //gtype = egtype.none;
        }



      
        
    }
}
