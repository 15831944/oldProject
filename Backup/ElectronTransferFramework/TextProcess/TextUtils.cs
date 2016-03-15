using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections;

namespace ElectronTransferFramework.TextProcess
{
    public static class TextUtils
    {
        /// <summary>
        /// 把一个字符串中的 低序位 ASCII 字符 替换成 &#x  字符
        /// 转换  ASCII  0 - 8  -> &#x0 - &#x8
        /// 转换  ASCII 11 - 12 -> &#xB - &#xC
        /// 转换  ASCII 14 - 31 -> &#xE - &#x1F
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static string ReplaceLowOrderASCIICharacters(this string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat("&#x{0:X};", ss);
                else info.Append(cc);
            }
            return info.ToString();
        }

        /// <summary>
        /// 把一个字符串中的下列字符替换成 低序位 ASCII 字符
        /// 转换  &#x0 - &#x8  -> ASCII  0 - 8
        /// 转换  &#xB - &#xC  -> ASCII 11 - 12
        /// 转换  &#xE - &#x1F -> ASCII 14 - 31
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetLowOrderASCIICharacters(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            int pos, startIndex = 0, len = input.Length;
            if (len <= 4) return input;

            StringBuilder result = new StringBuilder();
            while ((pos = input.IndexOf("&#x", startIndex)) >= 0)
            {
                bool needReplace = false;
                string rOldV = string.Empty, rNewV = string.Empty;

                int le = (len - pos < 6) ? len - pos : 6;
                int p = input.IndexOf(";", pos, le);

                if (p >= 0)
                {
                    rOldV = input.Substring(pos, p - pos + 1);

                    // 计算 对应的低位字符
                    short ss;
                    if (short.TryParse(rOldV.Substring(3, p - pos - 3), NumberStyles.AllowHexSpecifier, null, out ss))
                    {
                        if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                        {
                            needReplace = true;
                            rNewV = Convert.ToChar(ss).ToString();
                        }
                    }
                    pos = p + 1;
                }
                else pos += le;

                string part = input.Substring(startIndex, pos - startIndex);
                if (needReplace) result.Append(part.Replace(rOldV, rNewV));
                else result.Append(part);

                startIndex = pos;
            }
            result.Append(input.Substring(startIndex));
            return result.ToString();
        }
        public static byte[] Inverse(this byte[] bytes)
        {
            var result = new byte[bytes.Length];
            BitArray bitArray = new BitArray(bytes);
            bitArray.Not();
            bitArray.CopyTo(result, 0);
            return result;
        }
    }
}
