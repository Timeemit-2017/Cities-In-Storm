using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
///
/// </summary>
public class Block : MonoBehaviour
{
    public PieceControl pieceControl;

    public Position p;

    public PieceInstante pieceOn;  // 附属的棋子

    public GameObject operation;

    /// <summary>
    /// 地图块指定的类型
    /// </summary>
    private CIS_Terrain cisTerrain;

    private MeshRenderer meshRenderer;

    public MapSummon summoner;

    /// <summary>
    /// 方块是否已经初始化
    /// </summary>
    private bool isAlreadyInit = false;

    /// <summary>
    /// 是否被标记为查找
    /// </summary>
    public bool isChecked = false;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseEnter()
    {
        if (isChecked) return;
        meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, Color.black, 0.7f);
    }

    private void OnMouseUp()
    {
        if (isChecked)  // 用于移动
        {
            if(p.Equals(GameVar.blockBeingOperated.p))
            {
                Debug.Log("不能向自己移动棋子！");
            }
            else
            {
                GetPiece(GameVar.blockBeingOperated);
                pieceControl.MovePiece(GameVar.blockBeingOperated.p, this.p);
                //SendMessage("SetChecked", false);
                foreach (Block b in GameVar.blocks.blocks)
                {
                    if (b.isChecked)
                    {
                        b.SetColor();
                        b.isChecked = false;
                    }
                }
                operation.SetActive(false);
            }
        }
        else if (pieceOn != null && !GameVar.CheckVectorIn(operation.transform.position, (Vector2)operation.transform.position + operation.GetComponent<RectTransform>().sizeDelta, Input.mousePosition))  // 编辑Strom（打开或关闭选项卡Operation）
        {
            bool target = !operation.activeSelf;
            operation.SetActive(target);
            operation.GetComponent<OperationControl>().positionControl(Input.mousePosition);
            if (target)  // 公开此Block（即正在被操作的Block）
            {
                GameVar.blockBeingOperated = this;
            }
            else
            {
                GameVar.blockBeingOperated = null;
            }
        }
        else if (pieceOn == null && !operation.activeSelf)  // 生成Storm
        {
            if (GameVar.role == Role.Weather && cisTerrain.id == TerrainID.DeepOcean)
            {
                pieceOn = pieceControl.Spawn(p, PieceID.LightStorm);
            }
        }
    }

    public void GetPieceCanGoTo()
    {   
        List<Position> result = pieceOn.piece.BlockCanMoveTo();
        foreach (Position item in result)
        {   
            if(GameVar.blocks.GetBlock(item).cisTerrain.id != TerrainID.LandLocked)
            {
                summoner.PointOutBlock(item, Color.red);
                GameVar.blocks.GetBlock(item).SetChecked(true);
                //GameVar.pieceTemp = pieceOn;
            }
        }
    }


    private void OnMouseExit()
    {
        if (isChecked) return;
        try
        {
            SetColor();
        }
        catch
        {
            meshRenderer.material.color = Color.white;
        }
    }

    public void SetChecked(bool target)
    {
        isChecked = target;
        if (target)
        {
            meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, Color.cyan, 0.3f);
        }
        else
        {
            SetColor();
        }
    }

    /// <summary>
    /// 设置地图块的类型（初始化）
    /// </summary>
    /// <param name="id">指定的类型id</param>
    public void SetTerrainWithID(TerrainID id)
    {
        cisTerrain = MapDict.GetTerrainWithId(id);
        SetColor(id);
        isAlreadyInit = true;
    }

    /// <summary>
    /// 忽略地图块本身的类型，设置为指定类型的样式
    /// </summary>
    /// <param name="id"></param>
    public void SetColor(TerrainID id)
    {
        switch (id)
        {
            case TerrainID.Ocean:
                meshRenderer.material.color = new Color(0, 0.3f, 0.7f);
                break;
            case TerrainID.Land:
                meshRenderer.material.color = Color.green;
                if(!isAlreadyInit) MoveForward(-(int)id);
                break;
            case TerrainID.LandLocked:
                meshRenderer.material.color = Color.gray;
                if (!isAlreadyInit) MoveForward(-(int)id);
                break;
            case TerrainID.DeepOcean:
                meshRenderer.material.color = Color.blue;
                //MoveForward(1);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 根据地图块类型设置样式
    /// </summary>
    public void SetColor()
    {
        SetColor(cisTerrain.id);
    }

    /// <summary>
    /// 向摄像头移动step个单位
    /// </summary>
    /// <param name="step">移动的距离（以方块的深度为一单位）</param>
    public void MoveForward(int step)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + step * this.transform.localScale.z);
    }

    public void GetPiece(Position fromP)
    {
        Block from = GameVar.blocks.GetBlock(fromP);  // 得到目标Block
        // 转移棋子
        pieceOn = from.SendPiece();
        from.pieceOn = null;
    }
    public void GetPiece(Block from)
    {
        // 转移棋子
        pieceOn = from.SendPiece();
        from.pieceOn = null;
    }


    public PieceInstante SendPiece()
    {
        return pieceOn;
    }

}
