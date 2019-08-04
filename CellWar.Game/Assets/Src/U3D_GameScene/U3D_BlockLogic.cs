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

        Color m_CurrentColor;
        Color m_DestColor;
        Color m_PopulationColor;

        Renderer m_Renderer;

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
            m_Renderer = GetComponent<Renderer>();
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
            //m_PopulationColor =
            HexBlockModel.TotalPopulation = HexBlockModel.GetTotalPopulation();
        }

        /// <summary>
        /// 显示颜色变化
        /// </summary>
        private void FixedUpdate()
        
            // if the block is selected by mouse, change its corlor
            if (m_IsMouseEnter)
            {
                
            }
            // else change its corlor by population
            else
            {
                UpdateBlockColor();
            }
        }

        void UpdateBlockColor()
        {

        }

        void GetCurrentColor()
        {

        }

        private void ChangeBlockColor( Color color ) {
            foreach( Transform tran in GetComponentsInChildren<Transform>() ) {
                tran.gameObject.GetComponent<Renderer>().material.color = color;
            }
        }

        private void printColorWithPopulation() {
            var number = HexBlockModel.TotalPopulation;
            if( number > 10 ) {
                ChangeBlockColor( new Color( 1f, 0.4f, 0.4f, 0.5f ) );
            }
            if( number > 50 ) {
                ChangeBlockColor( new Color( 1f, 0.2f, 0.2f, 0.5f ) );
            }
            if( number > 100 ) {
                ChangeBlockColor( new Color( 1f, 0f, 0f, 0.5f ) );
            }
            if( number > 500 ) {
                ChangeBlockColor( new Color( 0.8f, 0f, 0f, 0.5f ) );
            }
            if( number > 1000 ) {
                ChangeBlockColor( new Color( 0.5f, 0f, 0f, 0.5f ) );
            }
        }

        private void printColorWithPopulation2() {
            float dr = populationDelta * 0.5f / HexBlockModel.Capacity;

            if( populationDelta > 0 ) {
                color.r = ( color.r - dr ) < 0.5f ? 0.5f : color.r - dr;
                color.g = ( color.g - dr ) < 0f ? 0f : color.g - dr;
                color.b = ( color.b - dr ) < 0f ? 0f : color.b - dr;
            } else {
                color.r = ( color.r + dr ) > 1f ? 1f : color.r + dr;
                color.g = ( color.g + dr ) > 0.5f ? 0.5f : color.g + dr;
                color.b = ( color.b + dr ) > 0.5f ? 0.5f : color.b + dr;
            }
            ChangeBlockColor( color );
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

        }

        private void ChangeMouseEnterColor() {
            MainGameCurrent.FocusedBlock = this;
            /*
            if( MainGameCurrent.HoldingStrain != null ) {
                /// TODO: 当手里拿着细菌准备放置时的代码
                ChangeBlockColor( Color.green );
            } else {
                /// TODO: 手里什么都没有拿时鼠标移动到格子上的代码
                ChangeBlockColor( Color.blue );
            }
            */
        }

        private void OnMouseExit() {
            m_IsMouseEnter = false;
            MainGameCurrent.FocusedBlock = null;
        }
    }
}
