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
    public class ClassController : Controller
    {
        private readonly MvcBirdsContext _context;

        public ClassController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Class
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ClassSortParm"] = sortOrder == null || sortOrder == "class_asc" ? "class_desc" : "class_asc";
            ViewData["PhylumSortParm"] = sortOrder == "phyllum_asc" ? "phyllum_desc" : "phyllum_asc";
            ViewData["KingdomSortParm"] = sortOrder == "kingdom_asc" ? "kingdom_desc" : "kingdom_asc";
            ViewData["DomainSortParm"] = sortOrder == "domain_asc" ? "domain_desc" : "domain_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var mvcBirdsContext = from x in _context.TaxClasses
                                  .Include(t => t.TaxPhylum)
                                  .Include(t => t.TaxPhylum.TaxKingdom)
                                  .Include(t => t.TaxPhylum.TaxKingdom.TaxDomain)
                                  select x;

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxClassName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "domain_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxPhylum.TaxKingdom.TaxDomain.TaxDomainName),
                "domain_asc" => mvcBirdsContext.OrderBy(x => x.TaxPhylum.TaxKingdom.TaxDomain.TaxDomainName),
                "kingdom_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxPhylum.TaxKingdom.TaxKingdomName),
                "kingdom_asc" => mvcBirdsContext.OrderBy(x => x.TaxPhylum.TaxKingdom.TaxKingdomName),
                "phyllum_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxPhylum.TaxPhylumName),
                "phyllum_asc" => mvcBirdsContext.OrderBy(x => x.TaxPhylum.TaxPhylumName),
                "class_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxClassName),
                _ => mvcBirdsContext.OrderBy(x => x.TaxClassName),
            };

            return View(await PaginatedList<TaxClass>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: Class/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxClass = await _context.TaxClasses
                .Include(t => t.TaxPhylum)
                .FirstOrDefaultAsync(m => m.TaxClassID == id);
            if (taxClass == null)
            {
                return NotFound();
            }

            return View(taxClass);
        }

        // GET: Class/Create
        public IActionResult Create()
        {
            PopulatePhylumDropDownList();
            return View();
        }

        // POST: Class/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxClassID,TaxPhylumID,TaxClassName")] TaxClass taxClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulatePhylumDropDownList(taxClass);
            return View(taxClass);
        }

        // GET: Class/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxClass = await _context.TaxClasses.FindAsync(id);
            if (taxClass == null)
            {
                return NotFound();
            }

            PopulatePhylumDropDownList(taxClass);
            return View(taxClass);
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxClassID,TaxPhylumID,TaxClassName")] TaxClass taxClass)
        {
            if (id != taxClass.TaxClassID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxClassExists(taxClass.TaxClassID))
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

            PopulatePhylumDropDownList(taxClass);
            return View(taxClass);
        }

        // GET: Class/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxClass = await _context.TaxClasses
                .Include(t => t.TaxPhylum)
                .FirstOrDefaultAsync(m => m.TaxClassID == id);
            if (taxClass == null)
            {
                return NotFound();
            }

            return View(taxClass);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxClass = await _context.TaxClasses.FindAsync(id);
            _context.TaxClasses.Remove(taxClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxClassExists(int id)
        {
            return _context.TaxClasses.Any(e => e.TaxClassID == id);
        }

        private void PopulatePhylumDropDownList(object taxPhylum = null)
        {
            ViewData["TaxPhylumID"] = new SelectList(_context.TaxPhylums, "TaxPhylumID", "TaxPhylumName", taxPhylum);
        }
    }
}
