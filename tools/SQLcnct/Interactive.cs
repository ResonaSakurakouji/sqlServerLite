using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using sqlServerLite.tools.toTable;

namespace sqlServerLite.tools.SQLcnct
{
    internal class Interactive
    {
        public static string? ReadSqlQuery()
        {
            bool doFlag = true;
            string sqlQuery = string.Empty;
            bool insideSingleQuotes = false;
            bool insideDoubleQuotes = false;

            while (doFlag)
            {
                string input = Console.ReadLine();
                if (input == null)
                {
                    input = string.Empty;
                }

                // 处理换行情况，可能在引号内
                sqlQuery += input.Trim() + " ";

                for (int i = 0; i < input.Length; i++)
                {
                    char currentChar = input[i];

                    if (currentChar == '"' && !insideSingleQuotes)
                    {
                        insideDoubleQuotes = !insideDoubleQuotes;
                    }
                    else if (currentChar == '\'' && !insideDoubleQuotes)
                    {
                        insideSingleQuotes = !insideSingleQuotes;
                    }

                    if (currentChar == ';' && !insideDoubleQuotes && !insideSingleQuotes)
                    {
                        bool breakFlag = false;
                        if (i == 0)
                        {
                            breakFlag = true;
                        } 
                        else if(i > 0 && input[i - 1] == ';')
                        {
                            Console.WriteLine("？不推荐的写法：请勿连续输入两个英文分号【;】，第一个分号后内容已被忽略。");
                            breakFlag = true;
                        }
                        // 删除分号前的所有空白字符
                        int j = i - 1;
                        while (j >= 0 && char.IsWhiteSpace(input[j]))
                        {
                            j--;
                        }
                        input = input.Remove(j + 1, i - j - 1);
                        if (breakFlag) {break;}
                        doFlag = false; // 结束输入
                    }
                }

                // 检查退出命令
                if (Regex.IsMatch(sqlQuery, @"\bexit\s*;", RegexOptions.IgnoreCase))
                {
                    Console.WriteLine("またね~");
                    Environment.Exit(0);
                    return null;
                }
            }
            return sqlQuery;
        }

        public static void Exe(string sqlQuery, SqlConnection connection)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        /*
                        ArrayList display_ArrayList = new ArrayList();
                        int count = 0;
                        IDataRecord fieldName_Record = null;
                        while (reader.Read())
                        {
                            if (count == 0)
                            {
                                fieldName_Record = reader;
                                display_ArrayList.Add(Data2ArrayList.Record2String(fieldName_Record));
                            }
                            else
                            {
                                IDataRecord dataRecord = reader;
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
                        else if (!sqlQuery.Trim().StartsWith("use", StringComparison.OrdinalIgnoreCase))
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
                        }*/
                        // 获取记录并转换为 List<Dictionary<string, string>>
                        List<Dictionary<string, string>> records = Obj2Table.GetRecordsAsList(reader);

                        if (records.Count > 0)
                        {
                            // 生成并输出表格
                            string tableString = Obj2Table.TableMake(records);
                            Console.WriteLine(tableString);
                            Console.WriteLine($"查询到行数: 【{records.Count}】");
                        }
                        else if (sqlQuery.Trim().StartsWith("use", StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"数据库已切换");
                        }
                        else if (!sqlQuery.Trim().StartsWith("use", StringComparison.OrdinalIgnoreCase))
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
                int error_count = 0;
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
                    if (error_count > 0)
                    {
                        Console.WriteLine($"执行 SQL 查询时出错: {ex.Message}");
                    }
                    error_count += 1;
                }
            }
        }
    }
}
