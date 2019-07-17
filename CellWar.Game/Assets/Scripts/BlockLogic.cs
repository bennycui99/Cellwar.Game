using UnityEngine;
using CellWar.Model.Map;
using System.Collections.Generic;

public class BlockLogic : MonoBehaviour {

    Block mHexBlock;

    private void OnTriggerEnter( Collider other ) {
        mHexBlock.FetchNeighborBlocksFromMap_OnTriggerEnter( other, MapLogic.basicSceneMap );
    }


    // Start is called before the first frame update
    void Start() {
        mHexBlock = MapLogic.basicSceneMap.FindBlockFromGameObjectName( gameObject.name );
    }

    /// <summary>
    /// 点击方块显示方块信息
    /// </summary>
    private void OnMouseDown() {
        Debug.Log("Clicked" + gameObject.name);
    }

    // Update is called once per frame
    void Update() {

    }
}
