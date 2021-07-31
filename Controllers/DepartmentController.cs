using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT DepartmentId, DepartmentName FROM dbo.Department
            ";

            DataTable myTable = new DataTable();
            string mySqlDataSource = _configuration.GetConnectionString("WebApiCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(mySqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    myTable.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(myTable);
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            string query = @"
                INSERT INTO dbo.Department VALUES (@DepartmentName)
            ";

            DataTable myTable = new DataTable();
            string mySqlDataSource = _configuration.GetConnectionString("WebApiCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(mySqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = myCommand.ExecuteReader();
                    myTable.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully!");
        }

        [HttpPut]
        public JsonResult Put(Department deb)
        {
            string query = @"
                UPDATE dbo.Department SET DepartmentName = @DepartmentName
                WHERE DepartmentId = @DepartmentId
            ";
            DataTable myTable = new DataTable();
            string mySqlDataSource = _configuration.GetConnectionString("WebApiCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(mySqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentID", deb.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", deb.DepartmentName);
                    myReader = myCommand.ExecuteReader();
                    myTable.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                DELETE FROM dbo.Department
                WHERE DepartmentId = @DepartmentId
            ";
            DataTable myTable = new DataTable();
            string mySqlDataSource = _configuration.GetConnectionString("WebApiCon");
            using (SqlConnection myCon = new SqlConnection(mySqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);
                    SqlDataReader myReader;
                    myReader = myCommand.ExecuteReader();
                    myTable.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully!");
        }
    }
}
