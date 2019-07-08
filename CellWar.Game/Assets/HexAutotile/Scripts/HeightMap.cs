using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HexAutotile
{
    public class HeightMap : MonoBehaviour
    {
        public int RandomSeed = -1;
        public bool usePredefinedRandomSeed;

        private BaseHexTerrain _owner;

        public HexTileSet altitudeTileSet;

        private int[,] mask;

        public float heightOffsetPerLevel = 0.18f;
        internal bool initRandomly = true;

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

            mask = new int[_owner.xCount, _owner.yCount];

            this._owner.IterateHex_Simple(delegate (int x, int y)
            {
                int percent = r.Next() % 100;
                int height;
                if (percent > 85)
                {
                    height = 0;
                }
                else if (percent > 80)
                {
                    height = 3;
                }
                else if (percent > 60)
                {
                    height = 2;
                }
                else
                {
                    height = 1;
                }
                Mask[x, y] = height;
            });
        }

        public void ApplyHeight(int x, int y, int newHeight)
        {
            newHeight = Mathf.Clamp(newHeight, 0, 9);

            if (newHeight >= 0)
            {
                Mask[x, y] = newHeight;
                Validate(x, y);
            } else
            {
                Debug.LogWarning("Non correct height");
            }
        }

        void Validate (int x, int y)
        {

        }

        public int xCount { get { return _owner.xCount; } }
        public int yCount { get { return _owner.yCount; } }

        public int[,] Mask
        {
            get
            {
                return mask;
            }
        }

        internal void CreateHeightMap(int xCount, int yCount)
        {
            mask = new int[xCount, yCount];
            this._owner.IterateHex_Simple(delegate (int x, int y)
            {
                Mask[x, y] = 1;
            });
        }

        public int GetHeight(int x, int y)
        {
            return Mask[x, y];
        }

        internal void SetupMask(int[,] newMask)
        {
            this.mask = newMask;
        }
    }
}
