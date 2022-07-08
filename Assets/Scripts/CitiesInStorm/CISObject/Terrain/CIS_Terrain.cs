using System;
using System.Collections.Generic;
using System.Text;

namespace CitesInStorm
{
    public class CIS_Terrain:CISObject
    {

        public static List<TerrainID> oceans = new List<TerrainID>() { TerrainID.Ocean, TerrainID.DeepOcean };
        public static List<TerrainID> lands = new List<TerrainID>() { TerrainID.Land, TerrainID.LandLocked };

        /// <summary>
        /// 防雨装置行动力减免
        /// </summary>
        public int ActionReduce
        {
            get
            {
                return reduceData[Role.Human];
            }
            set
            {
                reduceData[Role.Human] = value;
            }
        }

        /// <summary>
        /// 积雨云行动力减免
        /// </summary>
        public int StormReduce
        {
            get
            {
                return reduceData[Role.Weather];
            }
            set
            {
                reduceData[Role.Weather] = value;
            }
        }

        public Dictionary<Role, int> reduceData = new Dictionary<Role, int>
        {
            {Role.Human, 1 },
            {Role.Weather, 1 }
        };

        public TerrainID id;

        /// <summary>
        /// 表示地形相对于海平面的高度
        /// </summary>
        public int offset = 0;

        /// <summary>
        /// 获取地形信息组
        /// </summary>
        /// <returns></returns>
        public TerrainCondition GetTerrainCondition()
        {
            return new TerrainCondition(id, ActionReduce, StormReduce);
        }
    }
}
