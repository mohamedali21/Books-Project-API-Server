using API_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace API_Server.Controllers
{
    public class CategoryController : ApiController
    {
        BooksEntities db = new BooksEntities();

        [AllowAnonymous]
        [HttpGet]
        [Route("api/category/list")]
        public IEnumerable<CategoryVM> GetCategories()
        {
            var categories = 
                (from c in db.Categories
                select c).ToList();

            List<CategoryVM> categoriesList = new List<CategoryVM>();
            categoriesList.Add(new CategoryVM { Id = 0, Name = "All Category" });
            foreach (var item in categories)
            {
                categoriesList.Add(ConvertClass.CategoryToVM(item));
            }
            return categoriesList;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/categories/list")]
        public IEnumerable<CategoryVM> GetCategoryList()
        {
            var categories = (from c in db.Categories
                              select c).ToList();
            List<CategoryVM> categoriesList = new List<CategoryVM>();
            foreach (var item in categories)
            {
                var subCatLists = (from SC in db.SubCategories
                                   where SC.Category_Id == item.Id
                                   select SC).ToList();
                CategoryVM catVm = ConvertClass.CategoryToVM(item, subCatLists);
                categoriesList.Add(catVm);
            }
            return categoriesList;
        }





        ////////////////////////////////////////////
        public CategoryVM GetCategories(string ID)
        {
            var categories = (from c in db.Categories
                              where c.Name == ID
                              select c).ToList();
            CategoryVM categoriesList = new CategoryVM();
            foreach (var item in categories)
            {
                var subCatLists = (from SC in db.SubCategories
                                   where SC.Category_Id == item.Id
                                   select SC).ToList();
                CategoryVM catVm = ConvertClass.CategoryToVM(item, subCatLists);
                categoriesList = catVm;
            }
            return categoriesList;
        }


        ///////////////////////////////////////////////////
        [HttpPost]
        [ResponseType(typeof(Category))]
        public IHttpActionResult Postcat(CategoryVM cat)
        {
            bool catFlag = isAddedBefor(cat.Name);
            if (catFlag)
            {
                ModelState.AddModelError("cat", "Categoryl already exists");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Category newcat = new API_Server.Category();
            newcat = ConvertClass.VMToCategory(cat);
            db.Categories.Add(newcat);
            db.SaveChanges();


            var catId =
                (from c in db.Categories
                 where c.Name == cat.Name
                 select c.Id).FirstOrDefault();

            return Ok(catId);
        }

        public bool isAddedBefor(string name)
        {
            bool isExist = false;
            var cats =
                (from c in db.Categories
                 select c.Name).ToList();

            foreach (var cat in cats)
            {
                if (cat == name)
                    isExist = true;
            }
            return isExist;
        }

        [Route("api/category/addSub")]
        [ResponseType(typeof(SubCategory))]
        public IHttpActionResult PostsubCategory(SubCatVM2 subVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SubCategory sub = new API_Server.SubCategory();
            sub = ConvertClass.VMToSubCategory(subVM);
            db.SubCategories.Add(sub);
            db.SaveChanges();

            return Ok(sub);
        }



        [HttpGet]
        [Route("api/categoryById/{catName}")]
        public IHttpActionResult getCAtId(string catName)
        {
            var catId =
                (from c in db.Categories
                 where c.Name == catName
                 select c.Id).FirstOrDefault();


            return Ok(catId);
        }

        ////////////////////////////////////////////////////
    }
}
