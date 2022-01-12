using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CitesInStorm
{
    static class MapDict
    {
        public static Ocean Ocean() 
        {
            return new Ocean();
        }
        public static Land Land()
        {
            return new Land();
        }
        public static LandLocked LandLocked()
        {
            return new LandLocked();
        }

        public static DeepOcean DeepOcean()
        {
            return new DeepOcean();
        }

        public static CIS_Terrain GetTerrainWithId(TerrainID id)
        {
            switch (id)
            {
                case TerrainID.Ocean:
                    return Ocean();
                case TerrainID.Land:
                    return Land();
                case TerrainID.LandLocked:
                    return LandLocked();
                case TerrainID.DeepOcean:
                    return DeepOcean();
                default:
                    Debug.LogError("未找到指定id的CIS_Terrain。");
                    return null;
            }
        }

    }
}
