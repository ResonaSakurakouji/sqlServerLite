using sqlServerLite.sqlServerLite;
using sqlServerLite.tools.SQLcnct;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace sqlServerLite
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int connectCount = 0;
            // 定义连接字符串 Integrated Security=true 意思是开启windows凭证验证
            string connectionString = "Server=powerbi-prd,24333;Integrated Security=true;";
            Console.CancelKeyPress += new ConsoleCancelEventHandler(CancelKeyPressHandler);

            Continue_InvalidOperationException:  // 建立数据库连接
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (connectCount == 0)
                    {
                        Console.WriteLine("连接成功！退出请单独输入【exit;】\n如有使用问题请联系【QinRuiZheng】");

                        string sqlQuery0 = "SELECT name AS 'Database' FROM sys.databases;";
                        Interactive.Exe(sqlQuery0, connection);
                    }
                    else {
                        Console.WriteLine("重新连接成功！"); 
                    }
                    connectCount += 1;
                    while (true)
                    {
                        string sqlQuery1 = string.Empty;
                        Console.Write("Lite》");
                        sqlQuery1 = Interactive.ReadSqlQuery();
                        // 执行 SQL 查询
                        if (!string.IsNullOrWhiteSpace(sqlQuery1))
                        {
                            string sqlQuery2 = SyntacticSuger.ConvertToSqlServer(sqlQuery1);
                            Interactive.Exe(sqlQuery2, connection);
                        }
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("你输入的内容不合法，连接被损毁！\n重新连接");
                    goto Continue_InvalidOperationException;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("连接失败信息: " + ex.Message);
                    Console.WriteLine("一般来说就是没权限；\n你应该用公司的电脑适用这个软件；\n并且确保已经联系好IT为你开放数仓的访问权限");
                    Console.WriteLine("输入任意键退出~");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }

        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Ctrl+C 处理函数
        private static void CancelKeyPressHandler(object sender, ConsoleCancelEventArgs e)
        {
            // 设置为 true 防止程序终止
            e.Cancel = true;
            Console.WriteLine("\n检测到【Ctrl】+【C】组合键。\n如欲强行停止，请输入【Q】；\n输入其他按键将放弃退出。");
            ConsoleKeyInfo cmdKey = Console.ReadKey();
            if (cmdKey.Key == ConsoleKey.Q)
            {
                Console.WriteLine("\n程序已停止。");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("\n取消退出，程序继续运行。");
            }
        }
    }
}
