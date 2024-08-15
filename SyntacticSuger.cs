using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqlServerLite
{
    internal class SyntacticSuger
    {
        // 定义 MySQL 到 SQL Server 的命令映射
        private static readonly Dictionary<string, string> commandMappings = new Dictionary<string, string>
        {
            { "SHOW DATABASES;", "SELECT name AS 'Database' FROM sys.databases;" },
            { "SHOW TABLES;", "SELECT TABLE_NAME AS 'Tables' FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';" },
            { "SHOW VIEWS;", "SELECT TABLE_NAME AS 'Views' FROM INFORMATION_SCHEMA.VIEWS;" },
            { "SHOW TABLE STATUS;", "SELECT name AS 'Table', create_date AS 'CreationDate', modify_date AS 'LastModified' FROM sys.tables;" },
            { "SHOW VIEW STATUS;", "SELECT name AS 'View', create_date AS 'CreationDate', modify_date AS 'LastModified' FROM sys.views;" }
        };

        // 转换 MySQL 语法为 SQL Server 语法
        public static string ConvertToSqlServer(string sqlQuery)
        {
            // 移除首尾空白字符并转换为大写
            sqlQuery = sqlQuery?.Trim().ToUpper();

            // 空查询直接返回
            if (string.IsNullOrEmpty(sqlQuery))
            {
                return string.Empty;
            }

            // 匹配映射命令
            if (commandMappings.ContainsKey(sqlQuery))
            {
                return commandMappings[sqlQuery];
            }

            // 处理 SHOW COLUMNS FROM 或 DESCRIBE
            if (sqlQuery.StartsWith("SHOW COLUMNS FROM") || sqlQuery.StartsWith("DESCRIBE"))
            {
                string tableName = GetTableName(sqlQuery);
                if (!string.IsNullOrEmpty(tableName))
                {
                    return $"SELECT COLUMN_NAME AS '字段名', DATA_TYPE AS '数据类型', IS_NULLABLE AS '可空', COLUMN_DEFAULT AS '默认值' FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}';";
                }
            }

            // 处理 SHOW INDEX FROM
            if (sqlQuery.StartsWith("SHOW INDEX FROM"))
            {
                string tableName = GetTableName(sqlQuery);
                if (!string.IsNullOrEmpty(tableName))
                {
                    return $"EXEC sp_helpindex '{tableName}';";
                }
            }

            // 处理 SHOW CREATE TABLE
            if (sqlQuery.StartsWith("SHOW CREATE TABLE"))
            {
                string tableName = GetTableName(sqlQuery);
                if (!string.IsNullOrEmpty(tableName))
                {
                    return $"EXEC sp_helptext '{tableName}';";
                }
            }

            // 处理 SHOW CREATE VIEW
            if (sqlQuery.StartsWith("SHOW CREATE VIEW"))
            {
                string viewName = GetTableName(sqlQuery);
                if (!string.IsNullOrEmpty(viewName))
                {
                    return $"EXEC sp_helptext '{viewName}';";
                }
            }

            // 如果没有匹配到特定的 MySQL 命令，则返回原始 SQL 查询
            return sqlQuery;
        }

        // 提取表名或视图名
        private static string GetTableName(string sqlQuery)
        {
            string[] parts = sqlQuery.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 4)
            {
                return parts[3].Trim(';');
            }
            else if (parts.Length >= 2 && (parts[0].Equals("DESCRIBE", StringComparison.OrdinalIgnoreCase) || parts[0].Equals("SHOW", StringComparison.OrdinalIgnoreCase)))
            {
                return parts[1].Trim(';');
            }

            return string.Empty;
        }
    }
}
