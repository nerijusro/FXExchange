using System;

namespace Exchange
{
    class Program
    {
        private const string DefaultConfigPath = "appsettings.json";
        
        static void Main(string[] args)
        {
            var config = new JsonConfiguration(DefaultConfigPath);
            Console.WriteLine();
        }
    }
}