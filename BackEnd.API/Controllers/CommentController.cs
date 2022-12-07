using BackEnd.API.Domains.Comment.Commands;
using BackEnd.API.Domains.Comment.Queries;
using BackEnd.API.Domains.Post.Queries;
using BackEnd.Infrastructure.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class CommentController : ControllerBase
    {
        private readonly ISender mediator;
        public CommentController(ISender mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> getCommentsByPostId([BindRequired] int id)
        {
            var result = await mediator.Send(new GetCommentsByPostIdQuery() { PostId = id });
            return Ok(result);
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> createComment([BindRequired][FromRoute] int id, [FromBody] CommentRequest request)
        {
            try
            {
                await mediator.Send(new CreateCommentCommand
                {
                    PostId = id,
                    Message = request.Message,
                    AuthorName = User.Identity.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> deleteComment([BindRequired][FromRoute] int id)
        {
            try
            {
                await mediator.Send(new DeleteCommentCommand
                {
                    PostId = id,
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
