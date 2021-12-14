using MoreLinq;
using System.Text;

namespace AdventOfCode2021
{
  internal class ExtendedPolymerizationSolution : IChallenge
  {
    public string Title => "--- Day 14: Extended Polymerization ---";

    public DateTime DateTime => new(2021, 12, 14);

    private IEnumerable<string> inputFile = File.ReadAllLines("14ExtendedPolymerization/input.txt");

    record Rule(string Template, string Insert);
    
    private string template => inputFile.First();

    private IEnumerable<Rule> rules => inputFile.Where(x => x.Contains("->")).Select(x => new Rule(x.Split(" -> ")[0], x.Split(" -> ")[1]));

    public object SolvePart1()
    {
      var output = template;
      for (int i = 0; i < 10; i++)
      {
        var temp = string.Empty;
        for (int j = 0; j < output.Length - 1; j++)
        {
          var xx = output.Substring(j, 2);
          temp += output[j] + rules.First(x => x.Template == xx).Insert;
        }

        output = temp + output[output.Length - 1];
      }

      var group = output.GroupBy(x => x);
      return group.OrderByDescending(x => x.Count()).First().Count() - group.OrderBy(x => x.Count()).First().Count();
    }

    public object? SolvePart2()
    {
      var output = new StringBuilder(template);
      Dictionary<string, ulong> pairs = new Dictionary<string, ulong>();

      for (int j = 0; j < template.Length - 1; j++)
      {
        var key = template.Substring(j, 2);        
        if (pairs.ContainsKey(key))
        {
          pairs[key]++;
        }
        else
        {
          pairs.Add(key, 1);
        }
      }

      for (int i = 0; i < 40; i++)
      {
        Dictionary<string, ulong> tmpPairs = new Dictionary<string, ulong>();
        pairs.ForEach(x =>
        {
          var letter = rules.First(y => y.Template == x.Key).Insert;
          var pairsToAdd = x.Key[0] + letter;
          var pairsToAdd2 = letter + x.Key[1];

          if (tmpPairs.ContainsKey(pairsToAdd))
          {
            tmpPairs[pairsToAdd] += x.Value;
          }
          else
          {
            tmpPairs.Add(pairsToAdd, x.Value);
          }

          if (tmpPairs.ContainsKey(pairsToAdd2))
          {
            tmpPairs[pairsToAdd2] += x.Value;
          }
          else
          {
            tmpPairs.Add(pairsToAdd2, x.Value);
          }
        });

        pairs = tmpPairs;
      }

      Dictionary<char, ulong> charCount = new Dictionary<char, ulong>();
      pairs.ForEach(pair =>
      {
        if (charCount.ContainsKey(pair.Key[0]))
        {
          charCount[pair.Key[0]] += pair.Value;
        }
        else
        {
          charCount.Add(pair.Key[0], pair.Value);
        }
      });

      charCount[template[^1]]++;
      return charCount.Max(x => x.Value) - charCount.Min(x => x.Value);      
    }
  }
}

