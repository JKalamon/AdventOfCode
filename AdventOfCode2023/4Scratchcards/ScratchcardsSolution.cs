using MoreLinq;
using System.Text.RegularExpressions;

namespace AdventOfCode2023;

internal class ScratchcardsSolution : ChallengeBase
{
	public override string Title => "Scratchcards";

	public override int Day => 4;

	public override object SolvePart1()
	{
		var cards = File.ReadAllLines(this.InputPath).Select(Card.ParseLine);
		return cards.Sum(x => x.CardValue);
	}

	public override object? SolvePart2()
	{
		var cards = File.ReadAllLines(this.InputPath).Select(Card.ParseLine).ToArray();
		var cardsCount = Enumerable.Repeat(1, cards.Count()).ToArray();
		for (int i = 0; i < cards.Count(); i++)
		{
			var card = cards[i];
			for (int j = 1; j <= card.WinningNumbersCount; j++)
			{
				cardsCount[i + j] = cardsCount[i + j] + cardsCount[i];
			}
		}

		return cardsCount.Sum();
	}

	private record Card(int Id, IEnumerable<int> WinningNumbers, IEnumerable<int> CardNumbers)
	{
		public static Card ParseLine(string line)
		{
			var cardString = line.Split(":")[0];
			var setsString = line.Split(":")[1].Split("|");
			var id = int.Parse(Regex.Match(cardString, @"Card +([0-9]+)").Groups[1].Value);

			return new Card(id, setsString[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse), setsString[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
		}

		public int WinningNumbersCount => WinningNumbers.Intersect(CardNumbers).Count();

		public int CardValue => WinningNumbersCount > 0 ? (int)Math.Pow(2, WinningNumbersCount - 1) : 0;
	}
}
