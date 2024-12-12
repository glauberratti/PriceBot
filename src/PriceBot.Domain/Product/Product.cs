using PriceBot.Domain.SharedKernel.Enums;

namespace PriceBot.Domain.Product
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BRLValue { get; set; }
        public decimal USDValue { get; set; }
        public decimal EURValue { get; set; }

        public void UpdateUsdCurrency(decimal usdValue)
        {
            USDValue = usdValue * BRLValue;
        }

        public void UpdateEurCurrency(decimal eurValue)
        {
            EURValue = eurValue * BRLValue;
        }

        public void UpdateCurrencyValue(Currency currency, decimal currencyValue)
        {
            // This strategy was chosen momentarily because if new currencies are added, the maintenance of the code that calls this method will be minimal.
            //
            // Essa estratégia foi escolhida momentaneamente porque se novas moedas forem adicionadas, a manutenção do código que chama esse método será mínima.

            //Dictionary<string, Action> d = new()
            //{
            //    {Currency.USD.Value, () => USDValue = currencyValue * BRLValue },
            //    {Currency.EUR.Value, () => EURValue = currencyValue * BRLValue },
            //};

            //if (d.TryGetValue(currency.Value, out var action))
            //{
            //    action();
            //}

            Dictionary<Currency, Action> d = new()
            {
                {Currency.USD, () => UpdateUsdCurrency(currencyValue) },
                {Currency.EUR, () => UpdateEurCurrency(currencyValue) },
            };

            if (d.TryGetValue(currency, out var action))
            {
                action();
            }
        }
    }
}
