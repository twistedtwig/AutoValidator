using System;
using System.Linq.Expressions;
using AutoValidator.Impl;
using AutoValidator.Interfaces;
using AutoValidator.Tests.Models;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    [TestFixture]
    public class ObjectValidatorErrorMessageFactoryTests
    {
//        [Test]
//        public void Get_IValidatorExpression_IsEmailAddress()
//        {
//            // arrange
//            var factory = new ObjectValidatorErrorMessageFactory<Model3, string>();
//            factory.SetPropName("email");
//            factory.SetErrorFormat("{0} this is bad {1}");
//            Expression<Func<string, bool>> exp = (email) => email.Length > 5;
//            factory.SetupExpression(exp);
//
//            var model = new Model3 { EmailAddress = "jon@email.com" };
//
//            // act
//            var result = factory.Invoke(model);
//
//            // assert
//            result.Item1.Should().Be("lets see");
//            result.Item2.Count.Should().Be(2);
//        }

//        [Test]
//
//        public void Get_IValidatorExpression_NotNullOrEmpty()
//        {
//            // arrange
//            var factory = new ObjectValidatorErrorMessageFactory<Model3, string>();
//            factory.SetPropName("email");
//            Expression<Func<string, IValidatorExpression, bool>> exp = (email, validator) => validator.NotNullOrEmpty(email, null);
//            factory.SetupExpression(exp);
//
//            var model = new Model3 { EmailAddress = "jon@email.com" };
//
//            // act
//            Action action = () => factory.Invoke(model);
//
//            // assert
//            action.Should().NotThrow();
//        }
//
//        [Test]
//
//        public void Get_IValidatorExpression_MinLength()
//        {
//            // arrange
//            var factory = new ObjectValidatorErrorMessageFactory<Model3, string>();
//            factory.SetPropName("email");
//            Expression<Func<string, IValidatorExpression, bool>> exp = (email, validator) => validator.MinLength(email, 3, null);
//            factory.SetupExpression(exp);
//
//            var model = new Model3 { EmailAddress = "jon@email.com" };
//
//            // act
//            Action action = () => factory.Invoke(model);
//
//            // assert
//            action.Should().NotThrow();
//        }

//        [Test]
//
//        public void Get_IValidatorExpression_MaxLength()
//        {
//            // arrange
//            var factory = new ObjectValidatorErrorMessageFactory<Model3, string>();
//            factory.SetPropName("email");
//            Expression<Func<string, IValidatorExpression, bool>> exp = (email, validator) => validator.MaxLength(email, 99, null);
//            factory.SetupExpression(exp);
//
//            var model = new Model3 { EmailAddress = "jon@email.com" };
//
//            // act
//            Action action = () => factory.Invoke(model);
//
//            // assert
//            action.Should().NotThrow();
//        }
//
//        [Test]
//
//        public void Get_IValidatorExpression_MinValue()
//        {
//            // arrange
//            var factory = new ObjectValidatorErrorMessageFactory<Model3, int>();
//            factory.SetPropName("Number");
//            Expression<Func<int, IValidatorExpression, bool>> exp = (number, validator) => validator.MinValue(number, 18, null);
//            factory.SetupExpression(exp);
//
//            var model = new Model3 { Number = 23 };
//
//            // act
//            Action action = () => factory.Invoke(model);
//
//            // assert
//            action.Should().NotThrow();
//        }        
    }
}
