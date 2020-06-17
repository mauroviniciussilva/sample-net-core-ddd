using Sample.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;

namespace Sample.Api.Filter
{
    public class ProviderModelBinder : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(QueryFilter))
                return new BinderTypeModelBinder(typeof(QueryFilterModelBinder));

            return null;
        }
    }
}