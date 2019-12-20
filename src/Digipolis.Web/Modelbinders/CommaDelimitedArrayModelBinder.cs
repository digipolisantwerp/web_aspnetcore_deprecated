using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Digipolis.Web.Modelbinders
{
    public class CommaDelimitedArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelMetadata.IsEnumerableType)
            {
                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (valueProviderResult == ValueProviderResult.None)
                {
                    return Task.CompletedTask;
                }

                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                var value = valueProviderResult.FirstValue;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        var result = ParseArray(value, bindingContext.ModelType);

                        bindingContext.Model = result;
                        bindingContext.Result = ModelBindingResult.Success(result);
                    }
                    catch (Exception exception)
                    {
                        bindingContext.ModelState.TryAddModelError(
                            bindingContext.ModelName,
                            exception,
                            bindingContext.ModelMetadata);

                        return Task.CompletedTask;
                    }
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

        internal static object ParseArray(string arrayString, Type collectionType)
        {
            if (collectionType.IsArray && (collectionType.GetElementType() == typeof(string) || collectionType.GetElementType().GetTypeInfo().IsValueType))
            {
                var elementType = collectionType.GetElementType();

                var converter = TypeDescriptor.GetConverter(elementType);

                var values = arrayString.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => converter.ConvertFromString(x.Trim()))
                    .ToArray();

                var typedValues = Array.CreateInstance(elementType, values.Length);
                values.CopyTo(typedValues, 0);

                return typedValues;
            }
            else
            {
                if (!collectionType.GetInterfaces()
                    .Any(ti => ti.IsConstructedGenericType
                               && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                               && (ti.GenericTypeArguments[0] == typeof(string) || ti.GenericTypeArguments[0].GetTypeInfo().IsValueType)))
                    throw new NotSupportedException($"Parsing of comma seperated array to {collectionType.FullName} is not supported.");
                var elementType = collectionType.GenericTypeArguments[0];
                var converter = TypeDescriptor.GetConverter(elementType);

                var values = arrayString.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => converter.ConvertFromString(x.Trim()))
                    .ToArray();

                var list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

                foreach (var el in values)
                {
                    list.Add(el);
                }

                return list;
            }
        }
    }
}