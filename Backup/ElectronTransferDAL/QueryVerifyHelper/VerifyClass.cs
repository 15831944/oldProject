using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferDal.QueryVerifyHelper
{
    /// <summary>
    /// 校验类
    /// </summary>
    public class VerifyClass:IComparable
    {
        public int BH
        {
            get;
            set;
            
        }

        public long G3E_FID
        {
            get;
            set;
        }
        public string DeviceType
        {
            get;
            set;
        }

        public string SSBDZ { get; set; }
        public string SSXL { get; set; }
        public string SBMC
        {
            get;
            set;
        }
        public string DevState
        {
            get;
            set;
        }
        public string VerifyResult
        {
            get;
            set;
        }
        public int G3E_FNO { get; set; }



        #region IComparable 成员

        public int CompareTo(object obj)
        {
            if (obj is VerifyClass)
            {
                VerifyClass vc = obj as VerifyClass;
                return this.VerifyResult.CompareTo(vc.VerifyResult);
            }
            else
            {
                throw new ArgumentException("obj is not verifyClass");
            }
        }

        #endregion
    }
}
