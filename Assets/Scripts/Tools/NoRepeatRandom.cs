using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm.Tools
{
    /// <summary>
    ///
    /// </summary>
    public class NoRepeatRandom
    {
        public int min;
        public int max;

        public List<int> perhaps = new List<int>();

        /// <summary>
        /// 创建一个不允许重复的随机数工具
        /// Tip: 包括min不包括max
        /// </summary>
        /// <param name="min">最小值（包括）</param>
        /// <param name="max">最大值（不包括）</param>
        public NoRepeatRandom(int min, int max)
        {
            if(min < max)
            {
                this.min = min;
                this.max = max;
                for (int i = min; i < max; i++)
                {
                    perhaps.Add(i);
                }
            }
            else
            {
                this.min = max;
                this.max = min;
                for (int i = max; i < min; i++)
                {
                    perhaps.Add(i);
                }
            }
        }

        /// <summary>
        /// 重置获取记录
        /// </summary>
        public void Reset()
        {
            for (int i = min; i < max; i++)
            {
                perhaps.Add(i);
            }
        }

        /// <summary>
        /// 获取一个结果，如果已使用完，则返回0
        /// </summary>
        /// <returns></returns>
        public int Get(out bool isEmpty)
        {
            isEmpty = perhaps.Count == 0;
            if (isEmpty)
            {
                return 0;
            }
            int result = perhaps[Random.Range(0, perhaps.Count)];
            perhaps.Remove(result);
            return result;
        }

        public int Get()
        {
            return Get(out _);
        }

        /// <summary>
        /// 如果可能性不足自动重置的Get方法
        /// </summary>
        /// <returns></returns>
        public int SmartGet()
        {
            int result = Get(out bool isEmpty);
            if (isEmpty)
            {
                Reset();
                return Get();
            }
            else
            {
                return result;
            }
        }

    }
}

