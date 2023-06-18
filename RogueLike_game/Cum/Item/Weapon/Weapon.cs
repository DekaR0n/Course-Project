namespace Cum
{ public class Weapon : MapSpace
    {
        protected int Damage;
        protected int StanChanse;

        public int GetDamage
        {
            get { return Damage; }
        }
        public int SetDamage
        {
            get { return Damage; }
            set { Damage = value; }
        }
    /*    public int GetStanChanse
        {
            get { return StanChanse; }
        }*/
        public int SetStanChanse
        {
            get { return ChanceOfStan; }
            set
            {
                this.ChanceOfStan = value;
            }
        }

        public Weapon()
        {
            
        }
        public Weapon(int stan_chanse, int atk, int count, int chance, int def, int max_on_map)
        {
            UsedToAttack = true;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;

            Damage = atk;
            CountOfUse = count;
            ChanceOfSpawn = chance;
            StanChanse = stan_chanse;
            Protect = def; 
            MaxOnMap = max_on_map;
        }

        public Weapon(char mapChar, char displaychar, int X, int Y) : base(mapChar, X, Y)
        {

        }

        public override string InventoryToString()
        {
            return base.InventoryToString();
        }
    }



    public class Guns : Weapon
    {
        protected int MaxAmmo; // Максимальное количество патрон в "обойме"
        protected int Ammo; // Патроны в "обойме"
        protected int Charge; // Требуемое количество ходов для подготовки и выстрела
        protected bool EmptyClip; // Проверка на наличие патронов в обойме 

        public int GetMaxAmmo
        {
            get { return MaxAmmo; }
        }
        public int GetAmmo
        {
            get { return Ammo; }
        }

        public int GetCharge
        {
            get { return Charge; }
        }

        public bool GetClip
        {
            get { return EmptyClip; }
        }
        public Guns(char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y) { }
    }


    public class Revolver : Guns
    {

        public Revolver(int ammo, char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = true;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            EmptyClip = true;
            
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;

            Ammo = ammo;

            SetStanChanse = 105;
            Charge = 2;
            Protect = 100;
            MaxAmmo = 6;
        }

        public override string ToString()
        {
            if (!EmptyClip)
            {
                return "You collected Revolver.";
            }
            else
            {
                return "You collected a empty Revolver.";
            }
        }

        public override string InventoryToString()
        {
            if (!EmptyClip)
            {
                return "Revolver: Damage " + Damage + "; Ammo " + Ammo + ".";
            }
            else
            {
                return "Revolver: Damage " + Damage + "; Ammo 0.";
            }
        }
    }

    public class Shotgun : Guns
    {
        public Shotgun(int ammo, char mapChar, char displaychar, int X, int Y) : base(mapChar, displaychar, X, Y)
        {
            UsedToAttack = true;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            EmptyClip = true;

            SetStanChanse = 105;
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;

            Ammo = ammo;

            Protect = 250;
            Charge = 4;
            Damage = 250;
            MaxAmmo = 2;
        }


        public override string ToString()
        {
            if (!EmptyClip)
            {
                return "You collected a Shotgun.";
            }
            else
            {
                return "You collected a empty Shotgun.";
            }
        }

        public override string InventoryToString()
        {
            if (!EmptyClip)
            {
                return "Shotgun: Damage " + Damage + "; Ammo " + Ammo + ".";
            }
            else
            {
                return "Shotgun: Damage " + Damage + "; Ammo 0.";
            }
        }
    }
}