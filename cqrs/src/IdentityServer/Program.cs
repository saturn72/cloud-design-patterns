using IdentityServer;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var seqUrl = builder.Configuration["seq:url"] ?? throw new ArgumentNullException("seqUrl");

builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Seq(seqUrl));

builder.Services
    .AddIdentityServer()
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

var app = builder.Build();

var fho = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
};
fho.KnownNetworks.Clear();
fho.KnownProxies.Clear();
app.UseForwardedHeaders(fho);
app.UseHttpsRedirection();
app.UseIdentityServer();
app.Run();
