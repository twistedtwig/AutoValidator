using System;
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
    public class AutoValidationTests
    {
        private Impl.AutoValidation _subject;

        [Test]

        public void Passing_Null_Configuration_By_Constructor_Throws()
        {
            // arrange
            _subject = new AutoValidation((ValidatorConfigurationExpression)null);

            // act
            Action action = () => _subject.AssertExpressionsAreValid();

            // assert
            action.Should().Throw<ConfigurationExpressionException>().WithMessage("No configuration has been setup");
        }

        [Test]

        public void Passing_Bad_Configuration_Will_Throw()
        {
            // arrange
            var expression = new ValidatorConfigurationExpression();
            expression.AddProfile<DuplicateInvalidMappingProfile>();
            expression.AddProfile<MissingMappingProfile>();

            _subject = new AutoValidation(expression);

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
            var expression = new ValidatorConfigurationExpression();
            expression.AddProfile<DuplicateInvalidMappingProfile>();
            expression.AddProfile<MissingMappingProfile>();
            expression.AddProfile<Profile1>();

            _subject = new AutoValidation(expression);

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
            var expression = new ValidatorConfigurationExpression();
            expression.AddProfile<Profile2>();
            expression.AddProfile<Profile1>();

            _subject = new AutoValidation(expression);

            // act
            Action action = () => _subject.AssertExpressionsAreValid();

            // assert
            action.Should().NotThrow();
        }

        [Test]

        public void Passing_Bad_Configuration_Action__Will_Throw()
        {
            // arrange
            Action<IValidatorConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<DuplicateInvalidMappingProfile>();
                cfg.AddProfile<MissingMappingProfile>();
            };

            _subject = new AutoValidation(configure);

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
            Action<IValidatorConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<DuplicateInvalidMappingProfile>();
                cfg.AddProfile<MissingMappingProfile>();
                cfg.AddProfile<Profile1>();
            };

            _subject = new AutoValidation(configure);
            
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
            Action<IValidatorConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<Profile2>();
                cfg.AddProfile<Profile2>();
            };

            _subject = new AutoValidation(configure);

            // act
            Action action = () => _subject.AssertExpressionsAreValid();

            // assert
            action.Should().NotThrow();
        }

        [Test]

        public void Create_Factory_For_Config_That_Is_Not_Setup_Will_Throw_Exception()
        {
            // arrange
            _subject = new AutoValidation((ValidatorConfigurationExpression)null);

            // act
            Action action = () => _subject.CreateFactory();

            // assert
            action.Should().Throw<ArgumentNullException>().WithMessage("Configuration has not been configured");
        }

        [Test]

        public void Create_Factory_Will_Return_A_Valid_Factory()
        {
            // arrange
            Action<IValidatorConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<Profile2>();
                cfg.AddProfile<Profile2>();
            };

            _subject = new AutoValidation(configure);

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
            Action<IValidatorConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<Profile2>();
                cfg.AddProfile<Profile2>();
            };

            _subject = new AutoValidation(configure);

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
            _subject = new AutoValidation(cfg =>
                {
                    cfg.AddProfile<Profile2>();
                    cfg.AddProfile<Profile2>();
                });

            // act
            var factoryFunc = _subject.CreateFactoryFunc();

            var factory1 = factoryFunc();
            var factory2 = factoryFunc();

            // assert
            factory1.Equals(factory2).Should().BeFalse();
        }

        [Test]
        public void Has_CreateMap_For_Class_Finds_Map_Returns_True()
        {
            Action<IValidatorConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<Profile2>();
            };

            _subject = new AutoValidation(configure);

            var mapType = typeof(Model1);

            var result = _subject.HasMap(mapType);
            result.Should().BeTrue();
        }

        [Test]
        public void Has_CreateMap_For_Class_NoMapping_Returns_False()
        {
            Action<IValidatorConfigurationExpression> configure = cfg =>
            {
                cfg.AddProfile<Profile2>();
            };

            _subject = new AutoValidation(configure);

            var mapType = typeof(Model2);

            var result = _subject.HasMap(mapType);
            result.Should().BeFalse();
        }
    }
}
