using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Cum
{
    class Program
    {

        public static void Main(string[] args)
        {
            /*   async Task Music()
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
               }*/
            Game game = new Game("Kill");
            game.OpenGame();
            /* ConsoleKeyInfo key;
             CancellationTokenSource cst = new CancellationTokenSource();
             Music();
             while (true)
             {
                 Console.ForegroundColor = ConsoleColor.Yellow;
                 Console.WriteLine(game.HorizontalUpLine(game.CurrentMap.LevelMap.GetLength(0)));
                 Console.Write(game.CurrentMap.MapText());
                 Console.WriteLine(game.HorizontalDownLine(game.CurrentMap.LevelMap.GetLength(0)));
 
                 if (!is_start)
                 {
                     Console.Clear();
                     Console.WriteLine(Constants.Skul1);
                     Console.ReadKey();
                     is_start = true;
                     cst.Cancel();
                     cst = null;
                     Console.Clear();
                     Console.WriteLine(game.HorizontalUpLine(game.CurrentMap.LevelMap.GetLength(0)));
                     Console.Write(game.CurrentMap.MapText());
                     Console.WriteLine(game.HorizontalDownLine(game.CurrentMap.LevelMap.GetLength(0)));
                 }
 
                 Console.ForegroundColor = ConsoleColor.White;
                 Console.Write(game.HorizontalUpLine(game.CurrentPlayer.HP / 5));
                 Console.Write("  " + game.HorizontalUpLine(game.CurrentPlayer.Stamina / 5));
                 game.ShowHP();
                 game.ShowStamina();
                 Console.Write(game.CurrentPlayer.X + " " + game.CurrentPlayer.Y);
                 key = Console.ReadKey();
                 Console.Clear();
                 game.KeyHandler(key.KeyChar);
             }*/
        }
    }
}
