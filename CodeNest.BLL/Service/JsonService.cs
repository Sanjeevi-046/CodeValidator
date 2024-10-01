using CodeNest.DAL.Context;
using CodeNest.DAL.Models;
using CodeNest.DTO.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodeNest.BLL.Service
{
    public class JsonService : IJsonService
    {
        private readonly MangoDbService _mongodbService;
        public JsonService(MangoDbService mongodbService)
        {
            _mongodbService = mongodbService;
        }
        public async Task<ValidationDto> Validate(string jsonObject)
        {
            if (string.IsNullOrWhiteSpace(jsonObject))
            {
                return new ValidationDto { IsValid = false, Message = "Not Valid Json" };
            }
            jsonObject = jsonObject.Trim();
            if (jsonObject.StartsWith("{") && jsonObject.EndsWith("}") ||
                jsonObject.StartsWith("[") && jsonObject.EndsWith("]"))
            {

                try
                {
                    var parsedJson = JToken.Parse(jsonObject);

                    string beautifiedJson = parsedJson.ToString(Formatting.Indented);

                    return new ValidationDto
                    {
                        IsValid = true,
                        Message = "Valid JSON",
                        jsonDto = new JsonDto
                        {
                            JsonInput = jsonObject,
                            JsonOutput = beautifiedJson
                        }
                    };
                }
                catch (JsonReaderException ex)
                {
                    return new ValidationDto
                    {
                        IsValid = false,
                        Message = ex.ToString(),
                        jsonDto = new JsonDto
                        {
                            JsonInput = jsonObject
                        }
                    };
                }

            }
            else
            {
                return new ValidationDto
                {
                    IsValid = false,
                    Message = "Not a Valid Json",
                    jsonDto = new JsonDto
                    {
                        JsonInput = jsonObject
                    }

                };
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonInput"></param>
        /// <param name="workspaceId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ValidationDto> Save (string jsonInput , string workspaceId , string userId)
        {
            var workspaceName = await _mongodbService.workSpaces.Find(x=>x.Id == workspaceId).FirstOrDefaultAsync();
            var name = workspaceName.Name.Split('_')[0];
            var jsonData = new Json
            {
                Name = name +"_"+DateTime.Now.ToString("yyyyMMdd") +".json",
                JsonInput = jsonInput,
                Workspaces = workspaceId,
                CreatedBy = userId,
                CreatedOn = userId,
            };

            try
            {
                await _mongodbService.json.InsertOneAsync(jsonData);
                return new ValidationDto
                {
                    IsValid = true,
                    Message = "Data saved"
                };
            }
            catch (Exception ex) 
            {
                return new ValidationDto
                {
                    IsValid = false,
                    Message = "Error Ocuured"
                };
            }


        }
    }
}
