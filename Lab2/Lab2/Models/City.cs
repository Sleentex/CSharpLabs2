using Lab2.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab2.Models
{
    class City : IReadableFromString, IModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CityName { get; set; }
        public string PhoneCode { get; set; }
        public int Tariff { get; set; }

        [InverseProperty("City")]
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
