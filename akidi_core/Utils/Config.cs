using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace akidi_core.Utils
{
    public  class Config
    {
        public IConfigurationRoot configuration = null;
        public  Config()
        {

                configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Spécifie le chemin du répertoire actuel
            .AddJsonFile("appsettings.json") // Charge le fichier appsettings.json
            .Build();

        }
    }
}
