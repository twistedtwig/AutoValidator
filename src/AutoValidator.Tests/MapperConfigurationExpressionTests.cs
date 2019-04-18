using System;
using System.Linq;
using AutoValidator.Impl;
using AutoValidator.Tests.Models;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    [TestFixture]
    public class MapperConfigurationExpressionTests
    {
        private MapperConfigurationExpression _subject;

        [SetUp]
        public void Init()
        {
            _subject = new MapperConfigurationExpression();
        }

        [Test]

        public void Add_Profile_By_Instance_Is_Stored()
        {
            // arrange
            var p1 = new Profile1();
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfile(p1);

            // assert
            _subject.Profiles.Should().Contain(x => x.GetType() == p1.GetType());
        }

        [Test]

        public void Add_Multiple_Profiles_By_Instance_Are_Stored()
        {
            // arrange
            var p1 = new Profile1();
            var p2 = new Profile2();
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfile(p1);
            _subject.AddProfile(p2);

            // assert
            _subject.Profiles.Should().Contain(x => x.GetType() == p1.GetType());
            _subject.Profiles.Should().Contain(x => x.GetType() == p2.GetType());
        }

        [Test]

        public void Adding_Same_Profile_By_Instance_Twice_Will_Not_Double_Up()
        {
            // arrange
            var p1 = new Profile1();
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfile(p1);
            _subject.AddProfile(p1);

            // assert
            _subject.Profiles.ToList().Count.Should().Be(1);
        }

        [Test]

        public void Add_Profile_By_GenericType_Is_Stored()
        {
            // arrange
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfile<Profile1>();

            // assert
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile1));
        }

        [Test]

        public void Add_Multiple_Profiles_By_GenericType_Are_Stored()
        {
            // arrange
            // act
            _subject.AddProfile<Profile1>();
            _subject.AddProfile<Profile2>();

            // assert
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile1));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile2));
        }

        [Test]

        public void Adding_Same_Profile_By_GenericType_Twice_Will_Not_Double_Up()
        {
            // arrange
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfile<Profile1>();
            _subject.AddProfile<Profile1>();

            // assert
            _subject.Profiles.ToList().Count.Should().Be(1);
        }

        [Test]

        public void Add_Profile_By_Type_Is_Stored()
        {
            // arrange
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfile(typeof(Profile1));

            // assert
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile1));
        }

        [Test]

        public void Add_Multiple_Profiles_By_Type_Are_Stored()
        {
            // arrange
            // act
            _subject.AddProfile(typeof(Profile1));
            _subject.AddProfile(typeof(Profile2));


            // assert
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile1));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile2));
        }

        [Test]

        public void Adding_Same_Profile_By_Type_Twice_Will_Not_Double_Up()
        {
            // arrange
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfile(typeof(Profile1));
            _subject.AddProfile(typeof(Profile1));


            // assert
            _subject.Profiles.ToList().Count.Should().Be(1);
        }
        
        [Test]

        public void Adding_Profiles_From_Assembly()
        {
            // arrange
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfiles(new [] { System.Reflection.Assembly.GetExecutingAssembly() });

            // assert
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile1));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile2));
        }

        [Test]

        public void Profile_All_Mappings_Are_Valid()
        {
            // arrange
            var profile = new Profile1();
            
            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeTrue();
            result.ExpressionErrors.Count.Should().Be(0);
        }

        [Test]

        public void Profile_With_Ignore_And_Mappings_Are_Valid()
        {
            // arrange
            var profile = new Profile2();

            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeTrue();
            result.ExpressionErrors.Count.Should().Be(0);
        }

        [Test]

        public void Profile_Missing_A_Mapping_Is_Not_Valid()
        {
            // arrange
            var profile = new MissingMappingProfile();

            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeFalse();
            result.ExpressionErrors.Count.Should().Be(1);
            var model1Expression = result.ExpressionErrors[0];
            model1Expression.SourceClass.Should().Be<Model1>();

            model1Expression.Errors.Should().Contain(x => x == "Missing mapping for 'Name'");
            model1Expression.PropertiesThatHaveErrors.Should().Contain("Name");
        }

        [Test]

        public void Profile_With_Duplicate_Member_Mapping_Is_Not_Valid()
        { // arrange
            var profile = new DuplicateMappingProfile();

            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeFalse();
            result.ExpressionErrors.Count.Should().Be(1);
            var model1Expression = result.ExpressionErrors[0];
            model1Expression.SourceClass.Should().Be<Model1>();

            model1Expression.Errors.Count.Should().Be(1);
            model1Expression.Errors.Should().Contain(x => x == "Duplicate mapping for 'Age'");

            model1Expression.PropertiesThatHaveErrors.Count.Should().Be(1);
            model1Expression.PropertiesThatHaveErrors.Should().Contain("Age");
        }

        [Test]

        public void Profile_With_Multiple_Member_Mapping_Errors_Is_Not_Valid()
        { // arrange
            var profile = new MultipleMappingErrorsProfile();

            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeFalse();
            result.ExpressionErrors.Count.Should().Be(1);
            var model1Expression = result.ExpressionErrors[0];
            model1Expression.SourceClass.Should().Be<Model1>();

            model1Expression.Errors.Count.Should().Be(2);
            model1Expression.Errors.Should().Contain(x => x == "Missing mapping for 'Name'");
            model1Expression.Errors.Should().Contain(x => x == "Duplicate mapping for 'Age'");

            model1Expression.PropertiesThatHaveErrors.Count.Should().Be(2);
            model1Expression.PropertiesThatHaveErrors.Should().Contain("Age");
            model1Expression.PropertiesThatHaveErrors.Should().Contain("Name");
        }
        
        [Test]

        public void Profile_With_Multiple_Mappings_Each_Mapping_Has_Errors_Is_Not_Valid()
        { // arrange
            var profile = new MultipleMappingsWithErrorsProfile();

            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeFalse();
            result.ExpressionErrors.Count.Should().Be(2);
            var model1Expression = result.ExpressionErrors[0];
            model1Expression.SourceClass.Should().Be<Model1>();

            model1Expression.Errors.Count.Should().Be(1);
            model1Expression.Errors.Should().Contain(x => x == "Missing mapping for 'Name'");

            model1Expression.PropertiesThatHaveErrors.Count.Should().Be(1);
            model1Expression.PropertiesThatHaveErrors.Should().Contain("Name");

            var model2Expression = result.ExpressionErrors[1];
            model2Expression.SourceClass.Should().Be<Model2>();

            model2Expression.Errors.Count.Should().Be(2);
            model2Expression.Errors.Should().Contain(x => x == "Missing mapping for 'Number'");
            model2Expression.Errors.Should().Contain(x => x == "Duplicate mapping for 'Category'");

            model2Expression.PropertiesThatHaveErrors.Count.Should().Be(2);
            model2Expression.PropertiesThatHaveErrors.Should().Contain("Number");
            model2Expression.PropertiesThatHaveErrors.Should().Contain("Category");
        }
    }
}
