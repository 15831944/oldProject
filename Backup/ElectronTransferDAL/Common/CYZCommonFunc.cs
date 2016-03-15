using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronTransferDal.Common;
using ElectronTransferModel.V9_4;
using ElectronTransferModel.Base;
using ElectronTransferModel;

namespace ElectronTransferDal.Common
{
    public class CYZCommonFunc
    {
        static int randid=0;
        public static int getid() {
            
            string ss = "";
            try {
                if (randid == 0)
                {
                    /*
                    //ss = Guid.NewGuid().ToString().Replace("-", "");
                    Random ra = new Random((int)DateTime.Now.Ticks);
                    //ss = ss[0 + ra.Next(4)].ToString() +
                    //    ss[4 + ra.Next(4)].ToString() +
                    //           ss[8 + ra.Next(4)].ToString() +
                    //           ss[12 + ra.Next(4)].ToString() +
                    //           ss[16 + ra.Next(4)].ToString() +
                    //           ss[20 + ra.Next(4)].ToString() +
                    //           ss[24 + ra.Next(4)].ToString() +
                    //           ss[28 + ra.Next(4)].ToString();

                    //randid = int.Parse(H16ToD10(ss)) + (ra.Next(5)+1)*100000000;
                    ss = DateTime.Now.Ticks.ToString();
                    randid = int.Parse((ss).Substring(ss.Length - 8, 8));
                    randid += (ra.Next(5) + 1) * 100000000; ;
                     */
                    //2147483648
                    //1231246000
                    Random ra = new Random((int)DateTime.Now.Ticks);
                    int i = ra.Next(50000000);
                    randid = int.Parse(DateTime.Now.ToString("MMddHHmm"))*100;
                    randid+= +i;
                }
                else 
                {
                    randid = randid + 1;
                }
            }
            catch (Exception ex) { }
            return randid;
        }

        static string H16ToD10(string _h16)
        {
            string s = "";
            try
            {
                for (int ii = 0; ii < _h16.Length; ii++) 
                {
                    s += H16ToD10(_h16[ii]);
                }
            }
            catch (Exception ex) { }
            return s;
        }
        static int H16ToD10(char _h16)
        {
            int i = 0;
            try
            {
                switch (_h16.ToString().ToUpper())
                {
                    case "A":
                        i = 1;
                        break;
                    case "B":
                        i = 2;
                        break;
                    case "C":
                        i = 3;
                        break;
                    case "D":
                        i = 4;
                        break;
                    case "E":
                        i = 5;
                        break;
                    case "F":
                        i = 6;
                        break;
                    default:
                        i = int.Parse(_h16.ToString());
                        break;
                }

            }
            catch (Exception ex) { }
            return i;
        }
 

        public static DBEntity GetModelEntity(long fid)
        {
            return DBManager.Instance.GetEntity<Common_n>(o => o.G3E_FID ==fid && o.Duplicated==false);
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="fno"></param>
        /// <returns></returns>
        public static bool getEntSate(long fid,long fno)
        {
            switch (fno)
            {
                case 145:
                    {
                        DBEntity FNOCOMPONET = DBManager.Instance.GetEntity<Gg_pd_zfllkg_n>(o => o.G3E_FID == fid);
	                    if (FNOCOMPONET.EntityState == EntityState.Insert)
	                    {
	                        return false;
	                    }
                        break;
                    }
                    
                case 152:
                    {
                        DBEntity FNOCOMPONET = DBManager.Instance.GetEntity<Gg_pd_dlq_n>(o => o.G3E_FID == fid);
                        if (FNOCOMPONET.EntityState == EntityState.Insert)
                        {
                            return false;
                        }
                        break;
                    }
                case 154:
                    {
                        DBEntity FNOCOMPONET = DBManager.Instance.GetEntity<Gg_pd_zsbyq_n>(o => o.G3E_FID == fid);
                        if (FNOCOMPONET.EntityState == EntityState.Insert)
                        {
                            return false;

                        }
                        break;
                    }
                case 159:
                    {
                        DBEntity FNOCOMPONET = DBManager.Instance.GetEntity<Gg_pd_cbx_n>(o => o.G3E_FID == fid);
                        if (FNOCOMPONET.EntityState == EntityState.Insert)
                        {
                            return false;

                        }
                        break;
                    }
                case 160:
                    {

                        DBEntity FNOCOMPONET = DBManager.Instance.GetEntity<Gg_pd_dyshb_n>(o => o.G3E_FID == fid);
                        if (FNOCOMPONET.EntityState == EntityState.Insert)
                        {
                            return false;

                        }
                        break;
                    }
                default:
                    {
                        DBEntity FIDCommon = DBManager.Instance.GetEntity<Common_n>(o => o.G3E_FID == fid);
                        if (FIDCommon.EntityState == EntityState.Insert)
                        {
                            return false;
                        }
                        break;
                    }
            }
            return true;
        }
    }
}
