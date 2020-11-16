using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab4.Models;

namespace Lab4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Calls
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

            return View(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
