using Microsoft.EntityFrameworkCore;
using birds.Models;

public class MvcBirdsContext : DbContext
{
    public MvcBirdsContext(DbContextOptions<MvcBirdsContext> options)
        : base(options)
    {
    }

    // Used for importing Birdlife Data into a temp/holding table
    public DbSet<birds.Models.BirdLifeDigitalChecklist> BirdChecklist { get; set; }

    //Taxanomic
    public DbSet<birds.Models.TaxDomain> TaxDomains { get; set; }

    public DbSet<birds.Models.TaxKingdom> TaxKingdoms { get; set; }

    public DbSet<birds.Models.TaxPhylum> TaxPhylums { get; set; }

    public DbSet<birds.Models.TaxClass> TaxClasses { get; set; }

    public DbSet<birds.Models.TaxOrder> TaxOrders { get; set; }

    public DbSet<birds.Models.TaxFamily> TaxFamilies { get; set; }

    public DbSet<birds.Models.TaxGenus> TaxGenuses { get; set; }

    public DbSet<birds.Models.TaxSpecies> TaxSpecies { get; set; }

    public DbSet<birds.Models.TaxSubSpecies> TaxSubSpecies { get; set; }

    public DbSet<birds.Models.TaxSpeciesCommonName> TaxSpeciesCommonName { get; set; }


    // General
    public DbSet<birds.Models.Continent> Continents { get; set; }

    public DbSet<birds.Models.Country> Countries { get; set; }

    public DbSet<birds.Models.Habitat> Habitats { get; set; }

    public DbSet<birds.Models.Season> Seasons { get; set; }


    // Studies
    public DbSet<birds.Models.Journal> Journals { get; set; }

    public DbSet<birds.Models.Study> Studies { get; set; }

    public DbSet<birds.Models.StudyData> StudyData { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxDomain>().ToTable("TaxDomain");
        modelBuilder.Entity<TaxDomain>().Property(x => x.TaxDomainName)
    .UseCollation("NOCASE");

        modelBuilder.Entity<TaxKingdom>().ToTable("TaxKingdom");
        modelBuilder.Entity<TaxKingdom>().Property(x => x.TaxKingdomName)
    .UseCollation("NOCASE");

        modelBuilder.Entity<TaxPhylum>().ToTable("TaxPhylum");
        modelBuilder.Entity<TaxPhylum>().Property(x => x.TaxPhylumName)
    .UseCollation("NOCASE");

        modelBuilder.Entity<TaxClass>().ToTable("TaxClass");
        modelBuilder.Entity<TaxClass>().Property(x => x.TaxClassName)
    .UseCollation("NOCASE");

        modelBuilder.Entity<TaxOrder>().ToTable("TaxOrder");
        modelBuilder.Entity<TaxOrder>().Property(x => x.TaxOrderName)
    .UseCollation("NOCASE");

        modelBuilder.Entity<TaxFamily>().ToTable("TaxFamily");
        modelBuilder.Entity<TaxFamily>().Property(x => x.TaxFamilyName)
    .UseCollation("NOCASE");

        modelBuilder.Entity<TaxGenus>().ToTable("TaxGenus");
        modelBuilder.Entity<TaxGenus>().Property(x => x.TaxGenusName)
    .UseCollation("NOCASE");

        modelBuilder.Entity<TaxSpecies>().ToTable("TaxSpecies");
        modelBuilder.Entity<TaxSpecies>().Property(x => x.TaxSpeciesName)
    .UseCollation("NOCASE");

        modelBuilder.Entity<TaxSubSpecies>().ToTable("TaxSubSpecies");
        modelBuilder.Entity<TaxSubSpecies>().Property(x => x.TaxSubSpeciesName)
    .UseCollation("NOCASE");

        modelBuilder.Entity<TaxSpeciesCommonName>().ToTable("TaxSpeciesCommonName");
        modelBuilder.Entity<TaxSpeciesCommonName>().Property(x => x.CommonName)
    .UseCollation("NOCASE");


        // Habitat Config
        // manually populated composite key
        modelBuilder.Entity<Habitat>().HasKey(x => new { x.HabitatHierarchy1, x.HabitatHierarchy2, x.HabitatHierarchy3 });

        modelBuilder.Entity<Habitat>()
        .Property(x => x.HabitatHierarchy1)
        .ValueGeneratedNever();

        modelBuilder.Entity<Habitat>()
        .Property(x => x.HabitatHierarchy2)
        .ValueGeneratedNever();

        modelBuilder.Entity<Habitat>()
        .Property(x => x.HabitatHierarchy3)
        .ValueGeneratedNever();

        // Study Data Habitat Config
        // manually populated composite key
        modelBuilder.Entity<StudyDataHabitat>()
            .HasKey(x => new { x.StudyDataID, x.HabitatHierarchy1, x.HabitatHierarchy2, x.HabitatHierarchy3 });

        modelBuilder.Entity<StudyDataHabitat>()
        .Property(x => x.StudyDataID)
        .ValueGeneratedNever();

        modelBuilder.Entity<StudyDataHabitat>()
        .Property(x => x.HabitatHierarchy1)
        .ValueGeneratedNever();

        modelBuilder.Entity<StudyDataHabitat>()
        .Property(x => x.HabitatHierarchy2)
        .ValueGeneratedNever();

        modelBuilder.Entity<StudyDataHabitat>()
        .Property(x => x.HabitatHierarchy3)
        .ValueGeneratedNever();


        // Study Data Season Config
        // manually populated composite key
        modelBuilder.Entity<StudyDataSeason>()
            .HasKey(x => new { x.StudyDataID, x.SeasonID });

        modelBuilder.Entity<StudyDataSeason>()
        .Property(x => x.StudyDataID)
        .ValueGeneratedNever();

        modelBuilder.Entity<StudyDataSeason>()
        .Property(x => x.SeasonID)
        .ValueGeneratedNever();
    }
}