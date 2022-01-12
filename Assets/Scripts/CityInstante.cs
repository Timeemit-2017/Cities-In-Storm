using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
/// City在引擎中的具体实现
/// </summary>
public class CityInstante : MonoBehaviour
{
    public MapSummon Summoner;
    
    public City city;

    public Position Position
    {
        get
        {
            return city.p;
        }
        set
        {
            city.p = value;
        }
    }
}
