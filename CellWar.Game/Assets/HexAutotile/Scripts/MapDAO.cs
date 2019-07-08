using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

namespace HexAutotile
{
    public class MapDAO : MonoBehaviour
    {
        public string mapName;

        public BaseHexTerrain targetTerrain;
        XDocument currentXDocument;

        public HexTileSet altitudeTiles;
        public HexTileSet grassTiles;

        public HexTileSet dirtTiles;

        public int yCount { get { return targetTerrain.yCount; } }
        public int xCount { get { return targetTerrain.xCount; } }

        #region ReadMap #######################################################################################

        public void Load ()
        {
            currentXDocument = null;
            XDocument xml = TryReadFromStreamingAsset(mapName);
            if (xml != null)
            {
                try
                {
                    targetTerrain.ClearTerrain();
                    currentXDocument = xml;

                    XElement xterrain = xml.Root.Element("terrain");
                    if (xterrain != null)
                    {
                        ReadTerrainData(xterrain);
                    }                  

                    targetTerrain.Create ();
                } catch (Exception e)
                {
                    print(e.ToString ());

                    targetTerrain.Create();
                }
            }
        }

        private void ReadTerrainData(XElement xterrain)
        {
            int x = int.Parse (xterrain.Attribute("x").Value);
            int y = int.Parse (xterrain.Attribute("y").Value);

            targetTerrain.xCount = x;
            targetTerrain.yCount = y;

            ReadHeightMap(xterrain.Element ("heightmap"));
            ReadLayers(xterrain.Element("layers"));
        }

        private void ReadHeightMap(XElement xheightMap)
        {
            HeightMap oldh = targetTerrain.GetComponent<HeightMap>();
            if (oldh)
            {
                Destroy(oldh);
            }

            HeightMap h = targetTerrain.gameObject.AddComponent<HeightMap>();
            targetTerrain.HeightMap = h;
            h.altitudeTileSet = altitudeTiles;
            h.initRandomly = false;
            h.Init(targetTerrain);
            h.CreateHeightMap(xCount, yCount);

            XElement xmask = xheightMap.Element("layermask");
            ReadLayerMask(xmask.Value, h.Mask);
        }

        private void ReadLayers(XElement xlayers)
        {
            TerrainLayer[] terrainLayers = this.targetTerrain.GetComponents<TerrainLayer>();
            
            foreach (TerrainLayer t in terrainLayers)
            {
                t.layerId = -1;
                Destroy(t);
            }

            foreach (XElement xlayer in xlayers.Elements("layer"))
            {
                ReadLayer(xlayer);
            }           
        }

        private void ReadLayer (XElement xlayer)
        {
            int layerId = int.Parse(xlayer.Attribute("id").Value);
            TerrainLayer l = null;           

            l = targetTerrain.gameObject.AddComponent<TerrainLayer>();
            targetTerrain.TerrainLayers.Add(l);
            l.mask = new bool[xCount, yCount];
            
            l.initRandomly = false;            
            l.layerId = layerId;

            if (l.layerId == 0)
            {
                for (int y = 0; y < yCount; y++)
                {
                    for (int x = 0; x < xCount; x++)
                    {
                        l.mask[x, y] = true;
                    }
                }
                l.tileSet = dirtTiles;
            } else
            {
                l.tileSet = grassTiles;
            }

            XElement xmask = xlayer.Element("layermask");
            if (xmask != null)
            {
                ReadLayerMask(xmask.Value, l.mask);
            }
        }

        private void ReadLayerMask(string body, int [,] mask)
        {
            body = CleanMaskStringFromSpecialCharacters(body);
            int totalCount = xCount * yCount;

            IterateMask(delegate (int x, int y)
            {
                string myBody = body;

                    int i = x + y * xCount;
                    int value = 0;
                    char c = body[i];
                    try
                    {
                        value = (int)Char.GetNumericValue(c);
                    }
                    catch (Exception e)
                    {
                        print(e.ToString());
                    }
                    mask[x, y] = value;
            });
        }

        private void ReadLayerMask(string body, bool [,] mask)
        {
            body = CleanMaskStringFromSpecialCharacters(body);
            int totalCount = xCount * yCount;

            IterateMask(delegate (int x, int y)
            {
                int i = x + y * xCount;
                int value = 0;
                char c = body[i];
                try
                {
                    value = (int)Char.GetNumericValue(c);
                }
                catch (Exception e)
                {
                    print(e.ToString());
                }
                if (value == 0)
                {
                    mask[x, y] = false;
                }
                else
                {
                    mask[x, y] = true;
                }

            });
        }

        public static string CleanMaskStringFromSpecialCharacters(string maskStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in maskStr)
            {
                if ((c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public XDocument TryReadFromStreamingAsset(string fileName)
        {
            XDocument xml = null;
            try
            {
                xml = XDocument.Load(FullFileName (fileName));
            }
            catch (Exception e)
            {
                return TryReadFromPredefinedTextAsset();
//                print(e.ToString ());
            }
            return xml;
        }

        public TextAsset textAsset;

        public XDocument TryReadFromPredefinedTextAsset()
        {
            XDocument xml = null;
            try
            {
                if (textAsset != null)
                {
                    xml = XDocument.Parse(textAsset.text);
                } else
                {
                    xml = XDocument.Parse(StubXML.GetStubXMLText ());
                }
                
            }
            catch (Exception e)
            {
                return new XDocument();
            }
            return xml;
        }

        #endregion #######################################################################################

        #region ResizeTerrain #######################################################################################

        public void ResizeTerrain(int x, int y)
        {
            int newX = x;
            int newY = y;

            ResizeHeightMap(newX, newY);
            ResizeLayers(newX, newY);

            targetTerrain.xCount = x;
            targetTerrain.yCount = y;

            targetTerrain.ClearTerrain();
            targetTerrain.Create();
            targetTerrain.OrderRedraw();
        }

        void ResizeHeightMap (int newX, int newY)
        {
            int[,] oldMask = targetTerrain.HeightMap.Mask;
            int [,] newMask = new int[newX, newY];

            for (int i = 0; i < newX; i++)
            {
                for (int j = 0; j < newY; j++)
                {
                    newMask[i, j] = 1;
                }
            }

            IterateMask(delegate (int x, int y) {
                if (newMask.GetLength (0) > x && newMask.GetLength (1) > y 
                )
                {
                    int value = oldMask[x, y];
                    newMask[x, y] = value;                  
                } 
            });
            
            targetTerrain.HeightMap.SetupMask (newMask);
        }

        void ResizeLayers (int newX, int newY)
        {
            foreach (TerrainLayer t in targetTerrain.TerrainLayers)
            {
                bool[,] oldMask = t.mask;
                bool[,] newMask = new bool[newX, newY];
                IterateMask(delegate (int x, int y) {
                    if (newMask.GetLength(0) > x && newMask.GetLength(1) > y
                       )
                    {
                        bool value = oldMask[x, y];
                        newMask[x, y] = value;
                    }
                });

                t.mask = newMask;
            }
        }

        #endregion #######################################################################################

        #region SaveMap #######################################################################################

        internal void Save()
        {
            if (currentXDocument == null)
            {
                currentXDocument = new XDocument();
                currentXDocument.Add(new XElement ("map"));
            }
            XElement xterrainOld = currentXDocument.Root.Element("terrain");
            xterrainOld.Remove();
            xterrainOld.RemoveAll();

            WriteTerrain(currentXDocument.Root);

            string fileName = FullFileName(mapName);
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Dispose ();
            }

            currentXDocument.Save(fileName);            
        }

        void WriteTerrain (XElement xroot)
        {
            XElement xterrain = xroot.CreateChild("terrain");
            xterrain.CreateAttributeValue("x", xCount + "");
            xterrain.CreateAttributeValue("y", yCount + "");
            WriteHeightMap(xterrain);
            WriteLayers(xterrain);
        }

        void WriteHeightMap (XElement xterrain)
        {            
            XElement xheight = xterrain.CreateChild("heightmap");
            XElement layermask = xheight.CreateChild("layermask");

            StringBuilder s = new StringBuilder();
            int i = 0;

            IterateMask( delegate (int x, int y)
                {
                    int h = 0;
                    int[,] mask = this.targetTerrain.HeightMap.Mask;

                    if (x >= mask.GetLength (0) || y >= mask.GetLength(1))
                    {
                        throw new Exception();
                    } else 
                    {
                        h = mask[x, y];
                    }
                    
                    if (i >= xCount)
                    {
                        i = 0;
                        s.Append("\n");
                    }

                    s.Append(h);

                    i++;
                }
            );

            layermask.Value = s.ToString();
        }

        void WriteLayers(XElement xterrain)
        {
            XElement xlayers = xterrain.CreateChild("layers");

            foreach (TerrainLayer t in this.targetTerrain.TerrainLayers)
            {
                WriteLayer(xlayers, t);
            }
        }

        void WriteLayer (XElement xlayers, TerrainLayer t)
        {
            XElement xlayer = xlayers.CreateChild("layer");
            xlayer.CreateAttributeValue("id", t.layerId + "");
            if (t.layerId != 0)
            {
                XElement layermask = xlayer.CreateChild("layermask");

                StringBuilder s = new StringBuilder();
                int i = 0;

                IterateMask(delegate (int x, int y)
                {
                    bool b = t.mask[x, y];

                    if (i >= xCount)
                    {
                        i = 0;
                        s.Append("\n");
                    }

                    int toAppend;
                    if (b)
                    {
                        toAppend = 1;
                    }
                    else
                    {
                        toAppend = 0;
                    }

                    s.Append(toAppend);
                    i++;
                }
                );

                layermask.Value = s.ToString();
            }           
        }

        public string FullFileName (string fileName)
        {
            string assetsPath = Application.dataPath + "/StreamingAssets";

            if (!Directory.Exists (assetsPath))
            {
                Directory.CreateDirectory(assetsPath);
            }

            string fullPath = assetsPath + "/" + fileName + ".xml";

            return fullPath;
        }

        public delegate void Action_XY(int x, int y);
        public void IterateMask(Action_XY action)
        {
            for (int y = 0; y < yCount; y++)
            {
                for (int x = 0; x < xCount; x++)
                {
                    action(x, y);
                }
            }
        }

        #endregion #######################################################################################
    }

    static class StubXML
    {
        public static string GetStubXMLText ()
        {
            return "<?xml version='1.0' encoding='utf-8'?> <map> <terrain x='33' y='33'> <heightmap> <layermask>011001111221232241122310011221221 011111111212233342233321022322221 011011112222325333352222023222230 011011112212133333344322132222331 011111122222133333451112034332211 111221112111133334321111124333100 011221111211133333211112112222000 112211112410132222111222111111000 112111111110111112232301111122110 012111110310000111233022321211111 022121100100000012420033313211111 022211111100000022410003212211111 111111101100000123310001112211111 122211211100001222100000112321100 000001311000001111100111122112100 000112311100011101011111211111100 001111111111111131111110221111100 001100111111112211111110221111100 000110111121111111112100221111000 001121111211111111222100111100000 001111111111212211221111100000000 000001111112212211212111000000000 000010101112112211122320000000000 000111010101111011123320000000000 000111001010000011113210000000000 000000000111100121111110000000000 000000111111101121111100000000000 000001111111111101111000000000000 000001101111111100000000000000000 000010011111111100000000000000000 011221222221111100000000000000000 112221322221111000000000000000000 112221322210000000000000000000000</layermask> </heightmap> <layers> <layer id='0' /> <layer id='1'> <layermask>001000000110111011111111000000000 010000000111111111111111110000000 011000001111111111111111111010000 010000001111101111111111111111110 000000011111011111111111111111110 000110001000111111111111111111110 000110011011111111111111111111110 001100001111111111111111111111110 101000011111111111111111111111110 101000111111111111111111111111100 011011111111111111111111111111000 011101111111111111111111111111000 000001111111111111111111111111000 011111111111111111111111111100000 000011111111111111111111111100000 000011111111111111111111111100000 000011111111111111111111111100000 000011111111111111111111111000000 000011111111111011111111111000000 000011111111111111111111111000000 000011111111111111111111111000000 000001111111111111111111111000000 000011111111111111111111111000000 000011111111111111111111100000000 000011111111111111111111100000000 000011111111001111111111100000000 000011111110000001111111100000000 000011111111000001111111000000000 000011111111000001111100000000000 000011111111000000001000000000000 000011111111000000000000000000000 000001111100000000000000000000000 000000001000000000000000000000000</layermask> </layer> </layers> </terrain> </map>";
        }
    }
}
