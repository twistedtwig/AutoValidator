using System;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class MapperConfiguration : IConfigurationProvider
    {
        private readonly MapperConfigurationExpression _configurationExpression;

        public MapperConfiguration(Action<IMapperConfigurationExpression> configure)
            : this(Build(configure))
        {
        }

        public MapperConfiguration(MapperConfigurationExpression configurationExpression)
        {
            _configurationExpression = configurationExpression;
        }

        private static MapperConfigurationExpression Build(Action<IMapperConfigurationExpression> configureFunc)
        {
            var expr = new MapperConfigurationExpression();
            configureFunc(expr);
            return expr;
        }
    }
}
