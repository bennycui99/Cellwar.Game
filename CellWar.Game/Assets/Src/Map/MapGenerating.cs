using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerating : MonoBehaviour
{
    GameObject HexUnit;
    // Start is called before the first frame update
    void Start()
    {
        HexUnit = Resources.Load( "HexUnit" ) as GameObject;

        for( int i = 0; i < 4; i++ ) {

        GameObject instance = Instantiate( HexUnit );

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
