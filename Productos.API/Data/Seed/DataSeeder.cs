using Productos.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Productos.API.Data.Seed
{
    public static class DataSeeder
    {
        public async static Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var role in Roles.Todos)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = role
                    });

            }

            await SeedUsuarioAsync(userManager, "creador@productos.com", "Creador123!", Roles.Creador);
            await SeedUsuarioAsync(userManager, "visualizador@productos.com", "Visualizador123!", Roles.Visualizador);
        }

        private async static Task SeedUsuarioAsync(UserManager<ApplicationUser> userManager, string email, string password, string rol)
        {
            if (await userManager.FindByEmailAsync(email) is null)
            {
                //Que el usuario no existe
                var usuario = new ApplicationUser() { UserName = email, Email = email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(usuario, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(usuario, rol);
                }
            }
        }
    }


    public static class Roles
    {
        public static string Creador = "Creador";
        public static string Visualizador = "Visualizador";

        public static List<string> Todos = new List<string>() { Creador, Visualizador };
    }
}
