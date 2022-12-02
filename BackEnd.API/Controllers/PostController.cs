using BackEnd.API.Domains.Post.Commands;
using BackEnd.API.Domains.Post.Queries;
using BackEnd.Infrastructure.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class PostController : ControllerBase
    {
        private readonly ISender mediator;
        public PostController(ISender mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("/{page}")]
        public async Task<IActionResult> getPosts(int page = 1)
        {
            var result = await mediator.Send(new GetPostsQuery() { Page = page });
            if(result == null)
            {
                return BadRequest("Incorrect number of page");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("/{userName}/{page}")]
        public async Task<IActionResult>  getPostsByUserName([BindRequired][FromRoute] string userName, [FromRoute] int page = 1)
        {
            var result = await mediator.Send(new GetPostsByUserNameQuery() { UserName = userName, Page = page });
            if (result == null)
            {
                return BadRequest("Incorrect userName or number of page");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("/post/{id}")]
        public async Task<IActionResult> getPostById([BindRequired] int id)
        {
            var result = await mediator.Send(new GetPostByIdQuery() { Id = id });
            if(result == null)
            {
                return NotFound(id);
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> createPost(PostRequest request)
        {
            try
            {
                await mediator.Send(new CreatePostCommand
                {
                    Title = request.Title,
                    Context = request.Content,
                    AuthorName = User.Identity.Name
                });
            } catch(Exception ex)
            {
                return BadRequest();
            }
            
            return NoContent();
        }

        [HttpPut]
        [Route("/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> updatePost([BindRequired][FromRoute] int id, [FromBody] PostRequest request)
        {
            try
            {
                await mediator.Send(new UpdatePostCommand
                {
                    Id = id,
                    Title = request.Title,
                    Context = request.Content,
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
        [Route("/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> deletePost([BindRequired] int id)
        {
            try
            {
                await mediator.Send(new DeletePostCommand
                {
                    Id = id,
                    AuthorName = User.Identity.Name
                });
            } catch(Exception ex)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
