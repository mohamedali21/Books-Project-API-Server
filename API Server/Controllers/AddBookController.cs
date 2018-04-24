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
    public class AddBookController : ApiController
    {
        BooksEntities db = new BooksEntities();


        [ResponseType(typeof(AddbooVM))]
        public IHttpActionResult GetBook(int id)
        {
            var user = (from b in db.Seller_Approved_book
                        where b.Approved == "Yes" && b.Book_Id == id
                        select new { b.Seller_Id, b.User.UserName }).FirstOrDefault();
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            var citname = (from i in db.Cities
                           where i.Id == book.City_Id
                           select i.Name).First();
            var govid = (from i in db.Cities
                         where i.Id == book.City_Id
                         select i.Governate_Id).First();

            var govname = (from i in db.Governates
                           where i.Id == govid
                           select i.Name).First();

            AddbooVM bookVM = new AddbooVM();

            bookVM.BookName = book.Name;
            bookVM.Points = book.Points;



            bookVM.Discription = book.Discription;
            bookVM.SubCatogry = book.SubCategory.Name;
            bookVM.Governate = govname;
            bookVM.City = citname;

            var images = (from i in db.Images
                          where i.Book_Id == id
                          select i).ToList();

            bookVM.Img = new List<string>();
            for (int i = 0; i < images.Count; i++)
            {
                string Img_URL = images[i].Img_URL;
                bookVM.Img.Add(Img_URL);
            }
            return Ok(bookVM);
        }






        //  post add book
        [HttpPost]
        [ResponseType(typeof(AddbooVM))]
        public IHttpActionResult postbook(AddbooVM addbook)
        {
            bool govexsist = false;
            bool cityexsist = false;
            int govid ;
            int citid;
            string GoverStr= addbook.Governate;
            string cityStr = addbook.Governate;

            if (addbook != null)
            {
                if(addbook.Governate.Contains(" Governorate"))
                {
                    addbook.Governate = addbook.Governate.Replace(" Governorate", "");
                }
                if (addbook.City.Contains(" Governorate"))
                {
                    addbook.City = addbook.City.Replace(" Governorate", "");
                }
                if(addbook.City.Contains("Qesm"))
                {
                    addbook.City = addbook.City.Split(' ').Last();
                    if (addbook.City == "Asyut")
                        addbook.City = "Assuit";
                }
                var govnames = (from i in db.Governates
                                select i.Name).ToList();
                foreach (var item in govnames)
                {
                    if (GoverStr == addbook.Governate)
                    {
                        govexsist = true;
                    }

                }

                if (govexsist)
                {
                    govid = (from i in db.Governates
                             where i.Name == addbook.Governate
                             select i.Id).First();
                }
                else
                {
                    Governate Gov = new Governate();
                    Gov.Name = addbook.Governate;
                    db.Governates.Add(Gov);
                    db.SaveChanges();

                    govid = (from i in db.Governates
                             where i.Name == addbook.Governate
                             select i.Id).First();
                }

                var citynames = (from i in db.Cities
                                 select i.Name).ToList();
                foreach (var item in citynames)
                {
                    if (item == addbook.City)
                    {
                        cityexsist = true;
                    }

                }

                if (cityexsist)
                {
                    citid = (from i in db.Cities
                             where i.Name == addbook.City
                             select i.Id).First();
                }
                else
                {
                    City cit = new City();
                    cit.Name = addbook.City;
                    cit.Governate_Id = govid;
                    db.Cities.Add(cit);
                    db.SaveChanges();

                    citid = (from i in db.Cities
                             where i.Name == addbook.City
                             select i.Id).First();
                }
                var subid = (from i in db.SubCategories
                             where i.Name == addbook.SubCatogry
                             select i.Id).First();

                Book book = new Book();
                book.Name = addbook.BookName;
                book.Points = addbook.Points;
                book.Discription = addbook.Discription;
                book.Date_Uploaded = DateTime.Now;
                book.City_Id = citid;
                book.Subcategory_Id = subid;

                db.Books.Add(book);
                db.SaveChanges();

                var bookid = (from i in db.Books
                              where i.Name == addbook.BookName && i.Points == addbook.Points && i.Discription == addbook.Discription && i.City_Id == citid
                              select i.Id).First();
                Seller_Approved_book sab = new Seller_Approved_book();
                sab.Book_Id = bookid;
                sab.Seller_Id = addbook.UserId;
                sab.Approved = "no";
                db.Seller_Approved_book.Add(sab);
                db.SaveChanges();

                foreach (var item in addbook.Img)
                {
                    if (item != null)
                    {
                        Image im = new Image();
                        im.Book_Id = bookid;
                        im.Img_URL = item;
                        db.Images.Add(im);
                        db.SaveChanges();
                    }

                }



            }

            return Ok(addbook);

        }

        public IHttpActionResult Put(AddbooVM editbook)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            bool govexsist = false;
            bool cityexsist = false;
            int govid;
            int citid;
            var govnames = (from i in db.Governates
                            select i.Name).ToList();
            foreach (var item in govnames)
            {
                if (item == editbook.Governate)
                {
                    govexsist = true;
                }

            }

            if (govexsist)
            {
                govid = (from i in db.Governates
                         where i.Name == editbook.Governate
                         select i.Id).First();
            }
            else
            {
                Governate Gov = new Governate();
                Gov.Name = editbook.Governate;
                db.Governates.Add(Gov);
                db.SaveChanges();

                govid = (from i in db.Governates
                         where i.Name == editbook.Governate
                         select i.Id).First();
            }

            var citynames = (from i in db.Cities
                             select i.Name).ToList();
            foreach (var item in citynames)
            {
                if (item == editbook.City)
                {
                    cityexsist = true;
                }

            }

            if (cityexsist)
            {
                citid = (from i in db.Cities
                         where i.Name == editbook.City
                         select i.Id).First();
            }
            else
            {
                City cit = new City();
                cit.Name = editbook.City;
                cit.Governate_Id = govid;
                db.Cities.Add(cit);
                db.SaveChanges();

                citid = (from i in db.Cities
                         where i.Name == editbook.City
                         select i.Id).First();
            }
            var subid = (from i in db.SubCategories
                         where i.Name == editbook.SubCatogry
                         select i.Id).First();


            var imglist = (from i in db.Images
                           where i.Book_Id == editbook.BookId
                           select i.Id).ToList();
            foreach (var item in imglist)
            {
                var img = db.Images.Where(e => e.Id == item).First();
                db.Images.Remove(img);
                db.SaveChanges();

            }

            using (var ctx = new BooksEntities())
            {
                var existingbook = ctx.Books.Where(s => s.Id == editbook.BookId)
                                                        .FirstOrDefault<Book>();

                if (existingbook != null)
                {
                    existingbook.Name = editbook.BookName;
                    existingbook.Points = editbook.Points;
                    existingbook.Discription = editbook.Discription;
                    
                    existingbook.City_Id = citid;
                    existingbook.Subcategory_Id = subid;
                    ctx.SaveChanges();

                    foreach (var item in editbook.Img)
                    {if (item != null)
                        {
                            Image im = new Image();

                            im.Book_Id = editbook.BookId;
                            im.Img_URL = item;
                            ctx.Images.Add(im);
                            ctx.SaveChanges();
                        }


                    }


                }
                else
                {
                    return NotFound();
                }
            }

            return Ok(editbook);
        }

        public IHttpActionResult Delete(int id)
        { bool selll= false;
            if (id <= 0)
                return BadRequest("Not a valid Book id");
             var selby = (from i in db.Seller_Buyer_Book
                         select i.Book_Id).ToList();
            foreach (var item in selby)
            {
                if (item == id)
                {
                    selll = true;
                }

            }

            if (selll) { return BadRequest("Not a valid Book id"); }
            else
            {
                using (var ctx = new BooksEntities())
                {
                    var bookrequest = ctx.Requests
                        .Where(s => s.Book_Id == id)
                        .FirstOrDefault();
                    if (bookrequest != null)
                    {
                        ctx.Entry(bookrequest).State = System.Data.Entity.EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                    var bookapproved = ctx.Seller_Approved_book
                        .Where(s => s.Book_Id == id)
                        .FirstOrDefault();
                    if (bookapproved != null)
                    {
                        ctx.Entry(bookapproved).State = System.Data.Entity.EntityState.Deleted;
                        ctx.SaveChanges();
                    }

                    var imglist = (from i in db.Images
                                   where i.Book_Id == id
                                   select i.Id).ToList();
                    
                    foreach (var item in imglist)
                    {
                        var img = db.Images.Where(e => e.Id == item).First();
                        if (img != null)
                        {
                            db.Images.Remove(img);
                            db.SaveChanges();
                        }

                    }
                    var book = ctx.Books
                        .Where(s => s.Id == id)
                        .FirstOrDefault();

                    ctx.Entry(book).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }

            return Ok();
        }
    }
}
