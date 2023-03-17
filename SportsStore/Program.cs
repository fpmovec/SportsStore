using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(o => o.EnableEndpointRouting = false);
builder.Services.AddTransient<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
builder.Services.AddDbContext<ApplicationDbContext>
(
    o => o.UseSqlServer(ConfigurationExtensions.GetConnectionString(builder.Configuration, "ProductConnection")));


builder.Services.AddMemoryCache();
builder.Services.AddSession();

var app = builder.Build();
app.Services.CreateScope();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseSession();
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
