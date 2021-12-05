using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021
{
  internal class GiantSquidSolution : IChallenge
  {
    public string Title => "--- Day 4: Giant Squid ---";

    public DateTime DateTime => new(2021, 12, 4);

    private IEnumerable<string> commands = File.ReadAllLines("4GiantSquid/input.txt");

    record Cell(int Number)
    {
      public bool Marked { get; set; } = false;
    }

    record Board()
    {
      public Board(string[] board) : this()
      {
        if (board.Length != 5)
          throw new ArgumentException("Board must have a 5x5 dimensions");

        board.ForEach((x, index) => this.Rows[index] = x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(y => new Cell(int.Parse(y))).ToArray());

        for (int i = 0; i < 5; i++)
        {
          var xx = this.Rows.Select(x => x[i]);
          this.Columns[i] = this.Rows.Select(x => x[i]).ToArray();
        }
      }

      public Cell[][] Rows { get; set; } = new Cell[5][];

      public Cell[][] Columns { get; set; } = new Cell[5][];

      public void MarkNumber(int number)
      {
        Rows.SelectMany(x => x).Where(x => x.Number == number).ForEach(x => x.Marked = true);
      }

      public bool IsWin()
      {
        return Rows.Any(row => row.All(cell => cell.Marked)) || Columns.Any(column => column.All(cell => cell.Marked));
      }

      public int Score(int lastCall)
      {
        return Rows.SelectMany(x => x).Where(x => x.Marked == false).Sum(x => x.Number) * lastCall;
      }
    }

    public string SolvePart1()
    {
      var inputNumbers = this.commands.First().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x));
      var boards = new List<Board>();
      for (int line = 2; line < this.commands.Count(); line += 6)
      {
        boards.Add(new Board(this.commands.Slice(line, 5).ToArray()));
      }

      for (int i = 0; i < inputNumbers.Count(); i++)
      {
        var num = inputNumbers.ElementAt(i);
        boards.ForEach(x => x.MarkNumber(num));
        if(boards.Any(x => x.IsWin()))
        {
          return boards.First(x => x.IsWin()).Score(num).ToString();
        }
      }

      return "0";
    }

    public string? SolvePart2()
    {
      var inputNumbers = this.commands.First().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x));
      var boards = new List<Board>();
      for (int line = 2; line < this.commands.Count(); line += 6)
      {
        boards.Add(new Board(this.commands.Slice(line, 5).ToArray()));
      }

      for (int i = 0; i < inputNumbers.Count(); i++)
      {
        var num = inputNumbers.ElementAt(i);
        var boardLastToWin = boards.First(x => !x.IsWin());
        boards.ForEach(x => x.MarkNumber(num));
        if (boards.All(x => x.IsWin()))
        {
          return boardLastToWin.Score(num).ToString();
        }
      }

      return "0";
    }
  }
}