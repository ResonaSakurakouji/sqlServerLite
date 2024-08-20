using sqlServerLite.tools.textFormat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        // 获取 IDataRecord 并作为字典列表保存
        public static List<Dictionary<string, string>> GetRecordsAsList(SqlDataReader reader)
        {
            List<Dictionary<string, string>> records = new List<Dictionary<string, string>>();

            while (reader.Read())
            {
                var record = new Dictionary<string, string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string fieldName = reader.GetName(i);
                    string fieldValue = reader.IsDBNull(i) ? "NULL" : reader[i].ToString();
                    record[fieldName] = fieldValue;
                }

                records.Add(record);
            }

            return records;
        }


        public static string TableMake(List<Dictionary<string, string>> records)
        {
            if (records == null || records.Count == 0) return string.Empty;

            var fieldNames = records[0].Keys.ToArray();
            int[] widths = new int[fieldNames.Length];
            string result = LineP;
            string[][] showStrings = new string[records.Count][];

            // 计算列宽
            for (int i = 0; i < fieldNames.Length; i++)
            {
                widths[i] = Str2Length.GetStrLength(fieldNames[i]);
            }

            // 获取记录中的数据并计算每列的最大宽度
            for (int j = 0; j < records.Count; j++)
            {
                showStrings[j] = new string[fieldNames.Length];
                for (int i = 0; i < fieldNames.Length; i++)
                {
                    string value = records[j][fieldNames[i]];
                    showStrings[j][i] = value;
                    int valueLength = Str2Length.GetStrLength(value);
                    if (valueLength > widths[i])
                    {
                        widths[i] = valueLength;
                    }
                }
            }

            // 增加列宽度以增强可读性
            for (int i = 0; i < widths.Length; i++)
            {
                widths[i] += 2;
            }

            // 生成表头分隔线
            for (int j = 0; j < fieldNames.Length; j++)
            {
                result += new string(cLine2H, widths[j]) + LineP;
            }
            result += "\n" + Line2V;

            // 添加表头
            for (int j = 0; j < fieldNames.Length; j++)
            {
                result += Str2Length.Str2LengthCenter(fieldNames[j], widths[j]) + Line2V;
            }
            result += "\n" + LineP;

            // 添加表头下的分隔线
            for (int j = 0; j < fieldNames.Length; j++)
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
            for (int j = 0; j < fieldNames.Length; j++)
            {
                result += LineP + new string(cLine1H, widths[j]);
            }
            result += LineP;

            return result;
        }
    }
}
