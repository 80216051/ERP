using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using BaseLibrary;
using BaseLibrary.Threadlib;


namespace DAL
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class commonHelper
    {
        /// <summary>
        /// 获取查询集合数量（通用查询数据总量）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="where">条件</param>
        /// <param name="group">分组条件</param>
        /// <returns></returns>
        public int GetTableCount(string tableName, string where, string group)
        {
           
                SqlParameter[] Para = new SqlParameter[] {
                   new SqlParameter("@TableNames",tableName),
                   new SqlParameter("@PrimaryKey","*"),
                   new SqlParameter("@Fields","*"),
                   new SqlParameter("@PageSize",10),
                   new SqlParameter("@CurrentPage",1),
                   new SqlParameter("@Filter",where),
                   new SqlParameter("@Group",group),
                   new SqlParameter("@Order",DBNull.Value),
                   new SqlParameter("@RecordCount",1)
               };

            using (SqlDataReader reader = sqlHelper.ExecuteReader(CommandType.StoredProcedure, "usp_PagingLarge", Para))
            {
                int total = 0;
                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["Total"]);
                }
                reader.Close();
                return total;
            }
        }
        public int GetTableAcodeCount(string tableName, string where, string group)
        { 

            SqlParameter[] Para = new SqlParameter[] {
                   new SqlParameter("@TableNames",tableName),
                   new SqlParameter("@PrimaryKey","*"),
                   new SqlParameter("@Fields","*"),
                   new SqlParameter("@PageSize",10),
                   new SqlParameter("@CurrentPage",1),
                   new SqlParameter("@Filter",where),
                   new SqlParameter("@Group",group),
                   new SqlParameter("@Order",DBNull.Value),
                   new SqlParameter("@RecordCount",1)
               };

            using (SqlDataReader reader = sqlHelper.ExecuteReader(CommandType.StoredProcedure, "usp_PagingAcode", Para))
            {
                int total = 0;
                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["Total"]);
                }
                reader.Close();
                return total;
            }
        }



        //获取分页数据（公共部分）
        public static SqlDataReader GetTableList(string TableNames, string PrimaryKey, string Fields, int pageSize, int indexPage, string where, string orderby, int RecordCount, string group)
        {
            indexPage--;//这里是从0开始的
            SqlParameter[] Para = new SqlParameter[] {
                   new SqlParameter("@TableNames",TableNames),
                   new SqlParameter("@PrimaryKey",PrimaryKey),
                   new SqlParameter("@Fields",Fields),
                   new SqlParameter("@PageSize",pageSize),
                   new SqlParameter("@CurrentPage",Math.Abs(indexPage)),
                   new SqlParameter("@Filter",where),
                   new SqlParameter("@Group",group),
                   new SqlParameter("@Order",orderby),
                   new SqlParameter("@RecordCount",RecordCount)
               };
            SqlDataReader reader = sqlHelper.ExecuteReader(CommandType.StoredProcedure, "usp_PagingLarge", Para);
            return reader;
        }


        public static SqlDataReader GetNewsCommon_PageListUserLists(string TableNames, string Fields, int pageSize, int indexPage, string where, string orderby, int RecordCount)
        { 
           // indexPage--;//这里是从0开始的
            SqlParameter[] Para = new SqlParameter[] {
                   new SqlParameter("@tab",TableNames),
                   new SqlParameter("@strFld",Fields),
                   new SqlParameter("@strWhere",where),
                       new SqlParameter("@PageIndex",indexPage),
                   new SqlParameter("@PageSize",pageSize),
               
                   
                   new SqlParameter("@Sort",orderby),
                   new SqlParameter("@IsGetCount",RecordCount)
               };
            SqlDataReader reader = sqlHelper.ExecuteReader(CommandType.StoredProcedure, "Common_PageList", Para); 
            return reader;
        }



        /// <summary>
        /// SqlDataReader读取封装List对象集合
        /// --以下通过智能匹配赋值的方法使用前提是：数据表必须每个字段都非空，或者空值赋值给的Model对象属性类型为：double,String,int,datetime的时候才适用 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public static List<T> ConvertData<T>(SqlDataReader sdr)
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            while (sdr.Read())
            {
                T model = Activator.CreateInstance<T>();
                for (int i = 0; i < properties.Length; i++)
                {
                    for (int j = 0; j < sdr.FieldCount; j++)
                    {
                        //判断属性的名称和字段的名称是否相同
                        if (properties[i].Name == sdr.GetName(j))
                        {
                            Object value = sdr[j];
                            //将字段的值赋值给T中的属性
                            if (value != DBNull.Value)
                            {
                                properties[i].SetValue(model, value, null);
                            }
                            else
                            {
                                //例如：isUser=0 赋值UseDate的时候赋值一个字符串空值肯定错误。
                                //properties[i].SetValue(model, "", null);

                                switch (properties[i].PropertyType.Name)
                                {
                                    case "String":
                                        properties[i].SetValue(model, "", null);
                                        break;
                                    case "Int":
                                        properties[i].SetValue(model, 0, null);
                                        break;
                                    case "Float":
                                        properties[i].SetValue(model, 0, null);
                                        break;
                                    case "Double":
                                        properties[i].SetValue(model, 0, null);
                                        break;
                                    case "DateTime":
                                        properties[i].SetValue(model, null, null);
                                        break;
                                    default:
                                        properties[i].SetValue(model, null, null);
                                        break;
                                }
                            }
                            break;
                        }
                    }
                }
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// SqlDataReader读取封装对象
        /// --以下通过智能匹配赋值的方法使用前提是：数据表必须每个字段都非空，或者空值赋值给的Model对象属性类型为：double,String,int,datetime的时候才适用 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sdr"></param>
        /// <returns></returns>
        public static T ConvertObject<T>(SqlDataReader sdr)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            T model = Activator.CreateInstance<T>();
            while (sdr.Read())
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    for (int j = 0; j < sdr.FieldCount; j++)
                    {
                        //判断属性的名称和字段的名称是否相同
                        if (properties[i].Name == sdr.GetName(j))
                        {
                            Object value = sdr[j];
                            //将字段的值赋值给T中的属性
                            if (value != DBNull.Value)
                            {
                                properties[i].SetValue(model, value, null);
                            }
                            else
                            {
                                //例如：isUser=0 赋值UseDate的时候赋值一个字符串空值肯定错误。
                                //properties[i].SetValue(model, "", null);

                                switch (properties[i].PropertyType.Name)
                                {
                                    case "String":
                                        properties[i].SetValue(model, "", null);
                                        break;
                                    case "Int":
                                        properties[i].SetValue(model, 0, null);
                                        break;
                                    case "Float":
                                        properties[i].SetValue(model, 0, null);
                                        break;
                                    case "Double":
                                        properties[i].SetValue(model, 0, null);
                                        break;
                                    case "DateTime":
                                        properties[i].SetValue(model, null, null);
                                        break;
                                    default:
                                        properties[i].SetValue(model, null, null);
                                        break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 对数据库增删改的操作
        /// 对于不适用参数化传参的时候，应该对各个参数进行过滤敏感关键词
        /// </summary>
        /// <param name="sqlStr">sql语句或存储过程</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdParms">无参数则不传此值</param>
        /// <returns></returns>
        public bool ExecuteSQL(string sqlStr, CommandType cmdType, params SqlParameter[] cmdParms)
        {
            try
            {
                int i = sqlHelper.ExecuteNonQuery(cmdType, sqlStr, cmdParms);
                if (i <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog("sql:" + sqlStr + "--------------------" + ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 对数据库查询返回一个值的方法
        /// 对于不适用参数化传参的时候，应该对各个参数进行过滤敏感关键词
        /// </summary>
        /// <param name="sqlStr">sql语句或存储过程</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdParms">无参数则不传此值</param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlStr, CommandType cmdType, params SqlParameter[] cmdParms)
        {
            try
            {
                object i = sqlHelper.ExecuteScalar(cmdType, sqlStr, cmdParms);
                return i;
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog("sql:" + sqlStr + "--------------------" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 对数据库查询数据对象的封装
        /// --对于不适用参数化传参的时候，应该对各个参数进行过滤敏感关键词
        /// --以下通过智能匹配赋值的方法使用前提是：数据表必须每个字段都非空，或者空值赋值给的Model对象属性类型为：double,String,int,datetime的时候才适用 
        /// </summary>
        /// <param name="sqlStr">sql语句或存储过程</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdParms">无参数则不传此值</param>
        /// <returns></returns>
        public T QueryEntity<T>(string sqlStr, CommandType cmdType, params SqlParameter[] cmdParms)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            T model = Activator.CreateInstance<T>();
            try
            {
                SqlDataReader sdr = sqlHelper.ExecuteReader(cmdType, sqlStr, cmdParms);
                while (sdr.Read())
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        for (int j = 0; j < sdr.FieldCount; j++)
                        {
                            //判断属性的名称和字段的名称是否相同
                            if (properties[i].Name == sdr.GetName(j))
                            {
                                Object value = sdr[j];
                                //将字段的值赋值给T中的属性
                                if (value != DBNull.Value)
                                {
                                    properties[i].SetValue(model, value, null);
                                }
                                else
                                {
                                    //例如：isUser=0 赋值UseDate的时候赋值一个字符串空值肯定错误。
                                    //properties[i].SetValue(model, "", null);

                                    switch (properties[i].PropertyType.Name)
                                    {
                                        case "String":
                                            properties[i].SetValue(model, "", null);
                                            break;
                                        case "Int":
                                            properties[i].SetValue(model, 0, null);
                                            break;
                                        case "Float":
                                            properties[i].SetValue(model, 0, null);
                                            break;
                                        case "Double":
                                            properties[i].SetValue(model, 0, null);
                                            break;
                                        case "DateTime":
                                            properties[i].SetValue(model, null, null);
                                            break;
                                        default:
                                            properties[i].SetValue(model, null, null);
                                            break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog("sql:" + sqlStr + "--------------------" + ex.ToString());
                return model;
            }
            return model;
        }


        /// <summary>
        /// 对数据库查询数据对象集合的封装  不能查询单个字段，如select QID from TableA 
        /// --对于不适用参数化传参的时候，应该对各个参数进行过滤敏感关键词
        /// --以下通过智能匹配赋值的方法使用前提是：数据表必须每个字段都非空，或者空值赋值给的Model对象属性类型为：double,String,int,datetime的时候才适用 
        /// </summary>
        /// <param name="sqlStr">sql语句或存储过程</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdParms">无参数则不传此值</param>
        /// <returns></returns>
        public List<T> QueryEntityList<T>(string sqlStr, CommandType cmdType, params SqlParameter[] cmdParms)
        {
            try
            {
                List<T> list = new List<T>();
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();

                SqlDataReader sdr = sqlHelper.ExecuteReader(cmdType, sqlStr, cmdParms);
                while (sdr.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    for (int i = 0; i < properties.Length; i++)
                    {
                        for (int j = 0; j < sdr.FieldCount; j++)
                        {
                            //判断属性的名称和字段的名称是否相同
                            if (properties[i].Name == sdr.GetName(j))
                            {
                                Object value = sdr[j];
                                //将字段的值赋值给T中的属性
                                if (value != DBNull.Value)
                                {
                                    properties[i].SetValue(model, value, null);
                                }
                                else
                                {
                                    //例如：isUser=0 赋值UseDate的时候赋值一个字符串空值肯定错误。
                                    //properties[i].SetValue(model, "", null);

                                    switch (properties[i].PropertyType.Name)
                                    {
                                        case "String":
                                            properties[i].SetValue(model, "", null);
                                            break;
                                        case "Int":
                                            properties[i].SetValue(model, 0, null);
                                            break;
                                        case "Float":
                                            properties[i].SetValue(model, 0, null);
                                            break;
                                        case "Double":
                                            properties[i].SetValue(model, 0, null);
                                            break;
                                        case "DateTime":
                                            properties[i].SetValue(model, null, null);
                                            break;
                                        default:
                                            properties[i].SetValue(model, null, null);
                                            break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    list.Add(model);
                }
                sdr.Close();
                return list;
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog("sql:" + sqlStr + "--------------------" + ex.ToString());
                return null;
            }
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
                LogMsg.WriteLog("sql:" + sql + "--------------------" + ex.ToString());
                return null;
            }
        }
    }
}
