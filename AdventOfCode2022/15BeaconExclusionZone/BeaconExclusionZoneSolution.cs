using MoreLinq;
using Newtonsoft.Json.Linq;

namespace AdventOfCode2022;

internal class BeaconExclusionZoneSolution : IChallenge
{
    public string Title => "--- Day 15: Beacon Exclusion Zone ---";

    public DateTime DateTime => new(2022, 12, 15);

    private IEnumerable<BeaconSensor> Input = File.ReadAllLines("15BeaconExclusionZone/input.txt")
                                    .Select(x =>
                                    {
                                        x = x.Replace("Sensor at x=", "")
                                             .Replace(" y=", "")
                                             .Replace(": closest beacon is at x=", ",");
                                        var spl = x.Split(",").Select(a => int.Parse(a)).ToArray();
                                        return new BeaconSensor(new(spl[0], spl[1]), new(spl[2], spl[3]));
                                    });

    public object SolvePart1()
    {
        var mapMin = this.Input.SelectMany(x => new[] { x.Beacon.X, x.Sensor.X }).Min();
        var mapMax = this.Input.SelectMany(x => new[] { x.Beacon.X, x.Sensor.X }).Max();

		var distances = new List<Range2D>();
		var lineY = 10; 
        this.Input.ForEach(x => {
			var left = x.Distance - Math.Abs(lineY - x.Sensor.Y); 
			if(left < 0)
				return;

			var tmp = new List<Range2D>();
			var range = new Range2D(x.Sensor.X - left, x.Sensor.X + left);
			tmp.AddRange(distances.SelectMany(d => d.Substract(range)));
			tmp.Add(range);
			distances = tmp;
		});

        return distances.Sum(x => x.Distance);
    }

    public object? SolvePart2()
    {
		var mapMin = this.Input.SelectMany(x => new[] { x.Beacon.X, x.Sensor.X }).Min();
		if (mapMin < 0)
			mapMin = 0;
        var mapMax = this.Input.SelectMany(x => new[] { x.Beacon.X, x.Sensor.X }).Max();
		var distanceToFind = mapMax - mapMin;
		var distances = new List<Range2D>();
		var lineY = 0;
		while(lineY < 40000000 && CountDistance(distances) != distanceToFind){
			distances = new List<Range2D>();
			this.Input.ForEach(x => {
				var distLeftRight = x.Distance - Math.Abs(lineY - x.Sensor.Y); 
				if(distLeftRight < 0)
					return;

				var tmp = new List<Range2D>();

				var left = x.Sensor.X - distLeftRight;
				var right = x.Sensor.X + distLeftRight;
				if(left < mapMin)
					left = mapMin;
				if(right > mapMax)
					right = mapMax;
				var range = new Range2D(left, right);
				tmp.AddRange(distances.SelectMany(d => d.Substract(range)));
				tmp.Add(range);
				distances = tmp;
			});

			lineY++;
		}

        return FindMissingX(distances) + lineY.ToString();
    }

    public record Range2D(int Start, int End)
    {
        public int Distance => this.End - this.Start + 1;

        public IEnumerable<Range2D> Substract(Range2D another)
        {
            if (this.Start >= another.Start && this.End <= another.End)
                return new Range2D[0];

            if (this.End < another.Start || this.Start > another.End)
            	return new Range2D[1] { this };
			
			var resultArray = new List<Range2D>();
			var leftPart = new Range2D(this.Start, another.Start);
            if(leftPart.Distance > 0)
				resultArray.Add(leftPart);
			
			var rightPart = new Range2D(another.End, this.End);
			if(rightPart.Distance > 0)
				resultArray.Add(rightPart);
			
			return resultArray;
        }
    }

	public int CountDistance(IEnumerable<Range2D> ranges)
	{
		var lastStanding = int.MinValue;
		var xa = ranges.ToArray();
		var result = 0;
		ranges.OrderBy(x=> x.Start).ForEach(r => {
			result += r.Distance;
			if(lastStanding == r.Start){
				result--;
			}

			lastStanding = r.End;
		});

		return result;
	}

	public int FindMissingX(IEnumerable<Range2D> ranges)
	{
		var lastStanding = int.MinValue;
		var missing = int.MinValue;
		var xa = ranges.ToArray();
		var result = 0;
		ranges.OrderBy(x=> x.Start).ForEach(r => {
			result += r.Distance;
			if(lastStanding == r.Start){
				result--;
			}
			else{
				missing = r.Start - 1;
			}

			lastStanding = r.End;
		});

		if(missing == int.MinValue)
			return ++lastStanding;
		return result;
	}

    public record BeaconSensor(Point2D Sensor, Point2D Beacon)
    {
        public int Distance => this.Sensor.Distance(this.Beacon);
    };

    public record Point2D(int X, int Y)
    {
        public int Distance(Point2D point)
        {
            return Math.Abs(point.X - this.X) + Math.Abs(point.Y - this.Y);
        }
    };
}