using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Sys
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class Sys_Links
    {
        public Sys_Links() { }

        public Sys_Links(int LID, string LinkName, string LinkURL,string Remarks,int NewsClassID, int Priority, DateTime AddDate)
        {
            lID = LID;
            linkName = LinkName;
            linkURL = LinkURL;
            remarks = Remarks;
            newsClassID = NewsClassID;
            priority = Priority;
            addDate = AddDate;
        }

        private int lID;
        /// <summary>
        /// 友链ID
        /// </summary>
        public int LID
        {
            get { return lID; }
            set { lID = value; }
        }

        private string linkName;
        /// <summary>
        /// 友链名称
        /// </summary>
        public string LinkName
        {
            get { return linkName; }
            set { linkName = value; }
        }

        private string linkURL;
        /// <summary>
        /// 友链URL
        /// </summary>
        public string LinkURL
        {
            get { return linkURL; }
            set { linkURL = value; }
        }

        private string remarks;
        /// <summary>
        /// 友链备注信息
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        private int newsClassID;
        /// <summary>
        /// 对应的新闻站点ID（主站为0）
        /// </summary>
        public int NewsClassID
        {
            get { return newsClassID; }
            set { newsClassID = value; }
        }

        private int priority;
        /// <summary>
        /// 排序（升序）
        /// </summary>
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        private DateTime addDate;
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        private string addDateTemp;
        /// <summary>
        /// 添加时间字符串（临时字段）
        /// </summary>
        public string AddDateTemp
        {
            get { return addDateTemp; }
            set { addDateTemp = value; }
        }
    }
}
