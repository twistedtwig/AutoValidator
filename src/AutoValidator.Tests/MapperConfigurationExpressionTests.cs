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

        public void Add_Profile_By_Assembly_Is_Stored()
        {
            // arrange
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfiles(System.Reflection.Assembly.GetExecutingAssembly());

            // assert
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(DuplicateMappingProfile));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(MissingMappingProfile));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(MultipleMappingErrorsProfile));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(MultipleMappingsProfile));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(MultipleMappingsWithErrorsProfile));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile1));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile2));
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
            result.ExpressionResults.Count.Should().Be(0);
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
            result.ExpressionResults.Count.Should().Be(0);
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
            result.ExpressionResults.Count.Should().Be(1);
            var model1Expression = result.ExpressionResults[0];
            model1Expression.SourceClass.Should().Be<Model1>();

            var error = model1Expression.Errors.Single(x => x.PropertyName == "Name");
            error.Error.Should().Be("Missing mapping for property 'Name'");
        }

        [Test]

        public void Profile_With_Duplicate_Member_Mapping_IsValid()
        { // arrange
            var profile = new DuplicateMappingProfile();

            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeTrue();
            result.ExpressionResults.Count.Should().Be(0);
        }

        [Test]

        public void Profile_With_Duplicate_Member_And_Same_Expression_Mapping_Is_Not_Valid()
        { // arrange
            var profile = new DuplicateInvalidMappingProfile();

            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeFalse();
            result.ExpressionResults.Count.Should().Be(1);
            var model1Expression = result.ExpressionResults[0];
            model1Expression.SourceClass.Should().Be<Model1>();

            model1Expression.Errors.Count.Should().Be(1);
            var error = model1Expression.Errors.Single(x => x.PropertyName == "Age");
            error.Error.Should().Be("Duplicate mapping for property 'Age'");
        }
       

        [Test]

        public void Profile_With_Multiple_Member_Mapping_Errors_Is_Not_Valid()
        { // arrange
            var profile = new MultipleMappingErrorsProfile();

            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeFalse();
            result.ExpressionResults.Count.Should().Be(1);
            var model1Expression = result.ExpressionResults[0];
            model1Expression.SourceClass.Should().Be<Model1>();

            model1Expression.Errors.Count.Should().Be(2);

            var error1 = model1Expression.Errors.Single(x => x.PropertyName == "Age");
            error1.Error.Should().Be("Duplicate mapping for property 'Age'");

            var error2 = model1Expression.Errors.Single(x => x.PropertyName == "Name");
            error2.Error.Should().Be("Missing mapping for property 'Name'");
        }
        
        [Test]

        public void Profile_With_Multiple_Mappings_Each_Mapping_Has_Errors_Is_Not_Valid()
        { // arrange
            var profile = new MultipleMappingsWithErrorsProfile();

            // act
            var result = profile.ValidateExpression();

            // assert
            result.Success.Should().BeFalse();
            result.ExpressionResults.Count.Should().Be(2);
            var model1Expression = result.ExpressionResults[0];
            model1Expression.SourceClass.Should().Be<Model1>();

            model1Expression.Errors.Count.Should().Be(1);

            var error1 = model1Expression.Errors.Single(x => x.PropertyName == "Name");
            error1.Error.Should().Be("Missing mapping for property 'Name'");
            
            var model2Expression = result.ExpressionResults[1];
            model2Expression.SourceClass.Should().Be<Model2>();

            var error2 = model2Expression.Errors.Single(x => x.PropertyName == "Number");
            var error3 = model2Expression.Errors.Single(x => x.PropertyName == "Category");
            error2.Error.Should().Be("Missing mapping for property 'Number'");
            error3.Error.Should().Be("Duplicate mapping for property 'Category'");            
        }

        [Test]

        public void Valid_Profiles_Have_Been_Added_Is_Valid()
        {
            // arrange
            _subject.AddProfile<MultipleMappingsProfile>();
            _subject.AddProfile<Profile1>();

            // act
            var result = _subject.GetConfigurationExpressionValidation();

            // assert
            result.All(v => v.Success).Should().BeTrue();
        }


        [Test]

        public void InValid_Profiles_Have_Been_Added_Is_Not_Valid()
        {
            // arrange
            _subject.AddProfile<MultipleMappingsProfile>();
            _subject.AddProfile<Profile1>();
            _subject.AddProfile<DuplicateInvalidMappingProfile>();

            // act
            var result = _subject.GetConfigurationExpressionValidation();

            // assert
            result.All(v => v.Success).Should().BeFalse();
        }
    }
}
