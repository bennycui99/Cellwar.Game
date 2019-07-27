using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace HexAutotile
{
    public class TerrainCellLayer : MonoBehaviour
    {
        public List<int> calculatedIndexes;

        public HexTileSet hexTileSet { get { return _terrainLayer.tileSet; } }
        private TerrainLayer _terrainLayer;
        private BaseHexCell _owner;

        public void Init (BaseHexCell owner, TerrainLayer t)
        {
            _owner = owner;
            _terrainLayer = t;
        }

        public AutoTilingType autoTilingType
        {
            get
            {
                return this._terrainLayer.AutoTilingType;
            }
        }

        public bool isActiveInLayer
        {
            get {
                if (_terrainLayer.layerId == 0)
                {
                    return true;
                } else
                return _terrainLayer.IsActiveInLayer(X, Y);
            }
        }

        public void CreateSprites()
        {
            List<HexSpriteTileInfo> l = CalculateTileIndexes();
            if (l != null)
            {
                foreach (HexSpriteTileInfo info in l)
                {
                    GameObject go = new GameObject(gameObject.name + " child sprite ");
                    go.transform.parent = this.transform;
                    Vector3 localPosition = Vector3.zero;
                    localPosition[2] = -this._terrainLayer.layerId * 0.001f;
                    go.transform.localPosition = localPosition;
                    SpriteRenderer s = go.AddComponent<SpriteRenderer>();
                    s.sprite = info.GetSprite();
//                    s.enabled = false;
                }
            }
        }

        List<HexSpriteTileInfo> CalculateTileIndexes()
        {
            if (autoTilingType == AutoTilingType.None)
            {
                if (isActiveInLayer)
                {
                    return CreateRandomCenterIndexes();
                } else
                {
                    return null;
                }
            } else if (autoTilingType == AutoTilingType.Expanding)
            {
                if (isActiveInLayer)
                {
                    return CreateRandomCenterIndexes();
                } else
                {
                    return CalculateCornerIndexes();
                }
            } else
            {
                Debug.LogError("Not implemented yet");

                List<HexSpriteTileInfo> container = CreateRandomCenterIndexes();
                List<HexSpriteTileInfo> cornerTiles = CalculateCornerIndexes();

                container.AddRange(cornerTiles);
                return container;
            }    
        }

        public List<HexSpriteTileInfo> CalculateCornerIndexes()
        {
            int xPos = this.X;
            int yPos = this.Y;

            var n = this.NeighborActive_N;

            var ne = this.NeighborActive_NE;
            var se = this.NeighborActive_SE;

            var s = this.NeighborActive_S;
            var sw = this.NeighborActive_SW;

            var nw = this.NeighborActive_NW;

            return _owner.CalculateHexTiles(
                n, ne, se, s, sw, nw,
                hexTileSet
                );
        }

        protected List<HexSpriteTileInfo> CreateRandomCenterIndexes()
        {
            List<HexSpriteTileInfo> hexSpriteTiles = new List<HexSpriteTileInfo>();
            System.Random cellRandomizer;
            cellRandomizer = new System.Random(X * Y);
            HexSpriteTileInfo t = new HexSpriteTileInfo(false, hexTileSet);
            int randomTileIndex;
            if (cellRandomizer.Next(100) < 70 && hexTileSet.centerHexTiles.Length > 2)
            {
                randomTileIndex = cellRandomizer.Next(3);
            }
            else
            {
                randomTileIndex = cellRandomizer.Next(hexTileSet.centerHexTiles.Length);
            }
            t.AddIndex(randomTileIndex);
            hexSpriteTiles.Add(t);
            return hexSpriteTiles;
        }

        public bool GetNeighborActive(int x, int y)
        {
            if (x < 0 || x >= _terrainLayer.xCount || y < 0 || y >= _terrainLayer.yCount)
            {
                return this.isActiveInLayer;
            }
            int neighborHeight = _owner.HexTerrain.HeightMap.GetHeight(x, y);
            if (neighborHeight != _owner.GetHeight)
            {
                return false;
            }
            return _terrainLayer.IsActiveInLayer(x, y);         
        }

        public virtual bool NeighborActive_N
        {
            get
            {
                return this.GetNeighborActive(X, Y - 1);
            }
        }

        public virtual bool NeighborActive_NE
        {
            get
            {
                return this.GetNeighborActive(X + 1, _owner.GetNE_Y);
            }
        }

        public virtual bool NeighborActive_SE
        {
            get
            {
                return this.GetNeighborActive(X + 1, _owner.GetSE_Y);
            }
        }

        public virtual bool NeighborActive_S
        {
            get
            {
                return this.GetNeighborActive(X, Y + 1);
            }
        }

        public virtual bool NeighborActive_SW
        {
            get
            {
                int x = X;
                return this.GetNeighborActive(x - 1, _owner.GetSW_Y);
            }
        }

        public virtual bool NeighborActive_NW
        {
            get
            {

                return this.GetNeighborActive(X - 1, _owner.GetNW_Y);
            }
        }

        public int X { get { return _owner.X; } }
        public int Y { get { return _owner.Y; } }
    }
}
