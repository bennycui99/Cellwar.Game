using CellWar.GameData;
using CellWar.Utils;
using CellWar.Utils.Object;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;

namespace CellWar.View
{
    public class U3D_RegElement : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
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

        public void OnPointerDown( PointerEventData eventData ) {
            var regs = LabCurrent.Strain.PlayerSelectedGenes;
            var reg = regs.Find( m => m.Name == RegulatoryGene.Name );

            if( Input.GetMouseButtonDown( 0 ) ) { // Left
                if( reg == null ) {
                    regs.Add( reg = ObjectHelper.Clone( RegulatoryGene, RegulatoryGene.GetType() ) );
                } else {
                    regs.Remove( reg );
                }
                SwitchCodingGeneList( reg );
            }
            if( Input.GetMouseButtonDown( 1 ) && GetComponent<Toggle>().isOn ) { // Right
                SwitchCodingGeneList( reg );
            }
            U3D_LabSceneLoad.FreshLength();
        }

        public void OnPointerEnter( PointerEventData eventData ) {
            U3D_LabSceneLoad.ChangeMaxLengthText(
                string.Format(
                    "Name: {0}\n" +
                    "Description: {1}\n"
                    , RegulatoryGene.Name, RegulatoryGene.Description
                ) );
        }

        public void OnPointerExit( PointerEventData eventData ) {
            U3D_LabSceneLoad.FreshLength();
        }
    }
}
