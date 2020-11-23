using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab5.Models;

namespace Lab5.Pages.Calls
{
    public class EditModel : PageModel
    {
        private readonly Lab5.Models.ApplicationContext _context;

        public EditModel(Lab5.Models.ApplicationContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
           ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Call).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CallExists(Call.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CallExists(Guid id)
        {
            return _context.Calls.Any(e => e.Id == id);
        }
    }
}
