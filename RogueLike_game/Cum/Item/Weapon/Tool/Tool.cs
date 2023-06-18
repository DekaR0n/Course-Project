namespace Cum
{
    public class Tool : Weapon
    {
        protected bool BreakDoor;

        public bool GetBreakDoor
        {
            get { return BreakDoor; } 
        }

        public Tool()
        {
            PickUp = true;
            Equipped = true;
            
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;
        }

        public Tool(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y) {}
    }
}