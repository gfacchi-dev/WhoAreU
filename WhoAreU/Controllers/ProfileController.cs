using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WhoAreU.Models;
using WhoAreU.Extensions;

namespace WhoAreU.Controllers
{
    [Route("[controller]/{Username}")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SocialNetworkDBContext _socialNetworkDBContext;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            SocialNetworkDBContext socialNetworkDBContext)
        {
            _userManager = userManager;
            _socialNetworkDBContext = socialNetworkDBContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username);           
            if (user == null)
            {
                throw new ApplicationException("User not found");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var UserUser = _socialNetworkDBContext.UserUser.Select(u => new { fd = u.Ppkfkfollowed, fr = u.Ppkfkfollower });

            bool? followed = null;
            if (user != currentUser)
                followed = UserUser.Where(u => u.fd == currentUser.Id && u.fr == user.Id).FirstOrDefault() == null ? false : true;

            

            return View(new ProfileViewModel {
                Username = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Propic = user.AvatarImage,
                IsFollowed = followed,
                Followers = _userManager.FindAllFollowers(user, _socialNetworkDBContext),
                Followed = _userManager.FindAllFollowed(user, _socialNetworkDBContext),
                Posts = _socialNetworkDBContext.GetAllPosts(user)
            });
        }

        [HttpPost("/Follow")]
        [ActionName("Follow")]
        [Authorize]
        public async Task<IActionResult> Follow(string shouldFollowUser)
        {
            var userFound = await _userManager.FindByNameAsync(shouldFollowUser);
            if (userFound == null) return NotFound();
            try
            {
                await _socialNetworkDBContext.UserUser.AddAsync(new UserUser() { Ppkfkfollowed = _userManager.GetUserId(User), Ppkfkfollower = userFound.Id });
                await _socialNetworkDBContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(Index), new { Username = shouldFollowUser });
        } 
        
        [HttpPost("/Unfollow")]
        [ActionName("Unfollow")]
        [Authorize]
        public async Task<IActionResult> Unfollow(string shouldUnfollowUser)
        {
            var userFound = await _userManager.FindByNameAsync(shouldUnfollowUser);
            if (userFound == null) return NotFound();
            try
            {
                _socialNetworkDBContext.UserUser.Remove(new UserUser() { Ppkfkfollowed = _userManager.GetUserId(User), Ppkfkfollower = userFound.Id });
                _socialNetworkDBContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(Index), new { Username = shouldUnfollowUser });
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}