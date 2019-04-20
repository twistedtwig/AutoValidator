using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoValidator.Impl;
using AutoValidator.Interfaces;
using AutoValidator.Models;
using AutoValidator.Tests.Models;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    [TestFixture]
    public class MapperConfigurationTests
    {
        private MapperConfiguration _subject;

        [Test]

        public void Passing_Null_Configuration_By_Constructor_Throws()
        {
            // arrange
            _subject = new MapperConfiguration((MapperConfigurationExpression)null);

            // act
            Action action = () => _subject.AssertExpressionsAreValid();

            // assert
            action.Should().Throw<ConfigurationExpressionException>().WithMessage("No configuration has been setup");
        }

        [Test]

        public void Passing_Bad_Configuration_Will_Throw()
        {
            // arrange
            var expression = new MapperConfigurationExpression();
            expression.AddProfile<DuplicateInvalidMappingProfile>();
            expression.AddProfile<MissingMappingProfile>();

            _subject = new MapperConfiguration(expression);

            try
            {
                _subject.AssertExpressionsAreValid();
            }
            catch (ConfigurationExpressionException ex)
            {
                ex.Errors.ToList().Count.Should().Be(2);
                ex.Errors.Should().Contain(e => e.ProfileType == typeof(DuplicateInvalidMappingProfile));
                ex.Errors.Should().Contain(e => e.ProfileType == typeof(MissingMappingProfile));
            }
        }

        [Test]

        public void Passing_Bad_Configuration_Will_Throw_Will_Not_Contain_Good_Profile()
        {
            // arrange
            var expression = new MapperConfigurationExpression();
            expression.AddProfile<DuplicateInvalidMappingProfile>();
            expression.AddProfile<MissingMappingProfile>();
            expression.AddProfile<Profile1>();

            _subject = new MapperConfiguration(expression);

            try
            {
                _subject.AssertExpressionsAreValid();
            }
            catch (ConfigurationExpressionException ex)
            {
                ex.Errors.ToList().Count.Should().Be(2);
                ex.Errors.Should().Contain(e => e.ProfileType == typeof(DuplicateInvalidMappingProfile));
                ex.Errors.Should().Contain(e => e.ProfileType == typeof(MissingMappingProfile));
            }
        }

        [Test]

        public void Passing_Good_Configuration_Will_Not_Throw_Any_Exceptions_On_Assert()
        {
            // arrange
            var expression = new MapperConfigurationExpression();
            expression.AddProfile<Profile2>();
            expression.AddProfile<Profile1>();

            _subject = new MapperConfiguration(expression);

            // act
            Action action = () => _subject.AssertExpressionsAreValid();

            // assert
            action.Should().NotThrow();
        }

        [Test]

        public void Passing_Bad_Configuration_Action__Will_Throw()
        {
            // arrange
            Action<IMapperConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<DuplicateInvalidMappingProfile>();
                cfg.AddProfile<MissingMappingProfile>();
            };

            _subject = new MapperConfiguration(configure);

            try
            {
                _subject.AssertExpressionsAreValid();
            }
            catch (ConfigurationExpressionException ex)
            {
                ex.Errors.ToList().Count.Should().Be(2);
                ex.Errors.Should().Contain(e => e.ProfileType == typeof(DuplicateInvalidMappingProfile));
                ex.Errors.Should().Contain(e => e.ProfileType == typeof(MissingMappingProfile));
            }
        }

        [Test]

        public void Passing_Bad_Configuration_Action__Will_Throw_Will_Not_Contain_Good_Profile()
        {
            // arrange
            Action<IMapperConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<DuplicateInvalidMappingProfile>();
                cfg.AddProfile<MissingMappingProfile>();
                cfg.AddProfile<Profile1>();
            };

            _subject = new MapperConfiguration(configure);
            
            try
            {
                _subject.AssertExpressionsAreValid();
            }
            catch (ConfigurationExpressionException ex)
            {
                ex.Errors.ToList().Count.Should().Be(2);
                ex.Errors.Should().Contain(e => e.ProfileType == typeof(DuplicateInvalidMappingProfile));
                ex.Errors.Should().Contain(e => e.ProfileType == typeof(MissingMappingProfile));
            }
        }

        [Test]

        public void Passing_Good_Configuration_Action_Will_Not_Throw_Any_Exceptions_On_Assert()
        {
            // arrange
            Action<IMapperConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<Profile2>();
                cfg.AddProfile<Profile2>();
            };

            _subject = new MapperConfiguration(configure);

            // act
            Action action = () => _subject.AssertExpressionsAreValid();

            // assert
            action.Should().NotThrow();
        }

        [Test]

        public void Create_Factory_For_Config_That_Is_Not_Setup_Will_Throw_Exception()
        {
            // arrange
            _subject = new MapperConfiguration((MapperConfigurationExpression)null);

            // act
            Action action = () => _subject.CreateFactory();

            // assert
            action.Should().Throw<ArgumentNullException>().WithMessage("Configuration has not been configured");
        }

        [Test]

        public void Create_Factory_Will_Return_A_Valid_Factory()
        {
            // arrange
            Action<IMapperConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<Profile2>();
                cfg.AddProfile<Profile2>();
            };

            _subject = new MapperConfiguration(configure);

            var model = new Model1
            {
                Age = 23,
                Name = "Jon Hawkins"
            };

            // act
            var factory = _subject.CreateFactory();

            // assert
            factory.Should().NotBeNull();

            var validator = factory.Create<Model1>();
            validator.Should().NotBeNull();
            var validationResult = validator.Validate(model);
            validationResult.Should().NotBeNull();
            validationResult.Success.Should().BeTrue();
        }

        [Test]

        public void Create_Factory_Action_Will_Return_An_Action_That_Can_Be_Passed_Back_And_Actioned_Later()
        {
            Action<IMapperConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<Profile2>();
                cfg.AddProfile<Profile2>();
            };

            _subject = new MapperConfiguration(configure);

            var model = new Model1
            {
                Age = 23,
                Name = "Jon Hawkins"
            };

            // act
            var factoryFunc = _subject.CreateFactoryFunc();

            var factory = factoryFunc();

            // assert
            factory.Should().BeOfType<ValidatorFactory>();

            var validator = factory.Create<Model1>();
            validator.Should().NotBeNull();
            var validationResult = validator.Validate(model);
            validationResult.Should().NotBeNull();
            validationResult.Success.Should().BeTrue();
        }
        
        [Test]

        public void Create_Factory_Action_Will_Return_New_Instance_Each_Time()
        {
            Action<IMapperConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<Profile2>();
                cfg.AddProfile<Profile2>();
            };

            _subject = new MapperConfiguration(configure);

            // act
            var factoryFunc = _subject.CreateFactoryFunc();

            var factory1 = factoryFunc();
            var factory2 = factoryFunc();

            // assert
            factory1.Equals(factory2).Should().BeFalse();
        }
    }
}
