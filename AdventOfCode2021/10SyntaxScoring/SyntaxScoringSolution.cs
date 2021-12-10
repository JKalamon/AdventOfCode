using MoreLinq;

namespace AdventOfCode2021
{
  internal class SyntaxScoringSolution : IChallenge
  {
    public string Title => "--- Day 10: Syntax Scoring ---";

    public DateTime DateTime => new(2021, 12, 10);

    private IEnumerable<string> inputFile = File.ReadAllLines("10SyntaxScoring/input.txt");

    public Dictionary<char, char> chunks = new Dictionary<char, char>()
    {
      ['('] = ')',
      ['['] = ']',
      ['{'] = '}',
      ['<'] = '>'
    };

    public int GetScore(char c)
      => c switch
      {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => 0
      };

    public int GetScorePart2(char c)
      => c switch
      {
        ')' => 1,
        ']' => 2,
        '}' => 3,
        '>' => 4,
        _ => 0
      };


    public object SolvePart1()
    {
      var score = 0;
      foreach (var input in inputFile)
      {
        var expectedStack = new Stack<char>();
        input.ToCharArray().ForEach(c =>
        {
          if (chunks.ContainsKey(c))
          {
            expectedStack.Push(chunks[c]);
          }
          else
          {
            if (expectedStack.Pop() != c)
            {
              score += GetScore(c);
            }
          }
        });
      }

      return score;
    }

    public object? SolvePart2()
    {
      var scores = new List<ulong>();
      foreach (var input in inputFile)
      {
        var expectedStack = new Stack<char>();
        var corruptedLine = false;
        input.ToCharArray().ForEach(c =>
        {
          if (chunks.ContainsKey(c))
          {
            expectedStack.Push(chunks[c]);
          }
          else
          {
            if (expectedStack.Pop() != c)
            {
              corruptedLine = true;              
            }
          }
        });

        if (!corruptedLine)
        {
          ulong lineScore = 0;
          while (expectedStack.TryPop(out var expected))
          {
            lineScore = lineScore * 5 + (ulong)this.GetScorePart2(expected);
          }

          scores.Add(lineScore);
        }
      }

      scores = scores.OrderBy(x => x).ToList();
      return scores[(int)Math.Floor((decimal)scores.Count / 2)];
    }
  }
}

