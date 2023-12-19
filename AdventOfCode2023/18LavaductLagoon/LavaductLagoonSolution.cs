using MoreLinq;
using System.Text;

namespace AdventOfCode2023;

internal class LavaductLagoonSolution : ChallengeBase
{
	public override string Title => "Lavaduct Lagoon";

	public override int Day => 18;

	public override object SolvePart1()
	{
		var maze = Dig(this.ParseInput());
		PrintToFile(maze);
		return SpanFloodFill(maze, new Position(1, -1)) + maze.Sum(x => x.Length) - maze.Count();
	}

	//public override object SolvePart2()
	//{
	//	//var maze = Dig(this.ParseInput2());
	//	//PrintToFile(maze);
	//	return SpanFloodFill(maze, new Position(2, -1)) + maze.Sum(x => x.Length) - maze.Count();
	//}

	private IEnumerable<Line> Dig(IEnumerable<DigInstruction> instructions)
	{
		var position = new Position(0, 0);
		foreach (var digInstruction in instructions)
		{
			var (direction, howFar) = digInstruction;

			var newPosition = direction switch
			{
				Direction.Up => new Position(position.X, position.Y + 1 * howFar),
				Direction.Down => new Position(position.X, position.Y - 1 * howFar),
				Direction.Left => new Position(position.X - 1 * howFar, position.Y),
				Direction.Right => new Position(position.X + 1 * howFar, position.Y),
				_ => throw new Exception("Invalid direction")
			};

			yield return new Line(position, newPosition);
			position = newPosition;
		}
	}

	private long SpanFloodFill(IEnumerable<Line> loop, Position start)
	{
		var fillLines = new List<Line>();
		var queue = new Queue<Position>();
		queue.Enqueue(start);

		while (queue.Any())
		{
			var current = queue.Dequeue();
			if (loop.Any(x => x.IsPointOnLine(current)))
				continue;
			var lx = FindLx(loop, current);
			var rx = FindRx(loop, current);

			var filledLine = new Line(new Position(lx, current.Y), new Position(rx, current.Y));
			if (fillLines.Contains(filledLine))
				continue;

			fillLines.Add(filledLine);

			////scan
			loop.Where(x => x.DoLinesIntersect(Line.MaxHorizontal(current.Y + 1))).ForEach(x =>
			{
				if (x.MinX - 1 >= lx && x.MinX - 1 <= rx)
					queue.Enqueue(new Position(x.MinX - 1, current.Y + 1));
				if (x.MaxX + 1 <= rx && x.MaxX + 1 >= lx)
					queue.Enqueue(new Position(x.MaxX + 1, current.Y + 1));
			});

			if (!queue.Any(x => x.Y == current.Y + 1) && !loop.Any(x => x.IsPointOnLine(new Position(lx, current.Y + 1))))
			{
				queue.Enqueue(new Position(lx, current.Y + 1));
			}

			loop.Where(x => x.DoLinesIntersect(Line.MaxHorizontal(current.Y - 1))).ForEach(x =>
			{
				if (x.MinX - 1 >= lx && x.MinX - 1 <= rx)
					queue.Enqueue(new Position(x.MinX - 1, current.Y - 1));
				if (x.MaxX + 1 <= rx && x.MaxX + 1 >= lx)
					queue.Enqueue(new Position(x.MaxX + 1, current.Y - 1));
			});

			if (!queue.Any(x => x.Y == current.Y - 1) && !loop.Any(x => x.IsPointOnLine(new Position(lx, current.Y - 1))))
			{
				queue.Enqueue(new Position(lx, current.Y - 1));
			}
		}

		//PrintToFile(loop.Concat(fillLines));
		return fillLines.Sum(x => x.Length);
	}

	private int FindLx(IEnumerable<Line> loop, Position start)
	{
		return loop.Where(x => x.MaxX < start.X && x.DoLinesIntersect(Line.MaxHorizontal(start.Y))).OrderByDescending(x => x.MaxX).First().MaxX + 1;
	}

	private int FindRx(IEnumerable<Line> loop, Position start)
	{
		return loop.Where(x => x.MinX > start.X && x.DoLinesIntersect(Line.MaxHorizontal(start.Y))).OrderBy(x => x.MinX).First().MinX - 1;
	}

	private IEnumerable<Position> GetNeighbors(Position position)
	{
		yield return new Position(position.X + 1, position.Y);
		yield return new Position(position.X - 1, position.Y);
		yield return new Position(position.X, position.Y + 1);
		yield return new Position(position.X, position.Y - 1);
	}

	private void PrintToFile(IEnumerable<Line> lines)
	{
		var grid = Enumerable.Range(0, 1000).Select(x => new StringBuilder(string.Concat(Enumerable.Repeat('.', 1000)))).ToArray();
		lines.SelectMany(x => x.Points).ForEach(x =>
		{
			grid[x.Y * -1 + 500].Remove(x.X, 1);
			grid[x.Y * -1 + 500].Insert(x.X, '#');
		});

		File.WriteAllLines(this.InputPath.Replace(".txt", ".out.txt"), grid.Select(x => x.ToString()));
	}

	private IEnumerable<DigInstruction> ParseInput()
		=>
		File.ReadAllLines(this.InputPath).Select(x =>
		{
			var parts = x.Split(' ');
			var direction = parts[0] switch
			{
				"U" => Direction.Up,
				"D" => Direction.Down,
				"L" => Direction.Left,
				"R" => Direction.Right,
				_ => throw new Exception("Invalid direction")
			};

			var howFar = int.Parse(parts[1]);
			return new DigInstruction(direction, howFar);
		});

	private IEnumerable<DigInstruction> ParseInput2()
		=>
		File.ReadAllLines(this.InputPath).Select(x =>
		{
			var parts = x.Split(' ');
			var hexColor = parts[2].Replace("(#", "").Replace(")", "");

			var howFar = Convert.ToInt32(hexColor[0..4], 16);
			var dirChar = hexColor[5];
			var direction = dirChar switch
			{
				'3' => Direction.Up,
				'1' => Direction.Down,
				'2' => Direction.Left,
				'0' => Direction.Right,
				_ => throw new Exception("Invalid direction")
			};
			return new DigInstruction(direction, howFar);
		});

	private int HexToDecimal(string hex)
	{
		string hexValues = "0123456789ABCDEF";
		int decValue = 0;
		for (int i = 0; i < hex.Length; i++)
		{
			char current = hex[i];
			int next = hexValues.IndexOf(current);
			decValue = decValue * 16 + next;
		}

		return decValue;
	}

	private record DigInstruction(Direction Direction, int HowFar);

	private record Position(int X, int Y);
	
	private record Rectangle(Line Horizontal, Line Vertical);

	private record Line(Position Start, Position End)
	{
		public long Length => MaxX - MinX + MaxY - MinY + 1;

		public IEnumerable<Position> Points
		{
			get
			{
				if (IsHorizontal)
				{
					for (int i = MinX; i <= MaxX; i++)
						yield return new Position(i, Start.Y);
				}
				else if (IsVertical)
				{
					for (int i = MinY; i <= MaxY; i++)
						yield return new Position(Start.X, i);
				}
				else
				{
					throw new Exception("Not a line");
				}
			}
		}

		public static Line MaxHorizontal(int Y) => new Line(new Position(int.MinValue, Y), new Position(int.MaxValue, Y));

		public int MaxX => Math.Max(Start.X, End.X);

		public int MinX => Math.Min(Start.X, End.X);

		public int MaxY => Math.Max(Start.Y, End.Y);

		public int MinY => Math.Min(Start.Y, End.Y);

		public bool IsHorizontal => Start.Y == End.Y;

		public bool IsVertical => Start.X == End.X;

		public bool IsTouching(Position position)
		{
			if (IsVertical)
				return Math.Abs(position.X - Start.X) == 1 && position.Y >= Math.Min(Start.Y, End.Y) && position.Y <= Math.Max(Start.Y, End.Y);
			if (IsHorizontal)
				return position.Y == Start.Y && (position.X == MinX - 1 || position.X == MinX + 1);

			return false;
		}

		public bool DoLinesIntersect(Line other)
		{
			if (IsHorizontal && other.IsVertical)
			{
				return MinX <= other.MinX && other.MinX <= MaxX && other.MinY < MinY && MinY < other.MaxY;
			}

			if (IsVertical && other.IsHorizontal)
			{
				return other.MinX <= MinX && MinX <= other.MaxX && MinY <= other.MinY && other.MinY < MaxY;
			}

			if (IsHorizontal && other.IsHorizontal)
			{
				return MinY == other.MinY && ((MinX <= other.MinX && other.MinX <= MaxX) || (other.MinX <= MinX && MinX < other.MaxX));
			}

			return false; // not needed I guess both vertical
		}

		private bool IsOverlap(int start1, int end1, int start2, int end2)
		{
			return Math.Max(start1, start2) <= Math.Min(end1, end2);
		}

		public bool IsPointOnLine(Position point)
		{
			if (IsHorizontal)
				return point.Y == Start.Y && point.X >= MinX && point.X <= MaxX;

			if (IsVertical)
				return point.X == Start.X && point.Y >= MinY && point.Y <= MaxY;

			return false;
		}
	}

	enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}
}