namespace Cum
{
    public class Flashlight : MapSpace
    {
        protected int VisionSpace;

        public int GetVision
        {
            get { return VisionSpace; }
        }

        public Flashlight(char mapChar, int X, int Y) : base(mapChar, X, Y)
        {
            UsedToAttack = false;
            UsedToDefense = false;
            PickUp = true;
            Equipped = true;
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;
        }

        public override string ToString()
        {
            return "You collected a flashlight";
        }
        public override string InventoryToString()
        {
            return "Flashlight. Increases your visibility range by 5 squares.";
        }
    }

    public class CampingLamp : MapSpace
    {
        protected int VisionSpace;

        public int GetVision
        {
            get { return VisionSpace; }
        }

        public CampingLamp(char mapChar, int X, int Y) : base(mapChar, X, Y)
        {
            UsedToAttack = false;
            UsedToDefense = true;
            PickUp = true;
            Equipped = true;
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;
        }
        public override string ToString()
        {
            return "You collected a camping lamp.";
        }
        public override string InventoryToString()
        {
            return "Сamping lamp: Increases your visibility range by 3 squares.";
        }
    }

    public class Lighter : MapSpace
    {
        protected int VisionSpace;
        protected bool Based; // предмет базовый и есть с самого начала (без него видимость 3 клетки)

        public int GetVision
        {
            get { return VisionSpace; }
        }

        public Lighter(char mapChar, int X, int Y) : base(mapChar, X, Y)
        {
            UsedToAttack = false;
            UsedToDefense = false;
            PickUp = true;
            Equipped = true;
            Based = true;
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;
        }

        public override string ToString()
        {
            return "You collected a lighter";
        }
        public override string InventoryToString()
        {
            return "Lighter. Increases your visibility range by 2 squares.";
        }
    }
}