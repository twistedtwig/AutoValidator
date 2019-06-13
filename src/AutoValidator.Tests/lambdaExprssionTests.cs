using System;
using NUnit.Framework;
using System.Linq.Expressions;
using AutoValidator.Tests.Models;
using FluentAssertions;


namespace AutoValidator.Tests
{
    [TestFixture]
    public class lambdaExprssionTests
    {

        [Test]

        public void age_greater_than()
        {
            var model = new Model1
            {
                Name = "Jon",
                Age = 18
            };

            var parameter = Expression.Parameter(typeof(Model1), "x");
            var member = Expression.Property(parameter, "Age"); //x.Age
            var constant = Expression.Constant(3);
            var body = Expression.GreaterThanOrEqual(member, constant); //x.Id >= 3
            var finalExpression = Expression.Lambda<Func<Model1, bool>>(body, parameter); //x => x.Id >= 3

            var expression = finalExpression.Compile();
            var result = expression.Invoke(model);

            result.Should().BeTrue();
        }

        [Test]

        public void Age_Greater_Than_Premade_Expression()
        {
            // arrange
            var model = new Model1
            {
                Name = "Jon",
                Age = 18
            };
            
            Expression<Func<Model1, bool>> fullExpression = x => x.Age >= 3;
            var body = fullExpression.Body;

            var finalExpression = Expression.Lambda<Func<Model1, bool>>(body, fullExpression.Parameters); //x => x.Id >= 3

            //variable 'x' of type 'AutoValidator.Tests.Models.Model1' referenced from scope '', but it is not defined
            var expression = finalExpression.Compile();
            var result = expression.Invoke(model);

            result.Should().BeTrue();
        }
    }
}
