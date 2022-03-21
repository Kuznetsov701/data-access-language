using DataAccessLanguage.Http;
using DataAccessLanguage.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace DataAccessLanguage.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataAccessLanguage(this IServiceCollection serviceCollection, ServiceLifetime lifetime = ServiceLifetime.Singleton) =>
            Add(serviceCollection, null, lifetime);

        public static void AddDataAccessLanguage(this IServiceCollection serviceCollection, Action<Dictionary<string, Func<string, IExpressionPart>>, IServiceProvider> configure, ServiceLifetime lifetime = ServiceLifetime.Singleton) => 
            Add(serviceCollection, configure, lifetime);

        public static IHttpClientBuilder AddDataAccessLanguage(this IServiceCollection serviceCollection, Action<IServiceProvider, HttpClient> configureHttp, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) =>
            AddWithHttp(serviceCollection, null, configureHttp, null, serviceLifetime);

        public static void AddDataAccessLanguage(this IServiceCollection serviceCollection, Action<Dictionary<string, Func<string, IExpressionPart>>, IServiceProvider> configure, Action<IServiceProvider, HttpClient> configureHttp, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) =>
            AddWithHttp(serviceCollection, configure, configureHttp, null, serviceLifetime);

        public static void AddDataAccessLanguage(this IServiceCollection serviceCollection, Action<IServiceProvider, HttpClient> configureHttp, JsonSerializerOptions jsonSerializerOptions, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) =>
            AddWithHttp(serviceCollection, null, configureHttp, jsonSerializerOptions, serviceLifetime);

        public static void AddDataAccessLanguage(this IServiceCollection serviceCollection, Action<Dictionary<string, Func<string, IExpressionPart>>, IServiceProvider> configure, Action<IServiceProvider, HttpClient> configureHttp, JsonSerializerOptions jsonSerializerOptions, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) =>
            AddWithHttp(serviceCollection, configure, configureHttp, jsonSerializerOptions, serviceLifetime);

        internal static void Add(IServiceCollection services,
            Action<Dictionary<string, Func<string, IExpressionPart>>, IServiceProvider> configure, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(typeof(IExpressionFactory), s => {
                return new ExpressionFactory(x => {
                    var r = GetDefaultTypes(x);
                    configure?.Invoke(r, s);
                    return r;
                });
            }, serviceLifetime);
            services.Add(serviceDescriptor);
        }

        internal static IHttpClientBuilder AddWithHttp(IServiceCollection services, 
            Action<Dictionary<string, Func<string, IExpressionPart>>, IServiceProvider> configure, 
            Action<IServiceProvider, HttpClient> configureClient, 
            JsonSerializerOptions jsonSerializerOptions, 
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(typeof(IExpressionFactory), s => {
                    return new ExpressionFactory(x => {
                        var r = GetDefaultTypes(x);
                        r.Add("http", y => new HttpFunction(s.GetService<IHttpClientFactory>().CreateClient(nameof(IExpressionFactory)), x, x, y));
                        r.Add("post", y => new HttpPostFunction(y, x));
                        r.Add("postJson", y => new HttpPostFunction($"{y}.toJson(), str(application/json)", x));
                        r.Add("header", y => new HttpHeaderFucntion(y, x));
                        r.Add("toJson", y => new ToJsonPart(jsonSerializerOptions));
                        r.Add("fromJson", y => new FromJsonPart(jsonSerializerOptions));
                        r.Add("fromUrlSafe", y => new FromUrlSafePart());
                        r.Add("toUrlSafe", y => new ToUrlSafePart());
                        r.Add("get", y => new HttpGetFunction());
                        return r;
                    });
                }, serviceLifetime);

            services.Add(serviceDescriptor);
            return services.AddHttpClient(nameof(IExpressionFactory), (s, h) => {
                configureClient?.Invoke(s, h);
            });
        }

        private static Dictionary<string, Func<string, IExpressionPart>> GetDefaultTypes(ExpressionFactory expressionFactory)
        {
            return new Dictionary<string, Func<string, IExpressionPart>>
             {
                { "index", x => new IndexPart(int.Parse(x)) },
                { "selector", x => new SelectorPart(x) },
                { "for", x => new ForPart(x) },
                { "select", x => new SelectPart(expressionFactory, x) },
                { "pselect", x => new ParallelSelectPart(expressionFactory, x) },
                { "sum", x => new SumPart() },
                { "join", x => new JoinPart(x) },
                { "equals", x => new EqualsPart(expressionFactory, x) },
                { "notEquals", x => new NotEqualsPart(expressionFactory, x) },
                { "moreThan", x => new MoreThanPart(expressionFactory, x) },
                { "lessThan", x => new LessThanPart(expressionFactory, x) },
                { "equalsOrMoreThan", x => new EqualsOrMoreThanPart(expressionFactory, x) },
                { "equalsOrLessThan", x => new EqualsOrLessThanPart(expressionFactory, x) },
                { "iif", x => new IifPart(expressionFactory, x) },
                { "where", x => new WherePart(expressionFactory, x) },
                { "self", x => new SelfPart() },
                { "toLower", x => new ToLowerPart() },
                { "toUpper", x => new ToUpperPart() },
                { "groupBy", x => new GroupByPart(expressionFactory, x) },
                { "map", x => new MapPart(expressionFactory, x) },
                { "pmap", x => new MapPart(expressionFactory, x) },
                { "switch", x => new SwitchPart(expressionFactory, x) },
                { "concat", x => new ConcatPart(expressionFactory, x) },
                { "trim", x => new TrimPart(x) },
                { "split", x => new SplitPart(x) },
                { "replace", x => new ReplacePart(x) },
                { "avg", x => new AvgPart() },
                { "min", x => new MinPart() },
                { "max", x => new MaxPart() },
                { "distinct", x => new DistinctPart(expressionFactory, x) },
                { "selectMany", x => new SelectManyPart(expressionFactory, x) },
                { "pselectMany", x => new ParallelSelectManyPart(expressionFactory, x) },
                { "now", x => new NowPart() },
                { "count", x => new CountPart() },
                { "console", x => new ConsolePart() },
                { "round", x => new RoundPart(x) },
                { "orderBy", x => new OrderByPart(expressionFactory, x) },
                { "orderByDesc", x => new OrderByDescPart(expressionFactory, x) },
                { "in", x => new InPart(expressionFactory, x) },
                { "null", x => new NullPart() },
                { "add", x => new AddPart(expressionFactory, x) },

                { "str", x => new StringFunction(x) },
                { "int", x => new IntegerFunction(expressionFactory, x) },
                { "double", x => new DoubleFunction(expressionFactory, x) },
                { "decimal", x => new DecimalFunction(expressionFactory, x) },
                { "float", x => new FloatFunction(expressionFactory, x) },
                { "date", x => new DateTimeFunction(expressionFactory, x) },
                { "time", x => new TimeSpanFunction(expressionFactory, x) },
                { "bool", x => new BooleanFunction(expressionFactory, x) },
                { "long", x => new LongFunction(expressionFactory, x) },
                { "byte", x => new ByteFunction(expressionFactory, x) },
                { "arr", x => new ArrayFunction(expressionFactory, x) },

                { "dal", x => new DalPart(expressionFactory, x) },
                { "format", x => new FormatPart(x) }
            };
        }
    }
}