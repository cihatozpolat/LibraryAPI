using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using KütüphaneAPI.Validations;
using Microsoft.AspNetCore.Mvc;

namespace KütüphaneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HireController : ControllerBase
    {
        string _connectionString = "Server=.; Database=Kütüphane; Trusted_Connection=True;";
        public HireController()
        {
            List<string> errors = new List<string>();
        }

        // GET api/values
        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("select * from Hire where Hire.Id=" + id, con);
                SqlDataReader reader = command.ExecuteReader();
                object hire = null;
                while (reader.Read())
                {
                    hire = new
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        BookId = Int32.Parse(reader["BookId"].ToString()),
                        UserId = Int32.Parse(reader["UserId"].ToString()),
                        StartDate = reader["StartDate"].ToString(),
                        EndDate = reader["EndDate"].ToString(),
                        DeliveryDate = reader["DeliveryDate"].ToString()
                    };
                }
                return new JsonResult(hire);
            }
        }

        // GET api/hire?delivered=true
        [HttpGet()]
        public ActionResult Get(bool? delivered)
        {
            var sb = new StringBuilder("select * from Hire where 1=1");
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                if (delivered.HasValue)
                {
                    if (delivered.Value)
                    {
                        sb.Append("and  Hire.DeliveryDate is not null");
                    }
                    else
                    {
                        sb.Append("and  Hire.DeliveryDate is null");
                    }
                }

                SqlCommand command = new SqlCommand(sb.ToString(), con);
                SqlDataReader reader = command.ExecuteReader();
                List<object> hire = new List<object>();
                while (reader.Read())
                {
                    hire.Add(new
                    {
                        Id = Int32.Parse(reader["Id"].ToString()),
                        BookId = Int32.Parse(reader["BookId"].ToString()),
                        UserId = Int32.Parse(reader["UserId"].ToString()),
                        StartDate = reader["StartDate"].ToString(),
                        EndDate = reader["EndDate"].ToString(),
                        DeliveryDate = reader["DeliveryDate"].ToString()
                    });
                }
                return new JsonResult(hire);
            }
        }

        // POST api
        [HttpPost]
        public ActionResult Post([FromBody] hireRequest req)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                List<string> errors = new List<string>();
                con.Open();
                var hireValidator = new HireValidation();
                var result = hireValidator.Validate(req);

                if (result.IsValid)
                {
                    SqlCommand command = new SqlCommand("insert into hire values(@BookId,@UserId,@StartDate,@EndDate,@DeliveryDate)", con);
                    command.Parameters.Add("@BookId", SqlDbType.Int).Value = req.bookid;
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = req.userid;
                    command.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = req.startdate;
                    command.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = req.enddate;
                    command.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = req.deliverydate;
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        return Ok("Posted !");
                    }
                    catch (System.Exception ex)
                    {
                        System.Console.WriteLine(ex.Message);
                    }
                }
                foreach (var i in result.Errors)
                {
                    errors.Add($"ERROR! {i.PropertyName} : {i.ErrorMessage}");
                }
                return BadRequest(errors);
            }
        }

        // PUT api/values
        [HttpPut("{hireid}")]
        public ActionResult Put([FromBody] hireRequest req, [FromRoute] int hireid)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                List<string> errors = new List<string>();
                var hireValidator = new HireValidation();
                var result = hireValidator.Validate(req);

                if (result.IsValid)
                {
                    SqlCommand command = new SqlCommand("update Hire set BookId=@bookid,UserId=@userid,StartDate=@startdate,Enddate=@enddate,DeliveryDate=@deliverydate where Hire.Id=" + hireid, con);
                    command.Parameters.Add("@bookid", SqlDbType.Int).Value = req.bookid;
                    command.Parameters.Add("@userid", SqlDbType.Int).Value = req.userid;
                    command.Parameters.Add("@startdate", SqlDbType.DateTime).Value = req.startdate;
                    command.Parameters.Add("@enddate", SqlDbType.DateTime).Value = req.enddate;
                    command.Parameters.Add("@deliverydate", SqlDbType.DateTime).Value = req.deliverydate;
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
                    errors.Add($"ERROR!! {i.PropertyName}: {i.ErrorMessage}");
                }
                return new JsonResult(errors);
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


