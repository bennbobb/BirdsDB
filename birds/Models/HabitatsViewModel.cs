namespace birds.Models
{
    public class HabitatsViewModel
    {
        public int HabitatHierarchy1 { get; set; } // Parent ID

        public int HabitatHierarchy2 { get; set; } // Child ID

        public int HabitatHierarchy3 { get; set; } // GrandChild ID

        public string HabitatHierarchy1Name { get; set; }

        public string HabitatHierarchy2Name { get; set; }

        public string HabitatHierarchy3Name { get; set; }

        public string HabitatDescription { get; set; }

        public bool IUNC_RedListRecognised { get; set; }

        // Calculated Field
        public string HabitatHierarchyID => $"{HabitatHierarchy1}.{HabitatHierarchy2}.{HabitatHierarchy3}";
    }
}
