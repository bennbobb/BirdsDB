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
    public class KingdomController : Controller
    {
        private readonly MvcBirdsContext _context;

        public KingdomController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Kingdom
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["KingdomSortParm"] = sortOrder == null || sortOrder == "kingdom_asc" ? "kingdom_desc" : "kingdom_asc";
            ViewData["DomainSortParm"] = sortOrder == "domain_asc" ? "domain_desc" : "domain_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var mvcBirdsContext = from x in _context.TaxKingdoms
                                  .Include(t => t.TaxDomain)
                                  select x;

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxKingdomName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "domain_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxDomain.TaxDomainName),
                "domain_asc" => mvcBirdsContext.OrderBy(x => x.TaxDomain.TaxDomainName),
                "kingdom_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxKingdomName),
                _ => mvcBirdsContext.OrderBy(x => x.TaxKingdomName),
            };

            return View(await PaginatedList<TaxKingdom>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: Kingdom/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxKingdom = await _context.TaxKingdoms
                .Include(t => t.TaxDomain)
                .FirstOrDefaultAsync(m => m.TaxKingdomID == id);
            if (taxKingdom == null)
            {
                return NotFound();
            }

            return View(taxKingdom);
        }

        // GET: Kingdom/Create
        public IActionResult Create()
        {
            PopulateDomainDropDownList();
            return View();
        }

        // POST: Kingdom/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxKingdomID,TaxDomainID,TaxKingdomName")] TaxKingdom taxKingdom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxKingdom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDomainDropDownList(taxKingdom);
            return View(taxKingdom);
        }

        // GET: Kingdom/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxKingdom = await _context.TaxKingdoms.FindAsync(id);
            if (taxKingdom == null)
            {
                return NotFound();
            }

            PopulateDomainDropDownList(taxKingdom);
            return View(taxKingdom);
        }

        // POST: Kingdom/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxKingdomID,TaxDomainID,TaxKingdomName")] TaxKingdom taxKingdom)
        {
            if (id != taxKingdom.TaxKingdomID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxKingdom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxKingdomExists(taxKingdom.TaxKingdomID))
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

            PopulateDomainDropDownList(taxKingdom);
            return View(taxKingdom);
        }

        // GET: Kingdom/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxKingdom = await _context.TaxKingdoms
                .Include(t => t.TaxDomain)
                .FirstOrDefaultAsync(m => m.TaxKingdomID == id);
            if (taxKingdom == null)
            {
                return NotFound();
            }

            return View(taxKingdom);
        }

        // POST: Kingdom/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxKingdom = await _context.TaxKingdoms.FindAsync(id);
            _context.TaxKingdoms.Remove(taxKingdom);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxKingdomExists(int id)
        {
            return _context.TaxKingdoms.Any(e => e.TaxKingdomID == id);
        }

        private void PopulateDomainDropDownList(object taxKingdom = null)
        {
            ViewData["TaxDomainID"] = new SelectList(_context.TaxDomains, "TaxDomainID", "TaxDomainName", taxKingdom);
        }
    }
}
