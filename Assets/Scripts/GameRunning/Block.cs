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

    public PieceInstante PieceOn
    {
        get
        {
            return pieceControl.GetPiece(p);
        }
        set
        {
            pieceControl.SetPiece(p, value);
        }
    }

    public TerrainID id
    {
        get
        {
            return cisTerrain.id;
        }
    }

    public GameObject operation;

    public GameObject blockLight;

    /// <summary>
    /// 地图块指定的类型
    /// </summary>
    public CIS_Terrain cisTerrain;

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
                pieceControl.MovePiece(GameVar.blockBeingOperated.p, this.p);
                //SendMessage("SetChecked", false);
                GameVar.blocks.ClearAllBlockWhickIsChecked();
                operation.SetActive(false);
            }
        }
        else if (PieceOn == null && !operation.activeSelf)  // 生成Storm
        {
            if (GameVar.role == Role.Weather && cisTerrain.id == TerrainID.DeepOcean)
            {
                PieceOn = pieceControl.Spawn(p, PieceID.LightStorm);
            }
        }
    }

    public void GetPieceCanGoTo()
    {   
        Position[] result = PieceOn.piece.BlockCanMoveTo();
        foreach (Position item in result)
        {
            Block b = GameVar.blocks.GetBlock(item);
            if (b.cisTerrain.id != TerrainID.LandLocked)
            {
                b.SetChecked(true);
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
            //meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, Color.cyan, 0.3f);

            blockLight.SetActive(true);
        }
        else
        {
            //SetColor();
            blockLight.SetActive(false);
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
}
