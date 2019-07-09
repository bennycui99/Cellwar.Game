using System.Linq;
using System.Collections.Generic;

/// <summary>
/// ��ŵ�ͼ��ص�Model
/// </summary>
namespace CellWar.Model.Map { 
    public class Block {
        public enum Type {
            Common,
            Disabled
        }
        public Type Type { get; set; }
        /// <summary>
        /// �˿�����
        /// </summary>
        public int Capacity { get; set; }

        public List<Substance.Strain> Strains { get; set; }
        public List<Substance.Chemical> Chemicals { get; set; }

        /// <summary>
        /// ��ȡ���˿���
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
    
}

namespace CellWar.Model.Substance {
    /// <summary>
    /// ��
    /// Cell��
    /// </summary>
    public class Strain {
        public Gamer.Player Owner { get; set; }
        public string Name { get; set; }
        public uint Population { get; set; }
        public List<Skill.Gene> Genes { get; set; }
    }
    /// <summary>
    /// ��ѧ����
    /// </summary>
    public class Chemical { 
        public string Name { get; set; }
        public uint Count { get; set; }
    }
}

namespace CellWar.Model.Skill {
    /// <summary>
    /// ����
    /// </summary>
    public class Gene {
        public enum Type {
            Coding,     // �༭����
            Regulatory  // ���ػ���
        }

        public string Name { get; set; }
        public Gene.Type Type { get; set; }
        public uint Length { get; set; }

        public Effect Effect { get; set; }
        public List<Substance.Chemical> Condition { get; set; }
        public List<Resource> CostList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Resource {
        public enum Type {

        }
        public Resource.Type Type { get; set; }
        public uint Count { get; set; }
    }

    public class Effect {
        public string Description { get; set; }
        public virtual void Do();
    }
}