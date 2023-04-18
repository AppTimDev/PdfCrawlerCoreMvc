using System.Diagnostics;
using System.Net;
using WebAppMvc.Models;

namespace WebAppMvc.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApiDbContext>();
                if (context != null)
                {
                    context.Database.EnsureCreated();
                    if (!context.Users.Any())
                    {
                        context.Users.AddRange(new List<User>()
                        {
                            new User()
                            {
                                Id = new Guid(),
                                Name = "admin",
                                Pass = "123456",
                                CreateTime = DateTime.Now,
                                UpdateTime = DateTime.Now,
                                Age = 18
                            }
                        });
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
