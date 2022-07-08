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

        public static MapSummon mapSummon;

        public static PlayerControl playerControl;

        public static CityControl cityControl;

        public static PieceControl pieceControl;

        public static Role firstHand;

        public static Map map;

        public static int cityRange = 2;

        public static int cityCounts = 5;

        public static Vector3 BlockSize
        {
            get
            {
                return new Vector3(mapSummon.blockW, mapSummon.blockH, mapSummon.blockD);
            }
        }

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

        /// <summary>
        /// 给Position数组求差集
        /// </summary>
        /// <param name="origin">被减数</param>
        /// <param name="minus">减数</param>
        /// <returns></returns>
        static public List<T> MinusPositions<T>(T[] origin, T[] minus)
        {
            List<T> result = new List<T>();
            foreach (T originPosition in origin)
            {
                bool ifContain = false;
                foreach (T minusPosition in minus)
                {
                    if(originPosition.Equals(minusPosition))
                    {
                        ifContain = true;
                        break;
                    }
                }
                if (!ifContain)
                {
                    result.Add(originPosition);
                }
            }
            return result;
        }

        static public List<T> MinusPositions<T>(List<T> origin, List<T> minus)
        {
            List<T> result = new List<T>();
            foreach (T item in origin)
            {
                if (minus.Contains(item))
                {
                    origin.Remove(item);
                }
            }

            return result;
        }

        /// <summary>
        /// 向List批量添加元素
        /// </summary>
        /// <param name="origin">被添加的List</param>
        /// <param name="items">要添加的元素</param>
        static public void JoinArray<T>(List<T> origin, T[] items)
        {
            foreach (T item in items)
            {
                origin.Add(item);
            }
        }

        /// <summary>
        /// 用一个List覆盖另一个List
        /// </summary>
        /// <param name="target">被覆盖的List</param>
        /// <param name="items">覆盖的内容</param>
        static public void CoverTo<T>(List<T> target, List<T> items)
        {
            target.Clear();
            foreach (T item in items)
            {
                target.Add(item);
            }
        }
    }
}

