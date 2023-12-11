using MoreLinq;
using MoreLinq.Extensions;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AdventOfCode2023;

internal class PipeMazeSolution : ChallengeBase
{
	public override string Title => "Pipe Maze";

	public override int Day => 10;

	public override object SolvePart1()
	{
		var array = ParseInput('F');

		return array.Length / 2;
	}

	public override object SolvePart2()
	{
		var array = ParseInput('F');
		File.WriteAllText(this.InputPath + ".json", JsonSerializer.Serialize(array));

		//// TODO implement Flood Fill algorithm https://en.wikipedia.org/wiki/Flood_fill
		return "Please run now browser with generated input.";
	}

	private Point[] ParseInput(char startCharacter)
	{
		var lines = File.ReadAllLines(this.InputPath).ToArray();
		var startY = lines.FirstIndexOf(x => x.Contains("S"));
		var startX = lines.First(x => x.Contains("S")).IndexOf("S");
		var currentPoint = new Point(startX, startY, startCharacter);
		List<Point> list = [currentPoint];
		var startPoint = currentPoint;

		char GetChar(int x, int y)
		{
			if (lines.Length > y && lines[0].Length > x && x >= 0 && y >= 0)
			{
				return lines[y][x] == 'S' ? startCharacter : lines[y][x];
			}
			else
			{
				return '?';
			}
		};

		do
		{
			var down = new Point(currentPoint.X, currentPoint.Y + 1, GetChar(currentPoint.X, currentPoint.Y + 1));
			var left = new Point(currentPoint.X - 1, currentPoint.Y, GetChar(currentPoint.X - 1, currentPoint.Y));
			var up = new Point(currentPoint.X, currentPoint.Y - 1, GetChar(currentPoint.X, currentPoint.Y - 1));
			var right = new Point(currentPoint.X + 1, currentPoint.Y, GetChar(currentPoint.X + 1, currentPoint.Y));

			Point[] roads = currentPoint.C switch
			{
				'|' => [down, up],
				'-' => [left, right],
				'L' => [up, right],
				'J' => [up, left],
				'7' => [left, down],
				'F' => [down, right],
				_ => []
			};

			currentPoint = roads.FirstOrDefault(x => !list.Contains(x));
			if (currentPoint != null)
			{
				list.Add(currentPoint);
			}
		} while (currentPoint != null);

		return list.ToArray();
	}

	private record Point(int X, int Y, char C)
	{
		public bool IsTheSame(Point other) => X == other.X && Y == other.Y;
	}
}
