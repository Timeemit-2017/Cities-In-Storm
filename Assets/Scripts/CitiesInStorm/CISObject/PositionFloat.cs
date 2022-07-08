using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CitesInStorm
{
    /// <summary>
    ///
    /// </summary>
    public class PositionFloat
    {
        public float x;
        public float y;

        public float r
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
        public float c
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

        /// <summary>
        /// 邻近的格是否包括四个角
        /// </summary>
        public static bool isAngel = true;


        public PositionFloat(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public PositionFloat Plus(float width, float height)
        {
            return new PositionFloat(x + width, y + height);
        }

        public string OutPut()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ")";
        }

        /// <summary>
        /// 曼哈顿距离
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public float Manhattan(PositionFloat p)
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
        public PositionFloat[] Near(float width = -1, float height = -1)
        {
            List<PositionFloat> positions = new List<PositionFloat>();
            positions.Add(new PositionFloat(x - 1, y));
            positions.Add(new PositionFloat(x + 1, y));
            positions.Add(new PositionFloat(x, y - 1));
            positions.Add(new PositionFloat(x, y + 1));
            if (isAngel)
            {
                positions.Add(new PositionFloat(x - 1, y - 1));
                positions.Add(new PositionFloat(x + 1, y + 1));
                positions.Add(new PositionFloat(x + 1, y - 1));
                positions.Add(new PositionFloat(x - 1, y + 1));
            }
            if (width != -1 && height != -1)
            {
                List<PositionFloat> temp = new List<PositionFloat>();
                foreach (PositionFloat position in positions)
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
        /// </summary>
        /// <param name="plusOnX">在x轴上的增量</param>
        /// <param name="plusOnY">在y轴上的增量</param>
        /// <param name="isAllowedNagative">是否允许出现负的坐标（默认为false）</param>
        /// <param name="width">width边界</param>
        /// <param name="height">height边界</param>
        /// <returns></returns>
        public PositionFloat[] Expand(float plusOnX, float plusOnY, bool isAllowedNagative = false, float width = -1, float height = -1)
        {
            List<PositionFloat> positions = new List<PositionFloat>();

            // plusOnY / Mathf.Abs(plusOnY) 表示plusOnY的正负号
            for (float y = 0; plusOnY / Mathf.Abs(plusOnY) * y < Mathf.Abs(plusOnY); y += plusOnY / Mathf.Abs(plusOnY))
            {
                for (float x = 0; plusOnX / Mathf.Abs(plusOnX) * x < Mathf.Abs(plusOnX); x += plusOnX / Mathf.Abs(plusOnX))
                {
                    if (!isAllowedNagative && (this.x + x < 0 || this.y + y < 0))
                    {

                    }
                    else if (width != -1 && height != -1 && (this.x + x >= width || this.y + y >= height))
                    {

                    }
                    else
                    {
                        positions.Add(new PositionFloat(this.x + x, this.y + y));
                    }
                }
            }
            return positions.ToArray();
        }

        /// <summary>
        /// 检测x，y值是否满足给定的范围（包头不包尾）[min, max)
        /// </summary>
        /// <returns></returns>
        public bool CheckRange(float xMin, float xMax, float yMin, float yMax)
        {
            return xMin <= x && x < xMax && yMin <= y && y < yMax;
        }

        /// <summary>
        /// x和y值在同一个范围内检测
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public bool CheckRange(float min, float max)
        {
            return CheckRange(min, max, min, max);
        }

        /// <summary>
        /// 返回距离自己最近的边
        /// </summary>
        /// <param name="datas">一堆边</param>
        /// <returns></returns>
        public float GetClosest(float[] xAxis, float[] yAxis)
        {
            float result = -1;
            foreach (float x in xAxis)
            {
                float thisResult = Manhattan(new PositionFloat(x, this.y));
                if (result == -1 || thisResult < result)
                {
                    result = thisResult;
                }
            }
            foreach (float y in yAxis)
            {
                float thisResult = Manhattan(new PositionFloat(this.x, y));
                if (result == -1 || thisResult < result)
                {
                    result = thisResult;
                }
            }
            return result;
        }

        /// <summary>
        /// 欧拉距离
        /// </summary>
        /// <param name="target">另一个PositionFloat</param>
        /// <returns></returns>
        public float GetDistance(PositionFloat target)
        {
            return (float)Math.Sqrt(Math.Pow(x - target.x, 2) + Math.Pow(y - target.y, 2));
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
        public bool Equals(PositionFloat target)
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

        public static bool operator ==(PositionFloat p1, PositionFloat p2)
        {
            return Equals(p1, p2);
        }

        public static bool operator !=(PositionFloat p1, PositionFloat p2)
        {
            return p1.x != p2.x || p1.y != p2.y;
        }

    }
}

