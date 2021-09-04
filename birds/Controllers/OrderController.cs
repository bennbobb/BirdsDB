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
    public class OrderController : Controller
    {
        private readonly MvcBirdsContext _context;

        public OrderController(MvcBirdsContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["OrderSortParm"] = sortOrder == null || sortOrder == "order_asc" ? "order_desc" : "order_asc";
            ViewData["ClassSortParm"] = sortOrder == "class_asc" ? "class_desc" : "class_asc";

            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString;

            var mvcBirdsContext = from x in _context.TaxOrders
                                  .Include(t => t.TaxClass)
                                  select x;

            if (!string.IsNullOrEmpty(searchString))
            {
                // * performance penalty * //
                mvcBirdsContext = mvcBirdsContext.Where(x => x.TaxOrderName.ToUpper().Contains(searchString.ToUpper()));
            }

            // Order results based on user input
            mvcBirdsContext = sortOrder switch
            {
                "class_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxClass.TaxClassName),
                "class_asc" => mvcBirdsContext.OrderBy(x => x.TaxClass.TaxClassName),
                "order_desc" => mvcBirdsContext.OrderByDescending(x => x.TaxOrderName),
                _ => mvcBirdsContext.OrderBy(x => x.TaxOrderName),
            };

            return View(await PaginatedList<TaxOrder>
                .CreateAsync(mvcBirdsContext.AsNoTracking(), pageNumber ?? 1));
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxOrder = await _context.TaxOrders
                .Include(t => t.TaxClass)
                .FirstOrDefaultAsync(m => m.TaxOrderID == id);
            if (taxOrder == null)
            {
                return NotFound();
            }

            return View(taxOrder);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            PopulateClassDropDownList();
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaxOrderID,TaxClassID,TaxOrderName")] TaxOrder taxOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taxOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateClassDropDownList(taxOrder);
            return View(taxOrder);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxOrder = await _context.TaxOrders.FindAsync(id);
            if (taxOrder == null)
            {
                return NotFound();
            }

            PopulateClassDropDownList(taxOrder);
            return View(taxOrder);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxOrderID,TaxClassID,TaxOrderName")] TaxOrder taxOrder)
        {
            if (id != taxOrder.TaxOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxOrderExists(taxOrder.TaxOrderID))
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

            PopulateClassDropDownList(taxOrder);
            return View(taxOrder);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxOrder = await _context.TaxOrders
                .Include(t => t.TaxClass)
                .FirstOrDefaultAsync(m => m.TaxOrderID == id);
            if (taxOrder == null)
            {
                return NotFound();
            }

            return View(taxOrder);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxOrder = await _context.TaxOrders.FindAsync(id);
            _context.TaxOrders.Remove(taxOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxOrderExists(int id)
        {
            return _context.TaxOrders.Any(e => e.TaxOrderID == id);
        }

        private void PopulateClassDropDownList(object taxClass = null)
        {
            ViewData["TaxClassID"] = new SelectList(_context.TaxClasses, "TaxClassID", "TaxClassName", taxClass);
        }
    }
}
