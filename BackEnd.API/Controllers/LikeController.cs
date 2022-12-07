using BackEnd.API.Domains.Like.CommandHandlers;
using BackEnd.API.Domains.Like.Commands;
using BackEnd.API.Domains.Post.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class LikeController : ControllerBase
    {
        private readonly ISender mediator;
        public LikeController(ISender mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("/post/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> likePost([FromRoute][BindRequired] int id)
        {
            try
            {
                await mediator.Send(new PostLikeCommand
                {
                    Id = id,
                    AuthorName = User.Identity.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
