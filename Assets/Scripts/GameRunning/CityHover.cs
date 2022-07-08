using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CitesInStorm;

/// <summary>
/// 处理鼠标悬停在城市上的脚本
/// </summary>
public class CityHover : MonoBehaviour
{
    public CityInstante cityInstante;

    public bool CheckMouseIn()
    {
        Vector3 offset = new Vector3(GameVar.mapSummon.blockW, GameVar.mapSummon.blockH, 0);
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position - cityInstante.city.range / 2 * offset);
        Vector3 pos2 = Camera.main.WorldToScreenPoint(transform.position + cityInstante.city.range / 2 * offset);
        return GameVar.CheckVectorIn(pos, pos2, Input.mousePosition);
    }


}
