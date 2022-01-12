using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    ///
    /// </summary>
    public class Machine : Piece
    {   
        public Machine(Position p, int speed, Map map) : base(p, speed, map)
        {
            offset = 1;
        }
    }
}

