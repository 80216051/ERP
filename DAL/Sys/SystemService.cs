using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDAL;
using Model;
using System.Data.SqlClient;
using System.Data;

using BaseLibrary;
using BaseLibrary.Tool;
using Model.Sys;
using BaseLibrary.Threadlib;
using System.Web;

namespace DAL.Sys
{
    /// <summary>
    /// 后台数据处理数据访问层
    /// </summary>
    public class SystemService : IDAL.Sys.ISystemAdmin
    {
        #region sql语句
        string sqlInsertLoginLog = "insert Sys_LoginLog(AdminAction, UserName, LoginIP, ActionDate)values(@AdminAction,@UserName,@LoginIP,@ActionDate)";
        #endregion

        #region ISystemAdmin 成员

        /// <summary>
        /// 录题统计
        /// </summary>
        /// <returns></returns>
        public string getDataCharts(string startdate, string enddate)
        {
            string jsonData = "{";
            try
            {
                TimeSpan ts = Convert.ToDateTime(enddate) - Convert.ToDateTime(startdate);
                int days = ts.Days;

                string sqlDataCharts = "declare @total int,@avg int;select @total=count(*) from Exam_QuesContent where addDate>='" + startdate + "' and addDate<='" + enddate + "' and typeNo!=9 and typeNo!=11;select  s.AName as name,count(*) as data,@total as total,avg(@total/" + days + ") as totalavg,(select ceiling(convert(numeric(8,2),count(*))/" + days + ") from Exam_QuesContent where AID=e.AID and addDate>='" + startdate + "' and addDate<='" + enddate + "' and typeNo!=9 and typeNo!=11) as oneavg from  Exam_QuesContent e inner join Sys_Admin as s on e.AID=s.AID  where addDate>='" + startdate + "' and addDate<='" + enddate + "' and e.typeNo!=9 and e.typeNo!=11 group by  e.AID,s.AName";
                SqlDataReader reader = sqlHelper.ExecuteReader(CommandType.Text, sqlDataCharts);
                string names = "\"name\":[";//名字
                string datas = "\"data\":[";//数据
                string total = "";//总量
                string avg = "";//总平均量

                string oneData = "{\"name\":\"个人题量\",\"data\":[";//个人数量
                string oneAvg = "{\"name\":\"个人平均题量\",\"data\":[";//个人平均率
                while (reader.Read())
                {
                    if (total == "")
                    {
                        //只读一次
                        total = "\"total\":" + reader["total"] + ",";
                        avg = "\"totalavg\":" + reader["totalavg"];
                    }
                    names += "\"" + reader["name"] + "\",";
                    oneData += reader["data"] + ",";
                    oneAvg += reader["oneavg"] + ",";
                }

                if (!reader.HasRows)
                {
                    jsonData = "{";
                }
                else
                {
                    names = names.Substring(0, names.Length - 1) + "],";
                    datas = datas + oneData.Substring(0, oneData.Length - 1) + "]}," + oneAvg.Substring(0, oneAvg.Length - 1) + "]}],";
                    jsonData += names + datas + total + avg;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                jsonData += "}";
                return jsonData;
            }
            jsonData += "}";
            return jsonData;
        }

        /// <summary>
        /// 插入登入记录
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool InsertLoginLog(Sys_LoginLog log)
        {
            try
            {
                int b = sqlHelper.ExecuteNonQuery(CommandType.Text, sqlInsertLoginLog,
                        Field.SetParam("@AdminAction", log.AdminAction),
                        Field.SetParam("@ActionDate", log.ActionDate),
                        Field.SetParam("@UserName", log.UserName),
                        Field.SetParam("@LoginIP", log.LoginIP)
                        );
                if (b <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return false;
            }
            return true;
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
            //sql过滤
            where = CommonTool.SafeSql(where);

            List<Sys_LoginLog> list = new List<Sys_LoginLog>();
            try
            {
                string strSQL_SelectLoginLog = "declare @row int;select @row=count(*) from Sys_LoginLog where " + where + ";SELECT TOP " + pageSize + " @row as Record, * FROM Sys_LoginLog WHERE " + where + " and LogID NOT IN(SELECT TOP (" + pageSize + "*(" + indexPage + "-1)) LogID FROM Sys_LoginLog where " + where + " ORDER BY ActionDate desc)ORDER BY ActionDate desc";
                SqlDataReader reader = sqlHelper.ExecuteReader(CommandType.Text, strSQL_SelectLoginLog);
                while (reader.Read())
                {
                    Sys_LoginLog log = new Sys_LoginLog();
                    log.ActionDate = Convert.ToDateTime(reader["ActionDate"]);
                    log.AdminAction = reader["AdminAction"].ToString();
                    log.LogID = Convert.ToInt32(reader["LogID"]);
                    log.LoginIP = reader["LoginIP"].ToString();
                    log.UserName = reader["UserName"].ToString();
                    list.Capacity = Convert.ToInt32(reader["Record"]);
                    list.Add(log);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return null;
            }
            return list;
        }

        /// <summary>
        /// //sql执行返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = sqlHelper.GetQueryResult(sql);
                return dt;
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// sql 执行ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Exec_NnQuery(string sql)
        {
            try
            {
                return sqlHelper.ExecuteNonQuery(CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return 0;
            }
        }

        #endregion
    }
}
