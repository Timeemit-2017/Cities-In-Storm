using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    ///
    /// </summary>
    public class CommonMachine : Machine
    {
        public CommonMachine(Position p, Map map) : base(p, GameVar.machineSpeed, map)
        {
            id = -1;
            offset = 1;
        }
    }
}

