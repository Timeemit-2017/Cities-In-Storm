using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    ///
    /// </summary>
    public class Player
    {
        public string name;

        /// <summary>
        /// 所隶属的阵营
        /// </summary>
        public Role role;

        public Player(string name, Role role)
        {
            this.name = name;
            this.role = role;
        }

        public Player(string name)
        {
            this.name = name;
        }
        
    }

}

