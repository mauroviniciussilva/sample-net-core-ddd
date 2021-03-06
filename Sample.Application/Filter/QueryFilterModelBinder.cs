﻿using Sample.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Application.Filter
{
    /// <summary>
    /// A model binder for gathering query params from a request
    /// </summary>
    public class QueryFilterModelBinder : IModelBinder
    {
        /// <summary>
        /// Gather the QueryFilter based on the query filters from the request
        /// </summary>
        /// <param name="bindingContext">Binding Context</param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(QueryFilter))
                throw new ArgumentNullException(nameof(bindingContext));

            var queryString = bindingContext.ActionContext.HttpContext.Request.Query.ToDictionary(x => x.Key.ToLower(), x => x.Value);
            var query = new QueryFilter { Limit = GetLimit(queryString), Page = GetPage(queryString) };

            GetFilter(queryString, query);

            bindingContext.Result = ModelBindingResult.Success(query);
            return Task.CompletedTask;
        }

        private static int GetPage(Dictionary<string, StringValues> queryString) => !queryString.ContainsKey("page") ? 0 : Convert.ToInt32(queryString["page"]);

        private static int GetLimit(Dictionary<string, StringValues> queryString) => !queryString.ContainsKey("limit") ? 0 : Convert.ToInt32(queryString["limit"]);

        private static void GetFilter(Dictionary<string, StringValues> queryString, QueryFilter filter)
        {
            queryString.Remove("page");
            queryString.Remove("limit");
            queryString.Remove("_");

            foreach (var item in queryString)
                filter.AddFilter(item.Key, item.Value);
        }
    }
}