using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CellWar.Model.Map;
using CellWar.GameData;
using UnityEngine.UI;

namespace CellWar.View
{
    public class U3D_EditorBlockMouseDetect : MonoBehaviour
    {
        public Block HexBlockModel;

        bool m_IsMouseEnter = false;

        Renderer m_BlockRenderer;

        static Color INIT_COLOR = new Color(190f / 255f, 190f / 255f, 190f / 255f, 0f);
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

        [SerializeField]
        GameObject CoorTextObject;

        // Start is called before the first frame update
        void Start()
        {
            m_BlockRenderer = GetComponent<MeshRenderer>();
            m_BlockRenderer.enabled = false;

            CoorTextObject.GetComponent<TextMesh>().text = HexBlockModel.HexCoor.X.ToString() + "   " + HexBlockModel.HexCoor.Z.ToString();
        }

        public void Update()
        {
            if (!HexBlockModel.IsActive)
            {
                return;
            }
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
        /// 左键激活/放置细菌 右键关闭/取消手上细菌
        /// Z键撤销上次的放置细菌 X撤销上次放置的化学物质
        /// </summary>
        private void OnMouseOver()
        {
            // 0 1 2 左键 右键 中键
            if (Input.GetMouseButton(0))
            {
                if (MainGameCurrent.HoldingStrain == null && !HexBlockModel.IsActive)
                {
                    m_BlockRenderer.enabled = true;
                    HexBlockModel.IsActive = true;
                }
                else if (MainGameCurrent.HoldingStrain != null && HexBlockModel.IsActive)
                {
                    ProcessSelectedStrain();
                }
            }
            else if (Input.GetMouseButton(1))
            {
                if (MainGameCurrent.HoldingStrain != null)
                {
                    MainGameCurrent.HoldingStrain = null;
                }
                else if (MainGameCurrent.HoldingStrain == null && HexBlockModel.IsActive)
                {
                    m_BlockRenderer.enabled = false;
                    HexBlockModel.IsActive = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                if (HexBlockModel.IsActive && HexBlockModel.Strains.Count > 0)
                {
                    HexBlockModel.Strains.RemoveAt(HexBlockModel.Strains.Count - 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {

            }
        }

        public void ProcessSelectedStrain()
        {
            //Debug.Log(MainGameCurrent.HoldingStrain);
            ChangeBlockColor(Color.yellow);
            // 防止反复增加同一种细菌; 同一种叠加数量
            if (!HexBlockModel.Strains.Exists(m => m.Name == MainGameCurrent.HoldingStrain.Name)) 
            {
                HexBlockModel.Strains.Add(MainGameCurrent.HoldingStrain);
            }
            else
            {
                HexBlockModel.Strains.Find(m => m.Name == MainGameCurrent.HoldingStrain.Name).Population += MainGameCurrent.HoldingStrain.Population;
            }
            MainGameCurrent.HoldingStrain = null;
        }

        public void ProcessSelectedChemical()
        {
            ChangeBlockColor(Color.yellow);

            // 防止反复增加同一种化学物质; 同一种叠加数量
            if (!HexBlockModel.PublicChemicals.Exists(m => m.Name == MainGameCurrent.HoldingChemical.Name))
            {
                HexBlockModel.PublicChemicals.Add(MainGameCurrent.HoldingChemical);
            }
            else
            {
                HexBlockModel.PublicChemicals.Find(m => m.Name == MainGameCurrent.HoldingChemical.Name).Count += MainGameCurrent.HoldingChemical.Count;
            }
            MainGameCurrent.HoldingStrain = null;
        }

        private void OnMouseEnter()
        {
            //Debug.Log("Mouse Enter");
            m_IsMouseEnter = true;
            MainGameCurrent.FocusedHexBlock = HexBlockModel;

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
            MainGameCurrent.FocusedHexBlock = null;

            m_DestColor = m_PopulationColor;
        }
    }
}
