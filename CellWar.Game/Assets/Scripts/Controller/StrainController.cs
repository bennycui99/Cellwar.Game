using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Model.Json;
using CellWar.Model.Substance;
using CellWar.Utils.Object;
using static CellWar.Model.Substance.Strain;

namespace CellWar.Controller {
    public class StrainController {
        public int GetTotalLength( List<RegulatoryGene> regs ) {
            int total = 0;
            regs.ForEach( a => { total += a.Length; a.DominatedGenes.ForEach( b => total += b.Length ); } );
            return total;
        }

        public List<RegulatoryGene> CloneRegGeneList( List<RegulatoryGene> regs ) {
            List<RegulatoryGene> newRegs = new List<RegulatoryGene>();
            foreach( var reg in regs ) {
                newRegs.Add( ObjectHelper.Clone( reg, reg.GetType() ) );
            }
            return newRegs;
        }

        public bool IsValid( Strain strain ) {
            int totalLength = 0;
            strain.PlayerSelectedGenes.ForEach( gene => { totalLength += gene.Length; } );
            return totalLength < strain.BasicRace.MaxLength;
        }

        public Strain JsonModel2Strain_Npc( StrainJsonModel s ) {
            return new Strain {
                Name = s.Name,
                Owner = s.Owner,
                BasicRace = Local.FindRaceByName( s.BasicRaceName ),
                Population = s.Population,
                PlayerSelectedGenes = SemanticObjectController.GenerateText2RegGeneObjects( s.PlayerSelectedGenesName ),
                PrivateChemicals = string.IsNullOrEmpty( s.PrivateChemicalInfos ) ? new List<Chemical>() : SemanticObjectController.GenerateText2ChemicalsWithCountInfo( s.PrivateChemicalInfos )
            };
        }
        public StrainJsonModel Strain2JsonModel( Strain s ) {
            return new StrainJsonModel {
                Name = s.Name,
                Owner = s.Owner,
                BasicRaceName = s.BasicRace.Name,
                // Fix #issue 18
                // https://github.com/bennycui99/Cellwar.Game/issues/18
                Population = 100,
                PlayerSelectedGenesName = SemanticObjectController.GenerateRegGeneObjects2Text( s.PlayerSelectedGenes ),
            };
        }
        public Strain JsonModel2Strain_Player( StrainJsonModel s ) {
            return new Strain {
                Name = s.Name,
                Owner = s.Owner,
                BasicRace = Local.FindRaceByName( s.BasicRaceName ),
                Population = s.Population,
                PlayerSelectedGenes = SemanticObjectController.GenerateText2RegGeneObjects( s.PlayerSelectedGenesName ),
                PrivateChemicals = new List<Chemical>()
            };
        }
        public Strain Text2Strain_Npc( string name ) {
            return Local.AllNpcStrains.Find( m => m.Name == name );
        }
        public List<Strain> Text2Strains_Npc( string info ) {
            if( string.IsNullOrEmpty( info) ) {
                return new List<Strain>();
            }
            var ss = info.Split(';');
            List<Strain> strains = new List<Strain>();
            foreach( var s in ss ) {
                var npcStrain = Text2Strain_Npc( s );
                if( npcStrain != null ) {
                    strains.Add( npcStrain );
                }
            }
            return strains;
        }
        public string Strains2Text( List<Strain>ss ) {
            string t ="";
            foreach( var s in ss ) {
                t += string.Format( "{0};", s.Name );
            }
            return t == "" ? "" : t.TrimEnd();
        }
    }
}
