using WebApi.Domain.Entities;

namespace WebApi.Application.Services
{
    public interface IJsonFilesService
    {
        Task<JsonFile> ProcessJsonDataAsync(string content);
        Task<JsonFile> GetJsonFileByIdAsync(string jsonFileId);
        Task<JsonFile> UpdateJsonFileAsync(string jsonFileId, string content);
        Task<List<string>> GetAllJsonFilesIdsAsync();
    }

}
