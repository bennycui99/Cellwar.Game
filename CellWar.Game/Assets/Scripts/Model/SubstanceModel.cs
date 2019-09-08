using System;
using System.Collections.Generic;
using CellWar.Utils.Object;

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
        public class Race {
            public string Name { get; set; }

            public string Description { get; set; }
            /// <summary>
            /// 基因长度上限
            /// </summary>
            public int MaxLength { get; set; }
            /// <summary>
            /// 该种族携带的基因组
            /// </summary>
            public List<RegulatoryGene> RegulatoryGenes { get; set; } = new List<RegulatoryGene>();

            public Race Clone() {
                return new Race {
                    Name = Name,
                    Description = Description,
                    MaxLength = MaxLength,
                    RegulatoryGenes = ObjectHelper.CloneList2<RegulatoryGene>( this.RegulatoryGenes )
                };
            }
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
            /// 被调控基因支配的基因
            /// </summary>
            public List<CodingGene> DominatedGenes { get; set; } = new List<CodingGene>();

            /// <summary>
            /// Reg类型
            /// </summary>
            public string Type { get; set; }
        }



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

            #region CONSUME

            public string ConsumeChemicalName { get; set; }
            public int ConsumeChemicalCount { get; set; }
            public float ConsumeChemicalCoeffeicient { get; set; }
            public int ConsumeChemicalIntercept { get; set; }
            public bool IsConsumePublic { get; set; }

            #endregion

            #region DECOMPOSITION

            public string DecompositionChemicalName { get; set; }
            public int DecompositionChemicalCount { get; set; }
            public float DecompositionChemicalCoeffeicient { get; set; }
            public int DecompositionChemicalIntercept { get; set; }
            public bool IsDecompositionPublic { get; set; }

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
        public Race BasicRace { get; set; }

        public System.Object Clone() {
            return new Strain() {
                Population = 0,
                Name = this.Name,
                PlayerSelectedGenes = ObjectHelper.CloneList2( PlayerSelectedGenes ),
                Owner = this.Owner,
                BasicRace = this.BasicRace
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

        public System.Object Clone()
        {
            return new Chemical()
            {
                Name = this.Name,
                Description = this.Description,
                Detail = this.Detail,
                Count = this.Count
            };
        }

        /// <summary>
        /// 保留功能
        /// </summary>
        public int SpreadRate { get; set; }
    }
}