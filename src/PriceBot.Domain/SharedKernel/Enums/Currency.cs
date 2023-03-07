namespace PriceBot.Domain.SharedKernel.Enums;

public class Currency
{
    public string Value { get; private set; } = string.Empty;

    public static Currency USD { get { return new Currency() { Value = "USD" }; } }
    public static Currency EUR { get { return new Currency() { Value = "EUR" }; } }
    public static Currency BRL { get { return new Currency() { Value = "BRL" }; } }
}
