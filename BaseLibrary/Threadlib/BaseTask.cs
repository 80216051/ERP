using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLibrary.Threadlib
{
    /// <summary>
    /// 线程模型执行任务 基类
    /// </summary>
    public abstract class BaseTask
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TID { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string TName { get; set; }

        /// <summary>
        /// 线程模型任务
        /// </summary>
        public abstract void Run();
    }
}
