///<summary>
/// 游戏场景的结构必须是一个 Map空对象，子对象为Block
/// 
/// 形如： 
///
/// Map -
///     | -Block1
///     | -Block2
///     
/// </summary>
using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Map;
using UnityEngine;

namespace CellWar.View {
    public class U3D_MapLogic : MonoBehaviour {
        /// <summary>
        /// 地图中所有的格子
        /// 应该在地图加载时给该列表赋值
        /// </summary>
        public static List<U3D_BlockLogic> Blocks { get; set; } = new List<U3D_BlockLogic>();

        private void Awake() {
            LoadAllBlockUnityObjectFromTransform( transform );
        }


        /// <summary>
        /// 于MapLogic Awake中调用
        /// 若置于Start中会导致block列表为空的问题
        /// </summary>
        /// <param name="mapTransform"></param>
        public static void LoadAllBlockUnityObjectFromTransform( Transform mapTransform ) {
            foreach( Transform blockTransform in mapTransform ) {
                Blocks.Add( blockTransform.GetComponent<U3D_BlockLogic>() );
            }
            Debug.Log( Blocks.Count );
        }


        public static U3D_BlockLogic FindBlockFromGameObjectName( string gameObjectName ) {
            return Blocks.Find( b => b.gameObject.name == gameObjectName );
        }
    }
}
