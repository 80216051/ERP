using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BaseLibrary
{
    public class Field
    {
        // Disallow Construction 
        private Field() { }

        // By Number144
        static public string GetString(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return string.Empty;
            return reader.GetString(fldnum).Trim();
        }

        static public decimal GetDecimal(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return decimal.MinValue;
            return reader.GetDecimal(fldnum);
        }

        static public int GetInt(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                //return int.MinValue;
                return 0;
            return reader.GetInt32(fldnum);
        }

        static public bool GetBoolean(SqlDataReader reader, int fldnum)
        {
            return (GetInt(reader, fldnum) > 0);
        }

        static public byte GetByte(SqlDataReader rec, int fldnum)
        {
            if (rec.IsDBNull(fldnum))
                return 0;
            return rec.GetByte(fldnum);
        }

        static public DateTime GetDateTime(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return DateTime.MinValue;
            return reader.GetDateTime(fldnum);
        }

        static public double GetDouble(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return double.MinValue;
            return reader.GetDouble(fldnum);
        }

        static public float GetFloat(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return float.MinValue;
            return reader.GetFloat(fldnum);
        }

        static public Guid GetGuid(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return Guid.Empty;
            return reader.GetGuid(fldnum);
        }

        static public Int32 GetInt32(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return int.MinValue;
            return reader.GetInt32(fldnum);
        }

        static public Int16 GetInt16(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return Int16.MinValue;
            return reader.GetInt16(fldnum);
        }

        static public Int64 GetInt64(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return Int64.MinValue;
            return reader.GetInt64(fldnum);
        }

        static public ulong GetUlong(SqlDataReader reader, int fldnum)
        {
            if (reader.IsDBNull(fldnum))
                return ulong.MinValue;
            return ulong.Parse(reader[fldnum].ToString());
        }





        // By Name 
        static public string GetString(SqlDataReader reader, string fldname)
        {
            return GetString(reader, reader.GetOrdinal(fldname));
        }

        static public decimal GetDecimal(SqlDataReader reader, string fldname)
        {
            return GetDecimal(reader, reader.GetOrdinal(fldname));
        }

        static public int GetInt(SqlDataReader reader, string fldname)
        {
            return GetInt(reader, reader.GetOrdinal(fldname));
        }

        static public bool GetBoolean(SqlDataReader reader, string fldname)
        {
            return GetBoolean(reader, reader.GetOrdinal(fldname));
        }

        static public byte GetByte(SqlDataReader rec, string fldname)
        {
            return GetByte(rec, rec.GetOrdinal(fldname));
        }

        static public DateTime GetDateTime(SqlDataReader reader,
            string fldname)
        {
            return GetDateTime(reader, reader.GetOrdinal(fldname));
        }

        static public double GetDouble(SqlDataReader reader, string fldname)
        {
            return GetDouble(reader, reader.GetOrdinal(fldname));
        }

        static public float GetFloat(SqlDataReader reader, string fldname)
        {
            return GetFloat(reader, reader.GetOrdinal(fldname));
        }

        static public Guid GetGuid(SqlDataReader reader, string fldname)
        {
            return GetGuid(reader, reader.GetOrdinal(fldname));
        }

        static public Int32 GetInt32(SqlDataReader reader, string fldname)
        {
            return GetInt32(reader, reader.GetOrdinal(fldname));
        }

        static public Int16 GetInt16(SqlDataReader reader, string fldname)
        {
            return GetInt16(reader, reader.GetOrdinal(fldname));
        }

        static public Int64 GetInt64(SqlDataReader reader, string fldname)
        {
            return GetInt64(reader, reader.GetOrdinal(fldname));
        }


        static public ulong GetUlong(SqlDataReader reader, string fldname)
        {
            return GetUlong(reader, reader.GetOrdinal(fldname));
        }


        static public DateTime GetDateTime(DataRow dr,
            string fldname)
        {
            if (dr.IsNull(fldname))
                return DateTime.MinValue;
            return Convert.ToDateTime(dr[fldname]);
        }

        static public int GetInt(DataRow dr, string fldname)
        {
            if (dr.IsNull(fldname))
                //return int.MinValue;
                return 0;
            return Convert.ToInt32(dr[fldname]);
        }

        static public string GetString(DataRow dr, string fldname)
        {
            return dr[fldname].ToString();
        }

        static public decimal GetDecimal(DataRow dr, string fldname)
        {
            if (dr.IsNull(fldname))
                //return decimal.MinValue;
                return 0;
            return Convert.ToDecimal(dr[fldname]);
        }

        static public Int64 GetInt64(DataRow dr, string fldname)
        {
            if (dr.IsNull(fldname))
                return Int64.MinValue;
            return Convert.ToInt64(dr[fldname]);
        }

        static public double GetDouble(DataRow dr, string fldname)
        {
            if (dr.IsNull(fldname))
                return double.MinValue;
            return Convert.ToDouble(dr[fldname]);
        }



        #region
        /// <summary>
        /// 向数据库中插入或更新数据时，设置存储过程参数，如果数据库中该列值允许null，则插入null值
        /// </summary>
        /// <param name="pName">存储过程参数名</param>
        /// <param name="pValue">参数值</param>
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
                default:
                    return new SqlParameter(pName, pValue);
            }
        }

        #endregion

        #region 设置存储过程输出参数
        /// <summary>
        /// 设置存储过程输出参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="otype">参数类型</param>
        /// <param name="size">参数的长度</param>
        /// <returns></returns>
        public static SqlParameter SetOutParam(string pName, SqlDbType otype, int size)
        {
            SqlParameter oprm = new SqlParameter(pName, otype, size);
            oprm.Direction = ParameterDirection.Output;
            return oprm;
        }
        #endregion


        #region 设置存储过程输出参数
        /// <summary>
        /// 设置存储过程输出参数
        /// </summary>
        /// <param name="pName">参数名称</param>
        /// <param name="otype">参数类型</param>
        /// <param name="size">参数的长度</param>
        /// <returns></returns>
        public static SqlParameter SetReturnParam(string pName, SqlDbType otype, int size)
        {
            SqlParameter oprm = new SqlParameter(pName, otype, size);
            oprm.Direction = ParameterDirection.ReturnValue;
            return oprm;
        }
        #endregion
    }
}
