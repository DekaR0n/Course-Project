namespace Cum
{
    public class FireAxe : Tool
    {
        public FireAxe(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = true;
            UsedToDefense = true;
            BreakDoor = true;
            PickUp = true;
            
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;

            SetStanChanse = 80;
            Damage = 30;
            Protect = 20;
            CountOfUse = 15;
            StanChanse = 60;
        }

        public override string ToString()
        {
            return "You collected a fire axe.";
        }
        public override string InventoryToString()
        {
            return "Fire axe: Damage " + Damage + "; Block strength " + Protect + "; Stability " + CountOfUse + "; Chanse of stan " + StanChanse + ". Can break locked doors.";
        }
    }
}