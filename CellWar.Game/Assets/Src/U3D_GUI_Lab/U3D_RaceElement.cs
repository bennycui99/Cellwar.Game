using System.Collections;
using System.Collections.Generic;
using CellWar.Contoller;
using CellWar.GameData;
using CellWar.Utils;
using UnityEngine;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;

namespace CellWar.View {
    public class U3D_RaceElement : MonoBehaviour {
        public Race Race { get; set; }

        private void Start() {
            GetComponent<Toggle>().isOn = false;
            UIHelper.ChangeText( transform.Find( "Label" ).gameObject, Race.Name );
        }

        /// <summary>
        /// 当value on change时调用
        /// </summary>
        public void ChangeText() {
            LabCurrent.Strain.BasicRace = this.Race;
            U3D_CreatorSceneLoad.FreshLength();
        }
    }
}