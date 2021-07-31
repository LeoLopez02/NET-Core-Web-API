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
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT EmployeeId, EmployeeName, Department, convert(varchar(10), DateOfJoining, 120) as DateOfJoining, PhotoFileName FROM dbo.Employee
            ";
            DataTable myTable = new DataTable();
            string myDataSource = _configuration.GetConnectionString("WebApiCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(myDataSource))
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
        public JsonResult Post(Employee emp)
        {
            string query = @"
                INSERT INTO dbo.Employee(EmployeeName, Department, DateOfJoining, PhotoFileName) VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)
            ";
            DataTable myTable = new DataTable();
            string myDataSource = _configuration.GetConnectionString("WebApiCon");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(myDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    myTable.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully!");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            string query = @"
                UPDATE dbo.Employee SET EmployeeName = @EmployeeName, Department = @Department, DateOfJoining = @DateOfJoining, PhotoFileName = @PhotoFileName
                WHERE EmployeeId = @EmployeeId
            ";
            DataTable myTable = new DataTable();
            string mySqlDataSource = _configuration.GetConnectionString("WebApiCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(mySqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReader = myCommand.ExecuteReader();
                    myTable.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }

                return new JsonResult("Updated Successfully!");
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                DELETE FROM dbo.Employee WHERE EmployeeId = @EmployeeId
             ";
            DataTable myTable = new DataTable();
            string mySqlDataSource = _configuration.GetConnectionString("WebApiCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(mySqlDataSource)) 
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);
                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    myCon.Close();
                }

                return new JsonResult("Deleted Successfully!");
            }
        }
    }
}
