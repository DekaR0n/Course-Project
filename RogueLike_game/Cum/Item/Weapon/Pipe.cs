namespace Cum
{
    public class Pipe : Weapon
    {
        public Pipe(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = true;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;

            Damage = 15;
            Protect = 15;
            CountOfUse = 7;
            MaxOnMap = 10;
            StanChanse = 20;
        }

        public override string ToString()
        {
            return "You collected a metal pipe.";
        }

        public override string InventoryToString()
        {
            return "Metal pipe: Damage " + Damage + "; Stability " + CountOfUse + "; Chanse of stan " + StanChanse + ".";
        }
    }
}