using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEngine.Core
{
    public interface IChatGPTService
    {
        Task<string> SendRequestAsync(string prompt, int maxTokens = 100, double temperature = 0.7);
    }
}
