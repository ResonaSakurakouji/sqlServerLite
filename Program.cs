using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace sqlServerLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 定义连接字符串 Integrated Security=true 意思是开启windows凭证验证
            string connectionString = "Server=powerbi-prd,24333;Integrated Security=true;";

            // 建立数据库连接
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("连接成功！退出请单独输入【exit;】\n如有使用问题请联系【QinRuiZheng】");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接失败: " + ex.Message);
                }
                string sqlQuery0 = "SELECT name FROM sys.databases;";
                Interactive.Exe(sqlQuery0, connection);
                while (true)
                {
                    string sqlQuery1 = string.Empty;
                    Console.Write("Lite》");
                    bool doubleMarks = false;
                    bool doFlag = true;
                    while (doFlag)
                    {
                        sqlQuery1 += Console.ReadLine();
                        if (sqlQuery1.Contains(';'))
                        {
                            int semicolonCount = 1;
                            for (int i = 0; i< sqlQuery1.Length; i += 1)
                            {
                                if ((sqlQuery1[i] == '"'))
                                {
                                    doubleMarks = !doubleMarks;
                                }
                                if ((sqlQuery1[i].Equals(';')) && !doubleMarks) 
                                {
                                    semicolonCount += 1;
                                    if (semicolonCount > 1) 
                                    { 
                                        doFlag = false; 
                                        if (i == 0)
                                        {
                                            Console.WriteLine("？不推荐的写法：你在第一位输入了一个分号");
                                        }
                                        else
                                        {
                                            Console.WriteLine("？不推荐的写法：请勿连续输入两个英文分号【;】");
                                        }
                                        break; 
                                    }
                                }
                                else
                                {
                                    semicolonCount = 0;
                                }
                            }
                            if (doFlag == false) { break; }
                            if (Regex.IsMatch(sqlQuery1, @"exit\s*;", RegexOptions.IgnoreCase))
                            {
                                Console.WriteLine("またね~");
                                return;
                            }
                            break;
                        }
                    }
                    Interactive.Exe(sqlQuery1, connection);
                }
            }
        }
    }
}
