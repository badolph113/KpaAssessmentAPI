using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Services;
using WebApi.Web.Validators;

namespace WebApi.Web.Controllers
{
    [ApiController]
    [Route("api/v1/json-files")]
    public class JsonFilesController : ControllerBase, IJsonFilesController
    {
        private readonly IJsonFilesService _jsonDataService;

        public JsonFilesController(IJsonFilesService jsonDataService)
        {
            _jsonDataService = jsonDataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJsonFilesIdsAsync()
        {
            return Ok(await _jsonDataService.GetAllJsonFilesIdsAsync());
        }

        [HttpGet("{jsonFileId}")]
        public async Task<IActionResult> GetJsonFileByIdAsync(string jsonFileId)
        {
            return Ok(await _jsonDataService.GetJsonFileByIdAsync(jsonFileId));
        }

        [HttpPost]
        public async Task<IActionResult> UploadJsonFileAsync(IFormFile file)
        {
            var validator = new FileValidator();
            var validationResult = await validator.ValidateAsync(file);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            /*
             * If we expect large JSON files, we may consider processing the stream directly 
             * instead of reading it entirely into memory with ReadToEndAsync. 
             */
            string? content = null;
            using (var stream = new StreamReader(file.OpenReadStream()))
                content = await stream.ReadToEndAsync();

            var res = await _jsonDataService.ProcessJsonDataAsync(content);

            return Ok(res);
        }

        [HttpPut("{jsonFileId}")]
        public async Task<IActionResult> UpdateJsonFileAsync(string jsonFileId, [FromBody] object content)
        {
            var jsonFileIdValidator = new JsonFileIdValidator();
            var contentValidator = new ContentValidator();

            var idResult = await jsonFileIdValidator.ValidateAsync(jsonFileId);
            var contentResult = await contentValidator.ValidateAsync(content);

            if (!idResult.IsValid || !contentResult.IsValid)
            {
                var errors = idResult.Errors.Concat(contentResult.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            var res = await _jsonDataService.UpdateJsonFileAsync(jsonFileId, content.ToString());

            return Ok(res);
        }
    }
}