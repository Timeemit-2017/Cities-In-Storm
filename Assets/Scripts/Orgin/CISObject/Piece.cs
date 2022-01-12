using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CitesInStorm
{   
    /// <summary>
    /// Piece挂件
    /// 用于存储具体信息
    /// </summary>
    public class Piece:PieceOrigin
    {
        public int offset;  // 对于血量的操作

        public Piece(Position p, int speed, Map map) : base(p, speed, map)
        {
            offset = 0;
        }
    }
}
