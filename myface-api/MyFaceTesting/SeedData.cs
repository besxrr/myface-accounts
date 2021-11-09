using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MyFace;
using MyFace.Models.Database;

namespace MyFaceTesting
{
    public static class SeedData
    {
        public static void InitializeDbForTests(MyFaceDbContext db)
        {
            
            db.Posts.AddRange(GetSeedingPosts());
            db.Users.AddRange(GetSeedingUsers());
            db.SaveChanges();
        }

        public static List<Post> GetSeedingPosts()
        {
            return new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    Message = "testMessage",
                    UserId = 1,
                    User = null,
                    Interactions = null,
                    ImageUrl = "testURL",
                    PostedAt = DateTime.Now
                }
            };
        }
        
        public static List<User> GetSeedingUsers()
        {
            return new List<User>()
            {
                new User
                {
                    Id = 1,
                    FirstName = "Kania",
                    LastName = "Placido",
                    Username = "kplacido0",
                    HashedPassword = "QwlexniQID+XraloBUrXsl8jMfwaaBNvhzov/Yl2meI=",
                    Salt = "NLDCnN/5IyuhEYoFNc3k+w==",
                    Email = "kplacido0@qq.com",
                    ProfileImageUrl = "https://robohash.org/kplacido0?set=any&bgset=any",
                    CoverImageUrl = "https://picsum.photos/id/600/2400/900.jpg"
                }
            };
        }
    }
}