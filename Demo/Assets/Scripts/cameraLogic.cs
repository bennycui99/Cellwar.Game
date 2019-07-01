using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraLogic : MonoBehaviour
{
    [SerializeField]
    float cameraMove = 0.5f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Input.mousePosition.x>=Screen.width-10)
        {
            
            Camera.main.transform.position=new Vector3(Camera.main.transform.position.x+ cameraMove, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        else if(Input.mousePosition.x <= 10)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - cameraMove, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        if (Input.mousePosition.y >= Screen.height-10)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x , Camera.main.transform.position.y, Camera.main.transform.position.z + cameraMove);
        }
        else if (Input.mousePosition.y <= 10)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x , Camera.main.transform.position.y, Camera.main.transform.position.z - cameraMove);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 1, Camera.main.transform.position.z);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 1, Camera.main.transform.position.z);
        }


    }
}
