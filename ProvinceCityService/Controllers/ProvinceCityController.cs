using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProvinceCityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceCityController : ControllerBase
    {
        private readonly IProvinceCityRepository _provinceCityRepos;

        public ProvinceCityController(IProvinceCityRepository provinceCityRepos)
        {
            _provinceCityRepos = provinceCityRepos;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _provinceCityRepos.GetAll();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _provinceCityRepos.GetById(id);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUpdate(ProvinceCity model)
        {
            var status = new Status();
            if (!ModelState.IsValid)
            {
                status.StatusCode = 0;
                status.Message = "Validatation failed";
            }
            var result = await _provinceCityRepos.AddUpdate(model);
            status.StatusCode = result ? 1 : 0;
            status.Message = result ? "Saved successfully" : "Error has occured";
            return Ok(status);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _provinceCityRepos.Delete(id);
            var status = new Status
            {
                StatusCode = result ? 1 : 0,
                Message = result ? "Deleted successfully" : "Error has occured"
            };
            return Ok(status);
        }
    }
}
