using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ElectronTransferDal.Common;
using ElectronTransferDal;
using ElectronTransferDal.AutoGeneration;

namespace ElectronTransferDal.AutoGeneration
{
    public static class CheckInput
    {

        /// <summary>
        /// 验证输入是否是纯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_Number(string str)
        {
            //只能输入数字
            Regex reg = new Regex(@"^[-]?\d+[.]?\d*$");
            return reg.IsMatch(str);
        }
        /// <summary>
        /// 验证输入是否是纯字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_Character(string str)
        {
            //只能输入英文
            Regex reg = new Regex(@"^[a-zA-Z]+$");
            return reg.IsMatch(str);
        }
        /// <summary>
        /// 验证输入是否是纯汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_Chinese(string str)
        {
            //只能输入汉字
            Regex reg = new Regex(@"^[\u4e00-\u9fa5]+$");
            return reg.IsMatch(str);
        }
        /// <summary>
        /// 验证输入是否是纯字母加纯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_CharacterNumber(string str)
        {
            Regex reg = new Regex(@"^[a-zA-Z0-9]+$");
            return reg.IsMatch(str);
        }
        /// <summary>
        /// 验证输入非汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_NoChinese(string str)
        {
            String ss = Regex.Replace(str, @"[^\u4e00-\u9fa5]+", "");
            Regex reg = new Regex(@"^[\u4e00-\u9fa5]+$");
            return reg.IsMatch(ss);
        }
        /// <summary>
        /// 验证输入只能有*,+,[0-9]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_Special(string str)
        {
            String ss = Regex.Replace(str, @"[\*\+]", "");
            Regex reg = new Regex(@"^(-?[0-9]*[.]*[0-9]{0,3})$");
            return reg.IsMatch(ss);
        }
        /// <summary>
        /// 验证输入只能有#,[0-9]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_gh(string str)
        {
            Regex reg = new Regex(@"^[\#](-?[0-9]*[.]*[0-9]{0,3})$");
            return reg.IsMatch(str);
        }
        /// <summary>
        /// 验证输入只能有/./[0-9]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_DigitAndCharacter(string str)
        {
            //String ss = Regex.Replace(str, @"[\\.]", "");
            Regex reg = new Regex(@"^[0-9](-?[0-9]*[.]*[0-9]{0,3})$");
            return reg.IsMatch(str);
        }
        /// <summary>
        /// 匹配是否是特殊字符
        /// </summary>
        /// <param name="str">输入值</param>
        /// <returns>包含不特殊字符返回false否则返回true</returns>
        public static bool IsSpecialCharacter(string str)
        {
            Regex reg = new Regex("^#");
            return reg.IsMatch(str);
        }
      
        /// <summary>
        /// 匹配小数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNatural_Decimal(string str)
        {
            Regex reg = new Regex(@"^\d+[.]\d+[,， ]\d+[.]\d+$");
            return reg.IsMatch(str);
        }
    

    }
}
