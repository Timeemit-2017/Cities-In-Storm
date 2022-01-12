using System.Collections;
using System.Collections.Generic;

namespace CitesInStorm
{
    /// <summary>
    ///
    /// </summary>
    public class LightStorm : Storm
    {
        public LightStorm(Position p, Map map) : base(p, GameVar.stormSpeed, map)
        {
            id = 0;
            offset = -1;
        }
    }
}

