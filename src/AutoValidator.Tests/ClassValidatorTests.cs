using System.Linq;
using AutoValidator.Impl;
using AutoValidator.Interfaces;
using AutoValidator.Tests.Models;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    [TestFixture]
    public class ClassValidatorTests
    {
        private ClassValidator<Model1> _validator;

        [SetUp]
        public void Init()
        {
            var profile = new Profile1();

            _validator = new ClassValidator<Model1>(profile.MappingExpressions.OfType<IMappingExpression<Model1>>().Single());
        }

        [Test]

        public void Valid_Object_Will_Return_Correct_Result()
        {
            // arrange
            var model = new Model1
            {
                Name = "Jon Hawkins",
                Age = 21
            };
            
            // act
            var result = _validator.Validate(model);

            // assert
            result.Success.Should().BeTrue();
            result.Errors.Count.Should().Be(0);
        }

        [Test]

        public void Invalid_Object_Will_Return_Errors()
        {
            // arrange
            var model = new Model1
            {
                Name = "",
                Age = 17
            };

            // act
            var result = _validator.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Count.Should().Be(2);
            result.Errors["Name"].Should().Be("Name can't be null or empty");
            result.Errors["Age"].Should().Be("Age should be at least 18");
        }
    }
}
