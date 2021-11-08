using System;

namespace MyFace.Data
{
    public class DateGenerator
    {
        private static Random _random = new Random();
        private static int _maxInteractionAge = 10000;
        
        public static DateTime GetPostDate()
        {
            // Posts happen longer ago than the max interaction age so that we don't risk
            // interacting with a post before it is created.
            var randomMinsAgo = _random.Next(1, _maxInteractionAge);
            return DateTime.Now.AddMinutes(-1 * (_maxInteractionAge + randomMinsAgo));
        }
        
        public static DateTime GetInteractionDate()
        {
            var randomMinsAgo = _random.Next(1, _maxInteractionAge);
            return DateTime.Now.AddMinutes(-1 * randomMinsAgo);
        }
    }
}