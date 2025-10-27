using TareaZooplanet.Models.Entities;
using TareaZooplanet.Repositories;
using TareaZooplanet.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();

builder.Services.AddDbContext<AnimalesContext>();
builder.Services.AddScoped<ClasesServices>();
builder.Services.AddScoped<EspeciesServices>();
builder.Services.AddScoped(typeof(Repository<>), typeof(Repository<>));

var app = builder.Build();

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapDefaultControllerRoute();
app.UseStaticFiles();

app.Run();
