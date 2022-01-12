using System;
using System.Collections.Generic;
using System.Text;

namespace CitesInStorm
{
    public class City:MapComponent
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
        private int health;
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        public string name;

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

        public void CalculateHealth(Blocks blocks)
        {
            int increment = 0;
            foreach(Block b in blocks.Near(p, range, range))
            {
                if(b.pieceOn != null)
                {
                    increment += b.pieceOn.piece.offset;
                }
            }
            health = originHealth + increment;
        }
    }
}
