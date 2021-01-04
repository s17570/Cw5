using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required(ErrorMessage ="Nie został wprowadzony wymagany numer indeksu")]
        [RegularExpression("^s[0-9]+$")]
        [MaxLength(100)]
        public string IndexNumber { get; set; }
        [Required(ErrorMessage = "Nie zostało podane wymagane imię")]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Nie zostało podane wymagane nazwisko")]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Nie została podana wymagana data urodzenia")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage ="Nie została podana wymagana nazwa studiów")]
        public string Studies { get; set; }
    }
}
