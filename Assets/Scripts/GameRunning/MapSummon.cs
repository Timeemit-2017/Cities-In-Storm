using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
/// 地图生成的具体实现
/// </summary>
public class MapSummon : MonoBehaviour
{   
    [Space]
    public GameObject BlockPrefab;
    public Camera c;
    public PieceControl pieceControl;
    public GameObject operation;

    [Space]
    public int UserWidth = 30;
    public int UserHeight = 20;
    public int Width;
    public int Height;

    [HideInInspector]
    public float blockW;

    [HideInInspector]
    public float blockH;

    [HideInInspector]
    public float blockD;

    [Space]
    public int seed = 365;

    [Space]
    public int landNum = 1;
    public int health = 20;

    [Space]
    [Range(0.03f, 1f)]
    public float scale = 5;

    [Space]
    public string mapName = "default";

    private bool isAlreadySummon = false;
    public Map Map
    {
        get
        {
            return GameVar.map;
        }
        set
        {
            GameVar.map = value;
        }
    }
    //public Blocks blocks = GameVar.blocks;

    public void Awake()
    {
        GameVar.mapSummon = this;
    }

    public void PerlinSummon()
    {
        StartSummon(UserWidth, UserHeight);
        Map.PerlinNoise(seed, scale);
        PointOutBlock();
    }

    /// <summary>
    /// 根据地图长宽初始化地图
    /// </summary>
    /// <param name="widthCount"></param>
    /// <param name="heightCount"></param>
    public void StartSummon(int widthCount, int heightCount)
    {
        CheckIsAlreadySummon();

        Width = widthCount;
        Height = heightCount;
        GameVar.blocks = new Blocks(widthCount, heightCount);  // 重置Blocks
        pieceControl.Instante(widthCount, heightCount);  // 重置PieceControl
        Map = new Map(Width, Height);  // 生成新的数值地图

        float videoH_2 = c.orthographicSize;
        float videoW_2 = c.aspect * videoH_2;
        //blockH = videoH_2 * 2 / heightCount;
        //blockW = blockH;
        blockH = 0.5f;
        blockW = 0.5f;
        blockD = (blockW + blockH) / 2;
        BlockPrefab.transform.localScale = new Vector3(blockW, blockH, blockD);
        Vector3 leftUp = new Vector3(c.transform.position.x - videoW_2 + blockW / 2, c.transform.position.y - videoH_2 + blockH / 2);
        for (int y = 0; y < heightCount; y++)
        {
            for (int x = 0; x < widthCount; x++)
            {   
                // 实例化Block
                GameObject o = Instantiate(BlockPrefab, new Vector3(leftUp.x + x * blockW, leftUp.y + y * blockH), BlockPrefab.transform.rotation);
                if (!o.activeSelf)
                {
                    o.SetActive(true);
                }
                
                o.transform.parent = this.transform;
                o.name = x.ToString() + ", " +  y.ToString();
                Block o_b = o.GetComponent<Block>();
                o_b.pieceControl = pieceControl;
                o_b.p = new Position(x, y);
                o_b.operation = operation;
                o_b.summoner = this;
                //o.layer = 9;
                GameVar.blocks.SetBlock(o_b.p, o_b);
            }
        }
        // 偏移，使地图在屏幕中央
        Vector3 tp = transform.position;
        transform.position = new Vector3(tp.x + (videoW_2 * 2 - widthCount * blockW) / 2, tp.y, tp.z);
        isAlreadySummon = true;
    }
    
    /// <summary>
    /// 检测地图是否已经初始化了，如果是的，则删除已经初始化的结果。
    /// </summary>
    private void CheckIsAlreadySummon()
    {
        if (isAlreadySummon)
        {
            for(int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            GameVar.blocks = null;
            isAlreadySummon = false;
        }
    }

    /// <summary>
    /// 根据所给的地图信息为所有地图块上色
    /// </summary>
    public void PointOutBlock()
    {
        for(int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                GameVar.blocks.GetBlock(new Position(x, y)).SetTerrainWithID(Map.map[y, x]);
            }
        }
    }

    /// <summary>
    /// 为某一个地图块上色
    /// </summary>
    /// <param name="id"></param>
    public void PointOutBlock(Position p, Color c)
    {
        GameVar.blocks.GetBlock(p).GetComponent<MeshRenderer>().material.color = c;
    }

    /// <summary>
    /// 完全随机生成地图块地形
    /// </summary>
    public void Summon(int seed)
    {
        Random.InitState(seed);
        foreach(Block item in GameVar.blocks.blocks)
        {
            item.SetTerrainWithID((TerrainID)Random.Range(0, 4));
        }
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    /// <summary>
    /// 随机一个种子
    /// </summary>
    private void Summon()
    {
        seed = Random.Range(-900, 900);
    }

}
