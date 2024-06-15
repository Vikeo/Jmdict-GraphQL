using JMDict;
using JmdictGQL;

public class Program
{
    public static void Main(string[] args)
    {

        // Console.WriteLine("Start fetch");
        // var jmdict = await JmdictFetch.JmdictFetch.GetRemoteJmdict();
        // Console.WriteLine("Done");

        // if (jmdict is null)
        // {
        //     throw new Exception("Could not fetch jmdict");
        // }

        var jmdictPath = "dictionaries/JMdict_e.xml";
        var dictParser = new DictParser();
        var jmdict = dictParser.ParseXml<Jmdict>(jmdictPath);

        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddSingleton(jmdict);


        builder.Services
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

        var app = builder.Build();

        // Read the port from the PORT environment variable
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
        app.Urls.Add($"http://*:{port}");

        Console.WriteLine($"Application is listening on port {port}");

        app.UseCors("AllowAny");

        app.MapGraphQL();

        app.Run();
    }
}