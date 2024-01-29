using Microsoft.AspNetCore.Mvc;

namespace WebApi.Web.Controllers
{
    internal interface IJsonFilesController
    {
        Task<IActionResult> GetJsonFileByIdAsync(string jsonFileId);
        Task<IActionResult> GetJsonFilesIdsAsync();
        Task<IActionResult> UpdateJsonFileAsync(string jsonFileId, [FromBody] object dto);
        Task<IActionResult> UploadJsonFileAsync(IFormFile file);
    }
}