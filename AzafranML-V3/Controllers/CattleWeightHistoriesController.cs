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
    public class CattleWeightHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CattleWeightHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CattleWeightHistories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CattleWeightHistories.Include(c => c.Cattle);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CattleWeightHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CattleWeightHistories == null)
            {
                return NotFound();
            }

            var cattleWeightHistory = await _context.CattleWeightHistories
                .Include(c => c.Cattle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cattleWeightHistory == null)
            {
                return NotFound();
            }

            return View(cattleWeightHistory);
        }

        // GET: CattleWeightHistories/Create
        public IActionResult Create()
        {
            ViewData["CattleId"] = new SelectList(_context.Cattle, "Id", "Id");
            return View();
        }

        // POST: CattleWeightHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CattleId,RecordedDate,WeightInKg")] CattleWeightHistory cattleWeightHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cattleWeightHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CattleId"] = new SelectList(_context.Cattle, "Id", "Id", cattleWeightHistory.CattleId);
            return View(cattleWeightHistory);
        }

        // GET: CattleWeightHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CattleWeightHistories == null)
            {
                return NotFound();
            }

            var cattleWeightHistory = await _context.CattleWeightHistories.FindAsync(id);
            if (cattleWeightHistory == null)
            {
                return NotFound();
            }
            ViewData["CattleId"] = new SelectList(_context.Cattle, "Id", "Id", cattleWeightHistory.CattleId);
            return View(cattleWeightHistory);
        }

        // POST: CattleWeightHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CattleId,RecordedDate,WeightInKg")] CattleWeightHistory cattleWeightHistory)
        {
            if (id != cattleWeightHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cattleWeightHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CattleWeightHistoryExists(cattleWeightHistory.Id))
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
            ViewData["CattleId"] = new SelectList(_context.Cattle, "Id", "Id", cattleWeightHistory.CattleId);
            return View(cattleWeightHistory);
        }

        // GET: CattleWeightHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CattleWeightHistories == null)
            {
                return NotFound();
            }

            var cattleWeightHistory = await _context.CattleWeightHistories
                .Include(c => c.Cattle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cattleWeightHistory == null)
            {
                return NotFound();
            }

            return View(cattleWeightHistory);
        }

        // POST: CattleWeightHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CattleWeightHistories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CattleWeightHistories'  is null.");
            }
            var cattleWeightHistory = await _context.CattleWeightHistories.FindAsync(id);
            if (cattleWeightHistory != null)
            {
                _context.CattleWeightHistories.Remove(cattleWeightHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CattleWeightHistoryExists(int id)
        {
          return (_context.CattleWeightHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
