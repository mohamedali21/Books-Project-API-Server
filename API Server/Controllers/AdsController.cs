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
    public class AdsController : ApiController
    {
        BooksEntities db = new BooksEntities();

        public IEnumerable<AdVM> GetBooks()
        {
            var booksIDs = (from b in db.Seller_Approved_book
                            where b.Approved == "Yes"
                            select b).OrderByDescending(x => x.Book.Date_Uploaded); 
            List<AdVM> adsList = new List<AdVM>();
            foreach (var item in booksIDs)
            {
                //AdVM adVM = new AdVM();
                //adVM.BookID = item.Book_Id;
                //adVM.UserID = item.Seller_Id;
                //var book = (from b in db.Books
                //            where b.Id == item.Book_Id
                //            select b).FirstOrDefault();
                //var images = (from i in db.Images
                //              where i.Book_Id == item.Book_Id
                //              select i).FirstOrDefault();
                //adVM.Img_Url = images.Img_URL;
                //adVM.Name = book.Name;
                //adVM.Points = book.Points;
                //adVM.Date_Uploaded = book.Date_Uploaded;
                //adsList.Add(adVM);
                var OpDone = db.Seller_Buyer_Book.Where(op => op.BuyerConfirm == "Yes" && op.SellerConfirm == "Yes"&&op.Book_Id==item.Book_Id).FirstOrDefault();
                if(OpDone==null)
                adsList.Add(ConvertClass.ApprovedToAdsVm(item));
            }
            return adsList;
        }
        [HttpGet]
        [Route("api/GetUserAds/{userID:int}")]
        public IEnumerable<AdVM> GetUserAds(int userID)
        {
            var booksIDs = (from b in db.Seller_Approved_book
                            where b.Seller_Id==userID
                            select b).OrderByDescending(x => x.Book.Date_Uploaded);
            if(booksIDs==null)
            {
                return null;
            }
            List<AdVM> adsList = new List<AdVM>();
            foreach (var item in booksIDs)
            {
                //AdVM adVM = new AdVM();
                //adVM.BookID = item.Book_Id;
                //adVM.UserID = item.Seller_Id;
                //var book = (from b in db.Books
                //            where b.Id == item.Book_Id
                //            select b).FirstOrDefault();
                //var images = (from i in db.Images
                //              where i.Book_Id == item.Book_Id
                //              select i).FirstOrDefault();
                //adVM.Img_Url = images.Img_URL;
                //adVM.Name = book.Name;
                //adVM.Description = book.Discription;
                //adVM.Points = book.Points;
                //adVM.Date_Uploaded = book.Date_Uploaded;
                //adsList.Add(adVM);
                adsList.Add(ConvertClass.ApprovedToAdsVm(item));
            }
            return adsList;
        }

        /////////////////////////////////////////////
        /////   3-4-2018    mohamed alaa



        //  getbook by bookId
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/search/{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var book =
                (from b in db.Seller_Approved_book
                 where b.Book_Id == id
                 select b).FirstOrDefault();
            if (book == null)
            {
                return NotFound();
            }
            AdVM bookVm = new AdVM();
            bookVm = ConvertClass.ApprovedToAdsVm(book);
            return Ok(bookVm);
        }

        //  getbook by bookName
        public IEnumerable<AdVM> Get(string name)
        {
            var appBook =
                (from b in db.Seller_Approved_book
                 where b.Book.Name.Contains(name) && b.Approved == "Yes"
                 select b).OrderByDescending(x => x.Book.Date_Uploaded).ToList();
            if (appBook.Count == 0)
            {
                return null;
            }

            List<AdVM> adsList = new List<AdVM>();
            foreach (var item in appBook)
            {
                var OpDone = db.Seller_Buyer_Book.Where(op => op.BuyerConfirm == "Yes" && op.SellerConfirm == "Yes" && op.Book_Id == item.Book_Id).FirstOrDefault();
                if (OpDone == null)
                    adsList.Add(ConvertClass.ApprovedToAdsVm(item));
            }
            return (adsList);
        }


        //  get book by category id
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/byCategory/{id:int}")]
        public IEnumerable<AdVM> GetByCatId(int id)
        {
            var subCatIds =
                 (from c in db.SubCategories
                  where c.Category_Id == id
                  select c.Id).ToList();

            List<Book> booklist2 = new List<Book>();
            List<Seller_Approved_book> list3 = new List<Seller_Approved_book>();
            var approved =
                (from ab in db.Seller_Approved_book
                 where ab.Approved == "Yes"
                 select ab).OrderByDescending(x => x.Book.Date_Uploaded).ToList();

            foreach (var SubId in subCatIds)
            {
                foreach (var appBook in approved)
                {
                    var OpDone = db.Seller_Buyer_Book.Where(op => op.BuyerConfirm == "Yes" && op.SellerConfirm == "Yes" && op.Book_Id == appBook.Book_Id).FirstOrDefault();
                    if (OpDone == null)
                    {
                        if (appBook.Book.Subcategory_Id == SubId)
                            list3.Add(appBook);
                    }
                        
                }
            }
            List < AdVM > bookvmList3 = new List<AdVM>();
            foreach (var item in list3)
            {
                bookvmList3.Add(ConvertClass.ApprovedToAdsVm(item));
            }
            return bookvmList3;
        }

        //  get book by governate id
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/byGovernate/{id:int}")]
        public IEnumerable<AdVM> GetByGovId(int id)
        {
            var cities =
                 (from c in db.Cities
                  where c.Governate_Id == id
                  select c.Id).ToList();

            List<Book> booklist = new List<Book>();
            List<Book> booklist2 = new List<Book>();
            foreach (var item in cities)
            {
                booklist =
                    (from b in db.Books
                     where b.City_Id == item
                     select b).ToList();

                foreach (var item2 in booklist)
                {
                    booklist2.Add(item2);
                }
            }

            var approved =
                (from b in db.Seller_Approved_book
                 where b.Approved == "Yes"
                 select b).OrderByDescending(x => x.Book.Date_Uploaded).ToList();

            List<Seller_Approved_book> appBookList = new List<API_Server.Seller_Approved_book>();
            foreach (var appBook in approved)
            {
                foreach (var book in booklist2)
                {
                  
                        if (book.Id == appBook.Book_Id)
                            appBookList.Add(appBook);
                    
                    
                }
            }

            List<AdVM> adVm = new List<AdVM>();
            foreach (var appBook in appBookList)
            {
                var OpDone = db.Seller_Buyer_Book.Where(op => op.BuyerConfirm == "Yes" && op.SellerConfirm == "Yes" && op.Book_Id == appBook.Book_Id).FirstOrDefault();
                if (OpDone == null)
                {
                    adVm.Add(ConvertClass.ApprovedToAdsVm(appBook));
                }
               
            }
            return adVm;
        }

        //  get book by bookname and govId and categoryId
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/byCollection/{bookname}/{govId}/{catId}")]
        public IEnumerable<AdVM> GetByCollection(string bookname, int govId, int catId)
        {
            var book =
                (from appb in db.Seller_Approved_book
                 where appb.Approved == "Yes"
                 && appb.Book.Name.Contains(bookname)
                 && appb.Book.SubCategory.Category_Id == catId
                 && appb.Book.City.Governate_Id == govId
                 select appb).OrderByDescending(x => x.Book.Date_Uploaded).ToList();

            List<AdVM> adVmList = new List<AdVM>();
            foreach (var appBook in book)
            {
                var OpDone = db.Seller_Buyer_Book.Where(op => op.BuyerConfirm == "Yes" && op.SellerConfirm == "Yes" && op.Book_Id == appBook.Book_Id).FirstOrDefault();
                if (OpDone == null)
                {
                    adVmList.Add(ConvertClass.ApprovedToAdsVm(appBook));
                }
                
            }
            return adVmList;
            
        }


        //  return book within specific city
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/byCity/{cityName}")]
        public IEnumerable<AdVM> GetByCity(string cityName)
        {
            var books =
                (from b in db.Books
                 where b.City.Name == cityName
                 select b).ToList();

            var appBook =
                (from appB in db.Seller_Approved_book
                 where appB.Approved == "Yes"
                 select appB).OrderByDescending(x => x.Book.Date_Uploaded).ToList();
            List<Seller_Approved_book> appBookList = new List<API_Server.Seller_Approved_book>();
            foreach (var approved in appBook)
            {
                foreach (var book in books)
                {
                    if (book.Id == approved.Book_Id)
                        appBookList.Add(approved);
                }
            }

            List < AdVM > bookvmList3 = new List<AdVM>();
            foreach (var item in appBookList)
            {
                bookvmList3.Add(ConvertClass.ApprovedToAdsVm(item));
            }
            return bookvmList3;
        }



        // get book by name and specific category
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/byNameAndCategory/{bookname}/{catId}")]
        public IEnumerable<AdVM> GetByNameandCategory(string bookname, int catId)
        {
            var book =
                (from appb in db.Seller_Approved_book
                 where appb.Approved == "Yes" 
                 && appb.Book.Name.Contains(bookname) 
                 && appb.Book.SubCategory.Category_Id == catId
                 select appb).OrderByDescending(x => x.Book.Date_Uploaded).ToList();

            List<AdVM> adVmList = new List<AdVM>();
            foreach (var appBook in book)
            {
                var OpDone = db.Seller_Buyer_Book.Where(op => op.BuyerConfirm == "Yes" && op.SellerConfirm == "Yes" && op.Book_Id == appBook.Book_Id).FirstOrDefault();
                if (OpDone == null)
                {
                    adVmList.Add(ConvertClass.ApprovedToAdsVm(appBook));
                }
                
            }
            return adVmList;
        }


        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/byNameCatAndCity/{bookname}/{cityName}/{catId}")]
        public IEnumerable<AdVM> GetByCollection(string bookname, string cityName, int catId)
        {
            var book =
                (from appb in db.Seller_Approved_book
                 where appb.Approved == "Yes"
                 && appb.Book.Name.Contains(bookname)
                 && appb.Book.SubCategory.Category_Id == catId
                 && appb.Book.City.Name == cityName
                 select appb).OrderByDescending(x => x.Book.Date_Uploaded).ToList();

            List<AdVM> adVmList = new List<AdVM>();
            foreach (var appBook in book)
            {
                var OpDone = db.Seller_Buyer_Book.Where(op => op.BuyerConfirm == "Yes" && op.SellerConfirm == "Yes" && op.Book_Id == appBook.Book_Id).FirstOrDefault();
                if (OpDone == null)
                {
                    adVmList.Add(ConvertClass.ApprovedToAdsVm(appBook));
                }
            }
            return adVmList;
            
        }



        // get book by name and specific category and specific city
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/byNameCity/{bookname}/{cityName}")]
        public IEnumerable<AdVM> GetByNameAndCity(string bookname, string cityName)
        {
            var books =
                (from appb in db.Seller_Approved_book
                 where appb.Approved == "Yes"
                 && appb.Book.Name.Contains(bookname)
                 && appb.Book.City.Name == cityName
                 select appb).OrderByDescending(x => x.Book.Date_Uploaded).ToList();
            List<AdVM> adVm = new List<AdVM>();
            foreach (var book in books)
            {
                adVm.Add(ConvertClass.ApprovedToAdsVm(book));
            }
            return adVm;
        }




        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/byNameandGovernate/{bookname}/{govId}")]
        public IEnumerable<AdVM> GetByNameAndCity(string bookname, int govId)
        {
            var books =
                (from appb in db.Seller_Approved_book
                 where appb.Approved == "Yes"
                 && appb.Book.Name.Contains(bookname)
                 && appb.Book.City.Governate_Id == govId
                 select appb).OrderByDescending(x => x.Book.Date_Uploaded).ToList();

            List<AdVM> adList = new List<Models.AdVM>();
            foreach (var book in books)
            {
                var OpDone = db.Seller_Buyer_Book.Where(op => op.BuyerConfirm == "Yes" && op.SellerConfirm == "Yes" && op.Book_Id == book.Book_Id).FirstOrDefault();
                if (OpDone == null)
                {
                    adList.Add(ConvertClass.ApprovedToAdsVm(book));
                }
                
            }
            return adList;
        }

        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/pending")]
        public IEnumerable<AdVM> GetPendingBooks()
        {
            var booksIDs = (from b in db.Seller_Approved_book
                            where b.Approved == "No"
                            select b).OrderByDescending(x => x.Book.Date_Uploaded);
            List<AdVM> adsList = new List<AdVM>();
            foreach (var item in booksIDs)
            {

                adsList.Add(ConvertClass.ApprovedToAdsVm(item));
            }
            return adsList;
        }


        //////////////////////////////////////////
        //////////////////////////////////////////////
        //      5-4-2018
        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/GovCat/{govId}/{catId}")]
        public IEnumerable<AdVM> GetByGovIdCatId(int govId, int catId)
        {
            var book =
                (from appb in db.Seller_Approved_book
                 where appb.Approved == "Yes"
                 && appb.Book.SubCategory.Category_Id == catId
                 && appb.Book.City.Governate_Id == govId
                 select appb).OrderByDescending(x => x.Book.Date_Uploaded).ToList();

            List<AdVM> adVmList = new List<AdVM>();
            foreach (var appBook in book)
            {
                adVmList.Add(ConvertClass.ApprovedToAdsVm(appBook));
            }
            return adVmList;

        }

        [AllowAnonymous]
        [HttpGet]
        [ResponseType(typeof(Book))]
        [Route("api/book/byName/{bookname}")]
        public IEnumerable<AdVM> GetByNameAndCity(string bookname)
        {
            var books =
                (from appb in db.Seller_Approved_book
                 where appb.Approved == "Yes"
                 && appb.Book.Name.Contains(bookname)
                 select appb).OrderByDescending(x => x.Book.Date_Uploaded).ToList();
            List<AdVM> adVm = new List<AdVM>();
            foreach (var book in books)
            {
                adVm.Add(ConvertClass.ApprovedToAdsVm(book));
            }
            return adVm;
        }
        ///////////////////////////////////////////
    }
}
