using UnityEngine;
using System.Collections;
using HexAutotile;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace HexAutotile.UI
{
    public class MapEditor : MonoBehaviour, ITerrainObserver, IHexCellClickObserver
    {
        public MapDAO mapDao;
        public BaseHexTerrain hexTerrain;

        private IBrushStrategy currentBrushStrategy;
        HeightBrush heightBrush;
        GrassBrush grassBrush;
        NoneBrush noneBrush;

        public static MapEditor Instance;

        void Start()
        {
            MapEditor.Instance = this;

            InitBrushes();

            hexTerrain = mapDao.targetTerrain;
            hexTerrain.AddObserver(this);
            ReloadMap();
        }

        public void ReloadMap()
        {
            mapDao.Load();
            PrepareUI();
        }

        public void SaveMap()
        {
            mapDao.Save();
        }

        public void OnRedraw()
        {
            hexTerrain.IterateHex_Simple(delegate (BaseHexCell c) {
                HexCellClicker h = c.gameObject.GetComponent<HexCellClicker>();
                if (!h)
                {
                    h = c.gameObject.AddComponent<HexCellClicker>();
                }
                h.ClickObserver = this;
            });

            hexTerrain.DoRepositionCells();
        }

        bool clickEnabled;

        SystemTimer onMouseTimer = SystemTimer.CreateFrequency(50);

        public void OnMouseEnter(HexCellClicker hexCellClicker)
        {
            clickEnabled = true;
        }

        void FixedUpdate()
        {

        }

        private HexCellClicker currentHexCellClicker;

        private bool prevFrameClick = false;

        public void OnMouseOver(HexCellClicker hexCellClicker)
        {
            currentHexCellClicker = hexCellClicker;

            brushCells = GetTempBrushCells(hexCellClicker.HexCell);
            foreach (BaseHexCell c in brushCells)
            {
                if (!c || !c.hexCellClicker)
                {
                    continue;
                }
                c.hexCellClicker.OnBrushOver();
            }

            bool wasClick = false;
            bool leftMouse = false; ;

            if (Input.GetMouseButton(0))
            {
                wasClick = true;
                leftMouse = true;
            }
            else
            if (Input.GetMouseButton(1))
            {
                wasClick = true;
                leftMouse = false;
            }

            if (wasClick)
            {
                if (clickEnabled)
                {
                    if (onMouseTimer.EnoughTimeLeft())
                    {
                        OnMouse(hexCellClicker.HexCell, leftMouse);
                        clickEnabled = false;
                    }
                }
            }
            else
            {
                if (prevFrameClick)
                {
                    this.currentBrushStrategy.OnMouseFree();
                }
            }

            prevFrameClick = wasClick;
        }

        #region MapSize #######################################################################################

        bool canHandleUI = false;

        public InputField xField;
        public InputField yField;

        void PrepareUI()
        {
            canHandleUI = false;

            xField.text = hexTerrain.xCount + "";
            yField.text = hexTerrain.yCount + "";

            canHandleUI = true;
        }

        #endregion #######################################################################################

        #region MapSize #######################################################################################

        public void OnXSizeChanged(string value)
        {
            canHandleUI = false;

            int newX = int.Parse(value);
            newX %= 100;

            mapDao.ResizeTerrain(newX, hexTerrain.yCount);

            canHandleUI = true;
        }

        public void OnYSizeChanged(string value)
        {
            canHandleUI = false;

            int newY = int.Parse(value);
            newY %= 100;
            mapDao.ResizeTerrain(hexTerrain.xCount, newY);

            canHandleUI = true;
        }

        #endregion #######################################################################################

        #region Brush #######################################################################################

        public void OnBrushSelected(int brushId)
        {
            canHandleUI = false;

            if (brushId == 0)
            {
                SetHeightBrush();
            }
            else if (brushId == 1)
            {
                SetGrassBrush();
            }
            else
            {
                SetNoneBrush();
            }

            canHandleUI = true;
        }

        public int BrushSize = 1;

        public void OnBrushSizeSelected(int brushId)
        {
            canHandleUI = false;

            BrushSize = brushId + 1;

            canHandleUI = true;
        }

        public bool IsHeightBrush
        {
            get { return currentBrushStrategy == heightBrush; }
        }

        public bool IsGrassBrush
        {
            get { return currentBrushStrategy == grassBrush; }
        }

        public void SetHeightBrush()
        {
            currentBrushStrategy = heightBrush;
        }

        public void SetGrassBrush()
        {
            currentBrushStrategy = grassBrush;
        }

        private void SetNoneBrush()
        {
            this.currentBrushStrategy = noneBrush;
        }

        void InitBrushes()
        {
            heightBrush = new HeightBrush();
            grassBrush = new GrassBrush();
            noneBrush = new NoneBrush();

            currentBrushStrategy = heightBrush;
        }

        List<BaseHexCell> redrawCells = new List<BaseHexCell>();
        List<BaseHexCell> brushCells = new List<BaseHexCell>();

        internal void OnMouse(BaseHexCell hexCell, bool leftMouse)
        {
            currentBrushStrategy.OnMouse(brushCells, leftMouse);

            hexTerrain.DoRepositionCells();

            foreach (BaseHexCell c in redrawCells)
            {
                c.Redraw();
            }
        }

        private int prevBrush;
        private BaseHexCell prevCurrentHexCell;

        private List<BaseHexCell> GetTempBrushCells(BaseHexCell hexCell)
        {
            if (prevBrush == BrushSize && prevCurrentHexCell == hexCell)
            {
                return brushCells;
            }

            brushCells.Clear();
            brushCells.Add(hexCell);
            FillContainerWithNeighbors(brushCells, BrushSize - 1);

            redrawCells.Clear();
            brushCells.ForEach(x => redrawCells.Add(x));
            FillContainerWithNeighbors(redrawCells, 1);

            List<BaseHexCell> container = brushCells;
            prevBrush = this.BrushSize;
            prevCurrentHexCell = hexCell;

            return container;
        }

        List<BaseHexCell> tempEnumCells = new List<BaseHexCell>();

        private void FillContainerWithNeighbors(List<BaseHexCell> container, int brushSize)
        {
            for (int i = 0; i < brushSize; i++)
            {
                tempEnumCells.Clear();
                container.ForEach(x => tempEnumCells.Add(x));

                foreach (BaseHexCell c in container)
                {
                    if (!c) { continue; }
                    foreach (BaseHexCell n in c.Neighbors)
                    {
                        if (!n) { continue; }
                        if (!tempEnumCells.Contains(n))
                        {
                            tempEnumCells.Add(n);
                        }
                    }
                }

                foreach (BaseHexCell n in tempEnumCells)
                {
                    if (!n) { continue; }
                    if (!container.Contains(n))
                    {
                        container.Add(n);
                    }
                }
            }
        }

        public void OnResizeTerrain()
        {
        }

        public void OnMouseExit(HexCellClicker hexCellClicker)
        {
        }

        interface IBrushStrategy
        {
            void OnMouseDown(BaseHexCell hexCell, bool leftMouse);
            void OnMouse(BaseHexCell hexCell, bool leftMouse);
            void OnMouseUp(BaseHexCell hexCell, bool leftMouse);
            void OnMouse(List<BaseHexCell> brushCells, bool leftMouse);
            void OnMouseFree();
        }

        class HeightBrush : IBrushStrategy
        {
            public void OnMouse(BaseHexCell hexCell, bool leftMouse)
            {
                int h = MapEditor.Instance.prevCurrentHexCell.height;
                //               h = hexCell.height;
                h = targetHeight;
                if (leftMouse)
                {
                    h++;
                }
                else
                {
                    h--;
                }

                hexCell.HexTerrain.HeightMap.ApplyHeight(hexCell.X, hexCell.Y, h);
            }

            public void OnMouseDown(BaseHexCell hexCell, bool leftMouse)
            {

            }

            public void OnMouseUp(BaseHexCell hexCell, bool leftMouse)
            {
            }

            private BaseHexCell prevCenterCell = null;
            private int targetHeight = 0;

            public void OnMouse(List<BaseHexCell> brushCells, bool leftMouse)
            {
                if (!prevCenterCell)
                {
                    prevCenterCell = MapEditor.Instance.prevCurrentHexCell;
                    targetHeight = prevCenterCell.height;
                }

                //if (Instance.brushCells.Contains(prevHexCell))
                //{
                //    return;
                //}
                //prevHexCell = MapEditor.Instance.prevCurrentHexCell;

                foreach (BaseHexCell c in brushCells)
                {
                    OnMouse(c, leftMouse);
                }
            }

            public void OnMouseFree()
            {
                prevCenterCell = null;
            }
        }

        class GrassBrush : IBrushStrategy
        {
            public void OnMouse(List<BaseHexCell> brushCells, bool leftMouse)
            {
                foreach (BaseHexCell c in brushCells)
                {
                    OnMouse(c, leftMouse);
                }
            }

            public void OnMouse(BaseHexCell hexCell, bool leftMouse)
            {
                OnMouseUp(hexCell, leftMouse);
            }

            public void OnMouseDown(BaseHexCell hexCell, bool leftMouse)
            {

            }

            public void OnMouseFree()
            {
            }

            public void OnMouseUp(BaseHexCell hexCell, bool leftMouse)
            {
                TerrainLayer grassLayer = hexCell.HexTerrain.TerrainLayers[1];
                grassLayer.ApplyValue(hexCell.X, hexCell.Y, leftMouse);
                //                hexCell.HexTerrain.Redraw();
            }
        }

        class NoneBrush : IBrushStrategy
        {
            public void OnMouse(List<BaseHexCell> brushCells, bool leftMouse)
            {
            }

            public void OnMouse(BaseHexCell hexCell, bool leftMouse)
            {

            }

            public void OnMouseDown(BaseHexCell hexCell, bool leftMouse)
            {

            }

            public void OnMouseFree()
            {
            }

            public void OnMouseUp(BaseHexCell hexCell, bool leftMouse)
            {
                print("None brush selected");
            }
        }

        #endregion #######################################################################################
    }
}


