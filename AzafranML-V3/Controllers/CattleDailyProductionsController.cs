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
    public class CattleDailyProductionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CattleDailyProductionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CattleDailyProductions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CattleDailyProduction.Include(c => c.Cattle).Include(c => c.DailyProduction);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CattleDailyProductions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CattleDailyProduction == null)
            {
                return NotFound();
            }

            var cattleDailyProduction = await _context.CattleDailyProduction
                .Include(c => c.Cattle)
                .Include(c => c.DailyProduction)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cattleDailyProduction == null)
            {
                return NotFound();
            }

            return View(cattleDailyProduction);
        }

        // GET: CattleDailyProductions/Create
        public IActionResult Create()
        {
            ViewData["CattleId"] = new SelectList(_context.Cattle, "Id", "Id");

            // Adjust this line to populate the SelectList with dates.
            ViewData["DailyProductionId"] = _context.DailyProduction
                .Select(dp => new SelectListItem
                {
                    Value = dp.Id.ToString(),
                    Text = dp.Date.ToShortDateString()
                }).ToList();

            return View();
        }

        // POST: CattleDailyProductions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CattleId,DailyProductionId,AmountProduced")] CattleDailyProduction cattleDailyProduction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cattleDailyProduction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CattleId"] = new SelectList(_context.Cattle, "Id", "Id", cattleDailyProduction.CattleId);
            ViewData["DailyProductionId"] = new SelectList(_context.Set<DailyProduction>(), "Id", "Id", cattleDailyProduction.DailyProductionId);
            return View(cattleDailyProduction);
        }

        // GET: CattleDailyProductions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CattleDailyProduction == null)
            {
                return NotFound();
            }

            var cattleDailyProduction = await _context.CattleDailyProduction.FindAsync(id);
            if (cattleDailyProduction == null)
            {
                return NotFound();
            }
            ViewData["CattleId"] = new SelectList(_context.Cattle, "Id", "Id", cattleDailyProduction.CattleId);
            ViewData["DailyProductionId"] = new SelectList(_context.Set<DailyProduction>(), "Id", "Id", cattleDailyProduction.DailyProductionId);
            return View(cattleDailyProduction);
        }

        // POST: CattleDailyProductions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CattleId,DailyProductionId,AmountProduced")] CattleDailyProduction cattleDailyProduction)
        {
            if (id != cattleDailyProduction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cattleDailyProduction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CattleDailyProductionExists(cattleDailyProduction.Id))
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
            ViewData["CattleId"] = new SelectList(_context.Cattle, "Id", "Id", cattleDailyProduction.CattleId);
            ViewData["DailyProductionId"] = new SelectList(_context.Set<DailyProduction>(), "Id", "Id", cattleDailyProduction.DailyProductionId);
            return View(cattleDailyProduction);
        }

        // GET: CattleDailyProductions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CattleDailyProduction == null)
            {
                return NotFound();
            }

            var cattleDailyProduction = await _context.CattleDailyProduction
                .Include(c => c.Cattle)
                .Include(c => c.DailyProduction)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cattleDailyProduction == null)
            {
                return NotFound();
            }

            return View(cattleDailyProduction);
        }

        // POST: CattleDailyProductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CattleDailyProduction == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CattleDailyProduction'  is null.");
            }
            var cattleDailyProduction = await _context.CattleDailyProduction.FindAsync(id);
            if (cattleDailyProduction != null)
            {
                _context.CattleDailyProduction.Remove(cattleDailyProduction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> TotalMilkProduction(DateTime? startDate, DateTime? endDate, int? cattleId)  // Added cattleId parameter
        {
            var viewModel = new TotalMilkProductionViewModel
            {
                StartDate = startDate ?? DateTime.Now.AddDays(-1),
                EndDate = endDate ?? DateTime.Now.AddDays(1),
                CattleId = cattleId
            };

            if (startDate.HasValue && endDate.HasValue)
            {
                var query = _context.CattleDailyProduction
                .Where(dp => dp.DailyProduction.Date >= startDate.Value && dp.DailyProduction.Date <= endDate.Value);

                if (viewModel.CattleId.HasValue)
                {
                    query = query.Where(dp => dp.CattleId == viewModel.CattleId.Value);
                }

                query = query.Include(dp => dp.DailyProduction).Include(dp => dp.Cattle);

                var dailyProductions = await query.ToListAsync();


                // Group by Date and Sum AmountProduced for Total Production
                viewModel.DailyProductions = dailyProductions
                    .GroupBy(dp => dp.DailyProduction.Date)
                    .ToDictionary(g => g.Key, g => g.Sum(dp => dp.AmountProduced));

                // Populate DetailedProductions
                foreach (var dailyProduction in dailyProductions)
                {
                    if (!viewModel.DetailedProductions.ContainsKey(dailyProduction.DailyProduction.Date))
                    {
                        viewModel.DetailedProductions[dailyProduction.DailyProduction.Date] = new Dictionary<int, double>();
                    }
                    viewModel.DetailedProductions[dailyProduction.DailyProduction.Date][dailyProduction.CattleId] = dailyProduction.AmountProduced;
                }
            }

            return View(viewModel);
        }


        private bool CattleDailyProductionExists(int id)
        {
          return (_context.CattleDailyProduction?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
