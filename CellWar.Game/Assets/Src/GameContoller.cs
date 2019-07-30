using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellWar.Model.Substance;
using CellWar.Utils.Object;
using static CellWar.Model.Substance.Strain;

namespace CellWar.Contoller {
    public class StrainContoller {
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


    }
}
