///<summary>
/// 游戏中核心对象的Model实现
/// </summary>
using System.Collections.Generic;
using CellWar.Model.Substance;
using System;
using CellWar.Utils.Object;

/// <summary>
/// 存放地图相关的Model
/// </summary>
namespace CellWar.Model.Map {
    public class Map {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public List<Block> Blocks { get; set; } = new List<Block>();
        public List<Chemical> PlayerOwnedChemicals { get; set; } = new List<Chemical>();
    }

    public class StandardCoordinate {
        const float RADIUS = 1.73205f;
        public float X { get; set; }
        public float Z { get; set; }

        /// <summary>
        /// 将unity坐标转化为hex坐标
        /// </summary> 
        /// <returns></returns>
        public HexCoordinate StandardToHexCoordiante() {
            HexCoordinate hex = new HexCoordinate();
            hex.Z = ( int )( Math.Round( Z / 1.5f ) );
            hex.X = ( int )( Math.Round( ( X - hex.Z * ( RADIUS / 2 ) ) / RADIUS ) );
            return hex;
        }

        public float StandardDistance( StandardCoordinate a, StandardCoordinate b ) {
            return ( a.X - b.X ) * ( a.X - b.X ) + ( a.Z - b.Z ) * ( a.Z - b.Z );
        }
    }

    /// <summary>
    /// X/Z 轴双轴六边形坐标
    /// </summary>
    public class HexCoordinate {
        const float RADIUS = 1.73205f;
        /// <summary>
        /// Hex仿射坐标 X
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Hex仿射坐标 Z
        /// </summary>
        public int Z { get; set; }

        public StandardCoordinate HexToStandardCoordiante() {
            StandardCoordinate standard = new StandardCoordinate();
            standard.Z = Z * 1.5f;
            standard.X = Z * ( RADIUS / 2f ) + X * RADIUS;
            return standard;
        }

        public int HexDistance( HexCoordinate a, HexCoordinate b ) {
            return ( a.X - b.X ) * ( a.X - b.X ) + ( a.Z - b.Z ) * ( a.Z - b.Z );
        }
    }

    public class HexBlock {
        public const string Tag = "HexBlock";

        public bool IsActive { get; set; } = false;

        public HexCoordinate HexCoor { get; set; } = new HexCoordinate();
        public StandardCoordinate StandardCoor { get; set; } = new StandardCoordinate();

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
        /// 公共资源库
        /// </summary>
        public List<Substance.Chemical> PublicChemicals { get; set; } = new List<Chemical>();

        public bool IsPopulationFull() { return TotalPopulation > Capacity; }

        public int TotalPopulation = 0;

        /// <summary>
        /// 检查这次添加是否已满
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool IsPopulationBeingFull( int delta ) { return ( TotalPopulation + delta ) > Capacity; }
    }

    public class Block : HexBlock {
        /// <summary>
        /// 各自内存在的所有细菌
        /// </summary>
        public List<Substance.Strain> Strains { get; set; } = new List<Strain>();

        /// <summary>
        /// 统计所有菌类数量
        /// </summary>
        public int GetTotalPopulation() {
            int totalPopulation = 0;
            foreach( var s in Strains ) { totalPopulation += s.Population; }
            return totalPopulation;
        }
    }
}
