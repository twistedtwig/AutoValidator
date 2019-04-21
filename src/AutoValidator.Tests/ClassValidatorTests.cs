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

            result.Errors.Should().ContainKey("Name");
            var nameErrors = result.Errors["Name"];
            nameErrors.Count.Should().Be(1);
            nameErrors.Should().Contain(e => e == "Name can't be null or empty");

            result.Errors.Should().ContainKey("Age");
            var ageErrors = result.Errors["Age"];
            ageErrors.Count.Should().Be(1);
            ageErrors.Should().Contain(e => e == "Age should be at least 18");            
        }

        [Test]

        public void Multiple_Errors_On_A_Property_Show_In_Validation_Error()
        {
            // arrange
            var profile = new DuplicateMappingProfile();

            var model = new Model1
            {
                Age = 18,
                Name = "test"
            };

            _validator = new ClassValidator<Model1>(profile.MappingExpressions.OfType<IMappingExpression<Model1>>().Single());


            // act
            var result = _validator.Validate(model);

            // assert
            result.Success.Should().BeFalse();

            result.Errors.Should().ContainKey("Name");
            var nameErrors = result.Errors["Name"];
            nameErrors.Count.Should().Be(2);
            nameErrors.Should().Contain(e => e == "Name must be at least 13");
            nameErrors.Should().Contain(e => e == "Name should not be longer than 3");
        }
    }
}
