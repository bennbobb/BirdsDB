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
    public class PhylumController : Controller
    {
        private readonly MvcBirdsContext _context;

        public PhylumController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Phylum
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? parentID,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            // Sort is asc by defualt
            // Set sort pram used by <a> on page load to desc by default
            ViewData["PhylumSortParm"] = sortOrder == null || sortOrder == "phyllum_asc" ? "phyllum_desc" : "phyllum_asc";
            ViewData["KingdomSortParm"] = sortOrder == "kingdom_asc" ? "kingdom_desc" : "kingdom_asc";
            ViewData["DomainSortParm"] = sortOrder == "domain_asc" ? "domain_desc" : "domain_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var mvcBirdsContext = from x in _context.TaxPhylums
                                  .Include(x => x.TaxKingdom)
                                  .ThenInclude(x => x.TaxDomain)
                                  select x;

            if (parentID.HasValue)
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxKingdomID == parentID);

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxPhylumName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "domain_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxKingdom.TaxDomain.TaxDomainName),
                "domain_asc" => mvcBirdsContext.OrderBy(x => x.TaxKingdom.TaxDomain.TaxDomainName),
                "kingdom_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxKingdom.TaxKingdomName),
                "kingdom_asc" => mvcBirdsContext.OrderBy(x => x.TaxKingdom.TaxKingdomName),
                "phyllum_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxPhylumName),
                _ => mvcBirdsContext.OrderBy(x => x.TaxPhylumName),
            };

            return View(await PaginatedList<TaxPhylum>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: Phylum/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxPhylum = await _context.TaxPhylums
                .Include(t => t.TaxKingdom)
                .ThenInclude(t => t.TaxDomain)
                .FirstOrDefaultAsync(m => m.TaxPhylumID == id);
            if (taxPhylum == null)
            {
                return NotFound();
            }

            return View(taxPhylum);
        }

        // GET: Phylum/Create
        public IActionResult Create()
        {
            PopulateKingdomDropDownList();
            return View();
        }

        // POST: Phylum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxPhylumID,TaxKingdomID,TaxPhylumName")] TaxPhylum taxPhylum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxPhylum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateKingdomDropDownList(taxPhylum);
            return View(taxPhylum);
        }

        // GET: Phylum/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxPhylum = await _context.TaxPhylums.FindAsync(id);
            if (taxPhylum == null)
            {
                return NotFound();
            }

            PopulateKingdomDropDownList(taxPhylum.TaxKingdomID);
            return View(taxPhylum);
        }

        // POST: Phylum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxPhylumID,TaxKingdomID,TaxPhylumName")] TaxPhylum taxPhylum)
        {
            if (id != taxPhylum.TaxPhylumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxPhylum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxPhylumExists(taxPhylum.TaxPhylumID))
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
            PopulateKingdomDropDownList(taxPhylum.TaxKingdomID);
            return View(taxPhylum);
        }

        // GET: Phylum/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxPhylum = await _context.TaxPhylums
                .Include(t => t.TaxKingdom)
                .FirstOrDefaultAsync(m => m.TaxPhylumID == id);
            if (taxPhylum == null)
            {
                return NotFound();
            }

            return View(taxPhylum);
        }

        // POST: Phylum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxPhylum = await _context.TaxPhylums.FindAsync(id);
            _context.TaxPhylums.Remove(taxPhylum);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxPhylumExists(int id)
        {
            return _context.TaxPhylums.Any(e => e.TaxPhylumID == id);
        }

        private void PopulateKingdomDropDownList(object taxPhylum = null)
        {
            ViewData["TaxKingdomID"] = new SelectList(_context.TaxKingdoms, "TaxKingdomID", "TaxKingdomName", taxPhylum);
        }
    }
}
