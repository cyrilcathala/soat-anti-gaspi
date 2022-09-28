using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Soat.AntiGaspi.Api.Contracts;
using Soat.AntiGaspi.Api.Models;
using Soat.AntiGaspi.Api.Repository;

namespace Soat.AntiGaspi.Api.Controllers
{
    [ApiController]
    [Route("/api/offers")]
    public class OffersController : ControllerBase
    {
        private readonly ILogger<OffersController> _logger;
        private readonly AntiGaspiContext _antiGaspiContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public OffersController(
            AntiGaspiContext antiGaspiContext,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<OffersController> logger)
        {
            _logger = logger;
            _antiGaspiContext = antiGaspiContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOfferResponse>> Get(Guid id)
        {
            Offer? offer = await _antiGaspiContext.Offers.FindAsync(id);
            if (offer is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetOfferResponse>(offer));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateOfferRequest createOfferRequest)
        {
            Offer offer = _mapper.Map<Offer>(createOfferRequest);
            _antiGaspiContext.Add(offer);
            await _antiGaspiContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = offer.Id }, offer.Id);
        }
    }
}