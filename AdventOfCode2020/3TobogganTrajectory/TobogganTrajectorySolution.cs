using MoreLinq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
  internal static class TobogganTrajectorySolution
  {
    public static void Solve()
    {
      var inputList = File.ReadAllLines("3TobogganTrajectory/input.txt");
      Console.WriteLine($"Solution First Part {NumberOfTrees(inputList, 3, 1)}");

      var count1 = NumberOfTrees(inputList, 1, 1);
      var count2 = NumberOfTrees(inputList, 3, 1);
      var count3 = NumberOfTrees(inputList, 5, 1);
      var count4 = NumberOfTrees(inputList, 7, 1);
      var count5 = NumberOfTrees(inputList, 1, 2);
      Console.WriteLine($"Solution First Part {count1 * count2 * count3 * count4 * count5}");
    }

    private static int NumberOfTrees(string[] inputList, int moveX, int moveY)
    {
      var width = inputList[0].Length;
      var height = inputList.Length;
      var positionX = 0;
      var positionY = 0;      
      var count = 0;

      while (positionY < height)
      {
        if (inputList[positionY][positionX % width] == '#')
          count++;

        positionX += moveX;
        positionY += moveY;
      }

      return count;
    }
  }
}
