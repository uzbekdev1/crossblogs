using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Domain;
using crossblog.Dto;
using crossblog.Extensions;
using crossblog.Model;
using crossblog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace crossblog.Controllers
{
    [Route("articles")]
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentsController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        }

        // GET articles/5/comments
        [HttpGet("{articleId}/[controller]")]
        public async Task<IActionResult> Get([FromRoute]int? articleId)
        {
            if (articleId == null)
                return BadRequest(ApiResponse.BadRequest("Argument id is null"));

            var model = await _commentRepository.Query().Where(w => w.ArticleId == articleId).ToArrayAsync();
            var result = new CommentListModel
            {
                Comments = model.Select(c => c.AsDto())
            };

            return Ok(ApiResponse.Ok(result));
        }

        // GET articles/{articleId}/comments/5
        [HttpGet("{articleId}/[controller]/{id}")]
        public async Task<IActionResult> Get([FromRoute]int? articleId, [FromRoute]int? id)
        {
            if (articleId == null)
                return BadRequest(ApiResponse.BadRequest($"Argument article id is null"));

            if (id == null)
                return BadRequest(ApiResponse.BadRequest($"Argument comment id is null"));

            var model = await _commentRepository.Query().FirstOrDefaultAsync(c => c.ArticleId == articleId && c.Id == id);

            if (model == null)
                return NotFound(ApiResponse.NotFound($"Article or Comment not found with id [{articleId},{id}]"));

            var result = model.AsDto();

            return Ok(ApiResponse.Ok(result));
        }

        // POST articles/5/comments
        [HttpPost("{articleId}/[controller]")]
        public async Task<IActionResult> Post([FromRoute]int? articleId, [FromBody]CommentModel model)
        {
            if (articleId == null)
                return BadRequest(ApiResponse.BadRequest($"Argument article id is null"));

            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.BadRequest(ModelState));
            }

            var entity = new Comment
            {
                ArticleId = articleId,
                Email = model.Email,
                Title = model.Title,
                Content = model.Content,
                Date = DateTime.UtcNow,
                Published = model.Published
            };

            await _commentRepository.InsertAsync(entity);

            var result = new CommentModel
            {
                Id = entity.Id,
                Email = entity.Email,
                Title = entity.Title,
                Content = entity.Content,
                Date = entity.Date,
                Published = entity.Published,
                ArticleId = articleId
            };

            return Ok(ApiResponse.Ok(result));
        }

    }
}