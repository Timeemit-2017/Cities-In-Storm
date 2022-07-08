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

    public bool active = false;

    public int Health
    {
        get
        {
            return city.health;
        }
        set
        {
            city.health = value;
            HealthJudge();
        }
    }

    public Transform CityNames;

    public Transform CityDefends;

    public Position LocalPosition
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

    /// <summary>
    /// 对正CityInstante
    /// </summary>
    public void SetPosition()
    {
        transform.position = GameVar.blocks.GetWorldPosition(LocalPosition, new Vector2(1, 1), true);
    }

    public void InitCityMessage()
    {
        CityNames.GetComponent<TextMeshProChildrenControl>().SetTextUniformly(city.name);
        CityDefends.GetComponent<TextMeshProChildrenControl>().SetTextUniformly(city.defend.ToString());
    }

    public void DisplayCityMessage(bool target)
    {
        int direction;
        if (target)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        TextMeshProChildrenControl namesTMPCC = CityNames.GetComponent<TextMeshProChildrenControl>();
        TextMeshProChildrenControl defendsTMPCC = CityDefends.GetComponent<TextMeshProChildrenControl>();
        //StopCoroutine(namesTMPCC.CityHoverAnimation(direction));
        //StopCoroutine(defendsTMPCC.CityHoverAnimation(direction));
        StopAllCoroutines();
        StartCoroutine(namesTMPCC.CityHoverAnimation(direction));
        StartCoroutine(defendsTMPCC.CityHoverAnimation(direction));
        active = target;
    }

    public void SetCityMessageZ(int level)
    {
        CityNames.localPosition = Vector3.forward * (-level * GameVar.BlockSize.z - 0.125f);
        CityDefends.localPosition = Vector3.forward * (-level * GameVar.BlockSize.z - 0.125f);
    }

    public void HealthJudge()
    {
        city.HealthJudge();
        if (city.isBroken)
        {
            SetColor(Color.black);
            RemovePieces();
        }
        else if (city.isComplete)
        {
            SetColor(Color.yellow);
        }
        else if (city.isCapital)
        {
            SetColor(Color.Lerp(Color.red, Color.white, 0.5f));
        }
    }

    public void SetColor(Color target)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform thisChild = transform.GetChild(i);
            if (thisChild.CompareTag("CityBound"))
            {
                thisChild.GetComponent<MeshRenderer>().material.color = target;
            }
        }
    }

    public void RemovePieces()
    {
        foreach (Position item in city.positionsInCity)
        {
            GameVar.pieceControl.RemovePieceInstante(item);
        }
    }
}
