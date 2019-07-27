using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;

public class U3D_RaceElement : MonoBehaviour {
    public Race Race { get; set; }

    private void Start() {
        gameObject.GetComponent<Text>().text = Race.Name;
    }
}
