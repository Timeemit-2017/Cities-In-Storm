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

        public List<Position> BlockCanMoveTo_ManHattan()
        {
            List<Position> result = new List<Position>();  // 最终输出的结果
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

        public Position[] BlockCanMoveTo()
        {
            int this_block_round;
            List<Position> result = new List<Position>();
            Dictionary<Position, int> expand_dic = new Dictionary<Position, int>();
            List<Position> expand = new List<Position>();
            expand.Add(p);
            expand_dic.Add(p, speed);
            while (expand.Count != 0)
            {
                List<Position> temp = new List<Position>();
                foreach (Position item in expand)
                {
                    Position[] near = item.Near(map.Width, map.Height);
                    foreach (Position position in near)
                    {
                        this_block_round = expand_dic[item] - MapDict.GetTerrainWithId(map.GetTerrain(position)).reduceData[GameVar.role];
                        if (!expand.Contains(position) && !result.Contains(position) && !expand_dic.ContainsKey(position))
                        {   
                            if(this_block_round > 0)
                            {
                                temp.Add(position);
                                expand_dic.Add(position, this_block_round);
                            }
                            else if(this_block_round == 0)
                            {
                                result.Add(position);
                            }
                        }
                    }
                    result.Add(item);
                    expand_dic.Remove(item);
                }
                expand.Clear();
                foreach (Position item in temp)
                {
                    expand.Add(item);
                }
            }
            return result.ToArray();
        }
    }
}

