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
    }
}

