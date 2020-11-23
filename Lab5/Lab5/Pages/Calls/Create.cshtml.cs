using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab5.Models;

namespace Lab5.Pages.Calls
{
    public class CreateModel : PageModel
    {
        private readonly Lab5.Models.ApplicationContext _context;

        public CreateModel(Lab5.Models.ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
        ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public Call Call { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Calls.Add(Call);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
