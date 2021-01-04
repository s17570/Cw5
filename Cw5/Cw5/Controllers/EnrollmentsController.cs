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
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private Dictionary<string, Object> paramsDictionary = new Dictionary<string, object>();

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student()
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                Studies = request.Studies
            };
            var enr = new EnrollStudentResponse();

            using (var con = new SqlConnection(Program.GetConnectionString()))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();

                try
                {
                    com.CommandText = "SELECT idstudy FROM studies WHERE name=@name";
                    com.Parameters.AddWithValue("name", st.Studies);
                    com.Transaction = tran;

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return BadRequest("Nie udało się odnaleźć podanego kierunku studiów");
                    }

                    // znalezione studia
                    int idstudy = (int)dr["idstudy"];
                    // semestr, który nas interesuje
                    int semester = 1;

                    dr.Close();
                    com.CommandText = "SELECT * FROM enrollment WHERE idstudy=@idstudy AND semester=@semester";
                    com.Parameters.AddWithValue("idstudy", idstudy);
                    com.Parameters.AddWithValue("semester", semester);
                    com.Transaction = tran;

                    int idenrollment = 0;
                    DateTime startdate = DateTime.Now;

                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "SELECT COUNT(*) AS counter FROM enrollment";
                        com.Transaction = tran;
                        dr = com.ExecuteReader();
                        if (dr.Read())
                        {
                            idenrollment = ((int)dr["counter"]) + 1;
                        }
                        dr.Close();

                        com.CommandText = "INSERT INTO enrollment(idenrollment, semester, idstudy, startdate) VALUES(@idenrollment, @semester, @idstudy, @startdate)";
                        com.Parameters.AddWithValue("idenrollment", idenrollment);
                        com.Parameters.AddWithValue("semester", semester);
                        com.Parameters.AddWithValue("idstudy", idstudy);
                        com.Parameters.AddWithValue("startdate", startdate);
                        com.Transaction = tran;

                        var nonquery = com.ExecuteNonQuery();

                        if (nonquery <= 0)
                        {
                            tran.Rollback();
                            return BadRequest("Nie udało się dodać wpisu na semestr");
                        }
                    }
                    else
                    {
                        idenrollment = (int)dr["IdEnrollment"];
                        semester = (int)dr["Semester"];
                        idstudy = (int)dr["IdStudy"];
                        startdate = (DateTime)dr["StartDate"];
                    }

                    enr.IdEnrollment = idenrollment;
                    enr.Semester = semester;
                    enr.IdStudy = idstudy;
                    enr.StartDate = startdate;

                    if (!dr.IsClosed) 
                        dr.Close();
                    com.CommandText = "SELECT COUNT(*) AS counter FROM student WHERE indexnumber=@indexnumber";
                    com.Parameters.AddWithValue("indexnumber", st.IndexNumber);
                    com.Transaction = tran;
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        if ((int)dr["counter"] > 0)
                        {
                            dr.Close();
                            tran.Rollback();
                            return BadRequest("Student z podanym indeksem już istnieje w bazie danych");
                        }
                    }
                    dr.Close();

                    com.CommandText = "INSERT INTO student(indexnumber, firstname, lastname, birthdate, idenrollment) VALUES(@indexnumber, @firstname, @lastname, @birthdate, @idenrollment)";
                    com.Parameters.AddWithValue("indexnumber", st.IndexNumber);
                    com.Parameters.AddWithValue("firstname", st.FirstName);
                    com.Parameters.AddWithValue("lastname", st.LastName);
                    com.Parameters.AddWithValue("birthdate", st.BirthDate);
                    com.Parameters.AddWithValue("idenrollment", st.Studies);    
                    com.Transaction = tran;

                    tran.Commit();

                } catch (SqlException exc)
                {
                    return BadRequest(exc.ToString());
                }
            }

            var response = new EnrollStudentResponse();

            return Ok(enr);
        }
    }
}  
