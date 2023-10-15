using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigService
{
    public interface IConfigProvider
    {
        string GetValue(string name);
    }
}
