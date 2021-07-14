using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLibrary.Threadlib
{
    /// <summary>
    /// 自定义线程对象。多线程new ExecCustomThread，单线程直接用ExecCustomThread.GetInstrance
    /// </summary>
    public class ExecCustomThread : BaseThread<BaseTask>
    {
        public ExecCustomThread()
            : base("CustomThread")
        {
            
        }

        private static object synclock = new object();
        private static ExecCustomThread Instrance;
        public static ExecCustomThread GetInstrance()
        {
            if (Instrance == null)
            {
                lock (synclock)
                {
                    if (Instrance == null)
                    {
                        Instrance = new ExecCustomThread();
                    }
                }
            }
            return Instrance;
        }

        private static object synclock1 = new object();
        private static ExecCustomThread Instrance1;
        public static ExecCustomThread GetInstrance1()
        {
            if (Instrance1 == null)
            {
                lock (synclock1)
                {
                    if (Instrance1 == null)
                    {
                        Instrance1 = new ExecCustomThread();
                    }
                }
            }
            return Instrance1;
        }

        private static object synclock2 = new object();
        private static ExecCustomThread Instrance2;
        public static ExecCustomThread GetInstrance2()
        {
            if (Instrance2 == null)
            {
                lock (synclock2)
                {
                    if (Instrance2 == null)
                    {
                        Instrance2 = new ExecCustomThread();
                    }
                }
            }
            return Instrance2;
        }

    }
}
