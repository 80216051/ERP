using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sys
{
    /// <summary>
    /// 后台菜单及权限类
    /// </summary>
    public class Sys_ModelTable
    {
        public Sys_ModelTable() { }

        public Sys_ModelTable(string ModelName, string URL, int ParentID, int Display, int IsTop,string IconPic, string ActionID)
        {
            modelName = ModelName;
            uRL = URL;
            parentID = ParentID;
            display = Display;
            isTop = IsTop;
            iconPic = IconPic;
            actionID = ActionID;
        }

        private int modelID;
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModelID
        {
            get { return modelID; }
            set { modelID = value; }
        }

        private string modelName;
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        private string uRL;
        /// <summary>
        /// 模块链接地址
        /// </summary>
        public string URL
        {
            get { return uRL; }
            set { uRL = value; }
        }

        private int parentID;
        /// <summary>
        /// 父模块ID
        /// </summary>
        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        private int display;
        /// <summary>
        /// 是否在后台菜单树显示。1显示，否0
        /// </summary>
        public int Display
        {
            get { return display; }
            set { display = value; }
        }

        private int isTop;
        /// <summary>
        /// 是否置顶
        /// </summary>
        public int IsTop
        {
            get { return isTop; }
            set { isTop = value; }
        }

        private int priority;
        /// <summary>
        /// 显示优先级
        /// </summary>
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        private string iconPic;
        /// <summary>
        /// 图标
        /// </summary>
        public string IconPic
        {
            get { return iconPic; }
            set { iconPic = value; }
        }

        private string actionID;
        /// <summary>
        /// 操作权限标志字符
        /// v访问
        /// a增加
        /// d删除
        /// u编辑
        /// </summary>
        public string ActionID
        {
            get { return actionID; }
            set { actionID = value; }
        }
    }
}
