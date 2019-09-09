using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CellWar.Controller;
using CellWar.Model.Json;
using CellWar.Model.Substance;
using CellWar.Utils;
using CellWar.Utils.Object;
using CellWar.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static CellWar.Model.Substance.Strain;
using UnityEngine.UI;
using CellWar.Model.Gamer;

namespace CellWar.GameData {
    /// <summary>
    /// 游戏本地数据
    /// </summary>
    public static class Local {

        /// <summary>
        /// 检测UI穿透
        /// </summary>
        /// <returns></returns>
        public static bool CheckGuiRaycastObjects( EventSystem eventSystem, GraphicRaycaster graphicRaycaster ) {

            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> list = new List<RaycastResult>();
            graphicRaycaster.Raycast( eventData, list );
            //Debug.Log(list.Count);
            return list.Count > 0;
        }

        /// <summary>
        /// 将reg-cod规范的字符串转化为reg list对象
        /// geneNameList的规范为 r1;c1;c2;r2;c3;r3;c4
        /// reg后的cod会被自动归类到该r的支配基因中，读到下一个reg时停止
        /// r1;c1;c2;r2;c3;r3;c4
        /// 将会转化为
        /// r1 - c1
        ///      c2
        ///      
        /// r2 - c3
        /// 
        /// r3 - c4
        /// </summary>
        /// <returns></returns>
        public static List<RegulatoryGene> GenerateText2RegGeneObjects( string geneNames ) {
            var geneNameList = geneNames.Split( ';' );
            List<RegulatoryGene> regulatoryGenes = new List<RegulatoryGene>();

            RegulatoryGene previousReg = null;

            foreach( var geneName in geneNameList ) {
                var regGene = AllRegulartoryGenes.Find( m => m.Name == geneName );
                // 首个名字必须为 reg gene
                if( regGene == null && previousReg == null ) {
                    throw new InvalidOperationException( "Invalid GeneNames. you should input a reg gene at first of the string list"
                        + "\n current gene name:" + geneName );
                }
                else if( regGene == null && previousReg != null ) {
                    // 继续增加该reg下的支配cod 
                    var codGene = AllCodingGenes.Find( m => m.Name == geneName );
                    if( codGene != null ) {
                        previousReg.DominatedGenes.Add( codGene );
                    }
                    else {
                        throw new InvalidOperationException( "No gene named: " + geneName + "\n you should call LoadAllRaces() after LoadAllCodingGenes" );
                    }
                }
                else if( regGene != null ) {
                    // 开始添加下一个reg
                    previousReg = ObjectHelper.Clone( regGene, regGene.GetType() );
                    regulatoryGenes.Add( previousReg );
                }
            }
            return regulatoryGenes;
        }
        /// <summary>
        /// 将reg list对象转化为reg-cod规范的字符串
        /// geneNameList的规范为 r1;c1;c2;r2;c3;r3;c4
        /// r1 - c1
        ///      c2
        ///      
        /// r2 - c3
        /// 
        /// r3 - c4
        /// 将会转化为
        /// r1;c1;c2;r2;c3;r3;c4
        /// </summary>
        /// <returns></returns>
        public static string GenerateRegGeneObjects2Text( List<RegulatoryGene> regs ) {
            string generated = "";
            foreach( var reg in regs ) {
                generated += reg.Name + ";";
                foreach( var cod in reg.DominatedGenes ) {
                    generated += cod.Name + ";";
                }
            }
            return generated.Remove( generated.Length - 1 );
        }

        public static List<Race> AllRaces { get; set; }
        public static List<Race> LoadAllRaces() {
            var allRaceJson = CellWar.Utils.JsonHelper.Json2Object_NT<List<RaceJsonModel>>( GetGameDataPath( "race.json" ) );
            AllRaces = new List<Race>();
            foreach( var raceJson in allRaceJson ) {
                if( raceJson.CodingGeneNames == "" ) continue;

                AllRaces.Add( new Race {
                    Name = raceJson.Name,
                    MaxLength = raceJson.MaxLength,
                    RegulatoryGenes = GenerateText2RegGeneObjects( raceJson.CodingGeneNames )
                } );
            }
            return AllRaces;
        }

        public static List<CodingGene> AllCodingGenes { get; set; }
        public static List<CodingGene> LoadAllCodingGenes() {
            Local.AllCodingGenes = CellWar.Utils.JsonHelper.Json2Object_NT<List<CodingGene>>( GetGameDataPath( "coding_gene.json" ) );
            return AllCodingGenes;
        }

        public static List<Chemical> AllChemicals { get; set; }
        public static List<Chemical> LoadAllChemicals() {
            Local.AllChemicals = CellWar.Utils.JsonHelper.Json2Object_NT<List<Chemical>>( GetGameDataPath( "chemicals.json" ) );
            return AllChemicals;
        }

        public static List<RegulatoryGene> AllRegulartoryGenes { get; set; }
        public static List<RegulatoryGene> LoadAllRegulartoryGenes() {
            var allRegulatoryGeneJson = CellWar.Utils.JsonHelper.Json2Object_NT<List<RegulartoryGeneJsonModel>>( GetGameDataPath( "reg_gene.json" ) );
            List<RegulatoryGene> regulatoryGenes = new List<RegulatoryGene>();
            foreach( var geneJson in allRegulatoryGeneJson ) {
                RegulatoryGene gene = new RegulatoryGene();
                gene.Type = geneJson.Type;
                gene.Name = geneJson.Name;
                gene.Description = geneJson.Description;
                gene.Length = geneJson.Length;

                foreach( var cc in geneJson.Condition ) {
                    var ch = ObjectHelper.Clone( FindChemicalByName( cc.Chemical ) );
                    if( ch != null ) {
                        ch.Count = cc.Count;
                        gene.Conditions.Add( ch );
                    }
                }

                regulatoryGenes.Add( gene );
            }
            AllRegulartoryGenes = regulatoryGenes;
            return regulatoryGenes;
        }

        public static Chemical FindChemicalByName( string chemicalName ) => AllChemicals.Find( c => { return c.Name == chemicalName; } );
        public static Race FindRaceByName( string raceName ) => AllRaces.Find( c => { return c.Name == raceName; } );

        /// <summary>
        /// 所有的游戏内置strain
        /// </summary>
        public static List<Strain> AllNpcStrains { get; set; }
        public static List<Strain> LoadAllNpcStrains() {
            AllNpcStrains = JsonHelper.Json2Object_NT<List<Strain>>( GetGameDataPath( "npc_strains.json" ) );
            return AllNpcStrains;
        }

        public static List<Player> AllNpcs { get; set; }
        public static List<Player> LoadAllNpcs() {
            AllNpcs = JsonHelper.Json2Object_NT<List<Player>>( GetGameDataPath( "npc_characters.json" ) );
            return AllNpcs;
        }

        public static string GetGameDataPath( string fileName ) => "Resources/GameData/" + fileName;

        public static string Readable( string name, string description ) {
            return string.Format( "Name:{0}\nDescription:{1}", name, description );
        }
    }

    /// <summary>
    /// 玩家储存数据
    /// TODO：玩家信息
    /// </summary>
    public static class Save {
        public static List<Strain> Strains = new List<Strain>();
        public static string GetGameSavePath( string fileName ) => Path.Combine( Application.dataPath, "Resources/Save/" + fileName );
        public static void SaveAllStrains() {
            List<StrainJsonModel> strainJson = new List<StrainJsonModel>();

            foreach( var s in Strains ) {
                strainJson.Add( new StrainJsonModel {
                    Name = s.Name,
                    Owner = s.Owner,
                    BasicRaceName = s.BasicRace.Name,
                    Population = s.Population,
                    PlayerSelectedGenesName = Local.GenerateRegGeneObjects2Text( s.PlayerSelectedGenes ),
                } );
            }

            File.WriteAllText( Save.GetGameSavePath( "strains.json" ), JsonHelper.Object2Json( strainJson ) );
        }

        public static List<Strain> LoadAllStrains() {
            var strainJson = JsonHelper.Json2Object_NT<List<StrainJsonModel>>( GetGameSavePath( "strains.json" ) );

            Strains.Clear();
            foreach( var s in strainJson ) {
                Strains.Add( new Strain {
                    Name = s.Name,
                    Owner = s.Owner,
                    BasicRace = Local.FindRaceByName( s.BasicRaceName ),
                    Population = s.Population,
                    PlayerSelectedGenes = Local.GenerateText2RegGeneObjects( s.PlayerSelectedGenesName ),
                    PrivateChemicals = new List<Chemical>()
                } );
            }
            return Strains;
        }
    }

    /// <summary>
    /// 数据监测操作
    /// </summary>
    public static class Check {
        /// <summary>
        /// 如果检测到数据未加载，则返回数据加载场景
        /// </summary>
        public static void GameDataLoaded() {
            if( GameData.Local.AllChemicals == null ) {
                SceneManager.LoadScene( "ApplicationStartScene" );
            }
        }
    }
}
