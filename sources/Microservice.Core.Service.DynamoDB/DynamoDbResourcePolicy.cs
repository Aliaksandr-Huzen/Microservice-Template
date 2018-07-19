using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microservice.Core.Extensions;

namespace Microservice.Core.Service.DynamoDb
{
    public class DynamoDbResourcePolicy<TDTO> : IDynamoDbResourcePolicy<TDTO> where TDTO : class
    {
        protected readonly IPropertyHelper<TDTO> PropertyHelper;

        public DynamoDbResourcePolicy(IPropertyHelper<TDTO> propertyHelper)
        {
            PropertyHelper = propertyHelper;
        }

        public string GetKey(TDTO dto)
        {
            return PropertyHelper.Get<string>(dto, typeof(DynamoDBHashKeyAttribute));
        }
        public ExpectedState GetExpectedState(bool exists, TDTO dto)
        {
            return new ExpectedState
            {
                ExpectedValues = new Dictionary<string, ExpectedValue>()
                {
                    {
                        PropertyHelper.GetPropertyName(typeof(DynamoDBHashKeyAttribute)),
                        new ExpectedValue(exists) { Values = { new Primitive(PropertyHelper.Get<string>(dto, typeof(DynamoDBHashKeyAttribute)))}}
                    }
                },
                
            };
        }
    }
}