using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLibrary.Tool
{
    /// <summary>
    /// 题库工具类
    /// </summary>
    public class ExamTool
    {
        /// <summary>
        /// 复合题ID集合
        /// </summary>
        public static string parentTypeNoStr = "9,11,28";//复合题字符串,如 9,11

        /// <summary>
        /// 获取题目类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetQuestionType(int type)
        {
            try
            {
                string[] TypeData = { "单选题", "多选题", "判断题", "填空题", "连线题", "改错题", "选词填空", "完型填空", "阅读理解", "快速阅读", "多题干共选项题", "问答题", "简答题", "论述题", "翻译题", "计算题", "作文题", "案例题", "材料题", "推理题", "名词解释", "文言文", "现代文", "诗词鉴赏", "材料分析", "案例分析", "实务操作题", "会计计算分析题", "填空题", "会计分录", "选词填空" };
                return TypeData[type - 1];
            }
            catch (Exception)
            {
                return "未知题型";
            }
        }


        /// <summary>
        /// 获取ABC
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetABC(int index)
        {
            try
            {
                string[] optionABCdata = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1", "I1", "J1", "K1", "L1", "M1", "N1", "O1", "P1", "Q1", "R1", "S1", "T1", "U1", "V1", "W1", "X1", "Y1", "Z1" };
                return optionABCdata[index];
            }
            catch (Exception)
            {
                return "未知选项";
            }
        }

        /// <summary>
        /// 非复合题（跟parentID not null表示题目最小单位，但并不能说明有无子题，如果单选题和阅读理解都是属于这个范畴，但单选题没子题，阅读理解题有子题）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool GetNotBigQuestionType(int type)
        {
            string[] fuhe = parentTypeNoStr.Split(',');
            for (int i = 0; i < fuhe.Length; i++)
            {
                if (fuhe[i] == type.ToString())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将单双引号转为自定义字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DeReplaceSpecial(string str) {
            return str.Replace("(qiuzhikejiA)", "'").Replace("(qiuzhikejiB)", "\"");
        }
    }
}
