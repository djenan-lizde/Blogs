using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Posts.Contracts.Responses;
using Posts.Repository;
using System;
using System.Collections.Generic;

namespace Posts.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ILogger<TagsController> _logger;
        private readonly ITagRepository _repositoryTag;
        public TagsController(
            ILogger<TagsController> logger,
            ITagRepository repositoryTag
            )
        {
            _logger = logger;
            _repositoryTag = repositoryTag;
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<Tag>))]
        public IActionResult GetTags()
        {
            try
            {
                return Ok(_repositoryTag.Get());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500);
            }
        }
    }
}
