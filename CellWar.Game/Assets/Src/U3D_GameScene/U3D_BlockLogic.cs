using UnityEngine;
using CellWar.Model.Map;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using CellWar.GameData;
using CellWar.Model.Substance;

namespace CellWar.View {
    public class U3D_BlockLogic : MonoBehaviour {
        const float MAX_UPDATE_COUNT = 1.0f;
        float m_UpdateCount = MAX_UPDATE_COUNT;

        /// <summary>
        /// 目标颜色
        /// </summary>
        Color m_DestColor;
        /// <summary>
        /// 方块人口对应颜色
        /// </summary>
        Color m_PopulationColor;
        /// <summary>
        /// 当前颜色和目标差值
        /// </summary>
        const float STEP_COLOR = 1.0f;

        /// <summary>
        /// 颜色变化淡入淡出时间
        /// </summary>
        const float MAX_FADE_TIME = 0.1f;
        float m_FadeTimeCount = MAX_FADE_TIME;

        Renderer m_BlockRenderer;

        bool m_IsMouseEnter = false;

        public Block HexBlockModel;

        /// <summary>
        /// 初始化这个Block
        /// </summary>
        void Awake()
        {
            HexBlockModel = new Block();
            HexBlockModel.ParentUnityObjectName = gameObject.name;
            HexBlockModel.Capacity = 1000;
        }

        private void Start()
        {
            m_BlockRenderer = transform.Find("Block").gameObject.GetComponent<Renderer>();
        }

        private void Update()
        {
            // 每隔一秒更新一次
            if (m_UpdateCount > 0)
            {
                m_UpdateCount -= Time.deltaTime;
                return;
            }
            else
            {
                m_UpdateCount = MAX_UPDATE_COUNT;
            }

            // 遍历所有种类细菌
            for(var i = 0; i < HexBlockModel.Strains.Count; ++i)
            {
                Strain currentStrain = HexBlockModel.Strains[i];
                // 遍历该菌的所有基因
                foreach (var reg in HexBlockModel.Strains[i].PlayerSelectedGenes)
                {
                    // 检查基因条件
                    if (MainGameCurrent.RegCtor.IsTriggered(HexBlockModel.PublicChemicals, reg))
                    {
                        for (int j = 0; j < reg.DominatedGenes.Count; j++)
                        {
                            var cod = reg.DominatedGenes[j];
                            // 细菌活动
                            MainGameCurrent.CodCtor.Effect(ref currentStrain, ref HexBlockModel, ref cod);
                        }
                    }
                }
                //更新该菌种
                HexBlockModel.Strains[i] = currentStrain;
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
            if (n > 10)
            {
                return new Color(1f, 0.4f, 0.4f, 0.5f);
            }
            if (n > 50)
            {
                return new Color(1f, 0.2f, 0.2f, 0.5f);
            }
            if (n > 100)
            {
                return new Color(1f, 0f, 0f, 0.5f);
            }
            if (n > 500)
            {
                return new Color(0.8f, 0f, 0f, 0.5f);
            }
            if (n > 1000)
            {
                return new Color(0.5f, 0f, 0f, 0.5f);
            }
            
            return new Color(1f, 0.4f, 0.4f, 0.5f);
        }

        Color GetCurrentColor()
        {
            return m_BlockRenderer.material.color;
        }

        private void ChangeBlockColor( Color color ) {
            if (m_BlockRenderer)
            {
                m_BlockRenderer.material.color = color;
            }
        }

        /// <summary>
        /// 点击方块显示方块信息
        /// </summary>
        private void OnMouseDown() {
            processSelectedStrain();
        }

        public void processSelectedStrain() {
            if( MainGameCurrent.FocusedBlock != null && MainGameCurrent.HoldingStrain != null ) {
                MainGameCurrent.FocusedBlock.ChangeBlockColor( Color.yellow );
                // 防止反复增加同一种细菌
                if( !MainGameCurrent.FocusedBlock.HexBlockModel.Strains.Exists( m => m.Name == MainGameCurrent.HoldingStrain.Name ) ) {
                    MainGameCurrent.FocusedBlock.HexBlockModel.Strains.Add( MainGameCurrent.HoldingStrain );
                }
                GameObject.Find( MainGameCurrent.HoldingStrain.Name ).SetActive( false );
                MainGameCurrent.HoldingStrain = null;
            }
        }

        private void OnMouseEnter() {
            m_IsMouseEnter = true;
            MainGameCurrent.FocusedBlock = this;
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

        private void OnMouseExit() {
            m_IsMouseEnter = false;
            MainGameCurrent.FocusedBlock = null;

            m_DestColor = m_PopulationColor;
        }
    }
}
