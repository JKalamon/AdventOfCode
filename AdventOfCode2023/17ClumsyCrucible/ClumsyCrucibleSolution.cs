using MoreLinq;

namespace AdventOfCode2023;

internal class ClumsyCrucibleSolution : ChallengeBase
{
	public override string Title => "Clumsy Crucible";

	public override int Day => 17;

	private Dictionary<Position, IEnumerable<Vector>> cache = new Dictionary<Position, IEnumerable<Vector>>();


	private uint[][] records = [];
	private uint[][] recordsXY = [];

	private int width = 0;
	private int height = 0;

	public override void Init()
	{
		records = ParseInput();
		recordsXY = ParseInputXY();
		height = records.Count() - 1;
		width = records[0].Count() - 1;
	}

	public override object SolvePart1()
	{
		return Dijkstra();
	}

	public override object SolvePart2()
	{
		this.cache.Clear();
		return Dijkstra(4, 10);
	}

	private record Position(int X, int Y);

	private record State(Position Pos, Direction Direction, uint DistanceInTheSameWay, uint Heat);

	private record StateForVector(Position Pos, Direction DirectionComeFrom, uint Heat);

	private IEnumerable<Position> GetSiblings(IEnumerable<Position> records, int x, int y)
	{
		return records.Where(hr => (hr.Y == y && (hr.X == x - 1 || hr.X == x + 1) || (hr.X == x && (hr.Y == y - 1 || hr.Y == y + 1))));
	}

	private IEnumerable<Vector> GetSiblingsVectors(Position currentBlock, int maxVectorCount, int minimum = 1)
	{
		if(cache.ContainsKey(currentBlock))
			return cache[currentBlock];

		var vectors = new List<Vector>();
		var iterator = Enumerable.Range(minimum, maxVectorCount - minimum + 1);
		iterator.ForEach(i =>
		{
			var destination = new Position(currentBlock.X, currentBlock.Y - i);
			if (Exist(destination)) // UP
				vectors.Add(new(destination, recordsXY[currentBlock.X].Where((_, ii) => ii < currentBlock.Y && ii >= currentBlock.Y - i).Sum(), Direction.Up));

			destination = new Position(currentBlock.X, currentBlock.Y + i);
			if (Exist(destination)) // DOWN
				vectors.Add(new(destination, recordsXY[currentBlock.X].Where((_, ii) => ii > currentBlock.Y && ii <= currentBlock.Y + i).Sum(), Direction.Down));

			destination = new Position(currentBlock.X - i, currentBlock.Y);
			if (Exist(destination)) // LEFT
				vectors.Add(new(destination, records[currentBlock.Y].Where((_, ii) => ii < currentBlock.X && ii >= currentBlock.X - i).Sum(), Direction.Left));

			destination = new Position(currentBlock.X + i, currentBlock.Y);
			if (Exist(destination)) // Right
				vectors.Add(new(destination, records[currentBlock.Y].Where((_, ii) => ii > currentBlock.X && ii <= currentBlock.X + i).Sum(), Direction.Right));
		});

		cache.Add(currentBlock, vectors);
		return vectors;
	}


	bool Exist(Position pos) => Exist(pos.X, pos.Y);

	bool Exist(int x, int y)
		=> x >= 0 && y >= 0 && x <= width && y <= height;

	uint? FirstOrDefault(Position pos) => FirstOrDefault(pos.X, pos.Y);

	uint? FirstOrDefault(int x, int y)
	{
		if (x < 0 || y < 0 || x > width || y > height)
			return null;

		return records[y][x];
	}

	private record Vector(Position Destination, uint HeatLoss, Direction Direction);

	private enum Direction
	{
		Up,
		Down,
		Left,
		Right
	}

	uint Dijkstra(int minCount = 1, int maxCount = 3)
	{
		var visited = new Dictionary<StateForVector, uint>();
		var queue = new PriorityQueue<StateForVector, uint>();
		var destination = new Position(width, height);
		var start = new Position(0, 0);

		queue.Enqueue(new StateForVector(start, Direction.Down, 0), 0);
		queue.Enqueue(new StateForVector(start, Direction.Right, 0), 0);
		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			if (current.Pos == destination)
				return current.Heat;

			var vectors = GetSiblingsVectors(current.Pos, maxCount, minCount);
			foreach (var sib in vectors)
			{
				var direction = GetDirection(current.Pos, sib.Destination);
				if (current.DirectionComeFrom == GetOpposite(direction))
					continue; // cannot go backwards

				if (current.DirectionComeFrom == sib.Direction)
					continue; // cannot go the same way as we use now vectors

				var heat = current.Heat + sib.HeatLoss;
				var newState = new StateForVector(sib.Destination, direction, heat);
				var value = visited.GetValueOrDefault(newState, uint.MaxValue);

				if (heat < value)
				{
					visited[newState] = heat;
					queue.Enqueue(newState, newState.Heat);
				}
			}
		}

		return uint.MaxValue;

	}

	Direction GetOpposite(Direction d)
		=> d switch
		{
			Direction.Up => Direction.Down,
			Direction.Down => Direction.Up,
			Direction.Left => Direction.Right,
			Direction.Right => Direction.Left,
			_ => throw new NotImplementedException()
		};


	Direction GetDirection(Position from, Position to)
	{
		if (from.X == to.X)
		{
			if (from.Y > to.Y)
				return Direction.Up;
			else
				return Direction.Down;
		}
		else
		{
			if (from.X > to.X)
				return Direction.Left;
			else
				return Direction.Right;
		}
	}

	private uint[][] ParseInput()
		=> File.ReadAllLines(this.InputPath).Select((line, y) =>
			line.Select((c, x) => uint.Parse(c.ToString())).ToArray()).ToArray();

	private uint[][] ParseInputXY()
	{
		string[] lines = File.ReadAllLines(this.InputPath);
		int rows = lines.Length;
		int cols = lines[0].Length;

		uint[][] parsedArray = new uint[cols][];

		for (int x = 0; x < cols; x++)
		{
			parsedArray[x] = new uint[rows];
			for (int y = 0; y < rows; y++)
			{
				parsedArray[x][y] = uint.Parse(lines[y][x].ToString());
			}
		}

		return parsedArray;
	}
}