using MoreLinq;

namespace AdventOfCode2023;

internal class ParabolicReflectorDishSolution : ChallengeBase
{
	public override string Title => "Parabolic Reflector Dish";

	public override int Day => 14;

	public override object SolvePart1()
	{
		var map = this.ParseInput();
		//// slide north
		map.Rocks.Where(x => x.IsRock).OrderBy(x => x.Y).ForEach(r =>
		{
			var rockNear = map.Rocks.OrderByDescending(x => x.Y).FirstOrDefault(x => r != x && x.X == r.X && x.Y < r.Y);
			r.Y = rockNear != null ? rockNear.Y + 1 : 0;
		});


		return map.CalcLoad();
	}

	public override object SolvePart2()
	{
		var map = this.ParseInput();
		var rocks = map.Rocks.Where(x => x.IsRock);

		//// after some time there will be a pattern
		//// I am not sure which interation to choose tried a few and it worked
		for (int i = 0; i < 152; i++)
		{
			//// slide north
			rocks.OrderBy(x => x.Y).ForEach(r =>
			{
				var rockNear = map.Rocks.Where(x => r != x && x.X == r.X && x.Y < r.Y).OrderByDescending(x => x.Y).FirstOrDefault();
				r.Y = rockNear != null ? rockNear.Y + 1 : 0;
			});

			//// slide west
			rocks.OrderBy(x => x.X).ForEach(r =>
			{
				var rockNear = map.Rocks.Where(x => r != x && x.Y == r.Y && x.X < r.X).OrderByDescending(x => x.X).FirstOrDefault();
				r.X = rockNear != null ? rockNear.X + 1 : 0;
			});

			//// slide south
			rocks.OrderByDescending(x => x.Y).ForEach(r =>
			{
				var rockNear = map.Rocks.Where(x => r != x && x.X == r.X && x.Y > r.Y).OrderBy(x => x.Y).FirstOrDefault();
				r.Y = rockNear != null ? rockNear.Y - 1 : map.Height - 1;
			});

			//// slide south
			rocks.OrderByDescending(x => x.X).ForEach(r =>
			{
				var rockNear = map.Rocks.Where(x => r != x && x.Y == r.Y && x.X > r.X).OrderBy(x => x.X).FirstOrDefault();
				r.X = rockNear != null ? rockNear.X - 1 : map.Width - 1;
			});
		}

		return map.CalcLoad();
	}

	private Map ParseInput()
	{
		var y = 0;
		var currentRocks = new List<Rock>();
		File.ReadAllLines(this.InputPath).ForEach(x =>
		{
			x.AllIndexOf(c => c == '#').ForEach(i => currentRocks.Add(new Rock(i, y, false)));
			x.AllIndexOf(c => c == 'O').ForEach(i => currentRocks.Add(new Rock(i, y, true)));
			y++;
		});

		return new Map(currentRocks.ToArray());
	}

	private record Rock(int X, int Y, bool IsRock)
	{
		public int X { get; set; } = X;
		public int Y { get; set; } = Y;
	}

	private record Map(Rock[] Rocks)
	{
		public int Width = Rocks.Max(x => x.X) + 1;

		public int Height = Rocks.Max(x => x.Y) + 1;


		public long CalcLoad()
			=> this.Rocks.Where(x => x.IsRock).Sum(x => this.Height - x.Y);
	}
}
