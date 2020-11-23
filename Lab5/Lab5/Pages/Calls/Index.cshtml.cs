using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab5.Models;

namespace Lab5.Pages.Calls
{
    public class IndexModel : PageModel
    {
        private readonly Lab5.Models.ApplicationContext _context;

        public IndexModel(Lab5.Models.ApplicationContext context)
        {
            _context = context;
        }

        public IList<Call> Call { get;set; }

        public async Task OnGetAsync()
        {
            Call = await _context.Calls
                .Include(c => c.City)
                .Include(c => c.Client).ToListAsync();
        }
    }
}
