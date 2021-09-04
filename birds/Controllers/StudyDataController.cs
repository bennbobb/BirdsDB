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
    public class StudyDataController : Controller
    {
        private readonly MvcBirdsContext _context;

        public StudyDataController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: StudyData
        public async Task<IActionResult> Index()
        {
            var mvcBirdsContext = _context.StudyData.Include(s => s.Continent).Include(s => s.Country).Include(s => s.Study).Include(s => s.TaxSpecies).Include(s => s.TaxSubSpecies);
            return View(await mvcBirdsContext.ToListAsync());
        }

        // GET: StudyData/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyData = await _context.StudyData
                .Include(s => s.Continent)
                .Include(s => s.Country)
                .Include(s => s.Study)
                .Include(s => s.TaxSpecies)
                .Include(s => s.TaxSubSpecies)
                .FirstOrDefaultAsync(m => m.StudyDataID == id);
            if (studyData == null)
            {
                return NotFound();
            }

            return View(studyData);
        }

        // GET: StudyData/Create
        public IActionResult Create()
        {
            ViewData["ContinentID"] = new SelectList(_context.Continents, "ContinentID", "ContinentName");
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryCode");
            ViewData["StudyID"] = new SelectList(_context.Studies, "StudyID", "StudyID");
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName");
            ViewData["TaxSubSpeciesID"] = new SelectList(_context.TaxSubSpecies, "TaxSubSpeciesID", "TaxSubSpeciesName");
            return View();
        }

        // POST: StudyData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudyDataID,StudyID,TaxSpeciesID,TaxSubSpeciesID,ContinentID,CountryID,Latitude,Longitude,Pellets,PreyRemains,Observations,DNA,BodyMass,Num_obs,Notes")] StudyData studyData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studyData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContinentID"] = new SelectList(_context.Continents, "ContinentID", "ContinentName", studyData.ContinentID);
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryCode", studyData.CountryID);
            ViewData["StudyID"] = new SelectList(_context.Studies, "StudyID", "StudyID", studyData.StudyID);
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName", studyData.TaxSpeciesID);
            ViewData["TaxSubSpeciesID"] = new SelectList(_context.TaxSubSpecies, "TaxSubSpeciesID", "TaxSubSpeciesName", studyData.TaxSubSpeciesID);
            return View(studyData);
        }

        // GET: StudyData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyData = await _context.StudyData.FindAsync(id);
            if (studyData == null)
            {
                return NotFound();
            }
            ViewData["ContinentID"] = new SelectList(_context.Continents, "ContinentID", "ContinentName", studyData.ContinentID);
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryCode", studyData.CountryID);
            ViewData["StudyID"] = new SelectList(_context.Studies, "StudyID", "StudyID", studyData.StudyID);
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName", studyData.TaxSpeciesID);
            ViewData["TaxSubSpeciesID"] = new SelectList(_context.TaxSubSpecies, "TaxSubSpeciesID", "TaxSubSpeciesName", studyData.TaxSubSpeciesID);
            return View(studyData);
        }

        // POST: StudyData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudyDataID,StudyID,TaxSpeciesID,TaxSubSpeciesID,ContinentID,CountryID,Latitude,Longitude,Pellets,PreyRemains,Observations,DNA,BodyMass,Num_obs,Notes")] StudyData studyData)
        {
            if (id != studyData.StudyDataID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studyData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudyDataExists(studyData.StudyDataID))
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
            ViewData["ContinentID"] = new SelectList(_context.Continents, "ContinentID", "ContinentName", studyData.ContinentID);
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryCode", studyData.CountryID);
            ViewData["StudyID"] = new SelectList(_context.Studies, "StudyID", "StudyID", studyData.StudyID);
            ViewData["TaxSpeciesID"] = new SelectList(_context.TaxSpecies, "TaxSpeciesID", "TaxSpeciesName", studyData.TaxSpeciesID);
            ViewData["TaxSubSpeciesID"] = new SelectList(_context.TaxSubSpecies, "TaxSubSpeciesID", "TaxSubSpeciesName", studyData.TaxSubSpeciesID);
            return View(studyData);
        }

        // GET: StudyData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studyData = await _context.StudyData
                .Include(s => s.Continent)
                .Include(s => s.Country)
                .Include(s => s.Study)
                .Include(s => s.TaxSpecies)
                .Include(s => s.TaxSubSpecies)
                .FirstOrDefaultAsync(m => m.StudyDataID == id);
            if (studyData == null)
            {
                return NotFound();
            }

            return View(studyData);
        }

        // POST: StudyData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studyData = await _context.StudyData.FindAsync(id);
            _context.StudyData.Remove(studyData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudyDataExists(int id)
        {
            return _context.StudyData.Any(e => e.StudyDataID == id);
        }
    }
}
