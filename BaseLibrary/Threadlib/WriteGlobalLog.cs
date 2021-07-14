using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BaseLibrary;

namespace BaseLibrary.Threadlib
{
    /// <summary>
    /// 写入全局日志任务
    /// </summary>
    public class WriteGlobalLog : BaseTask
    {
        public HttpContext context;
        public WriteGlobalLog(HttpContext CurContext)
        {
            context = CurContext;
        }

        public override void Run()
        {
            Exception ex = context.Server.GetLastError();

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("发生时间:" + System.DateTime.Now.ToString() + "\n");
            sBuilder.Append("发生异常页: " + context.Request.Url.ToString() + "\n");
            sBuilder.Append("异常信息: " + ex.Message + "\n");
            sBuilder.Append("错误源:" + ex.Source + "\n");
            sBuilder.Append("堆栈信息:" + ex.StackTrace + "\n");

            //排除404错误，404不记录
            Exception exp = ex.GetBaseException();
            if (exp.GetType().Name.Equals("HttpException"))
            {
                if (404 == ((HttpException)exp).GetHttpCode())
                {
                    LogMsg.Write404Log(sBuilder.ToString());
                }
                else
                {
                    LogMsg.WriteGloballLog(sBuilder.ToString());
                }
            }
            else
            {
                LogMsg.WriteGloballLog(sBuilder.ToString());
            }
        }
    }
}
