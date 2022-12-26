using BackEnd.API.Domains.Like.CommandHandlers;
using BackEnd.API.Domains.Like.Commands;
using BackEnd.API.Domains.Post.Commands;
using BackEnd.API.Models.Dtos;
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
        [Route("Post/like/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> likePost([FromRoute][BindRequired] int id)
        {
            PostResponse result = null; 
            try
            {
                result = await mediator.Send(new PostLikeCommand
                {
                    Id = id,
                    AuthorName = User.Identity.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("Post/dislike/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> dislikePost([FromRoute][BindRequired] int id)
        {
            PostResponse result = null;
            try
            {
                result = await mediator.Send(new PostDislikeCommand
                {
                    Id = id,
                    AuthorName = User.Identity.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("Comment/like/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> likeComment([FromRoute][BindRequired] int id)
        {
            CommentResponse result = null;
            try
            {
                result = await mediator.Send(new CommentLikeCommand
                {
                    Id = id,
                    AuthorName = User.Identity.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("Comment/dislike/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> dislikeComment([FromRoute][BindRequired] int id)
        {
            CommentResponse result = null;
            try
            {
                result = await mediator.Send(new CommentDislikeCommand
                {
                    Id = id,
                    AuthorName = User.Identity.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
