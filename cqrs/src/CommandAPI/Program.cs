using AnyService.Events.RabbitMQ;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var seqUrl = builder.Configuration["seq:url"] ?? throw new ArgumentNullException("seqUrl");

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Seq(seqUrl));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configure rabbitMQ
//var rmqOptions = new RabbitMqConfigurar().Configure(builder.Services, builder.Configuration);

////configure OpenTelemetry (using jaeger)
//builder.Services.AddOpenTelemetryTracing((builder) =>
//{
//    builder.AddAspNetCoreInstrumentation()
//        .AddHttpClientInstrumentation()
//        .AddSqlClientInstrumentation();

//    if (!rmqOptions.AppName.IsNullOrEmpty())
//        builder.AddSource(rmqOptions.AppName);

//    //to configure see here: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Jaeger/README.md#environment-variables
//    builder.AddJaegerExporter();
//});

var app = builder.Build();
var fho = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
};
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    var swaggerClientId = app.Configuration.GetValue<string>("openId:swaggerClientId");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {

        foreach (var desc in provider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint($"swagger/{desc.GroupName}/swager.json", $"{desc.ApiVersion} CQRS - Command API ");
            options.RoutePrefix = string.Empty;
        }
        options.DocumentTitle = "CQRS - Command API documentation";
        options.OAuthClientId(swaggerClientId);
        options.OAuthAppName("CQRS - Command API");
        options.OAuthUsePkce();
    });
}
var corsOrigins = app.Configuration.GetValue<string>("corsOrigins").Split(",");
if (corsOrigins.Any())
    app.UseCors(builder => builder
    .WithOrigins(corsOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
