using System;
using System.Collections.Generic;

namespace Cum
{
    public class TowerOfHanoi 
    {
        private static int noDisks = 2;
        private static List<string> moves = new();

        public static void Play()
        {
            // Setup the console to be able to print special ASCII chars
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            GameService.PrintHeader();

     //       string input = string.Empty;
            TheGame();
        }

        public TowerOfHanoi()
        {
            
        }

        private static void TheGame()
        {
            Console.Clear();
            moves = new();
            GameService.PrintHeader();

            // Define the 3 rods at the beginning
            Stack<int>[] rods = new Stack<int>[3];

            // Fill the first rod with disks
            rods[0] = GameService.FillRod(noDisks);
            rods[1] = new Stack<int>();
            rods[2] = new Stack<int>();

            // Print the beginning state of the the rods
            GameService.PrintRods(rods[0], rods[1], rods[2], noDisks);

            // Choose between You/Computer
            int mode = GameService.SubMenu();
            // Clear everything and print again the header and the towers
            Console.Clear();
            GameService.PrintHeader();
            GameService.PrintRods(rods[0], rods[1], rods[2], noDisks);
            //if (mode == 0)
            UserPlay(rods);
            //else
            //    ComputerPlay(noDisks, rods[0], rods[1], rods[2]);

            // When the game finish, choose between PlayAgain/Exit
            /*int playAgain = GameService.SubMenuLoop();
            if(playAgain == 0)
            {
                Play();
            }else
            {
                Environment.Exit(0);
            }*/
        }

        // Method that is used when the user play the game (index 0)
        private static void UserPlay(Stack<int>[] rods)
        {
            int noSteps = 0;

            // The user should enter the tower labels From -> To
            string input = string.Empty;
            while (input.ToLower() != "exit")
            {
                char k1 = ' ';
                char k2 = ' ';
                Console.WriteLine(DesignCharConstants.LineBreak);
                Console.WriteLine("# " + noSteps + "\t No.Disks: " + noDisks);
                Console.WriteLine();
                Console.Write("> Move disk from tower: ");
                input = Console.ReadLine();
                if (input.ToLower() == "exit")
                {
                    break;
                }
                k1 = input[0];
                Console.Write("> Move disk to tower: ");
                input = Console.ReadLine();
                if (input.ToLower() == "exit")
                {
                    break;
                }
                k2 = input[0];
                moves.Add($"{k1} -> {k2}");
                // These should be valid stack indexes
                int in1 = GameService.ReturnLabelIndex(k1);
                int in2 = GameService.ReturnLabelIndex(k2);
                

                // Clear the console display
                Console.Clear();
                //ClearPartOfConsole();
                GameService.PrintHeader();

                string error = string.Empty;
                if (in1 == -1 || in2 == -1)
                {
                    GameService.PrintRods(rods[0], rods[1], rods[2], noDisks);
                    error = "!!! Invalid input for tower !!!";
                    Console.WriteLine(DesignCharConstants.LineBreak);
                    Console.WriteLine(error);
                    moves.Add(error);
                    noSteps++;
                }
                else if (in1 == in2)
                {
                    GameService.PrintRods(rods[0], rods[1], rods[2], noDisks);
                    error = "!!! You can't take and move disks from the same tower !!!";
                    Console.WriteLine(DesignCharConstants.LineBreak);
                    Console.WriteLine(error);
                    moves.Add(error);
                    noSteps++;
                }
                else if (in1 != -1 && in2 != -1 && in1 != in2) // If we have valid index of a label i.e. the number of the stack
                {
                    // If we took disk from the tower with index 'in1'...
                    if (rods[in1].Count > 0)
                    {
                        int topDisk = rods[in1].Peek();
                        // The disk should be moved to the tower with index 'in2'...
                        if (rods[in2].Count == 0)
                        {
                            rods[in1].Pop();
                            rods[in2].Push(topDisk);
                        }
                        else
                        {
                            if (rods[in2].Peek() > topDisk)
                            {
                                rods[in1].Pop();
                                rods[in2].Push(topDisk);
                            }
                            else
                            {
                                error = "!!! You can't move bigger on top of a smaller disk !!!";
                            }
                        }

                    }
                    else
                    {
                        error = "!!! You can't take disk from an empty tower !!!";
                    }

                    GameService.PrintRods(rods[0], rods[1], rods[2], noDisks);
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine(DesignCharConstants.LineBreak);
                        Console.WriteLine(error);
                        moves.Add(error);
                    }
                    noSteps++;
                    
                    if (rods[2].Count == noDisks)
                    {
                        Console.WriteLine(DesignCharConstants.LineBreak);
                        Console.WriteLine("# " + noSteps + "\t No.Disks: " + noDisks);
                        GameService.YouWonText("Y O U");
                        break;
                    }
                }
            }
        }
    }
}
