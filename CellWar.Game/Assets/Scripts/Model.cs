using System.Linq;
using System.Collections.Generic;

/// <summary>
/// 存放地图相关的Model
/// </summary>
namespace CellWar.Model.Map { 
    public class Block {
        public enum Type {
            Normal,
            Empty
        }
        public Type BlockType { get; set; }
        /// <summary>
        /// 人口上限
        /// </summary>
        public int Capacity { get; set; }

        public List<Substance.Strain> Strains { get; set; }
        public List<Substance.Chemical> Chemicals { get; set; }

        /// <summary>
        /// 获取总人口数
        /// </summary>
        /// <returns></returns>
        public uint GetTotalPopulation() {
            uint totalPopulation = 0;
            foreach( var s in Strains ) { totalPopulation += s.Population; }
            return totalPopulation;
        }
    }
}

namespace CellWar.Model.Gamer { 
    
    public class Player {
        public string Name { get; set; }

    }
    public class Skill
    {

    }
}

namespace CellWar.Model.Substance {
    /// <summary>
    /// 菌
    /// Cell？
    /// </summary>
    public class Species {
        public string BasicName { get; set; }
        public uint BasicGrowthRate { get; set; }
        public uint BasicSpreadRate { get; set; }
        public List<Skill.Gene> BasicGene { get; set; }
    }

    public class Strain {
        public Gamer.Player Owner { get; set; }
        public string Name { get; set; }
        public uint Population { get; set; }
        //所属某个species,这里不会写
        public List<Skill.Gene> Genes { get; set; }
        //public class storage,储存的物质，应该是把class gene里的移到这里
    }
    /// <summary>
    /// 化学物质
    /// </summary>
    public class Chemical { 
        public string Name { get; set; }
        public uint Count { get; set; }
        public uint SpreadRate { get; set; }
    }
}

namespace CellWar.Model.Skill {
    /// <summary>
    /// 基因
    /// </summary>
    public class Gene {
        public enum Type { 
            Coding,     // 编辑基因
            Regulatory  // 调控基因
        }
        public enum CodingType{//似乎属于effect class
            ChangeStrainProperty,//改变strain的某个属性值
            ChangeGeneProperty,
            ChangeSubstance//减少block上的某个物质的量
            
        }
        public string Name { get; set; }
        public Gene.Type Type { get; set; }
        public uint Length { get; set; }

        public Effect Effect { get; set; }
        public List<Substance.Chemical> Condition { get; set; }
        public List<Storage> CostList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Storage {
        public enum Type {

        }
        public Storage.Type StorageType { get; set; }
        public uint Count { get; set; }
    }

    public class Effect {
        public string Description { get; set; }
        public virtual void Do();
    }
}