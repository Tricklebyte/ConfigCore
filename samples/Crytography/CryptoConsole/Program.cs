
using ConfigCore.Cryptography;
using ConfigCore.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConfigCore.CryptoConsole
{
    public class Program
    {
        private static IConfiguration config;
        private static ICryptoHelper crypto;

        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var serviceCollection = new ServiceCollection();

            // build a local configuration that contains the plain-text encryption configuration settings.
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            // use the settings in the initial configuration to initialize Microsoft DPAPI
            // This can also be done in Startup.ConfigureServices using the standard Configuration from DI
            serviceCollection.AddDataProtectionServices(config);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            crypto = serviceProvider.GetRequiredService<ICryptoHelper>();


            // create an instance of StringEncryptor using the service provider and Encryption Prefix string
            var instance = ActivatorUtilities.CreateInstance<StringEncryptor>(serviceProvider, config["ConfigOptions:Cryptography:EncValPrefix"], crypto);
            instance.UserLoop();

        }
    }
    public class StringEncryptor
    {
        readonly ICryptoHelper _crypto   ;
        readonly string _prefix;
        public StringEncryptor(string encPrefix, ICryptoHelper crypto)
        {
            _prefix = encPrefix;
            _crypto = crypto;
        }
        internal void UserLoop()
        {
            string[] options;
            string optionString = null;
            string _encValPrefix = _prefix;


        Start:
            while (true)
            {
                string message =
        "\n-- Enter [E] to Encrypt a configuration setting\n"
        + "-- Enter [D] to Secrypt a configuration setting\n"
        + "-- Enter [X] to Exit\n\n";
                options = new string[] { "E", "D", "X" };

                string inputMain = GetUserChoice(message, options).ToUpper();
                if (inputMain == "X")
                {
                    return;
                }

                // if we are still here, then we wil either encrypt or decrypt a value
                // Ask the user to enter the name of the setting
                if (inputMain == "E")
                {
                    optionString = "ENCRYPT";

                }
                if (inputMain == "D")
                {
                    optionString = "DECRYPT";
                }

                if (_crypto != null)
                {
                    message = $"Enter the full key of the setting to be {optionString}ED";

                    //Get User Confirmation
                    string inputName = GetUserInput(message);

                    message = $"Enter the value to be {optionString}ED";

                    string inputValue = GetUserInput(message);

                    try
                    {
                        if (optionString == "ENCRYPT")
                        {
                            string encVal = _crypto.Protect(inputName, inputValue, _encValPrefix);
                            message = $"\nOperation Success!\n\nSettingName: {inputName}\nDecrypted Value:{inputValue}\nEncrypted Value:\n{encVal}";
                        }
                        if (optionString == "DECRYPT")
                        {
                            string plainVal = _crypto.Unprotect(inputName, inputValue, _encValPrefix);
                            message = $"\nOperation Success.\nSettingName: '{inputName}'\nEncrypted Value:'\n{inputValue}'\nDecrypted Value: '{plainVal}'";
                        }
                    }
                    catch (Exception e)
                    {
                        message = e.Message;
                    }
                    Console.WriteLine(message);
                }
                else
                {
                    throw new Exception("Cryptography services not initialized. Check configuration section 'ConfigOptions:Cryptography'");

                }

                goto Start;

            }

        }
        internal string GetUserInput(string message)
        {
            string input;
            while (true)
            {
                Console.WriteLine(message);
                input = Console.ReadLine();
                if (String.IsNullOrEmpty(input))
                    continue;
                else
                    return input;
            }
        }

        internal string GetUserChoice(string message, string[] options)
        {
            string input;
            while (true)
            {
                Console.WriteLine(message);
                input = Console.ReadLine().ToUpper();
                if (String.IsNullOrEmpty(input))
                    continue;
                else
                {
                    foreach (string option in options)
                    {
                        if (input.ToUpper().Contains(option))
                        {
                            return input;
                        }
                        else
                            continue;
                    }
                }
            }

        }


    }

}
