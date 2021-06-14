using System;
using System.IO; 
using System.Collections.Generic; 
using System.Text.Json;
using System.Threading.Tasks;

namespace numplays
{
    class Program
    {
        static async Task Main(string[] args)
        {
          // allocate memory and reads the json, builds a set of the games and a list of plays
          await Helpers.initializeEntriesAndSet();
          
          // uses the set of games to make a list of all the different combos with a counter for each
          Helpers.buildTally();
          
          // counts the actual combos, complexity is number of plays * number of combos
          Helpers.countCombos();

          // displays each combo and its count
          Helpers.showCount();
        }

    }
}
