using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEngine.Core.Models
{
    public class SemanticKernelOptions
    {
        public string ModelId { get; set; } = "gpt-4";
        public int MaxTokens { get; set; } = 100;
        public double Temperature { get; set; } = 0.7;
    }

}
