using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Utils;
using CellWar.Utils.Object;
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

        public void SwitchCodingGeneList( RegulatoryGene currentReg ) {
            var reg = currentReg;
            LabCurrent.RegulatoryGene = reg;
            string str = reg.Name + "'s Coding Gene(s)";
            UIHelper.ChangeText( GameObject.Find( "UI_CurrentReg" ), str );
            UIHelper.SwitchOffAllToggle( "UI_CodList" );
            // 显示所有的cod勾选状态
            foreach( var cod in reg.DominatedGenes ) {
                UIHelper.SwitchToggle( cod.Name, true );
            }
        }

        public void OnPointerClick( PointerEventData eventData ) {
            var regs = LabCurrent.Strain.PlayerSelectedGenes;
            var reg = regs.Find( m => m.Name == RegulatoryGene.Name );
            if( GetComponent<Toggle>().isOn ) {
                if( reg == null ) {
                    regs.Add( reg = RegulatoryGene );
                }
            } else {
                if( reg != null ) {
                    regs.Remove( reg );
                }
            }
            SwitchCodingGeneList( reg );
            U3D_CreatorSceneLoad.FreshLength();
        }
    }
}
