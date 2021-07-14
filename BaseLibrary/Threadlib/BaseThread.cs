using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BaseLibrary;

namespace BaseLibrary.Threadlib
{
    /// <summary>
    /// 自定义线程模型
    /// </summary>
    public abstract class BaseThread<T> where T : BaseTask
    {

        //通知一个或多个正在等待的线程已发生事件
        ManualResetEvent mre = new ManualResetEvent(false);
        //线程安全的队列
        System.Collections.Concurrent.ConcurrentQueue<T> cqueue = new System.Collections.Concurrent.ConcurrentQueue<T>();

        /// <summary>
        /// 自定义线程ID;
        /// </summary>
        public long TID { get; set; }

        public static bool IsRuning = true;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="tName">线程的名称</param>
        public BaseThread(string tName)
        {
            Thread _Thread = new Thread(Runing);
            _Thread.Name = tName;
            _Thread.Start();
        }

        /// <summary>
        /// 模拟新增任务
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(T task)
        {
            //添加任务到队列
            cqueue.Enqueue(task);
            //唤醒所有相关的挂起线程
            mre.Set();
        }

        void Runing()
        {
            //主循环 服务器运行标识
            while (IsRuning)
            {
                //如果是空则继续等待      服务器运行标识
                while (cqueue.IsEmpty && IsRuning)
                {
                    //重置线程暂停状态
                    mre.Reset();
                    //这个操作是以便服务器需要停止操作，
                    //如果停止调用线程的Thread.Abort()是会导致处理队列任务丢失
                    mre.WaitOne(2000);
                }
                T t;
                //取出队列任务
                if (cqueue.TryDequeue(out t))
                {
                    if (t != null)
                    {
                        Runing(t);
                    }
                }
            }
        }

        /// <summary>
        /// 设置运行方法为虚方法，方便子函数覆盖
        /// </summary>
        protected virtual void Runing(T run)
        {
            try
            {
                //执行任务
                run.Run();
            }
            catch (Exception)
            {

            }
        }
    }
}
