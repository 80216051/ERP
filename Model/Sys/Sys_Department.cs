using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sys
{
    /// <summary>
    /// 部门表
    /// </summary>
    [Serializable]
    public class Sys_Department
    {
        public Sys_Department() { }

        public Sys_Department(string DName, string DDescription, int DParentID, string DModelIDStr) 
        {
            this.dName = DName;
            this.dDescription = DDescription;
            this.dParentID = DParentID;
            this.dModelIDStr = DModelIDStr;
        }

        private int dID;
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DID
        {
            get { return dID; }
            set { dID = value; }
        }

        private string dName;
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DName
        {
            get { return dName; }
            set { dName = value; }
        }

        private string dDescription;
        /// <summary>
        /// 部门描述（职责等）
        /// </summary>
        public string DDescription
        {
            get { return dDescription; }
            set { dDescription = value; }
        }

        private int dParentID;
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DParentID
        {
            get { return dParentID; }
            set { dParentID = value; }
        }

        private int dPriority;
        /// <summary>
        /// 顺序优先级
        /// </summary>
        public int DPriority
        {
            get { return dPriority; }
            set { dPriority = value; }
        }

        private string dModelIDStr;
        /// <summary>
        /// 部门的拥有权限 1|v,2|v,11|v|a|d|u
        /// </summary>
        public string DModelIDStr
        {
            get { return dModelIDStr; }
            set { dModelIDStr = value; }
        }
    }
}
