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
    public class SubSpeciesController : Controller
    {
        private readonly MvcBirdsContext _context;

        public SubSpeciesController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: SubSpecies
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SubSpeciesSortParm"] = sortOrder == null || sortOrder == "subspecies_asc" ? "subspecies_desc" : "subspecies_asc";
            ViewData["SpeciesSortParm"] = sortOrder == "species_asc" ? "species_desc" : "species_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var mvcBirdsContext = from x in _context.TaxSubSpecies
                                  .Include(t => t.TaxSpecies)
                                  select x;

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxSubSpeciesName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "species_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxSpecies.TaxSpeciesName),
                "species_asc" => mvcBirdsContext.OrderBy(x => x.TaxSpecies.TaxSpeciesName),
                "subspecies_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxSubSpeciesName),
                _ => mvcBirdsContext.OrderBy(x => x.TaxSubSpeciesName),
            };

            return View(await PaginatedList<TaxSubSpecies>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: SubSpecies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxSubSpecies = await _context.TaxSubSpecies
                .Include(t => t.TaxSpecies)
                .FirstOrDefaultAsync(m => m.TaxSubSpeciesID == id);
            if (taxSubSpecies == null)
            {
                return NotFound();
            }

            return View(taxSubSpecies);
        }

        // GET: SubSpecies/Create
        public IActionResult Create()
        {
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName");
            return View();
        }

        // POST: SubSpecies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxSubSpeciesID,TaxSpeciesID,SubSpeciesName")] TaxSubSpecies taxSubSpecies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxSubSpecies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName", taxSubSpecies.TaxSpeciesID);
            return View(taxSubSpecies);
        }

        // GET: SubSpecies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxSubSpecies = await _context.TaxSubSpecies.FindAsync(id);
            if (taxSubSpecies == null)
            {
                return NotFound();
            }
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName", taxSubSpecies.TaxSpeciesID);
            return View(taxSubSpecies);
        }

        // POST: SubSpecies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxSubSpeciesID,TaxSpeciesID,SubSpeciesName")] TaxSubSpecies taxSubSpecies)
        {
            if (id != taxSubSpecies.TaxSubSpeciesID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxSubSpecies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxSubSpeciesExists(taxSubSpecies.TaxSubSpeciesID))
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
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName", taxSubSpecies.TaxSpeciesID);
            return View(taxSubSpecies);
        }

        // GET: SubSpecies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxSubSpecies = await _context.TaxSubSpecies
                .Include(t => t.TaxSpecies)
                .FirstOrDefaultAsync(m => m.TaxSubSpeciesID == id);
            if (taxSubSpecies == null)
            {
                return NotFound();
            }

            return View(taxSubSpecies);
        }

        // POST: SubSpecies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxSubSpecies = await _context.TaxSubSpecies.FindAsync(id);
            _context.TaxSubSpecies.Remove(taxSubSpecies);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxSubSpeciesExists(int id)
        {
            return _context.TaxSubSpecies.Any(e => e.TaxSubSpeciesID == id);
        }
    }
}
