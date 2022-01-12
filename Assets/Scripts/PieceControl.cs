using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
/// 棋子控制类
/// </summary>
public class PieceControl : MonoBehaviour
{
    public PieceInstante[,] pieces;
    public MapSummon summoner;
    public GameObject piecePrefab;

    public PieceInstante Spawn(Position p, PieceID id)
    {   
        Piece temp = PieceDict.GetPieceWithID(id, p, summoner.map);
        PieceInstante instante = Instantiate(piecePrefab, transform).GetComponent<PieceInstante>();
        instante.piece = temp;

        SetPosition(instante, p);

        SetPiece(p, instante);

        return instante;
    }

    public void Instante(int w, int h)
    {
        pieces = new PieceInstante[h, w];
    }

    public void ReadyToMovePiece()
    {
        if (GameVar.blockBeingOperated)
        {
            GameVar.blockBeingOperated.GetPieceCanGoTo();
        }
    }

    public PieceInstante GetPiece(Position p)
    {
        return pieces[p.r, p.c];
    }

    public void SetPiece(Position p, PieceInstante resource)
    {
        pieces[p.r, p.c] = resource;
    }

    public void MovePiece(Position pFrom, Position pTo)
    {
        PieceInstante thisPiece = GetPiece(pFrom);

        if(GetPiece(pTo) == null)
        {
            SetPiece(pTo, thisPiece);

            SetPosition(thisPiece, pTo);

            thisPiece.Position = pTo;
            thisPiece.piece.p = pTo;
        }
    }

    /// <summary>
    /// 设置棋子在实际棋盘上的位置
    /// </summary>
    /// <param name="p">棋子所在Block的坐标</param>
    public void SetPosition(PieceInstante pieceInstante, Position p)
    {
        Transform blockT = GameVar.blocks.GetBlock(p).transform;
        pieceInstante.transform.localScale = blockT.localScale * 0.4f;
        float x = blockT.position.x;
        float y = blockT.position.y;
        float z = blockT.position.z - blockT.localScale.z / 2 - 0.01f;
        pieceInstante.transform.position = new Vector3(x, y, z);
    }
}
