﻿using System;
using System.Globalization;
using System.Linq;

namespace Tomlet
{
    public static class TomlNumberUtils
    {
        public static long? GetLongValue(string input)
        {
            var isOctal = input.StartsWith("0o");
            var isHex = input.StartsWith("0x");
            var isBinary = input.StartsWith("0b");

            if (isBinary || isHex || isOctal)
                input = input.Substring(2);

            //Invalid characters, double underscores
            if (input.Contains("__") || input.Any(c => c != '_' && c != '-' && c != '+' && !char.IsDigit(c) && (c < 'a' || c > 'f')))
                return null;

            //Underscore without a digit before
            if (input.First() == '_')
                return null;

            //Underscore without a digit after
            if (input.Last() == '_')
                return null;

            input = input.Replace("_", "");

            try
            {
                if (isBinary)
                    return Convert.ToInt64(input, 2);

                if (isOctal)
                    return Convert.ToInt64(input, 8);

                if (isHex)
                    return Convert.ToInt64(input, 16);

                return Convert.ToInt64(input, 10);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static double? GetDoubleValue(string input)
        {
            var skippingFirst = input.Substring(1);

            if (input == "nan" || input == "inf" || skippingFirst == "nan" || skippingFirst == "inf")
            {
                //Special value
                if (input == "nan" || skippingFirst == "nan")
                    return double.NaN;
                if (input == "inf")
                    return double.PositiveInfinity;
                if (skippingFirst == "inf")
                    return input.StartsWith("-") ? double.NegativeInfinity : double.PositiveInfinity;
            }
            
            if (input.Contains("__") || input.Any(c => c != '_' && c != '-' && c != '+' && c != 'e' && c != '.' && !char.IsDigit(c)))
                return null;

            //Underscore without a digit before
            if (input.First() == '_')
                return null;

            //Underscore without a digit after
            if (input.Last() == '_')
                return null;

            input = input.Replace("_", "");

            //Theoretically we can have hex/octal/binary numbers with floating-point parts. I'm not implementing that.
            //None of the examples use it.
            if (double.TryParse(input, TomlNumberStyle.FloatingPoint, CultureInfo.InvariantCulture, out var val))
                return val;

            return null;
        }
    }
}