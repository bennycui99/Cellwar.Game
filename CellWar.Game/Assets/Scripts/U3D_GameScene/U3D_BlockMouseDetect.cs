using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Map;
using UnityEngine.EventSystems;
using CellWar.GameData;
using CellWar.Model.Substance;
using UnityEngine;

namespace CellWar.View {
    public class U3D_BlockMouseDetect : MonoBehaviour
    {
        public U3D_BlockLogic ParentBlockLogic;
        public Block HexBlockModel;

        bool m_IsMouseEnter = false;

        Renderer m_BlockRenderer;

        static Color INIT_COLOR = new Color(188f / 255f, 238f / 255f, 104f / 255f, 1f);

        /// <summary>
        /// 方块人口对应颜色
        /// </summary>
        Color m_PopulationColor = INIT_COLOR;

        /// <summary>
        /// 目标颜色
        /// </summary>
        Color m_DestColor = INIT_COLOR;
        /// <summary>
        /// 当前颜色和目标差值
        /// </summary>
        const float STEP_COLOR = 2.0f;

        /// <summary>
        /// 颜色变化淡入淡出时间
        /// </summary>
        const float MAX_FADE_TIME = 0.1f;
        float m_FadeTimeCount = MAX_FADE_TIME;

        // Start is called before the first frame update
        void Start()
        {
            m_BlockRenderer = GetComponent<MeshRenderer>();
            //ChangeBlockColor(INIT_COLOR);
        }

        public void BlockColorUpdate()
        {
            //更新这块方块颜色和数量
            HexBlockModel.TotalPopulation = HexBlockModel.GetTotalPopulation();
            m_PopulationColor = GetColorAccordingToPopulation(HexBlockModel.TotalPopulation);

            if (!m_IsMouseEnter)
            {
                m_DestColor = m_PopulationColor;
            }
        }

        /// <summary>
        /// 显示颜色变化
        /// </summary>
        private void FixedUpdate()
        {
            ChangeBlockColor(Color.Lerp(GetCurrentColor(), m_DestColor, STEP_COLOR * Time.deltaTime));
        }

        /// <summary>
        /// 返回人口对应颜色,颜色值需要再修订
        /// </summary>
        /// <param name="n">人口</param>
        /// <returns></returns>
        Color GetColorAccordingToPopulation(int n)
        {
            if (n > 1000)
            {
                return new Color(0.5f, 0f, 0f, 0.5f);
            }
            if (n > 500)
            {
                return new Color(0f, 0f, 0f, 0.5f);
            }
            if (n > 100)
            {
                return new Color(1f, 0f, 0f, 0.5f);
            }
            if (n > 50)
            {
                return new Color(1f, 0.2f, 0.2f, 0.5f);
            }
            if (n > 10)
            {
                return new Color(1f, 0.4f, 0.4f, 0.5f);
            }
            
            return INIT_COLOR;
        }
        
        Color GetCurrentColor()
        {
            return m_BlockRenderer.material.color;
        }
        private void ChangeBlockColor(Color color)
        {
            if (m_BlockRenderer)
            {
                m_BlockRenderer.material.color = color;
            }
        }
        

        /// <summary>
        /// 点击方块显示方块信息
        /// </summary>
        private void OnMouseDown()
        {
            processSelectedStrain();
        }

        public void processSelectedStrain()
        {
            //Debug.Log(MainGameCurrent.HoldingStrain);
            if (MainGameCurrent.FocusedBlock != null && MainGameCurrent.HoldingStrain != null)
            {
                ChangeBlockColor(Color.yellow);
                // 防止反复增加同一种细菌
                if (!MainGameCurrent.FocusedBlock.HexBlockModel.Strains.Exists(m => m.Name == MainGameCurrent.HoldingStrain.Name))
                {
                    MainGameCurrent.FocusedBlock.HexBlockModel.Strains.Add(MainGameCurrent.HoldingStrain);
                }
                GameObject.Find(MainGameCurrent.HoldingStrain.Name).SetActive(false);
                MainGameCurrent.HoldingStrain = null;
            }
        }

        private void OnMouseEnter()
        {
            //Debug.Log("Mouse Enter");
            m_IsMouseEnter = true;
            MainGameCurrent.FocusedBlock = ParentBlockLogic;
            
            if (MainGameCurrent.HoldingStrain != null)
            {
                /// 当手里拿着细菌准备放置时的代码
                m_DestColor = Color.green;
            }
            else
            {
                /// 手里什么都没有拿时鼠标移动到格子上的代码
                m_DestColor = Color.blue;
            }
            
        }

        private void OnMouseExit()
        {
            m_IsMouseEnter = false;
            MainGameCurrent.FocusedBlock = null;

            m_DestColor = m_PopulationColor;
        }

    }
}