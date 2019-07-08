using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexAutotile
{
    public class Shift3IsoHexCell : IsoHexCell
    {

        public override int GetNW_Y
        {
            get
            {
                int order = (X) % 3;
                switch (order)
                {
                    case (0): return Y;
                    case (1): return Y - 1;
                    case (2): return Y ;
                }
                return Y;
            }
        }

        public override int GetNE_Y
        {
            get
            {
                int order = (X) % 3;
                switch (order)
                {
                    case (0): return Y ;
                    case (1): return Y - 1;
                    case (2): return Y - 1;
                }
                return Y;
            }
        }

        public override int GetSE_Y
        {
            get
            {
                int order = (X) % 3;
                switch (order)
                {
                    case (0): return Y + 1;
                    case (1): return Y;
                    case (2): return Y;
                }
                return -1;
            }
        }

        public override int GetSW_Y
        {
            get
            {
                int order = (X) % 3;
                switch (order)
                {
                    case (0): return Y + 1;
                    case (1): return Y ;
                    case (2): return Y + 1;
                }
                return -1;
            }
        }

 
    }
}
