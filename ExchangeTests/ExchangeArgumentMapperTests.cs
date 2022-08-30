using Exchange;

namespace ExchangeTests;

public class ExchangeArgumentMapperTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GivenValidParams_WhileMappingToExchangeRequest_ThenMapsCorrectly()
    {
        var args = new[] { "EUR/DKK", "1" };

        var result = ExchangeArgumentMapper.TryMap(args, out var request);
        
        Assert.IsTrue(result);
        Assert.AreEqual("EUR", request.ConvertFromCurrency);
        Assert.AreEqual("DKK", request.ConvertToCurrency);
        Assert.AreEqual(1, request.AmountToConvert);
    }
    
    [Test]
    public void GivenInvalidParams_WhileMappingToExchangeRequest_ThenReturnsFalse()
    {
        var args = new[] { "1", "EUR/DKK" };

        var result = ExchangeArgumentMapper.TryMap(args, out var request);
        
        Assert.IsFalse(result);
        Assert.IsNull(request);
    }
    
    [Test]
    public void GivenInvalidNumberOfParams_WhileMappingToExchangeRequest_ThenReturnsFalse()
    {
        var args = new[] { "EUR/DKK" };

        var result = ExchangeArgumentMapper.TryMap(args, out var request);
        
        Assert.IsFalse(result);
        Assert.IsNull(request);
    }
}