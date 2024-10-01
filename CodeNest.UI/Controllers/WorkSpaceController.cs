using CodeNest.BLL.Service;
using CodeNest.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CodeNest.UI.Controllers
{
    public class WorkSpaceController : Controller
    {
        private readonly IWorkspaceService _workspaceService;
        public WorkSpaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;   
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var userId = HttpContext.Session.GetString("UserID");
            var result = await _workspaceService.CreateWorkspace(name,userId);

            if (result.IsValid)
            {
                HttpContext.Session.SetString("WorkspaceId", result.objectID);
                TempData["Success"] = "Created the workspcae !";
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["Error"] = "Error occured while creating workspcae !";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
