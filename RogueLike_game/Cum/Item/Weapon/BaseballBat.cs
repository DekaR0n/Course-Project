namespace Cum
{
    public class BaseballBat : Weapon
    {
        public BaseballBat(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = true;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;

            SetStanChanse = 80;
            Damage = 20;
            Protect = 20;
            CountOfUse = 12;
            StanChanse = 40;
        }

        public override string ToString()
        {
            return "You collected a baseball bat: Damage.";
        }
        public override string InventoryToString()
        {
            return "Baseball bat: Damage " + Damage + "; Block strength " + Protect + "; Stability " + CountOfUse + "; Chanse of stan "+ StanChanse + ".";
        }
    }
}