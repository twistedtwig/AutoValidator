using AutoValidator.Impl;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    public class ClassValidatorExpressionTests
    {
        private ClassValidatorExpression _subject;

        [SetUp]
        public void Init()
        {
            _subject = new ClassValidatorExpression();
        }

        [Test]

        public void Empty_String_Is_Not_Valid_Email_Address()
        {
            // arrange

            // act
            var result = _subject.IsEmailAddress(string.Empty);

            // assert
            result.Should().BeFalse();
        }


        [Test]

        public void Dot_Com_Is_Valid_Email()
        {
            // arrange

            // act
            var result = _subject.IsEmailAddress("test@test.com");

            // assert
            result.Should().BeTrue();
        }

        [Test]

        public void Null_String_Is_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.NotNullOrEmpty(null);

            // assert
            result.Should().BeFalse();
        }

        [Test]

        public void Empty_String_Is_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.NotNullOrEmpty(string.Empty);

            // assert
            result.Should().BeFalse();
        }

        [Test]

        public void White_Space_String_Is_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.NotNullOrEmpty(" ");

            // assert
            result.Should().BeFalse();
        }

        [Test]

        public void Any_String_Is_Valid()
        {
            // arrange

            // act
            var result = _subject.NotNullOrEmpty("123");

            // assert
            result.Should().BeTrue();
        }

        [Test]

        public void Too_long_string_Max_Length_String_not_valid()
        {
            // arrange
            
            // act
            var result = _subject.MaxLength("test", 3);

            // assert
            result.Should().BeFalse();
        }

        [Test]

        public void Too_Short_string_Min_Length_String_Not_Valid()
        {
            // arrange

            // act
            var result = _subject.MinLength("ab", 3);

            // assert
            result.Should().BeFalse();
        }

        [Test]

        public void Long_String_Min_Length_String_Is_Valid()
        {
            // arrange

            // act
            var result = _subject.MinLength("abc", 3);

            // assert
            result.Should().BeTrue();
        }

        [Test]

        public void Int_Min_Value_Too_Low_Is_Not_Valid()
        {
            // arrange
            
            // act
            var result = _subject.MinValue(2, 3);

            // assert
            result.Should().BeFalse();
        }

        [Test]

        public void Int_Min_Value_Equal_To_Min_Is_Valid()
        {
            // arrange

            // act
            var result = _subject.MinValue(3, 3);

            // assert
            result.Should().BeTrue();
        }

        [Test]

        public void Int_Min_Value_Greater_Than_Min_Is_Valid()
        {
            // arrange

            // act
            var result = _subject.MinValue(4, 3);

            // assert
            result.Should().BeTrue();
        }

        [Test]

        public void Ignore_Will_Always_Return_True()
        {
            // arrange
            
            // act
            var result = _subject.Ignore();

            // assert
            result.Should().BeTrue();
        }
    }
}
