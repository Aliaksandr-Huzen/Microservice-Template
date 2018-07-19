using System.Collections.Generic;

namespace Microservice.Core.Model
{
    public class ResourceRequest<TQuery> 
        where TQuery : class 
    {
        public TQuery Query { get; set; }

        public Paging Paging { get; set; }

        public ICollection<string> Select { get; set; }

        public ICollection<OrderBy> OrderBy { get; set; }
    }
}