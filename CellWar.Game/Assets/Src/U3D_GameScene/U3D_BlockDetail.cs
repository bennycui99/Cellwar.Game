using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using UnityEngine;
using UnityEngine.UI;

public class U3D_BlockDetail : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        gameObject.GetComponent<Text>().text = "Game Start!";
    }

    // Update is called once per frame
    void Update() {
        gameObject.GetComponent<Text>().text = MainGameCurrent.GetCurrentBlockDetailInfo();
    }
}
