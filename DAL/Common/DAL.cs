using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.DAL.Common
{
    public class DAL
    {
        public const string connectionString = @"server=L-PC197T2Z;database=CarpoolManagement;uid=wbpoc;pwd=sql@tfs2008";

        public SqlConnection connection;

        public DAL()
        {
            connection = new SqlConnection(connectionString);
            OpenConnection();
        }

        public void OpenConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void CloseConnection()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
            }
        }
    }
}