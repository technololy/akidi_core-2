using akidi_core.Utils.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace akidi_core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new Configuration();


            VFDConfiguration.INSTANCE().token = configuration.get("VFDToken");
            VFDConfiguration.INSTANCE().credential = configuration.get("VFDWalletCredential");
            VFDConfiguration.INSTANCE().baseUrl = configuration.get("VFDBaseUrl");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
