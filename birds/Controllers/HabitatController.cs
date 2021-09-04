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
    public class HabitatController : Controller
    {
        private readonly MvcBirdsContext _context;

        public HabitatController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Habitat
        public async Task<IActionResult> Index()
        {
            // Need to add support for 3rd level of Hierarchy // Not yet tested results
            var habitatsL1 = from h1 in _context.Habitats
                           where h1.HabitatHierarchy2 == 0
                              && h1.HabitatHierarchy3 == 0
                             select new HabitatsViewModel
                           {
                               HabitatHierarchy1 = h1.HabitatHierarchy1,
                               HabitatHierarchy2 = h1.HabitatHierarchy2,
                               HabitatHierarchy3 = h1.HabitatHierarchy3,
                               HabitatHierarchy1Name = h1.HabitatName,
                               HabitatHierarchy2Name = null,
                               HabitatHierarchy3Name = null,
                               HabitatDescription = h1.HabitatDescription,
                               IUNC_RedListRecognised = h1.IUNC_RedListRecognised
                           };

            var habitatsL2 = from h2 in _context.Habitats
                             join h1 in _context.Habitats on h2.HabitatHierarchy1 equals h1.HabitatHierarchy1
                             where h1.HabitatHierarchy2 == 0
                                && h1.HabitatHierarchy3 == 0
                             && h2.HabitatHierarchy2 != 0
                             && h2.HabitatHierarchy3 == 0
                             select new HabitatsViewModel
                             {
                                 HabitatHierarchy1 = h2.HabitatHierarchy1,
                                 HabitatHierarchy2 = h2.HabitatHierarchy2,
                                 HabitatHierarchy3 = h2.HabitatHierarchy3,
                                 HabitatHierarchy1Name = h1.HabitatName,
                                 HabitatHierarchy2Name = h2.HabitatName,
                                 HabitatHierarchy3Name = null,
                                 HabitatDescription = h2.HabitatDescription,
                                 IUNC_RedListRecognised = h2.IUNC_RedListRecognised
                             };

            var habitatsL3 = from h3 in _context.Habitats
                             join h1 in _context.Habitats on h3.HabitatHierarchy1 equals h1.HabitatHierarchy1
                             join h2 in _context.Habitats on new { h3.HabitatHierarchy1, h3.HabitatHierarchy2 }
                                                        equals new { h2.HabitatHierarchy1, h2.HabitatHierarchy2 }
                             where h1.HabitatHierarchy2 == 0
                                && h1.HabitatHierarchy3 == 0
                                && h2.HabitatHierarchy2 != 0
                                && h2.HabitatHierarchy3 == 0
                                && h3.HabitatHierarchy3 != 0
                             select new HabitatsViewModel
                             {
                                 HabitatHierarchy1 = h3.HabitatHierarchy1,
                                 HabitatHierarchy2 = h3.HabitatHierarchy2,
                                 HabitatHierarchy3 = h3.HabitatHierarchy3,
                                 HabitatHierarchy1Name = h1.HabitatName,
                                 HabitatHierarchy2Name = h2.HabitatName,
                                 HabitatHierarchy3Name = h3.HabitatName,
                                 HabitatDescription = h3.HabitatDescription,
                                 IUNC_RedListRecognised = h3.IUNC_RedListRecognised
                             };

            var habitatsCombinedList = habitatsL1.Union(habitatsL2).Union(habitatsL3)
                                                .OrderBy(x => x.HabitatHierarchy1)
                                                .ThenBy(x => x.HabitatHierarchy2);
            return base.View(await habitatsCombinedList.ToListAsync());
        }



        // GET: Habitat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitat = await _context.Habitats
                .FirstOrDefaultAsync(m => m.HabitatHierarchy1 == id);
            if (habitat == null)
            {
                return NotFound();
            }

            return View(habitat);
        }

        // GET: Habitat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Habitat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HabitatCategoryID,HabitatSubCategoryID,HabitatName,HabitatDescription,IUNC_RedListRecognised")] Habitat habitat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habitat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(habitat);
        }

        // GET: Habitat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitat = await _context.Habitats.FindAsync(id);
            if (habitat == null)
            {
                return NotFound();
            }
            return View(habitat);
        }

        // POST: Habitat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HabitatCategoryID,HabitatSubCategoryID,HabitatName,HabitatDescription,IUNC_RedListRecognised")] Habitat habitat)
        {
            if (id != habitat.HabitatHierarchy1)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habitat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitatExists(habitat.HabitatHierarchy1))
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
            return View(habitat);
        }

        // GET: Habitat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitat = await _context.Habitats
                .FirstOrDefaultAsync(m => m.HabitatHierarchy1 == id);
            if (habitat == null)
            {
                return NotFound();
            }

            return View(habitat);
        }

        // POST: Habitat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habitat = await _context.Habitats.FindAsync(id);
            _context.Habitats.Remove(habitat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitatExists(int id)
        {
            return _context.Habitats.Any(e => e.HabitatHierarchy1 == id);
        }
    }
}
