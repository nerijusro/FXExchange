using CurrencyExchange;

namespace Exchange;

public static class ExchangeArgumentMapper
{
    private const string DefaultArgumentFormat = "(currency pair) (amount to exchange)";

    public static bool TryMap(string[] args, out CurrencyExchangeRequest exchangeRequest)
    {
        exchangeRequest = null;
        if (args.Length != 2)
        {
            return false;
        }

        var currencyPair = args[0].Split("/");
        var amountInString = args[1];
        var isValid = decimal.TryParse(amountInString, out var amountInDecimal) && currencyPair.Length == 2;

        if (!isValid)
        {
            return false;
        }

        exchangeRequest = new CurrencyExchangeRequest
        {
            ConvertFromCurrency = currencyPair[0],
            ConvertToCurrency = currencyPair[1],
            AmountToConvert = amountInDecimal
        };

        return true;
    }

    public static string GetFormat()
    {
        return DefaultArgumentFormat;
    }
}