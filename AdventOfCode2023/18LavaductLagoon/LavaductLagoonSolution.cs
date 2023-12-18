using MoreLinq;

namespace AdventOfCode2023;

internal class LavaductLagoonSolution : ChallengeBase
{
	public override string Title => "Lavaduct Lagoon";

	public override int Day => 18;

	public override object SolvePart1()
	{
		var maze = Dig(this.ParseInput());
		var flood = FloodFill(maze, new Position(1, -1));
		return maze.Concat(flood).Distinct().Count();
	}

	private IEnumerable<Line> Dig(IEnumerable<DigInstruction> instructions)
	{
		var position = new Position(0, 0);
		foreach (var digInstruction in instructions)
		{
			var (direction, howFar, hexColor) = digInstruction;

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
		var lx = start.X;
		while (loop.Any(x => x.IsInLine(new Position(lx, start.Y))))
			lx--;
		var rx = start.X;
		while (loop.Any(x => x.IsInLine(new Position(rx, start.Y))))
			rx++;
		
	}

	private IEnumerable<Position> GetNeighbors(Position position)
	{
		yield return new Position(position.X + 1, position.Y);
		yield return new Position(position.X - 1, position.Y);
		yield return new Position(position.X, position.Y + 1);
		yield return new Position(position.X, position.Y - 1);
	}

	public override object SolvePart2()
	{
		return "";
		// var instructions = this.ParseInput().ToList();
		// var loop = Dig(instructions).Distinct().ToList();
		// var start = loop.MinBy(x => x.X).MinBy(x => x.Y).First();
		// var floodFill = FloodFill(loop, start).ToList();
		// var visited = new HashSet<Position>();
		// var queue = new Queue<Position>();
		// queue.Enqueue(start);
		// while (queue.Any())
		// {
		// 	var current = queue.Dequeue();
		// 	if (visited.Contains(current))
		// 		continue;
		// 	visited.Add(current);
		// 	foreach (var neighbor in GetNeighbors(current))
		// 	{
		// 		if (visited.Contains(neighbor))
		// 			continue;
		// 		queue.Enqueue(neighbor);
		// 	}
		// }

		// var unvisited = floodFill.Except(visited).ToList();
		// var colors = unvisited.Select(x => instructions.First(y => y.Direction == Direction.Up && y.HowFar == 1 && y.HexColor == "#000000")).ToList();
		// var colorIndex = 0;
		// var color = colors[colorIndex];
		// var result = 0;
		// foreach (var position in unvisited)
		// {
		// 	if (position.X == 0 && position.Y == 0)
		// 		continue;
		// 	if (position.X == 0 && position.Y == 1)
		// 		continue;
		// 	if (position.X == 1 && position.Y == 0)
		// 		continue;
		// 	if (position.X == 1 && position.Y == 1)
		// 		continue;
		// 	result++;
		// 	colorIndex++;
		// 	if (colorIndex >= colors.Count)
		// 		colorIndex = 0;
		// 	color = colors[colorIndex];
		// }

		// return result;

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
			var hexColor = parts[2].Replace("(#", "").Replace(")", "");
			return new DigInstruction(direction, howFar, hexColor);
		});

	private record DigInstruction(Direction Direction, int HowFar, string HexColor);

	private record Position(int X, int Y);

	private record Line(Position Start, Position End)
	{
		public IsInLine(Position position)
		{
			if (Start.X == End.X)
				return position.X == Start.X && position.Y >= Math.Min(Start.Y, End.Y) && position.Y <= Math.Max(Start.Y, End.Y);
			if (Start.Y == End.Y)
				return position.Y == Start.Y && position.X >= Math.Min(Start.X, End.X) && position.X <= Math.Max(Start.X, End.X);
			throw new Exception("Invalid line");
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