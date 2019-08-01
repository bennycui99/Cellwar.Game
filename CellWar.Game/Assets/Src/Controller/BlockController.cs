using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellWar.Model.Map;
using CellWar.View;
using UnityEngine;

namespace CellWar.Controller {
    public class BlockController {
        public bool IsPopulationFull( Block block ) { return GetTotalPopulation( block ) > block.Capacity; }

        public bool IsPopulationBeingFull( Block block, int delta ) { return ( GetTotalPopulation( block ) + delta ) > block.Capacity; }

        /// <summary>
        /// 获取总人口数
        /// </summary>
        /// <returns></returns>
        public int GetTotalPopulation( Block block ) {
            int totalPopulation = 0;
            foreach( var s in block.Strains ) { totalPopulation += s.Population; }
            return totalPopulation;
        }
        /// <summary>
        /// 通过碰撞器获取相邻的格子
        /// </summary>
        /// <param name="other">OnTriggerEnter参数</param>
        /// <param name="map">地图实例，用于查询Block实例</param>
        public void FetchNeighborBlocksFromMap_OnTriggerEnter( ref Block block, Collider other, string gameObjectName ) {
            if( other.gameObject.tag == Block.Tag
                && other.gameObject.name != gameObjectName
                && !block.NeighborBlocks.Exists( hb => { return hb.ParentUnityObjectName == other.gameObject.name; } ) ) {
                block.NeighborBlocks.Add( U3D_MapLogic.FindBlockFromGameObjectName( other.gameObject.name ).HexBlockModel );
                Debug.Log( other.name + " Added " + "to " + gameObjectName );
            }
        }
    }
}
