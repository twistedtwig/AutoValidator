using System;
using AutoValidator.Impl;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    [TestFixture]
    public class ValidationExpressionErrorMessageFactoryTests
    {
        private ValidationExpressionErrorMessageFactory _factory;

        [SetUp]
        public void Init()
        {
            _factory = new ValidationExpressionErrorMessageFactory();
        }


        [Test]

        public void Get_IValidatorExpression_Ignore()
        {
            // arrange
            Action action = () => _factory.Get<bool>((x, y) => y.Ignore(), "");
            // act

            // assert
            action.Should().NotThrow();
        }

        [Test]

        public void Get_IValidatorExpression_IsEmailAddress()
        {
            // arrange
            Action action = () => _factory.Get<bool>((x, y) => y.IsEmailAddress("email", null), "");
            // act

            // assert
            action.Should().NotThrow();
        }

        [Test]

        public void Get_IValidatorExpression_NotNullOrEmpty()
        {
            // arrange
            Action action = () => _factory.Get<bool>((x, y) => y.NotNullOrEmpty("email", null), "");
            // act

            // assert
            action.Should().NotThrow();
        }

        [Test]

        public void Get_IValidatorExpression_MinLength()
        {
            // arrange
            Action action = () => _factory.Get<bool>((x, y) => y.MinLength("email", 3, null), "");
            // act

            // assert
            action.Should().NotThrow();
        }

        [Test]

        public void Get_IValidatorExpression_MaxLength()
        {
            // arrange
            Action action = () => _factory.Get<bool>((x, y) => y.MaxLength("email", 3, null), "");
            // act

            // assert
            action.Should().NotThrow();
        }

        [Test]

        public void Get_IValidatorExpression_MinValue()
        {
            // arrange
            Action action = () => _factory.Get<int>((x, y) => y.MinValue(2, 3, null), "");
            // act

            // assert
            action.Should().NotThrow();
        }
    }
}
