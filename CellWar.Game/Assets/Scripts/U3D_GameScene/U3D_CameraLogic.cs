using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Map;
using CellWar.GameData;
using UnityEngine;

namespace CellWar.View {

    public class U3D_CameraLogic : MonoBehaviour {
        public Map StageMap;

        /// <summary>
        /// 镜头滚动速度
        /// </summary>
        const float CAMERA_SCROLL_SPEED = 0.5f;

        float m_CameraXMin = 1000.0f, m_CameraXMax = -1000.0f;
        float m_CameraZMin = 1000.0f, m_CameraZMax = -1000.0f;

        /// <summary>
        /// 边界距离
        /// </summary>
        const float EDGE_OFFSET = 5.0f;

        void Start()
        {
            StageMap = MainGameCurrent.StageMap;
            // 取得地图边界
            for (int i=0;i< StageMap.Blocks.Count; ++i)
            {
                if (m_CameraXMin > StageMap.Blocks[i].StandardCoor.X)
                {
                    m_CameraXMin = StageMap.Blocks[i].StandardCoor.X;
                }
                if (m_CameraXMax < StageMap.Blocks[i].StandardCoor.X)
                {
                    m_CameraXMax = StageMap.Blocks[i].StandardCoor.X;
                }

                if (m_CameraZMin > StageMap.Blocks[i].StandardCoor.Z)
                {
                    m_CameraZMin = StageMap.Blocks[i].StandardCoor.Z;
                }
                if (m_CameraZMax < StageMap.Blocks[i].StandardCoor.Z)
                {
                    m_CameraZMax = StageMap.Blocks[i].StandardCoor.Z;
                }
            }

            // 设置Camera边界距离（比地图格子边界要多一些）
            m_CameraXMin -= EDGE_OFFSET/2;
            m_CameraXMax += EDGE_OFFSET/2;
            m_CameraZMin -= EDGE_OFFSET;
            //ZMax 不需要增加offset,反而要减
            m_CameraZMax -= EDGE_OFFSET;

            Debug.Log(m_CameraXMax);
        }

        // Update is called once per frame
        void Update() {

            // 移动镜头,并控制镜头边界
            if (Input.mousePosition.x >= Screen.width && Camera.main.transform.position.x <= m_CameraXMax)
            {
                // Move Camera Right
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + CAMERA_SCROLL_SPEED, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
            else if (Input.mousePosition.x <= 0 && Camera.main.transform.position.x >= m_CameraXMin)
            {
                // Move Camera Left
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - CAMERA_SCROLL_SPEED, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }

            if (Input.mousePosition.y >= Screen.height && Camera.main.transform.position.z <= m_CameraZMax)
            {
                // Move Camera Down
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + CAMERA_SCROLL_SPEED);
            }
            else if (Input.mousePosition.y <= 0 && Camera.main.transform.position.z >= m_CameraZMin)
            {
                // Move Camera Up
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - CAMERA_SCROLL_SPEED);
            }

        }
    }
}