using AutoValidator.Impl;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    public class ValidatorFactoryTests
    {
        private ValidatorFactory _subject;

        [SetUp]
        public void Init()
        {
            _subject = new ValidatorFactory();
        }


        [Test]

        public void Return_A_New_Instance_Each_Time()
        {
            // arrange

            // act
            var v1 = _subject.Create();
            var v2 = _subject.Create();

            // assert
            v1.Should().NotBeSameAs(v2);
        }
    }
}
