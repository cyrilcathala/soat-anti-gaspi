using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using Soat.AntiGaspi.Api.Contracts;
using Soat.AntiGaspi.Api.Contracts.Paging;
using Soat.AntiGaspi.Api.Models;
using Soat.AntiGaspi.Api.Repository;
using System.Linq.Expressions;

namespace Soat.AntiGaspi.Api.Controllers
{
    [ApiController]
    [Route("/api/offers")]
    public class OffersController : ControllerBase
    {
        private readonly ILogger<OffersController> _logger;
        private readonly AntiGaspiContext _antiGaspiContext;
        private readonly IMapper _mapper;
        private readonly ISendGridClient _sendGridClient;

        private delegate IOrderedQueryable<TSource> OrderByAscendingOrDescending<TSource, TKey>(IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector);

        public OffersController(
            AntiGaspiContext antiGaspiContext,
            IMapper mapper,
            ISendGridClient sendGridClient,
            ILogger<OffersController> logger)
        {
            _logger = logger;
            _antiGaspiContext = antiGaspiContext;
            _mapper = mapper;
            this._sendGridClient = sendGridClient;
        }

        [HttpGet]
        public async Task<ActionResult<GetOffersResponse>> Get([FromQuery] PagingParameters pagingParameters)
        {
            var offersQuery = _antiGaspiContext
                .Offers
                .Where(offer => offer.Status == OfferStatus.Active);

            var total = await offersQuery.CountAsync();

            if (!string.IsNullOrWhiteSpace(pagingParameters.SortBy))
            {
                offersQuery = GetOrderedQuery(offersQuery, pagingParameters.SortBy, pagingParameters.SortOrder);
            }

            var offers = await offersQuery
                .Take(pagingParameters.PageSize)
                .Skip(pagingParameters.PageNumber * pagingParameters.PageSize)
                .ToArrayAsync();

            var offersResponse = new GetOffersResponse
            {
                Items = _mapper.Map<IReadOnlyCollection<GetOfferResponse>>(offers),
                Total = total
            };

            return Ok(offersResponse);

            IQueryable<Offer> GetOrderedQuery(IQueryable<Offer> offersQuery, string sortBy, SortOrder? sortOrder)
            {
                if (pagingParameters.SortOrder == SortOrder.Descending)
                {
                    return pagingParameters.SortBy.ToLower() switch
                    {
                        "availability" => offersQuery.OrderByDescending(o => o.Availability),
                        "companyname" => offersQuery.OrderByDescending(o => o.CompanyName),
                        "title" => offersQuery.OrderByDescending(o => o.Title),
                        "expiration" => offersQuery.OrderByDescending(o => o.Expiration),
                        _ => offersQuery
                    };
                }

                return pagingParameters.SortBy.ToLower() switch
                {
                    "availability" => offersQuery.OrderBy(o => o.Availability),
                    "companyname" => offersQuery.OrderBy(o => o.CompanyName),
                    "title" => offersQuery.OrderBy(o => o.Title),
                    "expiration" => offersQuery.OrderBy(o => o.Expiration),
                    _ => offersQuery
                };
            }
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

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            Offer? offer = await _antiGaspiContext.Offers.FindAsync(id);
            if (offer is null)
            {
                return NotFound();
            }

            offer.Status = OfferStatus.Active;
            await _antiGaspiContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var offer = new Offer { Id = id };
                _antiGaspiContext.Offers.Remove(offer);
                await _antiGaspiContext.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }


        [HttpPost("{id}/contact")]
        public async Task<IActionResult> Contact(Guid id, [FromBody] ContactRequest contactRequest)
        {
            Offer? offer = await _antiGaspiContext.Offers.FindAsync(id);
            if (offer is null)
            {
                return NotFound();
            }

            var contactOffer = _mapper.Map<ContactOffer>(contactRequest);
            contactOffer.OfferId = id;
            
            _antiGaspiContext.Add(contactOffer);
            await _antiGaspiContext.SaveChangesAsync();

            var mail = new SendGridMessage
            {
                From = new EmailAddress("antigaspi@soat.fr"),
                Subject = "Coucou toi"
            };
            mail.AddContent(MimeType.Html, "<html>Salut en <strong>gras</strong></html>");
            mail.AddTo(offer.Email);
            await _sendGridClient.SendEmailAsync(mail);

            return Ok();
        }
    }
}