///<summary>
/// 游戏中核心对象的Model实现
/// </summary>
using System.Collections.Generic;
using CellWar.Model.Substance;
using System;
using UnityEngine;
using CellWar.GameData;
using CellWar.Utils.Object;
using CellWar.Controller;

/// <summary>
/// 存放地图相关的Model
/// </summary>
namespace CellWar.Model.Map {
    public class Map {

    }

    public class Block {
        public const string Tag = "HexBlock";

        /// <summary>
        /// Unity 游戏对象名字
        /// </summary>
        public string ParentUnityObjectName;

        public enum Type {
            Normal,
            Unaccessable
        }

        public Type BlockType { get; set; }
        /// <summary>
        /// 人口上限
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// 相邻的格子集合
        /// </summary>
        public List<Block> NeighborBlocks { get; set; } = new List<Block>();

        /// <summary>
        /// 各自内存在的所有细菌
        /// </summary>
        public List<Substance.Strain> Strains { get; set; } = new List<Strain>();

        /// <summary>
        /// 公共资源库
        /// </summary>
        public List<Substance.Chemical> PublicChemicals { get; set; } = new List<Chemical>();
    }
}
