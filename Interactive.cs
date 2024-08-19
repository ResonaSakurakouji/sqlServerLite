using System.Collections;
using System.Data.SqlClient;
using System.Data;

namespace sqlServerLite
{
    internal class Interactive
    {
        public static void Exe(string sqlQuery, SqlConnection connection)
        {
            try
            {
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
                        reader.Close();
                        if (count > 0)
                        {
                            Console.WriteLine(ArrayList2Table.AutoWidth(display_ArrayList));
                            Console.WriteLine($"查询到行数: 【{count}】");
                        }
                        else if (sqlQuery.Trim().StartsWith("use", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"数据库已切换");
                        }
                        else if (!(sqlQuery.Trim().StartsWith("use", StringComparison.OrdinalIgnoreCase)))
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected < 0)
                            {
                                Console.WriteLine("！无效的操作");
                            }
                            else
                            {
                                Console.WriteLine($"受影响行数: 【{rowsAffected}】");
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    Console.WriteLine($"Error {i}: {ex.Errors[i].Message}");
                }
            }
        }

        // 处理异步的取消功能
        public static async Task ExeAsync(string sqlQuery, SqlConnection connection, CancellationToken cancellationToken)
        {
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                try
                {
                    // Execute the query asynchronously
                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("数据库操作已取消。");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"执行 SQL 查询时出错: {ex.Message}");
                }
            }
        }
    }
}
