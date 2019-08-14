using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Extensions
{
    public static class StringExtensions
    {
        public static string Mask(this string source, int start, int maskLength)
        {
            return source.Mask(start, maskLength, 'X');
        }

        public static string Mask(this string source, int start, int maskLength, char maskCharacter)
        {

        }
    }
}
