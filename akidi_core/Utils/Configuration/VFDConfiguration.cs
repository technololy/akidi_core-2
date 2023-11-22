using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace akidi_core.Utils.Configuration
{
    public class VFDConfiguration
    {
        private static VFDConfiguration Vfd;

        public string token { get; set; }
        public string authPath { get; set; }
        public string payIn { get; set; }
        public string payOut { get; set; }
        public string apiKey { get; set; }
        public string credential { get; set; }
        public string baseUrl { get; set; }

        private VFDConfiguration()
        {

        }

        public static VFDConfiguration INSTANCE()
        {
            if (Vfd == null)
            {
                return Vfd = new VFDConfiguration();
            }
            return Vfd;
        }

    }
}
