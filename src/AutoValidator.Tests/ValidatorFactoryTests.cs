using System;
using AutoValidator.Impl;
using AutoValidator.Tests.Models;
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
            var expression = new MapperConfigurationExpression();
            expression.AddProfile<Profile1>();

            _subject = new ValidatorFactory(expression);
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

        [Test]

        public void Create_Factory_Will_Return_A_Valid_Factory()
        {
            // arrange
            var model = new Model1
            {
                Age = 23,
                Name = "Jon Hawkins"
            };

            // act
            var validator = _subject.Create<Model1>();

            // assert
            validator.Should().NotBeNull();
            var validationResult = validator.Validate(model);
            validationResult.Should().NotBeNull();
            validationResult.Success.Should().BeTrue();
        }

        [Test]

        public void Unmapped_Model_Will_Throw_Error()
        {
            // arrange
            var model = new Model2
            {
                Category = "cat1",
                EmailAddress = "email@email.com",
                Number = 999
            };

            // act
            Action action = () => _subject.Create<Model2>();

            // assert
            action.Should().Throw<ArgumentNullException>().WithMessage("unmapped model requested");
        }
    }
}
