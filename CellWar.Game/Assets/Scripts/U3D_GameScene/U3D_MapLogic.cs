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
        /// 地图中所有的格子的GameObject
        /// 地图加载时,block自身awake 创建HexBlockModel
        /// </summary>
        public static List<GameObject> BlockGameObjectList { get; set; } = new List<GameObject>();

        /// <summary>
        /// Block radius 1.0f 相距 根号3(1.7320508）
        /// </summary>
        const float BLOCK_DISTANCE = 2.0f;

        private void Awake()
        {
            GetAllBlockGameObjects();
        }

        private void Start()
        {
            BuildNeighborNetwork();
        }


        /// <summary>
        /// 取得所有Block Object
        /// </summary>
        public void GetAllBlockGameObjects()
        {
            foreach( Transform child in transform ) {
                BlockGameObjectList.Add(child.gameObject);
            }
            Debug.Log("Total block numbers:"+BlockGameObjectList.Count);
        }

        /// <summary>
        /// 找齐所有Block的Neighbor
        /// </summary>
        void BuildNeighborNetwork()
        {
            for (int i = 0; i < BlockGameObjectList.Count; ++i)
            {
                Block block = BlockGameObjectList[i].GetComponent<U3D_BlockLogic>().HexBlockModel;
                for (int j = i+1;j < BlockGameObjectList.Count; ++j)
                {
                    Block neighbor = BlockGameObjectList[j].GetComponent<U3D_BlockLogic>().HexBlockModel;

                    // 中心距离小于BLOCK_DISTANCE 即互相为邻居
                    if ((BlockGameObjectList[i].transform.position - BlockGameObjectList[j].transform.position).magnitude <= BLOCK_DISTANCE)
                    {
                        block.NeighborBlocks.Add(neighbor);
                        neighbor.NeighborBlocks.Add(block);
                        //Debug.Log("Neighbor Built!");
                    }
                }
            }
        }

        /*
        /// <summary>
        /// 通过碰撞器获取相邻的格子
        /// </summary>
        /// <param name="other">OnTriggerEnter参数</param>
        /// <param name="map">地图实例，用于查询Block实例</param>
        public void FetchNeighborBlocksFromMap_OnTriggerEnter(ref Block block, Collider other, string gameObjectName)
        {
            if (other.gameObject.tag == Block.Tag
                && other.gameObject.name != gameObjectName
                && !block.NeighborBlocks.Exists(hb => { return hb.ParentUnityObjectName == other.gameObject.name; }))
            {
                block.NeighborBlocks.Add(U3D_MapLogic.FindBlockFromGameObjectName(other.gameObject.name).HexBlockModel);
            }
        }
        */
    }
}
