using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CellWar.View
{
    public class U3D_EditorChemicalPackageLogic : MonoBehaviour, IPointerClickHandler
    {
        public CellWar.Model.Substance.Chemical UIChemical;  

        // Start is called before the first frame update
        void Start()
        {
            gameObject.GetComponent<Text>().text = UIChemical.Name;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // 把另外一个置空
            MainGameCurrent.HoldingStrain = null;

            MainGameCurrent.HoldingChemical = UIChemical;
        }
    }

}
