using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFace.Data;
using MyFace.Models.Database;
using static MyFace.Helpers.PasswordHashHelper;

namespace MyFace
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var header = EncodeBase64(new BasicAuth("test-user", "secret"));
            Console.WriteLine(header);
            var decoded = DecodeBase64(header);
            Console.WriteLine($"Username: {decoded.Username}, Password: {decoded.Password}");
            

            CreateDbIfNotExists(host);
            
            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<MyFaceDbContext>();
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                var users = SampleUsers.GetUsers();
                context.Users.AddRange(users);
                context.SaveChanges();

                var posts = SamplePosts.GetPosts();
                context.Posts.AddRange(posts);
                context.SaveChanges();

                var interactions = SampleInteractions.GetInteractions();
                context.Interactions.AddRange(interactions);
                context.SaveChanges();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}