using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    /// Piece的原始派生类（没有hurt属性）
    /// 便于扩展
    /// </summary>
    public class PieceOrigin : CISObject
    {
        protected int speed;
        protected int defaultSpeed;
        public int id;
        public Map map; // 数值地图的引用

        public PieceOrigin(Position p, int speed, Map map)
        {
            defaultSpeed = speed;
            this.speed = speed;
            this.p = p;
            this.map = map;
        }


        public void SetSpeed(int newSpeed)
        {
            speed = newSpeed;
        }

        /// <summary>
        /// 给棋子速度翻倍
        /// </summary>
        /// <param name="speedTimes"></param>
        public void TimeSpeed(int speedTimes)
        {
            speed *= speedTimes;
        }

        public void ResetSpeed()
        {
            speed = defaultSpeed;
        }

        public void Move(Position newP)
        {
            p = newP;
        }

        public List<Position> BlockCanMoveTo()
        {
            List<Position> result = new List<Position>();
            List<Position> frontier = new List<Position>();
            frontier.Add(this.p);

            while (frontier.Count != 0)
            {
                Position current = frontier[0];
                frontier.Remove(current);
                result.Add(current);
                Position[] temp = map.FindNear(current);
                foreach (Position item in temp)
                {
                    if (item.Manhattan(this.p) <= speed && !result.Contains(item))
                    {
                        frontier.Add(item);
                    }
                }
            }
            return result;
        }


    }
}

