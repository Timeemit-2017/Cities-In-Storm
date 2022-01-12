using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CitesInStorm
{
    public struct Position
    {
        public int x;
        public int y;

        public int r
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public int c
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Position Plus(int width, int height)
        {
            return new Position(x + width, y + height);
        }

        /// <summary>
        /// 判断自身是否与另一个Position相等。（是否为同一坐标）
        /// </summary>
        /// <param name="target">另一个Position</param>
        /// <returns></returns>
        public bool Equals(Position target)
        {
            return this.x == target.x && this.y == target.y;
        }
        public string OutPut()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ")";
        }

        public int Manhattan(Position p)
        {
            return Mathf.Abs(p.x - this.x) + Mathf.Abs(p.y - this.y);
        }

        public Position[] Near(int width = -1, int height = -1)
        {
            List<Position> positions = new List<Position>();
            if(width == -1 && height == -1)
            {
                positions.Add(new Position(x - 1, y));
                positions.Add(new Position(x + 1, y));
                positions.Add(new Position(x, y - 1));
                positions.Add(new Position(x, y + 1));
                return positions.ToArray();
            }
            if(x != 0)
            {
                positions.Add(new Position(x - 1, y));
            }
            if(x != width)
            {
                positions.Add(new Position(x + 1, y));
            }
            if(y != 0)
            {
                positions.Add(new Position(x, y - 1));
            }
            if(y != height)
            {
                positions.Add(new Position(x, y + 1));
            }
            return positions.ToArray();
        }
    
        public Position[] Expland(int plusOnX, int plusOnY, bool isAllowedNagative=false, int width = -1, int height= -1)
        {
            List<Position> positions = new List<Position>();

            for(int y = 0; plusOnY / Mathf.Abs(plusOnY) * y < Mathf.Abs(plusOnY); y+= plusOnY / Mathf.Abs(plusOnY))
            {
                for(int x = 0; plusOnX / Mathf.Abs(plusOnX) * x < Mathf.Abs(plusOnX); x+= plusOnX / Mathf.Abs(plusOnX))
                {
                    if (!isAllowedNagative && (this.x + x < 0 || this.y + y < 0))
                    {
                        break;
                    }
                    else if(width != -1 && height != -1 && (this.x + x >= width || this.y + y >= height))
                    {
                        break;
                    }
                    positions.Add(new Position(this.x + x, this.y + y));
                }
            }
            return positions.ToArray();
        }
    }
}
