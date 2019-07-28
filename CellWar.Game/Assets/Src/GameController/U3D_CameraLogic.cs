﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellWar.View {

    public class U3D_CameraLogic : MonoBehaviour {
        float cameraMove = CellWar.Model.Settings.Camera.MoveSpeed;
        [SerializeField]
        float CameraXMax = CellWar.Model.Settings.Camera.XMax;

        [SerializeField]
        float CameraXMin = CellWar.Model.Settings.Camera.XMin;

        [SerializeField]
        float CameraYMax = CellWar.Model.Settings.Camera.YMax;

        [SerializeField]
        float CameraYMin = CellWar.Model.Settings.Camera.YMin;

        [SerializeField]
        float CameraZMax = CellWar.Model.Settings.Camera.ZMax;

        [SerializeField]
        float CameraZMin = CellWar.Model.Settings.Camera.ZMin;

        // Update is called once per frame
        void Update() {
            Vector3 newPosition = transform.position;

            if( Input.mousePosition.x >= Screen.width - 10 && Camera.main.transform.position.x <= CameraXMax ) {
                newPosition = new Vector3( Camera.main.transform.position.x + cameraMove, Camera.main.transform.position.y, Camera.main.transform.position.z );
            } else if( Input.mousePosition.x <= 10 && Camera.main.transform.position.x >= CameraXMin ) {
                newPosition = new Vector3( Camera.main.transform.position.x - cameraMove, Camera.main.transform.position.y, Camera.main.transform.position.z );
            }
            if( Input.mousePosition.y >= Screen.height - 10 && Camera.main.transform.position.y <= CameraYMax ) {
                newPosition = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + cameraMove );
            } else if( Input.mousePosition.y <= 10 && Camera.main.transform.position.z >= CameraYMin ) {
                newPosition = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - cameraMove );
            }

            /// 处理鼠标滚轮
            if( Input.GetAxis( "Mouse ScrollWheel" ) > 0 && Camera.main.transform.position.y >= CameraZMin ) {
                newPosition = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y - 1, Camera.main.transform.position.z );
            }
            if( Input.GetAxis( "Mouse ScrollWheel" ) < 0 && Camera.main.transform.position.y <= CameraZMax ) {
                newPosition = new Vector3( Camera.main.transform.position.x, Camera.main.transform.position.y + 1, Camera.main.transform.position.z );
            }

            transform.position = Vector3.Lerp( transform.position, newPosition, cameraMove );

        }
    }
}