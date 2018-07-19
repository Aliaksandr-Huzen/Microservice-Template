namespace Microservice.Core.Model
{
    public class ResourceSingleResponse<TModel>
        where TModel : class

    {
        public TModel Item { get; set; }
    }
}