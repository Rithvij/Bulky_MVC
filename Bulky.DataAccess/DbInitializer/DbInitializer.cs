using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }


        public void Initialize()
        {


            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex) { }



            //create roles if they are not created
            //if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            //{
            //    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
            //    _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            //    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
            //    _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();


            //    //if roles are not created, then we will create admin user as well
            //    _userManager.CreateAsync(new ApplicationUser
            //    {
            //        UserName = "admin@Rithvij.com",
            //        Email = "admin@Rithvij.com",
            //        Name = "Rithvij Pasupuleti",
            //        PhoneNumber = "1112223333",
            //        StreetAddress = "test 123 st",
            //        State = "KS",
            //        PostalCode = "23422",
            //        City = "Lawrence"
            //    }, "Admin123*").GetAwaiter().GetResult();


            //    ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@Rithvij.com");
            //    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

            //}
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();

                // If roles are not created, then we will create admin user as well
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@Rithvij.com",
                    Email = "admin@Rithvij.com",
                    Name = "Rithvij Pasupuleti",
                    PhoneNumber = "1112223333",
                    StreetAddress = "test 123 st",
                    State = "KS",
                    PostalCode = "23422",
                    City = "Lawrence"
                };

                var result = _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();

                // Check if the user creation was successful
                if (result.Succeeded)
                {
                    // Mark email as confirmed
                    adminUser.EmailConfirmed = true;

                    // Save changes to confirm email
                    _db.SaveChanges(); // This will update the database to confirm the email

                    // Add the admin user to the Admin role
                    _userManager.AddToRoleAsync(adminUser, SD.Role_Admin).GetAwaiter().GetResult();
                }
            }


            return;
        }
    }
}