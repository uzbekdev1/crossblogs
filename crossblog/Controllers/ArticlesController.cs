using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
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
    [Route("[controller]")]
    public class ArticlesController : Controller
    {
        private readonly IArticleRepository _articleRepository;

        public ArticlesController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        }

        // GET articles/search
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest(ApiResponse.BadRequest("Argument title is null"));

            var model = await _articleRepository.Query()
                .Where(a => a.Title.Contains(title) || a.Content.Contains(title))
                .OrderByDescending(a => a.Date)
                .Take(20)
                .ToArrayAsync();

            var result = new ArticleListModel
            {
                Articles = model.Select(a => a.AsDto())
            };

            return Ok(ApiResponse.Ok(result));
        }

        // GET articles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null)
                return BadRequest(ApiResponse.BadRequest("Argument id is null"));

            var model = await _articleRepository.GetAsync(id);

            if (model == null)
                return NotFound(ApiResponse.NotFound($"Article not found with id {id}"));

            var result = model.AsDto();

            return Ok(ApiResponse.Ok(result));
        }

        // POST articles
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ArticleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.BadRequest(ModelState));
            }

            var entity = new Article
            {
                Title = model.Title,
                Content = model.Content,
                Date = model.Date,
                Published = model.Published
            };

            await _articleRepository.InsertAsync(entity);

            return Ok(ApiResponse.Ok(entity.AsDto()));
        }

        // PUT articles/5
        [HttpPut("{id}")]
        //[ApiValidationFilter]
        public async Task<IActionResult> Put(int? id, [FromBody]ArticleModel model)
        {
            if (id == null)
                return BadRequest(ApiResponse.BadRequest("Argument id is null"));

            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse.BadRequest(ModelState));
            }

            var entity = await _articleRepository.GetAsync(id);

            if (entity == null)
                return NotFound(ApiResponse.NotFound($"Article not found with id {id}"));

            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.Date = DateTime.UtcNow;
            entity.Published = model.Published;

            await _articleRepository.UpdateAsync(entity);

            return Ok(ApiResponse.Ok());
        }

        // DELETE articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest(ApiResponse.BadRequest("Argument id is null"));

            var model = await _articleRepository.GetAsync(id);

            if (model == null)
                return NotFound(ApiResponse.NotFound($"Article not found with id {id}"));

            await _articleRepository.DeleteAsync(model);

            return Ok(ApiResponse.Ok());
        }

    }
}