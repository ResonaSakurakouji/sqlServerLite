namespace sqlServerLite
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;

    public class Data2ArrayList
    {
        public static ArrayList Record2ArrayList(IDataRecord record)
        {
            ArrayList result = new ArrayList();

            if (record != null)
            {
                for (int i = 0; i < record.FieldCount; i+=1)
                {
                    object value = record[i];
                    result.Add(value.ToString());
                }
            }
            
            return result;
        }

        private static string[] GetFieldNames(SqlDataReader reader)
        {
            string[] fieldNames = new string[reader.FieldCount];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                fieldNames[i] = reader.GetName(i);
            }
            return fieldNames;
        }

        public static string Record2String(IDataRecord record)
        {
            if (record == null || record.FieldCount == 0)
            {
                return string.Empty;
            }

            string result = "{";

            for (int i = 0; i < record.FieldCount; i++)
            {
                string fieldName = record.GetName(i);
                string fieldValue = record.IsDBNull(i) ? "NULL" : record[i].ToString();

                if (i > 0)
                {
                    result += ", ";
                }

                result += $"{fieldName}={fieldValue}";
            }

            result += "}";

            return result;
        }
    }
}
