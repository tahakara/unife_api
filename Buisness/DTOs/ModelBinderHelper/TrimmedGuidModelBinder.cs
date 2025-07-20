using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.DTOs.ModelBinderHelper
{
    public class TrimmedGuidModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (Guid.TryParse(value?.Trim(), out Guid parsedGuid))
            {
                bindingContext.Result = ModelBindingResult.Success(parsedGuid);
            }
            else
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Geçersiz GUID formatı.");
            }

            return Task.CompletedTask;
        }
    }
}
