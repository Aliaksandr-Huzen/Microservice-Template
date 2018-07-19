using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2.DataModel;

namespace Microservice.Core.Service.DynamoDb.Extensions
{
    public static class EntitiesExtension
    {
        public static string GetEntityTableName(this Type entityType, string tablePrefix)
        {
            var dynamoDbTableAttribute = (DynamoDBTableAttribute)Attribute.GetCustomAttribute(entityType, typeof(DynamoDBTableAttribute));

            if (dynamoDbTableAttribute == null)
            {
                throw new ApplicationException($"{nameof(DynamoDBTableAttribute)} is not found");
            }

            return $"{tablePrefix}{dynamoDbTableAttribute.TableName}";
        }

        public static bool IsHasDynamoDbTableAttribute(this Type entityType)
        {
            return Attribute.IsDefined(entityType, typeof(DynamoDBTableAttribute));
        }

        public static IEnumerable<Type> GetAllEntitiesTypes(this AppDomain appDomain)
        {
            return appDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsHasDynamoDbTableAttribute());
        }

        public static IEnumerable<Type> GetAllEntitiesTypes(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsHasDynamoDbTableAttribute());
        }


    }
}