using Google.Cloud.Functions.Hosting;
using Google.Cloud.Functions.Framework;
using JMDict;

[assembly: FunctionsStartup(typeof(JmdictGQL.Startup))]

namespace JmdictGQL
{
    public class Function : IHttpFunction
    {
        public async Task HandleAsync(HttpContext context)
        {
            await context.Response.WriteAsync("Use the /gql route to access the GraphQL endpoint.");
        }
    }

    public class Startup : FunctionsStartup
    {
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            // Configure services here
            var dictParser = new DictParser();
            var jmdictPath = "dictionaries/JMdict_e.xml";
            var kanjidicPath = "dictionaries/kanjidic2.xml";
            var jmdict = dictParser.ParseXml<Jmdict>(jmdictPath);
            var kanjidic = dictParser.ParseXml<Kanjidic>(kanjidicPath);

            services.AddSingleton(jmdict);
            services.AddSingleton(kanjidic);

            services
                .AddCors(options =>
                {
                    options.AddPolicy("AllowAny", builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
                })
                .AddGraphQLServer()
                .AddQueryType<Query>();
        }

        public override void Configure(WebHostBuilderContext context, IApplicationBuilder app)
        {
            // Configure the HTTP request pipeline here
            app.UseCors("AllowAny");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL("/gql");
            });
        }
    }
}
