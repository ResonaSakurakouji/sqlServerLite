using System.Text.RegularExpressions;

namespace sqlServerLite
{
    public class Str2Length
    {
        public static bool StrIsNumberInt(string str)
        {
            try
            {
                Convert.ToInt32(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool StrIsNumberDouble(string str)
        {
            try
            {
                Convert.ToDouble(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string Str2LengthCenter(string str, int length)
        {
            string result = "";
            if (StrIsNumberInt(str))
            {
                string formatLength = $"{{0,{length}}}";
                int resultInt = Convert.ToInt32(str);
                result = string.Format(formatLength, resultInt);
            }
            else if (StrIsNumberDouble(str))
            {
                int spaces = length - str.Length;
                result = str.PadLeft(length - spaces);
            }
            else
            {
                int spaces = length - str.Length;
                for (int i = 0; i < str.Length; i += 1)
                {
                    char ch = str[i];
                    string chStr = ch.ToString();
                    if (System.Text.RegularExpressions.Regex.IsMatch(chStr, @"[^\x00-\x7F]") &&
                        !chStr.Equals("—") && !chStr.Equals("―"))
                    {
                        spaces -= 1;
                    }
                }
                for (int i = 0; i < spaces; i += 1)
                {
                    if (i % 2 == 0)
                    {
                        str += " ";
                    }
                    else
                    {
                        result += " ";
                    }
                }
                result += str;
            }

            return result;
        }

        public static string Str2LengthLeft(string str, int length)
        {
            string result = "";

            if (StrIsNumberInt(str))
            {
                string formatLength = $"{{0,{length}}}";
                int resultInt = int.Parse(str);
                result = string.Format(formatLength, resultInt);
            }
            else if (StrIsNumberDouble(str))
            {
                int spaces = length - str.Length;
                for (int i = 0; i < spaces; i++)
                {
                    result += " ";
                }
                result += str;
            }
            else
            {
                int spaces = length - str.Length;
                foreach (char ch in str)
                {
                    string chStr = ch.ToString();
                    if (Regex.IsMatch(chStr, @"[^\x00-\x7F]") &&
                        !chStr.Equals("—") && !chStr.Equals("―"))
                    {
                        spaces--;
                    }
                }
                for (int i = 0; i < spaces; i++)
                {
                    result += " ";
                }
                result = str + result;
            }

            return result;
        }

        public static string Str2LengthRight(string str, int length)
        {
            string result = "";

            if (StrIsNumberInt(str))
            {
                string formatLength = $"{{0,{length}}}";
                int resultInt = int.Parse(str);
                result = string.Format(formatLength, resultInt);
            }
            else if (StrIsNumberDouble(str))
            {
                int spaces = length - str.Length;
                for (int i = 0; i < spaces; i++)
                {
                    result += " ";
                }
                result += str;
            }
            else
            {
                int spaces = length - str.Length;
                foreach (char ch in str)
                {
                    string chStr = ch.ToString();
                    if (Regex.IsMatch(chStr, @"[^\x00-\x7F]") &&
                        !chStr.Equals("—") && !chStr.Equals("―"))
                    {
                        spaces--;
                    }
                }
                for (int i = 0; i < spaces; i++)
                {
                    result += " ";
                }
                result = result + str;
            }

            return result;
        }

        public static int GetStrLength(string str)
        {
            int result = str.Length;
            for (int i = 0; i < str.Length; i += 1)
            {
                char ch = str[i];
                string chStr = ch.ToString();
                if (System.Text.RegularExpressions.Regex.IsMatch(chStr, @"[^\x00-\x7F]") &&
                    !chStr.Equals("—") && !chStr.Equals("―"))
                {
                    result += 1;
                }
            }
            return result;
        }
    }
}
