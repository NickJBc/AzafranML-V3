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
    public class CattleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CattleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cattle
        public async Task<IActionResult> Index()
        {
              return _context.Cattle != null ? 
                          View(await _context.Cattle.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Cattle'  is null.");
        }

        // GET: Cattle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cattle == null)
            {
                return NotFound();
            }

            var cattle = await _context.Cattle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cattle == null)
            {
                return NotFound();
            }

            return View(cattle);
        }

        // GET: Cattle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cattle/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tag,Age,Race,WeightInKg,FeedType")] Cattle cattle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cattle);
                await _context.SaveChangesAsync();

                // Add the initial weight to CattleWeightHistory using WeightInKg property
                var weightHistory = new CattleWeightHistory
                {
                    CattleId = cattle.Id,
                    RecordedDate = DateTime.Now,
                    WeightInKg = cattle.WeightInKg
                };
                _context.CattleWeightHistories.Add(weightHistory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(cattle);
        }

        // GET: Cattle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cattle == null)
            {
                return NotFound();
            }

            var cattle = await _context.Cattle.FindAsync(id);
            if (cattle == null)
            {
                return NotFound();
            }
            return View(cattle);
        }

        // POST: Cattle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tag,Age,Race,WeightInKg,FeedType")] Cattle cattle, double updatedWeight)
        {
            if (id != cattle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the cattle's current weight with the updatedWeight from the form
                    cattle.WeightInKg = updatedWeight;

                    _context.Update(cattle);

                    // Add the updated weight to CattleWeightHistory
                    var weightHistory = new CattleWeightHistory
                    {
                        CattleId = cattle.Id,
                        RecordedDate = DateTime.Now,
                        WeightInKg = updatedWeight
                    };
                    _context.CattleWeightHistories.Add(weightHistory);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CattleExists(cattle.Id))
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
            return View(cattle);
        }


        // GET: Cattle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cattle == null)
            {
                return NotFound();
            }

            var cattle = await _context.Cattle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cattle == null)
            {
                return NotFound();
            }

            return View(cattle);
        }

        // POST: Cattle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cattle == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cattle'  is null.");
            }
            var cattle = await _context.Cattle.FindAsync(id);
            if (cattle != null)
            {
                _context.Cattle.Remove(cattle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CattleExists(int id)
        {
          return (_context.Cattle?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
