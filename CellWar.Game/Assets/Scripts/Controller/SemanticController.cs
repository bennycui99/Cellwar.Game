
using System;
using System.Collections.Generic;
using CellWar.Utils.Object;
using CellWar.GameData;
using static CellWar.Model.Substance.Strain;
using CellWar.Model.Substance;
using UnityEngine;

namespace CellWar.Controller {
    /// <summary>
    /// 语意对象Controller
    /// </summary>
    public static class SemanticObjectController {
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
            var geneNameList = geneNames.Split(';');
            List<RegulatoryGene> regulatoryGenes = new List<RegulatoryGene>();

            RegulatoryGene previousReg = null;

            foreach( var geneName in geneNameList ) {
                var regGene = Local.AllRegulartoryGenes.Find(m => m.Name == geneName);
                // 首个名字必须为 reg gene
                if( regGene == null && previousReg == null ) {
                    throw new InvalidOperationException( "Invalid GeneNames. you should input a reg gene at first of the string list"
                        + "\n current gene name:" + geneName );
                }
                else if( regGene == null && previousReg != null ) {
                    // 继续增加该reg下的支配cod 
                    var codGene = Local.AllCodingGenes.Find(m => m.Name == geneName);
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

        /// <summary>
        ///
        /// 将文本转化为chemicals对象
        ///
        /// e.g
        /// 
        /// "PrivateChemicalInfos": "ATP:10;GFP:20;lactose:10"
        ///
        /// 形如 name:count;name2:count2
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static List<Chemical> GenerateText2ChemicalsWithCountInfo( string text ) {
            List<Chemical> chemicals = new List<Chemical>();

            if( string.IsNullOrEmpty( text ) ) {
                return chemicals;
            }

            var chemicalPairs = text.Split(';');
            foreach( var chemicalPair in chemicalPairs ) {
                if( string.IsNullOrEmpty( chemicalPair ) ) {
                    continue;
                }
                var nameAndCount = chemicalPair.Split(':');
                // name:count
                string name = nameAndCount[0];
                int count = Convert.ToInt32( nameAndCount[1] );

                var clone = Local.AllChemicals.Find(c1 => c1.Name == name);
                if( clone == null ) {
                    Debug.Log( "Chemical named[ " + name + "] does not exsit." );
                }
                var c = clone.Clone() as Chemical;
                c.Count = count;
                chemicals.Add( c );
            }
            return chemicals;
        }

        public static string GenerateChemicals2Text( List<Chemical> cs ) {
            string text = "";
            foreach( var c in cs ) {
                text += string.Format( "{0}:{1};", c.Name, c.Count );
            }
            return text.TrimEnd();
        }

    }
}