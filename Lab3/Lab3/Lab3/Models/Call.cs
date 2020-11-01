using Lab2.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lab2.Models
{
    class Call : IModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public Guid CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }

        public int ConversationDuration { get; set; }
        public DateTime DateStart { get; set; }

        public override string ToString()
        {
            return string.Join(" \\ ", new object[] { Id, ClientId, CityId, ConversationDuration, DateStart });
        }

    }
}
