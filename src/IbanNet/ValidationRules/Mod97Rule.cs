﻿using System.Globalization;
using System.Linq;
using System.Numerics;

namespace IbanNet.ValidationRules
{
	internal class Mod97Rule : IIbanValidationRule
	{
		private static readonly int CharCodeA = 'A';

		public IbanValidationResult InvalidResult { get; } = IbanValidationResult.InvalidCheckDigits;
		public bool Validate(string iban)
		{
			var upperIban = iban.ToUpperInvariant();
			var shiftedIban = upperIban.Substring(4) + upperIban.Substring(0, 4);

			var iso13616 = string.Join("", 
				shiftedIban.Select(c => char.IsNumber(c) 
					? c.ToString() 
					: (c - CharCodeA + 10).ToString()
				)
			);

			var largeInteger = BigInteger.Parse(iso13616, CultureInfo.InvariantCulture);
			return largeInteger % 97 == 1;
		}
	}
}
