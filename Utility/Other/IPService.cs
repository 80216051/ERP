using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using BaseLibrary.Tool;

namespace Utility.Other
{
    /// <summary>
    /// IP操作类
    /// </summary>
    public class IPService
    {
        /// <summary>
        /// 获取客户的IP地址
        /// </summary>
        /// <returns>客户的IP地址</returns>
        public static string GetIPAddress()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            return result;
        }

        /// <summary>
        /// IP解析
        /// </summary>
        /// <param name="IPAdress"></param>
        /// <returns></returns>
        public static string GetIPMind(object IPAdress)
        {
            try
            {
                QQWryLocator qqsry = new QQWryLocator(CommonTool.GetMapPath("/config/qqwry.dat"));
                IPLocation ip = qqsry.Query(IPAdress.ToString());
                return ip.Country + "-" + ip.Local;
            }
            catch (Exception)
            {
                return "";
            }
        }

  
    }
}
