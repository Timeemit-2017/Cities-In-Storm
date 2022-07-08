using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    /// 条件（基类）
    /// 用于筛选所需要的元素
    /// </summary>
    public class Condition
    {
        public Dictionary<string, object> data = new Dictionary<string, object>();
        public Dictionary<string, object> defaultData = new Dictionary<string, object>();

        public string[] names;

        public int Count
        {
            get
            {
                return names.Length;
            }
        }

        /*
        public Condition(params object[] values)
        {
            Init(new string[1] { "varName" }, new object[1] { "value" });
            Set(values);
        }
        */

        public void Init(string[] names, object[] defaults)
        {
            this.names = names;
            for(int i = 0; i < names.Length; i++)
            {
                defaultData[names[i]] = defaults[i];
            }
        }

        public void Set(params object[] values)
        {
            for (int i = 0; i < Count; i++)
            {
                if (i >= values.Length || values[i].ToString() == "__default__")
                {
                    data[names[i]] = defaultData[names[i]];
                }
                else
                {
                    data[names[i]] = values[i];
                }
            }
        }

        /// <summary>
        /// 将data数据转换成一个字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(data);
        }

        /// <summary>
        /// 将指定的Dictionary转变成一个字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string ToString(Dictionary<string, object> data)
        {
            string result = "";
            foreach (KeyValuePair<string, object> pair in data)
            {
                result += $"{pair.Value},";
            }
            return result.Remove(result.Length - 1);
        }

        /// <summary>
        /// 返回一个string[] 包含所有data数据
        /// </summary>
        /// <returns></returns>
        public string[] SplitToString()
        {
            return ToString().Split(',');
        }

        /// <summary>
        /// 返回已经被定义的data数据下标
        /// </summary>
        /// <returns></returns>
        public int[] CheckUnDefined()
        {
            List<int> result = new List<int>();
            string[] splitedData = SplitToString();
            string[] splitedDefaultData = ToString(defaultData).Split(',');
            for (int i = 0; i < splitedData.Length; i++)
            {
                if (splitedData[i] != splitedDefaultData[i])
                {
                    result.Add(i);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 返回几个指定下标的data数据
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public object[] GetValues(int[] limit)
        {
            List<object> result = new List<object>(limit.Length);
            foreach (int item in limit)
            {
                result.Add(SplitToString()[item]);
            }
            return result.ToArray();
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Condition temp = (Condition)obj;
            int[] thisDefined = CheckUnDefined();
            int[] tempDefined = temp.CheckUnDefined();
            int[] limit;
            if(thisDefined.Length > tempDefined.Length)
            {
                limit = tempDefined;
            }
            else
            {
                limit = thisDefined;
            }

            object[] thisData = GetValues(limit);
            object[] otherData = temp.GetValues(limit);
            for(int i = 0; i < limit.Length; i++)
            {
                if(!thisData[i].Equals(otherData[i]))
                {
                    return false;
                }
            }
            return true;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(Condition tc1, Condition tc2)
        {
            return tc1.Equals(tc2);
        }

        public static bool operator !=(Condition tc1, Condition tc2)
        {
            return !tc1.Equals(tc2);
        }

    }
}

