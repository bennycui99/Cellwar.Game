using System.Collections.Generic;
using CellWar.Controller;
using CellWar.Controller.Gene;
using CellWar.Model.Substance;
using CellWar.View;

namespace CellWar.GameData {
    /// <summary>
    /// 游戏及时数据
    /// </summary>
    public static class MainGameCurrent {
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

        public static string GetCurrentBlockDetailInfo() {
            if( CellWar.GameData.MainGameCurrent.FocusedBlock == null ) {
                return "";
            }
            var currentHexBlock = CellWar.GameData.MainGameCurrent.FocusedBlock.HexBlockModel;
            string showText = "Condition: " + currentHexBlock.BlockType.ToString() + "\n\n";
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
            if( FocusedBlock == null ) {
                return "";
            }
            var currentHexBlock = FocusedBlock.HexBlockModel;
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
        public static U3D_BlockLogic FocusedBlock = null;

        public static RegulatoryGeneController RegCtor = new RegulatoryGeneController();
        public static CodingGeneController CodCtor = new CodingGeneController();

        public static class Camera {
            /// <summary>
            /// 摄像机的移动速度
            /// </summary>
            public static float MoveSpeed { get; set; } = 0.07f;

            public static float XMax { get; set; }
            public static float YMax { get; set; }
            public static float ZMax { get; set; }

            public static float XMin { get; set; }
            public static float YMin { get; set; }
            public static float ZMin { get; set; }
        }
    }
}
