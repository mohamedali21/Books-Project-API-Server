using API_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_Server.Controllers
{
    public class UserController : ApiController
    {
        BooksEntities db = new BooksEntities();
        //[Authorize(Roles = "admin")]
        [AllowAnonymous]
        [HttpGet]
        [Route("api/user/list/norm")]
        public IEnumerable<UserVM> Get()
        {
            var normUser =
                (from u in db.Users
                 where u.Role == "user"
                 select u).ToList();
            List<UserVM> userVmList = new List<UserVM>();
            foreach (var user in normUser)
            {
                userVmList.Add(ConvertClass.UserToVM(user));
            }
            return (userVmList);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/user/list/admin")]
        public IEnumerable<UserVM> GetAdmin()
        {
            var normUser =
                (from u in db.Users
                 where u.Role == "admin"
                 select u).ToList();
            List<UserVM> userVmList = new List<UserVM>();
            foreach (var user in normUser)
            {
                userVmList.Add(ConvertClass.UserToVM(user));
            }
            return (userVmList);
        }


        [Authorize]
        [HttpGet]
        [Route("api/user/authintication")]
        public IHttpActionResult GetForAuthinticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var userID = identity.FindFirst("UserID").Value;
            var role = identity.FindFirst("role").Value;
            var email = identity.FindFirst("email").Value;
            var str = new { userID, email, role };
            return Ok(str);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/user/register")]
        [ResponseType(typeof(User))]
        public IHttpActionResult addUser(User u)
        {

            bool email = isAddedBefor(u.Email);
            if (email)
            {
                ModelState.AddModelError("email", "emial added before");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Users.Add(u);
            db.SaveChanges();
            Point point = new Point();
            point.User_Id = u.Id;
            point.Point1 = 25;
            db.Points.Add(point);
            db.SaveChanges();
            return Ok(u);
        }


        private bool isAddedBefor(string email)
        {
            bool addedBefore = false;
            var query =
                (from u in db.Users
                 select u.Email).ToList();

            foreach (var item in query)
            {
                if (item == email)
                    addedBefore = true;
            }
            return addedBefore;
        }


        [HttpGet]
        [Route("api/user/RoleChange/{userID:int}")]
        public IHttpActionResult AdminToUser(int userID)
        {
            var user = db.Users.Where(u => u.Id == userID).FirstOrDefault();
            if (user == null)
            {
                return Ok("user NotFound");
            }

            if (user.Role == "admin")
                user.Role = "user";
            else
                user.Role = "admin";
            db.SaveChanges();

            return Ok(user);
        }
        [HttpGet]
        [Route("api/user/activate/{userID:int}")]
        public IHttpActionResult UserActivate(int userID)
        {
            var user = db.Users.Where(u => u.Id == userID).FirstOrDefault();
            if (user == null)
            {
                return Ok("user NotFound");
            }
            if (user.IsActive == "Yes")
            {
                user.IsActive = "No";
            }
            else
            {
                user.IsActive = "Yes";
            }
            db.SaveChanges();
            return Ok(user);
        }
        [HttpGet]
        [Route("api/user/ActivtiesCount/{userID:int}")]
        public IHttpActionResult ActivitiesCount(int userID)
        {
            var AdsNum = db.Seller_Approved_book.Where(b => b.Seller_Id == userID).Count();
            var reqNum = db.Requests.Where(r => r.Requester_Id == userID).Count();
            var points = db.Points.Where(p => p.User_Id == userID).Select(p => p.Point1).FirstOrDefault();
            var Ob = new {NumberOfAds=AdsNum,NumberOfRequests=reqNum,points=points };
            return Ok(Ob);
        }
    }
}
