using System.Collections;
using System.Collections.Generic;

namespace CitesInStorm
{
    /// <summary>
    ///
    /// </summary>
    public class HeavyStorm : Storm
    {
        public HeavyStorm(Position p, Map map):base(p, GameVar.heavyStormSpeed, map)
        {
            id = 1;
            offset = -2;
        }
    }
}

