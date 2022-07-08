using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    /// 国家
    /// </summary>
    public class Country : Player
    {
        public static int machineSpeed = 4;

        public new Role role = Role.Human;

        public Country(string name) : base(name)
        {

        }
    }
}

