using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.BuisnessLogic.BuisnessLogicResults
{
    public class StepContext
    {
        public int StepIndex { get; set; }
        public IBuisnessLogicResult? LastResult { get; set; }
        public Dictionary<string, object> Items { get; set; } = new();
        public Func<StepContext, IBuisnessLogicResult, Task>? OnError { get; set; }
    }
}
