namespace Cum
{
    public class Crowbar : Tool
    {
        public Crowbar(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = false;
            UsedToDefense = false;
            BreakDoor = true;
            PickUp = true;
            
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;
            Protect = 30;
        }

        public override string ToString()
        {
            return "You collected a Crowbar";
        }
        public override string InventoryToString()
        {
            return "Crowbar - Can break locked doors.";
        }
    }
}