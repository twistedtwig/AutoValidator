using System;
using AutoValidator.Impl;
using AutoValidator.Models;
using AutoValidator.Tests.Models;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    public class ValidatorTests
    {
        private Validator _subject;

        [SetUp]
        public void Init()
        {
            _subject = new Validator();
        }

        [Test]
        public void Empty_String_Is_Not_Valid_Email_Address()
        {
            // arrange

            // act
            var result = _subject
                .IsEmailAddress(string.Empty)
                .Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainKey("email");
            var nameErrors = result.Errors["email"];
            nameErrors.Count.Should().Be(1);
            nameErrors.Should().Contain(e => e == "Invalid Email");
        }


        [Test]
        public void Dot_Com_Is_Valid_Email()
        {
            // arrange

            // act
            var result = _subject
                .IsEmailAddress("test@test.com")
                .Validate();

            // assert
            result.Success.Should().BeTrue();
            result.Errors.Keys.Count.Should().Be(0);
        }

        [Test]
        public void Null_String_Is_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.NotNullOrEmpty(null, "test").Validate();

            // assert
            result.Success.Should().BeFalse();

            result.Errors.Should().ContainKey("test");
            var errors = result.Errors["test"];
            errors.Count.Should().Be(1);
            errors.Should().Contain(e => e == "test can't be null or empty");
        }

        [Test]
        public void Empty_String_Is_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.NotNullOrEmpty(string.Empty, "test").Validate();

            // assert
            result.Success.Should().BeFalse();

            result.Errors.Should().ContainKey("test");
            var errors = result.Errors["test"];
            errors.Count.Should().Be(1);
            errors.Should().Contain(e => e == "test can't be null or empty");
        }

        [Test]
        public void White_Space_String_Is_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.NotNullOrEmpty(" ", "test").Validate();

            // assert
            result.Success.Should().BeFalse();

            result.Errors.Should().ContainKey("test");
            var errors = result.Errors["test"];
            errors.Count.Should().Be(1);
            errors.Should().Contain(e => e == "test can't be null or empty");
        }
        
        [Test]
        public void Any_String_Is_Valid()
        {
            // arrange

            // act
            var result = _subject
                .NotNullOrEmpty("123", "test")
                .Validate();

            // assert
            result.Success.Should().BeTrue();
            result.Errors.Keys.Count.Should().Be(0);
        }

        [Test]
        public void Two_Errors_Do_Combine()
        {
            // arrange

            // act
            var result = _subject
                .NotNullOrEmpty(" ", "test")
                .MaxLength("123", 2, "short")
                .Validate();

            // assert
            result.Success.Should().BeFalse();

            result.Errors.Should().ContainKey("test");
            var testErrors = result.Errors["test"];
            testErrors.Count.Should().Be(1);
            testErrors.Should().Contain(e => e == "test can't be null or empty");

            result.Errors.Should().ContainKey("short");
            var errors = result.Errors["short"];
            errors.Count.Should().Be(1);
            errors.Should().Contain(e => e == "short should not be longer than 2");
        }

        [Test]
        public void Too_long_string_Max_Length_String_not_valid()
        {
            // arrange
            
            // act
            var result = _subject.MaxLength("test", 3, "value").Validate();

            // assert
            result.Success.Should().BeFalse();

            result.Errors.Should().ContainKey("value");
            var errors = result.Errors["value"];
            errors.Count.Should().Be(1);
            errors.Should().Contain(e => e == "value should not be longer than 3");
        }

        [Test]
        public void string_Max_Length_String_valid()
        {
            // arrange

            // act
            var result = _subject.MaxLength("test", 4, "value").Validate();

            // assert
            result.Success.Should().BeTrue();
            result.Errors.Keys.Count.Should().Be(0);
        }

        [Test]
        public void Too_Short_string_Min_Length_String_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.MinLength("ab", 3, "test").Validate();

            // assert
            result.Success.Should().BeFalse();

            result.Errors.Should().ContainKey("test");
            var errors = result.Errors["test"];
            errors.Count.Should().Be(1);
            errors.Should().Contain(e => e == "test must be at least 3");
        }

        [Test]
        public void Long_String_Min_Length_String_Is_Valid()
        {
            // arrange

            // act
            var result = _subject.MinLength("abc", 3, "test").Validate();

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Int_Min_Value_Too_Low_Is_Not_Valid()
        {
            // arrange
            
            // act
            var result = _subject.MinValue(2, 3, "val").Validate();

            // assert
            result.Success.Should().BeFalse();

            result.Errors.Should().ContainKey("val");
            var errors = result.Errors["val"];
            errors.Count.Should().Be(1);
            errors.Should().Contain(e => e == "val should be at least 3");
        }

        [Test]
        public void Int_Min_Value_Equal_To_Min_Is_Valid()
        {
            // arrange

            // act
            var result = _subject.MinValue(3, 3, "val").Validate();

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Int_Min_Value_Greater_Than_Min_Is_Valid()
        {
            // arrange

            // act
            var result = _subject.MinValue(4, 3, "val").Validate();

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Multiple_Errors_On_A_Property_Show_In_Validation_Error()
        {
            // arrange
            
            // act
            var result = _subject
                .MinLength("text", 5, "Name")
                .MaxLength("text", 3, "Name")
                .MinValue(10, 5, "Age")
                .NotNullOrEmpty("", "Test")
                .Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Should().ContainKey("Name");
            result.Errors.Should().ContainKey("Test");

            var nameErrors = result.Errors["Name"];
            var testErrors = result.Errors["Test"];

            nameErrors.Count.Should().Be(2);
            nameErrors.Should().Contain(e => e == "Name must be at least 5");
            nameErrors.Should().Contain(e => e == "Name should not be longer than 3");

            testErrors.Count.Should().Be(1);
            testErrors.Should().Contain(e => e == "Test can't be null or empty");
        }

        [Test]
        public void Custom_Expression_is_valid()
        {
            // arrange
            
            // act
            var result = _subject.Custom(123, x => x < 500, "myProp", "{0} cust prop failed").Validate();

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Custom_Expression_is_not_valid()
        {
            // arrange

            // act
            var result = _subject.Custom(123, x => x < 100, "myProp", "{0} cust prop failed").Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors["myProp"].Should().Contain(e => e == "myProp cust prop failed");
        }

        [Test]
        public void Custom_Model1_Expression_is_valid()
        {
            // arrange
            var model = new Model1
            {
                Age = 18,
                Name = "Jon Hawkins"
            };

            // act
            var result = _subject.Custom(model, m => m.Age >= 18 && m.Name.StartsWith("Jon"), "myProp", "{0} cust prop failed").Validate();

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Custom_Model1_Expression_is_not_valid()
        {
            // arrange
            var model = new Model1
            {
                Age = 10,
                Name = "Jack Jones"
            };

            // act
            var result = _subject.Custom(model, m => m.Age >= 18 && m.Name.StartsWith("Jon"), "myProp", "{0} cust prop failed").Validate();

            // assert
            result.Success.Should().BeFalse();
        }

        [Test]
        public void Custom_Model1_Multiple_Expressions_are_valid()
        {
            // arrange
            var model = new Model1
            {
                Age = 18,
                Name = "Jon Hawkins"
            };

            // act
            var result = _subject
                .Custom(model, m => m.Age >= 18, "Age", "must be older than 17")
                .Custom(model, m => m.Age <= 65, "Age", "must be younger than 66")
                .Custom(model, m => !string.IsNullOrWhiteSpace(m.Name), "Name", "{0} should not be null")
                .Validate();

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Email_Test_Can_Display_Incorrect_Email_In_Error_Message()
        {
            // arrange
            var model = new Model2
            {
                EmailAddress = "jon.hawkins"
            };

            // act
            var result = _subject.IsEmailAddress(model.EmailAddress, "Email", "{1} is not a valid email address").Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Keys.Should().Contain("Email");
            result.Errors["Email"].Should().Contain("jon.hawkins is not a valid email address");
        }

        [Test]
        public void Custom_Expression_Can_Use_Value()
        {
            // arrange
            var model = new Model2
            {
                EmailAddress = "jon.hawkins"
            };

            // act
            var result = _subject.Custom(model.EmailAddress, m => m.Length > 555, "Email", "{1} is not long enough").Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Keys.Should().Contain("Email");
            result.Errors["Email"].Should().Contain("jon.hawkins is not long enough");
        }

        [Test]
        public void Custom_Can_Take_Class_And_Use_Member_Expression_And_Not_Need_Prop_Name_Given()
        {
            // arrange
            var model = new Model2
            {
                EmailAddress = "jon.hawkins"
            };

            // act
            var result = _subject.Custom(model, m => m.EmailAddress, email => email.Length > 555, "{1} is not long enough").Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Keys.Should().Contain("EmailAddress");
            result.Errors["EmailAddress"].Should().Contain("jon.hawkins is not long enough");
        }

        [Test]
        public void Custom_Can_Take_Class_And_Use_Member_Expression_And_Not_Need_Prop_Name_Given_Settings_Use_CamelCase_observed()
        {
            // arrange
            var settings = new ValidatorSettings
            {
                UseCamelCase = true
            };

            var model = new Model2
            {
                EmailAddress = "jon.hawkins"
            };

            _subject = new Validator(settings);

            // act
            var result = _subject.Custom(model, m => m.EmailAddress, email => email.Length > 555, "{1} is not long enough").Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors.Keys.Should().Contain("emailAddress");
            result.Errors["emailAddress"].Should().Contain("jon.hawkins is not long enough");
        }
    }
}
