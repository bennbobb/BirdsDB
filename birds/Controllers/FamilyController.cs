using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using birds.Models;

namespace birds.Controllers
{
    public class FamilyController : Controller
    {
        private readonly MvcBirdsContext _context;

        public FamilyController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Family
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FamilySortParm"] = sortOrder == null || sortOrder == "family_asc" ? "family_desc" : "family_asc";
            ViewData["OrderSortParm"] = sortOrder == "order_asc" ? "order_desc" : "order_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var mvcBirdsContext = from x in _context.TaxFamilies
                                  .Include(t => t.TaxOrder)
                                  select x;

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxFamilyName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "order_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxOrder.TaxOrderName),
                "order_asc" => mvcBirdsContext.OrderBy(x => x.TaxOrder.TaxOrderName),
                "family_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxFamilyName),
                _ => mvcBirdsContext.OrderBy(x => x.TaxFamilyName),
            };

            return View(await PaginatedList<TaxFamily>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: Family/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxFamily = await _context.TaxFamilies
                .Include(t => t.TaxOrder)
                .FirstOrDefaultAsync(m => m.TaxFamilyID == id);
            if (taxFamily == null)
            {
                return NotFound();
            }

            return View(taxFamily);
        }

        // GET: Family/Create
        public IActionResult Create()
        {
            PopulateOrderDropDownList();
            return View();
        }

        // POST: Family/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxFamilyID,TaxOrderID,TaxFamilyName")] TaxFamily taxFamily)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxFamily);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateOrderDropDownList(taxFamily);
            return View(taxFamily);
        }

        // GET: Family/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxFamily = await _context.TaxFamilies.FindAsync(id);
            if (taxFamily == null)
            {
                return NotFound();
            }

            PopulateOrderDropDownList(taxFamily);
            return View(taxFamily);
        }

        // POST: Family/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxFamilyID,TaxOrderID,TaxFamilyName")] TaxFamily taxFamily)
        {
            if (id != taxFamily.TaxFamilyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxFamily);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxFamilyExists(taxFamily.TaxFamilyID))
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

            PopulateOrderDropDownList(taxFamily);
            return View(taxFamily);
        }

        // GET: Family/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxFamily = await _context.TaxFamilies
                .Include(t => t.TaxOrder)
                .FirstOrDefaultAsync(m => m.TaxFamilyID == id);
            if (taxFamily == null)
            {
                return NotFound();
            }

            return View(taxFamily);
        }

        // POST: Family/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxFamily = await _context.TaxFamilies.FindAsync(id);
            _context.TaxFamilies.Remove(taxFamily);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxFamilyExists(int id)
        {
            return _context.TaxFamilies.Any(e => e.TaxFamilyID == id);
        }

        private void PopulateOrderDropDownList(object taxOrder = null)
        {
            ViewData["TaxOrderID"] = new SelectList(_context.TaxOrders, "TaxOrderID", "TaxOrderName", taxOrder);
        }
    }
}
