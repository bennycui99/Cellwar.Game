/// <summary>
/// 一个UI_Strain对应一个Strain实体
/// </summary>
using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Model.Substance;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace CellWar.View {
    public class U3D_StrainPackageLogic : MonoBehaviour, IPointerClickHandler {

        public Strain Strain { get; set; }

        public void OnPointerClick( PointerEventData eventData ) {
            MainGameCurrent.HoldingStrain = Strain;
        }

        // Start is called before the first frame update
        void Start() {
            gameObject.GetComponent<Text>().text = Strain.Name;
        }

        /*
        // Update is called once per frame
        void Update() {
        }
        */
    }
}
