using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElectronTransferDAL.cMath
{
    public class Rotate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public double getrotate(double x,double y)
        {
            try
            {
                if((x*x+y*y)<0.0001)
                {
                    return 0.0;
                }
                if(x==0.0)
                {
                    if(y>0)
                    {
                        return 90.0;
                    }
                    else
                    {
                        return 270.0;
                    }
                }
                if(y==0.0)
                {
                    if(x>0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 180.0;
                    }
                }
                if(x>0)
                {
                    return Math.Atan2(y, x);
                }
                if(x<0)
                {
                    return Math.Atan2(y, -x);
                }
            }
            catch(Exception ex)
            {
            }
            return 0.0;
        }


    }
}
