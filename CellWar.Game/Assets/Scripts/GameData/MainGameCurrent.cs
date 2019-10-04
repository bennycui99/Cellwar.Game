using System.Collections.Generic;
using CellWar.Controller;
using CellWar.Controller.Gene;
using CellWar.Model.Substance;
using CellWar.View;
using CellWar.Model.Map;
using CellWar.Utils;
using CellWar.Model.Json;

namespace CellWar.GameData {
    /// <summary>
    /// 游戏及时数据
    /// </summary>
    public static class MainGameCurrent {

        /// <summary>
        /// 地图的Instance
        /// </summary>
        public static Map StageMap = null;

        public static void LoadMap(string name) {
            var mapJson = JsonHelper.Json2Object_NT<MapJsonModel>(Local.GetGameDataPath(name));

            LoadMap( mapJson );
        }

        public static void LoadMap() {
            LoadMap("map.json");
        }

        public static void LoadMap( MapJsonModel model ) {
            StageMap = new MapController().JsonModel2Map( model );
        }

        /// <summary>
        /// Editor用的 手上的chemical
        /// </summary>
        public static Chemical HoldingChemical = null;

        /// <summary>
        /// 玩家手上是否拿着细菌准备防止重要数据
        /// 见 U3D_StrainPackageLogic.cs
        /// </summary>
        public static Strain HoldingStrain = null;

        /// <summary>
        /// 细菌的所有基因作用的间隔
        /// </summary>
        public static float GeneEffectInterval = 1f;
        public static float BlockColorInterval = 0.3f;
        public static float GetBlockColorChangeFps() {
            return 1 / BlockColorInterval;
        }
        public static List<Strain> StrainList = new List<Strain>();

        public static List<Strain> EditorNpcStrainList = new List<Strain>();

        public static string GetCurrentBlockDetailInfo() {
            if( CellWar.GameData.MainGameCurrent.FocusedHexBlock == null ) {
                return "";
            }
            var currentHexBlock = CellWar.GameData.MainGameCurrent.FocusedHexBlock;
            //string showText = "Condition: " + currentHexBlock.BlockType.ToString() + "\n\n";
            string showText = "Capacity: " + currentHexBlock.Capacity + "\n\n";
            showText += "Chemicals: \n";
            if( currentHexBlock.PublicChemicals.Count == 0 ) {
                showText += "Nothing so far.";
            }
            foreach( var chem in currentHexBlock.PublicChemicals ) {
                showText += string.Format( "{0}: {1}\n", chem.Name, chem.Count.ToString() );
            }
            showText += "\n\nStrains: \n";
            if( currentHexBlock.Strains.Count == 0 ) {
                showText += "Nothing so far.";
            }
            foreach( var str in currentHexBlock.Strains ) {
                showText += string.Format( "{0}: {1}\n", str.Name, str.Population.ToString() );
            }
            return showText;
        }

        public static string GetCurrentBlockStrainDetailInfo() {
            if( FocusedHexBlock == null ) {
                return "";
            }
            var currentHexBlock = FocusedHexBlock;
            string showText = "";
            foreach( var str in currentHexBlock.Strains ) {
                showText += str.Name + "\n";
                foreach( var gene in str.PlayerSelectedGenes ) {
                    showText += gene.Name + "\t";
                }
                showText += "\n";
                // showText += str.ConditionGene.IsTriggered( currentHexBlock.PublicChemicals ) ? "Working" : "Sleeping";
            }
            return showText;
        }
        /// <summary>
        /// 当前鼠标下的block的MonoBehavior实例
        /// </summary>
        public static CellWar.Model.Map.Block FocusedHexBlock = null;

        public static RegulatoryGeneController RegCtor = new RegulatoryGeneController();
        public static CodingGeneController CodCtor = new CodingGeneController();
    }
}
