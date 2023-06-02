using InmobiliariaV2;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

//builder.WebHost.UseUrls("http://192.168.0.103:5029", "http://*:7281");
builder.WebHost.UseUrls("http://localhost:5029", "http://192.168.0.104:5029", "http://*:5029");

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();
