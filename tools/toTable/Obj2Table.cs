using sqlServerLite.tools.textFormat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlServerLite.tools.toTable
{
    public class Obj2Table
    {
        public const string Line3H = "≡";
        public const string Line2H = "=";
        public const string Line1H = "-";
        public const string LineTH = "¯";
        public const string LineBH = "_";
        public const string LineX  = "x";
        public const string LineP  = "+";
        public const string LineV  = "¦";
        public const string Line2V = "‖";
        public const char  cLine3H = '≡';
        public const char  cLine2H = '=';
        public const char  cLine1H = '-';
        public const char  cLineTH = '¯';
        public const char  cLineBH = '_';
        public const char  cLineX  = 'x';
        public const char  cLineP  = '+';
        public const char  cLineV  = '¦';
        public const char  cLine2V = '‖';

        // 将 IDataRecord 直接转换为表格字符串
        public static string TableMake(IDataRecord record)
        {
            if (record == null || record.FieldCount == 0) return string.Empty;

            string result = LineP;
            int[] widths = new int[record.FieldCount];
            string[] titles = new string[record.FieldCount];
            string[][] showStrings = new string[1][];

            // 获取字段名称并计算列宽
            for (int i = 0; i < record.FieldCount; i++)
            {
                titles[i] = record.GetName(i);
                widths[i] = Str2Length.GetStrLength(titles[i]);
            }

            // 获取记录中的数据并计算每列的最大宽度
            showStrings[0] = new string[record.FieldCount];
            for (int i = 0; i < record.FieldCount; i++)
            {
                string value = record.IsDBNull(i) ? "NULL" : record[i].ToString();
                showStrings[0][i] = value;
                int valueLength = Str2Length.GetStrLength(value);
                if (valueLength > widths[i])
                {
                    widths[i] = valueLength;
                }
            }

            // 增加列宽度以增强可读性
            for (int i = 0; i < widths.Length; i++)
            {
                widths[i] += 2;
            }

            // 生成表头分隔线
            for (int j = 0; j < titles.Length; j++)
            {
                result += new string(cLine2H, widths[j]) + LineP;
            }
            result += "\n" + LineV;

            // 添加表头
            for (int j = 0; j < titles.Length; j++)
            {
                result += Str2Length.Str2LengthCenter(titles[j], widths[j]) + LineV;
            }
            result += "\n" + LineP;

            // 添加表头下的分隔线
            for (int j = 0; j < titles.Length; j++)
            {
                result += new string(cLine2H, widths[j]) + LineP;
            }
            result += "\n";

            // 添加表内容
            foreach (string[] showString in showStrings)
            {
                result += LineV;
                for (int i = 0; i < showString.Length; i++)
                {
                    // 如果是数字则右对齐，非数字则居中对齐
                    if (Str2Length.StrIsNumberDouble(showString[i]) || Str2Length.StrIsNumberInt(showString[i]))
                    {
                        result += Str2Length.Str2LengthRight(showString[i], widths[i]) + LineV;
                    }
                    else
                    {
                        result += Str2Length.Str2LengthCenter(showString[i], widths[i]) + LineV;
                    }
                }
                result += "\n";
            }
            // 生成底部分隔线
            for (int j = 0; j < titles.Length; j++)
            {
                result += LineP + new string(cLine1H, widths[j]);
            }
            result += LineP;
            return result;
        }
    }
}
