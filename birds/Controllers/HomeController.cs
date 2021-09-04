using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using birds.Models;
using Microsoft.EntityFrameworkCore;
using static birds.Controllers.HelperClass;

namespace birds.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MvcBirdsContext _context;
        private readonly BirdsStatistics _birdStats;

        public HomeController(ILogger<HomeController> logger, MvcBirdsContext context)
        {
            _logger = logger;

            _context = context;
            _birdStats = new BirdsStatistics(_context);
        }

        // GET: Load data
        public IActionResult Index()
        {
            return View(_birdStats);
        }

        [ViewLayout("_LayoutStudies")]
        public IActionResult IndexStudies()
        {
            return View(_birdStats);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
