using System;
using System.IO; 
using System.Collections.Generic; 
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace numplays
{
  // object for tracking each data point, I left in date because I thought the deserializer might get funky
  // without matching fields
  public class Entry
  {
    public string date_played {get; set;}
    public string[] games_played {get; set;}
  }

  // object for each combination of games and a counter for said combo
  // could be made generic to combinations of size greater than 2 with an array of strings instead of 2 fields
  public class Combo
  {
    public string game1 {get; set;}
    public string game2 {get; set;}

    public int count {get; set;}

    public void increment()
    {
        count++;
    }
  }

  // different helper functions
  class Helpers
  {
    // hash set to identify each unique game instead of eye balling and hard coding it (robust vs fragile in my mind) 
    public static HashSet<String> gameSet {get; set;}

    // list of each data point from plays.json
    public static List<Entry> entries {get; set;}
    
    // list of each different combination each with a counter
    // if I wanted to be slick it would be a fixed size array with another helper for nCr and factorial (using a loop not recursion)
    // but that would add a good amount of extra code I didn't really need
    public static List<Combo> tally {get; set;}


    // reads the json and builds the set of games and list of entries, had to be async because FileStream was being fussy 
    public static async Task initializeEntriesAndSet()
    {
      entries = new List<Entry>();
      gameSet = new HashSet<string>();

      using (FileStream fs = new FileStream("plays.json", FileMode.Open))
      {
        List<Entry> data = await JsonSerializer.DeserializeAsync<List<Entry>>(fs);

        foreach (Entry item in data)
        {
          entries.Add(item);

          foreach(String game in item.games_played)
            {
              if(!gameSet.Contains(game))
                gameSet.Add(game);
            }
        }
      }
    }

    // builds the tally board of game combinations
    public static void buildTally()
    {
      tally = new List<Combo>();

      string[] games = new List<string>(gameSet).ToArray();

      // looping 'for each game, add a combo for every other game' as # of combos = nCr (in this case 11 choose 2)
      for (int i = 0; i < games.Length; i++)
      {
        for (int j = i+1; j < games.Length; j++)
        {
          tally.Add(new Combo{game1=games[i], game2=games[j], count=0});
        }
      }
      
    }
    
    // counts the actual combinations
    public static void countCombos()
    {
      // I love lambdas...
      // checks if an array of strings contains a string (with syntactic sugar)
      Func<string, string[], bool> hasGame = (x, vals) => Array.Exists(vals, val => val == x);
      
      // uses previous lambda twice to check for a match of 2 games
      Func<string, string, string[], bool> isCombo = (x, y, vals) => hasGame(x, vals) && hasGame(y, vals);

      // for each combo, check each entry...
      foreach (Combo c in tally)
      {
        foreach(Entry e in entries)
        {
          // ...if it is a combo, increment counter
          if (isCombo(c.game1, c.game2, e.games_played))
            c.increment();
        }
      }
    }

    // outputs each combination (even if the count is 0)
    public static void showCount()
    {
      foreach (Combo c in tally)
      {
        Console.WriteLine("{0} and {1}: {2}", c.game1, c.game2, c.count);
      }
    }
  }





}