using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    ///
    /// </summary>
    public class PlayerControl
    {
        public List<Player> players;

        public int thisPlayerIndex = 0;

        public Player ThisPlayer
        {
            get
            {
                return players[thisPlayerIndex];
            }
        }

        public PlayerControl(int length)
        {
            players = new List<Player>(length);
        }

        /// <summary>
        /// 创建玩家列表
        /// </summary>
        public void JoinPlayer(Player p)
        {
            players.Add(p);
        }

        /// <summary>
        /// 下家
        /// </summary>
        public void NextRound()
        {
            thisPlayerIndex++;
        }
    }
}

