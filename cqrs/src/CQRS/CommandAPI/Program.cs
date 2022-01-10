using AnyService.Events.RabbitMQ;
using Microsoft.AspNetCore.HttpOverrides;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configure rabbitMQ
var rmqOptions = new RabbitMqConfigurar().Configure(builder.Services, builder.Configuration);

//configure OpenTelemetry (using jaeger)
builder.Services.AddOpenTelemetryTracing((builder) =>
{
    builder.AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSqlClientInstrumentation();

    if (!rmqOptions.AppName.IsNullOrEmpty())
        builder.AddSource(rmqOptions.AppName);

    //to configure see here: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Jaeger/README.md#environment-variables
    builder.AddJaegerExporter();
});

var app = builder.Build();

var fho = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
};
fho.KnownNetworks.Clear();
fho.KnownProxies.Clear();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
