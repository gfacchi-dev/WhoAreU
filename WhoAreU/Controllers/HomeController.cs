using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WhoAreU.Models;
using WhoAreU.Extensions;


namespace WhoAreU.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;
        private readonly SocialNetworkDBContext _socialNetworkDBContext;
        public HomeController(UserManager<ApplicationUser> userManager, IHostingEnvironment IHostingEnvironment, SocialNetworkDBContext socialNetworkDBContext)
        {
            _environment = IHostingEnvironment;
            _userManager = userManager;
            _socialNetworkDBContext = socialNetworkDBContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {        
            var user = await _userManager.GetUserAsync(User);
            if(user == null)
            {
                throw new ApplicationException($"Unable to load user '{user.UserName}'.");
            }
            return View(new ProfileViewModel {
                Username = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Propic = user.AvatarImage,
                Posts = _socialNetworkDBContext.GetAllPosts(user)
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(string Post)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            _socialNetworkDBContext.WritePost(new Post { Body = Post, Fkuser = currentUser.Id });
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
