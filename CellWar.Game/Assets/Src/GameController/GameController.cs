using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Substance;
using UnityEngine;

namespace CellWar.Model.Settings {
    /// <summary>
    /// 摄像机的所有相关设置
    /// </summary>
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