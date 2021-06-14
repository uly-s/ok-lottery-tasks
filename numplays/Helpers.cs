using System;
using System.IO; 
using System.Collections.Generic; 
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace numplays
{
  public class Entry
  {
    public string date_played {get; set;}
    public string[] games_played {get; set;}
  }

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

  class Helpers
  {
    public static HashSet<String> gameSet {get; set;}
    public static List<Entry> entries {get; set;}
    public static List<Combo> tally {get; set;}

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

    public static void buildTally()
    {
      tally = new List<Combo>();

      string[] games = new List<string>(gameSet).ToArray();

      for (int i = 0; i < games.Length; i++)
      {
        for (int j = i+1; j < games.Length; j++)
        {
          tally.Add(new Combo{game1=games[i], game2=games[j], count=0});
        }
      }
      
    }
    
    public static void countCombos()
    {

      Func<string, string[], bool> hasGame = (x, vals) => Array.Exists(vals, val => val == x);
      
      Func<string, string, string[], bool> isCombo = (x, y, vals) => hasGame(x, vals) && hasGame(y, vals);

      foreach (Combo c in tally)
      {
        foreach(Entry e in entries)
        {
          if (isCombo(c.game1, c.game2, e.games_played))
            c.increment();
        }
      }
    }

    public static void showCount()
    {
      foreach (Combo c in tally)
      {
        Console.WriteLine("{0} and {1}: {2}", c.game1, c.game2, c.count);
      }
    }
  }





}