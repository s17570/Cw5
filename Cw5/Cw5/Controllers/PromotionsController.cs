using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Controllers
{
    [Route("api/enrollments/promotions")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        [HttpPost]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            var psr = new PromoteStudentsResponse();
            using (var con = new SqlConnection(Program.GetConnectionString()))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                var tran = con.BeginTransaction();

                try
                {
                    com.CommandText = "EXEC PROMOTESTUDENTS @STUDIES = @studies, @SEMESTER = @semester";
                    com.Parameters.AddWithValue("studies", request.Studies);
                    com.Parameters.AddWithValue("semester", request.Semester);
                    com.Transaction = tran;

                    var dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return BadRequest("Procedura nie powiodła się");
                    }
                    else
                    {
                        psr.IdEnrollment = (int)dr["IdEnrollment"];
                        psr.IdStudy = (int)dr["IdStudy"];
                        psr.Semester = (int)dr["Semester"];
                        psr.StartDate = (DateTime)dr["StartDate"];
                    }

                    dr.Close();

                    tran.Commit();
                } catch (SqlException exc)
                {
                    tran.Rollback();
                    return BadRequest("Operacja nie przebiegła pomyślnie");
                }
            }

            return Ok(psr);
        }
    }
}
