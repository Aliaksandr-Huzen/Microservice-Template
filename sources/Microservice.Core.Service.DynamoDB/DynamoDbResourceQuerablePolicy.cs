using System;
using Microservice.Core.Extensions;

namespace Microservice.Core.Service.DynamoDb
{
    public class DynamoDbResourceQuerablePolicy<TServiceQuery, TDTO> : DynamoDbResourcePolicy<TDTO>, IDynamoDbResourceQuerablePolicy<TServiceQuery, TDTO> where TServiceQuery : class where TDTO : class
    {
        public DynamoDbResourceQuerablePolicy(IPropertyHelper<TDTO> propertyHelper) 
            : base(propertyHelper)
        {
        }

        public DynamoDbScanParameters Convert(TServiceQuery serviceQuery)
        {
            //TODO : Implement Generic Policy
            throw new NotImplementedException();
        }
    }
}