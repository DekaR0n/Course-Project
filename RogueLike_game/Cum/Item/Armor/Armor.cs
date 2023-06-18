namespace Cum
{
    public class Armor : MapSpace
    {
        protected bool Head;
        protected bool Boody;

        public bool GetHead
        {
            get { return Head; }
        }
        public bool GetBoody
        {
            get { return Boody; }
        }

        public Armor()
        {

        }
        public Armor(int def, int count, int max_spawn, int chance)
        {
            UsedToAttack = false;
            UsedToDefense = true;
            PickUp = true;
            Equipped = false;
            SpawnLocation = false;
            DropLocation = 0;
            IsDrop = false;
            
            CountOfUse = count;
            MaxOnMap = max_spawn;
            ChanceOfSpawn = chance;
            Protect = def;
        }

        public Armor(char mapChar, char displaychar, int X, int Y) : base(mapChar, X, Y)
        {

        }
    }

    public class MotorcycleHelmet : Armor
    {
        public MotorcycleHelmet(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = false;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            SpawnLocation = false;
            DropLocation = 0;
            IsDrop = false;

            Head = true;
            Boody = false;

            Protect = 20;
            CountOfUse = 10;
        }

        public override string ToString()
        {
            return "You collected a motorcycle helmet.";
        }
        public override string InventoryToString()
        {
            return "Motorcycle helmet: Defence " + Protect + "; Stability " + CountOfUse + ".";
        }
    }

    public class Jacket : Armor
    {
        public Jacket(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = false;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            SpawnLocation = false;
            DropLocation = 0;
            IsDrop = false;

            Head = false;
            Boody = true;

            Protect = 10;
            CountOfUse = 10;
        }
        public override string ToString()
        {
            return "You collected a jacket.";
        }
        public override string InventoryToString()
        {
            return "Jacket: Defence " + Protect + "; Stability " + CountOfUse + ".";
        }
    }

    public class LifeVest : Armor
    {
        public LifeVest(char mapChar, char displaychar, int X, int Y) : base(mapChar,displaychar, X, Y)
        {
            UsedToAttack = false;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            SpawnLocation = false;
            DropLocation = 0;
            IsDrop = false;

            Head = false;
            Boody = true;

            Protect = 20;
            CountOfUse = 10;
        }
        public override string ToString()
        {
            return "You collected a life vest.";
        }
        public override string InventoryToString()
        {
            return "Life vest: Defence " + Protect + "; Stability " + CountOfUse + ".";
        }
    }

    public class Helmet : Armor
    {
        public Helmet(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = false;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            SpawnLocation = false;
            DropLocation = 0;
            IsDrop = false;

            Head = true;
            Boody = false;

            Protect = 10;
            CountOfUse = 10;

        }
        public override string ToString()
        {
            return "You collected a helmet.";
        }
        public override string InventoryToString()
        {
            return "Helmet: Defence " + Protect + "; Stability " + CountOfUse + ".";
        }
    }
}