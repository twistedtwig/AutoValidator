using System.Linq;
using AutoValidator.Impl;
using AutoValidator.Interfaces;
using AutoValidator.Models;
using AutoValidator.Tests.Models;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    [TestFixture]
    public class ClassValidatorTests
    {
        private ClassValidator<Model1> _validator;
        private ValidatorSettings _settings;

        [SetUp]
        public void Init()
        {
            _validator = null;
            _settings = new ValidatorSettings();
        }

        public void setupBasicProfile()
        {
            var profile = new Profile1();
            _validator = new ClassValidator<Model1>(profile.MappingExpressions.OfType<IMappingExpression<Model1>>().Single(), _settings);
        }

        [Test]

        public void Valid_Object_Will_Return_Correct_Result()
        {
            // arrange
            setupBasicProfile();
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
            setupBasicProfile();
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
        public void Invalid_Object_Will_Return_Errors_Settings_Use_CamelCase_observed()
        {
            // arrange
            _settings.UseCamelCase = true;

            setupBasicProfile();
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

            result.Errors.Should().ContainKey("name");
            var nameErrors = result.Errors["name"];
            nameErrors.Count.Should().Be(1);
            nameErrors.Should().Contain(e => e == "Name can't be null or empty");

            result.Errors.Should().ContainKey("age");
            var ageErrors = result.Errors["age"];
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

            _validator = new ClassValidator<Model1>(profile.MappingExpressions.OfType<IMappingExpression<Model1>>().Single(), _settings);


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

        [Test]

        public void Profile_Passing_With_Whole_Object_Valid_data_Success()
        {
            // arrange
            var profile = new ProfileWithWholeObject();
            var model = new Model1
            {
                Age = 5,
                Name = "Jon"
            };

            _validator = new ClassValidator<Model1>(profile.MappingExpressions.OfType<IMappingExpression<Model1>>().Single(), _settings);
            
            // act
            var result = _validator.Validate(model);

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Profile_Passing_With_Whole_Object_Invalid_data_Failure()
        {
            // arrange
            var profile = new ProfileWithWholeObject();
            var model = new Model1
            {
                Age = 5,
                Name = "Jon hawkins"
            };

            _validator = new ClassValidator<Model1>(profile.MappingExpressions.OfType<IMappingExpression<Model1>>().Single(), _settings);

            // act
            var result = _validator.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainKey("Age");
            var nameErrors = result.Errors["Age"];
            nameErrors.Count.Should().Be(1);
            nameErrors.Should().Contain(e => e == "Age should be at least 11");
        }

        [Test]
        public void Profile_Alt_Passing_With_Whole_Object_valid_Age_Success()
        {
            // arrange
            var profile = new ProfileWithWholeObjectAgeToStringEqualLengthOfName();
            var model = new Model1
            {
                Age = 5,
                Name = "Jon h"
            };

            _validator = new ClassValidator<Model1>(profile.MappingExpressions.OfType<IMappingExpression<Model1>>().Single(), _settings);

            // act
            var result = _validator.Validate(model);

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Profile_Alt_Passing_With_Whole_Object_Invalid_age_Failure()
        {
            // arrange
            var profile = new ProfileWithWholeObjectAgeToStringEqualLengthOfName();
            var model = new Model1
            {
                Age = 44,
                Name = "Jon H"
            };

            _validator = new ClassValidator<Model1>(profile.MappingExpressions.OfType<IMappingExpression<Model1>>().Single(), _settings);

            // act
            var result = _validator.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainKey("Age");
            var nameErrors = result.Errors["Age"];
            nameErrors.Count.Should().Be(1);
            nameErrors.Should().Contain(e => e == "Age should be at least 5");
        }

        [Test]
        public void Profile_With_Expression_That_Calls_A_Method_Gets_Correct_Error_Message()
        {
            // arrange
            var profile = new ProfileWithWholeObject();
            var model = new Model2
            {
                Number = 3,
                EmailAddress = "jonathan.hawkins@test.com", // index of the @ is greater than the number should fail
            };

            var validator = new ClassValidator<Model2>(profile.MappingExpressions.OfType<IMappingExpression<Model2>>().Single(), _settings);

            // act
            var result = validator.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainKey("Number");
            var numErrors = result.Errors["Number"];
            numErrors.Count.Should().Be(1);
            numErrors.Should().Contain(e => e == "Number should be at least 16");
        }

        [Test]

        public void Using_Invalid_Email_Can_Be_Displayed_In_Error_Message()
        {
            // arrange
            var profile = new EmailCustomErrorMessageWithValue();

            var model = new Model2
            {
                EmailAddress = "jon.hawkins"
            };

            var validator = new ClassValidator<Model2>(profile.MappingExpressions.OfType<IMappingExpression<Model2>>().Single(), _settings);
            
            // act
            var result = validator.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainKey("EmailAddress");
            result.Errors["EmailAddress"].Should().Contain("jon.hawkins is not a valid email address");
        }

        [Test]

        public void Null_Checks_Catch_Errors()
        {
            // arrange
            var profile = new NullItemMappingProfile();

            var model = new NullableModel();

            var validator = new ClassValidator<NullableModel>(profile.MappingExpressions.OfType<IMappingExpression<NullableModel>>().Single(), _settings);

            // act
            var result = validator.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainKey("EmailAddress");
            result.Errors["EmailAddress"].Should().Contain("EmailAddress Is Null");

            result.Errors.Should().ContainKey("AreYouHappy");
            result.Errors["AreYouHappy"].Should().Contain("AreYouHappy Is Null");

            result.Errors.Should().ContainKey("Number");
            result.Errors["Number"].Should().Contain("Number Is Null");
        }

        [Test]

        public void Null_Checks_With_Values_Pass()
        {
            // arrange
            var profile = new NullItemMappingProfile();

            var model = new NullableModel
            {
                Model1 = new Model1(),
                EmailAddress = "e",
                Number = 22,
                AreYouHappy = true
            };

            var validator = new ClassValidator<NullableModel>(profile.MappingExpressions.OfType<IMappingExpression<NullableModel>>().Single(), _settings);

            // act
            var result = validator.Validate(model);

            // assert
            result.Success.Should().BeTrue();
        }
    }
}
