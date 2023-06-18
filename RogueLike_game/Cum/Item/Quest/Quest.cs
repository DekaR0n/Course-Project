namespace Cum
{
    public class Quest : MapSpace
    {
        private bool AfterWin;

        public bool GetAfterWin
        {
            get { return AfterWin; }
        }
        public bool SetAfterWin
        {
            get { return AfterWin; }
            set { AfterWin = value; }
        }

        public Quest(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = false;
            UsedToDefense = false;
            PickUp = true;
            Equipped = false;
            AfterWin = false; // выдается после победы в мини игре, изначально false
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;
        }

        public override string ToString()
        {
            return "You collected a metal key.";
        }
        public override string InventoryToString()
        {
            return "Metal key. Maybe you can open something?";
        }
    }
}