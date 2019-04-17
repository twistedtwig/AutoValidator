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

        public void Adding_Profile_From_Assembly()
        {
            // arrange
            _subject.Profiles.Should().BeEmpty();

            // act
            _subject.AddProfile(System.Reflection.Assembly.GetExecutingAssembly());

            // assert
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile1));
            _subject.Profiles.Should().Contain(x => x.GetType() == typeof(Profile2));
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

        public void Profile_All_Mappings_Is_Valid()
        {
            // arrange
            var profile = new Profile1();
            
            // act

            // assert
            throw new NotImplementedException();
        }

        [Test]

        public void Profile_With_Igore_And_Mappings_Is_Valid()
        {
            // arrange
            
            // act

            // assert
            throw new NotImplementedException();
        }

        [Test]

        public void Profile_Missing_A_Mapping_Is_Not_Valid()
        {
            // arrange
            
            // act

            // assert
            throw new NotImplementedException();
        }
    }
}
