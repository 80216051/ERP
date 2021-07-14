using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using IDAL;
using DAL;
using System.Reflection;
using System.Configuration;
using BaseLibrary;


namespace DALFactory
{
    /// <summary>
    /// 抽象工厂类
    /// </summary>
    public class DataAccess
    {
        /// <summary>
        /// 获取数据库的
        /// </summary>
        /// <returns></returns>
        private static string GetPath()
        {
            return ConfigurationManager.AppSettings["WebDAL"]; //获取数据操作层路径
        }

        /// <summary>
        /// 管理员类接口：使用缓存创建接口
        /// </summary>
        /// <returns></returns>
        public static IDAL.Sys.ISysAdminService CreateCacheSysAdminService()
        {
            string className = GetPath() + ".Sys.SysAdminService";
            object objType = DataCache.GetCache(className);
            if (objType == null)
            {
                try
                {
                    objType = Assembly.Load(GetPath()).CreateInstance(className);

                    DataCache.SetCache(className, objType);//写入缓存
                }
                catch { }

            }
            return (IDAL.Sys.ISysAdminService)objType;
        }

        /// <summary>
        /// 后台数据处理接口：使用缓存创建接口
        /// </summary>
        /// <returns></returns>
        public static IDAL.Sys.ISystemAdmin CreateCacheSystemAdmin()
        {
            string className = GetPath() + ".Sys.SystemService";
            object objType = DataCache.GetCache(className);
            if (objType == null)
            {
                try
                {
                    objType = Assembly.Load(GetPath()).CreateInstance(className);
                    DataCache.SetCache(className, objType);//写入缓存
                }
                catch { }

            }
            return (IDAL.Sys.ISystemAdmin)objType;
        }
    }
}
