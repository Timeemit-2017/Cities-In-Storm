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
        }

        public Land(bool isBorder)
        {
            this.isBorder = isBorder;
        }

    }
}
