namespace Cum
{
    public class Ammo : MapSpace
    {
        private bool Caliber; // Тип патронов
        private bool NeedGun; // Имеет ли игрок какой-либо огнестрел

        public bool GetCaliber
        {
            get { return Caliber; }
        }
        
        public bool GetNeedGun
        {
            get { return NeedGun; }
        }

        public Ammo(int count, char mapChar, int x, int y) : base(mapChar, x, y)
        {
            PickUp = true;
            NeedGun = false;
            
            CountOfUse = count;

        }
        public override string ToString()
        {
            return "You collected an ammo.";
        }
        public override string InventoryToString()
        {
            return "Ammo: Count of use " + CountOfUse;
        }
    }
}