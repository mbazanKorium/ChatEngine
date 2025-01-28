using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEngine.Evaluator.Interfaces
{
    public interface IEvaluator
    {
        public Task Evaluate(string prompt);
    }
}
