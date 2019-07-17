﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class U3D_CameraLogic : MonoBehaviour {
    float cameraMove = CellWar.Model.Settings.Camera.MoveSpeed;

    // Update is called once per frame
    void Update() {
        Vector3 newPosition = transform.position;

        if( Input.mousePosition.x >= Screen.width - 10 ) {
            newPosition = new Vector3( Camera.main.transform.position.x + cameraMove, Camera.main.transform.position.y, Camera.main.transform.position.z );
        } else if( Input.mousePosition.x <= 10 ) {
            newPosition = new Vector3( Camera.main.transform.position.x - cameraMove, Camera.main.transform.position.y, Camera.main.transform.position.z );
        }
        if( Input.mousePosition.y >= Screen.height - 10 ) {
            newPosition = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + cameraMove );
        } else if( Input.mousePosition.y <= 10 ) {
            newPosition = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - cameraMove );
        }

        /// 处理鼠标滚轮
        if( Input.GetAxis( "Mouse ScrollWheel" ) > 0 ) {
            newPosition = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y - 1, Camera.main.transform.position.z );
        }
        if( Input.GetAxis( "Mouse ScrollWheel" ) < 0 ) {
            newPosition = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y + 1, Camera.main.transform.position.z );
        }

        transform.position = Vector3.Lerp( transform.position, newPosition, cameraMove );

    }
}
