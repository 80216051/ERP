using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Data;

using Model.Sys;

namespace IDAL.Sys
{
    /// <summary>
    /// 后台数据处理接口
    /// </summary>
    public interface ISystemAdmin
    {
        //插入登入记录
        bool InsertLoginLog(Sys_LoginLog log);

        //获取所有登录操作记录（分页）
        List<Sys_LoginLog> GetLoginLogList(int pageSize, int indexPage, string where);

        //sql执行返回DataTable
        DataTable GetDataTable(string sql);

        //sql 执行ExecuteNonQuery
        int Exec_NnQuery(string sql);

        //录题统计
        string getDataCharts(string startdate,string enddate);
    }
}
