using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Gamer;
using CellWar.Model.Substance;

namespace CellWar.Test.Mock {
    public class Mocks {
        public static Strain Strain1 { get; set; } = new Strain {
            Name = "Strain1",
            Owner = MockPlayerCyf,
            Population = 100,
        };
        public static Strain Strain2 { get; set; } = new Strain {
            Name = "Strain2",
            Owner = MockPlayerCyf,
            Population = 200,
        };
        public static Strain Strain3 { get; set; } = new Strain {
            Name = "Strain3",
            Owner = MockPlayerCyf,
            Population = 300,
        };

        public static List<Strain> MockStrainList { get; set; } = new List<Strain>();

        public static Player MockPlayerCyf { get; set; } = new Player {
            Name = "cyf"
        };
    }
}
