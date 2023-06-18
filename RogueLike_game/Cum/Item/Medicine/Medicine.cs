using System.Collections.Generic;

namespace Cum
{
    public class Medicine : MapSpace
    {
        private int MedCit; // Восстанавливаемое здоровье
        private int Rare;
        public override int GetMedicine()
        {
             return MedCit;
        }


        public Medicine(int heal, char mapChar, int X, int Y) : base(mapChar,X,Y)
        {
            UsedToAttack = false;
            UsedToDefense = false;
            PickUp = true;
            Equipped = false;
            SpawnLocation = true;
            DropLocation = 0;
            IsDrop = false;
            
            MedCit = heal;
        }

        public override string ToString()
        {
            return "You collected a medicine pack with "+MedCit+" health points";
        }
        public override string InventoryToString()
        {
            return "Medicine pack. Healing you on " + MedCit + " health points.";
        }

        public override MapLevel AddDeletedItem(MapLevel levelMap, int x, int y,List<Item> items, int number)
        {
          //  base.AddDeletedItem();
          levelMap.LevelMap[x, y] = new Medicine(this.MedCit, Constants.MEDICINE, x, y);
          return levelMap;
        }
    }
}