using System.Collections;
using System.Collections.Generic;
using CellWar.Utils;
using UnityEngine;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;

public class U3D_RaceElement : MonoBehaviour {
    public Race Race { get; set; }

    private void Start() {
        GetComponent<Toggle>().isOn = false;
        UIHelper.ChangeText( transform.Find( "Label" ).gameObject, Race.Name );
    }

    public void ChangeText() {
        GameObject.Find( "UI_MaxLength" ).GetComponent<Text>().text =
            "Max Length: " + Race.MaxLength.ToString();
    }
}
