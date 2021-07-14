using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.Web;
using System.IO;
using BaseLibrary;
using BaseLibrary.Threadlib;

namespace Utility.Other
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public class EncryptService
    {
        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string GetMD5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            inputBye = Encoding.GetEncoding("utf-8").GetBytes(encypStr);

            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "");

            return retStr;
        }

        /// <summary>
        /// 获取大写的MD5签名结果
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string Getsha512(string encypStr)
        {
            string retStr="";
            SHA512CryptoServiceProvider SHA512CSP= new SHA512CryptoServiceProvider();
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(encypStr);
            byte[] bytHash = SHA512CSP.ComputeHash(bytValue);
            SHA512CSP.Clear();
            SHA512CSP.Dispose();//释放当前实例使用的所有资源
            String sHash = BitConverter.ToString(bytHash);//将运算结果转为string类型
            retStr = sHash.Replace("-", "");//替换
            return retStr;
        }

        /// <summary>
        /// 生成一个随机的加密的盐值
        /// </summary>
        /// <param name="size">盐值长度（必须大于2且是2的倍数）</param>
        /// <returns>16C643F9C5B4A6EAB461</returns>
        public static string getRandomSalt(int size)
        {
            try {
                RandomNumberGenerator rng = new RNGCryptoServiceProvider();
                byte[] saltBytes = new byte[size / 2];
                rng.GetBytes(saltBytes);//随机
                String sHash = BitConverter.ToString(saltBytes);//将运算结果转为string类型
                sHash = sHash.Replace("-", "");//替换
                return sHash;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 考无忧特有的加密方式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetKaoWuYouMD5(string str)
        {
            str = str.Replace("-", "");
            string retStr = "";
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            inputBye = Encoding.GetEncoding("utf-8").GetBytes(str);

            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            retStr += "www.EasyKao.net*&^www.EasyKao.cn";
            return retStr.ToUpper().Substring(0, 5) + "-" + retStr.ToUpper().Substring(5, 5) + "-" + retStr.ToUpper().Substring(10, 5) + "-" + retStr.ToUpper().Substring(15, 5);
        }

        /// <summary>
        /// 通过使用 new 运算符创建对象
        /// </summary>
        /// <param name="Parmas">需要加密的明文</param>
        /// <param name="CodeStyle">编码方式</param>
        /// <returns>string 字符串</returns>
        public string GetMd5Str(string Parmas, string CodeStyle)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //使用指定的编码方式把明文字符串转换为密文字节数组
            byte[] inputtStr = System.Text.Encoding.GetEncoding(CodeStyle).GetBytes(Parmas);
            //进行MD5加密
            byte[] EncryptStr = md5.ComputeHash(inputtStr);
            //转化成字符串 32位
            string strReturn = BitConverter.ToString(EncryptStr);
            strReturn = strReturn.Replace("-", "");
            return strReturn;
        }

        /// <summary>
        /// 获取对称密钥
        /// </summary>
        /// <returns></returns>
        private static string GetDescKey()
        {
            string key = "";
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objCache["DescKey"] != null)
            {
                key = objCache["DescKey"].ToString();
            }
            else
            {
                key = ConfigurationManager.AppSettings["DescKey"];
                objCache.Insert("DescKey", key);
            }
            return key;
        }


        /// <summary> 
        /// 对称加密
        /// </summary> 
        /// <param name="Text"></param> 
        /// <returns></returns> 
        public static string DESEncrypt(string Text)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray;
                inputByteArray = Encoding.Default.GetBytes(Text);
                des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(GetDescKey(), "md5").Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(GetDescKey(), "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:X2}", b);
                }
                return ret.ToString();
            }
            catch { return Text; }

        }

        /// <summary> 
        /// 对称解密
        /// </summary> 
        /// <param name="Text"></param> 
        /// <returns></returns> 
        public static string DESDecrypt(string Text)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len;
                len = Text.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(GetDescKey(), "md5").Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(GetDescKey(), "md5").Substring(0, 8));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.Default.GetString(ms.ToArray());
            }
            catch { return Text; }

        }

        /// <summary>
        /// MD5 16位加密
        /// </summary>
        /// <param name="encypStr">加密字符</param>
        /// <param name="charset">编码类型 </param>
        /// <returns></returns>
        public static string GetMd5Unit16(string encypStr, string charset)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            Encoding encod;
            try
            {
                encod = Encoding.GetEncoding(charset);
            }
            catch
            {
                encod = Encoding.GetEncoding("utf-8");
            }
            byte[] inputbye = encod.GetBytes(encypStr);
            byte[] outputbye = md5.ComputeHash(inputbye);
            string t2 = BitConverter.ToString(outputbye, 4, 8);
            inputbye = null;
            outputbye = null;
            t2 = t2.Replace("-", "");
            return t2;
        }

        /// <summary>
        /// 获取文件MD5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                //FileStream file = new FileStream(fileName, FileMode.Open);
                FileStream file = new FileStream(fileName, FileMode.Open,FileAccess.Read, FileShare.ReadWrite);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return "";
            }
        }
    }
}
