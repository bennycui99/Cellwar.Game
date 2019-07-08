using UnityEngine;

namespace HexAutotile
{
    public class Shift3IsoHexTerrain : BaseHexTerrain
    {
        public float horizontal_1XOffset = 0f;
        public float horizontal_1YOffset = 0f;

        public float horizontal_2XOffset = 0f;
        public float horizontal_2YOffset = -0.14f;

        public float horizontal_3XOffset = 0f;
        public float horizontal_3YOffset = 0.22f;

        public float horizontal_3XShift = 0.5f;
        public float vertical_3XShift = 0.0f;

        public float nextLevel_XOffset = 0.29f;
        public float nextLevel_YOffset = 0.29f;

        public void IterateHex_Ordered(ProcessCell_XY processCell)
        {
            for (int y = yCount - 1; y >= 0; y--)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int x = xCount - 1; x >= 0; x--)
                    {
                        int order = x % 3;
                        bool timeToProcessCell = false;
                        if (i == 0 && order == 1)
                        {
                            timeToProcessCell = true;
                        }
                        else if (i == 1 && order == 0)
                        {
                            timeToProcessCell = true;
                        }
                        else if (i == 2 && order == 2)
                        {
                            timeToProcessCell = true;
                        }

                        if (timeToProcessCell)
                        {
                            processCell(x, y);
                        }
                    }
                }
            }
        }

        protected override void RepositionCell(int x, int y)
        {
            Vector3 pos = GetPositionForCell(x, y);

            BaseHexCell c = _gridCells[x, y];
            c.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
            c.name = "Tile(" + x + "," + y + ")";

            if (x * 3 + 1 < (yCount - y))
            {
                c.gameObject.SetActive(false);
            }

            if ((xCount - x) * 3 < y)
            {
                c.gameObject.SetActive(false);
            }
        }

        public float GRID_Z_OFFSET = 5f;

        public override Vector3 GetPositionForCell(int x, int y)
        {
            float yPos = -ySize * y;

            float xPos = xSize * x;

            float zPos = -y + GRID_Z_OFFSET + yCount;

            if (x % 3 == 0)
            {
                yPos += horizontal_1YOffset;
                xPos += horizontal_1XOffset;
                zPos -= 0.1f;
            } else
            if (x % 3 == 1)
            {
                yPos += horizontal_2YOffset;
                xPos += horizontal_2XOffset;
                zPos += 0.1f;
            }
            else
            if (x % 3 == 2)
            {
                yPos += horizontal_3YOffset;
                xPos += horizontal_3XOffset;
            }

            if (x > 2)
            {
                int xShiftNum = Mathf.Max(0, (x) / 3);
                xPos -= horizontal_3XShift * xShiftNum;
                yPos -= vertical_3XShift * xShiftNum;
            }

            if (y > 0)
            {
                int yShiftCount = 1;
                yShiftCount = y;

                xPos += nextLevel_XOffset * yShiftCount;
                yPos += nextLevel_YOffset * yShiftCount;
            }

            int heightLevel = this.HeightMap.GetHeight(x, y);
            yPos += heightLevel * HeightMap.heightOffsetPerLevel;

            return new Vector3(xPos, yPos, zPos);
        }
    }
}
