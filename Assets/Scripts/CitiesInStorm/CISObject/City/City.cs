using System;
using System.Collections.Generic;
using System.Text;

namespace CitesInStorm
{
    public class City:CISObject
    {
        public int range;
        /// <summary>
        /// 防御值
        /// </summary>
        public int defend;

        public int originHealth;

        /// <summary>
        /// 生命值
        /// </summary>
        public int health;


        public string name;

        /// <summary>
        /// 从属的国家
        /// </summary>
        public Country belongsTo;

        /// <summary>
        /// 是否被破坏
        /// </summary>
        public bool isBroken;

        /// <summary>
        /// 是否完全防御
        /// </summary>
        public bool isComplete;

        /// <summary>
        /// 是否为首都
        /// </summary>
        public bool isCapital;

        /// <summary>
        /// 位置
        /// </summary>
        public new Position p;

        public Position[] positionsInCity;
        
        public City(Position p, int defend, string name, int range=2)
        {
            this.defend = defend;
            this.name = name;
            this.range = 2;
            this.health = range * range;
            this.originHealth = health;
            this.isBroken = false;
            this.isComplete = false;
            this.isCapital = false;
            this.p = p;
            positionsInCity = p.Expand(range, range, width: GameVar.map.Width, height: GameVar.map.Height);
        }

        public void CheckBroken()
        {
            if (!isComplete && health <= 0)
            {
                isBroken = true;
            }
        }

        public void CheckComplete()
        {
            if (!isBroken && health >= range * range * 2)
            {
                isComplete = true;
            }
        }

        public void Produce()
        {
            if(isCapital == false)
            {

            }
        }

        public void SetCapital()
        {   
            if(isBroken == false)
            {
                this.isCapital = true;
            }
        }

        public void HealthJudge()
        {
            CheckBroken();
            CheckComplete();
        }

        public PositionFloat GetCityMiddle()
        {
            float halfCityRange = (GameVar.cityRange - 1) / 2.0f;
            return new PositionFloat(p.x + halfCityRange, p.y + halfCityRange);
        }
    }
}
