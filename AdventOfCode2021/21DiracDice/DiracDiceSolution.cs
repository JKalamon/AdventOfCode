namespace AdventOfCode2021
{
  internal class DiracDiceSolution : IChallenge
  {
    public string Title => "--- Day 23: Amphipod ---";

    public DateTime DateTime => new(2021, 12, 23);

    public record GameState(int WinPoints, int[] Position, int[] Score)
    {
      public bool HasWinner => this.Score.Any(x => x >= WinPoints);

      public int PlayerWhoWonIndex => this.Score.ToList().FindIndex(x => x >= WinPoints);

      public bool P2Won => this.Score[1] >= WinPoints;

      public void Move(int player, int places)
      {
        this.Position[player] = (this.Position[player] + places) % 10;
        if (this.Position[player] == 0)
          this.Position[player] = 10;
        this.Score[player] += this.Position[player];
      }

      public GameState Duplicate()
        => new GameState(this.WinPoints, this.Position.ToArray(), this.Score.ToArray());
    }

    public object SolvePart1()
    {
      var game = new GameState(1000, new int[] { 6, 1 }, new int[] { 0, 0 });
      var dice = 0;
      var player = 0;
      var rollCount = 0;
      while (!game.HasWinner)
      {
        var move = 0;
        for (int i = 0; i < 3; i++)
        {
          dice = (dice + 1) % 101;
          if (dice == 0)
            dice++;

          move += dice;
          rollCount++;
        }

        game.Move(player, move);
        player = (player + 1) % 2;
      }

      return game.Score.Min() * rollCount;
    }

    public object? SolvePart2()
    {
      var game = new GameState(21, new int[] { 6, 1 }, new int[] { 0, 0 });
      var aa = CountWinUniverses(game);
      return aa.Max();
    }

    record PossibleOutcome(int Move, ulong UniversesCount);

    private ulong[] CountWinUniverses(GameState game, int player = 0)
    {
      var dicePossibleOutcomes = new PossibleOutcome[] { new(3, 1), new(4, 3), new(5, 6), new(6, 7), new(7, 6), new(8, 3), new(9, 1) };
      var returnInt = new ulong[] { 0, 0 };
      foreach (var possibleOutcome in dicePossibleOutcomes)
      {
        var gameCopy = game.Duplicate();
        gameCopy.Move(player, possibleOutcome.Move);
        if (gameCopy.HasWinner)
        {
          returnInt[gameCopy.PlayerWhoWonIndex] += possibleOutcome.UniversesCount;
          continue;
        }

        var otherCount = this.CountWinUniverses(gameCopy, (player + 1) % 2);
        for (int i = 0; i < otherCount.Length; i++)
        {
          otherCount[i] *= possibleOutcome.UniversesCount;
          returnInt[i] += otherCount[i];          
        }
      }

      return returnInt;
    }
  }
}

