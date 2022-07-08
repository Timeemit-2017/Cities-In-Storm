using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
/// 游戏信息控制
/// </summary>
public class GameControl : MonoBehaviour
{
    /// <summary>
    /// 开始一个新游戏
    /// 测试阶段
    /// </summary>
    /// <param name="weatherCount"></param>
    /// <param name="humanCount"></param>
    public void StartNewGame(int weatherCount, int humanCount)
    {
        GameVar.playerControl = new PlayerControl(weatherCount + humanCount);
        for(int i = 0; i < weatherCount; i++)
        {
            GameVar.playerControl.JoinPlayer(new Weather($"player{i}"));
        }
        for (int i = 0; i < humanCount; i++)
        {
            GameVar.playerControl.JoinPlayer(new Country($"player{i}"));
        }
    }

    public void NextRound()
    {
        GameVar.playerControl.NextRound();
        
    }
}
