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
    public class JournalController : Controller
    {
        private readonly MvcBirdsContext _context;

        public JournalController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Journal
        public async Task<IActionResult> Index()
        {
            return View(await _context.Journals.ToListAsync());
        }

        // GET: Journal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _context.Journals
                .FirstOrDefaultAsync(m => m.JournalID == id);
            if (journal == null)
            {
                return NotFound();
            }

            return View(journal);
        }

        // GET: Journal/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Journal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JournalID,JournalName,JournalDescription")] Journal journal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(journal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(journal);
        }

        // GET: Journal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _context.Journals.FindAsync(id);
            if (journal == null)
            {
                return NotFound();
            }
            return View(journal);
        }

        // POST: Journal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JournalID,JournalName,JournalDescription")] Journal journal)
        {
            if (id != journal.JournalID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(journal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JournalExists(journal.JournalID))
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
            return View(journal);
        }

        // GET: Journal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _context.Journals
                .FirstOrDefaultAsync(m => m.JournalID == id);
            if (journal == null)
            {
                return NotFound();
            }

            return View(journal);
        }

        // POST: Journal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var journal = await _context.Journals.FindAsync(id);
            _context.Journals.Remove(journal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JournalExists(int id)
        {
            return _context.Journals.Any(e => e.JournalID == id);
        }
    }
}
