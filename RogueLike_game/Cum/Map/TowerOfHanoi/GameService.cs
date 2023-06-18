﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Cum
{
    public static class GameService
    {
        public static void PrintRods(Stack<int> stack1, Stack<int> stack2, Stack<int> stack3, int noDisks)
        {
            Rod rod1 = new Rod(noDisks, 0);
            Rod rod2 = new Rod(noDisks, 1);
            Rod rod3 = new Rod(noDisks, 2);

            string space = string.Empty;
            int s1 = 0;
            int s2 = 0;
            int s3 = 0;
            // 1. Print the disks (if there are any)
            for (int i = 1; i <= noDisks; i++)
            {
                string line = string.Empty;
                if (stack1.Count == 0)
                {
                    space = Space(noDisks * 2);
                    line += ConstructLine(space, DesignCharConstants.RodChar);
                }
                else
                {
                    if (i <= (noDisks - stack1.Count))
                    {
                        space = Space(noDisks * 2);
                        line += ConstructLine(space, DesignCharConstants.RodChar);
                    }
                    else
                    {
                        // if there are disks
                        Disk disk = new Disk(stack1.ElementAt(s1));
                        string diskString = disk.GetDisk;
                        space = Space((noDisks * 2) - stack1.ElementAt(s1));
                        line += ConstructLine(space, diskString);
                        s1++;
                    }

                }
                line += DesignCharConstants.SpaceBetweenTowers;
                // 2nd rod
                if (stack2.Count == 0)
                {
                    space = Space(noDisks * 2);
                    line += ConstructLine(space, DesignCharConstants.RodChar);
                }
                else
                {
                    if (i <= (noDisks - stack2.Count))
                    {
                        space = Space(noDisks * 2);
                        line += ConstructLine(space, DesignCharConstants.RodChar);
                    }
                    else
                    {
                        // if there are disks
                        Disk disk = new Disk(stack2.ElementAt(s2));
                        string diskString = disk.GetDisk;
                        space = Space((noDisks * 2) - stack2.ElementAt(s2));
                        line += ConstructLine(space, diskString);
                        s2++;
                    }

                }
                line += DesignCharConstants.SpaceBetweenTowers;
                // 3rd rod
                if (stack3.Count == 0)
                {
                    space = Space(noDisks * 2);
                    line += ConstructLine(space, DesignCharConstants.RodChar);
                }
                else
                {
                    if (i <= (noDisks - stack3.Count))
                    {
                        space = Space(noDisks * 2);
                        line += ConstructLine(space, DesignCharConstants.RodChar);
                    }
                    else
                    {
                        // if there are disks
                        Disk disk = new Disk(stack3.ElementAt(s3));
                        string diskString = disk.GetDisk;
                        space = Space((noDisks * 2) - stack3.ElementAt(s3));
                        line += ConstructLine(space, diskString);
                        s3++;
                    }

                }
                Console.WriteLine(line);
            }

            // 2. Print the rod base
            Console.WriteLine(rod1.RodBase + DesignCharConstants.SpaceBetweenTowers + rod2.RodBase + DesignCharConstants.SpaceBetweenTowers + rod3.RodBase);

            string labelRods = string.Empty;
            // 3. Print the rod label
            int spaceLabel = (rod1.RodBase.Length / 2) - ((rod1.Label.Length) / 2);
            labelRods += Space(spaceLabel) + rod1.Label + Space(spaceLabel);

            labelRods += DesignCharConstants.SpaceBetweenTowers;

            spaceLabel = (rod2.RodBase.Length / 2) - ((rod2.Label.Length) / 2);
            labelRods += Space(spaceLabel) + rod2.Label + Space(spaceLabel);

            labelRods += DesignCharConstants.SpaceBetweenTowers;

            spaceLabel = (rod3.RodBase.Length / 2) - ((rod3.Label.Length) / 2);
            labelRods += Space(spaceLabel) + rod3.Label + Space(spaceLabel);

            Console.WriteLine(labelRods);
        }

        // This was done in order to reduce the display flickering 
        // 4 is the number of rows of the header
        public static void ClearPartOfConsole(int rowIndex)
        {
            Console.CursorTop = rowIndex;
            Console.Write(new string(' ', Console.WindowWidth));
        }

        private static string ConstructLine(string space, string el)
        {
            return space + el + space;
        }

        // The space first is calculated in order to find the correct length
        private static string Space(int size)
        {
            string s = string.Empty;
            for (int i = 0; i < size; i++)
            {
                s += " ";
            }
            return s;
        }

        public static int SubMenu()
        {
            Console.WriteLine();
            Console.WriteLine();
            return 0;
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - (Console.WindowWidth >= Console.BufferWidth ? 1 : 0));
        }

        public static void ClearToEndOfCurrentLine()
        {
            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;
            Console.Write(new String(' ', Console.WindowWidth - currentLeft));
            Console.SetCursorPosition(currentLeft, currentTop);
        }

        // Return the index of a label name
        public static int ReturnLabelIndex(char l)
        {
            for (int i = 0; i < DesignCharConstants.RodLabels.Length; i++)
            {
                if (DesignCharConstants.RodLabels[i] == l)
                {
                    return i;
                }
            }
            return -1;
        }

        public static Stack<int> FillRod(int n)
        {
            Stack<int> stack = new Stack<int>();

            for (int i = n; i > 0; i--)
            {
                stack.Push(i);
            }

            return stack;
        }

        public static void YouWonText(string role)
        {
            Console.WriteLine(DesignCharConstants.LineBreak);
            Console.WriteLine();
            Console.WriteLine($"> > > !!! {role}   W O N !!! < < <");
            Console.WriteLine();
        }

        public static void PrintHeader()
        {
            foreach(string s in DesignCharConstants.Header)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}