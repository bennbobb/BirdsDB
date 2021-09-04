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
    public class SpeciesController : Controller
    {
        private readonly MvcBirdsContext _context;

        public SpeciesController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Species
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["GenusSortParm"] = sortOrder == null || sortOrder == "genus_asc" ? "genus_desc" : "genus_asc";
            ViewData["SpeciesSortParm"] = sortOrder == "species_asc" ? "species_desc" : "species_asc";
            ViewData["AuthoritySortParm"] = sortOrder == "authority_asc" ? "authority_desc" : "authority_asc";
            ViewData["BLRecognisedSortParm"] = sortOrder == "blrecognised_asc" ? "blrecognised_desc" : "blrecognised_asc";
            ViewData["RedListSortParm"] = sortOrder == "redlist_asc" ? "redlist_desc" : "redlist_asc";
            ViewData["SourcesSortParm"] = sortOrder == "sources_asc" ? "sources_desc" : "sources_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var mvcBirdsContext = from x in _context.TaxSpecies
                                  select x;

            mvcBirdsContext = mvcBirdsContext
                .Include(t => t.TaxGenus);

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxSpeciesName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "sources_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxSources),
                "sources_asc" => mvcBirdsContext.OrderBy(x => x.TaxSources),
                "redlist_desc" => mvcBirdsContext.OrderByDescending(x => x.IUNC_RedListCategory),
                "redlist_asc" => mvcBirdsContext.OrderBy(x => x.IUNC_RedListCategory),
                "blrecognised_desc" => mvcBirdsContext.OrderByDescending(x => x.BirdLifeRecognised),
                "blrecognised_asc" => mvcBirdsContext.OrderBy(x => x.BirdLifeRecognised),
                "authority_desc" => mvcBirdsContext.OrderByDescending(x => x.Authority),
                "authority_asc" => mvcBirdsContext.OrderBy(x => x.Authority),
                "species_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxSpeciesName),
                "species_asc" => mvcBirdsContext.OrderBy(x => x.TaxSpeciesName),
                "genus_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxGenus.TaxGenusName),
                _ => mvcBirdsContext.OrderBy(x => x.TaxGenus.TaxGenusName),
            };

            return View(await PaginatedList<TaxSpecies>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: Species/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxSpecies = await _context.TaxSpecies
                .Include(t => t.TaxGenus)
                .FirstOrDefaultAsync(m => m.TaxSpeciesID == id);
            if (taxSpecies == null)
            {
                return NotFound();
            }

            return View(taxSpecies);
        }

        // GET: Species/Create
        public IActionResult Create()
        {
            ViewData["TaxGenusID"] = new SelectList(_context.TaxGenuses, "TaxGenusID", "TaxGenusName");
            return View();
        }

        // POST: Species/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxSpeciesID,TaxGenusID,TaxSpeciesName,Authority,BirdLifeRecognised,IUNC_RedListCategory,TaxSources,SISRecID")] TaxSpecies taxSpecies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxSpecies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaxGenusID"] = new SelectList(_context.TaxGenuses, "TaxGenusID", "TaxGenusName", taxSpecies.TaxGenusID);
            return View(taxSpecies);
        }

        // GET: Species/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxSpecies = await _context.TaxSpecies.FindAsync(id);
            if (taxSpecies == null)
            {
                return NotFound();
            }
            ViewData["TaxGenusID"] = new SelectList(_context.TaxGenuses, "TaxGenusID", "TaxGenusName", taxSpecies.TaxGenusID);
            return View(taxSpecies);
        }

        // POST: Species/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxSpeciesID,TaxGenusID,TaxSpeciesName,Authority,BirdLifeRecognised,IUNC_RedListCategory,TaxSources,SISRecID")] TaxSpecies taxSpecies)
        {
            if (id != taxSpecies.TaxSpeciesID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxSpecies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxSpeciesExists(taxSpecies.TaxSpeciesID))
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
            ViewData["TaxGenusID"] = new SelectList(_context.TaxGenuses, "TaxGenusID", "TaxGenusName", taxSpecies.TaxGenusID);
            return View(taxSpecies);
        }

        // GET: Species/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxSpecies = await _context.TaxSpecies
                .Include(t => t.TaxGenus)
                .FirstOrDefaultAsync(m => m.TaxSpeciesID == id);
            if (taxSpecies == null)
            {
                return NotFound();
            }

            return View(taxSpecies);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxSpecies = await _context.TaxSpecies.FindAsync(id);
            _context.TaxSpecies.Remove(taxSpecies);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxSpeciesExists(int id)
        {
            return _context.TaxSpecies.Any(e => e.TaxSpeciesID == id);
        }
    }
}
