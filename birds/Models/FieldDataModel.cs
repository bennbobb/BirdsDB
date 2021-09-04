using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace birds.Models
{
    [Index(nameof(ContinentName), IsUnique = true)]
    public class Continent
    {
        public int ContinentID { get; set; }

        [Required]
        [Display(Name = "Continent")]
        public string ContinentName { get; set; }

        public ICollection<Country> Countries { get; set; }
    }

    [Index(nameof(CountryName), IsUnique = true)]
    [Index(nameof(CountryCode), IsUnique = true)]
    public class Country
    {
        public int CountryID { get; set; }
        public int ContinentID { get; set; }

        [Required]
        [StringLength(6)]
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Country")]
        public string CountryName { get; set; }

        public Continent Continent { get; set; }
    }

    [Index(nameof(SeasonName), IsUnique = true)]
    public class Season
    {
        public int SeasonID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Season")]
        public string SeasonName { get; set; }

        [Display(Name = "Description")]
        public string SeasonDescription { get; set; }
    }

    // Configured using the Fluent API;
    public class Habitat
    {
        public int HabitatHierarchy1 { get; set; }
        public int HabitatHierarchy2 { get; set; }
        public int HabitatHierarchy3 { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Habitat Category")]
        public string HabitatName { get; set; }

        [Display(Name = "Description")]
        public string HabitatDescription { get; set; }

        public bool IUNC_RedListRecognised { get; set; }
    }

    [Index(nameof(JournalName), IsUnique = true)]
    public class Journal
    {
        public int JournalID { get; set; }

        [Required]
        [StringLength(450)]
        [Display(Name = "Journal")]
        public string JournalName { get; set; }

        [Display(Name = "Description")]
        public string JournalDescription { get; set; }

        public ICollection<Study> Studies { get; set; }
    }

    [Index(nameof(StudyName), nameof(Author), nameof(Year), IsUnique = true)]
    public class Study
    {
        public int StudyID { get; set; }
        public int JournalID { get; set; }

        [StringLength(450)]
        [Display(Name = "Study Author & Year")]
        public string StudyName { get; set; }

        [Display(Name = "Author/s")]
        public string Author { get; set; }

        public DateTime Year { get; set; }

        public int GetYear { get { return Year.Year; } }

        [Display(Name = "Description")]
        public string StudyDescription { get; set; }

        public Journal Journal { get; set; }
    }

    public class StudyData
    {
        public int StudyDataID { get; set; }

        public int StudyID { get; set; }
        public Study Study { get; set; }

        [Required]
        public int TaxSpeciesID { get; set; }
        public TaxSpecies TaxSpecies { get; set; }

        public int TaxSubSpeciesID { get; set; }
        public TaxSubSpecies TaxSubSpecies { get; set; }

        // Country or Continent can be null
        public int? ContinentID { get; set; }
        public Continent Continent { get; set; }

        public int? CountryID { get; set; }
        public Country Country { get; set; }

        //public Point Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool Pellets { get; set; }
        public bool PreyRemains { get; set; }
        public bool Observations { get; set; }
        public bool DNA { get; set; }
        public double BodyMass { get; set; }
        public const string BodyMassDescription = "Body mass(g)(EltonTraits)";
        public int Num_obs { get; set; }
        public string Notes { get; set; }


        public ICollection<StudyDataHabitat> StudyDataHabitats { get; set; }
        public ICollection<StudyDataSeason> StudyDataSeasons { get; set; }
    }

    // Composite keys configured using the Fluent API;
    public class StudyDataHabitat
    {
        public int StudyDataID { get; set; }
        public int HabitatHierarchy1 { get; set; }
        public int HabitatHierarchy2 { get; set; }
        public int HabitatHierarchy3 { get; set; }

        public int HabitatPercentage { get; set; }
        public const string HabitatPercentageDescription = "When study data is collected in multiple habitats"
            + " a percentage can be given to represent how much of the data comes from each of the habitat types";

        public StudyData StudyData { get; set; }
        public Habitat Habitat{ get; set; }
    }

    // Composite keys configured using the Fluent API;
    public class StudyDataSeason
    {
        public int StudyDataID { get; set; }
        public int SeasonID { get; set; }

        public StudyData StudyData { get; set; }
        public Season Season { get; set; }
    }
}
