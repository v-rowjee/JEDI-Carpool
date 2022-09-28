using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.DAL.Common
{
    public class DBCommand
    {
        public static DataTable GetData(string query)
        {
            DAL dal = new DAL();
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, dal.connection))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }

            dal.CloseConnection();

            return dt;
        }

        public static void InsertUpdateData(string query, List<SqlParameter> parameters)
        {
            DAL dal = new DAL();
            using (SqlCommand cmd = new SqlCommand(query, dal.connection))
            {
                cmd.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                cmd.ExecuteNonQuery();
            }
            dal.CloseConnection();

        }

        public static DataTable GetDataWithCondition(string query, List<SqlParameter> parameters)
        {
            DAL dal = new DAL();
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, dal.connection))
            {
                cmd.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    parameters.ForEach(parameter =>
                    {
                        cmd.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                    });
                }
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }

            }

            dal.CloseConnection();

            return dt;
        }
    }
}