using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.DAL.Common
{
    public class DAL
    {
        //public const string connectionString = @"server=L-PC197T2Z;database=JEDICarpool_db;uid=wbpoc;pwd=sql@tfs2008";
        public const string connectionString = @"server=jedicarpooldbserver.database.windows.net;database=JEDICarpool_db;uid=wbpoc;pwd=sql@tfs2008";

        public SqlConnection Connection;

        public DAL()
        {
            Connection = new SqlConnection(connectionString);
            OpenConnection();
        }

        public void OpenConnection()
        {
            try
            {
                if (Connection.State == System.Data.ConnectionState.Open)
                {
                    Connection.Close();
                }

                Connection.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void CloseConnection()
        {
            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }
    }
}