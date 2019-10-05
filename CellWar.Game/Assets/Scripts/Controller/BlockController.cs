using CellWar.GameData;
using CellWar.Model.Json;
using CellWar.Model.Map;
using CellWar.Model.Substance;
using CellWar.Utils;
using CellWar.Utils.Object;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CellWar.Controller {
    class MapController {
        public Map JsonModel2Map( MapJsonModel mapJson ) {
            Map StageMap = new Map();
            StageMap.Name = mapJson.Name;
            StageMap.Description = mapJson.Description;
            StageMap.GameOverCondition = mapJson.GameOverCondition;
            StageMap.Blocks = new List<Block>();
            // 填充blocks的strains数据
            foreach( var b in mapJson.Blocks ) {
                var bb = b as Block;
                bb.Strains = new StrainController().Text2Strains_Npc( b.StrainsInfo );
                bb.PublicChemicals = SemanticObjectController.GenerateText2ChemicalsWithCountInfo( b.ChemicalNamesInfo );
                StageMap.Blocks.Add( b as Block );
            }
            StageMap.PlayerOwnedChemicals = SemanticObjectController.GenerateText2ChemicalsWithCountInfo( mapJson.PlayerOwnedChemicalsDescription );
            return StageMap;
        }
        public MapJsonModel Map2JsonModel( Map map ) {
            MapJsonModel mjson = new MapJsonModel();
            mjson.GameOverCondition = map.GameOverCondition;
            mjson.Name = map.Name;
            mjson.Description = map.Description;
            mjson.PlayerOwnedChemicalsDescription = SemanticObjectController.GenerateChemicals2Text( map.PlayerOwnedChemicals );
            foreach( var b in map.Blocks.FindAll( m => m.IsActive ) ) {
                BlockJsonModel bbj = new BlockJsonModel();
                bbj.StrainsInfo = b.Strains == null ? "" : new StrainController().Strains2Text( b.Strains );
                // b.Strains = null;
                bbj.ChemicalNamesInfo = SemanticObjectController.GenerateChemicals2Text( b.PublicChemicals );
                // b.PublicChemicals = null;

                bbj.BlockType = b.BlockType;
                bbj.Capacity = b.Capacity;
                bbj.HexCoor = ObjectHelper.Clone( b.HexCoor );
                bbj.IsActive = b.IsActive;
                bbj.StandardCoor = ObjectHelper.Clone( b.StandardCoor );
                bbj.TotalPopulation = b.TotalPopulation;
                mjson.Blocks.Add( bbj );
            }
            return mjson;
        }
        public void SaveMapToLocal() {
            SaveMapToLocal( "map_generate.json" );
        }
        public void SaveMapToLocal( string fileName ) {
            // ?有待商榷
            // map.Blocks.AddRange( MainGameCurrent.StageMap.Blocks.FindAll( block => block.IsActive ) );

            File.WriteAllText( Save.GetGameSavePath( fileName ), JsonHelper.Object2Json( Map2JsonModel( MainGameCurrent.StageMap ) ) );
        }
    }

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
