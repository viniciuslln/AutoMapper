using System;
using AutoMapper.Mappers;

namespace AutoMapper
{
	public static class Mapper
	{
		private static Configuration _configuration;
		private static IMappingEngine _mappingEngine;

		public static TDestination Map<TSource, TDestination>(TSource source)
		{
			return Engine.Map<TSource, TDestination>(source);
		}

		public static TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
		{
			return Engine.Map(source, destination);
		}

		public static object Map(object source, Type sourceType, Type destinationType)
		{
			return Engine.Map(source, sourceType, destinationType);
		}

		public static object Map(object source, object destination, Type sourceType, Type destinationType)
		{
			return Engine.Map(source, destination, sourceType, destinationType);
		}

		public static TDestination DynamicMap<TSource, TDestination>(TSource source)
		{
			return Engine.DynamicMap<TSource, TDestination>(source);
		}

		public static TDestination DynamicMap<TDestination>(object source)
		{
			return Engine.DynamicMap<TDestination>(source);
		}

		public static object DynamicMap(object source, Type sourceType, Type destinationType)
		{
			return Engine.DynamicMap(source, sourceType, destinationType);
		}

		public static void Initialize(Action<IConfiguration> action)
		{
			Reset();

			action(Configuration);
		}

		public static IFormatterCtorExpression<TValueFormatter> AddFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
		{
			return Configuration.AddFormatter<TValueFormatter>();
		}

		public static IFormatterCtorExpression AddFormatter(Type valueFormatterType)
		{
			return Configuration.AddFormatter(valueFormatterType);
		}

		public static void AddFormatter(IValueFormatter formatter)
		{
			Configuration.AddFormatter(formatter);
		}

		public static void AddFormatExpression(Func<ResolutionContext, string> formatExpression)
		{
			Configuration.AddFormatExpression(formatExpression);
		}

		public static void SkipFormatter<TValueFormatter>() where TValueFormatter : IValueFormatter
		{
			Configuration.SkipFormatter<TValueFormatter>();
		}

		public static IFormatterExpression ForSourceType<TSource>()
		{
			return Configuration.ForSourceType<TSource>();
		}

		public static IMappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
		{
			return Configuration.CreateMap<TSource, TDestination>();
		}

		public static IProfileExpression CreateProfile(string profileName)
		{
			return Configuration.CreateProfile(profileName);
		}

		public static void CreateProfile(string profileName, Action<IProfileExpression> profileConfiguration)
		{
			Configuration.CreateProfile(profileName, profileConfiguration);
		}

		public static void AddProfile(Profile profile)
		{
			Configuration.AddProfile(profile);
		}

		public static void AddProfile<TProfile>() where TProfile : Profile, new()
		{
			Configuration.AddProfile<TProfile>();
		}

		public static TypeMap FindTypeMapFor(Type sourceType, Type destinationType)
		{
			return ConfigurationProvider.FindTypeMapFor(sourceType, destinationType);
		}

		public static TypeMap FindTypeMapFor<TSource, TDestination>()
		{
			return ConfigurationProvider.FindTypeMapFor<TSource, TDestination>();
		}

		public static TypeMap[] GetAllTypeMaps()
		{
			return ConfigurationProvider.GetAllTypeMaps();
		}

		public static void AssertConfigurationIsValid()
		{
			ConfigurationProvider.AssertConfigurationIsValid();
		}

		public static void Reset()
		{
			lock (typeof (IConfigurationProvider))
				lock (typeof (IMappingEngine))
				{
					_configuration = null;
					_mappingEngine = null;
				}
		}

		public static IMappingEngine Engine
		{
			get
			{
				if (_mappingEngine == null)
				{
					lock (typeof(IMappingEngine))
					{
						if (_mappingEngine == null)
						{
							_mappingEngine = new MappingEngine(ConfigurationProvider);
						}
					}
				}

				return _mappingEngine;
			}
		}

		private static IConfigurationProvider ConfigurationProvider
		{
			get
			{
				if (_configuration == null)
				{
					lock (typeof (Configuration))
					{
						if (_configuration == null)
						{
							_configuration = new Configuration(MapperRegistry.AllMappers());
						}
					}
				}

				return _configuration;
			}
		}

		private static IConfiguration Configuration
		{
			get { return (IConfiguration) ConfigurationProvider; }
		}
	}
}