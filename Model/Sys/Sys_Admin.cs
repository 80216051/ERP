using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sys
{
    /// <summary>
    /// 后台管理员类
    /// </summary>
    [Serializable]
    public class Sys_Admin
    {
        public Sys_Admin() { }

        public Sys_Admin(string AUserName, string APass, string Asalt, string AName, string ANiceName, string APhoto, string ASex, string AContact, int AIsUse, string AModelIDStr, int ARole, int DID, DateTime AInputDate, int IsEveryday, string DayCode) 
        {
            this.aUserName = AUserName;
            this.aPass = APass;
            this.asalt = Asalt;
            this.aName = AName;
            this.aNiceName = ANiceName;
            this.aPhoto = APhoto;
            this.aSex = ASex;
            this.aIsUse = AIsUse;
            this.aContact = AContact;
            this.AModelIDStr = AModelIDStr;
            this.aRole = ARole;
            this.dID = DID;
            this.aInputDate = AInputDate;
            this.isEveryday = IsEveryday;
            this.dayCode = DayCode;
        }


        public Sys_Admin(string AUserName, string APass, string Asalt, string AName, string ANiceName, string APhoto, string ASex, string AContact, int AIsUse, string AModelIDStr, int ARole, int DID, DateTime AInputDate, int IsEveryday, string DayCode, string ClassArray)
        {
            this.aUserName = AUserName;
            this.aPass = APass;
            this.asalt = Asalt;
            this.aName = AName;
            this.aNiceName = ANiceName;
            this.aPhoto = APhoto;
            this.aSex = ASex;
            this.aIsUse = AIsUse;
            this.aContact = AContact;
            this.AModelIDStr = AModelIDStr;
            this.aRole = ARole;
            this.dID = DID;
            this.aInputDate = AInputDate;
            this.isEveryday = IsEveryday;
            this.dayCode = DayCode;
            this.classArray = ClassArray;
        }


        public Sys_Admin(string AUserName,string APass) 
        {
            this.aUserName = AUserName;
            this.aPass = APass;

        }

        private int aID;
        /// <summary>
        /// 管理员ID
        /// </summary>
        public int AID
        {
            get { return aID; }
            set { aID = value; }
        }

        private string aUserName;
        /// <summary>
        /// 帐号
        /// </summary>
        public string AUserName
        {
            get { return aUserName; }
            set { aUserName = value; }
        }

        private string aPass;
        /// <summary>
        /// 密码
        /// </summary>
        public string APass
        {
            get { return aPass; }
            set { aPass = value; }
        }
        private string asalt;
        /// <summary>
        /// 密码盐值
        /// </summary>
        public string Asalt
        {
            get { return asalt; }
            set { asalt = value; }
        }
        private string aName;
        /// <summary>
        /// 姓名
        /// </summary>
        public string AName
        {
            get { return aName; }
            set { aName = value; }
        }

        private string aNiceName;
        /// <summary>
        /// 昵称
        /// </summary>
        public string ANiceName
        {
            get { return aNiceName; }
            set { aNiceName = value; }
        }

        private string aPhoto;
        /// <summary>
        /// 头像URL地址
        /// </summary>
        public string APhoto
        {
            get { return aPhoto; }
            set { aPhoto = value; }
        }

        private string aSex;
        /// <summary>
        /// 性别
        /// </summary>
        public string ASex
        {
            get { return aSex; }
            set { aSex = value; }
        }

        private string aContact;
        /// <summary>
        /// 联系电话
        /// </summary>
        public string AContact
        {
            get { return aContact; }
            set { aContact = value; }
        }

        private int aIsUse;
        /// <summary>
        /// 是否启用：0锁定，1正常
        /// </summary>
        public int AIsUse
        {
            get { return aIsUse; }
            set { aIsUse = value; }
        }

        private string aModelIDStr;
        /// <summary>
        /// 模块字符串（用于控制权限）,ID用英文逗号隔开
        /// </summary>
        public string AModelIDStr
        {
            get { return aModelIDStr; }
            set { aModelIDStr = value; }
        }

        private int aRole;
        /// <summary>
        /// 角色类型。1超管，0普管
        /// </summary>
        public int ARole
        {
            get { return aRole; }
            set { aRole = value; }
        }

        private int dID;
        /// <summary>
        /// 所属部门ID
        /// </summary>
        public int DID
        {
            get { return dID; }
            set { dID = value; }
        }

        private DateTime aInputDate;
        /// <summary>
        /// 管理员添加时间
        /// </summary>
        public DateTime AInputDate
        {
            get { return aInputDate; }
            set { aInputDate = value; }
        }

        private int isEveryday;
        /// <summary>
        /// 是否每天发放注册码 当不为-1时，即为每天发放数量
        /// </summary>
        public int IsEveryday
        {
            get { return isEveryday; }
            set { isEveryday = value; }
        }

        private string dayCode;
        /// <summary>
        /// 当前日期的取码数量
        /// </summary>
        public string DayCode
        {
            get { return dayCode; }
            set { dayCode = value; }
        }

        private string aInputDateTemp;
        /// <summary>
        /// 创建时间的临时字段
        /// </summary>
        public string AInputDateTemp
        {
            get { return aInputDateTemp; }
            set { aInputDateTemp = value; }
        }

        private string roleStrTemp;
        /// <summary>
        /// 管理员角色（临时字段）
        /// </summary>
        public string RoleStrTemp
        {
            get { return roleStrTemp; }
            set { roleStrTemp = value; }
        }

        private string departmentTemp;
        /// <summary>
        /// 所属部门名称（临时字段）
        /// </summary>
        public string DepartmentTemp
        {
            get { return departmentTemp; }
            set { departmentTemp = value; }
        }


        private string classArray;
        /// <summary>
        ///分类权限
        /// </summary>
        public string ClassArray
        {
            get { return classArray; }
            set { classArray = value; }
        }


      
        
    }

    [Serializable]
    public class AjaxClass
    {
        public string Msg { get; set; }
        public int Result { get; set; }
    }
}
