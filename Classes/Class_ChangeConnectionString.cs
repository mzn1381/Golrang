using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace PCLOR
{
    public partial class Class_ChangeConnectionString
    {
        public enum DbName { PACNT, PBANK, PBASE, PERP_MAIN, PSALE, PWHRS, PSTND,PCLOR }
        public static string CurrentConnection;
        public static string SetConnection(DbName dbName)
        {
            if (CurrentConnection != null)
            {
                string ConnectionString = null;
                System.Data.SqlClient.SqlConnection Con =
                    new System.Data.SqlClient.SqlConnection(CurrentConnection);

                if (dbName.ToString() == "PBASE")
                {
                    string DatabaseName = dbName.ToString();
                    DatabaseName += "_" + Con.Database.Substring(Con.Database.IndexOf("_") + 1, Con.Database.LastIndexOf("_") - Con.Database.IndexOf("_") - 1);
                    ConnectionString = CurrentConnection.Replace(Con.Database, DatabaseName);
                }
                else if (dbName.ToString() != "PERP_MAIN")
                    ConnectionString = CurrentConnection.Replace("PCLOR", dbName.ToString());
                else
                    ConnectionString = CurrentConnection.Replace(Con.Database, dbName.ToString());
                return ConnectionString;
            }
            else
                return null;
        }

       
    }
}
