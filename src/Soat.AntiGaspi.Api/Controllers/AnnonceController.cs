using Microsoft.AspNetCore.Mvc;
using Soat.AntiGaspi.Api.Models;
using Soat.AntiGaspi.Api.Repository;

namespace Soat.AntiGaspi.Api.Controllers
{
    [ApiController]
    [Route("/api/annonces")]
    public class AnnonceController : ControllerBase
    {
        private readonly ILogger<AnnonceController> _logger;

        public AnnonceController(ILogger<AnnonceController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public Task<IEnumerable<Annonce>> Get()
        {
            return Task.FromResult(
                Enumerable.Range(1, 5).Select(index => new Annonce())
            );
        }

        [HttpGet("{id}")]
        public Task<Annonce> Get(string id)
        {
            return Task.FromResult(new Annonce());
        }

        [HttpPost("")]
        public IActionResult Create(Annonce annonce)
        {
            return CreatedAtAction(nameof(Get), new { id = annonce.Id }, annonce);
        }
    }
}