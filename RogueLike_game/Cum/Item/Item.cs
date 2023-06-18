using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cum
{
    public class Item 
    {
        protected bool UsedToAttack; // Можно ли использовать предмет для нанесения урона
        protected bool UsedToDefense; // Можно ли использовать предмет для защиты
        protected bool PickUp; // Можно ли подобрать данный предмет
        protected bool Equipped; // Экиперован ли предмет в данный момен времени
        protected bool PassiveDamage; // Наносит пассивный урон
        
        protected int CountOfUse; // Кол-во исползований предмета (блок, удар, вскрытие замка/сейфа/двери)
        protected int ChanceOfSpawn; // Шанс спавна предмета
        protected int Protect; // Блокируемый предметом урон
        protected int MaxOnMap; // Максимальное кол-во предметов данного типа на карте

        protected int ChanceOfStan;

        protected bool IsDrop;
        protected int DropLocation;
        protected bool SpawnLocation;

        public int GetChanseStan
        {
            get { return ChanceOfStan; }
        }
        public int SetCountOfUse
        {
            get { return CountOfUse; }
            set { CountOfUse = value; }
        }

        public bool GetPassiveDamage
        {
            get { return PassiveDamage; }
        }

        public int SetDropLocation
        {
            get { return DropLocation;}
            set { DropLocation = value; }
        }
        public int GetDropLocation
        {
            get { return DropLocation;}
        }
        public bool SetPassiveDamage
        {
            get { return PassiveDamage; }
            set { PassiveDamage = value; }
        }
        public bool GetEquipped
        {
            get { return Equipped; }
        }
        public bool GetAttackable
        {
            get { return UsedToAttack; }
        }
        public bool GetDefenseable
        {
            get { return UsedToDefense; }
        }
        public bool GetPickUp
        {
            get { return PickUp; }
        }
        public int GetCountOfUse
        {
            get { return CountOfUse; }
        }
        public int GetChance
        {
            get { return ChanceOfSpawn; }
        }
        public int GetProtect
        {
            get { return Protect; }
        }

        public int GetMaxOnMap
        {
            get { return MaxOnMap; }
        }

      //  protected Item(char MapChar, int X, int Y) : this(MapChar,MapChar, X, Y) {}

        protected Item() {}

        public string CantAddItem()
        {
            return "Too much items in inventory";
        }

        public virtual string InventoryToString()
        {
            return null;
        }

        public virtual int GetMedicine()
        {
            return 0;
        }
        public string CantUseItem()
        {
            return "There is no such item in the inventory";
        }
        public virtual MapLevel AddDeletedItem(MapLevel levelMap, int x, int y, List<Item> items, int number)
        {
            return levelMap;
        }
        
    }
}
