using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BaseLibrary
{
    /// <summary>
    /// 缓存操作类
    /// </summary>
    public class DataCache
    {
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值2
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            if (objObject != null)
            {
                objCache.Insert(CacheKey, objObject);
            }
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="CacheKey"></param>
        public static void RemoveCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Remove(CacheKey);
        }

        #region 设置文件依赖缓存，缓存数据
        /// <summary>
        /// 设置文件依赖缓存，缓存数据
        /// </summary> 
        /// <param name="CacheKey">索引键值</param>
        /// <param name="objObject">缓存对象</param>
        /// <param name="cacheDepen">依赖对象</param>
        public static void SetCache(string CacheKey, object objObject, System.Web.Caching.CacheDependency dep)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(
                CacheKey,
                objObject,
                dep,
                System.Web.Caching.Cache.NoAbsoluteExpiration, //从不过期      
                System.Web.Caching.Cache.NoSlidingExpiration, //禁用可调过期       
                System.Web.Caching.CacheItemPriority.Default, null);
        }
        #endregion
    }
}
