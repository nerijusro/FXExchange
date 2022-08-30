using System;
using CurrencyExchange;
using Exchange.JsonConfiguration;

namespace Exchange
{
    internal static class Program
    {
        private const string DefaultConfigPath = "appsettings.json";

        static void Main(string[] args)
        {
            if (!ExchangeArgumentMapper.TryMap(args, out var exchangeRequest))
            {
                Console.WriteLine($"Usage: Exchange {ExchangeArgumentMapper.GetFormat()}");
                return;
            }
            
            var builder = new JsonConfigurationBuilder();
            var configRoot = builder.BuildJsonConfiguration(DefaultConfigPath);
            var configReader = new JsonConfigurationReader(configRoot);
            
            var mainCurrency = configReader.GetMainCurrencyOrDefault();
            if (mainCurrency is null)
            {
                Console.WriteLine("There was an error initialising the application: mainCurrency is not set in the config file.");
                return;
            }
            
            var amountToPurchase = configReader.GetAmountToPurchaseOrDefault();
            var exchangeRates = configReader.GetExchangeRatesOrDefault();

            var exchange = new FxExchange(mainCurrency, amountToPurchase, exchangeRates);
            
            try
            {
                var result = exchange.Convert(exchangeRequest);
                Console.WriteLine(result);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}