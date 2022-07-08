using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
///
/// </summary>
public class StartGameControl : MonoBehaviour
{
    public void Start()
    {
        Init();
    }

    public void Init()
    {
        // 地图
        GameVar.mapSummon.PerlinSummon();
        // 城市
        GameVar.cityControl.ResetMessage();
        GameVar.cityControl.Spawn(out _);
    }
}
