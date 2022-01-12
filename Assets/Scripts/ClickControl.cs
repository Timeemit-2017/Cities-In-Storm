using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
/// 管理游戏中各个按钮的点击事件
/// </summary>
public class ClickControl : MonoBehaviour
{
    public void ReadyToMovePiece()
    {
        if (GameVar.blockBeingOperated && !GameVar.blockBeingOperated.isChecked)
        {
            GameVar.blockBeingOperated.GetPieceCanGoTo();
        }
        else
        {
            Debug.Log(GameVar.blockBeingOperated.name);
        }
    }
}
