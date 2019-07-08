using System;
using UnityEngine;

namespace HexAutotile
{
    public class TerrainLayer : MonoBehaviour
    {        
        public AutoTilingType _autoTilingType = AutoTilingType.None;
        public int RandomSeed = -1;
        public bool usePredefinedRandomSeed;

        private BaseHexTerrain _owner;

        public int layerId;

        public HexTileSet tileSet;

        public bool[,] mask;
        internal bool initRandomly;

        internal void Init(BaseHexTerrain hexTerrain)
        {
            _owner = hexTerrain;
            if (initRandomly)
            {
                InitGridRandomly();
            }
        }
        
        void InitGridRandomly()
        {
            System.Random r;
            if (!usePredefinedRandomSeed)
            {
                RandomSeed = UnityEngine.Random.Range(-100, 100);
            }
            r = new System.Random(RandomSeed);

            mask = new bool[_owner.xCount, _owner.yCount];

            this._owner.IterateHex_Simple(delegate (int x, int y)
            {
                bool val;
                if (layerId == 0)
                {
                    val = true;
                } else
                {
                    val = r.Next() % 100 > 20;
                }
                mask[x, y] = val;
            });
        }

        public int xCount { get {return _owner.xCount; } }
        public int yCount { get { return _owner.yCount; } }

        public AutoTilingType AutoTilingType
        {
            get
            {
                return _autoTilingType;
            }
        }

        public bool IsActiveInLayer(int x, int y)
        {
            return mask[x, y];
        }

        internal void ApplyValue(int x, int y, bool leftMouse)
        {
            this.mask[x, y] = leftMouse;
        }
    }
}