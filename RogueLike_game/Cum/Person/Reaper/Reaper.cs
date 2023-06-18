using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cum
{
    public class Reaper : MapSpace
    {
        public readonly int MAX_HEALTH = 800;
        public readonly int MAX_DAMAGE = 30;
        public readonly int MAX_SPEED = 1;
        public readonly int MIN_SPEED = 1;
        public readonly int SPACE_VISION_X = 7;
        public readonly int SPACE_VISION_Y = 4;
        public readonly int BLIND_CONST = 5;
        private int HP;
        public bool Blind;

        public int getMaxBlind
        {
            get { return BLIND_CONST; }
        }
        public int getHealth
        {
            get { return HP; }
        }
        public int setHealth
        {
            get { return HP; }
            set { HP = value; }
        }
        private int X;
        private int Y;

        private bool IS_BLIND { get; set; }
        public int GetX
        {
            get { return X; }
        }
        public int GetY
        {
            get { return Y; }
        }

        public int SetX
        {
            get { return X; }
            set { X = value; }
        }
        public int SetY
        {
            get { return Y; }
            set { Y = value; }
        }


        public Reaper()
        {
            Blind = false;
            HP = MAX_HEALTH;
            DropLocation = 0;
        }
    }
}
