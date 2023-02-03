using ItemsAdministration.WebHost;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder);
var app = startup.Initialize();
app.Run();
