using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace Microservice.Core.Service.DynamoDb
{
    public class DynamoDbScanParameters
    {
        public IEnumerable<ScanCondition> ScanConditions { get; set; }

        public DynamoDBOperationConfig OperationConfig { get; set; }
    }
}