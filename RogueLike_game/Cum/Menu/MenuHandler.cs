using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Menu
{
    class MenuHandler
    {
        public void Start()
        {
            Title = "A fucking roguelike";
            RunMainMenu();

        }
        private void RunMainMenu()
        {
            string prompt = @"
 ▄██████▄   ▄████████    ▄████████    ▄████████ ███▄▄▄▄   
███    ███ ███    ███   ███    ███   ███    ███ ███▀▀▀██▄ 
███    ███ ███    █▀    ███    █▀    ███    ███ ███   ███ 
███    ███ ███         ▄███▄▄▄       ███    ███ ███   ███ 
███    ███ ███        ▀▀███▀▀▀     ▀███████████ ███   ███ 
███    ███ ███    █▄    ███    █▄    ███    ███ ███   ███ 
███    ███ ███    ███   ███    ███   ███    ███ ███   ███ 
 ▀██████▀  ████████▀    ██████████   ███    █▀   ▀█   █▀  
                                                          
            
Hey, glad to see you in our Rog, fucking, ulike.
(to control the menu, use the down-up arrow to push, you won't get lost. Enter - to choose)";
            string[] options = { "Play", "Save Game", "Load Game", "Learning", "Setting", "About of creaters", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    return;
                  //  RunGame();
                    break;
                case 1:
                    SaveGame();
                    break;
                case 2:
                    LoadGame();
                    break;
                case 3:
                    Learning();
                    break;
                case 4:
                    DisplayOptionsMenu();
                    break;
                case 5:
                    DisplayAboutInfo();
                    break;
                case 6:
                    ExitGame();
                    break;
            }
            ReadKey(true);
        }
        private void RunGame()
        {
            string prompt = "Get ready to rip your ass";
            string[] options = { "Play to game", "Back to menu" };
            Menu runMenu = new Menu(prompt, options);
            int selectedIndex = runMenu.Run();
            switch(selectedIndex) 
            {
                case 0:
                    break;
                case 1:
                    RunMainMenu();
                    break;
            }
        }
        private void DisplayOptionsMenu()
        {
            string prompt = "Here you can customize the game for yourself";
            string[] options = { "Sound settings", "Control Settings", "Back to menu" };
            Menu optionsMenu = new Menu(prompt, options);
            int selectedIndex = optionsMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    openSoundSettings();
                    break;
                case 1:
                    openControlSettings();
                    break;
                case 2:
                    RunMainMenu();
                    break;
            }
        }
        private void Learning()
        {
            string prompt = (@"Medicine {+} - hill from 15 to 50 hp
Axe {p} - Damage - 30, blocking - 20. Stun percentage - 50, durability - 15
Camping Lamp {|} - Blocking - 10, +3 visibility cells, durability - 3
Lighter {§} - Basic subject. +2 visibility cells
Flashlight {[} - +5 visibility cells
Branch {o} - Damage - 10, blocking - 5. Stun percentage - 25, durability - 2
Scrap {f} - Opens doors and drawers in 3 moves
Pipe {Ø} - Damage - 15, blocking - 15. Stun percentage - 35, durability - 7
Bottle {o} - Damage - 20, blocking - 0. Stun percentage - 30, durability - 2
Bit {/} - Damage - 20, blocking 20. Stun percentage - 40, durability - 12
Revolver {¬} - Damage - 50, blocking - 6 bullets per map. It can be used in the presence of ammo
Shotgun {¤} - Damage - 250, blocking - 2 bullets per map. It can be used in the presence of ammo
Shaverma {~} - unknown bullshit
Motorcycle helmet {D} - Blocking - 20. Durability - 15
Jacket {U} - Blocking - 5. Durability - 10
Vest {S} - Blocking - 8. Durability - 8
Helmet {N} - Blocking - 10. Durability - 15");
            string[] options = { "<<Back to menu>>" };
            Menu learningMenu = new Menu(prompt, options);
            int selectedIndex = learningMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    RunMainMenu();
                    break;
            }
        }
        private void DisplayAboutInfo()
        {
            string prompt = @"Чепенко Маргарита - Mommy детского сада
Паталаха Арсений - ""Папа, не бей""
Перцев Алексей - DJ Smash
Мальцев Максим - бездарный бездарь
Куракин Даниил - ленивый шакал";
            string[] options = { "<<Вернуться в меню>>" };
            Menu authorsMenu = new Menu(prompt, options);
            int selectedIndex = authorsMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    RunMainMenu();
                    break;
            }
        }
        private void ExitGame()
        {
            Clear();
            WriteLine("\nPress any key");
            ReadKey(true);
            Environment.Exit(0);
        }
        private void SaveGame()
        {
            string prompt = "";
            string[] options = { "First case", "Back to menu" };
            Menu saveMenu = new Menu(prompt, options);
            int selectedIndex = saveMenu.Run();
            switch (selectedIndex)
            {
                case 0:

                    break;
                case 1:
                    RunMainMenu();
                    break;
            }
        }
        private void LoadGame()
        {
            string prompt = "";
            string[] options = { "First case", "Back to menu" };
            Menu loadMenu = new Menu(prompt, options);
            int selectedIndex = loadMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    break;
                case 1:
                    RunMainMenu();
                    break;
            }
        }
        private void openSoundSettings()
        {
            string prompt = "Here you can adjust the volume level";
            string[] options = { "High", "Medium", "Low", "Back to settings" };
            Menu soundMenu = new Menu(prompt, options);
            int selectedIndex = soundMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    DisplayOptionsMenu();
                    break;
            }
        }
        private void openControlSettings()
        {
            string prompt = "Here you can configure the controls";
            string[] options = { " ", " ", " ", "Back to settings" };
            Menu soundMenu = new Menu(prompt, options);
            int selectedIndex = soundMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    DisplayOptionsMenu();
                    break;
            }
        }
    }
}
