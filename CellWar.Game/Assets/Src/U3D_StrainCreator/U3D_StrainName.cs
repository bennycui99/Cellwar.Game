using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Utils;
using UnityEngine;

public class U3D_StrainName : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void FinishCreating() {
        if( UIHelper.GetInputText( "UI_Strain_Name" ) == "" ) {
            U3D_CreatorSceneLoad.EmitAlert( "Please Enter Strain Name" );
            return;
        }
        if( StrainCreatorCurrent.IsLengthOverflowed ) {
            U3D_CreatorSceneLoad.EmitAlert( "The Length is Overflowed. Please Trim the Gene you carry." );
            return;
        }
        // 储存基因
    }

}
