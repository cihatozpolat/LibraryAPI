using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace KütüphaneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        string _connectionString = "Server=.; Database=Kütüphane; Trusted_Connection=True;";

        // GET api/values
        [HttpGet("{isactive}/avaible")]
        public ActionResult GetAvaible([FromRoute] string isactive)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("select * from Books where Books.IsActive=" + isactive, con);
                SqlDataReader reader = command.ExecuteReader();
                List<object> book = new List<object>();
                while (reader.Read())
                {
                    book.Add(new
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        ISBN = Int32.Parse(reader["ISBN"].ToString()),
                        Name = reader["Name"].ToString(),
                        IsActive = (reader["IsActive"])
                    });
                }
                return new JsonResult(book);
            }
        }

        [HttpGet()]
        public ActionResult Get([FromQuery]string name, int isbn, int isactive)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("select * from Books where books.Name like '%" + name + "%' and books.ISBN like '%" + isbn + "%' and books.IsActive like '%" + isactive + "%' ", con);
                SqlDataReader reader = command.ExecuteReader();
                List<object> book = new List<object>();
                while (reader.Read())
                {
                    book.Add(new
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        ISBN = Int32.Parse(reader["ISBN"].ToString()),
                        Name = reader["Name"].ToString(),
                        IsActive = (reader["IsActive"])
                    });
                }
                return new JsonResult(book);
            }
        }

        // GET api/values
        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("select * from Books where Books.Id=" + id, con);
                SqlDataReader reader = command.ExecuteReader();
                object book = null;

                while (reader.Read())
                {
                    book = new
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        ISBN = Int32.Parse(reader["ISBN"].ToString()),
                        Name = reader["Name"].ToString(),
                        IsActive = (reader["IsActive"])
                    };
                }
                return new JsonResult(book);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromQuery]int isbn, string name, bool isactive)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("insert into books values(@ISBN,@Name,@IsActive)", con);
                //command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@ISBN", SqlDbType.Int).Value = isbn;
                command.Parameters.Add("@Name", SqlDbType.VarChar).Value = name;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isactive;
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        // PUT api/values
        [HttpPut()]
        public void Put([FromQuery] int id, int isbn, string name, bool isactive)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("update Books set ISBN=@isbn, Name=@name, IsActive=@isactive where Books.Id=@id", con);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@isbn", SqlDbType.Int).Value = isbn;
                command.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                command.Parameters.Add("@isactive", SqlDbType.Bit).Value = isactive;

                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public void Delete([FromRoute]int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("delete from Books where Books.Id=" + id, con);
                try
                {
                    SqlDataReader reader = command.ExecuteReader();
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }
    }
}


