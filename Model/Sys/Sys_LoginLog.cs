using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sys
{
    /// <summary>
    /// 登录日志类
    /// </summary>
    public class Sys_LoginLog
    {
        public Sys_LoginLog() { }

        public Sys_LoginLog(string AdminAction, string UserName, string LoginIP, DateTime ActionDate) 
        {
            this.adminAction = AdminAction;
            this.userName = UserName;
            this.loginIP = LoginIP;
            this.actionDate = ActionDate;
        }

        private int logID;
        /// <summary>
        /// ID
        /// </summary>
        public int LogID
        {
            get { return logID; }
            set { logID = value; }
        }

        private string adminAction;
        /// <summary>
        /// 操作内容
        /// </summary>
        public string AdminAction
        {
            get { return adminAction; }
            set { adminAction = value; }
        }

        private string userName;
        /// <summary>
        /// 操作者账号（管理员账号）
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string loginIP;
        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIP
        {
            get { return loginIP; }
            set { loginIP = value; }
        }

        private DateTime actionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime ActionDate
        {
            get { return actionDate; }
            set { actionDate = value; }
        }
    }
}
