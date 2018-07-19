using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Microservice.Core.Extensions
{
    public class PropertyHelper<TObject> : IPropertyHelper<TObject> where TObject : class
    {
        private readonly PropertyHelper _genericPropertyHelper;

        public PropertyHelper()
        {
            _genericPropertyHelper = new PropertyHelper(typeof(TObject));
        }

        public TProperty Get<TProperty>(TObject obj, string propertyName)
        {
            return _genericPropertyHelper.Get<TProperty>(obj, propertyName);
        }

        public TProperty Get<TProperty>(TObject obj, Type attributeType)
        {
            return _genericPropertyHelper.Get<TProperty>(obj, attributeType);
        }

        public string GetPropertyName(Type attributeType)
        {
            return _genericPropertyHelper.GetPropertyName(attributeType);
        }

        public void Set<TProperty>(TObject obj, string propertyName, TProperty propertyValue)
        {
            _genericPropertyHelper.Set(obj, propertyName, propertyValue);
        }

        public void Set<TProperty>(TObject obj, Type attributeType, TProperty propertyValue)
        {
            _genericPropertyHelper.Set(obj, attributeType, propertyValue);
        }
    }

    public class PropertyHelper
    {
        private static readonly ConcurrentDictionary<Type, Tuple<IDictionary<string, Func<object, object>>, IDictionary<string, Action<object, object>>, IDictionary<string, Type>, IDictionary<Type, string>>> GettersSettersCache =
            new ConcurrentDictionary<Type, Tuple<IDictionary<string, Func<object, object>>, IDictionary<string, Action<object, object>>, IDictionary<string, Type>, IDictionary<Type, string>>>();

        private readonly Tuple<IDictionary<string, Func<object, object>>, IDictionary<string, Action<object, object>>, IDictionary<string, Type>, IDictionary<Type, string>> _gettersSetters;

        public PropertyHelper(Type type)
        {
            _gettersSetters = GettersSettersCache.GetOrAdd(type, GenerateGettersSetters);
        }

        public TProperty Get<TProperty>(object obj, string proprtyName)
        {
            return (TProperty)Get(obj, proprtyName);
        }

        public object Get(object obj, string proprtyName)
        {
            return _gettersSetters.Item1[proprtyName](obj);
        }

        public TProperty Get<TProperty>(object obj, Type attributeType)
        {
            return Get<TProperty>(obj, _gettersSetters.Item4[attributeType]);
        }

        public string GetPropertyName(Type attributeType)
        {
            return _gettersSetters.Item4[attributeType];
        }

        public Type GetPropertyType(string propertyName)
        {
            return _gettersSetters.Item3[propertyName];
        }

        public ICollection<string> GetPropertiesNames()
        {
            return _gettersSetters.Item1.Keys;
        }

        public void Set<TProperty>(object obj, string propertyName, TProperty propertyValue)
        {
            Set(obj, propertyName, (object)propertyValue);
        }

        public void Set(object obj, string propertyName, object propertyValue)
        {
            _gettersSetters.Item2[propertyName](obj, propertyValue);
        }

        public void Set<TProperty>(object obj, Type attributeType, TProperty propertyValue)
        {
            Set<TProperty>(obj, _gettersSetters.Item4[attributeType], propertyValue);
        }

        private static Tuple<IDictionary<string, Func<object, object>>, IDictionary<string, Action<object, object>>, IDictionary<string, Type>, IDictionary<Type, string>> GenerateGettersSetters(Type type)
        {
            IDictionary<string, Func<object, object>> getters = new Dictionary<string, Func<object, object>>();
            IDictionary<string, Action<object, object>> setters = new Dictionary<string, Action<object, object>>();
            IDictionary<Type, string> attributePropertyNames = new Dictionary<Type, string>();
            IDictionary<string, Type> propertyTypes = new Dictionary<string, Type>();

            Array.ForEach(
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public),
                p =>
                {
                    if (!p.CanWrite || !p.CanRead) return;

                    propertyTypes.Add(p.Name, p.PropertyType);

                    var getter = GetValueGetter(type, p);
                    var setter = GetValueSetter(type, p);

                    getters.Add(p.Name, getter);
                    setters.Add(p.Name, setter);

                    Array.ForEach(
                        p.GetCustomAttributes(false),
                        a =>
                        {
                            attributePropertyNames.Add(a.GetType(), p.Name);
                        }
                    );
                });

            return new Tuple<IDictionary<string, Func<object, object>>, IDictionary<string, Action<object, object>>, IDictionary<string, Type>, IDictionary<Type, string>>(getters, setters, propertyTypes, attributePropertyNames);
        }

        private static Func<object, object> GetValueGetter(Type type, PropertyInfo propertyInfo)
        {
            var wrappedObjectParameter = Expression.Parameter(typeof(object));

            return Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Property(
                            Expression.Convert(wrappedObjectParameter, type),
                            propertyInfo),
                        typeof(object)),
                    wrappedObjectParameter)
                   .Compile();
        }

        private static Action<object, object> GetValueSetter(Type type, PropertyInfo propertyInfo)
        {
            var wrappedObjectParameter = Expression.Parameter(typeof(object));
            var valueParameter = Expression.Parameter(typeof(object));

            return Expression.Lambda<Action<object, object>>(
                    Expression.Assign(
                        Expression.Property(Expression.Convert(wrappedObjectParameter, type), propertyInfo),
                        Expression.Convert(valueParameter, propertyInfo.PropertyType)),
                    wrappedObjectParameter,
                    valueParameter)
                   .Compile();
        }
    }


}