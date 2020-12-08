using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using WebApiCar.Model;

namespace WebApiCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        // we need to define our connection to database 
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
            // we set up the databse connection
            using (SqlConnection databaseConnection = new SqlConnection(conn))
            { 
                // we create a sql command 
               // We use the using so it closed the connection by itself we dont need to worry about the closing 
                //once we close the brackets connection is closed
                // sqlcommand takes 2 parameters, the query and the connection
                using (SqlCommand selectCommand = new SqlCommand(selectall, databaseConnection))
                {// we open the connection to the databse
                    databaseConnection.Open();
                    // reader to read from the database 
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    { // we use while loop so it keep looking for all the data 

                        while (reader.Read())
                        {
                            // the first column is int so I use .GetInt32
                            int id = reader.GetInt32(0);
                            // for string it is GetString
                            string vendor = reader.GetString(1);
                            string model = reader.GetString(2);
                            int price = reader.GetInt32(3);
                            // we create a new object and add it to the list and we return the list
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
            // in the query we can also write where id={id} it is the same and this is called sql injection since we are injecting the id 
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
        // in the teacher solution, he created a pricate method for the getAll so we dont have to have all the using 
        // and repetition for each methid that get objects from the database. and he called the method in the getBy Vendor
       
            // get: /api/Cars/byVendor/ford
        [HttpGet(("byVendor/{vendor}"), Name = "GetByVendor")]

        public IEnumerable<Car> GetByVendor(string vendor)
        {
            var carList = new List<Car>();
            
            string selectByVendor = "select id, model, price, vendor from Car where vendor = @vendor";

            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                using (SqlCommand selectCommand = new SqlCommand(selectByVendor, databaseConnection))
                {
                    // AddWithValue method:takes a string and an object as parameters 
                    //it adds a parameter and a value to an sql 
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

        // if we want to return something in this post method, we can use something else than void 
        public void Put(int id, [FromBody] Car value)
        {   
            string putQuery = " update Car set vendor =@vendor, model = @model, price =@price where id = @id";
            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                databaseConnection.Open();
                using (SqlCommand insertCommand = new SqlCommand(putQuery, databaseConnection))
                {
                   
                        insertCommand.Parameters.AddWithValue("@id", value.Id);
                        insertCommand.Parameters.AddWithValue("@model", value.Model);
                        insertCommand.Parameters.AddWithValue("@vendor", value.Vendor);
                        insertCommand.Parameters.AddWithValue("@price", value.Price);
                    // exectuteNonQurery method: it execute the sql statement and returns the number of affected rows 
                        int rowsAffected = insertCommand.ExecuteNonQuery();
                    
                        Console.WriteLine($"rowsAffected: { rowsAffected}");

                    }
                }
            
        }
        

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string deletequery = "delete from car where id=@id";
            using (SqlConnection dataBaseConnection = new SqlConnection (conn))
            {
                dataBaseConnection.Open();
                using (SqlCommand deleteCommand = new SqlCommand(deletequery, dataBaseConnection))
                {

                    deleteCommand.Parameters.AddWithValue("@id", id);

                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                }
            }
          
        }
    }

       //int GetId()
       // {
       //    // int max = carList.Max(x => x.Id);
       //     //return max+1;
       // }

    }

