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
    public class CommaDelimitedArrayModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var modelTypeInfo = context.Metadata.ModelType.GetTypeInfo();
            if (modelTypeInfo.IsArray)
            {
                var binderType = typeof(CommaDelimitedArrayModelBinder);
                return (IModelBinder)Activator.CreateInstance(binderType);
            }

            //if (modelTypeInfo.IsGenericType &&
            //    modelTypeInfo.GetGenericTypeDefinition().GetTypeInfo() == typeof(KeyValuePair<,>).GetTypeInfo())
            //{
            //    var typeArguments = modelTypeInfo.GenericTypeArguments;

            //    var keyMetadata = context.MetadataProvider.GetMetadataForType(typeArguments[0]);
            //    var keyBinder = context.CreateBinder(keyMetadata);

            //    var valueMetadata = context.MetadataProvider.GetMetadataForType(typeArguments[1]);
            //    var valueBinder = context.CreateBinder(valueMetadata);

            //    var binderType = typeof(KeyValuePairModelBinder<,>).MakeGenericType(typeArguments);
            //    return (IModelBinder)Activator.CreateInstance(binderType, keyBinder, valueBinder);
            //}

            return null;
        }
    }
}
