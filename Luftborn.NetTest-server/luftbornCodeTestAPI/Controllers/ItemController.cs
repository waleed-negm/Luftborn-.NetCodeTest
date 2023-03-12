using Core.Application.Dto;
using Core.Application.IunitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace luftbornCodeTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ItemController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Items.GetAllAsync();
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("getById")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Items.GetByIdAsync(id);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost]
        public async Task <IActionResult> AddAsync(ItemDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Items.AddAsync(model);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpPut]
        public async Task <IActionResult> UpdateAsync(ItemDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Items.Update(model);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Remove(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Items.Remove(id);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
    }
}
