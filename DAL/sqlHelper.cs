using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Configuration;

namespace DAL
{
    public class sqlHelper
    {
        //Database connection strings
        public static string SQLConString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
        private static SqlConnection conn;

        public static SqlConnection Conn
        {
            get
            {
                if (sqlHelper.conn == null)
                {
                    sqlHelper.conn = new SqlConnection(SQLConString);
                    sqlHelper.conn.Open();
                }
                else if (sqlHelper.conn.State == ConnectionState.Broken)
                {
                    sqlHelper.conn.Close();
                    sqlHelper.conn.Open();
                }
                else if (sqlHelper.conn.State == ConnectionState.Closed)
                {
                    sqlHelper.conn.Open();
                }
                return sqlHelper.conn;
            }
        }



        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="SQLConString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string sqlConString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(sqlConString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="SQLConString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(string sqlConString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(sqlConString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="SQLConString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string sqlConString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(sqlConString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }



        /// <summary>
        /// 执行不包含select命令的数据库查询 Execute a database query which does not include a select
        /// </summary>
        /// <param name="cmdType">命名类型，存储过程或SQL语句 Command type either stored procedure or SQL</param>
        /// <param name="cmdText">要执行的SQL命令 Acutall SQL Command</param>
        /// <param name="cmdParms">要绑定到命令的参数数组 Parameters to bind to the command</param>
        /// <returns>受SQL语句影响的记录行数</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            // Create a new Oracle command 创建一个新的Oracle命令
            SqlCommand cmd = new SqlCommand();

            //Create a connection 创建数据库连接
            using (SqlConnection conn = new SqlConnection(SQLConString))
            {

                //Prepare the command 
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                //Execute the command 执行命令
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }



        /// <summary>
        /// Execute a select query that will return a result set
        /// </summary>
        //// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or PL/SQL command</param>
        /// <param name="cmdParms">an array of OracleParamters used to execute the command</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            //Create the command and connection
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(SQLConString);

            try
            {
                //Prepare the command to execute
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);

                //Execute the query, stating that the connection should close when the resulting datareader has been read
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;

            }
            catch (Exception e)
            {

                //If an error occurs close the connection as the reader will not be used and we expect it to close the connection
                conn.Close();
                throw e;
            }
        }

        /// <summary>
        /// Execute Oracle Stored procedure
        /// </summary>
        /// <param name="spName"> Name of the Stored procedure to be execute</param>
        /// <param name="cmdParms">Parameter collection</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string spName, params SqlParameter[] cmdParms)
        {
            return ExecuteReader(CommandType.StoredProcedure, spName, cmdParms);
        }

        /// <summary>
        /// Execute an SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter(":prodid", 24));
        /// </remarks>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or PL/SQL command</param>
        /// <param name="cmdParms">an array of OracleParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(SQLConString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 用于执行存储过程
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParms)
        {
            return ExecuteScalar(CommandType.StoredProcedure, cmdText, cmdParms);
        }

        /// <summary>
        /// 用于执行无参数的SQL语句或存储过程
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            return ExecuteScalar(cmdType, cmdText, null);
        }




        /// <summary>
        /// 设置SqlParameter，如果参数是Int32、DateTime、double、decimal等类型并且对应的值是各类型的MinValue，
        /// 则将参数值设为DBNull.Value，和从数据库中取值时相对应（若对应的数据类型的值为DBNull，则设为其MinValue）
        /// Webdiyer 2005-12-28
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public static SqlParameter SetParam(string pName, object pValue)
        {

            //如果pValue为null，则直接返回 2004-12-6
            if (pValue == null)
                return new SqlParameter(pName, DBNull.Value);
            switch (Type.GetTypeCode(pValue.GetType()))
            {
                case TypeCode.String:
                    string tempStr = (string)pValue;
                    if (tempStr == null || tempStr.Trim().Length == 0)
                        return new SqlParameter(pName, DBNull.Value);
                    return new SqlParameter(pName, tempStr);
                case TypeCode.DateTime:
                    DateTime tempdt = (DateTime)pValue;
                    if (tempdt == DateTime.MinValue)
                        return new SqlParameter(pName, DBNull.Value);
                    return new SqlParameter(pName, tempdt);
                case TypeCode.Int32:
                    int tempnt = (int)pValue;
                    if (tempnt == int.MinValue)
                        return new SqlParameter(pName, DBNull.Value);
                    return new SqlParameter(pName, tempnt);
                case TypeCode.Double:
                    Double tempft = (Double)pValue;
                    if (tempft == double.MinValue)
                        return new SqlParameter(pName, DBNull.Value);
                    return new SqlParameter(pName, tempft);
                case TypeCode.Decimal:
                    decimal d = (decimal)pValue;
                    if (d == decimal.MinValue)
                        return new SqlParameter(pName, DBNull.Value);
                    return new SqlParameter(pName, d);
                default:
                    return new SqlParameter(pName, pValue);
            }
        }


        public static SqlParameter SetOutParam(string pName, SqlDbType otype, int size)
        {

            SqlParameter oprm = new SqlParameter(pName, otype, size);
            oprm.Direction = ParameterDirection.Output;
            return oprm;
        }

        /// <summary>
        /// 填充DataTable数据并返回
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmd"></param>
        /// <param name="oParams"></param>
        /// <returns></returns>
        public static DataTable FillDataTable(CommandType cmdType, string cmd, params SqlParameter[] oParams)
        {
            using (SqlConnection conn = new SqlConnection(SQLConString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd, conn);
                adapter.SelectCommand.CommandType = cmdType;
                if (oParams != null && oParams.Length > 0)
                {
                    foreach (SqlParameter prm in oParams)
                    {
                        adapter.SelectCommand.Parameters.Add(prm);
                    }
                }
                DataTable tb = new DataTable();
                adapter.Fill(tb);
                return tb;
            }
        }


        /// <summary>
        /// 填充DataTable数据并返回
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmd"></param>
        /// <param name="oParams"></param>
        /// <returns></returns>
        public static DataTable FillDataTable(string sqlConnstring, CommandType cmdType, string cmd, params SqlParameter[] oParams)
        {
            using (SqlConnection conn = new SqlConnection(sqlConnstring))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd, conn);
                adapter.SelectCommand.CommandType = cmdType;
                if (oParams != null && oParams.Length > 0)
                {
                    foreach (SqlParameter prm in oParams)
                    {
                        adapter.SelectCommand.Parameters.Add(prm);
                    }
                }
                DataTable tb = new DataTable();
                adapter.Fill(tb);
                return tb;
            }
        }

        public static DataSet FillDataSet(string sql, params SqlParameter[] oParams)
        {
            using (SqlConnection conn = new SqlConnection(SQLConString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                if (oParams != null && oParams.Length > 0)
                {
                    foreach (SqlParameter prm in oParams)
                    {
                        adapter.SelectCommand.Parameters.Add(prm);
                    }
                }
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
        public static DataSet FillDataSetByPRO(string sql, params SqlParameter[] oParams)
        {
            using (SqlConnection conn = new SqlConnection(SQLConString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (oParams != null && oParams.Length > 0)
                {
                    foreach (SqlParameter prm in oParams)
                    {

                        adapter.SelectCommand.Parameters.Add(prm);
                    }
                }
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }


        /// <summary>
        /// 获取查询结果
        /// </summary>
        /// <param name="sql">要执行的SQL语句</param>
        /// <returns></returns>
        public static DataTable GetQueryResult(string sql)
        {
            DataTable tbl = new DataTable();
            SqlConnection conn = new SqlConnection(SQLConString);
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
            {
                adapter.Fill(tbl);
            }
            return tbl;
        }

        /// <summary>
        /// 获取SqlDataReader中指定字符串字段的值
        /// Webdiyer 2004-10-18
        /// </summary>
        /// <returns>字段值，若字段为空则返回空字符串</returns>
        public static string GetStringFieldValue(object fieldName)
        {
            if (Convert.IsDBNull(fieldName))
                return "";
            return (string)fieldName;
        }

        /// <summary>
        /// 使用事务插入数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="sqlArray">Sql数组</param>
        /// <returns></returns>
        public static int ExecuteTransaction(string[] sqlArray)
        {
            SqlTransaction sqlTransaction;
            SqlConnection conn = new SqlConnection(SQLConString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqlCommand comm = conn.CreateCommand();
            sqlTransaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            comm.Transaction = sqlTransaction;
            comm.CommandType = CommandType.Text;
            int rowcount = 0;
            try
            {
                foreach (string sql in sqlArray)
                {
                    comm.CommandText = sql;
                    rowcount += comm.ExecuteNonQuery();
                }
                sqlTransaction.Commit();
                return rowcount;
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();
                return 0;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 使用事务插入数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="sqlArray">Sql数组</param>
        /// <returns></returns>
        public static int ExecuteTransaction(string sqlConString, string[] sqlArray)
        {
            SqlTransaction sqlTransaction;
            SqlConnection conn = new SqlConnection(sqlConString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqlCommand comm = conn.CreateCommand();
            sqlTransaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            comm.Transaction = sqlTransaction;
            comm.CommandType = CommandType.Text;
            int rowcount = 0;
            try
            {
                foreach (string sql in sqlArray)
                {
                    comm.CommandText = sql;
                    rowcount += comm.ExecuteNonQuery();
                }
                sqlTransaction.Commit();
                return rowcount;
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                return 0;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }


        #region 使用事务处理多个存储过程
        /// <summary>
        ///  使用事务处理多个存储过程
        /// </summary>
        /// <param name="paraArr">存储过程参数组</param>
        /// <param name="storeName">存储过程名称</param>
        /// <returns></returns>
        public static bool ExecStoreArr(SqlParameter[][] paraArr, params string[] storeName)
        {
            try
            {
                conn = CreateConnection(SQLConString);
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                SqlCommand commd = new SqlCommand();
                commd.Connection = conn;
                commd.Transaction = tran;
                for (int loop1 = 0; loop1 < storeName.Length; loop1++)
                {
                    commd.Parameters.Clear();
                    commd.CommandType = CommandType.StoredProcedure;
                    commd.CommandText = storeName[loop1];
                    foreach (SqlParameter para in paraArr[loop1])
                    {
                        commd.Parameters.Add(para);
                    }
                    commd.ExecuteNonQuery();
                }
                tran.Commit();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }

        }

        #endregion

        #region //--创建数据连接方法

        /// <summary>
        /// 创建数据连接
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="Conn"></param>
        /// <returns></returns>
        public static SqlConnection CreateConnection(string Connstr)
        {
            try
            {
                if (conn == null || string.IsNullOrEmpty(conn.ConnectionString))
                {
                    conn = new SqlConnection(Connstr);
                }
                return conn;
            }
            catch (Exception Err)
            {
                throw new Exception("很抱歉，数据库连接出错或者超时！详细信息：" + Err.ToString());
            }
        }

        #endregion
    }
}
