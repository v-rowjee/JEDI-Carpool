using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JEDI_Carpool.DAL
{
    public class CarDAL
    {
        private const string CreateCarQuery = @"
            INSERT INTO Car (DriverId, PlateNumber, Model, Year, Color, Seat) 
            VALUES (@DriverId, @PlateNumber, @Model, @Year, @Color, @Seat)
        ";

        public static bool Create(CarModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DriverId", model.DriverId));
            parameters.Add(new SqlParameter("@PlateNumber", model.PlateNumber));
            parameters.Add(new SqlParameter("@Model", model.Model));
            parameters.Add(new SqlParameter("@Year", model.Year));
            parameters.Add(new SqlParameter("@Color", model.Color));
            parameters.Add(new SqlParameter("@Seat", model.Seat));

            return DBCommand.InsertUpdateData(CreateCarQuery, parameters);

        }
    }
}