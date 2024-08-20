using System.Collections;

namespace sqlServerLite
{
    public class ArrayList2Table
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

        public static string AutoWidth_old1<T>(List<T> arrayList)
        {
            string result = LineP;
            int[] widths;
            string[] titles;
            string[][] showStrings = new string[arrayList.Count][];
            for (int i = 0; i < arrayList.Count; i += 1)
            {
                string buf0;
                if (arrayList[i] != null)
                {
                    buf0 = arrayList[i].ToString();
                }
                else
                {
                    buf0 = "";
                }
                string buf1 = buf0.Substring(buf0.IndexOf("{") + 1, buf0.IndexOf("}") - buf0.IndexOf("{") - 1);
                showStrings[i] = buf1.Split(", ");
            }
            titles = new string[showStrings[0].Length];
            widths = new int[showStrings[0].Length];
            for (int i = 0; i < titles.Length; i += 1)
            {
                titles[i] = showStrings[0][i].Split("=")[0];
                widths[i] = Str2Length.GetStrLength(titles[i]);
            }
            for (int j = 0; j < showStrings.Length; j += 1)
            {
                string[] showString = showStrings[j];
                for (int i = 0; i < titles.Length; i += 1)
                {
                    showString[i] = showStrings[j][i].Split("=")[1];
                    if (Str2Length.GetStrLength(showString[i]) > widths[i])
                    {
                        widths[i] = Str2Length.GetStrLength(showString[i]);
                    }
                }
                showStrings[j] = showString;
            }

            for (int i = 0; i < widths.Length; i += 1)
            {
                widths[i] += 2;
            }

            for (int j = 0; j < titles.Length; j += 1)
            {
                for (int i = 0; i < widths[j]; i += 1)
                {
                    result += Line2H;
                }
                result += LineP;
            }
            result += "\n" + LineV;
            for (int j = 0; j < titles.Length; j += 1)
            {
                result += Str2Length.Str2LengthCenter(titles[j], widths[j]) + LineV;
            }
            result += "\n" + LineP;
            for (int j = 0; j < titles.Length; j += 1)
            {
                for (int i = 0; i < widths[j]; i += 1)
                {
                    result += Line2H;
                }
                result += LineP;
            }
            result += "\n";
            foreach (string[] showString in showStrings)
            {
                result += LineV;
                for (int i = 0; i < showString.Length; i += 1)
                {
                    result += Str2Length.Str2LengthCenter(showString[i], widths[i]) + LineV;
                }
                result += "\n";
            }
            for (int j = 0; j < titles.Length; j += 1)
            {
                result += LineP;
                for (int i = 0; i < widths[j]; i += 1)
                {
                    result += Line1H;
                }
            }
            result += LineP;

            return result;
        }

        public static string AutoWidth<T>(List<T> arrayList)
        {
            if (arrayList == null || arrayList.Count == 0) return string.Empty;

            string result = LineP;
            int[] widths;
            string[] titles;
            string[][] showStrings = new string[arrayList.Count][];

            var properties = typeof(T).GetProperties();
            titles = properties.Select(p => p.Name).ToArray();
            widths = new int[titles.Length];

            for (int i = 0; i < titles.Length; i++)
            {
                widths[i] = Str2Length.GetStrLength(titles[i]);
            }
            for (int j = 0; j < arrayList.Count; j++)
            {
                showStrings[j] = new string[titles.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(arrayList[j], null)?.ToString() ?? string.Empty;
                    showStrings[j][i] = value;
                    int valueLength = Str2Length.GetStrLength(value);
                    if (valueLength > widths[i])
                    {
                        widths[i] = valueLength;
                    }
                }
            }
            for (int i = 0; i < widths.Length; i++)
            {
                widths[i] += 2;
            }
            for (int j = 0; j < titles.Length; j++)
            {
                result += new string(cLine2H, widths[j]) + LineP;
            }
            result += "\n" + LineV;
            for (int j = 0; j < titles.Length; j++)
            {
                result += Str2Length.Str2LengthCenter(titles[j], widths[j]) + LineV;
            }
            result += "\n" + LineP;
            for (int j = 0; j < titles.Length; j++)
            {
                result += new string(cLine2H, widths[j]) + LineP;
            }

            result += "\n";
            foreach (string[] showString in showStrings)
            {
                result += LineV;
                for (int i = 0; i < showString.Length; i++)
                {
                    result += Str2Length.Str2LengthCenter(showString[i], widths[i]) + LineV;
                }
                result += "\n";
            }
            for (int j = 0; j < titles.Length; j++)
            {
                result += LineP + new string(cLine2H, widths[j]);
            }
            result += LineP;

            return result;
        }

        public static string AutoWidth_old1(ArrayList arrayList)
        {
            string result = LineP;
            int[] widths;
            string[] titles;
            string[][] showStrings = new string[arrayList.Count][];
            for (int i = 0; i < arrayList.Count; i += 1)
            {
                string buf0;
                if (arrayList[i] != null)
                {
                    buf0 = (string)arrayList[i];
                }
                else
                {
                    buf0 = "";
                }
                string buf1 = buf0.Substring(buf0.IndexOf("{") + 1, buf0.IndexOf("}") - buf0.IndexOf("{") - 1);
                showStrings[i] = buf1.Split(", ");
            }
            titles = new string[showStrings[0].Length];
            widths = new int[showStrings[0].Length];
            for (int i = 0; i < titles.Length; i += 1)
            {
                titles[i] = showStrings[0][i].Split("=")[0];
                widths[i] = Str2Length.GetStrLength(titles[i]);
            }
            for (int j = 0; j < showStrings.Length; j += 1)
            {
                string[] showString = showStrings[j];
                for (int i = 0; i < titles.Length; i += 1)
                {
                    showString[i] = showStrings[j][i].Split("=")[1];
                    if (Str2Length.GetStrLength(showString[i]) > widths[i])
                    {
                        widths[i] = Str2Length.GetStrLength(showString[i]);
                    }
                }
                showStrings[j] = showString;
            }

            for (int i = 0; i < widths.Length; i += 1)
            {
                widths[i] += 2;
            }

            for (int j = 0; j < titles.Length; j += 1)
            {
                for (int i = 0; i < widths[j]; i += 1)
                {
                    result += Line2H;
                }
                result += LineP;
            }
            result += "\n" + LineV;
            for (int j = 0; j < titles.Length; j += 1)
            {
                result += Str2Length.Str2LengthCenter(titles[j], widths[j]) + LineV;
            }
            result += "\n" + LineP;
            for (int j = 0; j < titles.Length; j += 1)
            {
                for (int i = 0; i < widths[j]; i += 1)
                {
                    result += Line2H;
                }
                result += LineP;
            }
            result += "\n";
            foreach (string[] showString in showStrings)
            {
                result += LineV;
                for (int i = 0; i < showString.Length; i += 1)
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
            for (int j = 0; j < titles.Length; j += 1)
            {
                result += LineP;
                for (int i = 0; i < widths[j]; i += 1)
                {
                    result += Line1H;
                }
            }
            result += LineP;

            return result;
        }
        public static string AutoWidth(ArrayList arrayList)
        {
            if (arrayList == null || arrayList.Count == 0) return string.Empty;

            string result = LineP;
            int[] widths;
            string[] titles;
            string[][] showStrings = new string[arrayList.Count][];

            // 获取第一个对象的属性名称
            var properties = arrayList[0].GetType().GetProperties();
            titles = properties.Select(p => p.Name).ToArray();
            widths = new int[titles.Length];

            // 初始化列宽为属性名称的长度
            for (int i = 0; i < titles.Length; i++)
            {
                widths[i] = Str2Length.GetStrLength(titles[i]);
            }

            // 获取每个对象的属性值，并调整列宽
            for (int j = 0; j < arrayList.Count; j++)
            {
                var item = arrayList[j];
                showStrings[j] = new string[titles.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item, null)?.ToString() ?? string.Empty;
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
