///<summary>
/// 游戏中核心对象的Model实现
/// </summary>
using System.Collections.Generic;
using CellWar.Model.Substance;

/// <summary>
/// 存放地图相关的Model
/// </summary>
namespace CellWar.Model.Map {
    public class Map {
        public string Name { get; set; }
        public List<Block> Blocks { get; set; } = new List<Block>();
    }

    public class Block {
        public const string Tag = "HexBlock";

        /// <summary>
        /// Hex仿射坐标 X
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Hex仿射坐标 Y
        /// </summary>
        public int Y { get; set; }

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

        public bool IsPopulationFull() { return TotalPopulation > Capacity; }

        public int TotalPopulation;

        /// <summary>
        /// 统计所有菌类数量
        /// </summary>
        public int GetTotalPopulation()
        {
            int totalPopulation = 0;
            foreach (var s in Strains) { totalPopulation += s.Population; }
            return totalPopulation;
        }

        /// <summary>
        /// 检查这次添加是否已满
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool IsPopulationBeingFull(int delta) { return (TotalPopulation + delta) > Capacity; }

    }
}
