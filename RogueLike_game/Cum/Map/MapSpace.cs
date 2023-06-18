using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cum
{
       public class MapSpace : Item
            {
             //   public char DisplayInvisible { get; set; }
                public char MapCharacter { get; set; }
                public bool SearchRequired { get; set; }
                public char DisplayCharacter { get; set; }
                public bool isVisible { get; set; }
                public int X { get; set; }
                public int Y { get; set; }
    
                public MapSpace()
                {
                    // Create blank space for map
                    this.MapCharacter = ' ';
                    this.DisplayCharacter = ' ';
                    this.SearchRequired = false;
                    X = 0;
                    Y = 0;
                }
    
                public MapSpace(char mapChar, MapSpace oldSpace)
                {
                    this.MapCharacter = mapChar;
                 //   this.DisplayCharacter = mapChar;
                    this.SearchRequired = oldSpace.SearchRequired;
                    this.DisplayCharacter = ' ';
                    this.X = oldSpace.X;
                    this.Y = oldSpace.Y;
                }
    
                public MapSpace(char mapChar, char displayChar, int X, int Y)
                {
                    this.MapCharacter = mapChar;
                    this.DisplayCharacter = displayChar;
                    this.SearchRequired = false;
                    this.isVisible = true;
                    this.X = X;
                    this.Y = Y;
                }
    
                public MapSpace(char mapChar, int X, int Y)
                {
                    this.MapCharacter = mapChar;
                    this.DisplayCharacter = ' ';
                    this.SearchRequired = false;
                    this.X = X;
                    this.Y = Y;
                }
    
                public MapSpace(char mapChar, Boolean hidden, Boolean search, int X, int Y)
                {
                    this.MapCharacter = mapChar;
                    this.SearchRequired = search;
                    this.DisplayCharacter = ' ';
                    this.X = X;
                    this.Y = Y;
                }

              /*  public override string InventoryToString()
                {
                    return null;
                }*/
            }
}
