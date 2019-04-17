using System;
using System.Collections.Generic;
using System.Text;
using AutoValidator.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests.Helpers
{
    [TestFixture]
    public class PrimitiveExtensionsTests
    {
        interface Interface
        {
            int Value { get; }
        }

        class DestinationClass : Interface
        {
            int Interface.Value { get { return 123; } }

            public int PrivateProperty { get; private set; }

            public int PublicProperty { get; set; }
        }

        [Test]
        public void Should_find_explicitly_implemented_member()
        {
            PrimitiveHelper.GetFieldOrProperty(typeof(DestinationClass), "Value").Should().NotBeNull();
        }

        [Test]
        public void Should_not_flag_only_enumerable_type_as_writeable_collection()
        {
            PrimitiveHelper.IsListOrDictionaryType(typeof(string)).Should().BeFalse();
        }

        [Test]
        public void Should_flag_list_as_writable_collection()
        {
            PrimitiveHelper.IsListOrDictionaryType(typeof(int[])).Should().BeTrue();
        }

        [Test]
        public void Should_flag_generic_list_as_writeable_collection()
        {
            PrimitiveHelper.IsListOrDictionaryType(typeof(List<int>)).Should().BeTrue();
        }

        [Test]
        public void Should_flag_dictionary_as_writeable_collection()
        {
            PrimitiveHelper.IsListOrDictionaryType(typeof(Dictionary<string, int>)).Should().BeTrue();
        }
    }
}
