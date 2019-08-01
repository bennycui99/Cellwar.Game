using UnityEngine;
using CellWar.Model.Map;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using CellWar.GameData;

namespace CellWar.View {
    public class U3D_BlockLogic : MonoBehaviour {
        private float mSec = 0;
        private float mColorChangeSec = 0;
        private float populationDelta = 0;
        private Color color = new Color( 1f, 0.5f, 0.5f, 0.5f );
        public Block HexBlockModel;
        bool isMouseEnter = false;
        public void ChangeBlockColor( Color color ) {
            foreach( Transform tran in GetComponentsInChildren<Transform>() ) {//遍历当前物体及其所有子物体
                tran.gameObject.GetComponent<Renderer>().material.color = color;//更改物体的Layer层
            }
        }

        private void printColorWithPopulation() {
            var number = MainGameCurrent.BlockController.GetTotalPopulation( HexBlockModel );
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
        private void Update() {
            MainGameCurrent.MainGameController.UpdateByInterval(
                () => {
                    int previousPopulation = MainGameCurrent.BlockController.GetTotalPopulation( HexBlockModel );
                    for( var i = 0; i < HexBlockModel.Strains.Count; ++i ) {
                        HexBlockModel.Strains[i] = MainGameCurrent.MainGameController.StrainWork( HexBlockModel.Strains[i], ref HexBlockModel );
                    }
                    populationDelta = ( MainGameCurrent.BlockController.GetTotalPopulation( HexBlockModel ) - previousPopulation );
                }, ref mSec
            );
            if( isMouseEnter ) {
                ChangeMouseEnterColor();
            } else {
                MainGameCurrent.MainGameController.UpdateBlockColorByInterval( () => { printColorWithPopulation2(); }, ref mColorChangeSec );
            }
        }
        private void OnTriggerEnter( Collider other ) {
            MainGameCurrent.BlockController.FetchNeighborBlocksFromMap_OnTriggerEnter( ref HexBlockModel, other, U3D_MapLogic.basicSceneMap );
        }


        // Start is called before the first frame update
        void Start() {
            HexBlockModel = U3D_MapLogic.basicSceneMap.FindBlockFromGameObjectName( gameObject.name );
            HexBlockModel.Capacity = 1000;
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
            isMouseEnter = true;

        }

        private void ChangeMouseEnterColor() {
            MainGameCurrent.FocusedBlock = this;
            if( MainGameCurrent.HoldingStrain != null ) {
                /// TODO: 当手里拿着细菌准备放置时的代码
                ChangeBlockColor( Color.green );
            } else {
                /// TODO: 手里什么都没有拿时鼠标移动到格子上的代码
                ChangeBlockColor( Color.blue );
            }
        }

        private void OnMouseExit() {
            isMouseEnter = false;
            MainGameCurrent.FocusedBlock = null;
            if( HexBlockModel.Strains.Count != 0 ) {
                /// TODO: Block中有细菌时的代码
                ChangeBlockColor( Color.yellow );
            } else {
                /// TODO: Block中没有细菌时的代码
                ChangeBlockColor( Color.white );
            }
        }
    }
}
