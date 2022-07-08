using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;
using CitesInStorm.Tools;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 城市控制类
/// </summary>
public class CityControl : MonoBehaviour
{
    public GameObject CityPrefab;

    /*public void OnGUI()
    {
        if (GUILayout.Button("test"))
        {
            Spawn(out string reason);
            //Debug.Log(reason);
        }
        if (GUILayout.Button("csvTest"))
        {
            List<List<string>> data = CsvDecoder.Read("Assets/Data/City.csv", System.Text.Encoding.GetEncoding("GB2312"));
            Debug.Log(data[2][0]);
        }
    }*/

    public int CityRange
    {
        get
        {
            return GameVar.cityRange;
        }
        set
        {
            GameVar.cityRange = value;
        }
    }

    public int CityCounts
    {
        get
        {
            return GameVar.cityCounts;
        }
        set
        {
            GameVar.cityCounts = value;
        }
    }

    public City[] cities;

    public Dictionary<PositionFloat, CityInstante> cityInstantes;

    // 定义一个字典，便于通过某一个坐标查询城市中心
    public Dictionary<Position, PositionFloat> cityMiddleSearch;

    public List<Position> positionsThatInCity;

    public List<PositionFloat> positionFloatsCity;  // 每个城市的中心

    public bool hasInit = false;

    public List<List<string>> csvData;

    public bool isComplusiveShowName = false;

    public void Awake()
    {
        GameVar.cityControl = this;
    }

    public void Update()
    {
        if(hasInit)
        {
            ControlCityHover();
        }
        if (Input.GetKey(KeyCode.F))
        {
            isComplusiveShowName = true;
        }
        else
        {
            isComplusiveShowName = false;
        }
    }

    public void ResetMessage()
    {
        cities = new City[CityCounts];
        cityInstantes = new Dictionary<PositionFloat, CityInstante>();
        positionsThatInCity = new List<Position>();
        positionFloatsCity = new List<PositionFloat>();
        cityMiddleSearch = new Dictionary<Position, PositionFloat>();
    }

    public void ResetModel()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void ControlCityHover()
    {
        foreach (KeyValuePair<PositionFloat, CityInstante> item in cityInstantes)
        {
            CityHover cityHover = item.Value.GetComponent<CityHover>();
            CityInstante cityInstante = item.Value;

            bool basicCondition = isComplusiveShowName || cityHover.CheckMouseIn();

            if (basicCondition && !cityInstante.active)
            {
                cityInstante.DisplayCityMessage(true);
            }
            else if(!basicCondition && cityInstante.active)
            {
                cityInstante.DisplayCityMessage(false);
            }
        }
    }


    private List<Position> GetAvailableSpawnPosition(out bool isSuccess)
    {
        TerrainCondition tc = new TerrainCondition(TerrainID.Land);
        List<Position> lands = GameVar.map.GetTerrains(tc);  // 地图内全部Land
        List<Position> cityLands = new List<Position>();  // 选中用来放置城市的Land
        // 随机点位生成城市
        for (int i = 0; i < CityCounts;)
        {
            int index = Random.Range(0, lands.Count);
            bool ifJoin = true;  // 这个坐标是否可以生成城市
            Position[] positionThatInThisCity = lands[index].Expand(CityRange, CityRange);
            float RangeHalf = (CityRange - 1) / 2.0f;
            PositionFloat CityMiddle = new PositionFloat(lands[index].x + RangeHalf, lands[index].y + RangeHalf);
            // 检测城市范围内是否有不允许出现的地形 或 城市超出了地图边界
            foreach (Position p in positionThatInThisCity)
            {
                if (!p.CheckRange(0, GameVar.map.Width, 0, GameVar.map.Height) || !GameVar.map.GetTerrain(p).Equals(TerrainID.Land) || positionsThatInCity.Contains(p))
                {
                    ifJoin = false;
                    break;
                }
            }
            // 控制城市之间的距离
            if (ifJoin)
            {
                foreach (PositionFloat item in positionFloatsCity)
                {
                    if(CityMiddle.Manhattan(item) <= 4)
                    {
                        ifJoin = false;
                        break;
                    }
                }
            }
            // 确认能够生成城市的操作
            if (ifJoin)
            {
                cityLands.Add(lands[index]);
                positionFloatsCity.Add(CityMiddle);
                i++;
                GameVar.JoinArray(positionsThatInCity, positionThatInThisCity);
                foreach (Position item in positionThatInThisCity)
                {
                    cityMiddleSearch.Add(item, CityMiddle);
                }
            }
            lands.RemoveAt(index);
            if (lands.Count == 0)
            {
                // 陆地格不足以生成足够的城市
                isSuccess = false;
                
                return cityLands;
            }
        }
        isSuccess = true;
        return cityLands;
    }

    private List<Position> SmartGetAvailableSpawnPosition()
    {
        List<Position> result = GetAvailableSpawnPosition(out bool isSuccess);
        if (!isSuccess)
        {
            CityCounts = result.Count;
        }
        return result;
    }

    /// <summary>
    /// 生成城市
    /// </summary>
    /// <returns>是否生成成功</returns>
    public bool Spawn(out string reason)
    {
        if (hasInit)
        {
            ResetMessage();
            ResetModel();
        }

        List<Position> cityLands = SmartGetAvailableSpawnPosition();
        if(cityLands == null)
        {
            reason = "LandsNotEnough";
            return false;
        }

        csvData = CsvDecoder.Read("Assets/Data/City.csv", System.Text.Encoding.GetEncoding("GB2312"));
        List<string> cityNames = new List<string>();
        foreach (List<string> item in csvData)
        {
            cityNames.Add(item[0]);
        }
        cityNames.RemoveRange(0, 2);  // 去除表头和类型标识
        NoRepeatRandom noRepeatRandom = new NoRepeatRandom(0, cityNames.Count);  // 实例化不重复随机数工具

        // 实例化City
        for (int i = 0; i < cityLands.Count; i++)
        {
            GameVar.map.FindNearTerrain(cityLands[i], TerrainID.DeepOcean, out int distance);  //用于获取最近的海洋单元格与城市的距离
            float Bound = (GameVar.map.Width + GameVar.map.Height) / 2;  //长宽平均值

            int defend = (int)(distance / Bound * 5);
            if(defend <= 0)
            {
                defend = 1;
            }

            int cityNameIndex = noRepeatRandom.SmartGet();  // 随机一个城市名索引

            City temp = new City(cityLands[i], defend, cityNames[cityNameIndex], CityRange);
            cities[i] = temp;

            GameObject city = Instantiate(CityPrefab, this.transform);
            
            

            //初始化CityInstante并对正模型
            CityInstante cityInstante = city.GetComponent<CityInstante>();
            cityInstante.city = temp;
            cityInstante.SetPosition();
            if(!GameVar.map.CompareDistanceToTerrains(temp.positionsInCity, new TerrainCondition(TerrainID.LandLocked), 1, true))
            {
                cityInstante.SetCityMessageZ(1);
            }

            //添加一个通过城市中心查询城市实例的字典
            cityInstantes.Add(temp.GetCityMiddle(), cityInstante);

            //初始化CityHover组件
            CityHover cityHover = city.GetComponent<CityHover>();
            cityHover.cityInstante = cityInstante;

            //初始化City信息
            cityInstante.InitCityMessage();

        }

        reason = "success";
        hasInit = true;
        return true;
    }

    public bool CheckIfInCity(Position p, out CityInstante cityInstante)
    {
        bool result = positionsThatInCity.Contains(p);
        if (result)
        {
            cityInstante = cityInstantes[cityMiddleSearch[p]];
        }
        else
        {
            cityInstante = null;
        }
        return result;
    }

    public bool CheckIfInCity(Position p)
    {
        return CheckIfInCity(p, out _);
    }

}
