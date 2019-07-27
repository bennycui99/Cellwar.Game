using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace HexAutotile
{
    public class AltitudeCellLayer : MonoBehaviour
    {
        private HeightMap _heightMap;
        private BaseHexCell _owner;
        private HexTileSet hexTileSet { get { return _heightMap.altitudeTileSet; } }

        internal void Init(BaseHexCell isoHexCell, HeightMap heightMap)
        {
            _owner = isoHexCell;
            _heightMap = heightMap;
        }

        internal void CreateSprites()
        {
            List<HexSpriteTileInfo> l = CalculateTileIndexes();
            if (l != null)
            {
                foreach (HexSpriteTileInfo info in l)
                {
                    bool isBorderIndex = false; ;

                    int heightTimes = 1;
                    int index = info.StarterIndex;

//                    isBorderIndex = true;

                    if (2 <= index && index <= 4)
                    {
                        isBorderIndex = true;
                        heightTimes = _owner.GetHeight * 2;
                    }

                    for (int i = 0; i < heightTimes; i++)
                    {
                        GameObject go = new GameObject(index + " altitude ");
                        go.transform.parent = this.transform;
                        float yOffset = -i * _heightMap.heightOffsetPerLevel / 2f;

                        float z = 0f;
                        if (!isBorderIndex)
                        {
 //                           z += -0.01f;
                        }
                        z += -0.01f;
                        go.transform.localPosition = new Vector3 (0, yOffset, z);
                        SpriteRenderer s = go.AddComponent<SpriteRenderer>();
//                        s.enabled = false;
                        s.sprite = info.GetSprite();
                    }
                }
            }
        }

        private List<HexSpriteTileInfo> CalculateTileIndexes()
        {
            int xPos = this.X;
            int yPos = this.Y;

            bool n = this.NeighborActive_N;
            bool ne = this.NeighborActive_NE;
            bool se = this.NeighborActive_SE;
            bool s = this.NeighborActive_S;
            bool sw = this.NeighborActive_SW;
            bool nw = this.NeighborActive_NW;

            return _owner.CalculateHexTiles(
                n, ne, se, s, sw, nw,
                hexTileSet
                );
        }

        public int X { get { return _owner.X; } }
        public int Y { get { return _owner.Y; } }

        public bool GetNeighborActive(int x, int y)
        {
            if (x < 0 || x >= _heightMap.xCount || y < 0 || y >= _heightMap.yCount)
            {
                return true;
            }
            int neighborHeight = _heightMap.GetHeight(x, y);

            return neighborHeight < this._owner.GetHeight;
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
    }
}
