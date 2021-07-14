using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLibrary.Threadlib
{
    /// <summary>
    /// 写入自定义捕获日志
    /// </summary>
    public class WriteLog : BaseTask
    {
        /// <summary>
        /// 写入内容
        /// </summary>
        public string log;
        public WriteLog() { }

        public override void Run()
        {
            LogMsg.Log(log);
        }
    }
}
