namespace Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.DbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            this.CreateUser(context);
        }

        private void CreateUser(DbContext context)
        {
            var manager = new UserManager<AspNetUser>(new UserStore<AspNetUser>(new Data.DbContext()));
            if (manager.Users.Count() == 0)
            {
                var roleManager = new RoleManager<AspNetRole>(new RoleStore<AspNetRole>(new Data.DbContext()));

                var user = new AspNetUser()
                {
                    UserName = "admin",
                    Email = "admin@example.com.vn",
                    EmailConfirmed = true,
                    Name = "Administrator",
                    Gender = true,
                    Status = true
                };
                if (manager.Users.Count(x => x.UserName == "admin") == 0)
                {
                    manager.Create(user, "Admin@123");

                    if (!roleManager.Roles.Any())
                    {
                        roleManager.Create(new AspNetRole { Name = "Admin", Description = "Administrator" });
                        roleManager.Create(new AspNetRole { Name = "Member", Description = "User" });
                    }

                    var adminUser = manager.FindByName("admin");

                    manager.AddToRoles(adminUser.Id, new string[] { "Admin", "Member" });
                }
            }
        }
    }
}
