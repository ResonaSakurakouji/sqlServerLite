namespace sqlServerLite
{
    using System;
    using System.Collections.Generic;

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
            { "SHOW TABLE STATUS;",
                @"SELECT
                    t.name AS TableName,
                    col_count.ColumnsCount AS Fields,
                    row_count.RowsCount AS Records,
                    t.create_date AS CreationDate,
                    t.modify_date AS LastModified
                FROM
                    sys.tables t
                LEFT JOIN (
                    SELECT 
                        TABLE_NAME,
                        COUNT(*) AS ColumnsCount
                    FROM INFORMATION_SCHEMA.COLUMNS
                    GROUP BY TABLE_NAME
                ) AS col_count
                ON col_count.TABLE_NAME = t.name
                LEFT JOIN (
                    SELECT 
                        t.name AS TableName,
                        SUM(p.rows) AS RowsCount
                    FROM 
                        sys.tables t
                    INNER JOIN 
                        sys.partitions p ON p.object_id = t.object_id
                    WHERE 
                        p.index_id <= 1
                    GROUP BY 
                        t.name
                ) AS row_count
                ON row_count.TableName = t.name;"
            },
            { 
                "SHOW VIEW STATUS;",
                @"SELECT
                    v.name AS ViewName,
                    col_count.ColumnsCount AS Fields,
                    NULL AS Records,
                    v.create_date AS CreationDate,
                    v.modify_date AS LastModified
                FROM
                    sys.views v
                LEFT JOIN (
                    SELECT 
                        TABLE_NAME AS ViewName,
                        COUNT(*) AS ColumnsCount
                    FROM INFORMATION_SCHEMA.COLUMNS
                    GROUP BY TABLE_NAME
                ) AS col_count
                ON col_count.ViewName = v.name;"
            }
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
                        return $@"
                        SELECT 
                            COLUMN_NAME AS '字段名', 
                            DATA_TYPE AS '数据类型',
                            CASE 
                                WHEN DATA_TYPE IN ('decimal', 'numeric') THEN 
                                    DATA_TYPE + '(' + CAST(NUMERIC_PRECISION AS VARCHAR) + ',' + CAST(NUMERIC_SCALE AS VARCHAR) + ')'
                                WHEN DATA_TYPE IN ('char', 'varchar', 'nchar', 'nvarchar') THEN 
                                    DATA_TYPE + '(' + CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR) + ')'
                                ELSE DATA_TYPE
                            END AS '详细数据类型',
                            IS_NULLABLE AS '可空', 
                            COLUMN_DEFAULT AS '默认值',
                            COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') AS '是否自增',
                            (SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END 
                             FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                             WHERE TABLE_NAME = '{tableName}' AND COLUMN_NAME = COLUMNS.COLUMN_NAME AND CONSTRAINT_NAME LIKE 'PK%') AS '是否主键'
                        FROM INFORMATION_SCHEMA.COLUMNS COLUMNS
                        WHERE TABLE_NAME = '{tableName}';
                    ";
                    }
                }

                // 处理 SHOW INDEX FROM
                if (sqlQuery.StartsWith("SHOW INDEX FROM"))
                {
                    string tableName = GetTableName(sqlQuery);
                    if (!string.IsNullOrEmpty(tableName))
                    {
                        return $@"
                        SELECT 
                            INDEX_NAME AS '索引名', 
                            COLUMN_NAME AS '列名', 
                            IS_UNIQUE AS '唯一性', 
                            IS_PRIMARY_KEY AS '是否主键'
                        FROM sys.index_columns IC
                        JOIN sys.indexes I ON IC.object_id = I.object_id AND IC.index_id = I.index_id
                        JOIN sys.columns C ON IC.object_id = C.object_id AND IC.column_id = C.column_id
                        WHERE OBJECT_NAME(IC.object_id) = '{tableName}';
                    ";
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

}
