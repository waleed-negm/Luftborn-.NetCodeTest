using Core.Application.Dto;
using Core.Application.IunitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace luftbornCodeTestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Categories.GetAllAsync();
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("getById")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Categories.GetByIdAsync(id);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(CategoryDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Categories.AddAsync(model);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(CategoryDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Categories.Update(model);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Remove(Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Categories.Remove(id);
            if (!result.Status) return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("DeleteItems")]
        public async Task<IActionResult> DeleteItemsAsync(List<Guid> Ids)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ResponseDto result = await unitOfWork.Categories.DeleteItemsAsync(Ids);
            if (!result.Status) return BadRequest(result);
            return Ok(result);

        }
    }
}
