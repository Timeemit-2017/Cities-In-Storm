using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CitesInStorm
{
    public class Position : PositionFloat
    {
        public new int x;
        public new int y;

        public new int r
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
        public new int c
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

        public Position(int x, int y) : base(x, y)
        {
            this.x = x;
            this.y = y;
        }

        public Position Plus(int width, int height)
        {
            return new Position(x + width, y + height);
        }

        public int Manhattan(Position p)
        {
            return Mathf.Abs(p.x - this.x) + Mathf.Abs(p.y - this.y);
        }

        /// <summary>
        /// 获取周围的方格
        /// （是否包括四个角取决于isAngel这个静态变量）
        /// </summary>
        /// <param name="width">地图宽度</param>
        /// <param name="height">地图高度</param>
        /// <returns></returns>
        public Position[] Near(int width = -1, int height = -1)
        {
            List<Position> positions = new List<Position>();
            positions.Add(new Position(x - 1, y));
            positions.Add(new Position(x + 1, y));
            positions.Add(new Position(x, y - 1));
            positions.Add(new Position(x, y + 1));
            if (isAngel)
            {
                positions.Add(new Position(x - 1, y - 1));
                positions.Add(new Position(x + 1, y + 1));
                positions.Add(new Position(x + 1, y - 1));
                positions.Add(new Position(x - 1, y + 1));
            }
            if (width != -1 && height != -1)
            {
                List<Position> temp = new List<Position>();
                foreach (Position position in positions)
                {
                    if (position.CheckRange(0, width, 0, height))
                    {
                        temp.Add(position);
                    }
                }
                positions = temp;
            }
            return positions.ToArray();
        }

        /// <summary>
        /// 返回在 [this.x + plusOnX, this.y + plusOnY]这个区间的所有坐标
        /// 注：不包括自己本身
        /// 注2：添加的顺序为“先行后列，从左到右，从下到上（增量为负时从右到左 / 从上到下）”
        /// </summary>
        /// <param name="plusOnX">在x轴上的增量</param>
        /// <param name="plusOnY">在y轴上的增量</param>
        /// <param name="isAllowedNagative">是否允许出现负的坐标（默认为false）</param>
        /// <param name="width">width边界</param>
        /// <param name="height">height边界</param>
        /// <returns></returns>
        public Position[] Expand(int plusOnX, int plusOnY, bool isAllowedNagative = false, int width = -1, int height = -1)
        {
            List<Position> positions = new List<Position>();

            // plusOnY / Mathf.Abs(plusOnY) 表示plusOnY的正负号
            for (int y = 0; plusOnY / Mathf.Abs(plusOnY) * y < Mathf.Abs(plusOnY); y += plusOnY / Mathf.Abs(plusOnY))
            {
                for (int x = 0; plusOnX / Mathf.Abs(plusOnX) * x < Mathf.Abs(plusOnX); x += plusOnX / Mathf.Abs(plusOnX))
                {
                    if (!isAllowedNagative && (this.x + x < 0 || this.y + y < 0))
                    {
                        // 负数判定
                    }
                    else if (width != -1 && height != -1 && (this.x + x >= width || this.y + y >= height))
                    {
                        // 边界判定
                    }
                    else
                    {
                        // 条件都满足，确定结果
                        positions.Add(new Position(this.x + x, this.y + y));
                    }
                }
            }
            return positions.ToArray();
        }

        /// <summary>
        /// 检测x，y值是否满足给定的范围（包头不包尾）[min, max)
        /// </summary>
        /// <returns></returns>
        public bool CheckRange(int xMin, int xMax, int yMin, int yMax)
        {
            return xMin <= x && x < xMax && yMin <= y && y < yMax;
        }

        /// <summary>
        /// x和y值在同一个范围内检测
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public bool CheckRange(int min, int max)
        {
            return CheckRange(min, max, min, max);
        }

        /// <summary>
        /// 返回距离自己最近的边
        /// </summary>
        /// <param name="datas">一堆边</param>
        /// <returns></returns>
        public int GetClosest(int[] xAxis, int[] yAxis)
        {
            int result = -1;
            foreach (int x in xAxis)
            {
                int thisResult = Manhattan(new Position(x, this.y));
                if (result == -1 || thisResult < result)
                {
                    result = thisResult;
                }
            }
            foreach (int y in yAxis)
            {
                int thisResult = Manhattan(new Position(this.x, y));
                if (result == -1 || thisResult < result)
                {
                    result = thisResult;
                }
            }
            return result;
        }

        /// <summary>
        /// 以字符串的形式返回坐标值
        /// </summary>
        /// <returns></returns>
        public new string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ")";
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((PositionFloat)obj);
        }


        /// <summary>
        /// 判断自身是否与另一个Position相等。（是否为同一坐标）
        /// </summary>
        /// <param name="target">另一个Position</param>
        /// <returns></returns>
        public bool Equals(Position target)
        {
            if (target == null)
            {
                return false;
            }
            return x == target.x && y == target.y;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return Equals(p1, p2);
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return p1.x != p2.x || p1.y != p2.y;
        }
    }
}
