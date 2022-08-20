using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Exchange;

public class JsonConfiguration
{
    private const string JsonExtension = ".json";
    private const string InvalidFileNameErrorMessage = "Path provided is not a json file name";

    private readonly IConfigurationRoot _jsonConfiguration;
    
    public JsonConfiguration(string filePath)
    {
        if (Path.GetExtension(filePath).ToLower() != JsonExtension)
        {
            throw new ArgumentException(InvalidFileNameErrorMessage);
        }
        
        var builder = new ConfigurationBuilder();
        builder.Add(new JsonConfigurationSource { Path = filePath });
        _jsonConfiguration = builder.Build();
    }

    public string? GetDefaultInputFormatOrDefault()
    {
        return _jsonConfiguration.GetSection("defaultInputFormat").Value;
    }

    public string? GetMainCurrencyOrDefault()
    {
        return _jsonConfiguration.GetSection("mainCurrency").Value;
    }

    public string? GetAmountToPurchaseOrDefault()
    {
        return _jsonConfiguration.GetSection("amountToPurchase").Value;
    }

    public Dictionary<string, string> GetExchangeRatesOrDefault()
    {
        return _jsonConfiguration.GetSection("exchangeRates").GetChildren()
            .ToDictionary(x => x.Key, x => x.Value);
    }
}