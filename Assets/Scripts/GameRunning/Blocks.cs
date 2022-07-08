using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
/// 其实叫做BlocksControl更合适
/// </summary>
public class Blocks
{
    public Block[] blocks;
    public int width;
    public int height;

    public Blocks(int w, int h)
    {
        width = w;
        height = h;
        blocks = new Block[w * h];
    }

    public Block GetBlock(Position p)
    {
        return blocks[p.r * width + p.c];
    }

    public void SetBlock(Position p, Block target)
    {
        blocks[p.r * width + p.c] = target;
    }

    public Block[] Near(Position p)
    {
        List<Block> blocks_list = new List<Block>();
        Position[] ps = p.Near();
        foreach(Position item in ps)
        {
            blocks_list.Add(GetBlock(item));
        }
        return blocks_list.ToArray();
    }

    public Block[] Near(Position p, int plusOnX, int plusOnY)
    {
        List<Block> blocks_list = new List<Block>();
        Position[] ps = p.Expand(plusOnX, plusOnY, false, this.width, this.height);
        foreach(Position item in ps)
        {
            blocks_list.Add(GetBlock(item));
        }
        return blocks_list.ToArray();
    }

    /// <summary>
    /// 将所有Block的isChecked设定为false
    /// </summary>
    public void ClearAllBlockWhickIsChecked()
    {
        foreach (Block b in blocks)
        {
            if (b.isChecked)
            {
                b.SetColor();
                b.SetChecked(false);
            }
        }
    }

    /// <summary>
    /// 获取Block的实际坐标
    /// </summary>
    /// <param name="p">Block的索引坐标</param>
    /// <param name="direction">以方块中心点为原点，长或宽的一半为单位的平面方向组件。例：(1, 0) 表示向右，(-1, -1)表示左下。</param>
    /// <param name="isSurface">是否返回表面上的点（是否 -BlockSize.z）</param>
    public Vector3 GetWorldPosition(Position p, Vector2 direction = new Vector2(), bool isSurface = false)
    {
        Vector3 offset = new Vector3(direction.x * GameVar.BlockSize.x / 2, direction.y * GameVar.BlockSize.y / 2, -System.Convert.ToInt32(isSurface) * GameVar.BlockSize.z / 2);
        return GetBlock(p).transform.position + offset;
    }
}
