using System;

namespace Cum
{
    partial class BattleSystem 
    {
        public void Screen(int ReaperHealthPoints)
        {
            Reaper reaper = new Reaper();
            // Характеристики Маньяка
          //  int maniacHealthPoints = 500;
          //  int maniacDamage = 30;

            // Выводим прямоугольник с количеством ХП Маньяка
            Console.WriteLine("       _____________________________ ");
            Console.WriteLine("      |                             |");
            Console.WriteLine($"      |      Maniac's HP: {ReaperHealthPoints}       |");
            Console.WriteLine("      |_____________________________|");
            if (ReaperHealthPoints >= 700)
            {
                Console.WriteLine(Constants.full_health);
            }
            if (ReaperHealthPoints < 700 && ReaperHealthPoints > 400)
            {
                Console.WriteLine(Constants.medium_heath);
            }
            if (ReaperHealthPoints <= 400 && ReaperHealthPoints >= 0)
            {
                Console.WriteLine(Constants.low_health);
            }
           /* // Выводим изображение Маньяка в центре консоли
            Console.WriteLine("                   ,#####,");
            Console.WriteLine("                   #_   _#");
            Console.WriteLine("                   |a` `a|");
            Console.WriteLine("                   |  u  |");
            Console.WriteLine("                   \\  =  /");
            Console.WriteLine("                   |\\___/|");
            Console.WriteLine("          ___ ____/:     :\\____ ___");
            Console.WriteLine("        .'   `.-===-\\   /-===-.`   '.");
            Console.WriteLine("       /      .-\"\"\"\"\"-.\"\"\"\"\"\"-.      \\");
            Console.WriteLine("      /'             =:=             '\\");
            Console.WriteLine("    .'  ' .:    o   -=:=-   o    :. '  `.");
            Console.WriteLine("    (.'   /'. '-.....-'-.....-' .\\   '.)");
            Console.WriteLine("    /' ._/   \".     --:--     .\"   \\_. '\\");
            Console.WriteLine("   |  .'|      \".  ---:---  .\"      |'.  |");
            Console.WriteLine("   |  : |       |  ---:---  |       | :  |");
            Console.WriteLine("    \\ : |       |_____._____|       | : /");
            Console.WriteLine("    /   (       |----|------|       )   \\");
            Console.WriteLine("   /... .|      |    |      |      |. ...\\");
            Console.WriteLine("  |::::/'\\' jgs /     |       \\  )xx'\\::::|x[;;;;;;;;;;;;;;;>");
            Console.WriteLine("  ''\"\"\"\"       /'    .L_      '\\      \"\"\"\"\"'");
            Console.WriteLine("             /'-.,__/` `\\__..-'\\");
            Console.WriteLine("            ;      /     \\      ;");
            Console.WriteLine("           :     /       \\     |");
            Console.WriteLine("            |    /         \\.   |");
            Console.WriteLine("            |`../           |  ,/");
            Console.WriteLine("            ( _ )           |  _)");
            Console.WriteLine("            |   |           |   |");
            Console.WriteLine("            |___|           \\___|");
            Console.WriteLine("            :===|            |==|");
            Console.WriteLine("             \\  /            |__|");
            Console.WriteLine("             /\\/\\           /\"\"\"`8.__");
            Console.WriteLine("             |oo|           \\__.//___)");
            Console.WriteLine("             |==|");
            Console.WriteLine("             \\__/");*/

        
            Console.WriteLine("┌───────────────┬───────────┬────────────────────┬────────┐");
            Console.WriteLine("│   Инвентарь   │   Атака   │  Использовать бинт │  Побег │");
            Console.WriteLine("├───────────────┼───────────┼────────────────────┼────────┤");
            Console.WriteLine("│       I       │     A     │         M          │   P    │");
            Console.WriteLine("└───────────────┴───────────┴────────────────────┴────────┘");
        }

        public void WinGame()
        {
            Console.Clear();
            Console.WriteLine(Constants.win);
            Console.ReadKey();
            System.Environment.Exit(0);
        }
    }
}