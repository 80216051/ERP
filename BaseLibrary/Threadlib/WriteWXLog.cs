using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLibrary.Threadlib
{
    /// <summary>
    /// 写入微信日志任务
    /// </summary>
    public class WriteWXLog : BaseTask
    {
        /// <summary>
        /// 写入内容
        /// </summary>
        public string log;
        public WriteWXLog()
        {
        }

        public override void Run()
        {
            LogMsg.WXLog(log);
        }
    }
}
