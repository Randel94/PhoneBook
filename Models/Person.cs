using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required (ErrorMessage = "Введите имя.")]
        [StringLength(50)]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [StringLength(50)]
        [Display(Name = "Фамилия")]
        public string SName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата рождения")]
        public DateTime BDate { get; set; }
        [Display(Name = "Номера телефонов")]
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
