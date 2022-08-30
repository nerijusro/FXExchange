namespace CurrencyExchange;

public class CurrencyExchangeRequest
{
    public string ConvertFromCurrency { get; set; }
    public string ConvertToCurrency { get; set; }
    public decimal AmountToConvert { get; set; }

    public CurrencyExchangeRequest(string convertFromCurrency, string convertToCurrency, decimal amountToConvert)
    {
        ConvertFromCurrency = convertFromCurrency;
        ConvertToCurrency = convertToCurrency;
        AmountToConvert = amountToConvert;
    }

    public CurrencyExchangeRequest()
    {
    }
}