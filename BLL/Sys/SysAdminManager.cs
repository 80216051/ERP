using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using IDAL;
using DAL;
using DALFactory;
using Model.Sys;

namespace BLL.Sys
{
    /// <summary>
    /// 后台管理员类数据逻辑层
    /// </summary>
    public class SysAdminManager
    {
        #region 属性和构造函数
        private IDAL.Sys.ISysAdminService dal;
        public SysAdminManager()
        {
            dal = DALFactory.DataAccess.CreateCacheSysAdminService();
        }
        private static object synclock = new object();
        private static SysAdminManager Instrance;
        public static SysAdminManager GetInstrance()
        {
            if (Instrance == null)
            {
                lock (synclock)
                {
                    if (Instrance == null)
                    {
                        Instrance = new SysAdminManager();
                    }
                }
            }
            return Instrance;
        }
        #endregion

        /// <summary>
        /// 获取管理员（不分页）
        /// </summary>
        /// <param name="where">where后面的条件</param>
        /// <returns></returns>
        public List<Sys_Admin> GetSys_AdminList(string where)
        {
            return dal.GetSys_AdminList(where);
        }

        /// <summary>
        /// 获取用户登录密码盐值
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public Sys_Admin getSaltLogin(Sys_Admin admin)
        {
            return dal.getSaltLogin(admin);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public Sys_Admin login(Sys_Admin admin)
        {
            return dal.login(admin);
        }

        /// <summary>
        /// 修改管理员权限
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool UpdateSys_AdminRole(Sys_Admin admin)
        {
            return dal.UpdateSys_AdminRole(admin);
        }

        /// <summary>
        ///  修改-密码
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool UpdateSys_AdminPass(Sys_Admin admin,string salt)
        {
            return dal.UpdateSys_AdminPass(admin,salt);
        }

        /// <summary>
        /// 编辑修改管理员信息
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool EditeSys_Admin(Sys_Admin admin)
        {
            return dal.EditeSys_Admin(admin);
        }

        public bool EditePartSys_Admin(Sys_Admin admin) 
        {
            return dal.EditePartSys_Admin(admin); 
        }

        

        /// <summary>
        /// 获取管理员实体
        /// </summary>
        /// <param name="AID"></param>
        /// <returns></returns>
        public Sys_Admin GetSys_AdminByID(int AID)
        {
            return dal.GetSys_AdminByID(AID);
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
            return dal.GetSys_AdminList(TableNames, PrimaryKey, Fields, pageSize, indexPage, where, orderby, RecordCount);
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool AddSys_Admin(Sys_Admin admin)
        {
            return dal.AddSys_Admin(admin);
        }

        public bool AddPartAdmin(Sys_Admin admin) 
        {
            return dal.AddPartAdmin(admin);
        }

        


        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="AID"></param>
        /// <returns></returns>
        public bool DeleteSys_Admin(int AID)
        {
            return dal.DeleteSys_Admin(AID);
        }

        /// <summary>
        /// 修改管理员头像
        /// </summary>
        /// <param name="AID">管理员ID</param>
        /// <param name="URL">头像路径</param>
        /// <returns></returns>
        public bool ModifyPhoto(int AID, string URL)
        {
            return dal.ModifyPhoto(AID, URL);
        }

        /// <summary>
        /// 存在此管理员用户名
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckAdminNameIsTrue(string userName)
        {
            return dal.CheckAdminNameIsTrue(userName);
        }

        /// <summary>
        /// 更新管理员的注册码提取量
        /// </summary>
        /// <returns></returns>
        public void UpdateAdminDayCode()
        {
            dal.UpdateAdminDayCode();
        }
    }
}
