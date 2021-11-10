using System.Collections.Generic;

namespace MyFace.Models.Response
{
    public class InteractionCount
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
    public class GetInteractionCountResponse
    {
        private readonly InteractionCount _interactionCount;

        public GetInteractionCountResponse(InteractionCount interactionCount)
        {
            _interactionCount = interactionCount;
        }

        public int Likes => _interactionCount.Likes;
        public int Dislikes => _interactionCount.Dislikes;

    }
}