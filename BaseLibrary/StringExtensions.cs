using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLibrary
{
    /// <summary>
    /// 对于String类的拓展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 只替换第一个
        /// </summary>
        /// <param name="value">要操作的字符串</param>
        /// <param name="oldValue">被替换的字符串</param>
        /// <param name="newValue">要替换的字符串</param>
        /// <returns></returns>
        public static string ReplaceFirst(this String value, string oldValue, string newValue)
        {
            //先找出位置
            int index = value.IndexOf(oldValue);
            //取位置前部分+替换字符串+位置（加上查找字符长度）后部分
            string newstr = value.Substring(0, index) + newValue + value.Substring(index + oldValue.Length);
            return newstr;
        }

        /// <summary>
        /// 只替换最后一个
        /// </summary>
        /// <param name="value">要操作的字符串</param>
        /// <param name="oldValue">被替换的字符串</param>
        /// <param name="newValue">要替换的字符串</param>
        /// <returns></returns>
        public static string ReplaceEnd(this String value, string oldValue, string newValue)
        {
            //先找出最后一个的位置
            int index = value.LastIndexOf(oldValue);
            string newstr = value.Substring(0,index)+newValue+value.Substring(index+oldValue.Length);
            return newstr;
        }
    }
}
