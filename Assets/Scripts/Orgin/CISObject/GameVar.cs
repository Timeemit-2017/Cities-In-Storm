using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    ///
    /// </summary>
    public static class GameVar
    {
        public static int stormSpeed = 5;
        public static int heavyStormSpeed = 3;

        public static int machineSpeed = 4;

        public static Role role = Role.Weather;

        public static GameObject pieceTemp;
        public static Block blockBeingOperated;

        public static Blocks blocks;

        /// <summary>
        /// 检测main坐标是否在一个由pos1, pos2组成的矩形当中
        /// </summary>
        /// <param name="pos1">矩形的第一个的点</param>
        /// <param name="pos2">矩形的第二个点</param>
        /// <param name="main">要被检测的坐标</param>
        /// <returns></returns>
        static public bool CheckVectorIn(Vector2 pos1, Vector2 pos2, Vector2 main)
        {
            if(pos1.x != pos2.x && pos1.y != pos2.y)
            {
                return pos1.x < main.x && main.x < pos2.x && pos1.y < main.y && main.y < pos2.y
                    || pos2.x < main.x && main.x < pos1.x && pos2.y < main.y && main.y < pos1.y;
            }
            else
            {
                return false;
            }
        }

    }
}

