using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Substance;
using UnityEngine;

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

        public static U3D_BlockLogic FocusedBlock = null;
    }
}