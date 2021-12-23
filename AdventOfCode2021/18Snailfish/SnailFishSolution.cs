using System.Text;

namespace AdventOfCode2021
{
  internal class SnailFishSolution : IChallenge
  {
    public string Title => "--- Day 18: Snailfish ---";

    public DateTime DateTime => new(2021, 12, 18);

    public class Number
    {
      public static Number Add(Number left, Number right)
      {
        var output = new Number()
        {
          LeftNumber = left,
          RightNumber = right
        };

        left.Parent = output;
        right.Parent = output;
        return output;
      }
      public static Number FromString(string input)
      {
        var output = new Number();
        var tmpInput = input.Substring(1, input.Length - 2);
        if (tmpInput[0] == '[')
        {
          var buildString = "";
          var parathlessCount = 0;
          for (int i = 0; i < tmpInput.Length; i++)
          {
            if (tmpInput[i] == ']')
              parathlessCount--;

            if (tmpInput[i] == '[')
              parathlessCount++;

            buildString += tmpInput[i];

            if (parathlessCount == 0)
            {
              output.LeftNumber = Number.FromString(buildString);
              output.LeftNumber.Parent = output;
              tmpInput = tmpInput.Substring(i + 2);
              break;
            }
          }
        }
        else
        {
          output.LeftInt = int.Parse(tmpInput.Substring(0, tmpInput.IndexOf(",")));
          tmpInput = tmpInput.Substring(tmpInput.IndexOf(",") + 1);
        }

        if (tmpInput[0] == '[')
        {
          var buildString = "";
          var parathlessCount = 0;
          for (int i = 0; i < tmpInput.Length; i++)
          {
            if (tmpInput[i] == ']')
              parathlessCount--;

            if (tmpInput[i] == '[')
              parathlessCount++;

            buildString += tmpInput[i];

            if (parathlessCount == 0)
            {
              output.RightNumber = Number.FromString(buildString);
              output.RightNumber.Parent = output;
              break;
            }
          }
        }
        else
        {
          output.RightInt = int.Parse(tmpInput.Substring(0, 1));
        }

        return output;
      }

      public Number Reduce()
      {
        //Console.WriteLine(this.ToString());
        while (this.CanBeReduced())
        {
          if (this.CanExplode())
          {
            var nodeToExplode = NodesWhichCanExplode().First();
            var leftInt = nodeToExplode.LeftInt;
            var rightInt = nodeToExplode.RightInt;

            if (nodeToExplode.Parent!.LeftNumber == nodeToExplode)
            {
              var previousNode = nodeToExplode;
              var searchLeftParent = nodeToExplode.Parent;
              while (searchLeftParent != null && searchLeftParent.LeftNumber == previousNode)
              {
                previousNode = searchLeftParent;
                searchLeftParent = searchLeftParent.Parent;
              }

              if (searchLeftParent != null && searchLeftParent.LeftNumber == null)
              {
                searchLeftParent.LeftInt += leftInt;
                searchLeftParent = null;
              }

              if (searchLeftParent != null)
              {
                searchLeftParent = searchLeftParent.LeftNumber;
              }

              nodeToExplode.Parent.LeftNumber = null;
              nodeToExplode.Parent.LeftInt = 0;
              
              if (nodeToExplode.Parent.RightNumber != null)
              {
                var searchRight = nodeToExplode.Parent.RightNumber;
                while (searchRight.LeftNumber != null)
                {
                  searchRight = searchRight.LeftNumber;
                }

                searchRight.LeftInt += rightInt;
              }
              else
              {
                nodeToExplode.Parent.RightInt += rightInt;
              }


              if (searchLeftParent != null)
              {
                var searchRight = searchLeftParent;
                while (searchRight.RightNumber != null)
                {
                  searchRight = searchRight.RightNumber;
                }

                searchRight.RightInt += leftInt;
              }
            }

            if (nodeToExplode.Parent!.RightNumber == nodeToExplode)
            {
              var previousNode = nodeToExplode;
              var searchRightParent = nodeToExplode.Parent;
              while (searchRightParent != null && searchRightParent.RightNumber == previousNode)
              {
                previousNode = searchRightParent;
                searchRightParent = searchRightParent.Parent;
              }

              if (searchRightParent != null && searchRightParent.RightNumber == null)
              {
                searchRightParent.RightInt += rightInt;
                searchRightParent = null;
              }

              if (searchRightParent != null)
              {
                searchRightParent = searchRightParent.RightNumber;
              }

              nodeToExplode.Parent.RightNumber = null;
              nodeToExplode.Parent.RightInt = 0;

              if (nodeToExplode.Parent.LeftNumber != null)
              {
                var searchLeft = nodeToExplode.Parent.LeftNumber;
                while (searchLeft.RightNumber != null)
                {
                  searchLeft = searchLeft.RightNumber;
                }

                searchLeft.RightInt += leftInt;
              }
              else
              {
                nodeToExplode.Parent.LeftInt += leftInt;
              }
              

              if (searchRightParent != null)
              {
                var searchLeft = searchRightParent;
                while (searchLeft.LeftNumber != null)
                {
                  searchLeft = searchLeft.LeftNumber;
                }

                searchLeft.LeftInt += rightInt;
              }
            }
          }
          else
          {
            var toBeSplited = AllChildrenSearch(this, x => x.LeftInt >= 10 || (x.RightInt >= 10 && x.LeftNumber?.CanSplit() != true)).First();

            if (toBeSplited.LeftInt >= 10)
            {
              toBeSplited.LeftNumber = new Number()
              {
                LeftInt = (int)Math.Floor((decimal)toBeSplited.LeftInt / 2),
                RightInt = (int)Math.Ceiling((decimal)toBeSplited.LeftInt / 2),
                Parent = toBeSplited,
              };

              toBeSplited.LeftInt = null;
            } else if (toBeSplited.RightInt >= 10)
            {
              toBeSplited.RightNumber = new Number()
              {
                LeftInt = (int)Math.Floor((decimal)toBeSplited.RightInt / 2),
                RightInt = (int)Math.Ceiling((decimal)toBeSplited.RightInt / 2),
                Parent = toBeSplited,
              };

              toBeSplited.RightInt = null;
            };
          }

          //Console.WriteLine(this.ToString());
        }

        return this;
      }

      public bool CanBeReduced()
        => this.CanExplode() || this.CanSplit();

      public bool CanExplode()
        => NodesWhichCanExplode().Any();

      public IEnumerable<Number> NodesWhichCanExplode()
        => AllChildrenSearch(this, x => x.Depth == 4);


      public bool CanSplit()
      {
        if (this.LeftInt != null && this.LeftInt >= 10)
          return true;

        if (this.RightInt != null && this.RightInt >= 10)
          return true;

        return this.LeftNumber?.CanSplit() == true || this.RightNumber?.CanSplit() == true;
      }

      public override string ToString()
      {
        var leftString = this.LeftInt?.ToString() ?? this.LeftNumber?.ToString();
        var rightString = this.RightInt?.ToString() ?? this.RightNumber?.ToString();
        return $"[{leftString},{rightString}]";
      }

      public int? LeftInt { get; set; }

      public int? RightInt { get; set; }

      public Number? LeftNumber { get; set; }

      public Number? RightNumber { get; set; }

      public Number? Parent { get; set; }

      public IEnumerable<Number> DirectChildren => (new Number?[2] { this.LeftNumber, this.RightNumber }).OfType<Number>();

      public int Depth
      {
        get
        {
          var count = 0;
          var tmp = this.Parent;
          while (tmp != null)
          {
            tmp = tmp.Parent;
            count++;
          }

          return count;
        }
      }


      public int Magnitude
      {
        get
        {
          var left = this.LeftInt ?? this.LeftNumber!.Magnitude;
          var right = this.RightInt ?? this.RightNumber!.Magnitude;
          return 3 * left + 2 * right;
        }
      }

      public static IEnumerable<Number> AllChildrenSearch(Number root, Func<Number, bool> predicate)
      {
        var stack = new Stack<Number>();
        stack.Push(root);
        while (stack.Count > 0)
        {
          var current = stack.Pop();
          if (predicate(current))
            yield return current;

          foreach (var node in current.DirectChildren.Reverse())
            stack.Push(node);
        }
      }
    }

    private IEnumerable<Number> inputFile = File.ReadAllLines("18Snailfish/input.txt").Select(x => Number.FromString(x));

    public object SolvePart1()
    {
      var sum = inputFile.First();
      sum.Reduce();
      foreach (var item in inputFile.Skip(1))
      {        
        sum = Number.Add(sum, item);
        sum.Reduce();
      }
      
      return sum.Magnitude;
    }

    public object? SolvePart2()
    {
      var max = 0;
      foreach (var item1 in inputFile.ToList())
      {
        foreach (var item2 in inputFile.ToList().Where(x => x != item1))
        {
          var tmp = Number.Add(Number.FromString(item1.ToString()), item2).Reduce().Magnitude;
          
          if(tmp > max)
          {
            if(tmp == 3997)
            {
              Console.WriteLine(item1.ToString());
              Console.WriteLine(item2.ToString());
            }

            max = tmp;
          }
        }
      }

      return max;
    }
  }
}

