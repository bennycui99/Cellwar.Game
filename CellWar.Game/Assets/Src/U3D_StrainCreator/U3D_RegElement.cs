using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;

namespace CellWar.View {
    public class U3D_RegElement : MonoBehaviour, IPointerClickHandler {
        public RegulatoryGene RegulatoryGene { get; set; }

        // Start is called before the first frame update
        void Start() {
            transform.Find( "Label" ).GetComponent<Text>().text = RegulatoryGene.Name;
        }

        // Update is called once per frame
        void Update() {

        }

        public void SwitchCodingGeneList() {
            var reg = RegulatoryGene;
            GeneCreatorCurrent.RegulatoryGene = reg;
            string str = reg.Name + "'s Coding Gene(s)";
            UIHelper.ChangeText( GameObject.Find( "UI_CurrentReg" ), str );
            UIHelper.SwitchOffAllToggle( "UI_CodList" );
            // 显示所有的cod勾选状态
            foreach( var cod in reg.DominatedGenes ) {
                UIHelper.SwitchToggle( cod.Name, true );
            }
        }

        public void OnPointerClick( PointerEventData eventData ) {
            SwitchCodingGeneList();
        }
    }
}
