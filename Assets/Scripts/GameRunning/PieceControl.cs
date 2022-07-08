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

    public void Awake()
    {
        GameVar.pieceControl = this;
    }

    public PieceInstante Spawn(Position p, PieceID id)
    {   
        Piece temp = PieceDict.GetPieceWithID(id, p, summoner.Map);
        GameObject gameObject = Instantiate(piecePrefab, transform);
        PieceInstante instante = gameObject.GetComponent<PieceInstante>();
        instante.piece = temp;
        instante.Summoner = summoner;
        instante.name = "PieceInstante" + temp.p.ToString();
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        SetScale(instante);

        SetPosition(instante);

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

    /// <summary>
    /// 获取棋子
    /// </summary>
    /// <param name="p">棋子所在的坐标</param>
    /// <returns></returns>
    public PieceInstante GetPiece(Position p)
    {
        return pieces[p.r, p.c];
    }

    /// <summary>
    /// 设置棋子
    /// 注意：这个操作会覆盖原有位置上的棋子
    /// </summary>
    /// <param name="p">坐标</param>
    /// <param name="resource">棋子的引用</param>
    public void SetPiece(Position p, PieceInstante resource)
    {
        pieces[p.r, p.c] = resource;
        resource.P = p;
    }
    
    /// <summary>
    /// 删除棋子数据
    /// </summary>
    /// <param name="p">要删除的坐标</param>
    public void RemovePiece(Position p)
    {
        pieces[p.r, p.c] = null;
    }

    /// <summary>
    /// 销毁棋子的实例
    /// </summary>
    /// <param name="p"></param>
    public void RemovePieceInstante(Position p)
    {
        pieces[p.r, p.c].Delete();
        pieces[p.r, p.c] = null;
    }

    /// <summary>
    /// 尝试设置棋子，如果当前位置已有，则返回false
    /// </summary>
    /// <param name="p">坐标</param>
    /// <param name="resource">棋子的引用</param>
    /// <returns>是否设置成功</returns>
    public bool TrySetPiece(Position p, PieceInstante resource)
    {
        if(pieces[p.r, p.c] == null)
        {
            return false;
        }
        else
        {
            SetPiece(p, resource);
            return true;
        }
    }

    /// <summary>
    /// 移动棋子（强制）
    /// </summary>
    /// <param name="pFrom">棋子原先所在的坐标</param>
    /// <param name="pTo">棋子移动到的坐标</param>
    public void MovePiece(Position pFrom, Position pTo)
    {
        PieceInstante thisPiece = GetPiece(pFrom);

        if(GetPiece(pTo) == null)  // 如果目标格没有棋子
        {
            SetPiece(pTo, thisPiece);

            SetPosition(thisPiece, pTo);

            RemovePiece(pFrom);

            CityHealthControl(pFrom, pTo, thisPiece);
        }
    }

    /// <summary>
    /// 尝试移动棋子
    /// </summary>
    /// <param name="pFrom">棋子原位置</param>
    /// <param name="pTo">将要移动到的位置</param>
    /// <returns></returns>
    public bool TryMovePiece(Position pFrom, Position pTo)
    {
        if (GameVar.blocks.GetBlock(pTo).isChecked)
        {
            if(pFrom == pTo)
            {
                // 向自己移动时
                SetPosition(GetPiece(pFrom), pFrom);
                return false;
            }
            MovePiece(pFrom, pTo);
            return true;
        }
        else
        {
            // 将棋子退回原有的位置
            SetPosition(GetPiece(pFrom), pFrom);
            return false;
        }
    }

    /// <summary>
    /// 控制移动后城市血量的变化
    /// </summary>
    public void CityHealthControl(Position pFrom, Position pTo, PieceInstante thisPiece)
    {
        if (!GameVar.cityControl.CheckIfInCity(pFrom) && GameVar.cityControl.CheckIfInCity(pTo, out CityInstante cityInstante))
        {
            cityInstante.Health += thisPiece.piece.offset;
        }
        if (GameVar.cityControl.CheckIfInCity(pFrom, out CityInstante cityInstante1) && !GameVar.cityControl.CheckIfInCity(pTo))
        {
            cityInstante1.Health -= thisPiece.piece.offset;
        }
    }

    /// <summary>
    /// 设置棋子在实际棋盘上的位置
    /// </summary>
    public void SetPosition(PieceInstante pieceInstante)
    {
        SetPosition(pieceInstante, pieceInstante.P);
    }

    /// <summary>
    /// 设置棋子在实际棋盘上的位置
    /// </summary>
    /// <param name="p">棋子所在Block的坐标</param>
    public void SetPosition(PieceInstante pieceInstante, Position p)
    {
        Transform blockT = GameVar.blocks.GetBlock(p).transform;
        float x = blockT.position.x;
        float y = blockT.position.y;
        float z = blockT.position.z - blockT.localScale.z / 2 - 0.01f;
        pieceInstante.transform.position = new Vector3(x, y, z);
    }

    /// <summary>
    /// 设置棋子的大小
    /// 注意：不要重复调用该方法，否则会使棋子逐渐缩小。
    /// </summary>
    /// <param name="pieceInstante"></param>
    public void SetScale(PieceInstante pieceInstante)
    {
        Transform blockT = pieceInstante.GetBlock();
        // 0.8 / 1 = 棋子scale / blockT.scale   （0.8为标准棋子大小, 1为标准棋格大小）（单位为Tile）
        pieceInstante.transform.localScale = blockT.localScale.x * pieceInstante.transform.localScale;
    }

}
