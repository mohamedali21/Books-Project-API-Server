using API_Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace API_Server.Controllers
{
    public class LocationController : ApiController
    {
        BooksEntities db = new BooksEntities();
        [AllowAnonymous]
        [HttpGet]
        [Route("api/governate/list")]
        public IEnumerable<GovernateVM> Get()
        {
            var gov =
                (from g in db.Governates
                 select g).ToList();

            List<GovernateVM> govList = new List<Models.GovernateVM>();
            List<string> city = new List<string>();
            city.Add("no City");
            govList.Add(new GovernateVM { Id = 0, Name = "all  Governates", Cities = city });
            foreach (var item in gov)
            {
                govList.Add(ConvertClass.GovernateToVM(item));
            }
            return (govList);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/governate/byId/{id:int}")]
        public GovernateVM GetOne(int id)
        {
            var gov =
                (from g in db.Governates
                 where g.Id == id
                 select g).FirstOrDefault();

            GovernateVM govVm = new GovernateVM();
            if (gov != null)
            {
                govVm = ConvertClass.GovernateToVM(gov);
            }
            return (govVm);
        }
    }
}
