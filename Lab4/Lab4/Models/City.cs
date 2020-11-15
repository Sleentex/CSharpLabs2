using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class City
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Назва")]
        public string CityName { get; set; }
        [Display(Name = "Телефонний код")]
        public string PhoneCode { get; set; }
        [Display(Name = "Тариф")]
        public int Tariff { get; set; }
        public virtual ICollection<Call> Calls { get; set; }

        public void ReadFromStringArray(string[] values)
        {
            CityName = values[1];
            PhoneCode = values[2];
            Tariff = int.Parse(values[3]);
        }

        public override string ToString()
        {
            return string.Join(" \\ ", new object[] { Id, CityName, PhoneCode, Tariff });
        }
    }
}
