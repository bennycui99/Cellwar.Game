using System.Linq;
using System.Collections.Generic;

/// <summary>
/// ��ŵ�ͼ��ص�Model
/// </summary>
namespace CellWar.Model.Map { 
    public class Block {
        public enum Type {
            Normal,
            Empty
        }
        public Type BlockType { get; set; }
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
    public class Skill
    {

    }
}

namespace CellWar.Model.Substance {
    /// <summary>
    /// ��
    /// Cell��
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
        //����ĳ��species,���ﲻ��д
        public List<Skill.Gene> Genes { get; set; }
        //public class storage,��������ʣ�Ӧ���ǰ�class gene����Ƶ�����
    }
    /// <summary>
    /// ��ѧ����
    /// </summary>
    public class Chemical { 
        public string Name { get; set; }
        public uint Count { get; set; }
        public uint SpreadRate { get; set; }
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
        public enum CodingType{//�ƺ�����effect class
            ChangeStrainProperty,//�ı�strain��ĳ������ֵ
            ChangeGeneProperty,
            ChangeSubstance//����block�ϵ�ĳ�����ʵ���
            
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