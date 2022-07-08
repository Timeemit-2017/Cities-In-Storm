using System;
using System.Collections.Generic;
using System.Text;

namespace CitesInStorm
{
    public class Storm : Piece
    {

        public Storm(Position p, int speed, Map map) : base(p, speed, map)
        {
            offset = -1;
        }
    }
}
