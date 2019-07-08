using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexAutotile
{
    public enum AutoTilingType { Expanding, Contracting, None }

    public class HexTileSet : MonoBehaviour
    {
        public Sprite[] _1WeightCornerTiles;
        public Sprite[] _2WeightCornerTiles;
        public Sprite[] _3WeightCornerTiles;
        public Sprite[] _3WeightAdditionalCornerTiles;

        public Sprite[] centerHexTiles;

        internal Sprite GetCenterSprite(int index)
        {
             return centerHexTiles[index];
        }

        public static HashSet<int> IndexesFromNeighbors(
            bool n, bool ne, bool se, bool s,
            bool sw, bool nw
        )
        {
            HashSet<int> indexes = new HashSet<int>();

            if (n) indexes.Add(0);
            if (ne) indexes.Add(1);
            if (se) indexes.Add(2);
            if (s) indexes.Add(3);
            if (sw) indexes.Add(4);
            if (nw) indexes.Add(5);

            return indexes;
        }

        internal Sprite GetCornerSprite(int startPos, int arrayNum)
        {
            Sprite[] spriteArray = null;

            switch (arrayNum)
            {
                case (1): spriteArray = _1WeightCornerTiles; break;
                case (2): spriteArray = _2WeightCornerTiles; break;
                case (3): spriteArray = _3WeightCornerTiles; break;
                case (4): spriteArray = _3WeightAdditionalCornerTiles; break;
            }

            if (spriteArray != null)
            {
                return spriteArray[startPos];
            } else
            {
                return null;
            }
        }

        public bool HasAdditional3WeightSprites
        {
            get
            {
                return _3WeightAdditionalCornerTiles.Length > 0;
            }
        }

        public int MaximumWeight
        {
            get
            {
                if (_3WeightCornerTiles.Length > 0)
                {
                    return 3;
                }
                if (_2WeightCornerTiles.Length > 0)
                {
                    return 2;
                }
                return 1;
            }
        }
    }

    public class HexSpriteTileInfo
    {
        private bool _isCornerTile;
        private int _starterIndex = -1;
        private int _indexCount;
        private bool fullCell = false;
        private HexTileSet _hexTileSet;

        public HexSpriteTileInfo(bool isCornerTile, HexTileSet hexTileSet)
        {
            _isCornerTile = isCornerTile;
            _hexTileSet = hexTileSet;
        }

        public void AddIndex(int index)
        {
            if (CanAddMoreIndexes)
            {
                if (_starterIndex < 0)
                {
                    _starterIndex = index;
                }
                _indexCount++;
            }
        }

        public void AddIndexes(params int[] indexes)
        {
            foreach (int i in indexes)
            {
                AddIndex(i);
            }
        }

        public bool CanAddMoreIndexes
        {
            get
            {
                return _indexCount < _hexTileSet.MaximumWeight;
            }
        }

        public bool HasNoAnyIndex
        {
            get
            {
                return _indexCount == 0;
            }
        }

        public bool IsCornerTile
        {
            get
            {
                return _isCornerTile;
            }

            set
            {
                _isCornerTile = value;
            }
        }

        public int StarterIndex
        {
            get
            {
                return _starterIndex;
            }
        }

        public bool FullCell
        {
            get
            {
                return fullCell;
            }

            set
            {
                fullCell = value;
            }
        }

        internal Sprite GetSprite()
        {
            if (IsCornerTile)
            {
                if (FullCell)
                {
                    return _hexTileSet.GetCornerSprite(_starterIndex, 4);
                }

                return _hexTileSet.GetCornerSprite(_starterIndex, _indexCount);
            }
            else
            {
                int index = StarterIndex;
                return _hexTileSet.GetCenterSprite(index);                                
            }
        }
    }
}
