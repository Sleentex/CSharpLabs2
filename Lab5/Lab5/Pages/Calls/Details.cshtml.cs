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
    public class DetailsModel : PageModel
    {
        private readonly Lab5.Models.ApplicationContext _context;

        public DetailsModel(Lab5.Models.ApplicationContext context)
        {
            _context = context;
        }

        public Call Call { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Call = await _context.Calls
                .Include(c => c.City)
                .Include(c => c.Client).FirstOrDefaultAsync(m => m.Id == id);

            if (Call == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
