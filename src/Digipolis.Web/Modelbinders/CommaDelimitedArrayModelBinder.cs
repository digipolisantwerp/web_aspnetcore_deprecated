using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Digipolis.Web.UnitTests.Modelbinders
{
    public class CommaDelimitedArrayModelBinder :  IModelBinder
    {
        Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
        {

            if (bindingContext.ModelMetadata.IsEnumerableType)
            {
                if (bindingContext == null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }

                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (valueProviderResult == ValueProviderResult.None)
                {
                    return Task.CompletedTask;
                }

                //bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                var key = bindingContext.ModelName;
                var value = bindingContext.ValueProvider.GetValue(key).ToString();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    var elementType = bindingContext.ModelType.GetTypeInfo().GetElementType();
                    //TODO : omgaan met zowel generic collections als arrays
                    //var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
                    var converter = TypeDescriptor.GetConverter(elementType);

                    var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => converter.ConvertFromString(x.Trim()))
                        .ToArray();

                    var typedValues = Array.CreateInstance(elementType, values.Length);
                    values.CopyTo(typedValues, 0);

                    bindingContext.Model = typedValues;
                    bindingContext.Result = ModelBindingResult.Success(typedValues);
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Success(Array.CreateInstance(bindingContext.ModelType.GetElementType(), 0));
                }
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }


            return Task.CompletedTask;
        }
    }
}
