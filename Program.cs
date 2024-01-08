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
                    Console.WriteLine("连接成功！退出请单独输入【exit;】");
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
                    while (true)
                    {
                        sqlQuery1 += Console.ReadLine();
                        if (sqlQuery1.Contains(';'))
                        {
                            if (Regex.IsMatch(sqlQuery1, @"exit\s*;", RegexOptions.IgnoreCase))
                            {
                                return;
                            }
                            sqlQuery1 = sqlQuery1[..sqlQuery1.IndexOf(';')];
                            break;
                        }
                    }
                    Interactive.Exe(sqlQuery1, connection);
                }
            }
        }
    }
}
