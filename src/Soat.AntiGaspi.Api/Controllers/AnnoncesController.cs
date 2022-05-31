using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Soat.AntiGaspi.Api.Contracts;
using Soat.AntiGaspi.Api.Models;
using Soat.AntiGaspi.Api.Repository;

namespace Soat.AntiGaspi.Api.Controllers
{
    [ApiController]
    [Route("/api/annonces")]
    public class AnnoncesController : ControllerBase
    {
        private readonly ILogger<AnnoncesController> _logger;
        private readonly AntiGaspiContext _antiGaspiContext;
        private readonly IMapper _mapper;

        public AnnoncesController(
            AntiGaspiContext antiGaspiContext,
            IMapper mapper,
            ILogger<AnnoncesController> logger)
        {
            _logger = logger;
            _antiGaspiContext = antiGaspiContext;
            _mapper = mapper;
        }

        [HttpGet("")]
        public Task<IEnumerable<Annonce>> Get()
        {
            return Task.FromResult(
                Enumerable.Range(1, 5).Select(index => new Annonce())
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Annonce>> Get(Guid id)
        {
            Annonce? annonce = await _antiGaspiContext.Annonces.FindAsync(id);
            if (annonce is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Annonce>(annonce));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateAnnonceRequest createAnnonceRequest)
        {
            Annonce annonce = _mapper.Map<Annonce>(createAnnonceRequest);
            _antiGaspiContext.Add(annonce);
            await _antiGaspiContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = annonce.Id }, annonce);
        }

        [HttpPost("{id}/confirm")]
        public IActionResult Confirm(Guid id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            return Ok();
        }
    }
}