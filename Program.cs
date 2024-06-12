using JMDict;
using JmdictGQL;

Console.WriteLine("Start fetch");
var jmdict = await JmdictFetch.JmdictFetch.GetJmdict();
Console.WriteLine("Done");

if (jmdict is null)
{
    throw new Exception("Could not fetch jmdict");
}

var dictParser = new DictParser();

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

app.UseCors("AllowAny");

app.MapGraphQL();

app.Run();