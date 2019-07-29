using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Utils.Object;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;

namespace CellWar.View {
    public class U3D_CodElement : MonoBehaviour, IPointerClickHandler {
        public CodingGene CodingGene { get; set; }

        // Start is called before the first frame update
        void Start() {
            transform.Find( "Label" ).GetComponent<Text>().text = CodingGene.Name;
        }

        public void Switch() {
        }

        public void OnPointerClick( PointerEventData eventData ) {
            var dominatedGenes = LabCurrent.RegulatoryGene.DominatedGenes;
            var cod = dominatedGenes.Find( m => m.Name == CodingGene.Name );
            if( GetComponent<Toggle>().isOn ) {
                if( cod == null ) {
                    dominatedGenes.Add( ObjectHelper.Clone( CodingGene ) );
                }
            } else {
                dominatedGenes.Remove( cod );
            }
            U3D_CreatorSceneLoad.FreshLength();
        }
    }
}
