using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using birds.Models;
using static birds.Controllers.HelperClass;

namespace birds.Controllers
{
    [ViewLayout("_LayoutStudies")]
    public class ContinentController : Controller
    {
        private readonly MvcBirdsContext _context;

        public ContinentController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Continent
        public async Task<IActionResult> Index()
        {
            return View(await _context.Continents.ToListAsync());
        }

        // GET: Continent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var continent = await _context.Continents
                .FirstOrDefaultAsync(m => m.ContinentID == id);
            if (continent == null)
            {
                return NotFound();
            }

            return View(continent);
        }

        // GET: Continent/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Continent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContinentID,ContinentName")] Continent continent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(continent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(continent);
        }

        // GET: Continent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var continent = await _context.Continents.FindAsync(id);
            if (continent == null)
            {
                return NotFound();
            }
            return View(continent);
        }

        // POST: Continent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContinentID,ContinentName")] Continent continent)
        {
            if (id != continent.ContinentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(continent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContinentExists(continent.ContinentID))
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
            return View(continent);
        }

        // GET: Continent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var continent = await _context.Continents
                .FirstOrDefaultAsync(m => m.ContinentID == id);
            if (continent == null)
            {
                return NotFound();
            }

            return View(continent);
        }

        // POST: Continent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var continent = await _context.Continents.FindAsync(id);
            _context.Continents.Remove(continent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContinentExists(int id)
        {
            return _context.Continents.Any(e => e.ContinentID == id);
        }
    }
}
