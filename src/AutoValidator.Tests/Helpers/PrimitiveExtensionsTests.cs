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
        }

        [Test]
        public void Should_find_explicitly_implemented_member()
        {
            PrimitiveHelper.GetFieldOrProperty(typeof(DestinationClass), "Value").Should().NotBeNull();
        }        
    }
}
