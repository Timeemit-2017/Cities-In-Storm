using System;
using System.Collections.Generic;
using System.Text;

namespace CitesInStorm
{
    class CIS_Terrain:MapComponent
    {
        /// <summary>
        /// 防雨装置行动力减免
        /// </summary>
        public static int actionReduce = 1;

        /// <summary>
        /// 积雨云行动力减免
        /// </summary>
        public static int stormReduce = 1;

        public TerrainID id;

    }
}
