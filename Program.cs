using System;
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
                    // 打开连接
                    connection.Open();

                    // 连接已打开，可以执行查询或其他操作
                    Console.WriteLine("连接成功！");

                    // 示例：执行查询
                    string sqlQuery = "SELECT name FROM sys.databases";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    Console.Write($"{reader[i],-20}");
                                }
                                Console.WriteLine();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接失败: " + ex.Message);
                }
            }
        }
    }
}
