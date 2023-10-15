using ConfigService;
using System.Collections.Generic;

namespace ConfigReaderService
{
    public class LayeredConfigReader : IConfigReader
    {
        private readonly IEnumerable<IConfigProvider> _configs;
        public LayeredConfigReader(IEnumerable<IConfigProvider> configs)
        {
            _configs = configs;
        }

        public string GetValue(string name)
        {
            string value = null;

            foreach (var config in _configs)
            {
                string newValue = config.GetValue(name);
                if (newValue != null)
                    value = newValue;
            }

            return value;
        }
    }
}
