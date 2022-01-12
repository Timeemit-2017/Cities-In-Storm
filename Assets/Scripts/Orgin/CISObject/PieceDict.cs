using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    ///
    /// </summary>
    public class PieceDict
    {
        public static CommonMachine CommonMachine(Position p, Map map)
        {
            return new CommonMachine(p, map);
        }

        public static LightStorm LightStorm(Position p, Map map)
        {
            return new LightStorm(p, map);
        }
        public static HeavyStorm HeavyStorm(Position p, Map map)
        {
            return new HeavyStorm(p, map);
        }

        public static Piece GetPieceWithID(PieceID id, Position p, Map map)
        {
            switch (id)
            {
                case PieceID.CommonMachine:
                    return CommonMachine(p, map);

                case PieceID.LightStorm:
                    return LightStorm(p, map);
                case PieceID.HeavyStorm:
                    return HeavyStorm(p, map);
            }
            return null;
        }
    }
}
