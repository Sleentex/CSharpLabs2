using Lab2.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab2.Models
{
    class Client : IReadableFromString, IModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        [InverseProperty("Client")]
        public virtual ICollection<Call> Calls { get; set; }

        public void ReadFromStringArray(string[] values)
        {
            Name = values[2];
            Surname = values[1];
            MiddleName = values[3];
            Address = values[4];
            PhoneNumber = values[5];
        }

        public override string ToString()
        {
            return string.Join(" \\ ", new object[] { Id, Name, Surname, MiddleName, Address, PhoneNumber });
        }
    }
}
