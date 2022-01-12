using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
public class Seed
{
    public int seed; // 原数据

    public Seed(int seed)
    {
        this.seed = seed;
        Random.InitState(seed);
    }

    public int OutPutData()
    {
        return Random.Range(0, 4);
    }

}
