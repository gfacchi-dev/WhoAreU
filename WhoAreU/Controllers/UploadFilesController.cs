using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WhoAreU.Models;

namespace WhoAreU.Controllers
{
    public class UploadFilesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;
        public UploadFilesController(UserManager<ApplicationUser> userManager, IHostingEnvironment IHostingEnvironment)
        {
            _environment = IHostingEnvironment;
            _userManager = userManager;
        }


        [Authorize]
        [HttpPost("/UploadFiles")]
        public async Task<IActionResult> UploadPropic(IFormFile file)
        {
            if (file == null || file.Length == 0) return Content("file not selected");

            var fileName = file.FileName.Trim('"');
            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
            var FileExtension = Path.GetExtension(fileName);
            var newFileName = myUniqueFileName + FileExtension;
            fileName = Path.Combine(_environment.WebRootPath, "propics") + $@"\{newFileName}";
            var PathDB = "../propics/" + newFileName;

            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user '{user.UserName}'.");
            }

            user.AvatarImage = PathDB;

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Redirect("Profile/" + user.UserName);
            }

            return View();
        }
    }
}