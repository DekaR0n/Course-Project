namespace Cum
{
    public class Shaurma : MapSpace
    {
        private int KaraNebec; // время активации главного эффекта шавухи
        private int DamageBuf;
        private int DefenceBuf;
        
        public int GetKara
        {
            get { return KaraNebec; }
        }
        public int GetDamageBuf
        {
            get { return DamageBuf; }
        }
        public int GetDefenceBuf
        {
            get { return DefenceBuf; }
        }


        public Shaurma(char mapChar, char displaychar, int X, int Y) : base(mapChar, X, Y)
        {
            PickUp = true;
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;

            KaraNebec = 30;
            CountOfUse = 1;
            DefenceBuf = 20;
            DamageBuf = 30;
            MaxOnMap = 1;
        }
        public override string ToString()
        {
            return "You collected a Shaurma. So tasty. Maybe eat it?))";
        }
        public override string InventoryToString()
        {
            return "Shaurma. An appetizing-looking wrap made from pita bread, meat, lettuce and something moving";
        }
    }
 /*   public class Shaurma : Item
    {
        private int KaraNebec; // время активации главного эффекта шавухи
        private int DamageBuf;
        private int DefenceBuf;
        
        public int GetKara
        {
            get { return KaraNebec; }
        }
        public int GetDamageBuf
        {
            get { return DamageBuf; }
        }
        public int GetDefenceBuf
        {
            get { return DefenceBuf; }
        }

        public Shaurma(int count, int chance, int defbuf, int atkbuf, int max_on_map)
        {
            PickUp = true;
            
            CountOfUse = count;
            ChanceOfSpawn = chance;
            DefenceBuf = defbuf;
            DamageBuf = atkbuf;
            MaxOnMap = max_on_map;
        }

    }*/
}