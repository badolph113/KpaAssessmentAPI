using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApi.Application.Exceptions;
using WebApi.Domain.Entities;
using WebApi.Infrastructure;

namespace WebApi.Application.Services
{
    public class JsonFilesService : IJsonFilesService
    {
        /*
         * We should use a Repository Pattern + Unit of Work Pattern instead of directly accessing 
         * the context in a prod env, Especiailyy if we have multible entities.
         * For now let's apply KISS
         */

        private readonly ApplicationDbContext _dbContext;

        public JsonFilesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> GetAllJsonFilesIdsAsync()
        {
            return await _dbContext.JsonFiles.Select(x => x.Id).ToListAsync();
        }

        public async Task<JsonFile> GetJsonFileByIdAsync(string jsonFileId)
        {
            var entity = await _dbContext.JsonFiles.FirstOrDefaultAsync(x => x.Id == jsonFileId);
            if (entity == null)
                throw new EntityNotFoundException(jsonFileId);

            return entity;
        }

        public async Task<JsonFile> ProcessJsonDataAsync(string content)
        {
            var parsedData = JsonConvert.DeserializeObject(content);
            if (parsedData == null)
                throw new ValidationException("JSON data is invalid");

            var entity = new JsonFile() { JsonData = JsonConvert.SerializeObject(parsedData, Formatting.Indented) };

            _dbContext.JsonFiles.Add(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<JsonFile> UpdateJsonFileAsync(string jsonFileId, string content)
        {
            var entity = await _dbContext.JsonFiles.FirstOrDefaultAsync(x => x.Id == jsonFileId);
            if (entity == null)
                throw new EntityNotFoundException(jsonFileId);

            var parsedData = JsonConvert.DeserializeObject(content);
            if (parsedData == null)
                throw new ValidationException("JSON data is invalid");

            entity.JsonData = JsonConvert.SerializeObject(parsedData, Formatting.Indented); 

            _dbContext.JsonFiles.Update(entity);

            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
