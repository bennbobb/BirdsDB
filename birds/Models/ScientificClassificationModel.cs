using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace birds.Models
{
    [Index(nameof(TaxDomainName), IsUnique = true)]
    public class TaxDomain
    {
        public int TaxDomainID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Domain")]
        public string TaxDomainName { get; set; }

        public ICollection<TaxKingdom> TaxKingdoms { get; set; }
    }

    [Index(nameof(TaxKingdomName), IsUnique = true)]
    public class TaxKingdom
    {
        public int TaxKingdomID { get; set; }
        public int TaxDomainID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Kingdom")]
        public string TaxKingdomName { get; set; }

        public TaxDomain TaxDomain { get; set; }
        public ICollection<TaxPhylum> TaxPhylums { get; set; }
    }

    [Index(nameof(TaxPhylumName), IsUnique = true)]
    public class TaxPhylum
    {
        public int TaxPhylumID { get; set; }
        public int TaxKingdomID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Phylum")]
        public string TaxPhylumName { get; set; }

        public TaxKingdom TaxKingdom { get; set; }
        public ICollection<TaxClass> TaxClasses { get; set; }
    }

    [Index(nameof(TaxClassName), IsUnique = true)]
    public class TaxClass
    {
        public int TaxClassID { get; set; }
        public int TaxPhylumID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Class")]
        public string TaxClassName { get; set; }

        public TaxPhylum TaxPhylum { get; set; }
        public ICollection<TaxOrder> TaxOrders { get; set; }
    }

    [Index(nameof(TaxOrderName), IsUnique = true)]
    public class TaxOrder
    {
        public int TaxOrderID { get; set; }
        public int TaxClassID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Order")]
        public string TaxOrderName { get; set; }

        public TaxClass TaxClass { get; set; }
        public ICollection<TaxFamily> TaxFamilies { get; set; }
    }

    [Index(nameof(TaxFamilyName), IsUnique = true)]
    public class TaxFamily
    {
        public int TaxFamilyID { get; set; }
        public int TaxOrderID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Family")]
        public string TaxFamilyName { get; set; }

        public TaxOrder TaxOrder { get; set; }
        public ICollection<TaxGenus> TaxGenuses { get; set; }
    }

    [Index(nameof(TaxGenusName), IsUnique = true)]
    public class TaxGenus
    {
        public int TaxGenusID { get; set; }
        public int TaxFamilyID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Genus")]
        public string TaxGenusName { get; set; }

        public TaxFamily TaxFamily { get; set; }
        public ICollection<TaxSpecies> TaxSpecies { get; set; }
    }

    public class TaxSpecies
    {
        public int TaxSpeciesID { get; set; }
        public int TaxGenusID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Species")]
        public string TaxSpeciesName { get; set; }

        public bool BirdLifeRecognised { get; set; }
        public string IUNC_RedListCategory { get; set; }
        public string Authority { get; set; }
        public string TaxSources { get; set; }
        public int SISRecID { get; set; }

        public TaxGenus TaxGenus { get; set; }
        public ICollection<TaxSubSpecies> TaxSubSpecies { get; set; }
        public ICollection<TaxSpeciesCommonName> TaxSpeciesCommonNames { get; set; }
    }

    public class TaxSpeciesCommonName
    {
        public int TaxSpeciesCommonNameID { get; set; }
        public int TaxSpeciesID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Common Name")]
        public string CommonName { get; set; }
        public bool IsPrimaryCommonName { get; set; }

        public TaxSpecies TaxSpecies { get; set; }
    }

    public class TaxSubSpecies
    {
        public int TaxSubSpeciesID { get; set; }
        public int TaxSpeciesID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Sub Species Name")]
        public string TaxSubSpeciesName { get; set; }

        public TaxSpecies TaxSpecies { get; set; }
        //public ICollection<TaxSpeciesCommonName> TaxSpeciesCommonNames { get; set; }
    }
}
