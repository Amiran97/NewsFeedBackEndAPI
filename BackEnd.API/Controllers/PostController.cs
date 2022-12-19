using BackEnd.API.Domains.Post.Commands;
using BackEnd.API.Domains.Post.Queries;
using BackEnd.API.Models;
using BackEnd.API.Models.Dtos;
using BackEnd.API.Models.Dtos.Validators;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackEnd.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    //[Produces("application/json")]
    public class PostController : ControllerBase
    {
        private readonly ISender mediator;
        public PostController(ISender mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> getPosts([FromQuery] PostsRequest request)
        {
            PostsResponse result = null;
            if(request.UserName != null)
            {
                result = await mediator.Send(new GetPostsByUserNameQuery() { Page = request.Page, UserName = request.UserName, Option = request.Option });
            } else if(request.Tag != null)
            {
                result = await mediator.Send(new GetPostsByTagQuery() { Page = request.Page, Tag = request.Tag, Option = request.Option });
            } else
            {
                result = await mediator.Send(new GetPostsQuery() { Page = request.Page, Option = request.Option });
            }
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("detail/{id}")]
        public async Task<IActionResult> getPostById([BindRequired] int id)
        {
            var result = await mediator.Send(new GetPostByIdQuery() { Id = id });
            if (result == null)
            {
                return NotFound(id);
            }
            return Ok(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> createPost([FromForm] PostRequest request)
        {
            PostResponse result = null;
            try
            {
                IFormFileCollection formFiles = HttpContext.Request.Form.Files;
                result = await mediator.Send(new CreatePostCommand
                {
                    Title = request.Title,
                    Context = request.Content,
                    AuthorName = User.Identity.Name,
                    Tags = request.Tags,
                    Images = formFiles
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
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
                    AuthorName = User.Identity.Name,
                    Tags = request.Tags
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
        public async Task<IActionResult> deletePost([BindRequired] int id)
        {
            try
            {
                await mediator.Send(new DeletePostCommand
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
