using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Database;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;
using static MyFace.Helpers.AuthenticationHelper;

namespace MyFace.Controllers
{
    [ApiController]
    [Route("/interactions")]
    public class InteractionsController : ControllerBase
    {
        private readonly IInteractionsRepo _interactions;
        private readonly IUsersRepo _users;

        public InteractionsController(IInteractionsRepo interactions, IUsersRepo users)
        {
            _interactions = interactions;
            _users = users;
        }

        [HttpGet("")]
        public ActionResult<ListResponse<InteractionResponse>> Search([FromQuery] SearchRequest search)
        {
            var interactions = _interactions.Search(search);
            var interactionCount = _interactions.Count(search);
            return InteractionListResponse.Create(search, interactions, interactionCount);
        }

        [HttpGet("{id}")]
        public ActionResult<InteractionResponse> GetById([FromRoute] int id)
        {
            var interaction = _interactions.GetById(id);
            return new InteractionResponse(interaction);
        }
        

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateInteractionRequest newInteraction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Authenticate user
            var userId = GetUserIdFromRequest(_users, Request);
            if (userId == null)
            {
                return StatusCode(401, "Authentication Failed!");
            }
            
            newInteraction.UserId = (int) userId;

            //Check if interaction already exists
            var foundId = _interactions.GetInteractionId(newInteraction);
            
            //If it already exists, delete interaction instead of creating it
            if (foundId != null)
            {
                _interactions.Delete((int) foundId);
                return StatusCode(204, "Interaction deleted");
            }
            
            //Search for existing interaction with opposite interaction
            var opposite = new CreateInteractionRequest
            {
                UserId = (int) userId,
                PostId = newInteraction.PostId, 
                InteractionType = newInteraction.InteractionType == InteractionType.LIKE ? InteractionType.DISLIKE : InteractionType.LIKE,
            };
            var foundOpposite = _interactions.GetInteractionId(opposite);
            
            // If opposite interaction found, delete it
            if (foundOpposite != null)
            {
                _interactions.Delete((int) foundOpposite);
            }
            
            //Then, create interaction
            var interaction = _interactions.Create(newInteraction);
            var url = Url.Action("GetById", new {id = interaction.Id});
            var responseViewModel = new InteractionResponse(interaction);
            return Created(url, responseViewModel);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            _interactions.Delete(id);
            return Ok();
        }
    }
}