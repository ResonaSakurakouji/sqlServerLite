namespace sqlServerLite
{
    using System;
    using System.Collections;
    using System.Data;

    public class DataRecord2ArrayList
    {
        public static ArrayList Data2ArrayList(IDataRecord record)
        {
            ArrayList result = new ArrayList();

            if (record != null)
            {
                for (int i = 0; i < record.FieldCount; i++)
                {
                    object value = record[i];
                    result.Add(value.ToString());
                }
            }
            
            return result;
        }
    }
}
