using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using WebApiCar.Model;

namespace WebApiCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {

        static string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CarDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        //public static List<Car> carList = new List<Car>()
        //{
        //    new Car(){Id = 1,Model="x3",Vendor="Tesla", Price=400000},
        //    new Car(){Id = 2,Model="x2",Vendor="Tesla", Price=600000},
        //    new Car(){Id = 3,Model="x1",Vendor="Tesla", Price=800000},
        //    new Car(){Id = 4,Model="x0",Vendor="Tesla", Price=1400000},
        //};

        /// <summary>
        /// Method for get all the cars from the static list
        /// </summary>
        /// <returns>List of cars</returns>
        // GET: api/Cars
        [HttpGet]
        public IEnumerable<Car> Get()
        {
            var carList = new List<Car>();

            string selectall = "select id, vendor, model, price from Car";

            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                using (SqlCommand selectCommand = new SqlCommand(selectall, databaseConnection))
                {
                    databaseConnection.Open();

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string vendor = reader.GetString(1);
                            string model = reader.GetString(2);
                            int price = reader.GetInt32(3);

                            carList.Add(new Car(id,vendor,model,price));

                        }

                    }
                }


                    }

                return carList;
        }

        // GET: api/Cars/5
        [HttpGet("{id}", Name ="GetByID")]
        public Car Get(int id)
        { 
            var selectByID = "select id, vendor, model, price from Car where id= @id";

            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                using (SqlCommand selectCommand = new SqlCommand(selectByID, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@id", id);
                    databaseConnection.Open();

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id2 = reader.GetInt32(0);
                           string vendor = reader.GetString(1);
                            string model = reader.GetString(2);
                            int price = reader.GetInt32(3);
                            return new Car(id2, vendor, model, price);
                        }
                    }
                }
            }

            return null;
        }

        // get: /api/Cars/byVendor/ford
        [HttpGet(("byVendor/{vendor}"), Name = "GetByVendor")]

        public IEnumerable<Car> GetByVendor(string vendor)
        {
            var carList = new List<Car>();
            //Car c = new Car();
            string selectByVendor = "select id, model, price, vendor from Car where vendor = @vendor";

            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                using (SqlCommand selectCommand = new SqlCommand(selectByVendor, databaseConnection))
                {
                    selectCommand.Parameters.AddWithValue("@vendor", vendor);
                    databaseConnection.Open();

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                           string vendor2 = reader.GetString(1);
                            string model = reader.GetString(1);
                            int price = reader.GetInt32(2);


                            carList.Add(new Car(id, vendor2, model, price));
                           

                        }

                    }
                }

            }

            return carList;
        }


        /// <summary>
        /// Post a new car to the static list
        /// </summary>
        /// <param name="value"></param>
        // POST: api/Cars
        [HttpPost]
        public void Post([FromBody] Car value)
        {
           
          
            string insertSql = "insert into car(id, model, vendor, price) values(@id, @model, @vendor, @price)";

            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(insertSql, databaseConnection))
                {
                    insertCommand.Parameters.AddWithValue("@id", value.Id);
                    insertCommand.Parameters.AddWithValue("@model", value.Model);
                    insertCommand.Parameters.AddWithValue("@vendor", value.Vendor);
                    insertCommand.Parameters.AddWithValue("@price", value.Price);

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    Console.WriteLine($"rowsAffected: { rowsAffected}");

                }
                }

            }

        // PUT: api/Cars/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Car value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //carList.Remove(Get(id));
        }

       //int GetId()
       // {
       //    // int max = carList.Max(x => x.Id);
       //     //return max+1;
       // }

    }
}
