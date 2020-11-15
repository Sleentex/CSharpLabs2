using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Call
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }
        [Display(Name = "Клієнт")]
        public Client Client { get; set; }

        public Guid CityId { get; set; }
        [Display(Name = "Місто")]
        public City City { get; set; }

        [Display(Name = "Тривалість розмови")]
        public int ConversationDuration { get; set; }

        [Display(Name = "Дата початку розмови")]
        public DateTime DateStart { get; set; }

        public override string ToString()
        {
            return string.Join(" \\ ", new object[] { Id, ClientId, CityId, ConversationDuration, DateStart });
        }
    }
}
