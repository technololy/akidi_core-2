using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace akidi_core.Utils.Configuration
{
    public class Configuration
    {
        ConfigurationBuilder configurationBuilder;
        IConfigurationRoot Config;

        public Configuration()
        {
            configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");

        }

        public string get(string keyName)
        {
            Config = configurationBuilder.Build();
            return Config[keyName];

        }
    }
}
