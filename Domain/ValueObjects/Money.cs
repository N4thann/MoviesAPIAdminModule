using Domain.SeedWork;
using Domain.SeedWork.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public Money() { }

        public Money(decimal amount, string currency = "USD") 
        {
            Validate.GreaterThan((int)amount, -1, nameof(amount));
            Validate.NotNullOrEmpty(currency, nameof(currency));
            Validate.MaxLength(currency, 3, nameof(currency));

            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public override string ToString()
        {
            return Currency switch
            {
                "USD" => $"${Amount:N2} {Currency}",
                "BRL" => $"R${Amount:N2} {Currency}",
                "EUR" => $"€{Amount:N2} {Currency}",
                "GBP" => $"£{Amount:N2} {Currency}",
                _ => $"{Amount:N2} {Currency}"
            };
        }

        //Factory Methods
        public static Money Zero(string currency = "USD") => new(0, currency);
        public static Money Dollars(decimal amount) => new(amount, "USD");
        public static Money Reais(decimal amount) => new(amount, "BRL");
        public static Money Euros(decimal amount) => new(amount, "EUR");
    }
}
