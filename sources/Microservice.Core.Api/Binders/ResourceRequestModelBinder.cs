using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Core.Extensions;
using Microservice.Core.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microservice.Core.Api.Binders
{
    public class ResourceRequestModelBinder : IModelBinder
    {
        private const char ParametersSeporator = ',';

        private const char OrderBySeporator = ' ';

        private static class QueryParameterNames
        {
            public const string Query = "query";

            public const string Select = "select";

            public const string OrderBy = "orderby";

            public const string Page = "page";

            public const string PerPage = "per_page";
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var result = Activator.CreateInstance(bindingContext.ModelType);
            var resourceRequestPropertyHelper = new PropertyHelper(bindingContext.ModelType);

            var pageValueProviderResult = bindingContext.ValueProvider.GetValue(QueryParameterNames.Page);
            if (pageValueProviderResult != ValueProviderResult.None)
            {
                var paging = new Paging
                {
                    Page = Convert.ToInt32(pageValueProviderResult.FirstValue)
                };

                var perPageValueProviderResult = bindingContext.ValueProvider.GetValue(QueryParameterNames.PerPage);
                if (perPageValueProviderResult != ValueProviderResult.None)
                {
                    paging.PerPage = Convert.ToInt32(perPageValueProviderResult.FirstValue);
                }

                resourceRequestPropertyHelper.Set<Paging>(result, nameof(ResourceRequest<object>.Paging), paging);
            }

            var selectValueProviderResult = bindingContext.ValueProvider.GetValue(QueryParameterNames.Select);
            if (selectValueProviderResult != ValueProviderResult.None)
            {
                var selects = selectValueProviderResult.FirstValue
                    .Split(ParametersSeporator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();

                resourceRequestPropertyHelper.Set<ICollection<string>>(result, nameof(ResourceRequest<object>.Select), selects);
            }

            var orderByValueProviderResult = bindingContext.ValueProvider.GetValue(QueryParameterNames.OrderBy);
            if (orderByValueProviderResult != ValueProviderResult.None)
            {
                var orderbyValues = orderByValueProviderResult.FirstValue
                    .Split(ParametersSeporator, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim());

                var orderbys = new List<OrderBy>();
                foreach (var orderbyValue in orderbyValues)
                {
                    var orderbyValueParts = orderbyValue.Split(OrderBySeporator, StringSplitOptions.RemoveEmptyEntries);

                    var orderby = new OrderBy()
                    {
                        PropertyName = orderbyValueParts[0],
                    };

                    if (orderbyValueParts.Length > 1 && Enum.TryParse(orderbyValueParts[1], true, out OrderType orderbyOrderType))
                    {
                        orderby.OrderType = orderbyOrderType;
                    }

                    orderbys.Add(orderby);
                }

                resourceRequestPropertyHelper.Set<ICollection<OrderBy>>(result, nameof(ResourceRequest<object>.OrderBy), orderbys);
            }

            var quereyType = resourceRequestPropertyHelper.GetPropertyType(nameof(ResourceRequest<object>.Query));
            var queryPropertyHelper = new PropertyHelper(quereyType);
            object query = null;

            foreach (var propertyName in queryPropertyHelper.GetPropertiesNames())
            {
                var queryValueProviderResult = bindingContext.ValueProvider.GetValue($"{QueryParameterNames.Query}.{propertyName}");

                if (queryValueProviderResult == ValueProviderResult.None) continue;

                var queryValueType = queryPropertyHelper.GetPropertyType(propertyName);

                object queryValue = null;

                if (queryValueType.IsGenericType && queryValueType.GetGenericTypeDefinition() == typeof(ICollection<>))
                {
                    var queryValueArgumentType = queryValueType.GetGenericArguments()[0];

                    var queryList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(typeArguments: queryValueArgumentType));

                    queryValueProviderResult.FirstValue
                       .Split(ParametersSeporator, StringSplitOptions.RemoveEmptyEntries)
                       .Select(s => Convert.ChangeType(s, queryValueArgumentType))
                       .ToList()
                       .ForEach(v => queryList.Add(v));

                    queryValue = queryList;
                }
                else
                {
                    queryValue = Convert.ChangeType(queryValueProviderResult.FirstValue, queryValueType);
                }

                query = query ?? Activator.CreateInstance(quereyType);

                queryPropertyHelper.Set(query, propertyName, queryValue);
            }

            if (query != null)
            {
                resourceRequestPropertyHelper.Set(result, nameof(ResourceRequest<object>.Query), query);
            }

            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}