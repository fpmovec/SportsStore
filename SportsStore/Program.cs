using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(o => o.EnableEndpointRouting = false);
builder.Services.AddTransient<IProductRepository, EFProductRepository>();
builder.Services.AddDbContext<ApplicationDbContext>
(
    o => o.UseSqlServer(ConfigurationExtensions.GetConnectionString(builder.Configuration, "ProductConnection")));
var app = builder.Build();
app.Services.CreateScope();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseStatusCodePages();
app.UseMvc(routes => {
    routes.MapRoute(
        name: "pagination",
        template: "Products/Page{productPage}",
        defaults: new { Controller = "Product", action = "List"});
    });
SeedData.EnsurePopulated(app);
app.Run();
