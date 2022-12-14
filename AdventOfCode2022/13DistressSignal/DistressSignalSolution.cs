using MoreLinq;
using Newtonsoft.Json.Linq;

namespace AdventOfCode2022;

internal class DistressSignalSolution : IChallenge
{
    public string Title => "--- Day 13: Distress Signal ---";

    public DateTime DateTime => new(2022, 12, 13);

    private string[] Input = File.ReadAllLines("13DistressSignal/input.txt").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

    public object SolvePart1()
    {
		var pairs = new List<PairDs>();
        for (int i = 0; i < this.Input.Length; i += 2)
            pairs.Add(new(new(this.Input[i]), new(this.Input[i + 1])));
		
        return pairs.Select((p, i) => new {p, i = i + 1}).Where(x => x.p.First.IsLessThan(x.p.Second) == true).Sum(x => x.i);
    }

    public object? SolvePart2()
    {
        var list = new List<DistressSignal>();
        for (int i = 0; i < this.Input.Length; i++)
        {
            list.Add(new(this.Input[i]));
		}


		var div1 = new DistressSignal("[[2]]");
		var div2 = new DistressSignal("[[6]]");
		list.Add(div1);
		list.Add(div2);
		list.Sort();

        var sel = list.Select((p, i) => new {p, i = i + 1}).Where(x => x.p == div1 || x.p == div2);
		return sel.First().i * sel.Last().i;
    }

    public record PairDs(DistressSignal First, DistressSignal Second);
}

public class DistressSignal : IComparable
{
    public DistressSignal(int number)
    {
        this.IntValue = number;
    }

    public DistressSignal(string val)
    {
        if (!val.StartsWith('[') || !val.EndsWith(']'))
            throw new ArgumentException($"Unexpected input {val}");

		var json = JArray.Parse(val);
		this.Array = json.Select(x => {
			if(x.Type == JTokenType.Array){
				return new DistressSignal(x.ToString());
			}

			return new DistressSignal(x.Value<int>());
		}).ToArray();

    }

    public int? IntValue { get; init; }

    public DistressSignal[]? Array { get; init; }

    public int CompareTo(object? obj)
    {
		var comp = (DistressSignal)obj!;
		if (comp == null)
			return 0;
		
		var res = this.IsLessThan(comp);
		if (res.HasValue && !res.Value){
			return 1;
		}

		if (res.HasValue && res.Value){
			return -1;
		}

		return 0;
    }	

    public bool? IsLessThan(DistressSignal anotherNumber){
		if (this.IntValue != null && anotherNumber.IntValue != null){
			if (this.IntValue == anotherNumber.IntValue)
				return null;
			return this.IntValue < anotherNumber.IntValue;
		}

		var array1 = this.Array ?? new DistressSignal[] { new DistressSignal(this.IntValue!.Value) };
		var array2 = anotherNumber.Array ?? new DistressSignal[] { new DistressSignal(anotherNumber.IntValue!.Value) };		

		for (int i = 0; i < array1.Length; i++)
		{
			if (array2.Length - 1 < i)
				return false;
			
			var canDeterminate = array1[i].IsLessThan(array2[i]);
			if(canDeterminate.HasValue)
				return canDeterminate.Value;
		}

		if (array1.Length < array2.Length)
		{
			return true;
		}

		return null;
	}
}