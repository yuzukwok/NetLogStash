using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLogStash.Config
{
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException(IConfiguration configuration)
            : base(GetMessage(configuration))
        {
        }

        public InvalidConfigurationException(IConfiguration configuration, Exception innerException)
            : base(GetMessage(configuration), innerException)
        {
        }

        private static string GetMessage(IConfiguration configuration)
        {
            // TODO give better explanation of configuration issue
            return "Invalid configuration";
        }
    }
}
