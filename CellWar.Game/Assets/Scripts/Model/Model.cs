///<summary>
/// 游戏中核心对象的Model实现
/// </summary>
using System.Collections.Generic;
using CellWar.Model.Substance;
using System;
using UnityEngine;
using CellWar.GameData;

/// <summary>
/// 存放地图相关的Model
/// </summary>
namespace CellWar.Model.Map {

    public class Map {
        /// <summary>
        /// 地图中所有的格子
        /// 应该在地图加载时给该列表赋值
        /// </summary>
        public List<Block> Blocks { get; set; } = new List<Block>();

        /// <summary>
        /// 于MapLogic Awake中调用
        /// 若置于Start中会导致block列表为空的问题
        /// </summary>
        /// <param name="mapTransform"></param>
        public void LoadAllBlockUnityObjectFromTransform( Transform mapTransform ) {
            foreach( Transform blockTransform in mapTransform ) {
                Blocks.Add( new Block { UnityObject = blockTransform.gameObject } );
            }
            Debug.Log( Blocks.Count );
        }

        public Block FindBlockFromGameObjectName( string gameObjectName ) {
            return Blocks.Find( b => b.UnityObject.gameObject.name == gameObjectName );
        }
    }

    public class Block {

        public const string Tag = "HexBlock";

        /// <summary>
        /// Unity 游戏对象
        /// </summary>
        public GameObject UnityObject;

        public enum Type {
            Normal,
            Empty
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

        /// <summary>
        /// 获取总人口数
        /// </summary>
        /// <returns></returns>
        public int GetTotalPopulation() {
            int totalPopulation = 0;
            foreach( var s in Strains ) { totalPopulation += s.Population; }
            return totalPopulation;
        }

        /// <summary>
        /// 通过碰撞器获取相邻的格子
        /// </summary>
        /// <param name="other">OnTriggerEnter参数</param>
        /// <param name="map">地图实例，用于查询Block实例</param>
        public void FetchNeighborBlocksFromMap_OnTriggerEnter( Collider other, Map map ) {
            if( other.gameObject.tag == Block.Tag
                && other.gameObject.name != UnityObject.gameObject.name
                && !NeighborBlocks.Exists( hb => { return hb.UnityObject.name == other.gameObject.name; } ) ) {
                NeighborBlocks.Add( map.FindBlockFromGameObjectName( other.gameObject.name ) );
                Debug.Log( other.name + " Added " + "to " + UnityObject.gameObject.name );
            }
        }
    }
}

namespace CellWar.Model.Json {
    public class RegulartoryGeneJsonModel {
        public string Name { get; set; }
        public List<string> ConditionChemicalNames { get; set; } = new List<string>();
    }
}

/// <summary>
/// 物质相关
/// </summary>
namespace CellWar.Model.Substance {
    /// <summary>
    /// 菌系
    /// </summary>
    public class Strain : ICloneable {
        /// <summary>
        /// 种族
        /// 游戏初始的，某种自带的基因组。与DOTA中基础英雄属性增长分为三类相类似。
        /// </summary>
        public class Species {
            public string Name { get; set; }
            /// <summary>
            /// 该种族携带的基因组
            /// </summary>
            public List<RegulatoryGene> Genes { get; set; } = new List<RegulatoryGene>();
        }

        public class Gene {
            public string Name { get; set; }
            public int Length { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// 调控基因
        /// 不做任何事情，只负责编辑基因是否工作。
        /// 触发的条件是Condition有关
        /// 调控基因 支配 编码基因
        /// </summary>
        public class RegulatoryGene : Gene {
            /// <summary>
            /// 触发条件
            /// 满足所有物质存在且数量达标，才可以触发效果。
            /// </summary>
            public List<Substance.Chemical> Conditions { get; set; } = new List<Chemical>();

            /// <summary>
            /// 判断格子中的chemicals是否是conditions的父集，且满足一定条件：格子中chemical的Count不小于condition中的Count
            /// </summary>
            /// <param name="chemicalsInBlock"></param>
            /// <returns></returns>
            public virtual bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) { return default; }

            /// <summary>
            /// 调控基因支配的编码基因
            /// </summary>
            public List<CodingGene> CodingGenes { get; set; } = new List<CodingGene>();
            #region PRIVATE
            /// <summary>
            /// 判断是否满足所有条件
            /// </summary>
            /// <param name="chemicalsInBlock"></param>
            /// <returns></returns>
            protected bool isMeetAllCondition( List<Substance.Chemical> chemicalsInBlock ) {
                foreach( var cInCondition in Conditions ) {
                    // 寻找格子中是否含有条件的chemical
                    var result = chemicalsInBlock.Find( r => { return r.Name == cInCondition.Name; } );
                    // 如果不存在直接条件直接不成立
                    if( result == null ) {
                        return false;
                    }
                    // 如果存在，判断数量是否达标，如不达标直接不成立
                    if( result.Count < cInCondition.Count ) {
                        return false;
                    }
                }
                return true;
            }

            /// <summary>
            /// 仅有一个条件就触发
            /// </summary>
            /// <param name="chemicalsInBlock"></param>
            /// <returns></returns>
            protected bool isMeetAtLeastOneCondition( List<Substance.Chemical> chemicalsInBlock ) {
                foreach( var cInCondition in Conditions ) {
                    // 寻找格子中是否含有条件的chemical
                    var result = chemicalsInBlock.Find( r => { return r.Name == cInCondition.Name; } );
                    // 如果不存在直接条件直接不成立
                    if( result == null ) {
                        return false;
                    }
                    // 如果存在，判断数量是否达标，如不达标直接不成立
                    if( result.Count < cInCondition.Count ) {
                        return false;
                    }
                    return true;
                }
                return false; // 不应该到达这里
            }
            #endregion
        }

        #region MEANINGFUL_REGULARTOYGENES
        /// <summary>
        /// 正全调控基因
        /// 必须满足所有条件方可触发条件
        /// </summary>
        public class PositiveAllRegulatoryGene : RegulatoryGene {
            public override bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) {
                return isMeetAllCondition( chemicalsInBlock );
            }
        }
        /// <summary>
        /// 负全调控基因
        /// 当满足所有条件是关闭条件触发
        /// </summary>
        public class NegativeAllRegulartoryGene : RegulatoryGene {
            public override bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) {
                return !isMeetAllCondition( chemicalsInBlock );
            }
        }

        /// <summary>
        /// 正或调控基因
        /// </summary>
        public class PositiveOrRegulartoryGene : RegulatoryGene {
            public override bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) {
                return isMeetAtLeastOneCondition( chemicalsInBlock );
            }
        }

        /// <summary>
        /// 负或调控基因
        /// </summary>
        public class NegativeOrRegulartoryGene : RegulatoryGene {
            public override bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) {
                return isMeetAllCondition( chemicalsInBlock );
            }
        }
        #endregion

        public class CodingGene : Gene {
            #region PRODUCTION_CHEMICAL
            /// <summary>
            /// 改变的化学物质的名字
            /// </summary>
            public string ProductionChemicalName { get; set; }
            /// <summary>
            /// 改变化学物质的量
            /// Count可正可负
            /// </summary>
            public int ProductionChemicalCount { get; set; }
            public float ProductionChemicalCoeffeicient { get; set; }
            public int ProductionChemicalIntercept { get; set; }
            #endregion

            #region IMPORT_CHEMICAL
            /// <summary>
            /// 从外部拿来的化学物质的名字
            /// </summary>
            /// <seealso cref="CellWar.Model.Map.Block.PublicChemicals"/>
            public string ImportChemicalName { get; set; }
            /// <summary>
            /// 拿来化学物质的量
            /// Count可正可负
            /// </summary>
            /// <seealso cref="CellWar.Model.Map.Block.PublicChemicals"/>
            public int ImportChemicalCount { get; set; }
            public float ImportChemicalCoeffeicient { get; set; }
            public int ImportChemicalIntercept { get; set; }
            #endregion

            #region POPULATION
            /// <summary>
            /// 人口前的系数 
            /// </summary>
            public float PopulationCoefficient { get; set; }
            /// <summary>
            /// 人口截距
            /// </summary>
            public int PopulationIntercept { get; set; }
            #endregion

            /// <summary>
            /// 首次传播时的百分比
            /// 例如：当人口数达到格子人口上限的50%时对周围格子开始传播，传播量为30%
            /// 则FistSpreadMountRate为0.3
            /// </summary>
            public float FirstSpreadMountRate { get; set; }

            /// <summary>
            /// 首次传播达到人口上线的百分比
            /// 例如：当人口数达到格子人口上限的50%时对周围格子开始传播，传播量为30%
            /// 则SpreadConditionRate为0.5
            /// </summary>
            public float SpreadConditionRate { get; set; }

            /// <summary>
            /// 人口*系数 的值影响物质改变量的大小
            /// 7.18 改动?
            /// </summary>
            /// <param name="parentStrain"></param>
            /// <returns></returns>
            private float GetPopulationDelta( ref Strain parentStrain ) => ( parentStrain.Population * PopulationCoefficient ) + PopulationIntercept;
            private float GetProductionChemicalDelta( ref Strain parentStrain ) => ( parentStrain.Population * ProductionChemicalCoeffeicient ) + ProductionChemicalIntercept;
            private float GetImportChemicalDelta( ref Strain parentStrain ) => ( parentStrain.Population * ImportChemicalCoeffeicient ) + ImportChemicalIntercept;

            /// <summary>
            /// 游戏核心函数
            /// </summary>
            /// <param name="parentStrain">该基因的父细菌</param>
            /// <param name="currentBlock">父细菌所在的block</param>
            public void Effect( ref Strain parentStrain, ref Map.Block currentBlock ) {
                var productionChemical = Local.FindChemicalByName( ProductionChemicalName );
                // ----- 对化学物质产生影响 -----
                // 查找是否存在这个物质
                var productChem = currentBlock.PublicChemicals.Find( che => { return che.Name == ProductionChemicalName; } );
                if( productChem == null ) {
                    productChem = new Chemical {
                        Name = ProductionChemicalName,
                        Count = 0,
                        SpreadRate = productionChemical.SpreadRate
                    };
                    // 向block物质集中添加改变的chemical
                    currentBlock.PublicChemicals.Add( productChem );
                }
productChem.Count += ( int )GetProductionChemicalDelta( ref parentStrain );
                // ----- 对化学 物质产生影响 -----

                // ----- 对父strain产生影响 -----
                // --- 添加人口 ---
parentStrain.Population += ( int )GetPopulationDelta( ref parentStrain );

                // --- 添加私有化学库的量 ---
                // 先寻找block内是否存在该种化学物质
                var importChemical = Local.FindChemicalByName( ImportChemicalName );
                var publicChemical = currentBlock.PublicChemicals.Find( chem => { return chem.Name == ImportChemicalName; } );
                if( publicChemical != null ) {
                    var privateChemical = parentStrain.PrivateChemicals.Find( chem => { return chem.Name == publicChemical.Name; } );
                    if( privateChemical == null ) {
                        parentStrain.PrivateChemicals.Add( new Chemical {
                            Count = 0,
                            Name = ImportChemicalName,
                            SpreadRate = importChemical.SpreadRate
                        } ); // 如果没有，先添加
                    }
                    var importCount = ( int )GetPopulationDelta( ref parentStrain ) + ProductionChemicalCount;
                    if( publicChemical.Count >= privateChemical.Count ) {
privateChemical.Count += ( int )GetImportChemicalDelta( ref parentStrain );
publicChemical.Count -= ( int )GetImportChemicalDelta( ref parentStrain );
                    }
                }
                // ----- 对父strain产生影响 -----

                // ----- 细菌扩散 -----
                // 是否满足扩散条件
                if( parentStrain.Population * SpreadConditionRate >= currentBlock.Capacity ) {
                    var cloneStrain = ( Strain )parentStrain.Clone();
// 设定初始人口数
cloneStrain.Population = ( int )( parentStrain.Population * FirstSpreadMountRate );
                    // 为周围的格子添加该细菌
                    foreach( var block in currentBlock.NeighborBlocks ) {
                        block.Strains.Add( cloneStrain );
                    }
                }
                // ----- 细菌扩散 -----
            }
        }

        public Gamer.Player Owner { get; set; } = new Gamer.Player();
        public string Name { get; set; }
        public int Population { get; set; }

        /// <summary>
        /// 玩家选择的自带的或是库里默认存在的基因
        /// </summary>
        public List<RegulatoryGene> PlayerSelectedGenes { get; set; } = new List<RegulatoryGene>();

        /// <summary>
        /// 夺取的化学物质
        /// 从 PublicChemicals
        /// </summary>
        /// <seealso cref="CellWar.Model.Map.Block.PublicChemicals"/>
        public List<Substance.Chemical> PrivateChemicals { get; set; } = new List<Chemical>();

        /// <summary>
        /// 种族自带的基因
        /// </summary>
        public Species BasicSpecies { get; set; }

        public System.Object Clone() {
            return new Strain() {
                Population = 0,
                Name = this.Name,
                PlayerSelectedGenes = this.PlayerSelectedGenes,
                Owner = this.Owner,
                BasicSpecies = this.BasicSpecies
            };
        }
    }


    /// <summary>
    /// 化学物质
    /// </summary>
    public class Chemical {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public int Count { get; set; }
        /// <summary>
        /// 保留功能
        /// </summary>
        public int SpreadRate { get; set; }
    }
}