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
    public class GenusController : Controller
    {
        private readonly MvcBirdsContext _context;

        public GenusController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Genus
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["GenusSortParm"] = sortOrder == null || sortOrder == "genus_asc" ? "genus_desc" : "genus_asc";
            ViewData["FamilySortParm"] = sortOrder == "family_asc" ? "family_desc" : "family_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;


            var mvcBirdsContext = from x in _context.TaxGenuses
                                  .Include(t => t.TaxFamily)
                                  select x;

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxGenusName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "family_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxFamily.TaxFamilyName),
                "family_asc" => mvcBirdsContext.OrderBy(x => x.TaxFamily.TaxFamilyName),
                "genus_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxGenusName),
                _ => mvcBirdsContext.OrderBy(x => x.TaxGenusName),
            };

            return View(await PaginatedList<TaxGenus>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: Genus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxGenus = await _context.TaxGenuses
                .Include(t => t.TaxFamily)
                .FirstOrDefaultAsync(m => m.TaxGenusID == id);
            if (taxGenus == null)
            {
                return NotFound();
            }

            return View(taxGenus);
        }

        // GET: Genus/Create
        public IActionResult Create()
        {
            PopulateFamilyDropDownList();
            return View();
        }

        // POST: Genus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxGenusID,TaxFamilyID,TaxGenusName")] TaxGenus taxGenus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxGenus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateFamilyDropDownList(taxGenus);
            return View(taxGenus);
        }

        // GET: Genus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxGenus = await _context.TaxGenuses.FindAsync(id);
            if (taxGenus == null)
            {
                return NotFound();
            }

            PopulateFamilyDropDownList(taxGenus);
            return View(taxGenus);
        }

        // POST: Genus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxGenusID,TaxFamilyID,TaxGenusName")] TaxGenus taxGenus)
        {
            if (id != taxGenus.TaxGenusID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxGenus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxGenusExists(taxGenus.TaxGenusID))
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

            PopulateFamilyDropDownList(taxGenus);
            return View(taxGenus);
        }

        // GET: Genus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxGenus = await _context.TaxGenuses
                .Include(t => t.TaxFamily)
                .FirstOrDefaultAsync(m => m.TaxGenusID == id);
            if (taxGenus == null)
            {
                return NotFound();
            }

            return View(taxGenus);
        }

        // POST: Genus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxGenus = await _context.TaxGenuses.FindAsync(id);
            _context.TaxGenuses.Remove(taxGenus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxGenusExists(int id)
        {
            return _context.TaxGenuses.Any(e => e.TaxGenusID == id);
        }

        private void PopulateFamilyDropDownList(object taxFamily = null)
        {
            ViewData["TaxFamilyID"] = new SelectList(_context.TaxFamilies, "TaxFamilyID", "TaxFamilyName", taxFamily);
        }
    }
}
