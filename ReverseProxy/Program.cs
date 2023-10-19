using ReverseProxy;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ReverseProxyMiddleware>();

app.Run(async (context) =>
{
    await context.Response.WriteAsync("Hello!");
});

app.MapGet("/", () => "Hello World!");

app.Run();