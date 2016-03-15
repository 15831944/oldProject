using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferModel.Geo;

namespace ElectronTransferModel.Base
{
    /// <summary>
    /// 所有与设备相关的基类
    /// </summary>
    /// 
    [Serializable]
    public abstract class ElectronBase : DBEntity
    {
        public virtual long G3E_ID { get; set; }
        [KeyField]
        public virtual long G3E_FID { get; set; }
        //[KeyField]
        public virtual int G3E_FNO { get; set; }
        ///// <summary>
        ///// 设备在表中唯一id
        ///// </summary>
        //public ulong g3e_id { get; set; }
        ///// <summary>
        ///// 设备类型代号
        ///// </summary>
        //public int g3e_fno { get; set; }
        ///// <summary>
        ///// 设备组件代号
        ///// </summary>
        //public int g3e_cno { get; set; }
        ///// <summary>
        ///// 设备在全局范围内的唯一id
        ///// </summary>
        //public ulong g3e_fid { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //public ulong g3e_cid { get; set; }


        ///// <summary>
        ///// 工单id
        ///// </summary>
        //public ulong ltt_id { get; set; }
        ///// <summary>
        ///// 工单状态
        ///// </summary>
        //public string ltt_status { get; set; }
        ///// <summary>
        ///// 工单时间
        ///// </summary>
        //public DateTime ltt_date { get; set; }
        ///// <summary>
        ///// 工单某属性...
        ///// </summary>
        //public ulong ltt_tid { get; set; }

        //public egtype gtype { get; set; }
        
    }

    //public enum egtype 
    //{
    //    none=0,
    //    point=1,
    //    line=2,
    //    polygon=3
    //}

}
