using CodeNest.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeNest.BLL.Service
{
    public interface IWorkspaceService
    {
        Task<ValidationDto> CreateWorkspace(string name, string userId);
    }
}
