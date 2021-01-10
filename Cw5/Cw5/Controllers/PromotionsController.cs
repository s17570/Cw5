using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using Cw5.Services;
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
        private IStudentsDbService _service;

        public PromotionsController(IStudentsDbService service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            try
            {
                var psr = _service.PromoteStudents(request);

                return Ok(psr);

            } catch (Exception exc)
            {
                return BadRequest(exc.ToString());
            }
        }
    }
}
