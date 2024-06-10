using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRIX.Library.DiceValue
{
    public partial class DicePool
    {
		public override string ToString()
		{
			StringBuilder builder = new();

			foreach (Dice dice in Dice)
			{
				if (builder.Length > 0)
				{
					builder.Append('+');
				}

				builder.Append($"{dice.Count}d{dice.NumberOfFaces}");
			}

			if (Modifier < 0 || builder.Length == 0)
			{
				builder.Append($"{Modifier}");
			}
			else if (Modifier > 0)
			{
				builder.Append($"+{Modifier}");
			}

			return builder.ToString();
		}

		[GeneratedRegex("[d]{1}[0-9]+")]
		public static partial Regex DiceRegex();

		[GeneratedRegex("[0-9]+[d]{1}[0-9]+")]
		public static partial Regex MultiDiceRegex();

		[GeneratedRegex("[0-9]+")]
		public static partial Regex ModRegex();

		/// <summary>
		/// Парсит строку вида 3d6+d8+4 (reroll:1,2; crit:13x3roll; explode:2)
		/// reroll: переброс значений, указанных косле двоеточия.
		/// crit: первое число означает шанс, второе число — это модификатор,
		/// в случае прока значение умножается на второе число (модификатор), либо, если указан ключ roll,
		/// умножается только константа, а количество бросаемых костей умножается на модификатор.
		/// explode: при выпадении максимальных значений кость
		/// </summary>
		public static bool TryParse(string input, out DicePool? parsedDicePool)
		{
			try
			{
				return TryParseInternal(input, out parsedDicePool);
			}
			catch
			{
				parsedDicePool = null;

				return false;
			}
		}

		public static bool TryParseInternal(string input, out DicePool? parsedDicePool)
		{
			input = input.Replace('к', 'd');
			parsedDicePool = null;

			if (!IsValidDicePool(input))
			{
				return false;
			}
			else
			{
				parsedDicePool = new DicePool();
			}

			input = input.Replace(" ", string.Empty);
			string[] splittedString = input.Split('+');

			foreach (string entry in splittedString)
			{
				if (MultiDiceRegex().IsMatch(entry))
				{
					string[] splittedDice = entry.Split("d");

					parsedDicePool.Add(
						new Dice(
							int.Parse(splittedDice[1]),
							int.Parse(splittedDice[0])
						)
					);
				}
				else if (DiceRegex().IsMatch(entry))
				{
					parsedDicePool.Add(
						new Dice(int.Parse(entry[1..]))
					);
				}
				else if (ModRegex().IsMatch(entry))
				{
					parsedDicePool.Modifier += int.Parse(entry);
				}
			}

			return true;
		}

		public static bool IsValidDicePool(string input)
		{
			input = input.Replace(" ", string.Empty);
			string[] splittedInput = input.Split('+', StringSplitOptions.RemoveEmptyEntries);

			return splittedInput.All(x =>
				DiceRegex().IsMatch(x)
				|| MultiDiceRegex().IsMatch(x)
				|| ModRegex().IsMatch(x)
			);
		}
	}
}
