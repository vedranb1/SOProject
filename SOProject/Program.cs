using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SOProject.DomainModels2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new SOProjectDbContext())
            {
                db.Users.Add(new User { Email = "wefok@gmail.com", Mobile = "98123434", Username = "test123", PasswordHash = "123" });
                db.SaveChanges();

                foreach (var user in db.Users)
                {
                    Console.WriteLine(user.Username);
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
