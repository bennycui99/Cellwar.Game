using CellWar.Model.Substance;
using static CellWar.Model.Substance.Strain;

namespace CellWar.GameData {
    public static class LabCurrent {
        public static RegulatoryGene RegulatoryGene = null;
        public static Strain Strain = null;
        public static bool IsLengthOverflowed = false;

        public static void MakeStrainNotBeNull() {
            LabCurrent.Strain = Strain == null ? new Strain( GameData.Local.AllRaces[0] ) : Strain;
        }
    }
}
