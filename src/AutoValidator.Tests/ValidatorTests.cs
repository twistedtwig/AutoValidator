using System;
using AutoValidator.Impl;
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
            result.Errors["email"].Should().Be("Invalid Email");
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
            result.Errors["test"].Should().Be("test can't be null or empty");
        }

        [Test]

        public void Empty_String_Is_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.NotNullOrEmpty(string.Empty, "test").Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors["test"].Should().Be("test can't be null or empty");
        }

        [Test]

        public void White_Space_String_Is_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.NotNullOrEmpty(" ", "test").Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors["test"].Should().Be("test can't be null or empty");
        }

        [Test]

        public void Using_Same_Propname_Twice_Causes_Error()
        {
            // arrange

            // act
            Action action = () => _subject
                .NotNullOrEmpty(" ", "test")
                .NotNullOrEmpty(" ", "test")
                .Validate();

            // assert
            action.Should().Throw<ArgumentException>().WithMessage("key 'test' has already been used and errored");
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
            result.Errors["test"].Should().Be("test can't be null or empty");
            result.Errors["short"].Should().Be("short should not be longer than 2");
        }

        [Test]

        public void Too_long_string_Max_Length_String_not_valid()
        {
            // arrange
            
            // act
            var result = _subject.MaxLength("test", 3, "value").Validate();

            // assert
            result.Success.Should().BeFalse();
            result.Errors["value"].Should().Be("value should not be longer than 3");
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
            result.Errors["test"].Should().Be("test must be at least 3");
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
            result.Errors["val"].Should().Be("val should be at least 3");
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
    }
}
