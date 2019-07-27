using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexAutotile
{
    public abstract class BaseHexCell : MonoBehaviour
    {
        public bool isActiveInLayer;
        private int _x, _y;
        private BaseHexTerrain _hexTerrain;
        internal HexCellClicker hexCellClicker;
        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }

        public int height;

        public BaseHexCell[] Neighbors;

        public virtual void Init(BaseHexTerrain owner, int x, int y)
        {
            _hexTerrain = owner;
            _x = x;
            _y = y;  

            height = this._hexTerrain.HeightMap.GetHeight(X, Y);
        }

        internal void PostCreate()
        {
            Neighbors = new BaseHexCell[6];

            BaseHexCell n = this._hexTerrain.GetCell(X, Y - 1);
            BaseHexCell ne = this._hexTerrain.GetCell(X + 1, GetNE_Y);
            BaseHexCell se = this._hexTerrain.GetCell(X + 1, GetSE_Y);
            BaseHexCell s = this._hexTerrain.GetCell(X, Y + 1);
            BaseHexCell sw = this._hexTerrain.GetCell(X - 1, GetSW_Y);
            BaseHexCell nw = this._hexTerrain.GetCell(X - 1, GetNW_Y);

            Neighbors[0] = n;
            Neighbors[1] = ne;
            Neighbors[2] = se;
            Neighbors[3] = s;
            Neighbors[4] = sw;
            Neighbors[5] = nw;
        }

        public bool GetCell(int x, int y)
        {
            return _hexTerrain.GetCell(x, y);
        }

        public virtual void DrawSprites ()
        {
        }

        public virtual void ClearSprites ()
        {
            foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer> ())
            {
                Destroy(s.gameObject);
            }
        }

        public abstract int GetNW_Y
        {
            get;
        }

        public abstract int GetNE_Y
        {
            get;
        }

        public abstract int GetSE_Y
        {
            get;
        }

        public abstract int GetSW_Y
        {
            get;
        }

        public virtual List<HexSpriteTileInfo> CalculateHexTiles(bool n, bool ne, bool se, bool s, bool sw, bool nw, HexTileSet hexTileSet)
        {
            throw new NotImplementedException();
        }

        public int GetHeight
        {
            get
            {
                return height;
            }
        }

        public BaseHexTerrain HexTerrain
        {
            get
            {
                return _hexTerrain;
            }
        }

        internal void Redraw()
        {
            ClearSprites();
            DrawSprites();
        }
    }
}
