using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NegotiationsAPI.Core.Interfaces;

namespace NegotiationsAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NegotiationsController : ControllerBase
    {
        private readonly INegotiationsRepository _negotiationsRepository;

        NegotiationsController(INegotiationsRepository negotiationsRepository)
        {
            _negotiationsRepository = negotiationsRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var negotiation = await _negotiationsRepository.GetByIdAsync(id);
            if (negotiation == null)
            {
                return NotFound();
            }
            return Ok(negotiation);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var negotiations = await _negotiationsRepository.GetAllAsync();
            return Ok(negotiations);
        }
    }
}
