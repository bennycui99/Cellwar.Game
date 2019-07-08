using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HexAutotile
{
    public class CameraBorders : MonoBehaviour, ITerrainObserver
    {
        public BaseHexTerrain baseHexTerrain;
        public PixelArtCamera pixelArtCamera;

        void Start ()
        {
            baseHexTerrain = FindObjectOfType<BaseHexTerrain>();
            baseHexTerrain.AddObserver(this);

            pixelArtCamera = Camera.main.GetComponent<PixelArtCamera>();
        }

        private float prevScale = -1f;

        void CheckPixelArtScaleChaned ()
        {
            float scale = pixelArtCamera.scale;
            if (prevScale - scale != 0f)
            {
                OnResizeTerrain();
                prevScale = scale;
            }            
        }

        void FixedUpdate ()
        {
            CheckPixelArtScaleChaned();
        }

        void PositionGrid ()
        {
            Camera camera = Camera.main;
            camera.transform.localPosition = Vector3.zero;

            PCCameraControl cameraController = camera.GetComponent<PCCameraControl>();

            Vector3 p = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));

            //GameObject gm = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //gm.transform.position = p;
            //gm.transform.parent = camera.transform;

            float xoffset = Mathf.Abs(p.x);
            float yoffset = Mathf.Abs(p.y);

            cameraController.minPosX = xMinPosition + xoffset;
            cameraController.minPosY = yMinPosition - yoffset;
            cameraController.maxPosX = xMaxPosition - xoffset;
            cameraController.maxPosY = yMaxPosition + yoffset;

            if (cameraController.minPosX > cameraController.maxPosX)
            {
                float temp = cameraController.minPosX;
                cameraController.minPosX = cameraController.maxPosX;
                cameraController.maxPosX = temp;
            }

            if (cameraController.minPosY > cameraController.maxPosY)
            {
                float temp = cameraController.minPosY;
                cameraController.minPosY = cameraController.maxPosY;
                cameraController.maxPosY = temp;
            }
        }

        public void OnRedraw()
        {
        }

        public void OnResizeTerrain()
        {
            PositionGrid();
        }

        float xMinPosition
        {
            get
            {
                return baseHexTerrain.xMinPosition;
            }
        }

        float yMinPosition
        {
            get
            {
                return baseHexTerrain.yMinPosition;
            }
        }

        float xMaxPosition
        {
            get
            {
                return baseHexTerrain.xMaxPosition;
            }
        }

        float yMaxPosition
        {
            get
            {
                return baseHexTerrain.yMaxPosition;
            }
        }
    }
}
