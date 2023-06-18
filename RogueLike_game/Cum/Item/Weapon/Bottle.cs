namespace Cum
{
    public class Bottle : Weapon
    {
        public Bottle(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = true;
            UsedToDefense = false;
            PickUp = true;
            Equipped = true;
            SpawnLocation = false;
            DropLocation = 0;
            IsDrop = false;

            SetStanChanse = 90;
            Protect = 15;
            Damage = 20;
            CountOfUse = 2;
            StanChanse = 30;
        }

        public override string ToString()
        {
            return "You collected a glass bottle.";
        }
        public override string InventoryToString()
        {
            return "Glass bottle: Damage " + Damage + "; Stability " + CountOfUse + "; Chanse of stan " + StanChanse + ".";
        }
    }
}