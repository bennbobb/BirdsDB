using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using birds.Models;

namespace birds.Controllers
{
    public class DomainController : Controller
    {
        private readonly MvcBirdsContext _context;

        public DomainController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Domain
        // Set Sort to asc by defualt
        // Set sort pram used by <a> on page load to desc by default
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DomainSortParm"] = sortOrder == null || sortOrder == "domain_asc" ? "domain_desc" : "domain_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var mvcBirdsContext = from x in _context.TaxDomains
                                  select x;

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxDomainName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "domain_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxDomainName),
                _ => mvcBirdsContext.OrderBy(x => x.TaxDomainName),
            };

            return View(await PaginatedList<TaxDomain>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: Domain/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxDomain = await _context.TaxDomains
                .FirstOrDefaultAsync(m => m.TaxDomainID == id);
            if (taxDomain == null)
            {
                return NotFound();
            }

            return View(taxDomain);
        }

        // GET: Domain/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Domain/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxDomainID,TaxDomainName")] TaxDomain taxDomain)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxDomain);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taxDomain);
        }

        // GET: Domain/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxDomain = await _context.TaxDomains.FindAsync(id);
            if (taxDomain == null)
            {
                return NotFound();
            }
            return View(taxDomain);
        }

        // POST: Domain/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxDomainID,TaxDomainName")] TaxDomain taxDomain)
        {
            if (id != taxDomain.TaxDomainID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxDomain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxDomainExists(taxDomain.TaxDomainID))
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
            return View(taxDomain);
        }

        // GET: Domain/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxDomain = await _context.TaxDomains
                .FirstOrDefaultAsync(m => m.TaxDomainID == id);
            if (taxDomain == null)
            {
                return NotFound();
            }

            return View(taxDomain);
        }

        // POST: Domain/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxDomain = await _context.TaxDomains.FindAsync(id);
            _context.TaxDomains.Remove(taxDomain);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxDomainExists(int id)
        {
            return _context.TaxDomains.Any(e => e.TaxDomainID == id);
        }
    }
}
