using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Substance;
using UnityEngine;
using static CellWar.Model.Substance.Strain;

namespace CellWar.GameData {
    /// <summary>
    /// 游戏及时数据
    /// </summary>
    public static class Current {
        /// <summary>
        /// 玩家手上是否拿着细菌准备防止重要数据
        /// 见 U3D_StrainPackageLogic.cs
        /// </summary>
        public static Strain HoldingStrain = null;

        /// <summary>
        /// 当前鼠标下的block的MonoBehavior实例
        /// </summary>
        public static U3D_BlockLogic FocusedBlock = null;
    }
    /// <summary>
    /// 游戏本地数据
    /// </summary>
    public static class Local {
        public static List<CodingGene> LoadAllCodingGenes() {
            Local.AllCodingGenes = CellWar.Utils.JsonHelper.Json2Object_NT<List<CodingGene>>( GetGameDataPath( "coding_gene.json" ) );
            return AllCodingGenes;
        }
        public static List<CodingGene> AllCodingGenes { get; set; }

        public static List<Chemical> LoadAllChemicals() {
            Local.AllChemicals = CellWar.Utils.JsonHelper.Json2Object_NT<List<Chemical>>( GetGameDataPath( "chemicals.json" ) );
            return AllChemicals;
        }

        public static Chemical FindChemicalByName( string chemicalName ) => AllChemicals.Find( c => { return c.Name == chemicalName; } );

        public static List<Chemical> AllChemicals { get; set; }

        public static string GetGameDataPath( string fileName ) => "Resources/GameData/" + fileName;
    }
}