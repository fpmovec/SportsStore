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
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List" }
                );

    routes.MapRoute(
        name: null,
        template: "Page{productPage:int}",
        defaults: new { controller = "Product", action = "List", productPage = 1 }
    );

    routes.MapRoute(
        name: null,
        template: "{category}",
        defaults: new { controller = "Product", action = "List", productPage = 1 }
    );

    routes.MapRoute(
        name: null,
        template: "",
        defaults: new { controller = "Product", action = "List", productPage = 1 });

    routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
});
SeedData.EnsurePopulated(app);
app.Run();
