using UnityEngine;
using CellWar.Model.Map;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using CellWar.GameData;

public class U3D_BlockLogic : MonoBehaviour {

    public Block HexBlockModel { get; set; }

    public void ChangeBlockColor( Color color ) {
        foreach( Transform tran in GetComponentsInChildren<Transform>() ) {//遍历当前物体及其所有子物体
            tran.gameObject.GetComponent<Renderer>().material.color = color;//更改物体的Layer层
        }
    }

    private void OnTriggerEnter( Collider other ) {
        HexBlockModel.FetchNeighborBlocksFromMap_OnTriggerEnter( other, MapLogic.basicSceneMap );
    }


    // Start is called before the first frame update
    void Start() {
        HexBlockModel = MapLogic.basicSceneMap.FindBlockFromGameObjectName( gameObject.name );
    }

    /// <summary>
    /// 点击方块显示方块信息
    /// </summary>
    private void OnMouseDown() {
        Debug.Log( "Clicked" + gameObject.name );
    }

    private void OnMouseEnter() {
        Current.FocusedBlock = this;
        if( Current.HoldingStrain != null ) {
            ChangeBlockColor( Color.red );
        } else {
            ChangeBlockColor( Color.blue );
        }
    }

    private void OnMouseExit() {
        Current.FocusedBlock = null;
        if( HexBlockModel.Strains.Count != 0 ) {
            ChangeBlockColor( Color.yellow );
        } else {
            ChangeBlockColor( Color.white );
        }
    }
}
