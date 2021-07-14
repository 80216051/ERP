using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using BaseLibrary.Threadlib;

namespace BaseLibrary
{
    public class LogMsg
    {
        #region 常量定义
        /// <summary>
        /// 程序目录名
        /// (一般根据需要修改此变量值即可)
        /// </summary>
        private const string FolderName = "EventLog";

        /// <summary>
        /// 日志文件名
        /// (全名或后缀名,按日分类时为后缀名)
        /// </summary>
        private const string logFileName = ".Log";
        #endregion

        #region 构造函数
        /// <summary>
        /// 私有构造函数,不允许直接实例化
        /// </summary>
        private LogMsg()
        {
            //
        }
        #endregion

        #region 记录错误日志到文本文件到我的文档目录

        /// <summary>
        /// 记录错误日志到文本文件到我的文档目录
        /// 按月分,每月产生一个日志文件
        /// </summary>
        /// <param name="text">日志内容</param>
        static public void Log(string text)
        {
            string strPath = "/TempFiles/" + FolderName + "/" + System.DateTime.Now.ToString("yyyy-MM");
            string folderPath = Tool.CommonTool.GetMapPath(strPath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //格式化文件路径字符串
            string filePath = string.Format("{0}\\{1}{2}", folderPath, System.DateTime.Today.ToString("yyyy-MM-dd"), logFileName);

            LogToFile(filePath, text);
        }

        /// <summary>
        /// 队列写入捕获日志
        /// </summary>
        /// <param name="text"></param>
        public static void WriteLog(string text) {
            ExecCustomThread CustomThread = ExecCustomThread.GetInstrance();
            CustomThread.AddTask(new WriteLog() { log=text,TName="写入日志" });
        }

        /// <summary>
        /// 微信日志
        /// </summary>
        /// <param name="text">日志内容</param>
        static public void WXLog(string text)
        {
            string strPath = "/TempFiles/WXLog/" + System.DateTime.Now.ToString("yyyy-MM");
            string folderPath = Tool.CommonTool.GetMapPath(strPath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //格式化文件路径字符串
            string filePath = string.Format("{0}\\{1}{2}", folderPath, System.DateTime.Today.ToString("yyyy-MM-dd"), logFileName);

            LogToFile(filePath, text);
        }

        /// <summary>
        /// 队列写入微信日志
        /// </summary>
        /// <param name="text"></param>
        public static void WriteWXLog(string text)
        {
            ExecCustomThread CustomThread = ExecCustomThread.GetInstrance();
            CustomThread.AddTask(new WriteWXLog() { log=text,TName="写入微信日志"});
        }

        /// <summary>
        /// 写入全局日志
        /// </summary>
        /// <param name="text"></param>
        public static void WriteGloballLog(string text)
        {
            string strPath = "/TempFiles/GloballLog/" + System.DateTime.Now.ToString("yyyy-MM");
            string folderPath = Tool.CommonTool.GetMapPath(strPath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = string.Format("{0}\\{1}{2}", folderPath, System.DateTime.Today.ToString("yyyy-MM-dd"), logFileName);

            LogToFile(filePath, text);
        }

        /// <summary>
        /// 写入404日志
        /// </summary>
        /// <param name="text"></param>
        public static void Write404Log(string text)
        {
            string strPath = "/TempFiles/404Log/" + System.DateTime.Now.ToString("yyyy-MM");
            string folderPath = Tool.CommonTool.GetMapPath(strPath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = string.Format("{0}\\{1}{2}", folderPath, System.DateTime.Today.ToString("yyyy-MM-dd"), logFileName);

            LogToFile(filePath, text);
        }
        

        #endregion

        #region 记录文本到文本文件
        /// <summary>
        /// 记录文本到文本文件(根据微软MSDN2005帮助文档System.IO.File.AppendText()提供的示例修改)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="text">记录内容</param>
        static private void LogToFile(string filePath, string text)
        {
            //-------------------
            StreamWriter sw = null;
            try
            {
                if (!File.Exists(filePath))
                {
                    sw = File.CreateText(filePath);
                }
                else
                {
                    sw = File.AppendText(filePath);
                }

                //设置写入文件的文本
                string msg = string.Format("---------Log Time:{0}--------\r\n{1}", DateTime.Now.ToString(), text + "\r\n--------------------------------------------------------------------------------------\r\n");

                sw.WriteLine(msg);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
        #endregion
    }
}
