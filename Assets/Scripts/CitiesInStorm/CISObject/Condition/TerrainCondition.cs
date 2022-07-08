using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    /// 地形筛选
    /// </summary>
    public class TerrainCondition : Condition
    {

        public TerrainCondition(params object[] values)
        {
            Init(new string[3] { "TerrainID", "ActionReduce", "StormReduce" }, new object[3] { TerrainID.UnDefined, -1, -1 });
            Set(values);
        }
    }
}
