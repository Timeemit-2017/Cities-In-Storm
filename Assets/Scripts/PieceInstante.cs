using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
/// Piece在引擎中的具体实现
/// </summary>
public class PieceInstante : MonoBehaviour
{
    public MapSummon Summoner;

    public Piece piece;

    public Position Position
    {
        get
        {
            return piece.p;
        }
        set
        {
            piece.p = value;
        }
    }

    public void SetSize(Vector3 scale)
    {
        transform.localScale = scale;
    }
}
