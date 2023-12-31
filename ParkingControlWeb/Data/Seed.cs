using Microsoft.AspNetCore.Identity;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Models;
using System.Net;

namespace ParkingControlWeb.Data
{
	public class Seed
	{

		public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				//Roles
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				if (!await roleManager.RoleExistsAsync(Role.GlobalAdmin))
					await roleManager.CreateAsync(new IdentityRole(Role.GlobalAdmin));

				if (!await roleManager.RoleExistsAsync(Role.SystemAdmin))
					await roleManager.CreateAsync(new IdentityRole(Role.SystemAdmin));

				if (!await roleManager.RoleExistsAsync(Role.Expert))
					await roleManager.CreateAsync(new IdentityRole(Role.Expert));

				if (!await roleManager.RoleExistsAsync(Role.Driver))
					await roleManager.CreateAsync(new IdentityRole(Role.Driver));

				//Users
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

				string adminUserEmail = "parkingcontrolweb@gmail.com";

				var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
				if (adminUser == null)
				{
					var newAdminUser = new User()
					{
						Id = "1dd5ed28-b346-4d5c-8f17-10877c82e6b0",
						UserName = "Admin",
						Email = adminUserEmail,
						EmailConfirmed = true
					};
					await userManager.CreateAsync(newAdminUser, "?C0n4xu9=Y+t\",j7]&Q?xF'Sy4Ftwq76Bn=GLvj#A7cKeQ:yr@");
					await userManager.AddToRoleAsync(newAdminUser, Role.GlobalAdmin);
				}

				string ownerEmail = "owner@gmail.com";

				var ownerUser = await userManager.FindByEmailAsync(ownerEmail);
				if (ownerUser == null)
				{
					var newOwner = new User()
					{
						Id = "24209a88-b820-47af-9673-b56d5246fd1a",
						UserName = "Owner",
						Email = ownerEmail,
						EmailConfirmed = true
					};
					await userManager.CreateAsync(newOwner, "Owner_1_");
					await userManager.AddToRoleAsync(newOwner, Role.SystemAdmin);
				}

				string expertEmail = "expert@gmail.com";

				var expertUser = await userManager.FindByEmailAsync(expertEmail);
				if (expertUser == null)
				{
					var newExpert = new User()
					{
						Id = "32eb1f7e-991e-4449-baf7-a97e0bacf919",
						UserName = "Expert",
						Email = expertEmail,
						EmailConfirmed = true
					};
					await userManager.CreateAsync(newExpert, "Expert_1_");
					await userManager.AddToRoleAsync(newExpert, Role.Expert);
				}

				string driverEmail = "driver@gmail.com";

				var driverUser = await userManager.FindByEmailAsync(driverEmail);
				if (driverUser == null)
				{
					var newDriver = new User()
					{
						Id = "509d011e-cda3-4e66-882a-9423275b79aa",
						UserName = "Driver",
						Email = driverEmail,
						EmailConfirmed = true
					};
					await userManager.CreateAsync(newDriver, "Driver_1_");
					await userManager.AddToRoleAsync(newDriver, Role.Driver);
				}
			}
		}
	}

}
