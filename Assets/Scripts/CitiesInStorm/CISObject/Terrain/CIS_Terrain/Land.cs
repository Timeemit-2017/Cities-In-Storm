using System;
using System.Collections.Generic;
using System.Text;

namespace CitesInStorm
{
    class Land:CIS_Terrain
    {
        public bool isBorder;
        public Land()
        {
            id = TerrainID.Land;
            isBorder = false;
            StormReduce = 2;
            offset = 1;
        }

        public Land(bool isBorder)
        {
            this.isBorder = isBorder;
        }

    }
}
