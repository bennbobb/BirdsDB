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
    public class SpeciesCommonNameController : Controller
    {
        private readonly MvcBirdsContext _context;

        public SpeciesCommonNameController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: SpeciesCommonName
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CommonNameSortParm"] = sortOrder == null || sortOrder == "commonname_asc" ? "commonname_desc" : "commonname_asc";
            ViewData["IsPrimarySortParm"] = sortOrder == "isprimary_asc" ? "isprimary_desc" : "isprimary_asc";
            ViewData["SpeciesSortParm"] = sortOrder == "species_asc" ? "species_desc" : "species_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var mvcBirdsContext = from x in _context.TaxSpeciesCommonName
                                  .Include(t => t.TaxSpecies)
                                  select x;

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.CommonName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "species_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxSpecies.TaxSpeciesName),
                "species_asc" => mvcBirdsContext.OrderBy(x => x.TaxSpecies.TaxSpeciesName),
                "isprimary_desc" => mvcBirdsContext.OrderByDescending(x => x.IsPrimaryCommonName),
                "isprimary_asc" => mvcBirdsContext.OrderBy(x => x.IsPrimaryCommonName),
                "commonname_desc" => mvcBirdsContext.OrderByDescending(x => x.CommonName),
                _ => mvcBirdsContext.OrderBy(x => x.CommonName),
            };

            return View(await PaginatedList<TaxSpeciesCommonName>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: SpeciesCommonName/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxSpeciesCommonName = await _context.TaxSpeciesCommonName
                .Include(t => t.TaxSpecies)
                .FirstOrDefaultAsync(m => m.TaxSpeciesCommonNameID == id);
            if (taxSpeciesCommonName == null)
            {
                return NotFound();
            }

            return View(taxSpeciesCommonName);
        }

        // GET: SpeciesCommonName/Create
        public IActionResult Create()
        {
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName");
            return View();
        }

        // POST: SpeciesCommonName/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxSpeciesCommonNameID,TaxSpeciesID,CommonName,IsPrimaryCommonName")] TaxSpeciesCommonName taxSpeciesCommonName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxSpeciesCommonName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName", taxSpeciesCommonName.TaxSpeciesID);
            return View(taxSpeciesCommonName);
        }

        // GET: SpeciesCommonName/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxSpeciesCommonName = await _context.TaxSpeciesCommonName.FindAsync(id);
            if (taxSpeciesCommonName == null)
            {
                return NotFound();
            }
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName", taxSpeciesCommonName.TaxSpeciesID);
            return View(taxSpeciesCommonName);
        }

        // POST: SpeciesCommonName/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxSpeciesCommonNameID,TaxSpeciesID,CommonName,IsPrimaryCommonName")] TaxSpeciesCommonName taxSpeciesCommonName)
        {
            if (id != taxSpeciesCommonName.TaxSpeciesCommonNameID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxSpeciesCommonName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxSpeciesCommonNameExists(taxSpeciesCommonName.TaxSpeciesCommonNameID))
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
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName", taxSpeciesCommonName.TaxSpeciesID);
            return View(taxSpeciesCommonName);
        }

        // GET: SpeciesCommonName/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxSpeciesCommonName = await _context.TaxSpeciesCommonName
                .Include(t => t.TaxSpecies)
                .FirstOrDefaultAsync(m => m.TaxSpeciesCommonNameID == id);
            if (taxSpeciesCommonName == null)
            {
                return NotFound();
            }

            return View(taxSpeciesCommonName);
        }

        // POST: SpeciesCommonName/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxSpeciesCommonName = await _context.TaxSpeciesCommonName.FindAsync(id);
            _context.TaxSpeciesCommonName.Remove(taxSpeciesCommonName);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxSpeciesCommonNameExists(int id)
        {
            return _context.TaxSpeciesCommonName.Any(e => e.TaxSpeciesCommonNameID == id);
        }
    }
}
