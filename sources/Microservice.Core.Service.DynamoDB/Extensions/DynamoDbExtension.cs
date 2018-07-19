using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace Microservice.Core.Service.DynamoDb.Extensions
{
    public static class DynamoDbExtension
    {
        public static Task<string> CreateTableIfNotExistAsync<T>(this IAmazonDynamoDB client, string tablePrefix)
            where T : class
        {
            return client.CreateTableIfNotExistAsync(typeof(T), tablePrefix);
        }

        public static async Task<string> CreateTableIfNotExistAsync(this IAmazonDynamoDB client, Type entityType, string tablePrefix)
        {
            var tableName = entityType.GetEntityTableName(tablePrefix);

            var tableResponse = await client.ListTablesAsync();
            if (!tableResponse.TableNames.Contains(tableName))
            {
                var hashKeyProperty = entityType.GetProperties()
                    .SingleOrDefault(prop => Attribute.IsDefined(prop, typeof(DynamoDBHashKeyAttribute)));

                if (hashKeyProperty == null)
                {
                    throw new ApplicationException($"A property with {nameof(DynamoDBHashKeyAttribute)} is not found");
                }

                try
                {
                    await client.CreateTableAsync(new CreateTableRequest
                    {
                        TableName = tableName,

                        // Move to config
                        ProvisionedThroughput = new ProvisionedThroughput
                        {
                            ReadCapacityUnits = 5,
                            WriteCapacityUnits = 5
                        },
                        KeySchema = new List<KeySchemaElement>
                        {
                            new KeySchemaElement
                            {
                                AttributeName = hashKeyProperty.Name,
                                KeyType = KeyType.HASH
                            }
                        },
                        AttributeDefinitions = new List<AttributeDefinition>
                        {
                            new AttributeDefinition
                            {
                                AttributeName = hashKeyProperty.Name,
                                AttributeType = ScalarAttributeType.S
                            }
                        }
                    });

                    var isTableAvailable = false;
                    while (!isTableAvailable)
                    {
                        Thread.Sleep(1000);
                        var tableStatus = await client.DescribeTableAsync(tableName);
                        isTableAvailable = tableStatus.Table.TableStatus == TableStatus.ACTIVE;
                    }
                }
                catch (TableAlreadyExistsException)
                {
                }
            }

            return tableName;
        }
    }
}