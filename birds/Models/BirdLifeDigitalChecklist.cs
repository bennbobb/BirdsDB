using Microsoft.EntityFrameworkCore;

namespace birds.Models
{
    [Keyless]
    public class BirdLifeDigitalChecklist
    {
        public int Seq { get; set; }
        public int SubsppSeq { get; set; }
        public string Order { get; set; }
        public string FamilyName { get; set; }
        public string Family { get; set; }
        public string Subfamily { get; set; }
        public string Tribe { get; set; }
        public string CommonName { get; set; }
        public string ScientificName { get; set; }
        public string Authority { get; set; }
        public string BirdLife_tax_treat { get; set; }
        public string IUNC_RedListCat { get; set; }
        public string Synonyms { get; set; }
        public string AlternativeCommonName { get; set; }
        public string TaxSources { get; set; }
        public int SISRecID { get; set; }
        public int SpcRecID { get; set; }
        public string SubsppID { get; set; }
    }
}