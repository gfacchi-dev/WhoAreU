using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhoAreU.Models;
using System.Security.Claims;

namespace WhoAreU.Extensions
{
    public static class UserManagerExtensions
    {

        public static IEnumerable<ApplicationUser> FindBySurnameAsync(this UserManager<ApplicationUser> um, string surname)
        {
            return um?.Users?.Where(x => x.Surname.Contains(surname));
        }
        
        public static IEnumerable<ApplicationUser> FindAllFollowers(this UserManager<ApplicationUser> um, ApplicationUser user, SocialNetworkDBContext db)
        {
            var PPKFKFollowers = db.UserUser.Where(u => user.Id == u.Ppkfkfollower).Select(u => u.Ppkfkfollowed).ToList();
            var output = um?.Users?.Where(u => PPKFKFollowers.Contains(u.Id)).ToList();
            return output;
        }
        public static IEnumerable<ApplicationUser> FindAllFollowed(this UserManager<ApplicationUser> um, ApplicationUser user, SocialNetworkDBContext db)
        {
            var PPKFKFollowed = db.UserUser.Where(u => user.Id == u.Ppkfkfollowed).Select(u => u.Ppkfkfollower).ToList();
            var output = um?.Users?.Where(u => PPKFKFollowed.Contains(u.Id)).ToList();
            return output;
        }


        public static IEnumerable<Post> GetAllPosts(this SocialNetworkDBContext db, IdentityUser user)
        {
            return db.Post.Where(p => p.Fkuser == user.Id);
        }
        public static async void WritePost(this SocialNetworkDBContext db, Post p)
        {
            try
            {
                await db.Post.AddAsync(p);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async void DeletePost(this SocialNetworkDBContext db, Post p, IdentityUser user)
        {
            try
            {
                db.Post.Remove(p);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async void UpdatePost(this SocialNetworkDBContext db, Post p, IdentityUser user)
        {
            try
            {
                var post = db.Post.Find(p);
                post.Body = p.Body;
                post.PublishDate = DateTime.Now;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
