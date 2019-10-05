using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CellWar.Test.Mock;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Unit.TestRegGeneCondition_NO();
        Unit.TestRegGeneCondition_PA();

        Debug.Log("Finished Test. Reg gene condition");
        Unit.TestPrivateChemical_Usable();
        Unit.TestDeathGeneImportChemical_Usable();
        Debug.Log( "Finished Test. Priavte chemical usable" );

        Unit.TestGameOver();
        Debug.Log( "Finished Game Over" );
    }

    // Update is called once per frame
    void Update()
    {
    }
}
