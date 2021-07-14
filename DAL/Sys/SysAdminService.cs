using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IDAL;
using Model;
using System.Data.SqlClient;
using BaseLibrary;
using BaseLibrary.Tool;
using Model.Sys;
using BaseLibrary.Threadlib;
using IDAL.Sys;

namespace DAL.Sys
{
    /// <summary>
    /// 后台管理员数据
    /// </summary>
    public class SysAdminService : ISysAdminService
    {
        #region sql语句
        string strSQL_saltLogin = "select * from Sys_Admin where AUserName=@AUserName and AIsUse=1";
        string strSQL_Login = "select * from Sys_Admin where AUserName=@AUserName and APass=@APass and AIsUse=1";
        string strSQL_UpdateRole = "update Sys_Admin set AModelIDStr=@AModelIDStr where AID=@AID";
        string strSQL_UpdatePass = "update Sys_Admin set APass=@APass,Asalt=@Asalt where AID=@AID";
        string strGetAdminByID = "select * from Sys_Admin where AID=@AID";
        #endregion

        //封装实体类
        private Sys_Admin sqlDataReader(SqlDataReader sqlreader)
        {
            Sys_Admin admin = new Sys_Admin();
            if (sqlreader != null && !sqlreader.IsClosed)
            {
                admin.AID = Convert.ToInt32(sqlreader["AID"]);
                admin.AName = sqlreader["AName"].ToString();
                admin.ANiceName = sqlreader["ANiceName"].ToString();
                admin.APhoto = sqlreader["APhoto"].ToString();
                admin.APass = sqlreader["APass"].ToString();
                admin.ARole = Convert.ToInt32(sqlreader["ARole"]);
                admin.ASex = sqlreader["ASex"].ToString();
                admin.AContact = sqlreader["AContact"].ToString();
                admin.AIsUse = Convert.ToInt32(sqlreader["AIsUse"]);
                admin.AUserName = sqlreader["AUserName"].ToString();
                admin.AModelIDStr = sqlreader["AModelIDStr"].ToString();
                admin.DID = Convert.ToInt32(sqlreader["DID"]);
                admin.AInputDate = Convert.ToDateTime(sqlreader["AInputDate"]);
                admin.AInputDateTemp = admin.AInputDate.ToString("yyyy-MM-dd HH:mm:ss");
                admin.IsEveryday = Convert.ToInt32(sqlreader["IsEveryday"]);
                admin.DayCode = sqlreader["DayCode"].ToString();
                try
                {
                    admin.ClassArray = sqlreader["ClassArray"].ToString();
                }
                catch {}
                try {
                    admin.Asalt = sqlreader["Asalt"].ToString();
                }
                catch {}
            }
            return admin;
        }

        #region ISysAdminService 成员
        /// <summary>
        /// 获取用户的加密盐值
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public Sys_Admin getSaltLogin(Sys_Admin admin)
        {
            Sys_Admin ad = null;
            try {
                SqlDataReader sdr = sqlHelper.ExecuteReader(CommandType.Text, strSQL_saltLogin,
                        Field.SetParam("@AUserName", admin.AUserName));
                while (sdr.Read()) {
                    ad = new Sys_Admin();
                    ad = sqlDataReader(sdr);
                }
                sdr.Close();
            }
            catch (Exception ex) {
                LogMsg.WriteLog(ex.ToString());
                return null;
            }
            return ad;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public Sys_Admin login(Sys_Admin admin)
        {
            Sys_Admin ad = null;
            try
            {
                SqlDataReader sdr = sqlHelper.ExecuteReader(CommandType.Text, strSQL_Login,
                        Field.SetParam("@AUserName", admin.AUserName),
                        Field.SetParam("@APass", admin.APass));
                while (sdr.Read())
                {
                    ad = new Sys_Admin();
                    ad = sqlDataReader(sdr);
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return null;
            }
            return ad;
        }

        /// <summary>
        /// 修改管理员权限
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool UpdateSys_AdminRole(Sys_Admin admin)
        {
            try
            {
                int i = sqlHelper.ExecuteNonQuery(CommandType.Text, strSQL_UpdateRole, Field.SetParam("@AModelIDStr", admin.AModelIDStr), Field.SetParam("@AID", admin.AID));
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return false;
            }
        }

        /// <summary>
        ///  修改密码
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool UpdateSys_AdminPass(Sys_Admin admin,string salt)
        {
            try
            {
                int i = sqlHelper.ExecuteNonQuery(CommandType.Text, strSQL_UpdatePass,
                        Field.SetParam("@APass", admin.APass),
                        Field.SetParam("@AID", admin.AID),
                        Field.SetParam("@Asalt", salt));
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 获取一个超级管理员（用于自动生成注册码）
        /// </summary>
        /// <returns></returns>
        public Sys_Admin GetSuperManager()
        {
            Sys_Admin admin = new Sys_Admin();
            try
            {
                SqlDataReader reader = sqlHelper.ExecuteReader(CommandType.Text, "select top 1 * from Sys_Admin where ARole=1");
                while (reader.Read())
                {
                    admin = sqlDataReader(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return null;
            }
            return admin;
        }

        /// <summary>
        /// 编辑修改管理员信息
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool EditeSys_Admin(Sys_Admin admin)
        {
            try
            {
                string strSQL_EditeAdmin = "update Sys_Admin set APass=@APass,Asalt=@Asalt,AName=@AName,ANiceName=@ANiceName,ASex=@ASex,AContact=@AContact,AIsUse=@AIsUse,ARole=@ARole,AModelIDStr=@AModelIDStr,DID=@DID,IsEveryday=@IsEveryday,DayCode=@DayCode where AID=@AID";
                int i = sqlHelper.ExecuteNonQuery(CommandType.Text, strSQL_EditeAdmin,
                        Field.SetParam("@APass", admin.APass),
                        Field.SetParam("@Asalt", admin.Asalt),
                        Field.SetParam("@AName", admin.AName),
                        Field.SetParam("@ANiceName", admin.ANiceName),
                        Field.SetParam("@ASex", admin.ASex),
                        Field.SetParam("@AContact", admin.AContact),
                        Field.SetParam("@AIsUse", admin.AIsUse),
                        Field.SetParam("@AID", admin.AID),
                        Field.SetParam("@ARole", admin.ARole),
                        Field.SetParam("@AModelIDStr", admin.AModelIDStr),
                        Field.SetParam("@DID", admin.DID),
                        Field.SetParam("@IsEveryday", admin.IsEveryday),
                        Field.SetParam("@DayCode", admin.DayCode));
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 编辑兼职信息
        /// </summary>
        /// <param name="admin">用户信息</param>
        /// <returns></returns>
        public bool EditePartSys_Admin(Sys_Admin admin)
        {
            try
            {
                string strSQL_EditeAdmin = "update Sys_Admin set APass=@APass,Asalt=@Asalt,AName=@AName,ANiceName=@ANiceName,ASex=@ASex,AContact=@AContact,AIsUse=@AIsUse,ARole=@ARole,AModelIDStr=@AModelIDStr,DID=@DID,IsEveryday=@IsEveryday,DayCode=@DayCode,ClassArray=@ClassArray where AID=@AID";
                int i = sqlHelper.ExecuteNonQuery(CommandType.Text, strSQL_EditeAdmin,
                        Field.SetParam("@APass", admin.APass),
                        Field.SetParam("@Asalt", admin.Asalt),
                        Field.SetParam("@AName", admin.AName),
                        Field.SetParam("@ANiceName", admin.ANiceName),
                        Field.SetParam("@ASex", admin.ASex),
                        Field.SetParam("@AContact", admin.AContact),
                        Field.SetParam("@AIsUse", admin.AIsUse),
                        Field.SetParam("@AID", admin.AID),
                        Field.SetParam("@ARole", admin.ARole),
                        Field.SetParam("@AModelIDStr", admin.AModelIDStr),
                        Field.SetParam("@DID", admin.DID),
                        Field.SetParam("@IsEveryday", admin.IsEveryday),
                        Field.SetParam("@DayCode", admin.DayCode),
                Field.SetParam("@ClassArray", admin.ClassArray));
     
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return false;
            }
        }



        

        /// <summary>
        /// 获取管理员实体
        /// </summary>
        /// <param name="AID"></param>
        /// <returns></returns>
        public Sys_Admin GetSys_AdminByID(int AID)
        {
            Sys_Admin admin = new Sys_Admin();
            try
            {
                SqlDataReader reader = sqlHelper.ExecuteReader(CommandType.Text, strGetAdminByID, Field.SetParam("@AID", AID));
                while (reader.Read())
                {
                    admin = sqlDataReader(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return null;
            }
            return admin;
        }


        /// <summary>
        /// 获取管理员（分页）
        /// </summary>
        /// <param name="TableNames">表名</param>
        /// <param name="PrimaryKey">主键，可以为空，但@Order为空时该值不能为空</param>
        /// <param name="Fields">查询列</param>
        /// <param name="pageSize">每页显示</param>
        /// <param name="indexPage">当前页</param>
        /// <param name="where">条件</param>
        /// <param name="orderby">排序，可以为空，为空默认按主键升序排列，不用填 order by</param>
        /// <param name="RecordCount">返回总页数 1只返回总数，0只返回当前页数据，2返回总数并返回当前页数据</param>
        /// <returns></returns>
        public List<Sys_Admin> GetSys_AdminList(string TableNames, string PrimaryKey, string Fields, int pageSize, int indexPage, string where, string orderby, int RecordCount)
        {
            List<Sys_Admin> list = new List<Sys_Admin>();
            try
            {
                SqlDataReader reader = commonHelper.GetTableList(TableNames, PrimaryKey, Fields, pageSize, indexPage, where, orderby, RecordCount, "");
                while (reader.Read())
                {
                    Sys_Admin admin = new Sys_Admin();
                    admin = sqlDataReader(reader);
                    if (admin.ARole == 1)
                    {
                        admin.RoleStrTemp = "超级管理员";
                    }
                    else
                    {
                        admin.RoleStrTemp = "普通管理员";
                    }
                    admin.DepartmentTemp = reader["DName"].ToString();
                    list.Add(admin);
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
        /// 获取管理员（不分页）
        /// </summary>
        /// <param name="where">where后面的条件</param>
        /// <returns></returns>
        public List<Sys_Admin> GetSys_AdminList(string where)
        {
            //sql过滤
            where = CommonTool.SafeSql(where);
            string strSelectSQL = "select * from Sys_Admin where " + where;
            List<Sys_Admin> list = new List<Sys_Admin>();
            try
            {
                SqlDataReader reader = sqlHelper.ExecuteReader(CommandType.Text, strSelectSQL);
                while (reader.Read())
                {
                    Sys_Admin admin = new Sys_Admin();
                    admin = sqlDataReader(reader);
                    list.Add(admin);
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
        /// 添加管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool AddSys_Admin(Sys_Admin admin)
        {
            try
            {
                string strSQL_InsertAdmin = "insert Sys_Admin(AUserName, APass, Asalt, AName,ANiceName,APhoto, ASex,AContact,AIsUse,AModelIDStr,ARole,DID,AInputDate,IsEveryday,DayCode)values(@AUserName,@APass,@Asalt,@AName,@ANiceName,@APhoto,@ASex,@AContact,@AIsUse,@AModelIDStr,@ARole,@DID,@AInputDate,@IsEveryday,@DayCode)";
                int i = sqlHelper.ExecuteNonQuery(CommandType.Text, strSQL_InsertAdmin,
                        Field.SetParam("@AUserName", admin.AUserName),
                        Field.SetParam("@APass", admin.APass),
                        Field.SetParam("@Asalt", admin.Asalt),
                        Field.SetParam("@AName", admin.AName),
                        Field.SetParam("@ANiceName", admin.ANiceName),
                        Field.SetParam("@APhoto", admin.APhoto),
                        Field.SetParam("@ASex", admin.ASex),
                        Field.SetParam("@AModelIDStr", admin.AModelIDStr),
                        Field.SetParam("@ARole", admin.ARole),
                        Field.SetParam("@DID", admin.DID),
                        Field.SetParam("@AInputDate", admin.AInputDate),
                        Field.SetParam("@AContact", admin.AContact),
                        Field.SetParam("@AIsUse", admin.AIsUse),
                        Field.SetParam("@IsEveryday", admin.IsEveryday),
                        Field.SetParam("@DayCode", admin.DayCode)
                        );
                if (i > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return false;
            }
            return false;
        }

        /// <summary>
        /// 添加兼职
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool AddPartAdmin(Sys_Admin admin)
        {
            try
            {
                string strSQL_InsertAdmin = "insert Sys_Admin(AUserName, APass, Asalt,AName,ANiceName,APhoto, ASex,AContact,AIsUse,AModelIDStr,ARole,DID,AInputDate,IsEveryday,DayCode,ClassArray)values(@AUserName,@APass,@Asalt,@AName,@ANiceName,@APhoto,@ASex,@AContact,@AIsUse,@AModelIDStr,@ARole,@DID,@AInputDate,@IsEveryday,@DayCode,@ClassArray)";
                int i = sqlHelper.ExecuteNonQuery(CommandType.Text, strSQL_InsertAdmin,
                        Field.SetParam("@AUserName", admin.AUserName),
                        Field.SetParam("@APass", admin.APass),
                        Field.SetParam("@Asalt", admin.Asalt),
                        Field.SetParam("@AName", admin.AName),
                        Field.SetParam("@ANiceName", admin.ANiceName),
                        Field.SetParam("@APhoto", admin.APhoto),
                        Field.SetParam("@ASex", admin.ASex),
                        Field.SetParam("@AModelIDStr", admin.AModelIDStr),
                        Field.SetParam("@ARole", admin.ARole),
                        Field.SetParam("@DID", admin.DID),
                        Field.SetParam("@AInputDate", admin.AInputDate),
                        Field.SetParam("@AContact", admin.AContact),
                        Field.SetParam("@AIsUse", admin.AIsUse),
                        Field.SetParam("@IsEveryday", admin.IsEveryday),
                        Field.SetParam("@DayCode", admin.DayCode),
                          Field.SetParam("@ClassArray", admin.ClassArray)
                        );
                if (i > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return false;
            }
            return false;
        }





        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="AID"></param>
        /// <returns></returns>
        public bool DeleteSys_Admin(int AID)
        {
            try
            {
                string strSQL_DeleteAdmin = "delete Sys_Admin where AID=@AID";
                int result = sqlHelper.ExecuteNonQuery(CommandType.Text, strSQL_DeleteAdmin, Field.SetParam("@AID", AID));
                if (result > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
                return false;
            }
            return false;
        }

        /// <summary>
        /// 修改管理员头像
        /// </summary>
        /// <param name="AID">管理员ID</param>
        /// <param name="URL">头像路径</param>
        /// <returns></returns>
        public bool ModifyPhoto(int AID, string URL)
        {
            try
            {
                string sql = "update Sys_Admin set APhoto=@APhoto where AID=@AID";
                int result = sqlHelper.ExecuteNonQuery(CommandType.Text, sql, Field.SetParam("@AID", AID), Field.SetParam("@APhoto", URL));
                if (result <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
            }
            return true;
        }

        /// <summary>
        /// 存在此管理员用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckAdminNameIsTrue(string userName)
        {
            try
            {
                string strSQLCheckUserName = "select count(*) from Sys_Admin where AUserName=@AUserName";
                object i = sqlHelper.ExecuteScalar(CommandType.Text, strSQLCheckUserName, Field.SetParam("@AUserName", userName));
                if (Convert.ToInt32(i) == 0)
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
        /// 更新管理员的注册码提取量
        /// </summary>
        /// <returns></returns>
        public void UpdateAdminDayCode()
        {
            try
            {
                List<Sys_Admin> listuser = new List<Sys_Admin>();
                listuser = GetSys_AdminList(" ARole=0 and AIsUse=1 and IsEveryday>0");
                if (listuser != null && listuser.Count > 0)
                {
                    string nowDate = DateTime.Now.ToString("yyyy-MM-dd") + "|";
                    for (int i = 0; i < listuser.Count; i++)
                    {
                        sqlHelper.ExecuteNonQuery(CommandType.Text, "update Sys_Admin set DayCode=@DayCode where AID=@AID", Field.SetParam("@DayCode", nowDate + listuser[i].IsEveryday), Field.SetParam("@AID", listuser[i].AID));
                    }
                }
            }
            catch (Exception ex)
            {
                LogMsg.WriteLog(ex.ToString());
            }
        }

        #endregion
    }
}
