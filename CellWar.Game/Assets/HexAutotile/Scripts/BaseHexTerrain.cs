using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexAutotile.UI;
using UnityEngine;

namespace HexAutotile
{
    public class BaseHexTerrain : MonoBehaviour
    {
        public int xCount = 5, yCount = 5;
        public float xSize= 4f, ySize = 4f;
        public Sprite editorOverSprite;

        public BaseHexCell[,] _gridCells;

        internal void AddObserver(ITerrainObserver o)
        {
            terrainObservers.Add(o);
        }

        private List<TerrainLayer> terrainLayers = new List<TerrainLayer> ();
        private HeightMap heightMap;

        private bool _inited = false;

        //void Awake ()
        //{
        //    if (!_heightMap)
        //    {
        //        _inited = false;
        //    }           
        //}

        List<ITerrainObserver> terrainObservers = new List<ITerrainObserver> ();

        public void ClearTerrain ()
        {
            if (this != null)
            {
                foreach (Transform t in transform)
                {
                    Destroy(t.gameObject);
                }
            }
        }

        public void Create ()
        {
            _inited = true;

            _gridCells = new BaseHexCell[xCount, yCount];

            if (!HeightMap)
            {
                HeightMap = gameObject.GetComponentInChildren<HeightMap>();
            }

            HeightMap.Init(this);

            TerrainLayers.RemoveAll(x => x.layerId == -1);
            foreach (TerrainLayer t in TerrainLayers)
            {
                t.Init(this);
            }

            CreateCells();
            
            Redraw();
            Invoke("DoRepositionCells", 0f);
            DoRepositionCells();

            Camera camera = Camera.main;
            camera.transform.localPosition = Vector3.zero;

            Invoke("ResizeTerrain", 0.01f);
        }

        public float xMinPosition
        {
            get
            {
                float x = GetPositionForCell(0, yCount - 1).x;
                return x;
            }
        }

        public float yMinPosition
        {
            get
            {
                float y = GetPositionForCell(xCount - 1, 0).y;
                return y;
            }
        }

        public float xMaxPosition
        {
            get
            {
                float x = GetPositionForCell(xCount - 1, 0).x;
                return x;
            }
        }

        public float yMaxPosition
        {
            get
            {
                float y = GetPositionForCell(0, yCount - 1).y;
                return y;
            }
        }

        public HeightMap HeightMap
        {
            get
            {
                if (heightMap == null)
                {
                    Debug.LogError("wtf");
                    heightMap = GetComponentInChildren<HeightMap>();
                }
                return heightMap;
            }

            set
            {
                heightMap = value;
            }
        }

        public List<TerrainLayer> TerrainLayers
        {
            get
            {
                return terrainLayers;
            }

            //set
            //{
            //    terrainLayers = value;
            //}
        }

        public void DoRepositionCells ()
        {
            IterateHex_Simple(RepositionCell);
        }

        void ResizeTerrain()
        {
            foreach (ITerrainObserver listener in this.terrainObservers)
            {
                listener.OnResizeTerrain();
            }
        }

        protected void Redraw()
        {
            IterateHex_Simple(delegate (BaseHexCell cell)
            {
                cell.ClearSprites();
            });
            DrawSprites();
            foreach (ITerrainObserver o in this.terrainObservers)
            {
                o.OnRedraw();
            }
        }

        public void OrderRedraw()
        {
            Redraw();
        }

        protected virtual void CreateCells () //order of cell creation metters
        {
            IterateHex_Simple(CreateCell);
            IterateHex_Simple(PostCreateCell);
        }

        protected void CreateCell (int x, int y)
        {
            BaseHexCell c = new GameObject ().AddComponent<IsoHexCell> ();
            c.transform.parent = this.transform;
            c.name = "Tile(" + x + "," + y + ")";
            c.Init(this, x, y);
            _gridCells[x, y] = c;
        }

        protected void PostCreateCell(int x, int y)
        {
            BaseHexCell c = _gridCells[x, y];
            c.PostCreate();
        }

        protected virtual void RepositionCell (int x, int y)
        {
            Vector3 pos = GetPositionForCell(x, y);

            BaseHexCell c = _gridCells[x, y];
            c.transform.localPosition = new Vector3(pos.x, pos.y, 0f);
            c.name = "Tile(" + x + "," + y + ")";
        }

        public virtual Vector3 GetPositionForCell(int x, int y)
        {
            float yPos = -ySize * y;
            float xPos = xSize * x;

            if (x % 2 == 1)
            {
                yPos -= ySize / 2;
            }
            return new Vector3(xPos, yPos, 0f);
        }

        public bool repositionOnFixedUpdate = false;

        public bool forceRefresh = false;

        void FixedUpdate ()
        {
            if (!_inited)
            {
                return;
            }

            if (repositionOnFixedUpdate)
            {
                IterateHex_Simple(RepositionCell);
            }

            if (forceRefresh)
            {
                forceRefresh = false;
                Redraw();
            }
        }

        protected virtual void DrawSprites()
        {
            IterateHex_Simple(CreateCellSprite);
        }

        protected void CreateCellSprite (int x, int y)
        {
            BaseHexCell cell = GetCellSafe(x, y);
            cell.DrawSprites();
        }

        public BaseHexCell GetCellSafe(int x, int y)
        {
            if (x < 0)
            {
                x = 0;
            }
            else if (x >= xCount)
            {
                x = xCount - 1;
            }

            if (y < 0)
            {
                y = 0;
            }
            else if (y >= yCount)
            {
                y = yCount - 1;
            }

            return _gridCells[x, y];
        }

        internal BaseHexCell GetCell(int x, int y)
        {
            if (x < 0)
            {
                return null;
            }
            else if (x >= xCount)
            {
                return null;
            }

            if (y < 0)
            {
                return null;
            }
            else if (y >= yCount)
            {
                return null;
            }

            return _gridCells[x, y];
        }

        public bool GetCellActive(int x, int y)
        {
            return GetCellSafe(x, y).isActiveInLayer;
        }

        public delegate void ProcessCell_XY(int x, int y);
        public delegate void ProcessCell(BaseHexCell cell);

        public void IterateHex_Simple(ProcessCell_XY processCell)
        {
            for (int x = 0; x < xCount; x++)
            {
                for (int y = 0; y < yCount; y++)
                {
                    processCell(x, y);
                }
            }


            //for (int y = yCount - 1; y >= 0; y--)
            //{
            //    for (int x = xCount - 1; x >= 0; x--)
            //    {
            //        processCell(x, y);
            //    }
            //}
        }

        public void IterateHex_Simple(ProcessCell processCell)
        {
            for (int y = yCount - 1; y >= 0; y--)
            {
                for (int x = xCount - 1; x >= 0; x--)
                {
                    BaseHexCell cell = GetCellSafe(x, y);
                    processCell(cell);
                }
            }
        }

        //private List<IHexTerrainListener> hexTerrainListeners = new List<IHexTerrainListener>();

        //public void AddTerrainListener (IHexTerrainListener terrainListener)
        //{
        //    hexTerrainListeners.Add(terrainListener);
        //}
    }

    public interface ITerrainObserver
    {
        void OnRedraw ();
        void OnResizeTerrain();
    }
}
