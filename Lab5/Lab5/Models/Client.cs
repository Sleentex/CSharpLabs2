using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab5.Models
{
    public class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "По батькові")]
        public string MiddleName { get; set; }
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }
        [Display(Name = "Адреса")]
        public string Address { get; set; }
        [Display(Name = "Номер телефону")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string FullName => $"{Name} {MiddleName} {Surname}";

        public virtual ICollection<Call> Calls { get; set; }

        public void ReadFromStringArray(string[] values)
        {
            Name = values[1];
            Surname = values[2];
            MiddleName = values[3];
            Address = values[4];
            PhoneNumber = values[5];
        }

        public override string ToString()
        {
            return string.Join(" \\ ", new object[] { Id, Name, MiddleName, Surname, Address, PhoneNumber });
        }
    }
}
