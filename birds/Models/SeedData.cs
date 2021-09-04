using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace birds.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new MvcBirdsContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcBirdsContext>>());

            // Look for any DB entries.
            if (!context.Countries.Any())
            {
                #region Seed Continents

                context.Continents.AddRange(
                    new Continent
                    {
                        ContinentName = "Africa"
                    },
                    new Continent
                    {
                        ContinentName = "Afro-Eurasia"
                    },
                    new Continent
                    {
                        ContinentName = "Eurasia"
                    },
                    new Continent
                    {
                        ContinentName = "Europe"
                    },
                    new Continent
                    {
                        ContinentName = "Asia"
                    },
                    new Continent
                    {
                        ContinentName = "Americas"
                    },
                    new Continent
                    {
                        ContinentName = "North America"
                    },
                    new Continent
                    {
                        ContinentName = "South America"
                    },
                    new Continent
                    {
                        ContinentName = "Australia"
                    },
                    new Continent
                    {
                        ContinentName = "Oceania"
                    },
                    new Continent
                    {
                        ContinentName = "Antarctica"
                    }
                );

                context.SaveChanges();

                #endregion

                #region Seed Countries

                context.Countries.AddRange(
                    new Country
                    {
                        CountryCode = "Archaea",
                        CountryName = "Archaea",
                        ContinentID = context.Continents.Single(x => x.ContinentName == "Eurasia").ContinentID

                    }
                );
                context.SaveChanges();

                #endregion

                #region Seed Habitats

                // Habitats imported from CSV - RedListHabitatsCleaned.csv
                // CSV Data source https://www.iucnredlist.org/resources/habitat-classification-scheme

                #endregion

                #region Seed Seasons

                context.Seasons.AddRange(
                    new Season
                    {
                        SeasonName = "Spring",
                        SeasonDescription = "Spring"
                    }, new Season
                    {
                        SeasonName = "Summer",
                        SeasonDescription = "Summer"
                    }, new Season
                    {
                        SeasonName = "Autumn",
                        SeasonDescription = "Autumn also known as fall"
                    }, new Season
                    {
                        SeasonName = "Winter",
                        SeasonDescription = "Winter"
                    }, new Season
                    {
                        SeasonName = "Year Long",
                        SeasonDescription = "All seasons in the year"
                    }
                );
                context.SaveChanges();

                #endregion
            }

            if (context.TaxSpecies.Any())
            {
                return;   // DB has been seeded
            }

            context.Database.ExecuteSqlRaw("DELETE FROM TaxDomain");
            context.SaveChanges();

            #region Seed Domains

            context.TaxDomains.AddRange(
                new TaxDomain
                {
                    TaxDomainName = "Archaea"
                },

                new TaxDomain
                {
                    TaxDomainName = "Bacteria"
                },

                new TaxDomain
                {
                    TaxDomainName = "Eukarya"
                }
            );
            context.SaveChanges();

            #endregion

            #region Seed Kingdoms

            context.TaxKingdoms.AddRange(
                new TaxKingdom
                {
                    TaxKingdomName = "Fungi",
                    TaxDomainID = context.TaxDomains.Single(i => i.TaxDomainName == "Eukarya").TaxDomainID
                },

                 new TaxKingdom
                 {
                     TaxKingdomName = "Plantae",
                     TaxDomainID = context.TaxDomains.Single(i => i.TaxDomainName == "Eukarya").TaxDomainID
                 },

                  new TaxKingdom
                  {
                      TaxKingdomName = "Animalia",
                      TaxDomainID = context.TaxDomains.Single(i => i.TaxDomainName == "Eukarya").TaxDomainID
                  }
            );
            context.SaveChanges();

            #endregion

            #region Seed Phylums

            context.TaxPhylums.AddRange(
                new TaxPhylum
                {
                    TaxPhylumName = "Chordata",
                    TaxKingdomID = context.TaxKingdoms.Single(i => i.TaxKingdomName == "Animalia").TaxKingdomID
                }
            );
            context.SaveChanges();

            #endregion

            #region Seed Classes

            context.TaxClasses.AddRange(
                new TaxClass
                {
                    TaxClassName = "Aves",
                    TaxPhylumID = context.TaxPhylums.Single(i => i.TaxPhylumName == "Chordata").TaxPhylumID
                }
            );
            context.SaveChanges();

            #endregion

            #region Seed Orders

            context.TaxOrders.AddRange(
                new TaxOrder
                {
                    TaxOrderName = "Acciptriformes",
                    TaxClassID = context.TaxClasses.Single(i => i.TaxClassName == "Aves").TaxClassID
                },
                new TaxOrder
                {
                    TaxOrderName = "Falconiformes",
                    TaxClassID = context.TaxClasses.Single(i => i.TaxClassName == "Aves").TaxClassID
                },
                new TaxOrder
                {
                    TaxOrderName = "Strigiformes",
                    TaxClassID = context.TaxClasses.Single(i => i.TaxClassName == "Aves").TaxClassID
                }
            );
            context.SaveChanges();

            #endregion

            #region Seed Familes

            context.TaxFamilies.AddRange(
                new TaxFamily
                {
                    TaxFamilyName = "Acciptridae",
                    TaxOrderID = context.TaxOrders.Single(i => i.TaxOrderName == "Acciptriformes").TaxOrderID
                },
                new TaxFamily
                {
                    TaxFamilyName = "Falconidae",
                    TaxOrderID = context.TaxOrders.Single(i => i.TaxOrderName == "Falconiformes").TaxOrderID
                },
                new TaxFamily
                {
                    TaxFamilyName = "Strigidae",
                    TaxOrderID = context.TaxOrders.Single(i => i.TaxOrderName == "Strigiformes").TaxOrderID
                },
                new TaxFamily
                {
                    TaxFamilyName = "Tytonidae",
                    TaxOrderID = context.TaxOrders.Single(i => i.TaxOrderName == "Strigiformes").TaxOrderID
                });
            context.SaveChanges();

            #endregion

            #region Seed Genuses

            context.TaxGenuses.AddRange(
                new TaxGenus
                {
                    TaxGenusName = "Accipiter",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Aegolius",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Strigidae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Aquila",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Athene",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Strigidae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Bubo",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Strigidae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Buteo",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Buteogallus",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Circaetus",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Elanus",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Falco",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Falconidae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Geranoaetus",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Glaucidium",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Strigidae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Haliaeetus",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Haliastur",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Harpia",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Hieraaetus",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Ninox",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Strigidae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Spilornis",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Spizaetus",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Stephanoaetus",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                },
                new TaxGenus
                {
                    TaxGenusName = "Tyto",
                    TaxFamilyID = context.TaxFamilies.Single(i => i.TaxFamilyName == "Acciptridae").TaxFamilyID
                }
            );
            context.SaveChanges();

            #endregion

            #region Seed Species

            context.TaxSpecies.AddRange(
                new TaxSpecies
                {
                    TaxSpeciesName = "Accipiter cooperii",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Accipiter").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Accipiter gentilis",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Accipiter").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Accipiter striatus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Accipiter").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Accipiter fasciatus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Accipiter").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Accipiter nisus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Accipiter").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Aegolius acadicus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Aegolius").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Aquila chrysaetos",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Aquila").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Aquila fasciata",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Aquila").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Aquila audax",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Aquila").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Aquila heliaca",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Aquila").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Athene noctua",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Athene").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Bubo virginianus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Bubo").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Bubo ascalaphus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Bubo").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Bubo bubo",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Bubo").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Buteo hemilasius",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Buteo").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Buteo rufinus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Buteo").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Buteo regalis",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Buteo").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Buteo swainsoni",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Buteo").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Buteo jamaicensis",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Buteo").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Buteogallus coronatus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Buteogallus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Circaetus gallicus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Circaetus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Elanus leucurus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Elanus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Elanus caeruleus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Elanus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Falco cherrug",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Falco").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Falco mexicanus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Falco").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Falco biarmicus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Falco").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Falco sparverius",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Falco").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Falco longipennis",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Falco").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Falco peregrinus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Falco").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Falco columbarius",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Falco").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Falco femoralis",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Falco").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Geranoaetus melanoleucus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Geranoaetus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Glaucidium californicum",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Glaucidium").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Haliaeetus leucocephalus ",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Haliaeetus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Haliaeetus leucogaster",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Haliaeetus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Haliaeetus albicilla",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Haliaeetus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Haliastur sphenurus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Haliastur").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Harpia harpyja",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Harpia").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Hieraaetus pennatus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Hieraaetus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Hieraaetus morphnoides",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Hieraaetus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Ninox novaeseelandiae",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Ninox").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Spilornis cheela ",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Spilornis").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Spizaetus isidori",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Spizaetus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Stephanoaetus coronatus",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Stephanoaetus").TaxGenusID
                },
                new TaxSpecies
                {
                    TaxSpeciesName = "Tyto alba",
                    TaxGenusID = context.TaxGenuses.Single(i => i.TaxGenusName == "Tyto").TaxGenusID
                }
            );
            context.SaveChanges();

            #endregion

        }
    }
}
