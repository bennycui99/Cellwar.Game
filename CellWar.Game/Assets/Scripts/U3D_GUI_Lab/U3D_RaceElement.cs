using CellWar.GameData;
using CellWar.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;

namespace CellWar.View
{
    public class U3D_RaceElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public Race Race { get; set; }

        private void Start() {
            UIHelper.ChangeText( transform.Find( "Label" ).gameObject, Race.Name );
            GetComponent<Toggle>().isOn = LabCurrent.Strain.BasicRace.Name == Race.Name;
        }

        /// <summary>
        /// 当value on change时调用
        /// </summary>       
        public void ChangeText() {
            if( Race == null || LabCurrent.Strain == null ) {
                LabCurrent.MakeStrainNotBeNull();
                return;
            }
            try {
                LabCurrent.Strain.BasicRace = Race.Clone();
                U3D_LabSceneLoad.FreshLength();
            }
            catch( System.Exception ex ) {
                Debug.Log(ex);
            }
        }

        public void OnPointerEnter( PointerEventData eventData ) {
            U3D_LabSceneLoad.ChangeMaxLengthText(
                string.Format(
                    "Name: {0}\n" +
                    "Description: {1}\n"
                    , Race.Name, Race.Description
                ) );
        }

        public void OnPointerExit( PointerEventData eventData ) {
            U3D_LabSceneLoad.FreshLength();
        }
    }
}