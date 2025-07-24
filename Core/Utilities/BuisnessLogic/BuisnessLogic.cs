using Core.Utilities.BuisnessLogic.BuisnessLogicResults;
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
        public static async Task<IBuisnessLogicResult> Run(
            Func<StepContext, Task<IBuisnessLogicResult>>[] steps,
            Func<StepContext, IBuisnessLogicResult, Task>? onError = null)
        {
            var context = new StepContext { OnError = onError };

            for (int i = 0; i < steps.Length; i++)
            {
                context.StepIndex = i;
                var result = await steps[i](context);
                context.LastResult = result;

                if (!result.Success)
                {
                    if (onError != null)
                        await onError(context, result);

                    return result;
                }
            }

            return null;
        }
    }
}
