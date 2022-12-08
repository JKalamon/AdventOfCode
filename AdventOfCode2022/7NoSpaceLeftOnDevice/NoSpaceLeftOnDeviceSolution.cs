namespace AdventOfCode2022;

internal class NoSpaceLeftOnDeviceSolution : IChallenge
{
	public string Title => "--- Day 7: No Space Left On Device ---";

	public DateTime DateTime => new(2022, 12, 7);

	private List<FakeDirectory> allDirectories = new List<FakeDirectory>();

	public object SolvePart1()
	{
		ParseInput();
		return allDirectories.Distinct().Where(x => x.Size() < 100000).Sum(x => x.Size());
	}

	public object? SolvePart2()
	{
		int wholeDriveSize = 70000000;
		var root = ParseInput();
		var freeUpSpace = 30000000 - (wholeDriveSize - root.Size());
		return allDirectories.Distinct().Where(x => x.Size() > freeUpSpace).OrderBy(x => x.Size()).First().Size();
	}

	private FakeDirectory ParseInput()
	{
		var root = new FakeDirectory("/");
		var current = root;
		var lines = File.ReadAllLines("7NoSpaceLeftOnDevice/input.txt");
		for (int i = 0; i < lines.Length; i++)
		{
			var line = lines[i];
			if (line.StartsWith("$ cd "))
			{
				var dirName = line.Replace("$ cd ", "");
				if (dirName == "/")
				{
					current = root;
				}
				else if (dirName == "..")
				{
					current = current.Parent;
				}
				else
				{
					current = current.UpsertDirectory(dirName);
					allDirectories.Add(current);
				}
			}

			if (line.StartsWith("$ ls"))
			{
				var iterator = 1;
				while (lines.Length > i + iterator && !lines[i + iterator].StartsWith("$"))
				{
					var nextLine = lines[i + iterator];
					if (nextLine.StartsWith("dir"))
					{
						allDirectories.Add(current.UpsertDirectory(nextLine.Replace("dir ", "")));
					}
					else
					{
						current.Files.Add(new FakeFile(nextLine.Split(" ")[1], int.Parse(nextLine.Split(" ")[0])));
					}

					iterator++;
				}

				i += iterator - 1;
			}
		}

		return root;
	}
}

internal record FakeFile(string Name, int Size);

internal class FakeDirectory
{
	public FakeDirectory(string Name, FakeDirectory? parent = null)
	{
		this.Name = Name;
		this.Parent = parent ?? this;
	}

	public List<FakeFile> Files { get; init; } = new List<FakeFile>();

	public List<FakeDirectory> Directories { get; init; } = new List<FakeDirectory>();

	public FakeDirectory Parent { get; init; }

	public string Name { get; init; }

	public int Size()
			=> this.Files.Sum(x => x.Size) + this.Directories.Sum(x => x.Size());

	public FakeDirectory UpsertDirectory(string dirName)
	{
		var child = this.Directories.FirstOrDefault(x => x.Name == dirName);
		if (child == null)
		{
			child = new FakeDirectory(dirName, this);
			this.Directories.Add(child);
		}

		return child;
	}
};