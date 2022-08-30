using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Exchange.JsonConfiguration;

public class JsonConfigurationBuilder
{
    private const string JsonExtension = ".json";
    private const string InvalidFileNameErrorMessage = "Path provided is not a json file name";

    public IConfigurationRoot BuildJsonConfiguration(string filePath)
    {
        if (Path.GetExtension(filePath).ToLower() != JsonExtension)
        {
            throw new ArgumentException(InvalidFileNameErrorMessage);
        }
        
        var builder = new ConfigurationBuilder();
        builder.Add(new JsonConfigurationSource { Path = filePath });
        return builder.Build();
    }
}