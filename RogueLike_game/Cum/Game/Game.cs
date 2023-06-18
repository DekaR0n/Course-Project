using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Menu;

namespace Cum
{
    internal class Game
    {
        private const ConsoleKey KEY_WEST = ConsoleKey.A;
        private const ConsoleKey KEY_NORTH = ConsoleKey.W;
        private const ConsoleKey KEY_EAST = ConsoleKey.D;
        private const ConsoleKey KEY_SOUTH = ConsoleKey.S;
        private const ConsoleKey KEY_F = ConsoleKey.F;
        private const ConsoleKey KEY_I = ConsoleKey.I;
        private const ConsoleKey KEY_M = ConsoleKey.M;
        private const ConsoleKey KEY_H = ConsoleKey.H;
        private const ConsoleKey KEY_ESC = ConsoleKey.P;

        private bool OpenInventory = false;
        static bool is_start = false;
        private bool is_hanoi = false;
        private bool eated_shaurma = false;
        private MapSpace savedOld;
        private MapSpace savedNew;
        private int blind_time = 5;
        ConsoleKeyInfo key;
        CancellationTokenSource cst = new CancellationTokenSource();
        private int UntillDie = 20;
            async Task Music()
            {
                await Task.Run(() => M());
            }

            void M()
            {
                while (!is_start)
                {
                    Console.Beep(200, 1700);
                    Console.Beep(350, 1500);
                    Console.Beep(300, 2000);
                }
            }
        public MapLevel CurrentMap { get; set; }
        public Player CurrentPlayer { get; }
        public Reaper CurrentReaper { get; set; }
        

        public Game(string PlayerName)
        {
            this.CurrentMap = new MapLevel();
            this.CurrentPlayer = new Player(PlayerName);
            CurrentReaper = new Reaper();
            PlayerDirection();
        }
        
        public void PlayerDirection()
        {
            for (int i = 0; i < CurrentMap.LevelMap.GetLength(0); i++)
            {
                for (int j = 0; j < Constants.MAP_HT; j++)
                {
                    if (CurrentMap.LevelMap[i, j].DisplayCharacter == Constants.PERSON)
                    {
                        this.CurrentPlayer.X = i;
                        this.CurrentPlayer.Y = j;
                    }
                }
            }
        }
        public void ReaperDirection()
        {
            for (int i = 0; i < CurrentMap.LevelMap.GetLength(0); i++)
            {
                for (int j = 0; j < Constants.MAP_HT; j++)
                {
                    if (CurrentMap.LevelMap[i, j].DisplayCharacter == Constants.REAPER)
                    {
                        CurrentReaper.X = i;
                        CurrentReaper.Y = j;
                    }
                }
            }
            savedOld = new MapSpace(Constants.EMPTY, Constants.EMPTY, CurrentReaper.X, CurrentReaper.Y);
        }
        
        public void EraseVisibleZone(int p, int k)
        {
            for (int i = 0; i < CurrentMap.LevelMap.GetLength(0); i++)
            {
                for (int j = 0; j < Constants.MAP_HT; j++)
                {
                    if (Math.Abs((p - i))>=2 || (Math.Abs((k - j))>=2))
                    {
                        {
                            this.CurrentMap.LevelMap[i, j].DisplayCharacter = ' '; 
                        }
                    }
                }
            }
        }
        public void DrawVisibleZone(int p, int k)
        {
            try
            {
                for (int i = 0; i < CurrentPlayer.getVisibleX; i++)
                {
                    for (int j = 0; j<= CurrentPlayer.getVisibleY; j++)
                    {
                      if (j != 0 || i != 0) 
                      {
                            if (((Constants.street_char.Contains(this.CurrentMap.LevelMap[p, k].MapCharacter)||
                                 CurrentMap.LevelMap[p,k].GetDropLocation == 1)&&
                                CurrentMap.LevelMap[i,j].GetDropLocation != 2 && CurrentMap.LevelMap[p,k].GetDropLocation != 2))
                            {
                                if (p-i > 0 && k-j>0)
                                    ChangeStreetVisibility(p-i, k-j);
                                if (p-i>0 && k+j<Constants.MAP_HT)
                                    ChangeStreetVisibility(p - i, k + j);
                                if (p+i<Constants.MAP_WD && k-j>0)
                                    ChangeStreetVisibility(p+i, k-j);
                                if (p+i<Constants.MAP_WD && k+j<Constants.MAP_HT)
                                    ChangeStreetVisibility(p+i, k+j);
                            }
                            if ((Constants.house_char.Contains(this.CurrentMap.LevelMap[p, k].MapCharacter)
                                 ||CurrentMap.LevelMap[p,k].GetDropLocation == 2) &&
                                CurrentMap.LevelMap[i,j].GetDropLocation != 1 && CurrentMap.LevelMap[p,k].GetDropLocation != 1)
                            {
                                if (p-i > 0 && k-j>0)
                                    ChangeHouseVisibility(p-i, k-j);
                                if (p-i>0 && k+j<Constants.MAP_HT)
                                    ChangeHouseVisibility(p - i, k + j);
                                if (p+i<Constants.MAP_WD && k-j>0)
                                    ChangeHouseVisibility(p+i, k-j);
                                if (p+i<Constants.MAP_WD && k+j<Constants.MAP_HT)
                                    ChangeHouseVisibility(p+i,k+j);
                            } 
                      }
                    }
                }
            }
            catch
            {
                throw new RowNotInTableException();
            }
        }

        public void ChangeStreetVisibility(int horizontal, int vertical)
        {
            if ((Constants.street_char.Contains(this.CurrentMap.LevelMap[horizontal, vertical].MapCharacter) ||
                Constants.corners.Contains(this.CurrentMap.LevelMap[horizontal, vertical].MapCharacter) ||
                CurrentMap.LevelMap[horizontal, vertical].GetDropLocation==1)&&
                CurrentMap.LevelMap[horizontal, vertical].GetDropLocation!=2)
            {
                if (this.CurrentMap.LevelMap[horizontal, vertical].MapCharacter!=Constants.TRAP)
                    this.CurrentMap.LevelMap[horizontal, vertical].DisplayCharacter = 
                    this.CurrentMap.LevelMap[horizontal, vertical].MapCharacter;
            }
        }
        public void ChangeHouseVisibility(int horizontal, int vertical)
        {
            if ((Constants.house_char.Contains(this.CurrentMap.LevelMap[horizontal, vertical].MapCharacter) ||
                Constants.corners.Contains(this.CurrentMap.LevelMap[horizontal, vertical].MapCharacter)||
                CurrentMap.LevelMap[horizontal, vertical].GetDropLocation==2) &&
                CurrentMap.LevelMap[horizontal, vertical].GetDropLocation!=1)
            {
                if (this.CurrentMap.LevelMap[horizontal, vertical].MapCharacter!=Constants.TRAP)
                    this.CurrentMap.LevelMap[horizontal, vertical].DisplayCharacter = 
                    this.CurrentMap.LevelMap[horizontal, vertical].MapCharacter;
            }
        }
        public void ReaperMove()
        {
            if (!CurrentReaper.Blind)
            {
                bool is_move = false;
                while (!is_move)
                {
                    Random rand = new Random();
                    int direction = rand.Next(1, 5);
                    // int stamina = rand.Next(1, 3);
                    switch (direction)
                    {
                        case 1:
                            if (!Constants.corners.Contains(CurrentMap
                                    .LevelMap[CurrentReaper.X, CurrentReaper.Y - 1].MapCharacter)
                                && (CurrentReaper.Y - 1 > 3) &&
                                CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y - 1].MapCharacter !=
                                Constants.ROOM_DOOR)
                            {
                                is_move = true;
                                savedNew = CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y - 1];
                                CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y - 1].DisplayCharacter =
                                    Constants.REAPER;
                                CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y] = savedOld;
                                savedOld = savedNew;
                                CurrentReaper.Y--;

                            }

                            break;
                        case 2:
                            if (!Constants.corners.Contains(CurrentMap
                                    .LevelMap[CurrentReaper.X + 1, CurrentReaper.Y].MapCharacter)
                                && CurrentReaper.X + 1 < Constants.SIZE_X - 4 &&
                                CurrentMap.LevelMap[CurrentReaper.X + 1, CurrentReaper.Y].MapCharacter !=
                                Constants.ROOM_DOOR)
                            {
                                is_move = true;
                                savedNew = CurrentMap.LevelMap[CurrentReaper.X + 1, CurrentReaper.Y];
                                CurrentMap.LevelMap[CurrentReaper.X + 1, CurrentReaper.Y].DisplayCharacter =
                                    Constants.REAPER;
                                CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y] = savedOld;
                                savedOld = savedNew;
                                CurrentReaper.X++;
                            }

                            break;
                        case 3:
                            if (CurrentReaper.X - 1 > 4 && !Constants.corners.Contains(CurrentMap
                                    .LevelMap[CurrentReaper.X - 1, CurrentReaper.Y].MapCharacter) &&
                                CurrentMap.LevelMap[CurrentReaper.X - 1, CurrentReaper.Y].MapCharacter !=
                                Constants.ROOM_DOOR)
                            {
                                is_move = true;
                                savedNew = CurrentMap.LevelMap[CurrentReaper.X - 1, CurrentReaper.Y];
                                CurrentMap.LevelMap[CurrentReaper.X - 1, CurrentReaper.Y].DisplayCharacter =
                                    Constants.REAPER;
                                CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y] = savedOld;
                                savedOld = savedNew;
                                CurrentReaper.X--;
                            }

                            break;
                        case 4:
                            if (!Constants.corners.Contains(CurrentMap
                                    .LevelMap[CurrentReaper.X, CurrentReaper.Y + 1].MapCharacter)
                                && CurrentReaper.Y + 1 < Constants.SIZE_Y - 4 &&
                                CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y + 1].MapCharacter !=
                                Constants.ROOM_DOOR)
                            {
                                is_move = true;
                                savedNew = CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y + 1];
                                CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y + 1].DisplayCharacter =
                                    Constants.REAPER;
                                CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y] = savedOld;
                                savedOld = savedNew;
                                CurrentReaper.Y++;
                            }
                            break;
                    }

                }
                FightRun();
            }
            else
            {
                blind_time--;
                if (blind_time == 0)
                {
                    CurrentReaper.Blind = false;
                    blind_time = CurrentReaper.getMaxBlind;
                }
            }
        }

        public void FightRun()
        {
            if ((Constants.street_char.Contains(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter))
                && ((CurrentPlayer.X - CurrentReaper.X) * (CurrentPlayer.X - CurrentReaper.X) +
                    (CurrentPlayer.Y - CurrentReaper.Y) * (CurrentPlayer.Y - CurrentReaper.Y)) <= 9)
            {
                BattleSystem battle = new BattleSystem();
                
                bool fight_over = false;
                ConsoleKeyInfo key;
                CurrentReaper.setHealth = CurrentReaper.MAX_HEALTH;
                while (!fight_over || !CurrentReaper.Blind)
                {
                    if (CurrentPlayer.HP<=0) 
                        System.Environment.Exit(0);
                    if (CurrentReaper.getHealth == 0)
                    {
                        fight_over = true;
                        battle.WinGame();
                    }
                  //  CurrentReaper.setHealth = CurrentReaper.MAX_HEALTH;
                    Console.Clear();
                    battle.Screen(CurrentReaper.getHealth);
                    CurrentPlayer.ShowHP();
                    key = Console.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.I:
                            if (!OpenInventory)
                            {
                                Console.Clear();
                                CurrentPlayer.InventoryToString();
                                OpenInventory = true;
                                while (OpenInventory)
                                { 
                                    Console.Clear();
                                    CurrentPlayer.InventoryToString();
                                    key = Console.ReadKey();
                                    InventoryHandler(key.KeyChar);
                                }
                            }
                            break;
                        case ConsoleKey.A:
                            if (!((CurrentPlayer.hand is Shotgun || CurrentPlayer.hand is Revolver) && !CurrentPlayer.IsAmmo()))
                            {
                                Random random = new Random();
                                
                                if (CurrentPlayer.hand!=null && random.Next(1, 101) > CurrentPlayer.hand.GetChanseStan)
                                {
                                    fight_over = true;
                                    CurrentMap.Messages.Add("You successfully stunned reaper");
                                    CurrentReaper.Blind = true;
                                }
                                int hp = CurrentReaper.getHealth - CurrentPlayer.Strength;
                                CurrentReaper.setHealth = hp;
                                CurrentPlayer.WeaponHandler();
                            }
                            CurrentPlayer.HP =
                                CurrentPlayer.HP - (CurrentReaper.MAX_DAMAGE - CurrentPlayer.getArmor);
                            if (CurrentPlayer.hand != null)
                            {
                                CurrentPlayer.CountOfUseHandler();
                            }

                            break;
                        case ConsoleKey.P:
                            if (CurrentPlayer.IsBottle() || CurrentPlayer.IsBranch())
                            {
                                CurrentPlayer.HP = CurrentPlayer.HP - (CurrentReaper.MAX_DAMAGE-CurrentPlayer.getArmor);
                                CurrentPlayer.CountOfUseHandler();
                                CurrentReaper.Blind = true;
                                fight_over = true;
                                CurrentMap.Messages.Add("You successfully escaped from reaper");
                            }
                            break;
                        case ConsoleKey.M:
                            CurrentPlayer.UseMedicine();
                          //  CurrentPlayer.HP = CurrentPlayer.HP - (CurrentReaper.MAX_DAMAGE-CurrentPlayer.getArmor);
                            CurrentPlayer.CountOfUseHandler();
                            break;
                    }
                }
            }
        }

        public void EraseReaper()
        {
            if (Math.Abs(CurrentReaper.X - CurrentPlayer.X) > CurrentPlayer.getVisibleX 
                || Math.Abs(CurrentReaper.Y - CurrentPlayer.Y) > CurrentPlayer.getVisibleY)
            {
                CurrentMap.LevelMap[CurrentReaper.X, CurrentReaper.Y].DisplayCharacter = ' ';
            }
        }
        public void KeyHandler(ConsoleKey KeyVal)
        {
            if (CurrentPlayer.HP <= 0)
            {
                CurrentPlayer.Dead();
            }
            GotIntoTrap();
            switch (KeyVal)
            {
                case KEY_WEST: 
                    if (!Constants.corners.Contains(this.CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].MapCharacter) 
                        && CurrentPlayer.X-1>0)
                    {
                        changeDirection(CurrentPlayer.X-1, CurrentPlayer.Y, CurrentPlayer.X, CurrentPlayer.Y, false);
                        this.CurrentPlayer.X--;
                        ReaperMove();
                        EraseReaper();
                    }
                    else
                    {
                        DrawingZone(CurrentPlayer.X,CurrentPlayer.Y);
                    }
                    break;
                case KEY_NORTH:
                    if (!Constants.corners.Contains(this.CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].MapCharacter) 
                        && CurrentPlayer.Y - 1 > 0)
                    {
                        changeDirection(CurrentPlayer.X, CurrentPlayer.Y-1, CurrentPlayer.X, CurrentPlayer.Y, false);
                        this.CurrentPlayer.Y--;
                        ReaperMove();
                        EraseReaper();
                    }
                    else
                    {
                        DrawingZone(CurrentPlayer.X,CurrentPlayer.Y);
                    }
                    break;
                case KEY_EAST:
                    if (!Constants.corners.Contains(this.CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].MapCharacter)
                        && CurrentPlayer.X + 1<Constants.MAP_WD)
                    {
                        changeDirection(CurrentPlayer.X+1, CurrentPlayer.Y, CurrentPlayer.X, CurrentPlayer.Y, false);
                        this.CurrentPlayer.X++;
                        ReaperMove();
                        EraseReaper();
                    }
                    else
                    {
                        DrawingZone(CurrentPlayer.X, CurrentPlayer.Y);
                    }
                    break;
                case KEY_SOUTH:
                    if (!Constants.corners.Contains(this.CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y + 1].MapCharacter)
                        && CurrentPlayer.Y+1<Constants.MAP_HT)
                    {
                        changeDirection(CurrentPlayer.X, CurrentPlayer.Y+1, CurrentPlayer.X, CurrentPlayer.Y, false);
                        this.CurrentPlayer.Y++;
                        ReaperMove();
                        EraseReaper();
                    }
                    else
                    { 
                        DrawingZone(CurrentPlayer.X, CurrentPlayer.Y);
                    }
                    break;
                case KEY_F:
                    TakeStuff();
                    break;
                case KEY_I:
                    if (!OpenInventory)
                    {
                        Console.Clear();
                        CurrentPlayer.InventoryToString();
                        OpenInventory = true;
                    }
                    else
                    {
                        OpenInventory = false;
                    }
                    break;
                case KEY_M:
                    if (!CurrentPlayer.UseMedicine())
                    {
                        CurrentMap.Messages.Add(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].CantUseItem());
                    }
                    break;
                case KEY_H:
                    EatShaurma();
                    break;
                case KEY_ESC:
                    MenuStarter();
                    break;
            }
        }

        public void MenuStarter()
        {
            MenuHandler menu = new MenuHandler();
            menu.Start();
        }

        public void EatShaurma()
        {
            if (CurrentPlayer.UseShaurma())
            {
                eated_shaurma = true;
            }
            else
            {
                CurrentMap.Messages.Add("You don't have a shaurma");
            }
        }

        public void GotIntoTrap()
        {
            Console.WriteLine(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].GetPassiveDamage);
            if (CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].GetPassiveDamage)
            {
                CurrentPlayer.HP -= Constants.TRAP_DAMAGE;
                CurrentMap.Messages.Add(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].ToString());
                CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].SetPassiveDamage = false;
                CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter = Constants.EMPTY;
            }
        }
        
        public void TakeStuff()
        {
            if (CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.TOWERS_OF_HANOI 
                && CurrentPlayer.items.Count<8)
            {
                Console.Clear();
                TowerOfHanoi.Play();
                CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter = Constants.ROOM_INT;
                CurrentPlayer.items.Add(new Quest(Constants.KEY,Constants.KEY,CurrentPlayer.X,CurrentPlayer.Y));
                is_hanoi = true;
            }

            if (CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.SHOTGUN ||
                CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.REVOLVER)
            {
                if (!is_hanoi || !CurrentPlayer.AddItem(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y]))
                {
                    CurrentMap.Messages.Add(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].CantAddItem());
                }
                else
                {
                    CurrentPlayer.RemoveKey();
                    Console.Beep(2000,100);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter = Constants.ROOM_INT;
                    CurrentMap.Messages.Add(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].ToString());
                }
            }
            else if (CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].GetPickUp)
            {
                if (!CurrentPlayer.AddItem(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y]))
                {
                    CurrentMap.Messages.Add(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].CantAddItem());
                }
                else
                {
                    Console.Beep(2000,100);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter =
                       Constants.street_char.Contains(
                           CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter)
                           ?CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].GetDropLocation==2?Constants.ROOM_INT: Constants.EMPTY
                           :CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].GetDropLocation==1? Constants.EMPTY:Constants.ROOM_INT;
                    CurrentMap.Messages.Add(CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].ToString());
                }
            }
        }
        
        public void changeDirection(int horizontal, int vertical, int x, int y, bool is_reaper)
        {
            this.CurrentMap.LevelMap[x, y].DisplayCharacter =
                this.CurrentMap.LevelMap[x, y].MapCharacter;
            this.CurrentMap.LevelMap[horizontal, vertical].DisplayCharacter = is_reaper?Constants.REAPER :
                Constants.PERSON;
             if (!is_reaper)
              DrawingZone(horizontal,vertical);
        }

        public void DrawingZone(int horizontal, int vertical)
        {
            EraseVisibleZone(horizontal,vertical);
            DrawVisibleZone(horizontal,vertical);
        }
        public string HorizontalUpLine(int index)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < index+1; i++)
            {
                sb.Append(i == 0 ? "╔" : i == (index)? "╗" : "═");
            }
            return sb.ToString();
        }
        public string HorizontalDownLine(int index)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < index+1; i++)
            {
                sb.Append(i == 0 ? "╚" : i == (index) ? "╝" : "═");
            }
            return sb.ToString();
        }

        public void OpenGame()
        {
            ReaperDirection();
            KeyHandler(ConsoleKey.W);
            while (true)
            {
                if (!OpenInventory )
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(HorizontalUpLine(CurrentMap.LevelMap.GetLength(0)));
                    Console.Write(CurrentMap.MapText());
                    Console.WriteLine(HorizontalDownLine(CurrentMap.LevelMap.GetLength(0)));

                    if (!is_start)
                    {
                        Console.Clear();
                        Console.WriteLine(Constants.Skul1);
                        Console.ReadKey();
                        is_start = true;
                        cst.Cancel();
                        cst = null;
                        Console.Clear();
                        Console.WriteLine(HorizontalUpLine(CurrentMap.LevelMap.GetLength(0)));
                        Console.Write(CurrentMap.MapText());
                        Console.WriteLine(HorizontalDownLine(CurrentMap.LevelMap.GetLength(0)));
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    CurrentPlayer.ShowHP();
                    CurrentPlayer.ShowStamina();
                }
                if (eated_shaurma)
                {
                    UntillDie--;
                    if (UntillDie <= 0)
                    {
                        CurrentMap.Messages.Add("You died from shaurma");
                        System.Environment.Exit(0);
                    }
                }

                key = Console.ReadKey();
                KeyHandler(key.Key); 
                while (OpenInventory)
                {
                    key = Console.ReadKey();
                    Console.Clear();
                    CurrentPlayer.InventoryToString();
                    InventoryHandler(key.KeyChar);
                }
            }
        }
        
        private char chosen;
        public void InventoryHandler(char KeyVal)
        {
            int max = 57;
            try
            {
                if (KeyVal == 'i')
                {
                    OpenInventory = false;
                }

                if (KeyVal > 48 && KeyVal <= 57 && KeyVal <= max)
                {
                    chosen = KeyVal;
                    Console.WriteLine("You choose " +(KeyVal-48)+". "+ CurrentPlayer.items[KeyVal-49].GetType());
                }
                if (KeyVal == 48 && chosen > 48 && chosen <= 57)
                {
                    List<Item> newList = new List<Item>(CurrentPlayer.items.Count-1);
                   for (int i = 0; i < CurrentPlayer.items.Count; i++)
                   {
                       if (i != chosen - 49)
                       {
                           newList.Add(CurrentPlayer.items[i]);
                       }
                   }
                   if (DropItem(CurrentPlayer.items[chosen-49], (chosen-49)))
                         CurrentPlayer.items = newList;
                   Console.Clear();
                   CurrentPlayer.InventoryToString();
                }
                if (KeyVal == 'e' && chosen > 48 && chosen <= 57)
                {
                    EquipLight(CurrentPlayer.items[chosen-49]);
                    Console.Clear();
                    CurrentPlayer.InventoryToString();
                //    Console.WriteLine("You successfully used "+CurrentPlayer.items[chosen-50].GetType());
                }
            }
            catch
            {
                return;
            }
        }

        public void EquipLight(Item item)
        {
            if (item is Medicine)
            {
                CurrentPlayer.UseMedicine();
            }

            if (item is Flashlight)
            {
                CurrentPlayer.setVisibleX = 7;
                CurrentPlayer.setVisibleY = 3;
            }

            if (item is CampingLamp)
            {
                CurrentPlayer.setVisibleX = 8;
                CurrentPlayer.setVisibleY = 4;
            }

            int armor = CurrentPlayer.getArmor;
            if (item is MotorcycleHelmet)
            {
            //    item = item as MotorcycleHelmet;
                if (CurrentPlayer.head == null)
                {
                    CurrentPlayer.setArmor = armor + item.GetProtect;
                    CurrentPlayer.head = item;
                }
                else if (CurrentPlayer.head is Helmet)
                {
                    CurrentPlayer.setArmor = armor + item.GetProtect-10;
                    CurrentPlayer.head = item;
                }
            }
            if (item is Helmet)
            {
              //  item = item as Helmet;
                if (CurrentPlayer.head == null)
                {
                    CurrentPlayer.setArmor = armor + item.GetProtect;
                    CurrentPlayer.head = item;
                }
                else if (CurrentPlayer.head is MotorcycleHelmet)
                {
                    CurrentPlayer.setArmor = armor -item.GetProtect;
                    CurrentPlayer.head = item;
                }
            }
            if (item is Jacket)
            {
                if (CurrentPlayer.body == null)
                {
                    CurrentPlayer.setArmor = armor + item.GetProtect;
                    CurrentPlayer.body = item;
                }
                else if (CurrentPlayer.body is LifeVest)
                {
                    CurrentPlayer.setArmor = armor -item.GetProtect;
                    CurrentPlayer.body = item;
                }
            }
            if (item is LifeVest)
            { 
                if (CurrentPlayer.body == null)
                {
                    CurrentPlayer.setArmor = armor + item.GetProtect;
                    CurrentPlayer.body = item;
                }
                else if (CurrentPlayer.body is Jacket)
                {
                    CurrentPlayer.setArmor = armor + item.GetProtect-10;
                    CurrentPlayer.body = item;
                }
            }

            if (item is Weapon)
            {
                CurrentPlayer.Strength = CurrentPlayer.getStartingStrength + item.GetProtect;
                CurrentPlayer.hand = item;
            }
        }
        
        public bool DropItem(Item item, int i)
        {
            if ((CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].MapCharacter == Constants.EMPTY ||
                CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT)&& 
                (CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY ||
                CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT))
            {
                if (item is Medicine)
                {
                    Medicine medicine = item as Medicine;
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y] = new Medicine(medicine.GetMedicine(),
                        Constants.MEDICINE, CurrentPlayer.X + 1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Pipe)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y] = 
                        new Pipe(Constants.PIPE, Constants.PIPE, CurrentPlayer.X + 1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is BaseballBat)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y] = 
                        new BaseballBat(Constants.BIT, Constants.BIT, CurrentPlayer.X + 1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Bottle)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y] = new Bottle(
                        Constants.BOTTLE, Constants.BOTTLE, CurrentPlayer.X+1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is MotorcycleHelmet)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y] = new MotorcycleHelmet(
                        Constants.MOTOHELMET, Constants.MOTOHELMET, CurrentPlayer.X+1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Helmet)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X+1, CurrentPlayer.Y] = new Helmet(
                        Constants.HELMET, Constants.HELMET, CurrentPlayer.X+1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is LifeVest)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X+1, CurrentPlayer.Y] = new LifeVest(
                        Constants.VEST, Constants.VEST, CurrentPlayer.X+1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Jacket)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X+1, CurrentPlayer.Y] = new Jacket(
                        Constants.JACKET, Constants.JACKET, CurrentPlayer.X+1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Lighter)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X+1, CurrentPlayer.Y] = new Lighter(
                        Constants.LIGHTER,  CurrentPlayer.X+1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is CampingLamp)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X+1, CurrentPlayer.Y] = new CampingLamp(
                        Constants.CAMPING_LAMP,  CurrentPlayer.X+1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Flashlight)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X+1, CurrentPlayer.Y] = new Flashlight(
                        Constants.SMALL_LAMP,  CurrentPlayer.X+1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Branch)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X+1, CurrentPlayer.Y] = new Branch(
                        Constants.BRANCH,Constants.BRANCH,  CurrentPlayer.X+1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Crowbar)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y] = new Crowbar(
                        Constants.SCRAP, Constants.SCRAP, CurrentPlayer.X + 1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X + 1, CurrentPlayer.Y].SetDropLocation =
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT ? 2 : 1;
                    return true;
                }
            }
            if ((CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].MapCharacter == Constants.EMPTY ||
                 CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT)&& 
                (CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY ||
                 CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT))
            {
                if (item is Medicine)
                {
                    Medicine medicine = item as Medicine;
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y] = new Medicine(medicine.GetMedicine(), Constants.MEDICINE, CurrentPlayer.X - 1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                if (item is Crowbar)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y] = new Crowbar(
                        Constants.SCRAP, Constants.SCRAP, CurrentPlayer.X - 1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation =
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT ? 2 : 1;
                    return true;
                }
                if (item is BaseballBat)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y] = 
                        new BaseballBat(Constants.BIT, Constants.BIT, CurrentPlayer.X - 1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Pipe)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y] = 
                        new Pipe(Constants.PIPE, Constants.PIPE, CurrentPlayer.X - 1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Bottle)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y] = new Bottle(
                        Constants.BOTTLE, Constants.BOTTLE, CurrentPlayer.X-1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is MotorcycleHelmet)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y] = new MotorcycleHelmet(
                        Constants.MOTOHELMET, Constants.MOTOHELMET, CurrentPlayer.X-1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    CurrentPlayer.setArmor = 10;
                    return true;
                } 
                if (item is Helmet)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X-1, CurrentPlayer.Y] = new Helmet(
                        Constants.HELMET, Constants.HELMET, CurrentPlayer.X-1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is LifeVest)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X-1, CurrentPlayer.Y] = new LifeVest(
                        Constants.VEST, Constants.VEST, CurrentPlayer.X-1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Jacket)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X-1, CurrentPlayer.Y] = new Jacket(
                        Constants.JACKET, Constants.JACKET, CurrentPlayer.X-1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Lighter)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X-1, CurrentPlayer.Y] = new Lighter(
                        Constants.LIGHTER,  CurrentPlayer.X-1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                if (item is CampingLamp)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X-1, CurrentPlayer.Y] = new CampingLamp(
                        Constants.CAMPING_LAMP,  CurrentPlayer.X-1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                if (item is Flashlight)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X-1, CurrentPlayer.Y] = new Flashlight(
                        Constants.SMALL_LAMP,  CurrentPlayer.X-1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                if (item is Branch)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X-1, CurrentPlayer.Y] = new Branch(
                        Constants.BRANCH,Constants.BRANCH,  CurrentPlayer.X-1, CurrentPlayer.Y);
                    CurrentMap.LevelMap[CurrentPlayer.X - 1, CurrentPlayer.Y].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
            }
            if ((CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].MapCharacter == Constants.EMPTY ||
                 CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].MapCharacter == Constants.ROOM_INT)&& 
                (CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY ||
                 CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT))
            {
                if (item is Medicine)
                {
                    Medicine medicine = item as Medicine;
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1] = new Medicine(medicine.GetMedicine(), Constants.MEDICINE, CurrentPlayer.X, CurrentPlayer.Y+1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                if (item is Crowbar)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1] = new Crowbar(
                        Constants.SCRAP, Constants.SCRAP, CurrentPlayer.X, CurrentPlayer.Y+1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation =
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT ? 2 : 1;
                    return true;
                }
                if (item is BaseballBat)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1] = 
                        new BaseballBat(Constants.BIT, Constants.BIT, CurrentPlayer.X, CurrentPlayer.Y+1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Pipe)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1] = 
                        new Pipe(Constants.PIPE, Constants.PIPE, CurrentPlayer.X, CurrentPlayer.Y+1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Bottle)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1] = new Bottle(
                        Constants.BOTTLE, Constants.BOTTLE, CurrentPlayer.X, CurrentPlayer.Y+1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is MotorcycleHelmet)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y + 1] = new MotorcycleHelmet(
                        Constants.MOTOHELMET, Constants.MOTOHELMET, CurrentPlayer.X, CurrentPlayer.Y + 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                if (item is Helmet)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y + 1] = new Helmet(
                        Constants.HELMET, Constants.HELMET, CurrentPlayer.X, CurrentPlayer.Y + 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is LifeVest)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y + 1] = new LifeVest(
                        Constants.VEST, Constants.VEST, CurrentPlayer.X, CurrentPlayer.Y + 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Jacket)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y + 1] = new Jacket(
                        Constants.JACKET, Constants.JACKET, CurrentPlayer.X, CurrentPlayer.Y + 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Lighter)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y + 1] = new Lighter(
                        Constants.LIGHTER,  CurrentPlayer.X, CurrentPlayer.Y + 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                if (item is CampingLamp)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y + 1] = new CampingLamp(
                        Constants.CAMPING_LAMP,  CurrentPlayer.X, CurrentPlayer.Y + 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                if (item is Flashlight)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y + 1] = new Flashlight(
                        Constants.SMALL_LAMP,  CurrentPlayer.X, CurrentPlayer.Y + 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                if (item is Branch)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y + 1] = new Branch(
                        Constants.BRANCH,Constants.BRANCH,  CurrentPlayer.X, CurrentPlayer.Y + 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y+1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY? 1:2;
                    return true;
                }
                
            }

            if ((CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].MapCharacter == Constants.EMPTY ||
                 CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].MapCharacter == Constants.ROOM_INT) &&
                (CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.EMPTY ||
                 CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT))
            {
                if (item is Medicine)
                {
                    Medicine medicine = item as Medicine; 
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new Medicine(medicine.GetMedicine(), 
                        Constants.MEDICINE, CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT
                            ? 2 
                            : 1;
                    return true;
                }
                if (item is Crowbar)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new Crowbar(
                        Constants.SCRAP, Constants.SCRAP, CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation =
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT ? 2 : 1;
                    return true;
                }
                if (item is BaseballBat)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y-1] = 
                        new BaseballBat(Constants.BIT, Constants.BIT, CurrentPlayer.X, CurrentPlayer.Y-1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y-1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Pipe)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y-1] = 
                        new Pipe(Constants.PIPE, Constants.PIPE, CurrentPlayer.X, CurrentPlayer.Y-1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y-1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is Bottle)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y-1] = new Bottle(
                        Constants.BOTTLE, Constants.BOTTLE, CurrentPlayer.X, CurrentPlayer.Y-1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y-1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT? 2:1;
                    return true;
                }
                if (item is MotorcycleHelmet)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new MotorcycleHelmet(
                        Constants.MOTOHELMET, Constants.MOTOHELMET, CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT
                            ? 2 
                            : 1;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Helmet)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new Helmet(
                        Constants.HELMET, Constants.HELMET, CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT
                            ? 2 
                            : 1;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is LifeVest)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new LifeVest(
                        Constants.VEST, Constants.VEST, CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT
                            ? 2 
                            : 1;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Jacket)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new Jacket(
                        Constants.JACKET, Constants.JACKET, CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT
                            ? 2 
                            : 1;
                    CurrentPlayer.setArmor = 10;
                    return true;
                }
                if (item is Lighter)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new Lighter(
                        Constants.LIGHTER,  CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT
                            ? 2 
                            : 1;
                    return true;
                }
                if (item is CampingLamp)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new CampingLamp(
                        Constants.CAMPING_LAMP,  CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT
                            ? 2 
                            : 1;
                    return true;
                }
                if (item is Flashlight)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new Flashlight(
                        Constants.SMALL_LAMP,  CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT
                            ? 2 
                            : 1;
                    return true;
                }
                if (item is Branch)
                {
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1] = new Branch(
                        Constants.BRANCH,Constants.BRANCH,  CurrentPlayer.X, CurrentPlayer.Y - 1);
                    CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y - 1].SetDropLocation = 
                        CurrentMap.LevelMap[CurrentPlayer.X, CurrentPlayer.Y].MapCharacter == Constants.ROOM_INT
                            ? 2 
                            : 1;
                    return true;
                }
            }
            return false;
        }
    }
}
