using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace HexAutotile
{
    public class MapRandomizer : MonoBehaviour
    {
        public string mapName;

        public BaseHexTerrain targetTerrain;
        XDocument currentXDocument;

        public HexTileSet altitudeTiles;
        public HexTileSet grassTiles;
        public HexTileSet dirtTiles;

        void Awake ()
        {
            ReadDataFor(targetTerrain);
        }

        public void ReadDataFor (BaseHexTerrain terrain)
        {
            targetTerrain = terrain;
            XDocument xml = TryReadFrom(mapName);
            if (xml != null)
            {
                targetTerrain.ClearTerrain();

                currentXDocument = xml;
                ReadHeightMap();
                ReadLayers();

                print("Reading map was successfull");
            }
        }

        private void ReadHeightMap()
        {
            HeightMap h = targetTerrain.gameObject.AddComponent<HeightMap>();
            h.altitudeTileSet = altitudeTiles;
            h.usePredefinedRandomSeed = true;
            h.RandomSeed = -21;
        }

        private void ReadLayers()
        {
            CreateLayer();
        }

        private void CreateLayer ()
        {
            TerrainLayer l = targetTerrain.gameObject.AddComponent<TerrainLayer>();
            l.tileSet = grassTiles;
        }

        public static XDocument TryReadFrom(string fileName)
        {
            XDocument xml = null;
            try
            {
                string assetsPath = Application.dataPath + "/StreamingAssets";
                string fullPath = assetsPath + "/" + fileName + ".xml";
                xml = XDocument.Load(fullPath);
            }
            catch (Exception e)
            {
                print(e.ToString ());
            }
            return xml;
        }
    }
}
