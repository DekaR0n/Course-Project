using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cum
{
    internal class Player : MapSpace
    {
        private const int MAX_HP = 100;
        private const int STARTING_STRENGTH = 10;
        private int startingArmor;
        private int visibleX;
        private int visibleY;
        public List<Item> items;
        public List<Item> equipment;

        public Item head;
        public Item body;
        public Item hand;
        
        
        private int _x { get; set; }
        private int _y { get; set; }

        public string PlayerName { get; set; }
        public int HP { get; set; }
        public int HPDamage { get; set; }
        public int Stamina { get; set; }
        public bool StaminaMod { get; set; }
        public int Strength { get; set; }
        public int getVisibleX
        {
            get { return visibleX; }
        }
        public int getVisibleY
        {
            get { return visibleY; }
        }

        public int getStartingStrength
        {
            get { return STARTING_STRENGTH; }
        }
        public int setVisibleX
        {
            get { return visibleX; }
            set { visibleX = value; }
        }
        public int setVisibleY
        {
            get { return visibleY; }
            set { visibleY = value; }
        }
        public int setArmor
        {
            get { return startingArmor; }
            set { startingArmor = value; }
        }
        public int getArmor
        {
            get { return startingArmor; }
        }
        
        public Player(string PlayerName)
        {
            this.PlayerName = PlayerName;
            startingArmor = 10;
            visibleX = 5;
            visibleY = 3;
            this.HP = MAX_HP;
            this.HPDamage = 0;
            this.Stamina = STARTING_STRENGTH;
            this.StaminaMod = false;
            this.Strength = 10;
            items = new List<Item>();
            equipment = new List<Item>(4);
            head = null;
            body = null;
            hand = null;
        }

        public void Dead()
        {
           System.Environment.Exit(0); 
        }

        public void SetArmorStrength()
        {
            this.Strength = STARTING_STRENGTH + (hand == null ? 0 : hand.GetProtect);
        }
        
        public void RemoveHeadEquip()
        {
            items.Remove(head);
            head = null;
        }
        public void RemoveHandEquip()
        {
            items.Remove(hand);
            hand = null;
        }
        public void RemoveBodyEquip()
        {
            items.Remove(body);
            body = null;
        }
        public void RemoveKey()
        {
            try
            {
                foreach (var item in items)
                {
                    if (item is Quest)
                    {
                        items.Remove(item);
                    }
                }
            }
            catch
            {
                return;
            }
        }

        public bool AddItem(Item item)
        {
            if (items.Count < 9)
            {
                items.Add(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InventoryToString()
        {
            Console.WriteLine("--||--");
            for (int i = 0; i < items.Count; i++)
            {
                if (head!=null && items[i].GetType() == head.GetType() || body!=null && items[i].GetType() == body.GetType() ||
                    hand!=null && items[i].GetType() == hand.GetType())
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                Console.WriteLine("║  "+(i+1)+".  "+items[i].InventoryToString()+"\n");
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("--||--");
        }

        public bool UseMedicine()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].GetType() == typeof(Medicine))
                {
                    if (HP + items[i].GetMedicine() > MAX_HP)
                        HP = MAX_HP;
                    else
                    {
                        this.HP += items[i].GetMedicine();
                    }
                    items.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public bool UseShaurma()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].GetType() == typeof(Shaurma))
                {
                    items.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool IsAmmo()
        {
            foreach (var item in items)
            {
                if (item is Ammo)
                {
                    items.Remove(item);
                    return true;
                }
            }
            return false;
        }

        public bool IsBottle()
        {
            foreach (var item in items)
            {
                if (item is Bottle)
                {
                    items.Remove(item);
                    return true;
                }
            }
            return false;
        }
        public bool IsBranch()
        {
            foreach (var item in items)
            {
                if (item is Branch)
                {
                    items.Remove(item);
                    return true;
                }
            }
            return false;
        }
        public void ShowHP()
        { 
            Console.WriteLine();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < this.HP / 5; i++)
            {
                sb.Append("▒");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            sb.Append(HP + "/100  ");
            Console.Write(sb.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void ShowStamina()
        {
            StringBuilder sb = new StringBuilder();
            SetArmorStrength();
            for (int i = 0; i < this.Strength / 6; i++)
            {
                sb.Append("▒");
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            sb.Append("Strength: "+Strength + "/250  ");
            sb.Append("Armor: "+getArmor);
            //sb.Append(X + " " + Y);
            //sb.Append("  "+CurrentPlayer.X + " " + CurrentPlayer.Y);
            Console.Write("  " + sb.ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void CountOfUseHandler()
        {
            if (head != null)
            {
                head.SetCountOfUse -= 1;
                if (head.GetCountOfUse == 0)
                {
                    RemoveHeadEquip();
                }
            }
            if (body != null)
            {
                body.SetCountOfUse -= 1;
                if (body.GetCountOfUse == 0)
                {
                    RemoveBodyEquip();
                }
            }

        }

        public void WeaponHandler()
        {
            if (hand != null)
            {
                hand.SetCountOfUse -= 1;
                if (hand.GetCountOfUse == 0)
                {
                    RemoveHandEquip();
                }
            }
        }
    }
}
