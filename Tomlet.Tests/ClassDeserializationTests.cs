﻿using System;
using System.Collections.Generic;
using Tomlet.Tests.TestModelClasses;
using Xunit;

namespace Tomlet.Tests
{
    public class ClassDeserializationTests
    {
        [Fact]
        public void DictionaryDeserializationWorks()
        {
            var dict = TomletMain.To<Dictionary<string, string>>(TestResources.SimplePrimitiveDeserializationTestInput);
            
            Assert.Equal(4, dict.Count);
            Assert.Equal("Hello, world!", dict["MyString"]);
            Assert.Equal("690.42", dict["MyFloat"]);
            Assert.Equal("true", dict["MyBool"]);
            Assert.Equal("1970-01-01T07:00:00", dict["MyDateTime"]);
        }
        
        [Fact]
        public void SimpleCompositeDeserializationWorks()
        {
            var type = TomletMain.To<SimplePrimitiveTestClass>(TestResources.SimplePrimitiveDeserializationTestInput);
            
            Assert.Equal("Hello, world!", type.MyString);
            Assert.True(Math.Abs(690.42 - type.MyFloat) < 0.01);
            Assert.True(type.MyBool);
            Assert.Equal(new DateTime(1970, 1, 1, 7, 0, 0, DateTimeKind.Utc), type.MyDateTime);
        }

        [Fact]
        public void SimplePropertyDeserializationWorks()
        {
            var type = TomletMain.To<SimplePropertyTestClass>(TestResources.SimplePrimitiveDeserializationTestInput);

            Assert.Equal("Hello, world!", type.MyString);
            Assert.True(Math.Abs(690.42 - type.MyFloat) < 0.01);
            Assert.True(type.MyBool);
            Assert.Equal(new DateTime(1970, 1, 1, 7, 0, 0, DateTimeKind.Utc), type.MyDateTime);
        }

        [Fact]
        public void SimpleRecordDeserializationWorks()
        {
            var type = TomletMain.To<SimpleTestRecord>(TestResources.SimplePrimitiveDeserializationTestInput);

            Assert.Equal("Hello, world!", type.MyString);
            Assert.True(Math.Abs(690.42 - type.MyFloat) < 0.01);
            Assert.True(type.MyBool);
            Assert.Equal(new DateTime(1970, 1, 1, 7, 0, 0, DateTimeKind.Utc), type.MyDateTime);
        }

        [Fact]
        public void AnArrayOfEmptyStringsCanBeDeserialized()
        {
            var wrapper = TomletMain.To<StringArrayWrapper>(TestResources.ArrayOfEmptyStringTestInput);
            var array = wrapper.array;
            
            Assert.Equal(5, array.Length);
            Assert.All(array, s => Assert.Equal(string.Empty, s));
        }
    }
}