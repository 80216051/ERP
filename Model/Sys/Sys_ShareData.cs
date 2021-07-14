using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sys
{
    /// <summary>
    /// 分享数据回流表
    /// </summary>
    public class Sys_ShareData
    {
        public Sys_ShareData() { }

        public Sys_ShareData(string GID,string SID, int UID, int IsReg, string ShareType, DateTime RegDate,DateTime CreateDate)
        {
            this.gID = GID;
            this.sID = SID;
            this.uID = UID;
            this.isReg = IsReg;
            this.shareType = ShareType;
            this.regDate = RegDate;
            this.createDate = CreateDate;
        }

        private string gID;
        /// <summary>
        /// 一次点击的ID
        /// </summary>
        public string GID
        {
            get { return gID; }
            set { gID = value; }
        }

        private string sID;
        /// <summary>
        /// 分享的数据ID
        /// </summary>
        public string SID
        {
            get { return sID; }
            set { sID = value; }
        }

        private int uID;
        /// <summary>
        /// 数据分享用户ID
        /// </summary>
        public int UID
        {
            get { return uID; }
            set { uID = value; }
        }

        private int isReg;
        /// <summary>
        /// 是否注册 1转为用户 0为注册
        /// </summary>
        public int IsReg
        {
            get { return isReg; }
            set { isReg = value; }
        }

        private string shareType;
        /// <summary>
        /// 分享平台
        /// </summary>
        public string ShareType
        {
            get { return shareType; }
            set { shareType = value; }
        }

        private DateTime regDate;
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegDate
        {
            get { return regDate; }
            set { regDate = value; }
        }

        private DateTime createDate;
        /// <summary>
        /// 点击时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        private string regDateTemp;
        /// <summary>
        /// 注册时间临时字段
        /// </summary>
        public string RegDateTemp
        {
            get { return regDateTemp; }
            set { regDateTemp = value; }
        }

        private string createDateTemp;
        /// <summary>
        /// 点击时间临时字段
        /// </summary>
        public string CreateDateTemp
        {
            get { return createDateTemp; }
            set { createDateTemp = value; }
        }
    }
}
