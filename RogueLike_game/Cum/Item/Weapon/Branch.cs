namespace Cum
{
    public class Branch : Weapon
    {
        public Branch(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = true;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            SpawnLocation = false;
            DropLocation = 0;
            IsDrop = false;

            SetStanChanse = 90;
            Damage = 10;
            Protect = 5;
            CountOfUse = 2;
            StanChanse = 25;
        }

        public override string ToString()
        {
            return "You collected a wooden branch.";
        }
        public override string InventoryToString()
        {
            return "Wooden branch: Damage " + Damage + "; Block strength " + Protect + "; Stability " + CountOfUse + "; Chanse of stan " + StanChanse + ".";
        }
    }
}