using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<Annonce>>> Get()
        {
            var annonces = await _antiGaspiContext.Annonces.ToArrayAsync();

            return Ok(_mapper.Map<IEnumerable<Annonce>>(annonces));
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var annonce = new Annonce { Id = id };
                _antiGaspiContext.Annonces.Remove(annonce);                
                var nbChanges = await _antiGaspiContext.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
    }
}