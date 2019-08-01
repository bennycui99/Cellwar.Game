using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellWar.Model.Map;
using CellWar.Model.Substance;
using CellWar.Utils.Object;
using UnityEngine;
using static CellWar.Model.Substance.Strain;
using static CellWar.Utils.LambdaHelper;

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

    public class BlockController {
        public bool IsPopulationFull( Block block ) { return block.GetTotalPopulation() > block.Capacity; }
    }

    public class MainGameController {
        /// <summary>
        /// 按每个间隔更新一次
        /// </summary>
        /// <param name="eventUpdateInInterval"></param>
        /// <see cref="GameData.MainGameCurrent.GeneEffectInterval"/>
        public void UpdateByInterval( BaseEvent eventUpdateInInterval, ref float sec ) {
            if( sec >= GameData.MainGameCurrent.GeneEffectInterval ) {
                eventUpdateInInterval();
                sec = 0;
            }
            sec += Time.deltaTime;
        }
        public Strain StrainWork( Strain strain, ref Block block ) {
            BlockController blockController = new BlockController();

            if( blockController.IsPopulationFull( block) ) {
                return strain;
            }
            foreach( var reg in strain.PlayerSelectedGenes ) {
                Debug.Log( reg.GetType().ToString() );
                if( reg.IsTriggered( block.PublicChemicals ) ) {
                    Debug.Log( "Triggered" );
                    foreach( var cod in reg.DominatedGenes ) {
                        Debug.Log( "Effect" );
                        cod.Effect( ref strain, ref block );
                    }
                }
            }
            return strain;
        }
    }
}
