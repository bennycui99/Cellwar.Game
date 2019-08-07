using System.Collections.Generic;
using CellWar.Model.Substance;
using CellWar.Utils.Object;
using static CellWar.Model.Substance.Strain;

namespace CellWar.Controller {
    public class StrainController {
        public int GetTotalLength( List<RegulatoryGene> regs ) {
            int total = 0;
            regs.ForEach( a => { total += a.Length; a.DominatedGenes.ForEach( b => total += b.Length ); } );
            return total;
        }

        public List<RegulatoryGene> CloneRegGeneList( List<RegulatoryGene> regs ) {
            List<RegulatoryGene> newRegs = new List<RegulatoryGene>();
            foreach( var reg in regs ) {
                newRegs.Add( ObjectHelper.Clone( reg, reg.GetType() ) );
            }
            return newRegs;
        }

        public bool IsValid( Strain strain ) {
            int totalLength = 0;
            strain.PlayerSelectedGenes.ForEach( gene => { totalLength += gene.Length; } );
            return totalLength < strain.BasicRace.MaxLength;
        }
    }
}
