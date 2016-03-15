using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferFramework;
using ElectronTransferDal.AutoGeneration;
using ElectronTransferModel.V9_4;
using ElectronTransferModel.Base;

namespace ElectronTransferDal.HelperEventArgs
{
    public class VerifyRuleEventArgs:ValueEventArgs
    {
        /// <summary>
        /// 保存错误信息
        /// </summary>
        public StringBuilder  ErrorMsg { get; set; }
        /// <summary>
        /// 记录校验对象信息
        /// </summary>
        public RequiredDevTables DevTables { get; set; }
        /// <summary>
        /// 是否已近过了工厂校验库
        /// </summary>
        public bool IsUseFactoryVerify { get; set; }

    }
}
