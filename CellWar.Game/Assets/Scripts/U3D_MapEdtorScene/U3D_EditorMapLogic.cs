using System.Collections.Generic;
using CellWar.Model.Map;
using UnityEngine;
using CellWar.Utils;
using CellWar.GameData;

namespace CellWar.View
{
    public class U3D_EditorMapLogic : MonoBehaviour
    {
        public Map StageMap = new Map();
        [SerializeField]
        public GameObject BlockPrefab;

        private void Awake()
        {
            // 为editor加载地图
            MainGameCurrent.LoadMap();
            GenerateBlockContainer();
        }

        /// <summary>
        /// 初始化地图编辑器的地图
        /// </summary>
        void GenerateBlockContainer()
        {
            List<Block> blocks = new List<Block>();
            for(int x = -10; x < 10; ++x)
            {
                for(int z = -10; z < 10; ++z)
                {
                    // 生成blocks
                    Block HexBlockModel = null;
                    var currentBlock = MainGameCurrent.StageMap.Blocks.Find( m => m.HexCoor.X == x && m.HexCoor.Z == z );
                    if( currentBlock == null ) {
                        HexBlockModel = new Block();
                        HexBlockModel.HexCoor.X = x;
                        HexBlockModel.HexCoor.Z = z;
                        HexBlockModel.StandardCoor = HexBlockModel.HexCoor.HexToStandardCoordiante();

                        HexBlockModel.Capacity = 1000;
                        HexBlockModel.BlockType = Block.Type.Normal;
                    } else {
                        HexBlockModel = currentBlock;
                    }
                    blocks.Add(HexBlockModel);

                    //生成object
                    var blockObject = Instantiate(BlockPrefab, gameObject.transform) as GameObject;
                    //赋值BlockMouseDetect
                    blockObject.transform.Find("Block").transform.Find("挤压").transform.Find("封顶_2").GetComponent<U3D_EditorBlockMouseDetect>().HexBlockModel = HexBlockModel;
                    //移动到正确位置
                    blockObject.transform.position = new Vector3(HexBlockModel.StandardCoor.X, 0, HexBlockModel.StandardCoor.Z);
                    blockObject.SetActive(true);
                }
            }
            MainGameCurrent.StageMap.Blocks = blocks;
        }

    }

}
