using Microsoft.AspNetCore.Identity;

namespace SportsStore.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";

        public static async void EnsurePopulated(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            //UserManager<IdentityUser> userManager = app.ApplicationServices
            //    .GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser user = await userManager.FindByIdAsync(adminUser); 
            if (user == null)
            {
                user = new IdentityUser(adminUser);
                await userManager.CreateAsync(user, adminPassword);
            }
        }
    }
}
