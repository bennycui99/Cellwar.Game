using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Model.Substance;
using CellWar.Utils;
using CellWar.Utils.Object;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CellWar.View {
    public class U3D_StrainElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
        public Strain Strain { get; set; }

        public void LoadStrainInfo() {
            LabCurrent.Strain = GetComponent<Toggle>().isOn ? ObjectHelper.Clone( Strain ) : null;
            if( LabCurrent.Strain == null ) {
                return;
            }

            LabCurrent.RegulatoryGene = Strain.PlayerSelectedGenes.Count != 0 ? Strain.PlayerSelectedGenes[0] : null;
            // 勾选race
            UIHelper.SwitchToggle( LabCurrent.Strain.BasicRace.Name, true );
            // 勾选所有的reg
            foreach( Transform reg in GameObject.Find( "UI_RegList" ).transform ) {
                reg.gameObject.GetComponent<Toggle>().isOn = LabCurrent.Strain.PlayerSelectedGenes.Exists( m => m.Name == reg.gameObject.name );
            }
            // 刷新长度提示
            U3D_CreatorSceneLoad.FreshLength();
            // 替换input名字
            UIHelper.SetInputText( "UI_Strain_Name", LabCurrent.Strain.Name );
        }

        private void Start() {
            transform.Find( "Label" ).GetComponent<Text>().text = Strain.Name;
        }


        public void OnPointerEnter( PointerEventData eventData ) {
            U3D_CreatorSceneLoad.ChangeMaxLengthText(
                string.Format(
                    "Name: {0}\n" +
                    "Description: {1}\n"
                    , Strain.Name, ""
                ) );
        }

        public void OnPointerExit( PointerEventData eventData ) {
            U3D_CreatorSceneLoad.FreshLength();
        }

        public void OnPointerDown( PointerEventData eventData ) {
            if( Input.GetMouseButtonDown( 1 ) ) { // Right
                U3D_AlertPanel.EmitAlert( "Do you want to delete the strain?",
                    () => {
                        Save.Strains.Remove( Strain );
                        gameObject.SetActive( false );
                        Save.SaveAllStrains();
                        U3D_CreatorSceneLoad.RefreshStrainList();
                    },
                    () => { } );
            }
        }
    }
}
