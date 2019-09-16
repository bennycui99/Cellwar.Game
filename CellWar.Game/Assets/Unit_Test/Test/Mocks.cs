using System.Collections;
using System.Collections.Generic;
using CellWar.Controller.Gene;
using CellWar.Model.Gamer;
using CellWar.Model.Substance;
using CellWar.Utils.Object;
using UnityEngine;
using static CellWar.Model.Substance.Strain;

namespace CellWar.Test.Mock {
    public class Mocks {
        public static Strain Strain1 { get; set; } = new Strain {
            Name = "Strain1",
            Owner = MockPlayerCyf,
            Population = 100,
        };
        public static Strain Strain2 { get; set; } = new Strain {
            Name = "Strain2",
            Owner = MockPlayerCyf,
            Population = 200,
        };
        public static Strain Strain3 { get; set; } = new Strain {
            Name = "Strain3",
            Owner = MockPlayerCyf,
            Population = 300,
        };

        public static List<Strain> MockStrainList { get; set; } = new List<Strain>();

        public static Player MockPlayerCyf { get; set; } = new Player {
            Name = "cyf"
        };
    }

    public static class Unit
    {
        // no 只要一个成立就不工作
        public static void TestRegGeneCondition_NO()
        {
            RegulatoryGene reg = new RegulatoryGene
            {
                Conditions = new List<Chemical>
                {
                    new Chemical
                    {
                        Name = "1", Count = 10
                    },
                    new Chemical
                    {
                        Name = "2", Count = 200
                    },
                                        new Chemical
                    {
                        Name = "3", Count = -200
                    }
                },
                Type = "NO"
            };

            Debug.Assert( false == triggered( "1", 10, reg) );
            Debug.Assert( true == triggered("1", 5, reg));
            Debug.Assert( false == triggered("2", 210, reg));
            Debug.Assert( true == triggered("2", 190, reg));
            Debug.Assert( true == triggered("3", 1190, reg) );
            Debug.Assert( false == triggered("3", 190, reg) );
            Debug.Assert(true == triggered("4", 190, reg));
            Debug.Assert(true == triggered("5", 190, reg));

        }


        public static void TestRegGeneCondition_PA()
        {
            var condition = new List<Chemical>
            {
                                    new Chemical
                    {
                        Name = "1", Count = 10
                    },
                    new Chemical
                    {
                        Name = "2", Count = 200
                    },
                                        new Chemical
                    {
                        Name = "3", Count = -200
                    }
            };
            RegulatoryGene reg = new RegulatoryGene
            {
                Conditions = ObjectHelper.CloneList2( condition ),
                Type = "PA"
            };

            Debug.Assert(true == ctor.IsTriggered( condition, reg ));
            Debug.Assert(!triggered("1", 190, reg));
            Debug.Assert(!triggered("2", 190, reg));
            Debug.Assert(!triggered("3", 190, reg));

        }

        static RegulatoryGeneController ctor = new RegulatoryGeneController();

        static bool triggered( string name, int count, RegulatoryGene reg )
        {
            return (ctor.IsTriggered(new List<Chemical>
            {
                new Chemical
                {
                    Name =  name, Count = count
                }
            }, reg));
        }
    }
}
