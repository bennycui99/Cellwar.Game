///<summary>
/// 游戏场景的结构必须是一个 Map空对象，子对象为BlockContainer
/// 
/// 形如： 
///
/// Map - MapLogic.cs   游戏启动时读取全部地图信息存储, 使用prefab生成所有BlockContainer(内含一个Block) 
/// 
///     | -BlockContainer - BlockLogic.cs   MapLogic生成的GameObject, HexBlockModel 是 MapLogic生成时给予的
///         | -Block    - BlockMouseDetect.cs   用于检测鼠标和格子交互/ 格子的颜色动画
///         | -纸片人1
///         | -纸片人2
///     | -BlockContainer
///     | -BlockContainer
///     
/// </summary>
using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Map;
using UnityEngine;
using CellWar.Utils;
using CellWar.GameData;

namespace CellWar.View {
    public class U3D_MapLogic : MonoBehaviour {

        public Map StageMap;

        [SerializeField]
        public GameObject BlockPrefab;

        /// <summary>
        /// Block HexCoor 相距 <=2
        /// </summary>
        const int BLOCK_DISTANCE = 2;

        private void Awake()
        {
            StageMap = MainGameCurrent.StageMap;
            GenerateBlockContainer();
        }

        private void Start()
        {
            BuildNeighborNetwork();
        }

        void GenerateBlockContainer()
        {
            StageMap = JsonHelper.Json2Object_NT<Map>(CellWar.GameData.Local.GetGameDataPath("map.json"));

            for (int i = 0; i < StageMap.Blocks.Count; ++i)
            {
                Block HexBlockModel = StageMap.Blocks[i];
                //生成object
                GameObject blockObject = Instantiate(BlockPrefab,gameObject.transform) as GameObject;
                //赋值block
                blockObject.GetComponent<U3D_BlockLogic>().HexBlockModel = HexBlockModel;
                //移动到正确位置
                blockObject.transform.position = new Vector3(HexBlockModel.StandardCoor.X, 0, HexBlockModel.StandardCoor.Z);
                blockObject.SetActive(true);
            }
        }

        /// <summary>
        /// 找齐所有Block的Neighbor
        /// </summary>
        void BuildNeighborNetwork()
        {
            for (int i = 0; i < StageMap.Blocks.Count; ++i)
            {
                Block block = StageMap.Blocks[i];
                for (int j = i+1;j < StageMap.Blocks.Count; ++j)
                {
                    Block neighbor = StageMap.Blocks[j];

                    // 中心距离小于BLOCK_DISTANCE 即互相为邻居
                    if (block.HexCoor.HexDistance(block.HexCoor,neighbor.HexCoor) <= BLOCK_DISTANCE)
                    {
                        block.NeighborBlocks.Add(neighbor);
                        neighbor.NeighborBlocks.Add(block);
                        //Debug.Log("Neighbor Built!");
                    }
                }
            }
        }

    }
}
