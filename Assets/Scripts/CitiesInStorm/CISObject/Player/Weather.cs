using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CitesInStorm
{
    /// <summary>
    /// 气候
    /// </summary>
    public class Weather : Player
    {
        public int stormSpeed = 5;
        public int heavyStormSpeed = 3;

        public new Role role = Role.Weather;

        public Weather(string name) : base(name)
        {

        }

    }
}

