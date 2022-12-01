using BackEnd.API.Domains.Post.Commands;
using BackEnd.API.Domains.Post.Queries;
using BackEnd.API.Domains.Post.QueryHandlers;
using BackEnd.Infrastructure.Models.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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
                    Context = request.Content,
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
