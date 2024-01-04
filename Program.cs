using System.Collections;
using System.Data;
using System.Data.SqlClient;
namespace sqlServerLite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 定义连接字符串
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

                // 示例：执行查询
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
                            sqlQuery1 = sqlQuery1[..sqlQuery1.IndexOf(';')];
                            break;
                        }
                    }
                    if (sqlQuery1.ToLower() == "exit;")
                    {
                        return;
                    }
                    Interactive.Exe(sqlQuery1, connection);
                }
            }
        }
    }
}
