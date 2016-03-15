using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferDal.QueryVerifyHelper
{
    /// <summary>
    /// 校验结果状态
    /// </summary>
    public enum VerifyRes
    {
        /// <summary>
        /// 属性数据缺失
        /// </summary>
        AttributeDefect=0,
        /// <summary>
        /// 拓扑关系数据缺失
        /// </summary>
        TopologyDefect=1,
        /// <summary>
        /// 台账数据缺失
        /// </summary>
        TzDefect,
        /// <summary>
        /// 未同步台账数据
        /// </summary>
        NoSyncTzData,
        /// <summary>
        /// 数据错误
        /// </summary>
        DataError,
        /// <summary>
        /// 校验异常
        /// </summary>
        VerifyExp,
        None,
    }
}
