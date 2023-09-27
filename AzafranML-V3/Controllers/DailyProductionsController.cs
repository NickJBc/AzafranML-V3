using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzafranML_V3.Data;
using AzafranML_V3.Models;

namespace AzafranML_V3.Controllers
{
    public class DailyProductionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyProductionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailyProductions
        public async Task<IActionResult> Index()
        {
              return _context.DailyProduction != null ? 
                          View(await _context.DailyProduction.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DailyProduction'  is null.");
        }

        // GET: DailyProductions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DailyProduction == null)
            {
                return NotFound();
            }

            var dailyProduction = await _context.DailyProduction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyProduction == null)
            {
                return NotFound();
            }

            return View(dailyProduction);
        }

        // GET: DailyProductions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DailyProductions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date")] DailyProduction dailyProduction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyProduction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dailyProduction);
        }

        // GET: DailyProductions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DailyProduction == null)
            {
                return NotFound();
            }

            var dailyProduction = await _context.DailyProduction.FindAsync(id);
            if (dailyProduction == null)
            {
                return NotFound();
            }
            return View(dailyProduction);
        }

        // POST: DailyProductions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date")] DailyProduction dailyProduction)
        {
            if (id != dailyProduction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyProduction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyProductionExists(dailyProduction.Id))
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
            return View(dailyProduction);
        }

        // GET: DailyProductions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DailyProduction == null)
            {
                return NotFound();
            }

            var dailyProduction = await _context.DailyProduction
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyProduction == null)
            {
                return NotFound();
            }

            return View(dailyProduction);
        }

        // POST: DailyProductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DailyProduction == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DailyProduction'  is null.");
            }
            var dailyProduction = await _context.DailyProduction.FindAsync(id);
            if (dailyProduction != null)
            {
                _context.DailyProduction.Remove(dailyProduction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyProductionExists(int id)
        {
          return (_context.DailyProduction?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
