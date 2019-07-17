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

public class U3D_StrainPackageLogic : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

    public Strain Strain { get; set; } = CellWar.Mock.Mocks.MockStrainForStrainUI;

    public void OnPointerDown( PointerEventData eventData ) {
        Current.HoldingStrain = Strain;
        Debug.Log( "Dragging...." );
    }

    public void OnPointerUp( PointerEventData eventData ) {
        if( Current.FocusedBlock != null ) {
            Current.FocusedBlock.ChangeBlockColor( Color.yellow );
            Current.FocusedBlock.HexBlockModel.Strains.Add( Current.HoldingStrain );
            Debug.Log( "Deployed" );
        }
        Current.HoldingStrain = null;
        Debug.Log( "MouseUp....." );
    }

    // Start is called before the first frame update
    void Start() {
        gameObject.GetComponent<Text>().text = "Strain: " + Strain.Name;
    }

    // Update is called once per frame
    void Update() {
    }
}
