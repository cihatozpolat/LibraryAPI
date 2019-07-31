using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using KütüphaneAPI.Validations;
using Microsoft.AspNetCore.Mvc;

namespace KütüphaneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        string _connectionString = "Server=.; Database=Kütüphane; Trusted_Connection=True; Application Name = Cihat";

        // GET api/values
        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("select * from Users where Users.Id=" + id, con);
                SqlDataReader reader = command.ExecuteReader();
                object user = null;
                while (reader.Read())
                {
                    user = new
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        Name = reader["Name"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        EMail = reader["EMail"].ToString(),
                        Adress = reader["Adress"].ToString()
                    };
                }
                return new JsonResult(user);
            }
        }
        [HttpGet("top/{pageNumber}/{pageSize}")]
        public ActionResult GetTop10([FromRoute] int id, int pageNumber, int pageSize)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                var a = DateTime.Now;
                var b = a.Subtract(new TimeSpan(30, 0, 0, 0));
                SqlCommand command = new SqlCommand(@"select
                                                        u.Id,
                                                        u.Name,
                                                        COUNT(h.Id) hireCount
                                                        from Users  u
                                                        left join Hire h on h.UserId = u .Id
                                                        where h.StartDate between @b and @a
                                                        group by u.Id,u.Name
                                                        order by hireCount desc
                                                        OFFSET @pagenumber ROWS 
                                                        FETCH NEXT @pagesize ROWS ONLY;", con);
                command.Parameters.Add("@pagenumber", SqlDbType.Int).Value = pageNumber;
                command.Parameters.Add("@pagesize", SqlDbType.Int).Value = pageSize;
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@a", SqlDbType.DateTime).Value = a;
                command.Parameters.Add("@b", SqlDbType.DateTime).Value = b;
                SqlDataReader reader = command.ExecuteReader();
                List<object> user = new List<object>();
                while (reader.Read())
                {
                    user.Add(
                        new
                        {
                            Id = Int32.Parse(reader["Id"].ToString()),
                            Name = reader["Name"].ToString(),
                            HireCount = reader["hireCount"].ToString()
                        });
                }
                return new JsonResult(user);
            }
        }
        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody] userRequest req)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                List<string> errors = new List<string>();
                var userValidator = new UserValidation();
                var result = userValidator.Validate(req);

                if (result.IsValid)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("insert into Users values(@Name,@LastName,@EMail,@Adress)", con);
                    command.Parameters.Add("@Name", SqlDbType.VarChar).Value = req.name;
                    command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = req.lastname;
                    command.Parameters.Add("@EMail", SqlDbType.VarChar).Value = req.email;
                    command.Parameters.Add("@Adress", SqlDbType.VarChar).Value = req.adress;
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                    }
                    catch (System.Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                }

                foreach (var i in result.Errors)
                {
                    errors.Add($"ERROR!! {i.PropertyName} : {i.ErrorMessage}");
                }

                return new JsonResult(errors);
            }
        }

        // PUT api/values/5
        [HttpPut("{userid}")]
        public ActionResult Put([FromRoute]int userid, [FromBody] userRequest req)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                List<string> errors = new List<string>();
                var userValidator = new UserValidation();
                var result = userValidator.Validate(req);

                if (result.IsValid)
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("update Users set Name=@Name,LastName=@LastName,EMail=@EMail,Adress=@Adress where Users.Id=" + userid, con);
                    command.Parameters.Add("@Name", SqlDbType.VarChar).Value = req.name;
                    command.Parameters.Add("@LastName", SqlDbType.VarChar).Value = req.lastname;
                    command.Parameters.Add("@EMail", SqlDbType.VarChar).Value = req.email;
                    command.Parameters.Add("@Adress", SqlDbType.VarChar).Value = req.adress;
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                    }
                    catch (System.Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                }
                foreach (var i in result.Errors)
                {
                    errors.Add($"ERROR!! {i.PropertyName} : {i.ErrorMessage}");
                }

                return new JsonResult(errors);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete([FromRoute]int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("delete from Users where Users.Id=" + id, con);
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
