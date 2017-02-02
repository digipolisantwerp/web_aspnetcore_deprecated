using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Digipolis.Web.Modelbinders
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

            var modelType = context.Metadata.ModelType;
            if (TypeIsSupported(modelType))
            {
                var binderType = typeof(CommaDelimitedArrayModelBinder);
                return (IModelBinder)Activator.CreateInstance(binderType);
            }

            return null;
        }

        internal bool TypeIsSupported(Type modelType)
        {
            var type = modelType.GetElementType() ?? modelType.GetGenericArguments().FirstOrDefault();
            return type != null && (type.GetTypeInfo().IsValueType || type == typeof(string));
        }
    }
}
