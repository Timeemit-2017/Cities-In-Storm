using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CitesInStorm;

/// <summary>
///
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
        Position[] ps = p.Expland(plusOnX, plusOnY, false, this.width, this.height);
        foreach(Position item in ps)
        {
            blocks_list.Add(GetBlock(item));
        }
        return blocks_list.ToArray();
    }
}
