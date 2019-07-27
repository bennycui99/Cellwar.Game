﻿using System;
using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Json;
using CellWar.Model.Substance;
using CellWar.Utils.Object;
using CellWar.View;
using UnityEngine;
using static CellWar.Model.Substance.Strain;

namespace CellWar.GameData {
    /// <summary>
    /// 游戏及时数据
    /// </summary>
    public static class Current {
        /// <summary>
        /// 玩家手上是否拿着细菌准备防止重要数据
        /// 见 U3D_StrainPackageLogic.cs
        /// </summary>
        public static Strain HoldingStrain = null;

        public static List<Strain> StrainList = new List<Strain>();

        public static string GetCurrentBlockDetailInfo() {
            if( CellWar.GameData.Current.FocusedBlock == null ) {
                return "";
            }
            var currentHexBlock = CellWar.GameData.Current.FocusedBlock.HexBlockModel;
            string showText = "Condition: " + currentHexBlock.BlockType.ToString() + "\n\n";
            showText += "Chemicals: \n";
            if( currentHexBlock.PublicChemicals.Count == 0 ) {
                showText += "Nothing so far.";
            }
            foreach( var chem in currentHexBlock.PublicChemicals ) {
                showText += string.Format( "{0}: {1}\n", chem.Name, chem.Count.ToString() );
            }
            showText += "\n\nStrains: \n";
            if( currentHexBlock.Strains.Count == 0 ) {
                showText += "Nothing so far.";
            }
            foreach( var str in currentHexBlock.Strains ) {
                showText += string.Format( "{0}: {1}\n", str.Name, str.Population.ToString() );
            }
            return showText;
        }

        public static string GetCurrentBlockStrainDetailInfo() {
            if( FocusedBlock == null ) {
                return "";
            }
            var currentHexBlock = FocusedBlock.HexBlockModel;
            string showText = "";
            foreach( var str in currentHexBlock.Strains ) {
                showText += str.Name + "\n";
                foreach( var gene in str.PlayerSelectedGenes ) {
                    showText += gene.Name + "\t";
                }
                showText += "\n";
                showText += str.ConditionGene.IsTriggered( currentHexBlock.PublicChemicals ) ? "Working" : "Sleeping";
            }
            return showText;
        }
        /// <summary>
        /// 当前鼠标下的block的MonoBehavior实例
        /// </summary>
        public static U3D_BlockLogic FocusedBlock = null;
    }
    /// <summary>
    /// 游戏本地数据
    /// </summary>
    public static class Local {

        public static List<Race> LoadAllRaces() {
            var allRaceJson = CellWar.Utils.JsonHelper.Json2Object_NT<List<RaceJsonModel>>( GetGameDataPath("race.json") );
            AllRaces = new List<Race>();
            foreach( var raceJson in allRaceJson ) {
                var geneNameList = raceJson.CodingGeneNames.Split(';');
                List<CodingGene> genes = new List<CodingGene>();
                foreach( var geneName in geneNameList ) {
                    var tgene = AllCodingGenes.Find( m => m.Name == geneName );
                    if( tgene != null ) {
                        genes.Add( tgene );
                    } else {
                        throw new InvalidOperationException( "No gene named: " + geneName + "\n you should call LoadAllRaces() after LoadAllCodingGenes");
                    }
                }
                AllRaces.Add( new Race {
                    Name = raceJson.Name,
                    MaxLength = raceJson.MaxLength,
                    Genes = genes
                } );
            }
            return AllRaces;
        }
        public static List<Race> AllRaces { get; set; }

        public static List<CodingGene> LoadAllCodingGenes() {
            Local.AllCodingGenes = CellWar.Utils.JsonHelper.Json2Object_NT<List<CodingGene>>( GetGameDataPath( "coding_gene.json" ) );
            return AllCodingGenes;
        }
        public static List<CodingGene> AllCodingGenes { get; set; }

        public static List<Chemical> LoadAllChemicals() {
            Local.AllChemicals = CellWar.Utils.JsonHelper.Json2Object_NT<List<Chemical>>( GetGameDataPath( "chemicals.json" ) );
            return AllChemicals;
        }

        public static List<RegulatoryGene> LoadAllRegulartoryGenes() {
            var allRegulatoryGeneJson = CellWar.Utils.JsonHelper.Json2Object_NT<List<RegulartoryGeneJsonModel>>( GetGameDataPath( "reg_gene.json" ) );
            List<RegulatoryGene> regulatoryGenes = new List<RegulatoryGene>();
            foreach( var geneJson in allRegulatoryGeneJson ) {
                RegulatoryGene gene;
                var c = ( RegulartoryGeneType )Enum.Parse( typeof( RegulartoryGeneType ), geneJson.Type );
                switch( c ) {
                    case RegulartoryGeneType.PA:
                        gene = new PositiveAllRegulatoryGene();
                        break;
                    case RegulartoryGeneType.NA:
                        gene = new NegativeAllRegulartoryGene();
                        break;
                    case RegulartoryGeneType.PO:
                        gene = new PositiveOrRegulartoryGene();
                        break;
                    case RegulartoryGeneType.NO:
                        gene = new NegativeOrRegulartoryGene();
                        break;
                    default:
                        throw new Exception( "In LoadAllRegulartoryGenes. Type is Invalid." );
                }

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

        public static List<RegulatoryGene> AllRegulartoryGenes { get; set; }
        public static List<Chemical> AllChemicals { get; set; }

        public static string GetGameDataPath( string fileName ) => "Resources/GameData/" + fileName;
    }
}