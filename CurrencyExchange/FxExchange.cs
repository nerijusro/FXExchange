namespace CurrencyExchange;

public class FxExchange
{
    private const string InvalidCurrencyErrorMessage = "Currency provided is not valid: ";
    
    private readonly string _mainCurrency;
    private readonly int _amountToPurchase;
    private readonly Dictionary<string, decimal> _exchangeRates;

    private IEnumerable<string> SupportedCurrencies => _exchangeRates.Keys.Append(_mainCurrency);

    public FxExchange(string mainCurrency, int amountToPurchase, Dictionary<string, decimal> exchangeRates)
    {
        _mainCurrency = mainCurrency;
        _amountToPurchase = amountToPurchase;
        _exchangeRates = exchangeRates;
    }

    public decimal Convert(CurrencyExchangeRequest request)
    {
        ValidateInput(new[] { request.ConvertFromCurrency, request.ConvertToCurrency });

        var convertFromPriceInMainCurrency = _mainCurrency.Equals(request.ConvertFromCurrency) ? 1 
            : _exchangeRates[request.ConvertFromCurrency] / _amountToPurchase;
        
        var convertToPriceInMainCurrency = _mainCurrency.Equals(request.ConvertToCurrency) ? 1 
            : _exchangeRates[request.ConvertToCurrency] / _amountToPurchase;

        var exchangeRate = convertFromPriceInMainCurrency / convertToPriceInMainCurrency;
        
        return exchangeRate * request.AmountToConvert;
    }

    private void ValidateInput(IEnumerable<string> currencies)
    {
        var unsupportedCurrencies = 
            currencies.Where(currency => !SupportedCurrencies.Contains(currency)).ToList();

        if (unsupportedCurrencies.Any())
        {
            throw new ArgumentException($"{InvalidCurrencyErrorMessage}{string.Join(", ", unsupportedCurrencies)}");
        }
    }
}