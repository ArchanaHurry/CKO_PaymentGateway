using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaymentGateway.Utilities
{
    public class CardNumMasker
    {
        public CardNumMasker()
        {
        }

        public string MaskNumber(string cardNumber)
        {

            var firstDigits = cardNumber.Substring(0, 4);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new String('X', cardNumber.Length - firstDigits.Length - lastDigits.Length);

            string maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
            string newMaskedString = Regex.Replace(maskedString, ".{4}", "$0 ");

            return newMaskedString;

        }

    }
}
