using System;
using System.Collections.Generic;
using System.Text;

namespace CitesInStorm
{
    class LandLocked:Land
    {
        public LandLocked()
        {
            id = TerrainID.LandLocked;
            offset = 2;
        }
    }
}
