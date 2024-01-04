using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    if (!char.IsLetterOrDigit(ch))
                    {
                        spaces -= 1;
                    }
                }

                result = spaces % 2 == 0
                    ? str.PadLeft(length - spaces / 2)
                    : str.PadLeft(length - spaces / 2 - 1);

                result = result.PadRight(length);
            }

            return result;
        }

        public static string Str2LengthLeft(string str, int length)
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
                    if (!char.IsLetterOrDigit(ch))
                    {
                        spaces -= 1;
                    }
                }

                result = str.PadRight(length - spaces);
            }

            return result;
        }

        public static int GetStrLength(string str)
        {
            int result = str.Length;
            foreach (char ch in str)
            {
                if (!char.IsLetterOrDigit(ch))
                {
                    result += 1;
                }
            }
            return result;
        }
    }
}
