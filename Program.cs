using sqlServerLite.sqlServerLite;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading;
namespace sqlServerLite
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 定义连接字符串 Integrated Security=true 意思是开启windows凭证验证
            string connectionString = "Server=powerbi-prd,24333;Integrated Security=true;";
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressHandler);

            // 建立数据库连接
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    Console.WriteLine("连接成功！退出请单独输入【exit;】\n如有使用问题请联系【QinRuiZheng】");

                    string sqlQuery0 = "SELECT name AS 'Database' FROM sys.databases;";
                    await Interactive.ExeAsync(string.Empty, connection, cancellationTokenSource.Token);
                    Interactive.Exe(sqlQuery0, connection);

                    while (true)
                    {
                        string sqlQuery1 = string.Empty;
                        Console.Write("Lite》");
                        bool insideDoubleQuotes = false;  // 双引号内部判定
                        bool insideSingleQuotes = false;  // 单引号内部判定
                        bool doFlag = true;

                        while (doFlag)
                        {
                            string input = Console.ReadLine();
                            input = input == null ? " " : (input + " ");
                            sqlQuery1 += input;

                            for (int i = 0; i < input.Length; i++)
                            {
                                if (input[i] == '"' && !insideSingleQuotes)
                                {
                                    insideDoubleQuotes = !insideDoubleQuotes;
                                }
                                else if (input[i] == '\'' && !insideDoubleQuotes)
                                {
                                    insideSingleQuotes = !insideSingleQuotes;
                                }

                                if (input[i] == ';' && !insideDoubleQuotes && !insideSingleQuotes)
                                {
                                    if (i == 0 || (i > 0 && input[i - 1] == ';'))
                                    {
                                        Console.WriteLine("？不推荐的写法：请勿连续输入两个英文分号【;】");
                                        doFlag = false;
                                        break;
                                    }
                                    else
                                    {
                                        doFlag = false;
                                    }
                                }
                            }

                            if (Regex.IsMatch(sqlQuery1, @"\bexit\s*;", RegexOptions.IgnoreCase))
                            {
                                Console.WriteLine("またね~");
                                return;
                            }
                        }

                        // 执行 SQL 查询
                        if (!string.IsNullOrWhiteSpace(sqlQuery1))
                        {
                            string sqlQuery2 = SyntacticSuger.ConvertToSqlServer(sqlQuery1);
                            Interactive.Exe(sqlQuery2, connection);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连接失败: " + ex.Message);
                    Console.WriteLine("一般来说就是没权限；\n你应该用公司的电脑适用这个软件；\n并且确保已经联系好IT为你开放数仓的访问权限");
                    Console.WriteLine("输入任意键退出~");
                    Console.ReadKey();
                }
            }
        }

        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Ctrl+C 处理函数
        private static void CancelKeyPressHandler(object sender, ConsoleCancelEventArgs e)
        {
            // 设置为 true 防止程序终止
            e.Cancel = true;
            Console.WriteLine("\n检测到【Ctrl】+【C】组合键，正在取消当前操作...");
            cancellationTokenSource.Cancel(); // 触发取消
        }
    }
}
