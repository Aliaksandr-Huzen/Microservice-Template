using System.Collections.Generic;

namespace Microservice.Core.Model
{
    public class ResourceMultipleResponse<TModel>
        where TModel : class
    {
        public ICollection<TModel> Items { get; set; }
    }
}