using Exchange.JsonConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Moq;

namespace ExchangeTests;

public class JsonConfigurationReaderTests
{
    private IEnumerable<IConfigurationProvider> _jsonConfigurationProviders;
    
    [SetUp]
    public void Setup()
    {
        var jsonConfigurationSource = new JsonConfigurationSource();
        _jsonConfigurationProviders = new List<IConfigurationProvider> { new JsonConfigurationProvider(jsonConfigurationSource) };
    }

    [Test]
    public void GivenConfigurationRoot_WhenRootContainsJsonProvider_ReturnsJsonConfigurationReader()
    {
        var configurationRoot = new Mock<IConfigurationRoot>();
        configurationRoot.Setup(x => x.Providers).Returns(() => _jsonConfigurationProviders);

        var reader = new JsonConfigurationReader(configurationRoot.Object);
        Assert.That(reader.GetType(), Is.EqualTo(typeof(JsonConfigurationReader)));
    }
    
    
    [Test]
    public void GivenConfigurationRoot_WhenRootDoesNotContainJsonProvider_ThrowsException()
    {
        var configurationRoot = new Mock<IConfigurationRoot>();
        configurationRoot.Setup(x => x.Providers).Returns(() => new List<IConfigurationProvider>());

        var ex = Assert.Throws<ArgumentException>(() => new JsonConfigurationReader(configurationRoot.Object));
        Assert.That(ex?.Message, Is.EqualTo("Json configuration provider is not found in the configuration root provided"));
    }
    
    [Test]
    public void GivenConfigurationReader_WhenConfigurationContainsMainCurrency_ReturnsMainCurrency()
    {
        var configurationRoot = new Mock<IConfigurationRoot>();
        configurationRoot.Setup(x => x.Providers).Returns(() => _jsonConfigurationProviders);
        configurationRoot.Setup(x => x.GetSection("mainCurrency").Value).Returns(() => "DKK");

        var reader = new JsonConfigurationReader(configurationRoot.Object);
        var mainCurrency = reader.GetMainCurrencyOrDefault();
        Assert.That("DKK", Is.EqualTo(mainCurrency));
    }
    
    [Test]
    public void GivenConfigurationReader_WhenConfigurationDoesNotContainMainCurrency_ReturnsNull()
    {
        var configurationRoot = new Mock<IConfigurationRoot>();
        configurationRoot.Setup(x => x.Providers).Returns(() => _jsonConfigurationProviders);
        configurationRoot.Setup(x => x.GetSection("mainCurrency").Value).Returns(() => null);

        var reader = new JsonConfigurationReader(configurationRoot.Object);
        var mainCurrency = reader.GetMainCurrencyOrDefault();
        Assert.IsNull(mainCurrency);
    }
    
    [Test]
    public void GivenConfigurationReader_WhenConfigurationContainsAmountToPurchase_ReturnsAmountToPurchase()
    {
        var configurationRoot = new Mock<IConfigurationRoot>();
        configurationRoot.Setup(x => x.Providers).Returns(() => _jsonConfigurationProviders);
        configurationRoot.Setup(x => x.GetSection("amountToPurchase").Value).Returns(() => "100");

        var reader = new JsonConfigurationReader(configurationRoot.Object);
        var amountToPurchase = reader.GetAmountToPurchaseOrDefault();
        Assert.That(100, Is.EqualTo(amountToPurchase));
    }
    
    [Test]
    public void GivenConfigurationReader_WhenConfigurationDoesNotContainAmountToPurchase_ReturnsZero()
    {
        var configurationRoot = new Mock<IConfigurationRoot>();
        configurationRoot.Setup(x => x.Providers).Returns(() => _jsonConfigurationProviders);
        configurationRoot.Setup(x => x.GetSection("amountToPurchase").Value).Returns(() => null);

        var reader = new JsonConfigurationReader(configurationRoot.Object);
        var amountToPurchase = reader.GetAmountToPurchaseOrDefault();
        Assert.That(amountToPurchase, Is.EqualTo(0));
    }
    
    [Test]
    public void GivenConfigurationReader_WhenConfigurationContainsExchangeRates_ReturnsExchangeRates()
    {
        var configurationRoot = new Mock<IConfigurationRoot>();
        configurationRoot.Setup(x => x.Providers).Returns(() => _jsonConfigurationProviders);

        var eurRate = new Mock<IConfigurationSection>();
        eurRate.Setup(x => x.Key).Returns("EUR");
        eurRate.Setup(x => x.Value).Returns("743.94");
        var exchangeRates = new List<IConfigurationSection> { eurRate.Object };
        
        configurationRoot.Setup(x => x.GetSection("exchangeRates").GetChildren()).Returns(() => exchangeRates);

        var reader = new JsonConfigurationReader(configurationRoot.Object);
        var result = reader.GetExchangeRatesOrDefault();
        
        Assert.That(1, Is.EqualTo(result.Count));
        Assert.That("EUR", Is.EqualTo(result.FirstOrDefault().Key));
        Assert.That(743.94, Is.EqualTo(result.FirstOrDefault().Value));
    }
    
    [Test]
    public void GivenConfigurationReader_WhenConfigurationDoesNotContainExchangeRate_ReturnsEmpty()
    {
        var configurationRoot = new Mock<IConfigurationRoot>();
        configurationRoot.Setup(x => x.Providers).Returns(() => _jsonConfigurationProviders);

        var eurRate = new Mock<IConfigurationSection>();
        eurRate.Setup(x => x.Key).Returns("EUR");
        var exchangeRates = new List<IConfigurationSection> { eurRate.Object };
        
        configurationRoot.Setup(x => x.GetSection("exchangeRates").GetChildren()).Returns(() => exchangeRates);

        var reader = new JsonConfigurationReader(configurationRoot.Object);
        var result = reader.GetExchangeRatesOrDefault();
        
        Assert.That(0, Is.EqualTo(result.Count));
    }
}