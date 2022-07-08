using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    /// 数值地图
    /// </summary>
    public class Map
    {
        public TerrainID[,] map;
        private int width;
        private int height;

        public int Width
        {
            get
            {
                return width;
            }

            set
            {   
                if(value > 0)
                {
                    width = value;
                }
            }

        }
        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                if(value > 0)
                {
                    height = value;
                }
            }
        }

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new TerrainID[height, width];
        }

        public TerrainID GetTerrain(Position p)
        {
            return map[p.r, p.c];
        }

        /// <summary>
        /// 柏林噪声生成
        /// </summary>
        /// <param name="seed"></param>
        public void PerlinNoise(int seed, float scale)
        {
            Random.InitState(seed);
            float xOrg = Random.Range(0, (float)seed * 355 + 1299);
            float yOrg = Random.Range(0, (float)seed * 355 + 1299);
            for (int yCount = 0; yCount < Height; yCount++)
            {
                for (int xCount = 0; xCount < Width; xCount++)
                {
                    float xCloord = xOrg + xCount * scale;
                    float yCloord = yOrg + yCount * scale;
                    float temp = Mathf.PerlinNoise(xCloord, yCloord);
                    if (0 <= temp && temp < 0.35)
                    {
                        map[yCount, xCount] = TerrainID.DeepOcean;
                    }
                    else if (0.35 <= temp && temp < 0.5)
                    {
                        map[yCount, xCount] = TerrainID.Ocean;
                    }
                    else if (0.5 <= temp && temp < 0.75)
                    {
                        map[yCount, xCount] = TerrainID.Land;
                    }
                    else if (0.75 <= temp && temp <= 1.0)
                    {
                        map[yCount, xCount] = TerrainID.LandLocked;
                    }
                }
            }
            Random.InitState((int)System.DateTime.Now.Ticks);
            SetOceanNearLand();
        }

        /// <summary>
        /// 保证陆地周围的海域是浅海
        /// </summary>
        public void SetOceanNearLand()
        {
            for(int r = 0; r < height; r++)
            {
                for(int c = 0; c < width; c++)
                {
                    if(map[r, c] == TerrainID.Land)
                    {
                        ReplaceNear(r, c, TerrainID.DeepOcean, TerrainID.Ocean);
                    }
                }
            }
        }

        public void Replace(int r, int c, TerrainID fromID, TerrainID toID)
        {
            if(map[r, c] == fromID)
            {
                map[r, c] = toID;
            }
        }
        public void Replace(Position p, TerrainID fromID, TerrainID toID)
        {
            Replace(p.r, p.c, fromID, toID);
        }

        /// <summary>
        /// 返回周围的地形
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public int[][] FindNear(int r, int c)
        {
            int[][] result = new int[4][];
            int i = 0;
            if(r == 0)
            {
                result[0] = new int[] { r + 1, c };
                i++;
            }
            else if(r == height - 1)
            {
                result[0] = new int[] { r - 1, c };
                i++;
            }
            else
            {
                result[0] = new int[] { r + 1, c };
                result[1] = new int[] { r - 1, c };
                i += 2;
            }

            if (c == 0)
            {
                result[i] = new int[] { r, c + 1 };
                i++;
            }
            else if (c == width - 1)
            {
                result[i] = new int[] { r, c - 1 };
                i++;
            }
            else
            {
                result[i] = new int[] { r, c + 1 };
                result[i + 1] = new int[] { r, c - 1 };
                i += 2;
            }
            if(i < 4)
            {
                int[][] result2 = new int[i][];
                int deviation = 0;
                for (int j = 0; j < 4; j++)
                {   
                    if(result[j] != null)
                    {
                        result2[j - deviation] = result[j];
                    }
                    else
                    {
                        deviation++;
                    }
                }
                result = result2;
            }
            return result;
        }
        public Position[] FindNear(Position p)
        {
            int[][] temp = FindNear(p.r, p.c);
            Position[] result = new Position[temp.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                int r = temp[i][0];
                int c = temp[i][1];
                result[i] = new Position(c, r);
            }
            return result;
        }

        /// <summary>
        /// 将周围的某种地形替换成某种地形
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <param name="fromID"></param>
        /// <param name="toID"></param>
        public void ReplaceNear(int r, int c, TerrainID fromID, TerrainID toID)
        {
            int[][] temp = FindNear(r, c);
            for(int i = 0; i < temp.Length; i++)
            {
                int r1 = temp[i][0];
                int c1 = temp[i][1];
                if (map[r1, c1] == fromID)
                {
                    map[r1, c1] = toID;
                }
            }
        }
        public void ReplaceNear(Position p, TerrainID fromID, TerrainID toID)
        {
            ReplaceNear(p.r, p.c, fromID, toID);
        }

        /// <summary>
        /// 寻找距离start点最近的某种地形
        /// </summary>
        /// <param name="start"></param>
        /// <param name="target">地形类型</param>
        /// <param name="distance">结果点与start点的距离（不考虑移动力减免）</param>
        /// <returns>地形的坐标</returns>
        public Position FindNearTerrain(Position start, TerrainID target, out int distance)
        {
            int steps = 0;  //距离

            // 开始格的地形就是所求的情况
            if (GetTerrain(start) == target)
            {
                distance = steps;
                return start;
            }

            List<Position> expands = new List<Position>();  //要扩张的格子
            List<Position> lastExpands = new List<Position>();  //上一回合扩张的格子
            List<Position> nears = new List<Position>();
            expands.Add(start);
            while (expands.Count != 0)
            {
                steps++;
                foreach (Position expand in expands)
                {
                    GameVar.JoinArray(nears, expand.Near(Width, Height));
                }
                expands = GameVar.MinusPositions(expands, lastExpands);
                foreach (Position near in nears)
                {
                    if(GetTerrain(near) == target)
                    {
                        distance = steps;
                        return near;
                    }
                }
                GameVar.CoverTo(lastExpands, expands);
                GameVar.CoverTo(expands, nears);
            }
            distance = -1;
            return new Position(0, 0);
        }
        
        /// <summary>
        /// 寻找距离某一片区域最近的某种地形
        /// </summary>
        /// <returns></returns>
        public Position FindNearTerrain_Area(Position[] start, TerrainID target, out int distance)
        {
            Position minPosition = FindNearTerrain(start[0], target, out int thisDistance); ;
            distance = thisDistance;
            for(int i = 0; i < start.Length; i++)
            {
                Position temp = FindNearTerrain(start[i], target, out int distanceTemp);
                if(distanceTemp < distance)
                {
                    minPosition = temp;
                    distance = distanceTemp;
                }
            }
            return minPosition;
        }

        /// <summary>
        /// 将目标点与目标地形的距离与目标距离相比较
        /// 为减少性能消耗，不会求出真实的距离。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="tc"></param>
        /// <param name="requiredDistance"></param>
        /// <param name="isGreater">是否是大于。如果是false，表示小于</param>
        /// <returns></returns>
        public bool CompareDistanceToTerrain(Position start, TerrainCondition tc, int requiredDistance, bool isGreater)
        {
            List<Position> expands = new List<Position>();  //要扩张的格子
            List<Position> lastExpands = new List<Position>();  //上一回合扩张的格子
            List<Position> nears = new List<Position>();
            expands.Add(start);
            for(int i = 0; i < requiredDistance; i++)
            {
                foreach (Position expand in expands)
                {
                    GameVar.JoinArray(nears, expand.Near(Width, Height));
                }
                expands = GameVar.MinusPositions(expands, lastExpands);
                foreach (Position near in nears)
                {
                    if (MapDict.GetTerrainWithId(GetTerrain(near)).GetTerrainCondition() == tc)
                    {
                        return !isGreater;
                    }
                }
                GameVar.CoverTo(lastExpands, expands);
                GameVar.CoverTo(expands, nears);
            }
            return isGreater;
        }

        /// <summary>
        /// 将一些目标点与目标地形的距离与目标距离相比较
        /// Tips：有一个目标点不满足条件就返回false
        /// </summary>
        /// <param name="starts"></param>
        /// <param name="tc"></param>
        /// <param name="requiredDistance"></param>
        /// <param name="isGreater"></param>
        /// <returns></returns>
        public bool CompareDistanceToTerrains(Position[] starts, TerrainCondition tc, int requiredDistance, bool isGreater)
        {   
            foreach (Position item in starts)
            {
                if(!CompareDistanceToTerrain(item, tc, requiredDistance, isGreater))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取整张地图中所有符合要求的坐标
        /// </summary>
        /// <param name="tc">筛选条件</param>
        /// <returns>符合条件的坐标</returns>
        public List<Position> GetTerrains(TerrainCondition tc)
        {
            List<Position> result = new List<Position>();
            for(int r = 0; r < Height; r++)
            {
                for(int c = 0; c < Width; c++)
                {
                    Position p = new Position(c, r);
                    TerrainID tID = GetTerrain(p);
                    if (MapDict.GetTerrainWithId(tID).GetTerrainCondition() == tc)
                    {
                        result.Add(p);
                    }
                }
            }
            return result;
        }

        public List<TerrainID> GetTerrainsInArray(Position[] target)
        {
            List<TerrainID> result = new List<TerrainID>(target.Length);
            foreach (Position item in target)
            {
                result.Add(GetTerrain(item));
            }
            return result;
        }
    }
}

