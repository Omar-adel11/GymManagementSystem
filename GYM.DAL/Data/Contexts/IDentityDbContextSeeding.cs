using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GYM.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GYM.DAL.Data.Contexts
{
    public class IDentityDbContextSeeding
    {
        public async static Task<bool> SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var HasUsers = userManager.Users.Any();
                var HasRoles = roleManager.Roles.Any();

                if (HasUsers && HasRoles) return false;
                
                if(!HasRoles)
                {
                    var Roles = new List<IdentityRole>() 
                    {
                        new(){Name = "SuperAdmin"},
                        new(){Name = "Admin"}
                    };

                    foreach(var Role in Roles)
                    {  
                        if(! await roleManager.RoleExistsAsync(Role.Name!))
                        {
                            await roleManager.CreateAsync(Role);
                        }
                    };
                }

                if(!HasUsers)
                {
                    var SuperAdmin = new ApplicationUser()
                    {
                        FirstName = "Gym",
                        LastName = "SuperAdmin",
                        UserName = "SuperAdmin",
                        Email = "omaradel1258@gmail.com",
                        PhoneNumber = "01287276101"
                    };

                    await userManager.CreateAsync(SuperAdmin,"P@ssw0rd");
                    await userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin");

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Gym",
                        LastName = "Admin",
                        UserName = "Admin",
                        Email = "o.adel1029@gmail.com",
                        PhoneNumber = "01062390839"
                    };

                    await userManager.CreateAsync(Admin, "P@ssw0rd");
                    await userManager.AddToRoleAsync(SuperAdmin, "Admin");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding failed : {ex}");
                return false;
            }

        }
    }
}
