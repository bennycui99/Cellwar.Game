using CellWar.GameData;
using CellWar.Model.Map;
using CellWar.Model.Substance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellWar.Controller {
    class BlockController {
        private Block HexBlockModel;

        public BlockController( Block b ) {
            HexBlockModel = b;
        }
        /// <summary>
        /// Update关键函数
        /// </summary>
        public void BlockBacteriaUpdate() {
            UpdateStrainsAndRace();
            RemoveZeroCountStrainsAndChemicals();
        }

        /// <summary>
        /// 将数量或人口为0的细菌或chem移除列表
        /// </summary>
        private void RemoveZeroCountStrainsAndChemicals() {
            HexBlockModel.Strains.RemoveAll( s => s.Population <= 0 );
            HexBlockModel.PublicChemicals.RemoveAll( c => c.Count <= 0 );
        }

        private void UpdateStrainsAndRace() {
            // 遍历所有种类细菌
            for( var i = 0; i < HexBlockModel.Strains.Count; ++i ) {
                Strain currentStrain = HexBlockModel.Strains[i];
                // 遍历该菌的所有基因
                foreach( var reg in HexBlockModel.Strains[i].PlayerSelectedGenes ) {
                    // 检查基因条件
                    if( MainGameCurrent.RegCtor.IsTriggered( HexBlockModel.PublicChemicals, reg ) ) {
                        for( int j = 0; j < reg.DominatedGenes.Count; j++ ) {
                            var cod = reg.DominatedGenes[j];
                            // 细菌活动
                            MainGameCurrent.CodCtor.Effect( ref currentStrain, ref HexBlockModel, ref cod );
                        }
                    }

                }
                /// 使race生效
                foreach( var reg in HexBlockModel.Strains[i].BasicRace.RegulatoryGenes ) {
                    // 检查基因条件
                    if( MainGameCurrent.RegCtor.IsTriggered( HexBlockModel.PublicChemicals, reg ) ) {
                        for( int j = 0; j < reg.DominatedGenes.Count; j++ ) {
                            var cod = reg.DominatedGenes[j];
                            // 细菌活动
                            MainGameCurrent.CodCtor.Effect( ref currentStrain, ref HexBlockModel, ref cod );
                        }
                    }
                }
                //更新该菌种
                HexBlockModel.Strains[i] = currentStrain;
            }
        }
    }
}
