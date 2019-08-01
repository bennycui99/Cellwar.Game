using System.Collections;
using System.Collections.Generic;
using CellWar.Controller;
using CellWar.GameData;
using CellWar.Utils;
using CellWar.Utils.Object;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;

namespace CellWar.View {
    public class U3D_RaceElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        public Race Race { get; set; }

        private void Start() {
            GetComponent<Toggle>().isOn = LabCurrent.Strain.BasicRace.Name == Race.Name;
            UIHelper.ChangeText( transform.Find( "Label" ).gameObject, Race.Name );
        }

        /// <summary>
        /// 当value on change时调用
        /// </summary>
        public void ChangeText() {
            LabCurrent.Strain.BasicRace = Race.Clone();
            U3D_CreatorSceneLoad.FreshLength();
        }

        public void OnPointerEnter( PointerEventData eventData ) {
            U3D_CreatorSceneLoad.ChangeMaxLengthText(
                string.Format(
                    "Name: {0}\n" +
                    "Description: {1}\n"
                    , Race.Name, Race.Description
                ) );
        }

        public void OnPointerExit( PointerEventData eventData ) {
            U3D_CreatorSceneLoad.FreshLength();
        }
    }
}