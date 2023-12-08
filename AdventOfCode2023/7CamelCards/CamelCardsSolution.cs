namespace AdventOfCode2023;

internal class CamelCardsSolution : ChallengeBase
{
	public override string Title => "Camel Cards";

	public override int Day => 7;

	public override object SolvePart1()
	{
		var hand = ParseInput(false).OrderByDescending(x => x).ToArray();
		long result = 0;
		for (int i = 1; i <= hand.Length; i++)
		{
			result += hand[i - 1].bid * i;
		}

		return result;
	}

	public override object SolvePart2()
	{
		var hand = ParseInput(true).OrderByDescending(x => x).ToArray();
		var result = 0;
		for (int i = 1; i <= hand.Length; i++)
		{
			result += hand[i - 1].bid * i;
		}

		return result;
	}
	
	private IEnumerable<PokerHand> ParseInput(bool jokerAvailable)
		=> File.ReadAllLines(this.InputPath).Select(x =>
		{
			var split = x.Split(' ');
			var bid = int.Parse(split[1]);
			var hand = split[0];
			return new PokerHand(hand.Select(x => new Card(x, jokerAvailable)).ToArray(), bid, jokerAvailable);
		});

	public record Card(char card, bool jokerAvailable)
	{
		public int Value()
		{
			switch (card)
			{
				case 'A':
					return 14;
				case 'K':
					return 13;
				case 'Q':
					return 12;
				case 'J':
					return jokerAvailable ? 0 : 11;
				case 'T':
					return 10;
				default:
					return int.Parse(card.ToString());
			}
		}
	}

	public record PokerHand(Card[] hand, int bid, bool jokerAvailable) : IComparable<PokerHand>
	{
		public bool HasJoker() => jokerAvailable && hand.Any(x => x.card == 'J');

		public int Rank()
		{
			// remove duplicated from string
			var countCards = this.hand.Where(x => !jokerAvailable || x.card != 'J').GroupBy(x => x).OrderByDescending(x => x.Count()).ToArray();
			var firstGroup = countCards.FirstOrDefault();
			var secondGroup = countCards.Count() > 1 ? countCards[1] : null;
			var howManyJokers = jokerAvailable ? hand.Count(x => x.card == 'J') : 0;


			switch ((firstGroup?.Count() ?? 0) + howManyJokers, secondGroup?.Count() ?? 0)
			{
				case (5, _):
					//// five of a kind
					return 1;
				case (4, _):
					//// four of a kind					
					return 2;
				case (3, 2):
					//// full house
					return 3;
				case (3, _):
					//// three of a kind
					return 4;
				case (2, 2):
					//// two pair
					return 5;
				case (2, _):
					//// one pair
					return 6;
				case (1, 1):
					////high card
					return 7;
				default:
					return 1;
			}
		}

		public override string ToString()
		{
			var rank = this.Rank() switch
			{
				1 => "five of a kind",
				2 => "four of a kind",
				3 => "full house",
				4 => "three of a kind",
				5 => "two pair",
				6 => "one pair",
				7 => "high card",
				_ => throw new NotImplementedException(),
			};

			return $"Hand {string.Concat(this.hand.Select(x => x.card))} is {rank} and bid: {bid}";
		}

		int IComparable<PokerHand>.CompareTo(PokerHand? other)
		{
			if (other == null)
			{
				throw new ArgumentNullException(nameof(other));
			}

			if (this.Rank() > other.Rank())
			{
				return 1;
			}
			else if (this.Rank() < other.Rank())
			{
				return -1;
			}
			else
			{
				for (int i = 0; i < this.hand.Length; i++)
				{
					if (this.hand[i].Value() > other.hand[i].Value())
					{
						return -1;
					}
					else if (this.hand[i].Value() < other.hand[i].Value())
					{
						return 1;
					}
				}

				return 0;
			}
		}
	}
}
