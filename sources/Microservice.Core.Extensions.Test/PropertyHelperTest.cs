using System;
using NUnit.Framework;

namespace Microservice.Core.Extensions.Test
{
    [TestFixture]
    public class PropertyHelperTest
    {
        private class TestKeyAttribute : Attribute
        {
        }

        private class TestObject
        {
            [TestKey]
            public int IntProperty { get; set; }

            public string StringProperty { get; set; }
        }

        private int _intProperty;

        private string _stringProperty;

        private PropertyHelper _propertyHelper;

        [OneTimeSetUp]
        public void Setup()
        {
            _propertyHelper = new PropertyHelper(typeof(TestObject));

            _intProperty = 10;
            _stringProperty = "Test Data";
        }

        [Test]
        public void GetByPropertyNameTest()
        {
            var testObject = new TestObject()
            {
                IntProperty = _intProperty,
                StringProperty = _stringProperty
            };

            var intPropertyActual = _propertyHelper.Get<int>(testObject, nameof(TestObject.IntProperty));

            Assert.AreEqual(_intProperty, intPropertyActual);
        }

        [Test]
        public void GetByAttributeTest()
        {
            var testObject = new TestObject()
            {
                IntProperty = _intProperty,
                StringProperty = _stringProperty
            };

            var intPropertyActual = _propertyHelper.Get<int>(testObject, typeof(TestKeyAttribute));

            Assert.AreEqual(_intProperty, intPropertyActual);
        }

        [Test]
        public void SetTest()
        {
            var testObject = new TestObject();

            _propertyHelper.Set<string>(testObject, nameof(TestObject.StringProperty), _stringProperty);

            Assert.AreEqual(_stringProperty, testObject.StringProperty);
        }
    }
}
