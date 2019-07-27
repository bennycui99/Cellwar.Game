using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
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

        // Update is called once per frame
        void Update() {

        }

        public void Switch() {

        }

        public void OnPointerClick( PointerEventData eventData ) {
            var dominatedGenes = GeneCreatorCurrent.RegulatoryGene.DominatedGenes;
            if( GetComponent<Toggle>().isOn ) {
                if( !dominatedGenes.Exists( m => m.Name == CodingGene.Name ) ) {
                    dominatedGenes.Add( CodingGene );
                }
            } else {
                dominatedGenes.Remove( CodingGene );
            }
        }
    }
}
