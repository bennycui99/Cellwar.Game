using System.Linq;
using System.Collections.Generic;
using CellWar.Model.Substance;
using System;

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

        /// <summary>
        /// �����ڴ��ڵ�����ϸ��
        /// </summary>
        public List<Substance.Strain> Strains { get; set; } = new List<Strain>();

        /// <summary>
        /// ������Դ��
        /// </summary>
        public List<Substance.Chemical> PublicChemicals { get; set; } = new List<Chemical>();

        /// <summary>
        /// ��ȡ���˿���
        /// </summary>
        /// <returns></returns>
        public int GetTotalPopulation() {
            int totalPopulation = 0;
            foreach( var s in Strains ) { totalPopulation += s.Population; }
            return totalPopulation;
        }
    }
}

/// <summary>
/// ������Model
/// </summary>
namespace CellWar.Model.Gamer {
    public class Player {
        public string Name { get; set; }
    }
}

/// <summary>
/// �������
/// </summary>
namespace CellWar.Model.Substance {
    /// <summary>
    /// ��ϵ
    /// </summary>
    public class Strain : ICloneable {
        /// <summary>
        /// ����
        /// ��Ϸ��ʼ�ģ�ĳ���Դ��Ļ����顣��DOTA�л���Ӣ������������Ϊ���������ơ�
        /// </summary>
        public class Species {
            public string Name { get; set; }
            /// <summary>
            /// ������Я���Ļ�����
            /// </summary>
            public List<RegulatoryGene> Genes { get; set; } = new List<RegulatoryGene>();
        }

        /// <summary>
        /// ���ػ���
        /// �����κ����飬ֻ����༭�����Ƿ�����
        /// ������������Condition�й�
        /// ���ػ��� ֧�� �������
        /// </summary>
        public class RegulatoryGene {
            public string Name { get; set; }
            public int Length { get; set; }
            /// <summary>
            /// ��������
            /// �����������ʴ�����������꣬�ſ��Դ���Ч����
            /// </summary>
            public List<Substance.Chemical> Conditions { get; set; } = new List<Chemical>();
            /// <summary>
            /// �жϸ����е�chemicals�Ƿ���conditions�ĸ�����������һ��������������chemical��Count��С��condition�е�Count
            /// </summary>
            /// <param name="chemicalsInBlock"></param>
            /// <returns></returns>
            public virtual bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) { return default; }

            /// <summary>
            /// ���ػ���֧��ı������
            /// </summary>
            public List<CodingGene> CodingGenes { get; set; } = new List<CodingGene>();
            #region PRIVATE
            /// <summary>
            /// �ж��Ƿ�������������
            /// </summary>
            /// <param name="chemicalsInBlock"></param>
            /// <returns></returns>
            protected bool isMeetAllCondition( List<Substance.Chemical> chemicalsInBlock ) {
                foreach( var cInCondition in Conditions ) {
                    // Ѱ�Ҹ������Ƿ���������chemical
                    var result = chemicalsInBlock.Find( r => { return r.Name == cInCondition.Name; } );
                    // ���������ֱ������ֱ�Ӳ�����
                    if( result == null ) {
                        return false;
                    }
                    // ������ڣ��ж������Ƿ��꣬�粻���ֱ�Ӳ�����
                    if( result.Count < cInCondition.Count ) {
                        return false;
                    }
                }
                return true;
            }

            /// <summary>
            /// ����һ�������ʹ���
            /// </summary>
            /// <param name="chemicalsInBlock"></param>
            /// <returns></returns>
            protected bool isMeetAtLeastOneCondition( List<Substance.Chemical> chemicalsInBlock ) {
                foreach( var cInCondition in Conditions ) {
                    // Ѱ�Ҹ������Ƿ���������chemical
                    var result = chemicalsInBlock.Find( r => { return r.Name == cInCondition.Name; } );
                    // ���������ֱ������ֱ�Ӳ�����
                    if( result == null ) {
                        return false;
                    }
                    // ������ڣ��ж������Ƿ��꣬�粻���ֱ�Ӳ�����
                    if( result.Count < cInCondition.Count ) {
                        return false;
                    }
                    return true;
                }
                return false; // ��Ӧ�õ�������
            }
            #endregion
        }

        #region MEANINGFUL_REGULARTOYGENES
        /// <summary>
        /// ��ȫ���ػ���
        /// �������������������ɴ�������
        /// </summary>
        public class PositiveAllRegulatoryGene : RegulatoryGene {
            public override bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) {
                return isMeetAllCondition( chemicalsInBlock );
            }
        }
        /// <summary>
        /// ��ȫ���ػ���
        /// ���������������ǹر���������
        /// </summary>
        public class NegativeAllRegulartoryGene : RegulatoryGene {
            public override bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) {
                return !isMeetAllCondition( chemicalsInBlock );
            }
        }

        /// <summary>
        /// ������ػ���
        /// </summary>
        public class PositiveOrRegulartoryGene : RegulatoryGene {
            public override bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) {
                return isMeetAtLeastOneCondition( chemicalsInBlock );
            }
        }

        /// <summary>
        /// ������ػ���
        /// </summary>
        public class NegativeOrRegulartoryGene : RegulatoryGene {
            public override bool IsTriggered( List<Substance.Chemical> chemicalsInBlock ) {
                return isMeetAllCondition( chemicalsInBlock );
            }
        }
        #endregion

        public class CodingGene {
            public string Name { get; set; }
            public int Length { get; set; }

            /// <summary>
            /// �˿�ǰ��ϵ�� 
            /// </summary>
            public int PopulationCoefficient { get; set; }

            /// <summary>
            /// �ı�Ļ�ѧ����
            /// Count�����ɸ�
            /// </summary>
            public Chemical ProductionChemicalInfo { get; set; }

            /// <summary>
            /// ���ⲿ��������Դ
            /// Count һ��Ϊ��
            /// </summary>
            /// <seealso cref="CellWar.Model.Map.Block.PublicChemicals"/>
            public Chemical ImportChemicalInfo { get; set; }

            /// <summary>
            /// �ؾ�
            /// </summary>
            public int Intercept { get; set; }

            /// <summary>
            /// �״δ���ʱ�İٷֱ�
            /// ���磺���˿����ﵽ�����˿����޵�50%ʱ����Χ���ӿ�ʼ������������Ϊ30%
            /// ��FistSpreadMountRateΪ0.3
            /// </summary>
            public float FirstSpreadMountRate { get; set; }

            /// <summary>
            /// �״δ����ﵽ�˿����ߵİٷֱ�
            /// ���磺���˿����ﵽ�����˿����޵�50%ʱ����Χ���ӿ�ʼ������������Ϊ30%
            /// ��SpreadConditionRateΪ0.5
            /// </summary>
            public float SpreadConditionRate { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="parentStrain"></param>
            /// <param name="currentBlock"></param>
            /// <param name="neigborBlocks"></param>
            public void Effect( ref Strain parentStrain, ref Map.Block currentBlock, ref List<Map.Block> neigborBlocks ) {
                // �˿�*ϵ�� ��ֵӰ�����ʸı����Ĵ�С
                var delta = ( parentStrain.Population * PopulationCoefficient ) + Intercept;

                // ----- �Ի�ѧ���ʲ���Ӱ�� -----
                // �����Ƿ�����������
                var productChem = currentBlock.PublicChemicals.Find( che => { return che.Name == ProductionChemicalInfo.Name; } );
                if( productChem == null ) {
                    productChem = new Chemical {
                        Name = ProductionChemicalInfo.Name,
                        Count = 0,
                        SpreadRate = ProductionChemicalInfo.SpreadRate
                    };
                    // ��block���ʼ�����Ӹı��chemical
                    currentBlock.PublicChemicals.Add( productChem );
                }
                productChem.Count += ( ProductionChemicalInfo.Count * delta );
                // ----- �Ի�ѧ ���ʲ���Ӱ�� -----

                // ----- �Ը�strain����Ӱ�� -----
                // --- ����˿� ---
                parentStrain.Population += delta;

                // --- ���˽�л�ѧ����� ---
                // ��Ѱ��block���Ƿ���ڸ��ֻ�ѧ����
                var publicChemical = currentBlock.PublicChemicals.Find( chem => { return chem.Name == ImportChemicalInfo.Name; } );
                if( publicChemical != null ) {
                    var privateChemical = parentStrain.PrivateChemicals.Find( chem => { return chem.Name == publicChemical.Name; } );
                    if( privateChemical == null ) {
                        parentStrain.PrivateChemicals.Add( new Chemical {
                            Count = 0,
                            Name = ImportChemicalInfo.Name,
                            SpreadRate = ImportChemicalInfo.SpreadRate
                        } ); // ���û�У������
                    }
                    if( publicChemical.Count >= privateChemical.Count ) {
                        privateChemical.Count += ImportChemicalInfo.Count;
                        publicChemical.Count -= ImportChemicalInfo.Count;
                    }
                }
                // ----- �Ը�strain����Ӱ�� -----

                // ----- ϸ����ɢ -----
                // �Ƿ�������ɢ����
                if( parentStrain.Population * SpreadConditionRate >= parentStrain.Population ) {
                    var cloneStrain = ( Strain )parentStrain.Clone();
                    // �趨��ʼ�˿���
                    cloneStrain.Population = ( int )( parentStrain.Population * FirstSpreadMountRate );
                    // Ϊ��Χ�ĸ�����Ӹ�ϸ��
                    foreach( var block in neigborBlocks ) {
                        block.Strains.Add( cloneStrain );
                    }
                }
                // ----- ϸ����ɢ -----
            }
        }

        public Gamer.Player Owner { get; set; } = new Gamer.Player();
        public string Name { get; set; }
        public int Population { get; set; }

        /// <summary>
        /// ���ѡ����Դ��Ļ��ǿ���Ĭ�ϴ��ڵĻ���
        /// </summary>
        public List<RegulatoryGene> PlayerSelectedGenes { get; set; } = new List<RegulatoryGene>();

        /// <summary>
        /// ��ȡ�Ļ�ѧ����
        /// �� PublicChemicals
        /// </summary>
        /// <seealso cref="CellWar.Model.Map.Block.PublicChemicals"/>
        public List<Substance.Chemical> PrivateChemicals { get; set; } = new List<Chemical>();

        /// <summary>
        /// �����Դ��Ļ���
        /// </summary>
        public Species BasicSpecies { get; set; }

        public Object Clone() {
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
    /// ��ѧ����
    /// </summary>
    public class Chemical {
        public string Name { get; set; }
        public int Count { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public int SpreadRate { get; set; }
    }
}