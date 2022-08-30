#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Exchange.JsonConfiguration;

public class JsonConfigurationReader
{
    private const string MainCurrencySection = "mainCurrency";
    private const string AmountToPurchaseSection = "amountToPurchase";
    private const string ExchangeRateSection = "exchangeRates";
    private const string JsonProviderNotFound = "Json configuration provider is not found in the configuration root provided";

    private readonly IConfigurationRoot _jsonConfiguration;
    
    public JsonConfigurationReader(IConfigurationRoot configurationRoot)
    {
        if (configurationRoot.Providers.Any(x => x.GetType() == typeof(JsonConfigurationProvider)))
        {
            _jsonConfiguration = configurationRoot;
        }
        else
        {
            throw new ArgumentException(JsonProviderNotFound);
        }
    }

    public string? GetMainCurrencyOrDefault()
    {
        return _jsonConfiguration.GetSection(MainCurrencySection).Value;
    }

    public int GetAmountToPurchaseOrDefault()
    {
        return Convert.ToInt32(_jsonConfiguration.GetSection(AmountToPurchaseSection).Value);
    }

    public Dictionary<string, decimal> GetExchangeRatesOrDefault()
    {
        return _jsonConfiguration.GetSection(ExchangeRateSection).GetChildren()
            .Where(x => x.Value is not null)
            .ToDictionary(x => x.Key, x => decimal.Parse(x.Value!, CultureInfo.InvariantCulture));
    }
}