using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.Sys;

namespace IDAL.Sys
{
    /// <summary>
    /// 后台管理员接口类
    /// </summary>
    public interface ISysAdminService
    {
        //获取用户登录密码盐值
        Sys_Admin getSaltLogin(Sys_Admin admin);
        //登录
        Sys_Admin login(Sys_Admin admin);

        //修改权限
        bool UpdateSys_AdminRole(Sys_Admin admin);

        //修改密码
        bool UpdateSys_AdminPass(Sys_Admin admin,string salt);

        //编辑修改管理员信息
        bool EditeSys_Admin(Sys_Admin admin);

        /// <summary>
        /// 编辑兼职信息
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        bool EditePartSys_Admin(Sys_Admin admin); 
        

        //获取管理员实体
        Sys_Admin GetSys_AdminByID(int AID);

        //获取管理员（分页）
        List<Sys_Admin> GetSys_AdminList(string TableNames, string PrimaryKey, string Fields, int pageSize, int indexPage, string where, string orderby, int RecordCount);

        //获取管理员（不分页）
        List<Sys_Admin> GetSys_AdminList(string where);

        //添加管理员
        bool AddSys_Admin(Sys_Admin admin);


        bool AddPartAdmin(Sys_Admin admin); 
        

        //删除管理员
        bool DeleteSys_Admin(int AID);

        //修改管理员头像
        bool ModifyPhoto(int AID,string URL);

        //存在此管理员用户名
        bool CheckAdminNameIsTrue(string userName);

        //更新管理员的注册码提取量
        void UpdateAdminDayCode();
    }
}
