using JEDI_Carpool.DAL.Common;
using JEDI_Carpool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace JEDI_Carpool.DAL
{
    public class CarDAL
    {
        private const string GetCarQuery = @"
            SELECT * FROM Car
            WHERE DriverId = @DriverId";

        public static CarModel GetCar(int DriverId)
        {
            CarModel car = null;
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DriverId", DriverId));

            var dt = DBCommand.GetDataWithCondition(GetCarQuery, parameters);
            foreach (DataRow row in dt.Rows)
            {
                car = new CarModel();
                car.CarId = int.Parse(row["CarId"].ToString());
                car.PlateNumber = row["PlateNumber"].ToString();
                car.Model = row["Model"].ToString();
                car.Year = int.Parse(row["Year"].ToString());
                car.Color = row["Color"].ToString();
                car.Seat = int.Parse(row["Seat"].ToString());
            }

            return car;
        }


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


        private const string EditCarQuery = @"
            UPDATE Car SET
            PlateNumber = @PlateNumber,
            Model = @Model,
            Year = @Year,
            Color = @Color,
            Seat = @Seat
            WHERE DriverId = @DriverId";
        public static bool Edit(CarModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DriverId", model.DriverId));
            parameters.Add(new SqlParameter("@PlateNumber", model.PlateNumber));
            parameters.Add(new SqlParameter("@Model", model.Model));
            parameters.Add(new SqlParameter("@Year", model.Year));
            parameters.Add(new SqlParameter("@Color", model.Color));
            parameters.Add(new SqlParameter("@Seat", model.Seat));

            return DBCommand.InsertUpdateData(EditCarQuery, parameters);
        }


        private const string DeleteCarQuery = @"
            DELETE FROM Car WHERE DriverId = @DriverId";
        public static bool Delete(int DriverId)
        {
            var parameter = new SqlParameter("@DriverId", DriverId);

            return DBCommand.DeleteData(DeleteCarQuery, parameter);
        }
    }
}