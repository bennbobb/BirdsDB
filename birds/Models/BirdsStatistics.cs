using System;
using System.Linq;

namespace birds.Models
{
    public class BirdsStatistics
    {
        private readonly MvcBirdsContext _context;

        public int numDomains, numKingdoms, numPhylums, numClasses, numOrders,
            numFamily, numGenus, numSpecies, numSubSpecies, numCommonNames;

        public BirdsStatistics(MvcBirdsContext context)
        {
            _context = context;
            numDomains = _context.TaxDomains.Count();
            numKingdoms = _context.TaxKingdoms.Count();
            numPhylums = _context.TaxPhylums.Count();
            numClasses = _context.TaxClasses.Count();
            numOrders = _context.TaxOrders.Count();
            numFamily = _context.TaxFamilies.Count();
            numGenus = _context.TaxGenuses.Count();
            numSpecies = _context.TaxSpecies.Where(x => x.BirdLifeRecognised).Count();
            numSubSpecies = _context.TaxSubSpecies.Count();
            numCommonNames = _context.TaxSpeciesCommonName.Count();
        }
    }
}
