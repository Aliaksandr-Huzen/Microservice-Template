using System;

namespace Microservice.Core.Extensions
{
    public interface IPropertyHelper<in TObject>
        where TObject : class
    {
        TProperty Get<TProperty>(TObject obj, string propertyName);

        TProperty Get<TProperty>(TObject obj, Type attributeType);

        string GetPropertyName(Type attributeType);

        void Set<TProperty>(TObject obj, string propertyName, TProperty propertyValue);

        void Set<TProperty>(TObject obj, Type attributeType, TProperty propertyValue);
    }
}