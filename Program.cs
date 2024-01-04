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
                    Console.WriteLine("连接成功！");

                    // 示例：执行查询
                    string sqlQuery = "SELECT name FROM sys.databases";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            ArrayList display_ArrayList = new ArrayList();
                            int count = 0;
                            IDataRecord fieldName_Record = null;
                            while (reader.Read())
                            {
                                if (count == 0)
                                {
                                    fieldName_Record = (IDataRecord)reader;
                                    display_ArrayList.Add(Data2ArrayList.Record2String(fieldName_Record));
                                }
                                else
                                {
                                    IDataRecord dataRecord = (IDataRecord)reader;
                                    display_ArrayList.Add(Data2ArrayList.Record2String(dataRecord));
                                }
                                count += 1;
                            }
                            Console.WriteLine(ArrayList2Table.AutoWidth(display_ArrayList));
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
