using System;
using CellWar.GameData;
using CellWar.Model.Map;
using CellWar.Model.Substance;
using UnityEngine;
using static CellWar.Utils.LambdaHelper;

namespace CellWar.Controller {
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
        public void UpdateBlockColorByInterval( BaseEvent eventUpdateInInterval, ref float sec ) {
            if( sec >= GameData.MainGameCurrent.BlockColorInterval ) {
                eventUpdateInInterval();
                sec = 0;
            }
            sec += Time.deltaTime;
        }
        public Strain StrainWork( Strain strain, ref Block block ) {
            BlockController blockController = new BlockController();

            if( blockController.IsPopulationFull( block ) ) {
                return strain;
            }
            foreach( var reg in strain.PlayerSelectedGenes ) {
                if( MainGameCurrent.RegCtor.IsTriggered( block.PublicChemicals, reg ) ) {
                    for( Int32 i = 0; i < reg.DominatedGenes.Count; i++ ) {
                        var cod = reg.DominatedGenes[i];
                        Debug.Log( "Effect" );
                        MainGameCurrent.CodCtor.Effect( ref strain, ref block, ref cod );
                    }
                }
            }
            return strain;
        }

    }
}
