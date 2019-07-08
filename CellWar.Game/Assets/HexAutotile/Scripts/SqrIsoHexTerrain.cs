using UnityEngine;

namespace HexAutotile
{
    public class SqrIsoHexTerrain : BaseHexTerrain
    {
        public float horizontal_3XOffset = 0.25f;
        public float horizontal_6XOffset = 0.25f;

        public float horizontal_1YOffset = -0.14f;
        
        public float horizontal_2XShift = 0.5f;
        public float horizontal_2YOffset = 0.22f;

        public float vertical1_XOffset = 0.29f;
        public float vertical1_YOffset = 0.29f;
        public float vertical2_XOffset = 0.29f;
        public float vertical2_YOffset = 0.29f;

        public float group2_YOffset = 0.29f;

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
            BaseHexCell c = _gridCells[x, y];
            c.name = "Tile(" + x + "," + y + ")";
            int heightLevel = this.HeightMap.GetHeight(x, y);

            const float Z_OFFSET = 50f;

            float yPos = -ySize * y;
            float xPos = xSize * x;
            float zPos = -y + Z_OFFSET;

            int yLayer = y % 3;
            int xLayer = x % 3;

            //if (xLayer < 2)
            //{
            //    x++;
            //}

            if (x % 3 == 0 && y % 3 == 0)
            {
                c.gameObject.SetActive(false);
            }

            //if (y % 3 == 0)
            //{
            //    int xShiftNum = (x - 3 - 2) / 3 + 1;
            //    x += xShiftNum;
            //}

            //if ( y % 3 == 0)
            //{
            //    int xShiftNum = (x - 3 - 2) / 3 + 1;
            //    x += xShiftNum;
            //}

            if (x % 3 == 1)
            {
                yPos += horizontal_1YOffset;
                zPos -= 0.1f;
            }
            else
            if (x % 3 == 2)
            {
                yPos += horizontal_2YOffset;
                zPos += 0.1f;
            }

            if (x >= 2)
            {
                int xShiftNum = (x - 2) / 3 + 1;
                xPos -= horizontal_2XShift * xShiftNum;                
            }

            if (x >= 3)
            {
                int xShiftNum = (x - 3) / 3 + 1;
                xPos -= horizontal_3XOffset * xShiftNum;
            }

            if (x >= 6)
            {
                int xShiftNum = (x - 6) / 6 + 1;
                xPos -= horizontal_6XOffset * xShiftNum;
            }

            if (y % 3 == 1)
            {
                int yShiftCount = 1;
                yShiftCount = y;
                yShiftCount %= 3;

                xPos += vertical1_XOffset * yShiftCount;
                yPos += vertical1_YOffset;
            }

            if (y % 3 == 2)
            {
                int yShiftCount = 1;
                yShiftCount = y;
                yShiftCount %= 3;

                xPos += vertical2_XOffset * yShiftCount;
                yPos += vertical2_YOffset;
            }

            if (y > 2)
            {
                yPos += group2_YOffset;
            }

            yPos += heightLevel * HeightMap.heightOffsetPerLevel;
            
            c.transform.localPosition = new Vector3(xPos, yPos, zPos);
        }
    }
}
