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
    public class StudyController : Controller
    {
        private readonly MvcBirdsContext _context;

        public StudyController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Study
        public async Task<IActionResult> Index()
        {
            var mvcBirdsContext = _context.Studies.Include(s => s.Journal);
            return View(await mvcBirdsContext.ToListAsync());
        }

        // GET: Study/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var study = await _context.Studies
                .Include(s => s.Journal)
                .FirstOrDefaultAsync(m => m.StudyID == id);
            if (study == null)
            {
                return NotFound();
            }

            return View(study);
        }

        // GET: Study/Create
        public IActionResult Create()
        {
            ViewData["JournalID"] = new SelectList(_context.Journals, "JournalID", "JournalName");
            return View();
        }

        // POST: Study/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudyID,JournalID,StudyName,Author,Year,StudyDescription")] Study study)
        {
            if (ModelState.IsValid)
            {
                _context.Add(study);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JournalID"] = new SelectList(_context.Journals, "JournalID", "JournalName", study.JournalID);
            return View(study);
        }

        // GET: Study/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var study = await _context.Studies.FindAsync(id);
            if (study == null)
            {
                return NotFound();
            }
            ViewData["JournalID"] = new SelectList(_context.Journals, "JournalID", "JournalName", study.JournalID);
            return View(study);
        }

        // POST: Study/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudyID,JournalID,StudyName,Author,Year,StudyDescription")] Study study)
        {
            if (id != study.StudyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(study);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudyExists(study.StudyID))
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
            ViewData["JournalID"] = new SelectList(_context.Journals, "JournalID", "JournalName", study.JournalID);
            return View(study);
        }

        // GET: Study/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var study = await _context.Studies
                .Include(s => s.Journal)
                .FirstOrDefaultAsync(m => m.StudyID == id);
            if (study == null)
            {
                return NotFound();
            }

            return View(study);
        }

        // POST: Study/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var study = await _context.Studies.FindAsync(id);
            _context.Studies.Remove(study);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudyExists(int id)
        {
            return _context.Studies.Any(e => e.StudyID == id);
        }
    }
}
