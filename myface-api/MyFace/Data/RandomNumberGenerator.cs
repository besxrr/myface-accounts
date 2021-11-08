using System;
using MyFace.Models.Database;

namespace MyFace.Data
{
    public class RandomNumberGenerator
    {
        private static Random _random = new Random();
        
        public static int GetUserId()
        {
            return _random.Next(1, SampleUsers.NumberOfUsers + 1);
        }
        
        public static int GetPostId()
        {
            return _random.Next(1, SamplePosts.NumberOfPosts + 1);
        }

        public static InteractionType GetInteractionType()
        {
            return _random.Next(0, 2) == 0 ? InteractionType.LIKE : InteractionType.DISLIKE;
        }
    }
}