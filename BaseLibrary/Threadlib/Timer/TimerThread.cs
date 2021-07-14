using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BaseLibrary.Threadlib.Timer
{
    /// <summary>
    /// 定时执行的线程
    /// </summary>
    public class TimerThread
    {
        public TimerThread()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
            thread.Name = "Time_Thread";
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 任务队列
        /// </summary>
        private List<TimerTaskBase> taskQueue = new List<TimerTaskBase>();

        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="t"></param>
        public void AddTask(TimerTaskBase t)
        {
            if (t.IsStartAction)
            {
                //满足添加队列前先执行一次
                t.Run();
            }
            lock (taskQueue)
            {
                taskQueue.Add(t);
            }
        }

        public long GetDate()
        {
            return Convert.ToInt64(System.DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }

        //这里的线程同步器，不是用来通知的，
        //只是用来暂停的，因为Thread.Sleep() 消耗开销比较大
        ManualResetEvent mre = new ManualResetEvent(false);

        /// <summary>
        /// 重构函数执行器
        /// </summary>
        private void Run()
        {
            ///无限循环执行函数器
            while (true)
            {
                if (taskQueue.Count > 0)
                {
                    IEnumerable<TimerTaskBase> collections = null;
                    lock (taskQueue)
                    {
                        //拷贝一次队列 预防本次轮训检查的时候有新的任务添加
                        //否则循环会出错 集合被修改无法迭代
                        collections = new List<TimerTaskBase>(taskQueue);
                    }
                    //开始迭代
                    foreach (TimerTaskBase tet in collections)
                    {
                        int actionCount = tet.AActionCount;
                        long timers = GetDate();
                        if ((tet.EndTime > 0 && timers > tet.EndTime) || (tet.ActionCount > 0 && actionCount >= tet.ActionCount))
                        {
                            //任务过期
                            lock (taskQueue)
                            {
                                taskQueue.Remove(tet);
                            }
                            continue;
                        }
                        //获取最后一次的执行时间
                        long lastactiontime = tet.LastTime;
                        if (lastactiontime != 0 && Math.Abs(timers - lastactiontime) < tet.IntervalTime)
                        {
                            continue;
                        }
                        //记录出来次数
                        tet.AActionCount++;
                        //记录最后执行的时间
                        tet.LastTime = timers;

                        //上面的代码执行情况是非常几乎不用考虑消耗问题

                        //下面是任务的执行需要考虑消耗，

                        //这里我们不考虑执行耗时问题，
                        //我们我这里没有涉及到后台线程池
                        //也没有具体的业务逻辑，所以都放到这里统一执行
                        tet.Run();
                    }
                }
                //暂停10毫秒后再次检查
                mre.WaitOne(10);
            }
        }
    }
}
