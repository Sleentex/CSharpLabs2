using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Lab5.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, ApplicationContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<ClientAndCities> ClientAndCities { get; set; }

        public void OnGet()
        {
            ClientAndCities = _context.Calls
               .Join(_context.Clients, calls => calls.ClientId, clients => clients.Id, (o, c) => new { CityId = o.CityId, Name = c.Name, Surname = c.Surname })
               .Join(_context.Cities, c => c.CityId, o => o.Id, (c, o) => new { City = o.CityName, Name = c.Name, Surname = c.Surname }).ToList()
               .GroupBy(table => new { table.Surname, table.Name })
               .Where(g => g.Count() >= 2)
               .Select(r => new ClientAndCities
               {
                   FirstName = r.Key.Name,
                   LastName = r.Key.Surname,
                   Cities = string.Join(", ", r.Select(q => q.City))
               });
        }
    }
}
