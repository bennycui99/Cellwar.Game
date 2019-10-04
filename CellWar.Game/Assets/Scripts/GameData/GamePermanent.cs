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
        public static List<Race> AllRaces { get; set; }
        public static List<Race> LoadAllRaces() {
            var allRaceJson = CellWar.Utils.JsonHelper.Json2Object_NT<List<RaceJsonModel>>( GetGameDataPath( "race.json" ) );
            AllRaces = new List<Race>();
            foreach( var raceJson in allRaceJson ) {
                if( raceJson.CodingGeneNames == "" ) continue;

                AllRaces.Add( new Race {
                    Description = raceJson.Description,
                    Name = raceJson.Name,
                    MaxLength = raceJson.MaxLength,
                    RegulatoryGenes = SemanticObjectController.GenerateText2RegGeneObjects( raceJson.CodingGeneNames )
                } );
            }
            return AllRaces;
        }

        public static List<CodingGene> AllCodingGenes { get; set; }
        public static List<CodingGene> LoadAllCodingGenes() {
            AllCodingGenes = JsonHelper.Json2Object_NT<List<CodingGene>>( GetGameDataPath( "coding_gene.json" ) );
            return AllCodingGenes;
        }

        public static List<Chemical> AllChemicals { get; set; }
        public static List<Chemical> LoadAllChemicals() {
            AllChemicals = JsonHelper.Json2Object_NT<List<Chemical>>( GetGameDataPath( "chemicals.json" ) );
            return AllChemicals;
        }

        public static List<RegulatoryGene> AllRegulartoryGenes { get; set; }
        public static List<RegulatoryGene> LoadAllRegulartoryGenes() {
            try
            {
                var allRegulatoryGeneJson = JsonHelper.Json2Object_NT<List<RegulartoryGeneJsonModel>>( GetGameDataPath( "reg_gene.json" ) );
                List<RegulatoryGene> regulatoryGenes = new List<RegulatoryGene>();
                foreach( var geneJson in allRegulatoryGeneJson ) {
                    RegulatoryGene gene = new RegulatoryGene();
                    gene.Type = geneJson.Type;
                    gene.Name = geneJson.Name;
                    gene.Description = geneJson.Description;
                    gene.Length = geneJson.Length;
                    gene.IsInternal = geneJson.IsInternal;
                    gene.Conditions = SemanticObjectController.GenerateText2ChemicalsWithCountInfo(geneJson.ChemicalConditions);

                    regulatoryGenes.Add( gene );
                }
                AllRegulartoryGenes = regulatoryGenes;
                return regulatoryGenes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static Chemical FindChemicalByName( string chemicalName ) => AllChemicals.Find( c => { return c.Name == chemicalName; } );
        public static Race FindRaceByName(string raceName) {
            var race = AllRaces.Find(c => { return c.Name == raceName; });
            if (race == null)
            {
                throw new Exception(string.Format("Race named [ {0} ] does not exsits.", raceName));
            }
            return race;

        }

        /// <summary>
        /// 所有的游戏内置strain
        /// </summary>
        public static List<Strain> AllNpcStrains { get; set; } = new List<Strain>();
        public static List<Strain> LoadAllNpcStrains() {
            var strainJson = JsonHelper.Json2Object_NT<List<StrainJsonModel>>( GetGameDataPath( "npc_strains.json" ) );
            StrainController strainController = new StrainController();
            AllNpcStrains.Clear();
            foreach (var s in strainJson)
            {
                AllNpcStrains.Add( strainController.JsonModel2Strain_Npc(s) );
            }
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
            StrainController strainController = new StrainController();

            foreach( var s in Strains ) {
                strainJson.Add( strainController.Strain2JsonModel( s ) );
            }

            File.WriteAllText( GetGameSavePath( "strains.json" ), JsonHelper.Object2Json( strainJson ) );
        }

        /// <summary>
        /// 通过路径调用；省略Resources: "Save/strains.json"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<Strain> LoadStrainsWithFilePath(string path) {
            string filePath = Path.Combine(Application.dataPath, "Resources/" + path);
            var strainJson = JsonHelper.Json2Object_NT<List<StrainJsonModel>>( filePath );
            StrainController strainController = new StrainController();

            List<Strain> newStrains = new List<Strain>();
            newStrains.Clear();
            foreach( var s in strainJson ) {
                newStrains.Add( strainController.JsonModel2Strain_Player( s ) );
            }
            return newStrains;
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

    public static class Textures {
        public static string GetArtworkPath( string path ) {
            return Application.dataPath + "/Resources/Textures/Artworks/" + path;
        }
        public static readonly string DefaultArtworkPath = GetArtworkPath( "default.png" );
    }
}
