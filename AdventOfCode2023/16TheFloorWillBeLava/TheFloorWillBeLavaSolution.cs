using MoreLinq;

namespace AdventOfCode2023;

internal class TheFloorWillBeLavaSolution : ChallengeBase
{
	public override string Title => "The Floor Will Be Lava";

	public override int Day => 16;

	Dictionary<PointBeam, Point[]> Cache = new();

	private Map map = new([new Point(0, 0, ContraptionItem.EmptySpace)]);

	public override object SolvePart1()
	{
		this.map = ParseInput();
		var startPoint = new PointBeam(this.map.GetPoint(0, 0), BeamDirection.Right);
		var aaa = ProcessBeam(startPoint);
		return aaa.Length;
	}

	public override object SolvePart2()
	{
		this.map = ParseInput();
		List<PointBeam> allPoints = Enumerable.Range(0, this.map.Width).Select(x => new PointBeam(this.map.GetPoint(x, 0), BeamDirection.Down)).ToList();
		allPoints.AddRange(Enumerable.Range(0, this.map.Width).Select(x => new PointBeam(this.map.GetPoint(x, this.map.Height - 1), BeamDirection.Up)));
		allPoints.AddRange(Enumerable.Range(0, this.map.Height).Select(y => new PointBeam(this.map.GetPoint(0, y), BeamDirection.Right)));
		allPoints.AddRange(Enumerable.Range(0, this.map.Height).Select(y => new PointBeam(this.map.GetPoint(this.map.Width - 1, y), BeamDirection.Down)));

		var max = 0;
		var iterator = 0;
		allPoints.ForEach(x =>
		{
			var aaa = ProcessBeam(x);
			if (aaa.Length > max)
			{
				max = aaa.Length;
			}

			iterator++;
			Console.WriteLine($"Progress: {iterator} / {allPoints.Count}, Max: {max}");
		});
		
		return max;
	}

	private void Print(Point[] markedPoints)
	{
		Console.SetCursorPosition(0, 0);
		for (int y = 0; y < this.map.Height; y++)
		{
			for (int x = 0; x < this.map.Width; x++)
			{
				Console.Write(markedPoints.Any(p => p.X == x && p.Y == y) ? "#" : ".");
			}

			Console.WriteLine();
		}
	}

	private Map ParseInput()
	{
		var points = new List<Point>();
		File.ReadAllLines(this.InputPath).ForEach((line, y) =>
		{
			line.ForEach((c, x) =>
			{
				points.Add(new Point(x, y, c switch
				{
					'/' => ContraptionItem.MirrorLeftTop,
					'\\' => ContraptionItem.MirrorLeftBottom,
					'-' => ContraptionItem.HorizontalSplitter,
					'|' => ContraptionItem.VerticalSplitter,
					_ => ContraptionItem.EmptySpace,
				}));
			});
		});

		return new Map(points.ToArray());
	}

	private Point[] ProcessBeam(PointBeam startBeam)
	{
		List<PointBeam> currentBeams = [startBeam];
		List<PointBeam> oldBeams = [];
		List<Point> visitedPoints = [];
		while (currentBeams.Any())
		{
			currentBeams.ToArray().ForEach(x =>
			{
				currentBeams.Remove(x);

				if (oldBeams.Contains(x))
					return;

				oldBeams.Add(x);

				if (this.map.IsOutsideOfMap(x.Point))
					return;

				if (!visitedPoints.Contains(x.Point))
					visitedPoints.Add(x.Point);


				currentBeams.AddRange(x.GetNextPoints().Select(n => new PointBeam(map.GetPoint(n.X, n.Y), n.Direction)));
			});
		};
		return visitedPoints.ToArray();
	}

	private record PointBeamSimple(int X, int Y, BeamDirection Direction);

	private record PointBeam(Point Point, BeamDirection Direction)
	{
		public PointBeamSimple[] GetNextPoints()
		{
			BeamDirection[] outputDirections = (this.Point.Item, this.Direction) switch
			{
				(ContraptionItem.EmptySpace, _) => [this.Direction],
				(ContraptionItem.MirrorLeftTop, BeamDirection.Right) => [BeamDirection.Up], // /
				(ContraptionItem.MirrorLeftTop, BeamDirection.Down) => [BeamDirection.Left],
				(ContraptionItem.MirrorLeftTop, BeamDirection.Left) => [BeamDirection.Down],
				(ContraptionItem.MirrorLeftTop, BeamDirection.Up) => [BeamDirection.Right],
				(ContraptionItem.MirrorLeftBottom, BeamDirection.Left) => [BeamDirection.Up], // \
				(ContraptionItem.MirrorLeftBottom, BeamDirection.Up) => [BeamDirection.Left],
				(ContraptionItem.MirrorLeftBottom, BeamDirection.Down) => [BeamDirection.Right],
				(ContraptionItem.MirrorLeftBottom, BeamDirection.Right) => [BeamDirection.Down],
				(ContraptionItem.HorizontalSplitter, BeamDirection.Up) => [BeamDirection.Left, BeamDirection.Right],
				(ContraptionItem.HorizontalSplitter, BeamDirection.Down) => [BeamDirection.Left, BeamDirection.Right],
				(ContraptionItem.HorizontalSplitter, _) => [this.Direction],
				(ContraptionItem.VerticalSplitter, BeamDirection.Left) => [BeamDirection.Up, BeamDirection.Down],
				(ContraptionItem.VerticalSplitter, BeamDirection.Right) => [BeamDirection.Up, BeamDirection.Down],
				(ContraptionItem.VerticalSplitter, _) => [this.Direction],
				_ => throw new NotImplementedException(),
			};


			return outputDirections.Select(x =>
			{
				return x switch
				{
					BeamDirection.Left => new PointBeamSimple(this.Point.X - 1, this.Point.Y, x),
					BeamDirection.Right => new(this.Point.X + 1, this.Point.Y, x),
					BeamDirection.Up => new(this.Point.X, this.Point.Y - 1, x),
					BeamDirection.Down => new(this.Point.X, this.Point.Y + 1, x),
					_ => throw new NotImplementedException(),
				};
			}).ToArray();
		}
	}


	private record Map(Point[] Points)
	{
		public Point GetPoint(int x, int y) => Points.FirstOrDefault(p => p.X == x && p.Y == y) ?? new Point(x, y, ContraptionItem.EmptySpace);

		public int Width = Points.Max(x => x.X) + 1;

		public int Height = Points.Max(x => x.Y) + 1;

		public bool IsOutsideOfMap(Point p) => p.X < 0 || p.Y < 0 || p.X >= Width || p.Y >= Height;
	}

	private record Point(int X, int Y, ContraptionItem Item);

	enum ContraptionItem
	{
		EmptySpace, // .
		MirrorLeftTop, // /
		MirrorLeftBottom, // \
		HorizontalSplitter, // -
		VerticalSplitter, // |
	}

	enum BeamDirection
	{
		Left,
		Right,
		Up,
		Down,
	}
}