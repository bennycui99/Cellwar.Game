using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Gamer;
using CellWar.Model.Substance;

namespace CellWar.Mock {
    public class Mocks {
        public static Strain MockStrainForStrainUI { get; set; } = new Strain {
            Name = "Strain1",
            Owner = MockPlayerCyf,
            Population = 100,
        };
        public static Player MockPlayerCyf { get; set; } = new Player {
            Name = "cyf"
        };
    }
}
