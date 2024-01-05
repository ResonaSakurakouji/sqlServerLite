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
        public const string LineX = "x";
        public const string LineP = "+";
        public const string LineV = "¦";
        public const string Line2V = "‖";

        public static string AutoWidth<T>(List<T> arrayList)
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

        public static string AutoWidth(ArrayList arrayList)
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
    }
}
