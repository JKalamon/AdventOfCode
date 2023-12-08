using MoreLinq;

namespace AdventOfCode2023;

internal class HauntedWastelandSolution : ChallengeBase
{
	public override string Title => "Haunted Wasteland";

	public override int Day => 8;

	public override object SolvePart1()
	{
		var seq = File.ReadAllLines(this.InputPath)[0];
		var nodes = ParseInput();

		var i = 0;
		var counter = 0;
		var currentNode = nodes.First(x => x.IsStart);
		while (true)
		{
			currentNode = seq[i] == 'L' ? currentNode.Left : currentNode.Right;

			counter++;
			i = (i + 1) % seq.Length;
			if (currentNode!.IsDestination)
			{
				break;
			}
		}

		return counter;
	}

	public override object SolvePart2()
	{
		var seqIsLeft = File.ReadAllLines(this.InputPath)[0].Select(x => x == 'L').ToArray();
		var nodes = ParseInput();

		var i = 0;
		long counter = 0;
		var currentNodes = nodes.Where(x => x.StartMaybe).ToArray();
		var exits = currentNodes.Select(x => 0L).ToArray();


		while (true)
		{
			counter++;
			for (var j = 0; j < currentNodes.Length; j++)
			{
				currentNodes[j] = seqIsLeft[i] ? currentNodes[j].Left! : currentNodes[j].Right!;
				if (currentNodes[j].DestinationMaybe)
					exits[j] = counter;
			}


			i = (i + 1) % seqIsLeft.Length;
			if (currentNodes.All(x => x.DestinationMaybe) || exits.All(x => x > 0))
			{
				break;
			}
		}

		if (currentNodes.All(x => x.DestinationMaybe))
			return counter;

		return LeastCommonMultiple(exits);
	}

	public long LeastCommonMultiple(params long[] numbers)
	{
		if (numbers == null || numbers.Length == 0)
		{
			throw new ArgumentException("At least one number must be provided");
		}

		long lcm = Math.Abs(numbers[0]);

		for (int i = 1; i < numbers.Length; i++)
		{
			lcm = FindLeastCommonMultiple(lcm, Math.Abs(numbers[i]));
		}

		return lcm;
	}

	private long FindLeastCommonMultiple(long a, long b)
	{
		return a / FindGreatestCommonDivisor(a, b) * b;
	}

	private long FindGreatestCommonDivisor(long a, long b)
	{
		while (b != 0)
		{
			long temp = b;
			b = a % b;
			a = temp;
		}

		return a;
	}

	private IEnumerable<GhostElement> ParseInput()
	{
		var output = new List<GhostElement>();
		File.ReadAllLines(this.InputPath).Skip(2).ForEach(x =>
		{
			var split = x.Split('=', StringSplitOptions.RemoveEmptyEntries);
			var name = split[0].Trim();
			var nameLeft = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries)[0].Replace("(", "").Trim();
			var nameRight = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries)[1].Replace(")", "").Trim();
			var nodeLeft = EnsureNode(output, nameLeft);
			var nodeRight = EnsureNode(output, nameRight);
			var currentNode = EnsureNode(output, name);

			currentNode.Right = nodeRight;
			currentNode.Left = nodeLeft;
		});

		return output;
	}

	private GhostElement EnsureNode(List<GhostElement> elements, string nodeName)
	{
		var output = elements.FirstOrDefault(x => x.Name == nodeName);
		if (output == null)
		{
			output = new GhostElement(nodeName);
			elements.Add(output);
		}

		return output;
	}

	private record GhostElement(string Name)
	{
		public GhostElement? Left { get; set; }

		public GhostElement? Right { get; set; }

		public bool IsDestination => Name == "ZZZ";

		public bool DestinationMaybe => Name.EndsWith('Z');

		public bool StartMaybe => Name.EndsWith('A');

		public bool IsStart => Name == "AAA";
	}
}
