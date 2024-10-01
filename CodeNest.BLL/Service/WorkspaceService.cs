using AutoMapper;
using CodeNest.DAL.Context;
using CodeNest.DAL.Models;
using CodeNest.DTO.Models;
using MongoDB.Driver;
namespace CodeNest.BLL.Service
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly MangoDbService _mongoService;
        private readonly IMapper _mapper;
        public WorkspaceService(MangoDbService mongoService, IMapper mapper)
        {
            _mongoService = mongoService;
            _mapper = mapper;
        }

        public async Task<ValidationDto> CreateWorkspace(string name , string userId)
        {
            try
            {
                var workspaceDto = new WorkspacesDto();
                workspaceDto.Name = name + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                workspaceDto.CreatedBy = userId;
                workspaceDto.CreatedOn = DateTime.Now.ToString();
                var workspace = _mapper.Map<Workspaces>(workspaceDto);
                try
                {
                    await _mongoService.workSpaces.InsertOneAsync(workspace);
                    var workspaceId = workspace.Id.ToString(); // Make sure workspace.Id is of type ObjectId

                    var userDetail = await _mongoService.userModel.Find(x=>x.Id == userId).FirstOrDefaultAsync();
                    if (userDetail != null) 
                    {
                        userDetail.Workspaces.Add(workspace.Id);
                        var updateDefinition = Builders<Users>.Update.Set(u => u.Workspaces, userDetail.Workspaces);
                        await _mongoService.userModel.UpdateOneAsync(x => x.Id == userId, updateDefinition);
                    }
                }
                catch
                {
                    return new ValidationDto
                    {
                        IsValid = false,
                        Message = "Error occured while creating Namespace!",
                        
                    };
                }
               
                return new ValidationDto
                {
                    IsValid = true,
                    Message ="Created Namespace successfully!",
                    objectID = workspace.Id
                   
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ValidationDto
                {
                    IsValid = false,
                    Message = "Error occured while creating Namespace!",
                };
            }
        }
    }
}
