using CurrencyExchange;

namespace CurrencyExchangeTests;

public class FxExchangeTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GivenValidInput_WhenConvertingFromMainCurrency_ThenConvertsSuccessfully()
    {
        var mainCurrency = "DKK";
        var amountToPurchase = 100;
        var exchangeRates = new Dictionary<string, decimal> { { "EUR", decimal.Parse("743,94") } };
        var fxExchange = new FxExchange(mainCurrency, amountToPurchase, exchangeRates);

        var result = fxExchange.Convert(new CurrencyExchangeRequest("DKK", "EUR", 1));
        
        Assert.AreEqual(decimal.Parse("0,1344194424281528080221523241"), result);
    }
    
    [Test]
    public void GivenValidInput_WhenConvertingToMainCurrency_ThenConvertsSuccessfully()
    {
        var mainCurrency = "DKK";
        var amountToPurchase = 100;
        var exchangeRates = new Dictionary<string, decimal> { { "EUR", decimal.Parse("743,94") } };
        var fxExchange = new FxExchange(mainCurrency, amountToPurchase, exchangeRates);

        var result = fxExchange.Convert(new CurrencyExchangeRequest("EUR", "DKK", 1));
        
        Assert.AreEqual(decimal.Parse("7,4394"), result);
    }
    
    [Test]
    public void GivenValidInput_WhenConvertingNonMainCurrencies_ThenConvertsSuccessfully()
    {
        var mainCurrency = "DKK";
        var amountToPurchase = 100;
        var exchangeRates = new Dictionary<string, decimal>
        {
            { "EUR", decimal.Parse("743,94") },
            { "GBP", decimal.Parse("852,85") }
        };
        var fxExchange = new FxExchange(mainCurrency, amountToPurchase, exchangeRates);

        var result = fxExchange.Convert(new CurrencyExchangeRequest("EUR", "GBP", 1));
        
        Assert.AreEqual(decimal.Parse("0,8722987629712141642727326025"), result);
    }
    
    [Test]
    public void GivenValidInput_WhenConvertingMainCurrencyToMainCurrency_ThenConvertsSuccessfully()
    {
        var mainCurrency = "DKK";
        var amountToPurchase = 100;

        var fxExchange = new FxExchange(mainCurrency, amountToPurchase, new Dictionary<string, decimal>());

        var result = fxExchange.Convert(new CurrencyExchangeRequest("DKK", "DKK", 1));
        
        Assert.AreEqual(decimal.Parse("1"), result);
    }
    
    [Test]
    public void GivenValidInput_WhenConvertingNonMainCurrencyToSameNonMainCurrency_ThenConvertsSuccessfully()
    {
        var mainCurrency = "DKK";
        var amountToPurchase = 100;
        var exchangeRates = new Dictionary<string, decimal>
        {
            { "EUR", decimal.Parse("743,94") },
        };
        var fxExchange = new FxExchange(mainCurrency, amountToPurchase, exchangeRates);

        var result = fxExchange.Convert(new CurrencyExchangeRequest("EUR", "EUR", 1));
        
        Assert.AreEqual(decimal.Parse("1"), result);
    }
    
    [Test]
    public void GivenInvalidInput_WhenConvertingFromNonExistingCurrency_ThenThrowsException()
    {
        var mainCurrency = "DKK";
        var amountToPurchase = 100;
        var exchangeRates = new Dictionary<string, decimal>
        {
            { "EUR", decimal.Parse("743,94") },
        };
        var fxExchange = new FxExchange(mainCurrency, amountToPurchase, exchangeRates);
        
        var ex = Assert.Throws<ArgumentException>(() => fxExchange.Convert(new CurrencyExchangeRequest("KRW", "EUR", 1)));
        Assert.That(ex?.Message, Is.EqualTo("Currency provided is not valid: KRW"));
    }
    
    [Test]
    public void GivenInvalidInput_WhenConvertingToNonExistingCurrency_ThenThrowsException()
    {
        var mainCurrency = "DKK";
        var amountToPurchase = 100;
        var exchangeRates = new Dictionary<string, decimal>
        {
            { "EUR", decimal.Parse("743,94") },
        };
        var fxExchange = new FxExchange(mainCurrency, amountToPurchase, exchangeRates);
        
        var ex = Assert.Throws<ArgumentException>(() => fxExchange.Convert(new CurrencyExchangeRequest("EUR", "LIT", 1)));
        Assert.That(ex?.Message, Is.EqualTo("Currency provided is not valid: LIT"));
    }
    
    [Test]
    public void GivenInvalidInput_WhenConvertingNullCurrency_ThenThrowsException()
    {
        var mainCurrency = "DKK";
        var amountToPurchase = 100;
        var exchangeRates = new Dictionary<string, decimal>
        {
            { "EUR", decimal.Parse("743,94") },
        };
        var fxExchange = new FxExchange(mainCurrency, amountToPurchase, exchangeRates);
        
        var ex = Assert.Throws<ArgumentException>(() => fxExchange.Convert(new CurrencyExchangeRequest("EUR", null, 1)));
        Assert.That(ex?.Message, Is.EqualTo("Currency provided is not valid: "));
    }
}