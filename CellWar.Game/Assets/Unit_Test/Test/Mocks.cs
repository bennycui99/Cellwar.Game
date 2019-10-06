using System.Collections;
using System.Collections.Generic;
using CellWar.Controller.Gene;
using CellWar.GameData;
using CellWar.Model.Gamer;
using CellWar.Model.Map;
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

        /// <summary>
        /// 游戏性问题 #22
        /// https://github.com/bennycui99/Cellwar.Game/issues/22
        /// </summary>
        public static void TestPrivateChemical_Usable()
        {
            CodingGene g = new CodingGene
            {
                Name = "g",
                ConsumeChemicalName = "c",
                ConsumeChemicalIntercept = 1,
                ConsumeChemicalCoeffeicient = 0.1f,
                IsConsumePublic = false
            };

            Chemical c = new Chemical
            {
                Name = "c",
                Count = 100
            };

            Strain s = new Strain();

            s.PrivateChemicals.Add(c);

            Block b = new Block();

            CodingGeneController ctor = new CodingGeneController();
            ctor.Consume(ref s, ref b, ref g);
            Debug.Log(s.PrivateChemicals.Find(ss => ss.Name == "c").Count);
            Debug.Assert(s.PrivateChemicals.Find(ss => ss.Name == "c").Count < 100);
            ctor.Consume(ref s, ref b, ref g);
            Debug.Log(s.PrivateChemicals.Find(ss => ss.Name == "c").Count);

            Debug.Assert(s.PrivateChemicals.Find(ss => ss.Name == "c").Count < 100);

            Debug.Log(s.PrivateChemicals.Find(ss => ss.Name == "c").Count);
            ctor.Consume(ref s, ref b, ref g);
            Debug.Assert(s.PrivateChemicals.Find(ss => ss.Name == "c").Count < 100);

            Debug.Log(s.PrivateChemicals.Find(ss => ss.Name == "c").Count);
            ctor.Consume(ref s, ref b, ref g);
            Debug.Assert(s.PrivateChemicals.Find(ss => ss.Name == "c").Count < 100);
        }

        public static void TestGameOver() {
            Map map = new Map {
                Blocks = new List<Block>{
                    new Block{
                        PublicChemicals = new List<Chemical>{
                            new Chemical {
                                Name = "Cu", Count = 100
                            }
                        }
                    }
                },
                GameOverCondition = "Cu:10"
            };

            Debug.Assert( MainGameCurrent.IsGameOver( map ) );
            map.GameOverCondition = "Cu:1000";
            Debug.Assert( !MainGameCurrent.IsGameOver( map ) );
            map.GameOverCondition = "Cu:-1000";
            Debug.Assert( MainGameCurrent.IsGameOver( map ) );
            map.GameOverCondition = "Cu:-100";
            Debug.Assert( !MainGameCurrent.IsGameOver( map ) );
        }

        /// <summary>
        /// 游戏性问题 #22
        /// https://github.com/bennycui99/Cellwar.Game/issues/22
        /// </summary>
        public static void TestDeathGeneImportChemical_Usable()
        {
            CodingGene g = new CodingGene
            {
                Name = "g",
                ImportChemicalName = "c",
                ImportChemicalIntercept = 1,
                ImportChemicalCoeffeicient = 0.01f,
            };

            Chemical c = new Chemical
            {
                Name = "c",
                Count = 10
            };

            Strain s = new Strain();

            s.PrivateChemicals.Add(ObjectHelper.Clone(c));

            Block b = new Block();

            GameData.Local.AllChemicals = new List<Chemical>();
            GameData.Local.AllChemicals.Add(ObjectHelper.Clone(c));
            c.Count = 100;
            b.PublicChemicals.Add( ObjectHelper.Clone( c));

            CodingGeneController ctor = new CodingGeneController();
            ctor.ImportChemical(ref s, ref b, ref g);

            //
            Debug.Log(b.PublicChemicals.Find(cc => cc.Name == "c").Count);
            Debug.Log(s.PrivateChemicals.Find(cc => cc.Name == "c").Count);
            Debug.Assert(b.PublicChemicals.Find(cc => cc.Name == "c").Count < 100);

            //
            s.PrivateChemicals.Clear();
            ctor.ImportChemical(ref s, ref b, ref g);
            Debug.Log(b.PublicChemicals.Find(cc => cc.Name == "c").Count);
            Debug.Log(s.PrivateChemicals.Find(cc => cc.Name == "c").Count);
            Debug.Assert(b.PublicChemicals.Find(cc => cc.Name == "c").Count < 100);


            GameData.Local.AllChemicals = null;
        }
    }
}
