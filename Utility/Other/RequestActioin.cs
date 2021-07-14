using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using BaseLibrary;
using BaseLibrary.Threadlib;

namespace Utility.Other
{
    public static class RequestActioin
    {
        /// <summary>
        /// Post 提交调用抓取
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="param">参数</param>
        /// <returns>string</returns>
        public static string webRequestPostzhua(string url, string param)
        {
            string responseFromServer = "";
            try
            {
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(param);

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "Post";
                req.Timeout = 120 * 1000;
                req.ContentType = "application/x-www-form-urlencoded;";
                req.ContentLength = bs.Length;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Flush();
                }
                using (WebResponse wr = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理 

                    Stream strm = wr.GetResponseStream();

                    StreamReader sr = new StreamReader(strm, System.Text.Encoding.UTF8);

                    string line;

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line + System.Environment.NewLine);
                    }
                    sr.Close();
                    strm.Close();
                    responseFromServer=sb.ToString();
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
            }
            return responseFromServer;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string webRequestPost(string url)
        {
            string responseFromServer = "";

            try
            {
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Clean up the streams and the response.
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
            }

            return responseFromServer;
        }

        /// <summary>
        /// GET提交调用
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="param">参数</param>
        /// <param name="Timeout">超时时间</param>
        /// <returns>string</returns>
        public static string webRequestGet(string url, string param, int Timeout)
        {
            string responseFromServer = "";
            try
            {
                url += "?" + param;
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "GET";
                req.Timeout = Timeout * 1000;
                req.ContentType = "application/x-www-form-urlencoded;";

                using (WebResponse wr = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理 

                    Stream strm = wr.GetResponseStream();

                    StreamReader sr = new StreamReader(strm, System.Text.Encoding.UTF8);

                    string line;

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line + System.Environment.NewLine);
                    }
                    sr.Close();
                    strm.Close();
                    responseFromServer = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
            }
            return responseFromServer;
        }




    }
}
