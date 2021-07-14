using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using IDAL;
using DAL;
using DALFactory;
using System.Data;
using Model.Sys;

namespace BLL.Sys
{
    /// <summary>
    /// 后台数据处理数据逻辑层
    /// </summary>
    public class SystemManager
    {
        #region 属性和构造函数
        private IDAL.Sys.ISystemAdmin dal;
        private SystemManager()
        {
            dal = DALFactory.DataAccess.CreateCacheSystemAdmin();
        }
        private static object synclock = new object();
        private static SystemManager Instrance;
        public static SystemManager GetInstrance()
        {
            if (Instrance == null)
            {
                lock (synclock)
                {
                    if (Instrance == null)
                    {
                        Instrance = new SystemManager();
                    }
                }
            }
            return Instrance;
        }
        #endregion

        /// <summary>
        /// 录题统计
        /// </summary>
        /// <returns></returns>
        public string getDataCharts(string startdate, string enddate)
        {
            return dal.getDataCharts(startdate, enddate);
        }

        /// <summary>
        /// 插入登入记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool InsertLoginLog(Sys_LoginLog log)
        {
            return dal.InsertLoginLog(log);
        }

        /// <summary>
        /// 获取所有登录操作记录（分页）
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="indexPage"></param>
        /// <param name="where">条件，已带where关键词</param>
        /// <returns></returns>
        public List<Sys_LoginLog> GetLoginLogList(int pageSize, int indexPage, string where)
        {
            return dal.GetLoginLogList(pageSize, indexPage, where);
        }

        /// <summary>
        /// //sql执行返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return dal.GetDataTable(sql);
        }

        /// <summary>
        /// sql 执行ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Exec_NnQuery(string sql)
        {
            return dal.Exec_NnQuery(sql);
        }
    }
}