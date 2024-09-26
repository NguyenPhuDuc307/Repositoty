using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BilliardManagement.Data;
using BilliardManagement.Data.Entities;

namespace BilliardManagement.Controllers
{
    public class PriceListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PriceListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PriceLists
        public async Task<IActionResult> Index()
        {
            var ApplicationDbContext = _context.PriceLists.Include(p => p.Branch).Include(p => p.Table);
            return View(await ApplicationDbContext.ToListAsync());
        }

        // GET: PriceLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceList = await _context.PriceLists
                .Include(p => p.Branch)
                .Include(p => p.Table)
                .FirstOrDefaultAsync(m => m.TableId == id);
            if (priceList == null)
            {
                return NotFound();
            }

            return View(priceList);
        }

        // GET: PriceLists/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id");
            ViewData["TableId"] = new SelectList(_context.Tables, "Id", "Id");
            return View();
        }

        // POST: PriceLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TableId,BranchId,StartTime,EndTime,Price")] PriceList priceList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priceList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", priceList.BranchId);
            ViewData["TableId"] = new SelectList(_context.Tables, "Id", "Id", priceList.TableId);
            return View(priceList);
        }

        // GET: PriceLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceList = await _context.PriceLists.FindAsync(id);
            if (priceList == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", priceList.BranchId);
            ViewData["TableId"] = new SelectList(_context.Tables, "Id", "Id", priceList.TableId);
            return View(priceList);
        }

        // POST: PriceLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TableId,BranchId,StartTime,EndTime,Price")] PriceList priceList)
        {
            if (id != priceList.TableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priceList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceListExists(priceList.TableId))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", priceList.BranchId);
            ViewData["TableId"] = new SelectList(_context.Tables, "Id", "Id", priceList.TableId);
            return View(priceList);
        }

        // GET: PriceLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceList = await _context.PriceLists
                .Include(p => p.Branch)
                .Include(p => p.Table)
                .FirstOrDefaultAsync(m => m.TableId == id);
            if (priceList == null)
            {
                return NotFound();
            }

            return View(priceList);
        }

        // POST: PriceLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priceList = await _context.PriceLists.FindAsync(id);
            if (priceList != null)
            {
                _context.PriceLists.Remove(priceList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceListExists(int id)
        {
            return _context.PriceLists.Any(e => e.TableId == id);
        }
    }
}
