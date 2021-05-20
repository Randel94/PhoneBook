using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    public class PhoneNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер")]
        [Required (ErrorMessage = "Введите номер.")]
        [RegularExpression(@"^\+?\d{5,11}$", ErrorMessage = "Неправильный формат номера.")]
        public string Number { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
