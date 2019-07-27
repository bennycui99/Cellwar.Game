using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexAutotile
{
    public class IsoHexCell : BaseHexCell
    {
        TerrainCellLayer[] cellLayers;
        AltitudeCellLayer altitudeCellLayer;

        public override void Init(BaseHexTerrain hexTerrain, int x, int y)
        {
            base.Init(hexTerrain, x, y);
        }

        public override void DrawSprites()
        {
            height = this.HexTerrain.HeightMap.GetHeight(X, Y);

            if (GetHeight == 0)
            {
                return;
            }

            cellLayers = new TerrainCellLayer[HexTerrain.TerrainLayers.Count];

            altitudeCellLayer = new GameObject(this.name + " altitudeCell").AddComponent<AltitudeCellLayer>();
            altitudeCellLayer.Init(this, HexTerrain.HeightMap);
            altitudeCellLayer.transform.parent = this.transform;
            altitudeCellLayer.transform.localPosition = Vector3.zero;

            List<TerrainLayer> tlayers = HexTerrain.TerrainLayers;
            for (int i = tlayers.Count - 1; i >= 0; i--)
            {
                TerrainLayer t = tlayers[i];
                TerrainCellLayer cellLayer = new GameObject(this.name + " layer " + t.layerId).AddComponent<TerrainCellLayer>();
                cellLayers[i] = cellLayer;
                cellLayer.Init(this, t);
                cellLayer.transform.parent = this.transform;
                cellLayer.transform.localPosition = Vector3.zero;
            }

            altitudeCellLayer.CreateSprites();

            for (int i = cellLayers.Length - 1; i >= 0; i--)
            {
                TerrainCellLayer c = cellLayers[i];
                c.CreateSprites();
                if (c.isActiveInLayer)
                {
                    if (c.autoTilingType != AutoTilingType.Contracting)
                    {
                        break;
                    }
                }
            }
        }

        public override void ClearSprites()
        {
            if (cellLayers != null)
            {
                for (int i = cellLayers.Length - 1; i >= 0; i--)
                {
                    TerrainCellLayer c = cellLayers[i];
                    if (c)
                        Destroy(c.gameObject);
                }
            }
            if (altitudeCellLayer)
                Destroy(altitudeCellLayer.gameObject);
        }

        public override int GetNW_Y
        {
            get
            {
                int order = (X) % 2;
                switch (order)
                {
                    case (0): return Y - 1;
                    case (1): return Y;
                }
                return Y;
            }
        }

        public override int GetNE_Y
        {
            get
            {
                int order = (X) % 2;
                switch (order)
                {
                    case (0): return Y - 1;
                    case (1): return Y;
                }
                return Y;
            }
        }

        public override int GetSE_Y
        {
            get
            {
                int order = (X) % 2;
                switch (order)
                {
                    case (0): return Y;
                    case (1): return Y + 1;
                }
                return -1;
            }
        }

        public override int GetSW_Y
        {
            get
            {
                int order = (X) % 2;
                switch (order)
                {
                    case (0): return Y;
                    case (1): return Y + 1;
                }
                return -1;
            }
        }

        public override List<HexSpriteTileInfo> CalculateHexTiles(
    bool n, bool ne, bool se, bool s,
    bool sw, bool nw, HexTileSet hexTileSet)
        {
            HashSet<int> indexes = new HashSet<int>();

            indexes = HexTileSet.IndexesFromNeighbors(n, ne, se, s, sw, nw);

            return CalculateHexTiles(indexes, hexTileSet);
        }

        protected static List<HexSpriteTileInfo> CalculateHexTiles(HashSet<int> indexes, HexTileSet hexTileSet)
        {
            List<HexSpriteTileInfo> hexSpriteTiles = new List<HexSpriteTileInfo>();

            int iterationCount = 0;
            while (indexes.Count > 0)
            {
                HexSpriteTileInfo t = new HexSpriteTileInfo(true, hexTileSet);
                FillTileSpriteIndexes(t, indexes, hexTileSet);

                hexSpriteTiles.Add(t);

                iterationCount++;
                if (iterationCount > 100)
                {
                    throw new System.Exception();
                }
            }

            if (hexTileSet.HasAdditional3WeightSprites && hexSpriteTiles.Count > 2)
            {
                HexSpriteTileInfo t = new HexSpriteTileInfo(true, hexTileSet);
                t.FullCell = true;
                if (hexSpriteTiles[0].StarterIndex == 0)
                {
                    t.AddIndexes(new int[] { 0, 2, 4 });
                }
                else
                {
                    t.AddIndexes(new int[] { 1, 3, 5 });
                }

                hexSpriteTiles.Clear();

                hexSpriteTiles.Add(t);
            }

            return hexSpriteTiles;
        }

        protected static void FillTileSpriteIndexes(HexSpriteTileInfo t, HashSet<int> indexes, HexTileSet hexTileSet)
        {
            if (hexTileSet.HasAdditional3WeightSprites && indexes.Count == 6)
            {
                t.AddIndexes(0, 1, 2);
                indexes.Remove(0);
                indexes.Remove(1);
                indexes.Remove(2);
                return;
            }

            int startIndex = indexes.First();
            int step = 1;

            for (int i = 0; i < 5; i++)
            {
                bool isCurrentIndexTrue = indexes.Contains(startIndex);

                if (!isCurrentIndexTrue)
                {
                    break;
                }

                startIndex -= step;

                //make iteration cycled
                if (startIndex > 5) // must be lesser 5
                {
                    startIndex = 0;
                }

                if (startIndex < 0) // must be bigger 2
                {
                    startIndex = 5;
                }
            }

            int index = startIndex;

            while (t.CanAddMoreIndexes) //max 3 indexes
            {
                bool isCurrentIndexTrue = indexes.Contains(index);

                if (isCurrentIndexTrue)
                {
                    t.AddIndex(index);
                    indexes.Remove(index);
                }
                else
                {
                    if (!t.HasNoAnyIndex)
                    {
                        break;
                    }
                }

                index += step; //nextStep

                //make iteration cycled
                if (index > 5) // must be lesser 5
                {
                    index = 0;
                }

                if (index < 0) // must be bigger 2
                {
                    index = 5;
                }
            }
        }

    }
}
