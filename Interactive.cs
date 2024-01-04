
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
                            Console.WriteLine($"受影响行数: 【{rowsAffected}】");
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
    }
}
