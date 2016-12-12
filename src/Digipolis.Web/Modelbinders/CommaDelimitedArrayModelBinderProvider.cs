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
            if (TypeIsSupported(modelTypeInfo))
            {
                var binderType = typeof(CommaDelimitedArrayModelBinder);
                return (IModelBinder)Activator.CreateInstance(binderType);
            }

            return null;
        }

        internal bool TypeIsSupported(TypeInfo modelTypeInfo)
        {
            return (modelTypeInfo.IsArray && modelTypeInfo.GetElementType().GetTypeInfo().IsValueType)
                || (modelTypeInfo.GetInterfaces()
                    .Any(ti => ti.IsConstructedGenericType
                     && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                     && (ti.GenericTypeArguments[0].GetTypeInfo().IsValueType || ti.GenericTypeArguments[0] == typeof(string))));
        }
    }
}
