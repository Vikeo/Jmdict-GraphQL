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
            await context.Response.WriteAsync("Hello from Google Cloud Function!");
        }
    }

    public class Startup : FunctionsStartup
    {
        public override void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {
            // Configure services here
            var dictParser = new DictParser();
            var jmdictPath = "dictionaries/JMdict_e.xml";
            var jmdict = dictParser.ParseXml<Jmdict>(jmdictPath);

            services.AddSingleton(jmdict);
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

            // Middleware to handle OPTIONS requests
            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Method == "OPTIONS")
                {
                    ctx.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                    ctx.Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                    ctx.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, x-api-key");
                    ctx.Response.StatusCode = 204;
                    await ctx.Response.CompleteAsync();
                }
                else
                {
                    await next();
                }
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
