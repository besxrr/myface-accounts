using System;
using System.Collections.Generic;
using System.Linq;
using MyFace.Models.Database;

namespace MyFace.Data
{
    public static class SampleInteractions
    {
        public static IEnumerable<Interaction> GetInteractions()
        {
            return Enumerable.Range(1, 1000)
                .Select(_ => CreateRandomPostUserPair())
                .Distinct()
                .Select(CreateRandomInteraction);
        }

        private static Tuple<int, int> CreateRandomPostUserPair()
        {
            return new Tuple<int, int>(RandomNumberGenerator.GetPostId(), RandomNumberGenerator.GetUserId());
        }

        private static Interaction CreateRandomInteraction(Tuple<int, int> postUserPair)
        {
            return new Interaction
            {
                PostId = postUserPair.Item1,
                UserId = postUserPair.Item2,
                Type = RandomNumberGenerator.GetInteractionType(),
                Date = DateGenerator.GetPostDate(),
            };
        }
    }
}