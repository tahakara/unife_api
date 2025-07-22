using Core.Utilities.BuisnessLogic.BuisnessLogicResults.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.BuisnessLogic
{
    public sealed class BuisnessLogic
    {
        public static async Task<IBuisnessLogicResult> Run(params Func<Task<IBuisnessLogicResult>>[] steps)
        {
            foreach (var step in steps)
            {
                var result = await step();
                if (!result.Success)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
