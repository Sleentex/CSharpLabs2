using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    public class CallsController : Controller
    {
        private readonly ApplicationContext _context;

        public CallsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Calls
        [Route("/calls")]
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Calls.Include(c => c.City).Include(c => c.Client);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Calls/Details/5
        [Route("/calls/details/{id?}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call = await _context.Calls
                .Include(c => c.City)
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // GET: Calls/Create
        [Route("/calls/create")]
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName");
            return View();
        }

        // POST: Calls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/calls/create")]
        public async Task<IActionResult> Create([Bind("Id,ClientId,CityId,ConversationDuration,DateStart")] Call call)
        {
            if (ModelState.IsValid)
            {
                call.Id = Guid.NewGuid();
                _context.Add(call);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", call.CityId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", call.ClientId);
            return View(call);
        }

        // GET: Calls/Edit/5
        [Route("/calls/edit/{id?}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call = await _context.Calls.FindAsync(id);
            if (call == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", call.CityId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", call.ClientId);
            return View(call);
        }

        // POST: Calls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/calls/edit/{id?}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ClientId,CityId,ConversationDuration,DateStart")] Call call)
        {
            if (id != call.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(call);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallExists(call.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", call.CityId);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "FullName", call.ClientId);
            return View(call);
        }

        // GET: Calls/Delete/5
        [Route("/calls/delete/{id?}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var call = await _context.Calls
                .Include(c => c.City)
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (call == null)
            {
                return NotFound();
            }

            return View(call);
        }

        // POST: Calls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("/calls/delete/{id?}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var call = await _context.Calls.FindAsync(id);
            _context.Calls.Remove(call);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CallExists(Guid id)
        {
            return _context.Calls.Any(e => e.Id == id);
        }
    }
}
