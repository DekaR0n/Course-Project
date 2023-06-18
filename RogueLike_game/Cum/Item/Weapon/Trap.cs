namespace Cum
{
    public class Trap : MapSpace
    {

        public override string ToString()
        {
            return "You got into trap and lost 30 HP points";
        }
        public Trap(char mapChar,char displayChar, int X, int Y) : base(mapChar,X,Y)
        { 
            UsedToAttack = false;
            UsedToDefense = false;
            PickUp = false;
            Equipped = false;
            PassiveDamage = true;
        }
    }
}