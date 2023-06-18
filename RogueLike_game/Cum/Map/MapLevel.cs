using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace Cum
{

    public class MapLevel 
    {
        private enum Direction
        {
            None = 0,
            North = 1,
            East = 2,
            South = -1,
            West = -2
        }
        
        private Dictionary<MapSpace, Direction> deadEnds =
            new Dictionary<MapSpace, Direction>();

        // Random generator

        private bool created_person;
        private bool created_shaurma;
        private bool created_hanoi;
        private bool created_shotgun;
        private bool created_reaper;
        private bool created_revolver;
        // Array to hold map definition.
        private MapSpace[,] levelMap = new MapSpace[Constants.SIZE_X,Constants.SIZE_Y];
        private List<String> messages = new List<String>(){};

        // Random number generator
        private static Random rand = new Random();
        
        public MapSpace[,] LevelMap
        {
            // Make map available to other classes.
            get { return levelMap; }
        }

        public MapSpace[,] SetLevel
        {
            get { return levelMap; }
            set { levelMap = value; }
        }

        public List<String> Messages
        {
            get { return messages; }
        }

        public MapLevel()
        {
            // Constructor - generate a new map for this level.
            MapGeneration();
            
            while (!VerifyMap())
            {
             //   Console.BackgroundColor = ConsoleColor.Red;
                Debug.Write(MapText());
                MapGeneration();
            }
        }

        private bool VerifyMap()
        {
            // Verify that the generate map is free of isolated rooms or sections.

            bool retValue = true;
            List<char> dirCheck = new List<char>();

            // Check horizontal for blank rows which no hallways. Top and bottom might be legitimately blank
            // so just check a portion of the map.

            for (int y = Constants.REGION_HT - Constants.MIN_ROOM_HT; y < (Constants.REGION_HT * 2) + Constants.MIN_ROOM_HT; y++)
            {
                dirCheck.Clear();
                for (int x = 0; x <= Constants.MAP_WD - 1; x++)
                {
                    if (!dirCheck.Contains(levelMap[x, y].MapCharacter))
                        dirCheck.Add(levelMap[x, y].MapCharacter);
                }
                retValue = dirCheck.Count > 1;
                if (!retValue) { break; }
            }

            // Check vertical.

            if (retValue)
            {
                for (int x = Constants.REGION_WD - Constants.MIN_ROOM_WT; x < (Constants.REGION_WD * 2) + Constants.MIN_ROOM_WT; x++)
                {
                    dirCheck.Clear();
                    for (int y = 0; y <= Constants.MAP_HT - 1; y++)
                    {
                        if (!dirCheck.Contains(levelMap[x, y].MapCharacter))
                            dirCheck.Add(levelMap[x, y].MapCharacter);
                    }
                    retValue = dirCheck.Count > 1;
                    if (!retValue) { break; }
                }
            }

            return retValue;
        }

        private void MapGeneration()
        {
            // Primary generation procedure
            // Screen is divided into nine cell regions and a room is randomly generated in each.
            // Room exterior must be at least four spaces in each direction but not more than the
            // size of its cell region, minus one space, to allow for hallways between rooms.

            //var rand = new Random();
            int roomWidth = 0, roomHeight = 0, roomAnchorX = 0, roomAnchorY = 0;

            // Clear map by creating new array of map spaces.
            levelMap = new MapSpace[Constants.SIZE_X, Constants.SIZE_X];

            // Define the map left to right, top to bottom.
            // Increment the count based on a third of the way in each direction.
            // First row and first column of array are skipped so everything is 1 based.
            for (int y = 1; y < Constants.REGION_HT * 2 + 2; y += Constants.REGION_HT)
            {
                for (int x = 1; x < Constants.REGION_WD * 2 + 2; x += Constants.REGION_WD)
                {
                    if (rand.Next(101) <= Constants.ROOM_CREATE_PCT)
                    {
                        // Room size
                        roomHeight = rand.Next(Constants.MIN_ROOM_HT, Constants.MAX_ROOM_HT + 1);
                        roomWidth = rand.Next(Constants.MIN_ROOM_WT, Constants.MAX_ROOM_WT + 1);

                        // Center room in region
                        roomAnchorY = (int)((Constants.REGION_HT - roomHeight) / 2) + y;
                        roomAnchorX = (int)((Constants.REGION_WD - roomWidth) / 2) + x;

                        // Create room - let's section this out in its own procedure
                        RoomGeneration(roomAnchorX, roomAnchorY, roomWidth, roomHeight);
                    }
                }
            }

            // After the rooms are generated, fill in the
            // blanks for the remaining cells.
            for (int y = 0; y < Constants.SIZE_Y; y++)
            {
                for (int x = 0; x < Constants.SIZE_X; x++)
                {
                    if (levelMap[x, y] is null)
                        levelMap[x, y] = new MapSpace(' ', false, false, x, y);
                }
            }

            // Create hallways and add stairway
            HallwayGeneration();
            AddStairway();
            TrapCreator();
            ReaperCreator();
        }

        public void ReaperCreator()
        {
            if (!created_reaper)
            {
                created_reaper = true;
                for (int i = levelMap.GetLength(0) / 2; i < levelMap.GetLength(0); i++)
                {
                    for (int j = Constants.MAP_HT / 2; j < Constants.MAP_HT; j++)
                    {
                        if (rand.Next(1, 101) > 95 && levelMap[i, j].MapCharacter == Constants.HALLWAY)
                        {
                            levelMap[i, j] = new MapSpace(Constants.EMPTY, Constants.REAPER, i, j);
                            return;
                        }
                    }
                }
            }
        }
        
        private void AddStairway()
        {
            int x = 1; int y = 1;

            // Search the array randomly for an interior room space
            // and mark it as a hallway.
            while (levelMap[x, y].MapCharacter != Constants.ROOM_INT)
            {
                x = rand.Next(1, Constants.MAP_WD);
                y = rand.Next(1, Constants.MAP_HT);
            }

            levelMap[x, y] = new MapSpace(Constants.STAIRWAY, x, y);
        }

        
        private void RoomGeneration(int westWallX, int northWallY, int roomWidth, int roomHeight)
        {
            // Create room on map based on inputs

            int eastWallX = westWallX + roomWidth;          // Calculate room east
            int southWallY = northWallY + roomHeight;       // Calculate room south

            // Regions are defined 1 to 9, L to R, top to bottom.
            int regionNumber = GetRegionNumber(westWallX, northWallY);
            int doorway = 0, doorCount = 0, goldX, goldY, personX, personY;

            //var rand = new Random();

            // Create horizontal and vertical walls for room.
            for (int y = northWallY; y <= southWallY; y++)
            {
                for (int x = westWallX; x <= eastWallX; x++)
                {
                    if (y == northWallY || y == southWallY)
                    {
                        levelMap[x, y] = new MapSpace(Constants.HORIZONTAL, false, false, x, y);
                    }
                    else if (x == westWallX || x == eastWallX)
                    {
                        levelMap[x, y] = new MapSpace(Constants.VERTICAL, false, false, x, y);
                    }
                    else if (levelMap[x, y] == null)
                        levelMap[x, y] = new MapSpace(Constants.ROOM_INT, false, false, x, y);
                }
            }

            // Add doorways and initial hallways on room. Room walls facing the edges of the map do not get exits
            // so the ROOM_EXIT_PCT constant needs to be high to ensure that every room gets at least one and we
            // still might need to repeat the process anyway.

            while (doorCount == 0)
            {
                if (regionNumber >= 4 && rand.Next(101) <= Constants.ROOM_EXIT_PCT)  // North doorways
                {
                    doorway = rand.Next(westWallX + 1, eastWallX);
                    levelMap[doorway, northWallY] = new MapSpace(Constants.ROOM_DOOR, false, false, doorway, northWallY);
                    levelMap[doorway, northWallY - 1] = new MapSpace(Constants.HALLWAY, false, false, doorway, northWallY - 1);
                    deadEnds.Add(levelMap[doorway, northWallY - 1], Direction.North);
                    doorCount++;
                }

                if (regionNumber <= 6 && rand.Next(101) <= Constants.ROOM_EXIT_PCT)  // South doorways
                {
                    doorway = rand.Next(westWallX + 1, eastWallX);
                    levelMap[doorway, southWallY] = new MapSpace(Constants.ROOM_DOOR, false, false, doorway, southWallY);
                    levelMap[doorway, southWallY + 1] = new MapSpace(Constants.HALLWAY, false, false, doorway, southWallY + 1);
                    deadEnds.Add(levelMap[doorway, southWallY + 1], Direction.South);
                    doorCount++;
                }

                if ("147258".Contains(regionNumber.ToString()) && rand.Next(101) <= Constants.ROOM_EXIT_PCT)  // East doorways
                {
                    doorway = rand.Next(northWallY + 1, southWallY);
                    levelMap[eastWallX, doorway] = new MapSpace(Constants.ROOM_DOOR, false, false, eastWallX, doorway);
                    levelMap[eastWallX + 1, doorway] = new MapSpace(Constants.HALLWAY, false, false, eastWallX + 1, doorway);
                    deadEnds.Add(levelMap[eastWallX + 1, doorway], Direction.East);
                    doorCount++;
                }

                if ("258369".Contains(regionNumber.ToString()) && rand.Next(101) <= Constants.ROOM_EXIT_PCT)  // West doorways
                {
                    doorway = rand.Next(northWallY + 1, southWallY);
                    levelMap[westWallX, doorway] = new MapSpace(Constants.ROOM_DOOR, false, false, westWallX, doorway);
                    levelMap[westWallX - 1, doorway] = new MapSpace(Constants.HALLWAY, false, false, westWallX - 1, doorway);
                    deadEnds.Add(levelMap[westWallX - 1, doorway], Direction.West);
                    doorCount++;
                }
            }

            // Set the room corners.

            levelMap[westWallX, northWallY] = new MapSpace(Constants.CORNER_NW, false, false, westWallX, northWallY);
            levelMap[eastWallX, northWallY] = new MapSpace(Constants.CORNER_NE, false, false, eastWallX, northWallY);
            levelMap[westWallX, southWallY] = new MapSpace(Constants.CORNER_SW, false, false, westWallX, southWallY);
            levelMap[eastWallX, southWallY] = new MapSpace(Constants.CORNER_SE, false, false, eastWallX, southWallY);

            // Evaluate for an item deposit

            int chance;
            chance = rand.Next(1, 101);
            if (!created_hanoi && chance > 60)
            {
                created_hanoi = true;
                GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY);
                levelMap[medX, medY] = new MapSpace(Constants.TOWERS_OF_HANOI, medX, medY);
            }
            
            int count = rand.Next(1, 6);
            for (int i = 0; i < count; i++)
            {
                chance = rand.Next(1, 101);
                if (chance > Constants.ROOM_MEDICINE_PCT){
                    GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY); 
                    levelMap[medX, medY] = new Medicine(rand.Next(1,4)*15, Constants.MEDICINE, medX, medY); 
                }
            }

            count = rand.Next(1, 3);
            for (int i = 0; i < count; i++)
            {
                chance = rand.Next(1, 101);
                if (chance > Constants.ROOM_PIPE_PCT){
                    GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY); 
                    levelMap[medX, medY] = new Pipe(Constants.PIPE, Constants.PIPE, medX, medY); 
                }
            }
            
            count = rand.Next(1, 5);
            for (int i = 0; i < count; i++)
            {
                chance = rand.Next(1, 101);
                if (chance > Constants.ROOM_CAMPING_PCT){
                    GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY); 
                    levelMap[medX, medY] = new CampingLamp(Constants.CAMPING_LAMP,  medX, medY); 
                }
            }
            
            count = rand.Next(1, 5);
            for (int i = 0; i < count; i++)
            {
                chance = rand.Next(1, 101);
                if (chance > Constants.ROOM_LIGHTER_PCT){
                    GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY); 
                    levelMap[medX, medY] = new Lighter(Constants.LIGHTER,  medX, medY); 
                }
            }
            count = rand.Next(1, 4);
            for (int i = 0; i < count; i++)
            {
                chance = rand.Next(1, 101);
                if (chance > Constants.ROOM_SCRAP_PCT){
                    GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY); 
                    levelMap[medX, medY] = new Crowbar(Constants.SCRAP, Constants.SCRAP, medX, medY); 
                }
            }
            count = rand.Next(1, 5);
            for (int i = 0; i < count; i++)
            {
                chance = rand.Next(1, 101);
                if (chance > Constants.ROOM_PIPE_PCT){
                    GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY); 
                    levelMap[medX, medY] = new BaseballBat(Constants.BIT, Constants.BIT, medX, medY); 
                }
            }
            
            count = rand.Next(1, 3);
            for (int i = 0; i < count; i++)
            {
                chance = rand.Next(1, 101);
                if (chance > Constants.ROOM_LIGHTER_PCT){
                    GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY); 
                    levelMap[medX, medY] = new Ammo(2, Constants.AMMO, medX, medY); 
                }
            }
            
            if (!created_shaurma && rand.Next(1,101)>40)
            {
                created_shaurma = true;
                for (int i = 0; i < 1; i++)
                {
                    GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY);
                    levelMap[medX, medY] = new Shaurma(Constants.SHAVERMA, Constants.SHAVERMA, medX, medY);
                }
            }

            if (!created_shotgun && rand.Next(1, 101) > 50)
            {
                created_shotgun = true;
                GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY);
                levelMap[medX, medY] = new Shotgun(0,Constants.SHOTGUN, Constants.SHOTGUN, medX, medY);
            }

            if (!created_revolver && rand.Next(1, 101) > 80)
            {
                created_revolver = true;
                GenerateItem(westWallX, northWallY, eastWallX, southWallY, out int medX, out int medY);
                levelMap[medX, medY] = new Revolver(0,Constants.REVOLVER, Constants.REVOLVER, medX, medY);
            }
            
            if (!created_person && rand.Next(1, 101) > 55)
            {
                created_person = true;
                personX = westWallX; personY = northWallY;
                
                while (levelMap[personX, personY].MapCharacter != Constants.ROOM_INT)
                {
                    personX = rand.Next(westWallX + 1, eastWallX);
                    personY = rand.Next(northWallY + 1, southWallY);
                }
                levelMap[personX, personY] = new MapSpace(Constants.ROOM_INT,Constants.PERSON, personX, personY);
            }
        }

        private void GenerateItem(int westWallX, int northWallY, int eastWallX, int southWallY, out int medX, out int medY)
        {
            medX = westWallX; medY = northWallY;
                
            while (levelMap[medX, medY].MapCharacter != Constants.ROOM_INT)
            {
                medX = rand.Next(westWallX + 1, eastWallX);
                medY = rand.Next(northWallY + 1, southWallY);
            }
        }

        private void HallwayGeneration()
        {
            // After all rooms are generated with exits and initial hallway characters, scan for any possible disconnected
            // rooms and look for other rooms to connect to.
            Direction hallDirection = Direction.None; Direction direction90; Direction direction270;
            MapSpace hallwaySpace, newSpace;
            Dictionary<Direction, MapSpace> adjacentChars = new Dictionary<Direction, MapSpace>();
            Dictionary<Direction, MapSpace> surroundingChars = new Dictionary<Direction, MapSpace>();
            //var rand = new Random();

            // Iterate through the list of hallway endings (deadends) until all are resolved one way or another.
            // Count backwards so we can remove processed items.

            // If there are doors on more than one side, the hallway is already connected.
            for (int i = deadEnds.Count - 1; i >= 0; i--)
            {
                hallwaySpace = deadEnds.ElementAt(i).Key;

                if (SearchAdjacent(Constants.ROOM_DOOR, hallwaySpace.X, hallwaySpace.Y).Count > 1)
                    deadEnds.Remove(hallwaySpace);
            }

            while (deadEnds.Count > 0)
            {
                // If there's a neighboring hallway space, this one is already connected.
                for (int i = deadEnds.Count - 1; i >= 0; i--)
                {
                    hallwaySpace = deadEnds.ElementAt(i).Key;

                    if (SearchAdjacent(Constants.HALLWAY, hallwaySpace.X, hallwaySpace.Y).Count > 1)
                        deadEnds.Remove(hallwaySpace);
                }

                for (int i = deadEnds.Count - 1; i >= 0; i--)
                {
                    // Establish current space and three directions - forward and to the sides.
                    hallwaySpace = deadEnds.ElementAt(i).Key;
                    hallDirection = deadEnds.ElementAt(i).Value;
                    direction90 = GetDirection90(hallDirection);
                    direction270 = GetDirection270(hallDirection);

                    // Look for distant hallways in three directions.  If one is found, connect to it.
                    if (hallDirection != Direction.None)
                    {
                        surroundingChars = SearchAllDirections(hallwaySpace.X, hallwaySpace.Y);

                        switch (true)
                        {
                            case true when (surroundingChars[hallDirection] != null &&
                                    surroundingChars[hallDirection].MapCharacter == Constants.HALLWAY):

                                DrawHallway(hallwaySpace, surroundingChars[hallDirection], hallDirection);
                                deadEnds.Remove(hallwaySpace);

                                break;
                            case true when (surroundingChars[direction90] != null &&
                                    surroundingChars[direction90].MapCharacter == Constants.HALLWAY):

                                DrawHallway(hallwaySpace, surroundingChars[direction90], direction90);
                                deadEnds.Remove(hallwaySpace);

                                break;
                            case true when (surroundingChars[direction270] != null &&
                                    surroundingChars[direction270].MapCharacter == Constants.HALLWAY):

                                DrawHallway(hallwaySpace, surroundingChars[direction270], direction270);
                                deadEnds.Remove(hallwaySpace);

                                break;
                            default:
                                // If there's no hallway to connect to, just add another space where possible for the
                                // next iteration to pick up on.
                                adjacentChars = SearchAdjacent(Constants.EMPTY, hallwaySpace.X, hallwaySpace.Y);
                                if (adjacentChars.ContainsKey(hallDirection))
                                {
                                    newSpace = new MapSpace(Constants.HALLWAY, adjacentChars[hallDirection]);
                                    levelMap[adjacentChars[hallDirection].X, adjacentChars[hallDirection].Y] = newSpace;
                                    deadEnds.Remove(hallwaySpace);
                                    deadEnds.Add(newSpace, hallDirection);
                                }
                                else if (adjacentChars.ContainsKey(direction90))
                                {
                                    newSpace = new MapSpace(Constants.HALLWAY, adjacentChars[direction90]);
                                    levelMap[adjacentChars[direction90].X, adjacentChars[direction90].Y] = newSpace;
                                    deadEnds.Remove(hallwaySpace);
                                    deadEnds.Add(newSpace, direction90);
                                }
                                else if (adjacentChars.ContainsKey(direction270))
                                {
                                    newSpace = new MapSpace(Constants.HALLWAY, adjacentChars[direction270]);
                                    levelMap[adjacentChars[direction270].X, adjacentChars[direction270].Y] = newSpace;
                                    deadEnds.Remove(hallwaySpace);
                                    deadEnds.Add(newSpace, direction270);
                                }
                                break;
                        }
                    }
                    else
                    {
                        deadEnds.Remove(hallwaySpace);
                    }

                //    Console.Write(MapTextExample());
                //    Console.ReadKey();
                }
            }
        }

        private void DrawHallway(MapSpace start, MapSpace end, Direction hallDirection)
        {

            // Draw a hallway between specified spaces.  Break off if another hallway
            // is discovered to the side.

            switch (hallDirection)
            {
                case Direction.North:
                    for (int y = start.Y; y >= end.Y; y--)
                    {
                        levelMap[end.X, y] = new MapSpace(Constants.HALLWAY, end.X, y);
                        if (SearchAdjacent(Constants.HALLWAY, end.X, y).Count > 1)
                            break;
                    }
                    break;
                case Direction.South:
                    for (int y = start.Y; y <= end.Y; y++)
                    {
                        levelMap[end.X, y] = new MapSpace(Constants.HALLWAY, end.X, y);
                        if (SearchAdjacent(Constants.HALLWAY, end.X, y).Count > 1)
                            break;
                    }
                    break;
                case Direction.East:
                    for (int x = start.X; x <= end.X; x++)
                    {
                        levelMap[x, end.Y] = new MapSpace(Constants.HALLWAY, x, end.Y);
                        if (SearchAdjacent(Constants.HALLWAY, x, end.Y).Count > 1)
                            break;
                    }
                    break;
                case Direction.West:
                    for (int x = start.X; x >= end.X; x--)
                    {
                        levelMap[x, end.Y] = new MapSpace(Constants.HALLWAY, x, end.Y);
                        if (SearchAdjacent(Constants.HALLWAY, x, end.Y).Count > 1)
                            break;
                    }
                    break;
            }
        }

        private Direction GetDirection90(Direction startingDirection)
        {
            Direction retValue = (Math.Abs((int)startingDirection) == 1) ? (Direction)2 : (Direction)1;
            return retValue;
        }

        private Direction GetDirection270(Direction startingDirection)
        {
            // Return direction 270 degrees from original (opposite of 90 degrees) based on forward direction.
            Direction retValue = (Math.Abs((int)startingDirection) == 1) ? (Direction)2 : (Direction)1;
            retValue = (Direction)((int)retValue * -1);
            return retValue;
        }

        private Dictionary<Direction, MapSpace> SearchAdjacent(char character, int x, int y)
        {

            // Search for specific character in four directions around point for a 
            // specific character. Return list of directions and characters found.

            Dictionary<Direction, MapSpace> retValue = new Dictionary<Direction, MapSpace>();

            if (y - 1 >= 0 && levelMap[x, y - 1].MapCharacter == character)  // North
                retValue.Add(Direction.North, levelMap[x, y - 1]);

            if (x + 1 <= Constants.MAP_WD && levelMap[x + 1, y].MapCharacter == character) // East
                retValue.Add(Direction.East, levelMap[x + 1, y]);

            if (y + 1 <= Constants.MAP_HT && levelMap[x, y + 1].MapCharacter == character)  // South
                retValue.Add(Direction.South, levelMap[x, y + 1]);

            if ((x - 1) >= 0 && levelMap[x - 1, y].MapCharacter == character)  // West
                retValue.Add(Direction.West, levelMap[x - 1, y]);

            return retValue;
        }
        private Dictionary<Direction, MapSpace> SearchAdjacent(int x, int y)
        {

            // Search in four directions around point. Return list of directions and characters found.

            Dictionary<Direction, MapSpace> retValue = new Dictionary<Direction, MapSpace>();
            retValue.Add(Direction.North, levelMap[x, y - 1]);
            retValue.Add(Direction.East, levelMap[x + 1, y]);
            retValue.Add(Direction.South, levelMap[x, y + 1]);
            retValue.Add(Direction.West, levelMap[x - 1, y]);

            return retValue;
        }

        private Dictionary<Direction, MapSpace> SearchAllDirections(int currentX, int currentY)
        {
            // Look in all directions and return a Dictionary of the first non-space characters found.
            Dictionary<Direction, MapSpace> retValue = new Dictionary<Direction, MapSpace>();

            retValue.Add(Direction.North, SearchDirection(Direction.North, currentX, currentY - 1));
            retValue.Add(Direction.South, SearchDirection(Direction.South, currentX, currentY + 1));
            retValue.Add(Direction.East, SearchDirection(Direction.East, currentX + 1, currentY));
            retValue.Add(Direction.West, SearchDirection(Direction.West, currentX - 1, currentY));

            return retValue;
        }
        private MapSpace SearchDirection(Direction direction, int startX, int startY)
        {
            // Get the next non-space object found in a given direction.
            // Return null if none is found.
            int currentX = startX, currentY = startY;
            MapSpace? retValue = null;

            currentY = (currentY > Constants.MAP_HT) ? Constants.MAP_HT : currentY;
            currentY = (currentY < 0) ? 0 : currentY;
            currentX = (currentX > Constants.MAP_WD) ? Constants.MAP_WD : currentX;
            currentX = (currentX < 0) ? 0 : currentX;

            switch (direction)
            {
                case Direction.North:
                    while (levelMap[currentX, currentY].MapCharacter == Constants.EMPTY && currentY > 0)
                        currentY--;
                    break;
                case Direction.East:
                    while (levelMap[currentX, currentY].MapCharacter == Constants.EMPTY && currentX < Constants.MAP_WD)
                        currentX++;
                    break;
                case Direction.South:
                    while (levelMap[currentX, currentY].MapCharacter == Constants.EMPTY && currentY < Constants.MAP_HT)
                        currentY++;
                    break;
                case Direction.West:
                    while (levelMap[currentX, currentY].MapCharacter == Constants.EMPTY && currentX > 0)
                        currentX--;
                    break;
            }

            if (levelMap[currentX, currentY].MapCharacter != Constants.EMPTY)
                retValue = levelMap[currentX, currentY];

            return retValue;
        }
        private int GetRegionNumber(int RoomAnchorX, int RoomAnchorY)
        {
            int returnVal;

            int regionX = ((int)RoomAnchorX / Constants.REGION_WD) + 1;
            int regionY = ((int)RoomAnchorY / Constants.REGION_HT) + 1;

            returnVal = (regionX) + ((regionY - 1) * 3);

            return returnVal;
        }

        private void TrapCreator()
        {
            for (int i = 0; i < Constants.SIZE_X; i++)
            {
                for (int j = 0; j < Constants.SIZE_Y; j++)
                {
                    if (levelMap[i, j].MapCharacter == Constants.EMPTY && rand.Next(1, 101) > 93)
                    {
                        int randomStuff = rand.Next(1, 4);
                        switch (randomStuff)
                        {
                            case 1:
                                levelMap[i, j] = new Trap(Constants.TRAP,Constants.EMPTY, i, j);
                                break;
                            case 2:
                                levelMap[i, j] = new Branch(Constants.BRANCH,Constants.BRANCH, i, j);
                                break;
                            case 3:
                                levelMap[i, j] = new Bottle(Constants.BOTTLE,Constants.BOTTLE, i, j);
                                break;
                        }
                    }
                }
            }
            for (int i = 0; i < Constants.SIZE_X; i++)
            {
                for (int j = 0; j < Constants.SIZE_Y; j++)
                {
                    if ((levelMap[i, j].MapCharacter == Constants.EMPTY)
                        && rand.Next(1, 101) > 96)
                    {
                        int randomStuff = rand.Next(0, 5);
                        switch (randomStuff)
                        {
                            case 1:
                                levelMap[i, j] = new Jacket(Constants.JACKET,Constants.JACKET, i, j);
                                break;
                            case 2:
                                levelMap[i, j] = new MotorcycleHelmet(Constants.MOTOHELMET,Constants.MOTOHELMET, i, j);
                                break;
                            case 3:
                                levelMap[i, j] = new Helmet(Constants.HELMET,Constants.HELMET, i, j);
                                break;
                            case 4:
                                levelMap[i, j] = new LifeVest(Constants.VEST,Constants.VEST, i, j);
                                break;
                        }
                    }
                }
            }
        }
      
        public string MapText()
        {
            StringBuilder sbReturn = new StringBuilder();
            int i = messages.Count;
            for (int y = 0; y <= Constants.MAP_HT; y++)
            {
                sbReturn.Append("║");
                for (int x = 0; x <= Constants.MAP_WD; x++)
                {
                    sbReturn.Append(levelMap[x, y].DisplayCharacter);
                }

                if (i >= 1)
                {
                    sbReturn.Append("║  " + messages[i - 1]);
                    i--;
                }
                else
                {
                    sbReturn.Append("║");
                }
                //sbReturn.Append(i>=1? "║": "║  " + messages[i-1]);
              //  i--;
                sbReturn.Append("\n");
            }

            return sbReturn.ToString();
        }
        public string MapTextExample()
        {
            StringBuilder sbReturn = new StringBuilder();
            int i = messages.Count;
            for (int y = 0; y <= Constants.MAP_HT; y++)
            {
                sbReturn.Append("║");
                for (int x = 0; x <= Constants.MAP_WD; x++)
                {
                    sbReturn.Append(levelMap[x, y].MapCharacter);
                }

                if (i >= 1)
                {
                    sbReturn.Append("║  " + messages[i - 1]);
                    i--;
                }
                else
                {
                    sbReturn.Append("║");
                }
                //sbReturn.Append(i>=1? "║": "║  " + messages[i-1]);
                //  i--;
                sbReturn.Append("\n");
            }

            return sbReturn.ToString();
        }
    }
}
