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
        UnityEngine.EventSystems.EventSystem m_EventSystem;
        GraphicRaycaster m_GraphicRaycaster;

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
            m_EventSystem = GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>();
            m_GraphicRaycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();

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
            if (n > 200)
            {
                return new Color(0.5f, 0f, 0f, 0.5f);
            }
            if (n > 100)
            {
                return new Color(0f, 0f, 0f, 0.5f);
            }
            if (n > 40)
            {
                return new Color(1f, 0f, 0f, 0.5f);
            }
            if (n > 20)
            {
                return new Color(1f, 0.2f, 0.2f, 0.5f);
            }
            if (n > 5)
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
            if (Local.CheckGuiRaycastObjects(m_EventSystem, m_GraphicRaycaster)) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (MainGameCurrent.HoldingChemical == null && MainGameCurrent.HoldingStrain == null && HexBlockModel.IsActive)
                {
                    HexBlockModel.Capacity += 1000;
                    if (HexBlockModel.Capacity == 10000)
                    {
                        HexBlockModel.Capacity = 1000;
                    }
                }
            }

            // 0 1 2 左键 右键 中键
            if (Input.GetMouseButton(0))
            {
                if (MainGameCurrent.HoldingStrain == null && MainGameCurrent.HoldingChemical == null && !HexBlockModel.IsActive)
                {
                    m_BlockRenderer.enabled = true;
                    HexBlockModel.IsActive = true;
                }
            }
            else if (Input.GetMouseButton(1))
            {
                if (MainGameCurrent.HoldingStrain == null && MainGameCurrent.HoldingChemical == null && HexBlockModel.IsActive)
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
                if (HexBlockModel.IsActive && HexBlockModel.PublicChemicals.Count > 0)
                {
                    HexBlockModel.PublicChemicals.RemoveAt(HexBlockModel.PublicChemicals.Count - 1);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (MainGameCurrent.HoldingStrain != null && HexBlockModel.IsActive)
                {
                    ProcessSelectedStrain();
                }

                if (MainGameCurrent.HoldingChemical != null && HexBlockModel.IsActive)
                {
                    ProcessSelectedChemical();
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (MainGameCurrent.HoldingStrain != null)
                {
                    MainGameCurrent.HoldingStrain = null;
                }

                if (MainGameCurrent.HoldingChemical != null)
                {
                    MainGameCurrent.HoldingChemical = null;
                }
            }   
        }

        public void ProcessSelectedStrain()
        {
            //Debug.Log(MainGameCurrent.HoldingStrain);
            ChangeBlockColor(Color.yellow);
            // 防止反复增加同一种细菌; 同一种叠加数量
            if (!HexBlockModel.Strains.Exists(m => m.Name == MainGameCurrent.HoldingStrain.Name)) 
            {
                //深拷贝
                HexBlockModel.Strains.Add((CellWar.Model.Substance.Strain)MainGameCurrent.HoldingStrain.Clone());
                HexBlockModel.Strains[HexBlockModel.Strains.Count - 1].Population = MainGameCurrent.HoldingStrain.Population;
            }
            else
            {
                HexBlockModel.Strains.Find(m => m.Name == MainGameCurrent.HoldingStrain.Name).Population += MainGameCurrent.HoldingStrain.Population;
            }
            //MainGameCurrent.HoldingStrain = null;
        }

        public void ProcessSelectedChemical()
        {
            ChangeBlockColor(Color.black);

            // 防止反复增加同一种化学物质; 同一种叠加数量
            if (!HexBlockModel.PublicChemicals.Exists(m => m.Name == MainGameCurrent.HoldingChemical.Name))
            {
                //深拷贝
                HexBlockModel.PublicChemicals.Add((CellWar.Model.Substance.Chemical)MainGameCurrent.HoldingChemical.Clone());
            }
            else
            {
                HexBlockModel.PublicChemicals.Find(m => m.Name == MainGameCurrent.HoldingChemical.Name).Count += MainGameCurrent.HoldingChemical.Count;
            }
            //MainGameCurrent.HoldingChemical = null;
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
            else if (MainGameCurrent.HoldingChemical != null)
            {
                // 拿着chemical
                m_DestColor = Color.grey;
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
