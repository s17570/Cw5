using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DTOs.Requests
{
    public class PromoteStudentsRequest
    {
        [Required(ErrorMessage = "Nie została podana wymagana nazwa studiów")]
        public string Studies { get; set; }
        [Required(ErrorMessage = "Nie został podany wymagany semestr")]
        public int Semester { get; set; }
    }
}
